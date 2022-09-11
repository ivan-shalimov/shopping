using Newtonsoft.Json;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;
using Shopping.SpecFlow.Extensions;
using Shopping.SpecFlow.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Shopping.SpecFlow.StepDefinitions
{
    [Binding]
    public class ProductKindStepDefinitions
    {
        public ShoppingWebApplicationFactory Factory { get; }

        public ProductKindStepDefinitions(ShoppingWebApplicationFactory factory)
        {
            Factory = factory;
        }

        [Given(@"I want to add new product kind '([^']*)'")]
        public void GivenTheNameForNewProductKindIs(string productKindName)
        {
            var request = new AddProductKind { Name = productKindName, };
            Factory.RequestContent = request.ToHttpContent();
        }

        [Given(@"I want to change the name for product kind from '([^']*)' to '([^']*)'")]
        public void GivenIChangeTheNameForProductKindFromTo(string oldProductKindName, string newProductKindName)
        {
            var productKind = Factory.Context.ProductKinds.Single(x => x.Name == oldProductKindName);
            var request = new UpdateProductKind { Name = newProductKindName, };
            Factory.RequestParameters["id"] = productKind.Id.ToString();
            Factory.RequestContent = request.ToHttpContent();
        }

        [Given(@"I want to delete product kind '([^']*)'")]
        public void GivenIWantToDeleteProduct(string productKindName)
        {
            var productKind = Factory.Context.ProductKinds.Single(x => x.Name == productKindName);
            Factory.RequestParameters["id"] = productKind.Id.ToString();
        }

        [Then(@"The response should contains the product kind '([^']*)'")]
        public async Task ThenTheResponseShouldContainsTheProductKindAsync(string productKindName)
        {
            var responseContentAsString = await Factory.Response.Content.ReadAsStringAsync();
            var productKindCollection = JsonConvert.DeserializeObject<ProductKindModel[]>(responseContentAsString);
            productKindCollection.Should().ContainEquivalentOf(new { Name = productKindName }, config => config.ExcludingMissingMembers());
        }

        [Then(@"The DB should have the product kind '([^']*)'")]
        public void ThenTheDBShouldHaveTheProductKind(string productKindName)
        {
            Factory.Context.ProductKinds.AsNoTracking()
                .Should().ContainSingle(pk => pk.Name == productKindName);
        }

        [Then(@"DB should not have the product kind '([^']*)'")]
        public void ThenDBShouldNotHaveTheProductKind(string productKindName)
        {
            Factory.Context.ProductKinds.AsNoTracking().ToArray()
                .Should().NotContain(pk => pk.Name == productKindName);
        }
    }
}