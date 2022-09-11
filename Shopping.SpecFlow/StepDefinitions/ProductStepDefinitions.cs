using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;
using Shopping.SpecFlow.Extensions;
using Shopping.SpecFlow.Infrastructure;

namespace Shopping.SpecFlow.StepDefinitions
{
    [Binding]
    public class ProductStepDefinitions
    {
        public ShoppingWebApplicationFactory Factory { get; }

        public ProductStepDefinitions(ShoppingWebApplicationFactory factory)
        {
            Factory = factory;
        }

        [Given(@"I want to add new product '([^']*)' for the product kind '([^']*)'")]
        public void GivenIWantToAddNewProductForTheProductKind(string productName, string productKindName)
        {
            var productKind = Factory.Context.ProductKinds.Single(x => x.Name == productKindName);
            var request = new AddProduct
            {
                Name = productName,
                ProductKindId = productKind.Id,
            };
            Factory.RequestContent = request.ToHttpContent();
        }

        [Given(@"I want to rename product '([^']*)' to '([^']*)' and change product kind to '([^']*)'")]
        public void GivenIWantToRenameProductToAndChangeProductKindTo(string oldProductName, string newProductName, string newProductKindName)
        {
            var productKind = Factory.Context.ProductKinds.Single(x => x.Name == newProductKindName);
            var product = Factory.Context.Products.Single(x => x.Name == oldProductName);
            var request = new UpdateProduct { Name = newProductName, ProductKindId = productKind.Id, };
            Factory.RequestParameters["id"] = product.Id.ToString();
            Factory.RequestContent = request.ToHttpContent();
        }

        [Given(@"I want to delete product '([^']*)'")]
        public void GivenIWantToDeleteProduct(string productName)
        {
            var product = Factory.Context.Products.Single(x => x.Name == productName);
            Factory.RequestParameters["id"] = product.Id.ToString();
        }

        [Then(@"The result contains the product '([^']*)' with the product kind '([^']*)'")]
        public async Task ThenTheResultContainsTheProductWithTheProductKindAsync(string productName, string productKindName)
        {
            var responseContentAsString = await Factory.Response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<ProductModel[]>(responseContentAsString);
            products.Should().ContainEquivalentOf(new { Name = productName, ProductKindName = productKindName }, config => config.ExcludingMissingMembers());
        }

        [Then(@"The DB should have the product '([^']*)' for the product kind '([^']*)'")]
        public void ThenTheDBShouldHaveTheProductForTheProductKind(string productName, string productKindName)
        {
            var productKind = Factory.Context.ProductKinds.Single(x => x.Name == productKindName);
            Factory.Context.Products.AsNoTracking()
                .Should().ContainSingle(p => p.Name == productName && p.ProductKindId == productKind.Id);
        }

        [Then(@"The DB should not have the product '([^']*)'")]
        public void ThenTheDBShouldNotHaveTheProduct(string productName)
        {
            Factory.Context.Products.AsNoTracking()
                .Should().NotContain(p => p.Name == productName);
        }
    }
}