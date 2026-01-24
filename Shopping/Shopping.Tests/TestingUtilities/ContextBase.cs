using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Shopping.Tests.TestingUtilities
{
    public abstract class ContextBase : IDisposable
    {
        protected ShoppingDbContext Context { get; init; }

        protected ContextBase()
        {
            var dbId = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<ShoppingDbContext>()
                .UseInMemoryDatabase(dbId)
                .Options;
            Context = new ShoppingDbContext(options);
            Debug.WriteLine("In-memory database created for testing.");
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }

        public void UpdateEntity<TSource>(Expression<Func<TSource, bool>> predicate, Action<TSource> update) where TSource : class
        {
            var entity = Context.Set<TSource>().FirstOrDefault(predicate);
            if (entity == null)
            {
                throw new InvalidOperationException("Entity not found for update.");
            }

            update(entity);
            Context.SaveChanges();
        }
    }
}