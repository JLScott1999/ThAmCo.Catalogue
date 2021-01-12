namespace ThAmCo.Catalogue
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Sockets;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Polly;
    using Polly.Extensions.Http;
    using ThAmCo.Catalogue.Data;
    using ThAmCo.Catalogue.Repositories.Product;
    using ThAmCo.Catalogue.Services.Order;
    using ThAmCo.Catalogue.Services.Product;
    using ThAmCo.Catalogue.Services.Review;
    using ThAmCo.Catalogue.Services.StockManagement;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Setup database connection
            string databaseConnectionURL = this.Configuration.GetConnectionString("DatabaseConnection");
            if (string.IsNullOrWhiteSpace(databaseConnectionURL))
            {
                Debug.WriteLine("Using Fake ProductRepository");
                services.AddTransient<IProductRepository, FakeProductRepository>();
            }
            else
            {
                services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(databaseConnectionURL)
                );
                services.AddTransient<IProductRepository, ProductRepository>();
            }

            // Setup Service Layer
            services.AddTransient<IProductService, ProductService>();

            string stockManagementServiceURL = this.Configuration["Services:StockManagement:URL"];
            if (string.IsNullOrWhiteSpace(stockManagementServiceURL))
            {
                Debug.WriteLine("Using Fake StockManagementService");
                services.AddTransient<IStockManagementService, FakeStockManagementService>();
            }
            else
            {
                services.AddHttpClient<IStockManagementService, StockManagementService>(client =>
                {
                    client.BaseAddress = new Uri(stockManagementServiceURL);
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());
            }

            string orderServiceURL = this.Configuration["Services:OrderService:URL"];
            if (string.IsNullOrWhiteSpace(orderServiceURL))
            {
                Debug.WriteLine("Using Fake OrderService");
                services.AddTransient<IOrderService, FakeOrderService>();
            }
            else
            {
                services.AddHttpClient<IOrderService, OrderService>(client =>
                {
                    client.BaseAddress = new Uri(orderServiceURL);
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());
            }

            string reviewServiceURL = this.Configuration["Services:ReviewService:URL"];
            if (string.IsNullOrWhiteSpace(reviewServiceURL))
            {
                Debug.WriteLine("Using Fake ReviewService");
                services.AddTransient<IReviewService, FakeReviewService>();
            }
            else
            {
                services.AddHttpClient<IReviewService, ReviewService>(client =>
                {
                    client.BaseAddress = new Uri(reviewServiceURL);
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());
            }

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Catalogue}/{action=Products}/{id?}");
            });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            Random jitterer = new Random();
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<SocketException>()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3,
                    retryAttempt =>
                    {
                        var waitTime = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 100));
                        Console.WriteLine($"Retry attempt {retryAttempt} waiting {waitTime.TotalSeconds} seconds");
                        return waitTime;
                    }
                );
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<SocketException>()
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30),
                onBreak: (exception, timeSpan) =>
                {
                    Console.WriteLine("Breaking circuit for " + timeSpan.TotalSeconds + "seconds due to " + exception.Exception.Message);
                },
                onReset: () =>
                {
                    Console.WriteLine("Trial call succeeded: circuit closing again.");
                },
                onHalfOpen: () =>
                {
                    Console.WriteLine("Circuit break time elapsed. Circuit now half open: permitting a trial call.");
                });
        }

    }
}
