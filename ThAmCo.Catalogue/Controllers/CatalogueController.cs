namespace ThAmCo.Catalogue.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using ThAmCo.Catalogue.Services.Product;
    using ThAmCo.Catalogue.Services.StockManagement;
    using ThAmCo.Catalogue.ViewModels;

    public class CatalogueController : Controller
    {

        private readonly IProductService productService;
        private readonly IStockManagementService stockService;

        public CatalogueController(
            IProductService productService,
            IStockManagementService stockManagementService
        )
        {
            this.productService = productService;
            this.stockService = stockManagementService;
        }

        public IActionResult Products()
        {
            try
            {
                return this.View(this.productService.GetProducts()
                    .Join(this.stockService.GetProductsStock(),
                        pm => pm.Id,
                        psm => psm.Id,
                        (pm, psm) => new ProductViewModel()
                        {
                            Id = pm.Id,
                            Name = pm.Name,
                            Description = pm.Description,
                            Stock = psm.Stock
                        }
                    )
                );
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
