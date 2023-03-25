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
            var product = new Product { Id = Guid.NewGuid(), Type = new Faker().Name.Random.Word(), Name = new Faker().Name.Random.Word(), ProductKindId = productKind.Id };
            _scenarioContext[TheProduct] = product;

            var request = (new AddProduct
            {
                Id = product.Id,
                Type = product.Type,
                Name = product.Name,
                ProductKindId = productKind.Id,
            });
            _scenarioContext[RequestContentModel] = request;
        }

        [Given(@"I want to update product")]
        public void GivenIWantToUpdateProduct()
        {
            var product = GetTheProduct();

            var request = new UpdateProduct { Id = product.Id, Name = product.Name, ProductKindId = product.ProductKindId, };
            _scenarioContext[ProductId] = product.Id;
            _scenarioContext[RequestContentModel] = request;
        }

        [Given(@"I want to update product's type")]
        public void GivenIWantToUpdateProductsType()
        {
            var request = _scenarioContext.GetValueOrDefault<UpdateProduct>(RequestContentModel);
            var theNewType = new Faker().Name.Random.Word();

            _scenarioContext[TheNewType] = theNewType;
            request.Type = theNewType;
        }

        [Given(@"I want to rename the product")]
        public void GivenIWantToRenameTheProduct()
        {
            var request = _scenarioContext.GetValueOrDefault<UpdateProduct>(RequestContentModel);
            var theNewName = new Faker().Name.Random.Word();

            _scenarioContext[TheNewName] = theNewName;
            request.Name = theNewName;
        }

        [Given(@"I want to change product kind to another product kind")]
        public void GivenIWantToChangeProductKindToAnotherProductKind()
        {
            var anotherProductKind = _scenarioContext.GetValueOrDefault<ProductKind>(AnotherProductKind);
            var request = _scenarioContext.GetValueOrDefault<UpdateProduct>(RequestContentModel);
            request.ProductKindId = anotherProductKind.Id;
        }

        [Given(@"I want to delete the product")]
        public void GivenIWantToDeleteProduct()
        {
            var product = GetTheProduct();
            _scenarioContext[ProductId] = product.Id;
        }

        [Given(@"I want to merge the product with another product")]
        public void GivenIWantToMergeTheProductWithAnotherProduct()
        {
            var theProduct = _scenarioContext.GetValueOrDefault<Product>(TheProduct);
            var anotherProduct = _scenarioContext.GetValueOrDefault<Product>(AnotherProduct);
            var request = new MergeProduct
            {
                SavedProductId = theProduct.Id,
                RemovedProductId = anotherProduct.Id,
            };
            _scenarioContext[RequestContentModel] = request;
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
        public void ThenTheResponseShouldContainsTheProducts()
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


        [Then(@"The response should contains products only for the product kind")]
        public void ThenTheResponseShouldContainsProductsOnlyForTheProductKind()
        {
            var productKind = _scenarioContext.GetValueOrDefault<ProductKind>(TheProductKind);

            var products = _scenarioContext.GetDeserializedCollectionOrEmpty<ProductModel>(ResponseContent);
            products.Should().OnlyContain(item => item.ProductKindId == productKind.Id);
        }

        [Then(@"The response should not contain products with hidden flag")]
        public void ThenTheResponseShouldNotContainProductsWithHiddenFlag()
        {
            var products = _scenarioContext.GetDeserializedCollectionOrEmpty<ProductModel>(ResponseContent);
            products.Should().NotContain(item => item.Hidden);
        }

        [Then(@"The response should contains the product with hidden flag")]
        public void ThenTheResponseShouldContainsTheProductWithHiddenFlag()
        {
            var products = _scenarioContext.GetDeserializedCollectionOrEmpty<ProductModel>(ResponseContent);
            products.Should().Contain(item => item.Hidden);
        }

    }
}