using Bogus;
using Shopping.Models.Domain;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;
using Shopping.SpecFlow.Extensions;
using static Shopping.SpecFlow.StepDefinitions.ScenarioContextKeys;

namespace Shopping.SpecFlow.StepDefinitions
{
    [Binding]
    public class ProductStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        private ProductKind GetTheProductKind() => _scenarioContext.GetValueOrDefault<ProductKind>(TheProductKind);

        private Product GetTheProduct() => _scenarioContext.GetValueOrDefault<Product>(TheProduct);

        public ProductStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I want to add a product for the product kind")]
        public void GivenIWantToAddNewProductForTheProductKind()
        {
            var productKind = GetTheProductKind();
            var product = new Product { Id = Guid.NewGuid(), Name = new Faker().Name.Random.Word(), ProductKindId = productKind.Id };
            _scenarioContext[TheProduct] = product;

            var request = (new AddProduct
            {
                Id = product.Id,
                Name = product.Name,
                ProductKindId = product.Id,
            });
            _scenarioContext[RequestContentModel] = request;
        }

        [Given(@"I want to rename the product and change product kind to another product kind")]
        public void GivenIWantToRenameTheProductAndChangeProductKindToAnotherProductKind()
        {
            var anotherProductKind = _scenarioContext.GetValueOrDefault<ProductKind>(AnotherProductKind);
            var product = GetTheProduct();
            var theNewName = new Faker().Name.Random.Word();

            _scenarioContext[TheNewName] = theNewName;

            var request = new UpdateProduct { Id = product.Id, Name = theNewName, ProductKindId = anotherProductKind.Id, };
            _scenarioContext[ProductId] = product.Id.ToString();
            _scenarioContext[RequestContentModel] = request;
        }

        [Given(@"I want to delete the product")]
        public void GivenIWantToDeleteProduct()
        {
            var product = GetTheProduct();
            _scenarioContext[ProductId] = product.Id.ToString();
        }

        [When(@"The result contains the product with the product kind")]
        public void WhenTheResultContainsTheProductWithTheProductKind()
        {
            var productKind = GetTheProductKind();
            var product = GetTheProduct();
            var expected = new { Name = product.Name, ProductKindName = productKind.Name };

            var products = _scenarioContext.GetDeserializedCollectionOrEmpty<ProductModel>(ResponseContent);
            products.Should().ContainEquivalentOf(expected, config => config.ExcludingMissingMembers());
        }

        [Then(@"The response should contains both products")]
        public void ThenTheResponseShouldContainsTheProduct()
        {
            var product = GetTheProduct();
            var productKind = GetTheProductKind();
            var expected = new { Name = product.Name, ProductKindName = productKind.Name, Used = true };

            var anotherProductKind = _scenarioContext.GetValueOrDefault<ProductKind>(AnotherProductKind);
            var anotherProduct = _scenarioContext.GetValueOrDefault<Product>(AnotherProduct);
            var expectedAnother = new { Name = anotherProduct.Name, ProductKindName = anotherProductKind.Name, Used = false };

            var products = _scenarioContext.GetDeserializedCollectionOrEmpty<ProductModel>(ResponseContent);
            products.Should().ContainEquivalentOf(expected, config => config.ExcludingMissingMembers());
            products.Should().ContainEquivalentOf(expectedAnother, config => config.ExcludingMissingMembers());
        }
    }
}