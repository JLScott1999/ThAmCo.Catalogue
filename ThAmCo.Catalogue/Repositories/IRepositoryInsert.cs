namespace ThAmCo.Catalogue.Repositories
{
    public interface IRepositoryInsert<TModel> : IRepository
    {

        public void Insert(TModel model);

    }
}
