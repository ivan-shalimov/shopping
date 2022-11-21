using Shopping.SpecFlow.Extensions;
using System.Net;
using static Shopping.SpecFlow.StepDefinitions.ScenarioContextKeys;

namespace Shopping.SpecFlow.StepDefinitions
{
    [Binding]
    public class HttpStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly HttpClient _client;

        public HttpStepDefinitions(
            ScenarioContext scenarioContext,
            HttpClient client)
        {
            _scenarioContext = scenarioContext;
            _client = client;
        }

        [When(@"I make a POST request to '([^']*)'")]
        public async Task WhenIMakeAPOSTRequestTo(string requestPath)
        {
            requestPath = _scenarioContext.FillInterpolatedString(requestPath);

            await MakeRequest(HttpMethod.Post, requestPath);
        }

        [When(@"I make a PUT request to '([^']*)'")]
        public async Task WhenIMakeAPUTRequestToAsync(string requestPath)
        {
            requestPath = _scenarioContext.FillInterpolatedString(requestPath);

            await MakeRequest(HttpMethod.Put, requestPath);
        }

        [When(@"I make a DELETE request to '([^']*)'")]
        public async Task WhenIMakeADELETERequestTo(string requestPath)
        {
            requestPath = _scenarioContext.FillInterpolatedString(requestPath);

            await MakeRequest(HttpMethod.Delete, requestPath);
        }

        [When(@"I make a DELETE request to '([^']*)' with query parameters")]
        public async Task WhenIMakeADELETERequestWithQueryTo(string requestPath, Table table)
        {
            requestPath = _scenarioContext.FillInterpolatedString(requestPath);
            var query = table.GetInterpolatedQuery(_scenarioContext);

            await MakeRequest(HttpMethod.Delete, $"{requestPath}?{query}");
        }

        [When(@"I make a GET request to '([^']*)'")]
        public async Task WhenIMakeAGETRequestToAsync(string requestPath)
        {
            requestPath = _scenarioContext.FillInterpolatedString(requestPath);

            await MakeRequest(HttpMethod.Get, requestPath);
        }

        [When(@"I make a GET request to '([^']*)' with query parameters")]
        public async Task WhenIMakeAGETRequestWithQueryToAsync(string requestPath, Table table)
        {
            requestPath = _scenarioContext.FillInterpolatedString(requestPath);
            var query = table.GetInterpolatedQuery(_scenarioContext);

            await MakeRequest(HttpMethod.Get, $"{requestPath}?{query}");
        }

        #region status should be

        [Then(@"The response status should be success")]
        public void ThenTheResponseStatusShouldBeSuccess()
        {
            ThenTheResponseStatusShouldBe(HttpStatusCode.OK);
        }

        [Then(@"The response status should be access denied")]
        public void ThenTheResponseStatusShouldBeDeny()
        {
            ThenTheResponseStatusShouldBe(HttpStatusCode.Forbidden);
        }

        [Then(@"The response status should be bad request")]
        public void ThenTheResponseStatusShouldBeBadRequest()
        {
            ThenTheResponseStatusShouldBe(HttpStatusCode.BadRequest);
        }

        [Then(@"The response status should be '([^']*)'")]
        public void ThenTheResponseStatusShouldBe(HttpStatusCode status)
        {
            var statusCode = _scenarioContext.Get<HttpStatusCode>(ResponseStatusCode);
            statusCode.Should().Be(status);
        }

        [Then(@"The response should contains the error '([^']*)'")]
        public void ThenTheResponseShouldContainsTheError(string error)
        {
            var responseContent = _scenarioContext.Get<string>(ResponseContent);
            responseContent.Should().Contain(error);
        }

        #endregion status should be

        private async Task MakeRequest(HttpMethod httpMethod, string requestPathAndQuery)
        {
            var message = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_client.BaseAddress}{requestPathAndQuery}"),
                Method = httpMethod,
                Content = _scenarioContext.TryGetValue(RequestContentModel, out var contentModel) ? contentModel.ToHttpContent() : null,
            };

            var response = await _client.SendAsync(message);

            _scenarioContext[ResponseStatusCode] = response.StatusCode;

            var content = await response.Content.ReadAsStringAsync();
            _scenarioContext[ResponseContent] = content;

#if DEBUG
            if (response.StatusCode != HttpStatusCode.OK)
            {
                System.Diagnostics.Debug.WriteLine(content);
            }
#endif
        }
    }
}