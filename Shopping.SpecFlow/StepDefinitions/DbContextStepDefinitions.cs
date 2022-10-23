using Bogus;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Models.Domain;
using Shopping.SpecFlow.Extensions;
using static Shopping.SpecFlow.StepDefinitions.ScenarioContextKeys;
using Product = Shopping.Models.Domain.Product;

namespace Shopping.SpecFlow.StepDefinitions
{
    [Binding]
    public class DbContextStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        public ShoppingDbContext _context { get; }

        public DbContextStepDefinitions(ScenarioContext scenarioContext, ShoppingDbContext context)
        {
            _scenarioContext = scenarioContext;
            _context = context;
        }

        [Given(@"The DB has the product kind")]
        public void GivenTheDBHasAProductKind()
        {
            var productKind = new ProductKind { Id = Guid.NewGuid(), Name = new Faker().Name.Random.Word() };
            _scenarioContext[TheProductKind] = productKind;
            _context.ProductKinds.Add(productKind);
            _context.SaveChanges();
        }

        [Given(@"The DB has another product kind")]
        public void GivenTheDBHasAnotherProductKind()
        {
            var productKind = new ProductKind { Id = Guid.NewGuid(), Name = new Faker().Name.Random.Word() };
            _scenarioContext[AnotherProductKind] = productKind;
            _context.ProductKinds.Add(productKind);
            _context.SaveChanges();
        }

        [Given(@"The DB has a product for the product kind")]
        public void GivenTheDBHasAProductForTheProductKind()
        {
            var productKind = _scenarioContext.GetValueOrDefault<ProductKind>(TheProductKind);
            var product = new Product { Id = Guid.NewGuid(), ProductKindId = productKind.Id, Name = new Faker().Name.Random.Word() };
            _scenarioContext[TheProduct] = product;
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        [Given(@"The DB has another product for another product kind")]
        public void GivenTheDBHasAnotherProductForAnotherProductKind()
        {
            var anotherProductKind = _scenarioContext.GetValueOrDefault<ProductKind>(AnotherProductKind);
            var anotherProduct = new Product { Id = Guid.NewGuid(), ProductKindId = anotherProductKind.Id, Name = new Faker().Name.Random.Word() };
            _scenarioContext[AnotherProduct] = anotherProduct;
            _context.Products.Add(anotherProduct);
            _context.SaveChanges();
        }

        [Then(@"The DB should contain the product")]
        public void ThenTheDBShouldContainTheProduct()
        {
            var product = _scenarioContext.GetValueOrDefault<Product>(TheProduct);
            _context.Products.AsNoTracking()
                .Should().NotContainEquivalentOf(product);
        }

        [Then(@"The DB should not contain the product")]
        public void ThenTheDBShouldNotContainTheProduct()
        {
            var product = _scenarioContext.GetValueOrDefault<Product>(TheProduct);
            _context.Products.AsNoTracking()
                .Should().NotContainEquivalentOf(product);
        }

        [Then(@"The DB should contain the product for the another product kind with the new name")]
        public void ThenTheDBShouldContainTheProductForTheAnotherProductKindWithTheNewName()
        {
            var productId = _scenarioContext.GetValueOrDefault<string>(ProductId);
            var anotherProductKind = _scenarioContext.GetValueOrDefault<ProductKind>(AnotherProductKind);
            var newName = _scenarioContext.GetValueOrDefault<string>(TheNewName);

            var product = new Product { Id = Guid.Parse(productId), Name = newName, ProductKindId = anotherProductKind.Id };
            _context.Products.AsNoTracking()
                .Should().ContainEquivalentOf(product);
        }

        [Then(@"The DB should contain the product kind")]
        public void ThenTheDBShouldContainTheProductKind()
        {
            var productKind = _scenarioContext.GetValueOrDefault<ProductKind>(TheProductKind);
            _context.ProductKinds.AsNoTracking()
                .Should().ContainEquivalentOf(productKind);
        }

        [Then(@"The DB should not contain the product kind")]
        public void ThenTheDBShouldNotHaveTheProduct()
        {
            var productKind = _scenarioContext.GetValueOrDefault<ProductKind>(TheProductKind);
            _context.ProductKinds.AsNoTracking()
                .Should().NotContainEquivalentOf(productKind);
        }

        [Then(@"The DB should not contain another product kind")]
        public void ThenTheDBShouldNotContainAnotherProductKind()
        {
            var anotherProductKind = _scenarioContext.GetValueOrDefault<ProductKind>(AnotherProductKind);
            _context.ProductKinds.AsNoTracking()
                .Should().NotContainEquivalentOf(anotherProductKind);
        }

        [Then(@"The DB should contain the product kind with the new name")]
        public void ThenTheDBShouldContainTheProductKindWithTheNewName()
        {
            var productKindId = _scenarioContext.GetValueOrDefault<string>(ProductKindId);
            var newName = _scenarioContext.GetValueOrDefault<string>(TheNewName);

            var product = new ProductKind { Id = Guid.Parse(productKindId), Name = newName };
            _context.ProductKinds.AsNoTracking()
                .Should().ContainEquivalentOf(product);
        }

        [Then(@"The DB should contain both products for the product kind")]
        public void ThenTheDBShouldContainBothProductsForTheProductKind()
        {
            var productKind = _scenarioContext.GetValueOrDefault<ProductKind>(TheProductKind);

            var product = _scenarioContext.GetValueOrDefault<Product>(TheProduct);
            var anotherProduct = _scenarioContext.GetValueOrDefault<Product>(AnotherProduct);

            _context.Products.AsNoTracking()
                .Should().ContainEquivalentOf(new Product { Id = product.Id, Name = product.Name, ProductKindId = productKind.Id });
            _context.Products.AsNoTracking()
                .Should().ContainEquivalentOf(new Product { Id = anotherProduct.Id, Name = anotherProduct.Name, ProductKindId = productKind.Id });
        }
    }
}