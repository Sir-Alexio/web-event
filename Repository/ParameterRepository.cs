using WebEvent.API.Context;
using WebEvent.API.Model.Entity;
using WebEvent.API.Repository.Abstract;
using WebEvent.API.Repository.Base;

namespace WebEvent.API.Repository
{
    public class ParameterRepository : RepositoryBase<Parameter>,IParameterRepository
    {
        public ParameterRepository(ApplicationContext db) : base(db)
        {
        }
    }
}
