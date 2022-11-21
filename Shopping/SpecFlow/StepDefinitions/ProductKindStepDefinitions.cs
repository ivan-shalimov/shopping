using Bogus;
using Shopping.Models.Domain;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;
using Shopping.SpecFlow.Extensions;
using static Shopping.SpecFlow.StepDefinitions.ScenarioContextKeys;

namespace Shopping.SpecFlow.StepDefinitions
{
    [Binding]
    public class ProductKindStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        private ProductKind GetTheProductKind() => _scenarioContext.GetValueOrDefault<ProductKind>(TheProductKind);

        public ProductKindStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I want to add a new product kind")]
        public void GivenTheNameForNewProductKindIs()
        {
            var request = new AddProductKind { Id = Guid.NewGuid(), Name = new Faker().Name.Random.Word() };

            _scenarioContext[TheProductKind] = new ProductKind { Id = request.Id, Name = request.Name };
            _scenarioContext[RequestContentModel] = request;
        }

        [Given(@"I want to add a new product kind with the same name as another product kind")]
        public void GivenIWantToAddANewProductKindWithTheSameNameAsAnotherProductKind()
        {
            var anotherProductKind = _scenarioContext.GetValueOrDefault<ProductKind>(AnotherProductKind);
            var request = new AddProductKind { Name = anotherProductKind.Name };
            _scenarioContext[RequestContentModel] = request;
        }

        [Given(@"I want to change the name for the product kind")]
        public void GivenIChangeTheNameForProductKindFromTo()
        {
            var productKind = GetTheProductKind();
            var theNewName = new Faker().Name.Random.Word();

            _scenarioContext[TheNewName] = theNewName;

            var request = new UpdateProductKind { Name = theNewName };
            _scenarioContext[ProductKindId] = productKind.Id.ToString();
            _scenarioContext[RequestContentModel] = request;
        }

        [Given(@"I want to change the name for product to use the same name as another product kind")]
        public void GivenIWantToChangeTheNameForProductToUseTheSameNameAsAnotherProductKind()
        {
            var productKind = GetTheProductKind();
            var anotherProductKind = _scenarioContext.GetValueOrDefault<ProductKind>(AnotherProductKind);

            var request = new UpdateProductKind { Name = anotherProductKind.Name };
            _scenarioContext[ProductKindId] = productKind.Id.ToString();
            _scenarioContext[RequestContentModel] = request;
        }

        [Given(@"I want to delete the product kind")]
        public void GivenIWantToDeleteProduct()
        {
            var productKind = GetTheProductKind();
            _scenarioContext[ProductKindId] = productKind.Id.ToString();
        }

        [Given(@"I want to merge the product kind with another product kind")]
        public void GivenIWantToMergeTheProductKindWithAnotherProductKind()
        {
            var productKind = GetTheProductKind();
            var anotherProductKind = _scenarioContext.GetValueOrDefault<ProductKind>(AnotherProductKind);

            var request = new MergeProductKind
            {
                Id = productKind.Id,
                FirstProductKindId = productKind.Id,
                SecondProductKindId = anotherProductKind.Id,
                NewProductKindName = productKind.Name
            };
            _scenarioContext[RequestContentModel] = request;
        }

        [Then(@"The response should contains both product kind")]
        public void ThenTheResponseShouldContainsTheProductKindAsync()
        {
            var productKind = GetTheProductKind();
            var anotherProductKind = _scenarioContext.GetValueOrDefault<ProductKind>(AnotherProductKind);

            var expectedProductKind = new { Id = productKind.Id, Name = productKind.Name, HasProducts = true };
            var expectedAnotherProductKind = new { Id = anotherProductKind.Id, Name = anotherProductKind.Name, HasProducts = false };

            var productKindCollection = _scenarioContext.GetDeserializedCollectionOrEmpty<ProductKindModel>(ResponseContent);
            productKindCollection.Should().ContainEquivalentOf(expectedProductKind, config => config.ExcludingMissingMembers());
            productKindCollection.Should().ContainEquivalentOf(expectedAnotherProductKind, config => config.ExcludingMissingMembers());
        }
    }
}