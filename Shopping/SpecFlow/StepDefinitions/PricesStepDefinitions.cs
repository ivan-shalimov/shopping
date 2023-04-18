using Shopping.DataAccess;
using Shopping.Models.Domain;
using Shopping.SpecFlow.Contexts;
using Shopping.SpecFlow.Extensions;
using static Shopping.SpecFlow.StepDefinitions.ScenarioContextKeys;

namespace Shopping.SpecFlow.StepDefinitions
{
    [Binding]
    public class PricesStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly ShoppingContext _shoppingContext;
        private readonly ShoppingDbContext _context;

        public PricesStepDefinitions(ScenarioContext scenarioContext, ShoppingContext shoppingContext, ShoppingDbContext context)
        {
            _scenarioContext = scenarioContext;
            _shoppingContext = shoppingContext;
            _context = context;
        }

        [Given(@"I have created a new receipt for '([^']*)' shop")]
        public void GivenIHaveCreatedANewReceiptForTheShop(string shopName)
        {
            var theShopReceipt = new Receipt { Date = DateTime.UtcNow, Id = Guid.NewGuid(), Description = shopName };
            _context.Receipts.Add(theShopReceipt);
            _context.SaveChanges();

            _scenarioContext[TheReceiptId] = theShopReceipt.Id;
        }

        [Given(@"I have selected the following products")]
        public void GivenIHaveSelectedTheFollowingProductsAndAddThemToReceipt(Table table)
        {
            var productIds = new List<Guid>();
            foreach (var row in table.Rows)
            {
                var productName = row["Product Name"];

                productIds.Add(_shoppingContext.Prodcuts[productName]);
            }

            _scenarioContext[ProductIds] = productIds;
        }

        [Given(@"I want to get prices for the products of the shop to fill them for selected products")]
        public void GivenIWantToGetPricesForTheProductsOfTheShopToFillThemForSelectedProducts()
        {
            // there is already filled ProductIds
        }

        [Then(@"The response should contains the following prices")]
        public void ThenTheResponseShouldContainsTheFollowingPrices(Table table)
        {
            var expexted = table.Rows.ToDictionary(
                r => _shoppingContext.Prodcuts.TryGetValue(r[0], out var id) ? id
                : throw new InvalidOperationException($"There is no product with name {r[0]}"),
                r => decimal.TryParse(r[1], out var v) ? v
                : throw new InvalidOperationException($"The value {r[1]} is not valid decimal"));

            var response = _scenarioContext.GetDeserializedValueOrDefault<Dictionary<Guid, decimal>>(ResponseContent);
            response.Should().BeEquivalentTo(expexted);
        }
    }
}