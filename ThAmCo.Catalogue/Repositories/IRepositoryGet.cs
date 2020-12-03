namespace ThAmCo.Catalogue.Repositories
{
    using System;
    using System.Collections.Generic;

    public interface IRepositoryGet<TModel> : IRepository
    {

        public IEnumerable<TModel> Get();

        public TModel Get(Guid id);

    }
}
