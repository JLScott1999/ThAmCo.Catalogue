namespace ThAmCo.Catalogue.Models
{
    using System;

    public class ProductStockModel
    {

        public Guid Id { get; set; }

        public int Stock { get; set; }

        internal object Where(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }
    }
}
