using Shopping.SpecFlow.Infrastructure;
using System.Net;

namespace Shopping.SpecFlow.StepDefinitions
{
    [Binding]
    public class CommonStepDefinitions
    {
        public ShoppingWebApplicationFactory Factory { get; }

        public CommonStepDefinitions(ShoppingWebApplicationFactory factory)
        {
            Factory = factory;
        }

        [Given(@"The DB has a product kind '([^']*)'")]
        public void GivenTheDBHasAProductKind(string productKindName)
        {
            if (!Factory.Context.ProductKinds.Any(x => x.Name == productKindName))
            {
                Factory.Context.ProductKinds.Add(new Models.Domain.ProductKind { Id = Guid.NewGuid(), Name = productKindName });
                Factory.Context.SaveChanges();
            }
        }

        [Given(@"The DB has a product '([^']*)' for the product kind '([^']*)'")]
        public void GivenTheDBHasAProductForTheProductKind(string productName, string productKindName)
        {
            var productKind = Factory.Context.ProductKinds.Single(x => x.Name == productKindName);
            if (!Factory.Context.Products.Any(x => x.Name == productName && x.ProductKindId == productKind.Id))
            {
                Factory.Context.Products.Add(new Models.Domain.Product { Id = Guid.NewGuid(), ProductKindId = productKind.Id, Name = productName });
                Factory.Context.SaveChanges();
            }
        }

        [Given(@"I am a client")]
        public void GivenIAmAClient()
        {
            Factory.InitClient();
        }

        [When(@"I make a Get request to '([^']*)'")]
        public async Task WhenIMakeAGETRequestTo(string requestUri)
        {
            Factory.Response = await Factory.Client.GetAsync(requestUri);
        }

        [When(@"I make a POST request to '([^']*)'")]
        public async Task WhenIMakeAPOSTRequestTo(string requestUri)
        {
            Factory.Response = await Factory.Client.PostAsync(requestUri, Factory.RequestContent);
        }

        [When(@"I make a PUT request to '([^']*)'")]
        public async Task WhenIMakeAPUTRequestTo(string requestUri)
        {
            var uri = requestUri.Replace("{id}", Factory.RequestParameters["id"]);
            Factory.Response = await Factory.Client.PutAsync(uri, Factory.RequestContent);
        }

        [When(@"I make a Delete request to '([^']*)'")]
        public async Task WhenIMakeADeleteRequestTo(string requestUri)
        {
            var uri = requestUri.Replace("{id}", Factory.RequestParameters["id"]);
            Factory.Response = await Factory.Client.DeleteAsync(uri);
        }

        [Then(@"The response should have status code '(.*)'")]
        public void ThenTheResponseStatusCodeIs(int statusCode)
        {
            var expected = (HttpStatusCode)statusCode;
            Factory.Response.StatusCode.Should().Be(expected);
        }

        [Then(@"The response should contains the error '([^']*)'")]
        public async Task ThenTheResponseShouldContainsTheErrorAsync(string error)
        {
            var responseContentAsString = await Factory.Response.Content.ReadAsStringAsync();
            responseContentAsString.Should().Contain(error);
        }
    }
}