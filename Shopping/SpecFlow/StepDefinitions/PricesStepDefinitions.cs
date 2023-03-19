using Shopping.DataAccess;
using Shopping.Models.Domain;
using Shopping.SpecFlow.Extensions;
using static Shopping.SpecFlow.StepDefinitions.ScenarioContextKeys;

namespace Shopping.SpecFlow.StepDefinitions
{
    [Binding]
    public class PricesStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        public ShoppingDbContext _context { get; }

        public PricesStepDefinitions(ScenarioContext scenarioContext, ShoppingDbContext context)
        {
            _scenarioContext = scenarioContext;
            _context = context;

            _scenarioContext[TheShop] = "Market";
            _scenarioContext[AnotherShop] = "ATB";
        }

        [Given(@"There are some receipt of two shops")]
        public void GivenThereAreSomeReceiptOfTwoShops()
        {
            var dateOfReceipt = DateTime.UtcNow.AddDays(-10);

            var theProduct = _scenarioContext.GetValueOrDefault<Product>(TheProduct);
            var anotherProduct = _scenarioContext.GetValueOrDefault<Product>(AnotherProduct);

            var theShop = _scenarioContext.GetValueOrDefault<string>(TheShop);
            var anotherShop = _scenarioContext.GetValueOrDefault<string>(AnotherShop);

            var counter = 0;
            var rnd = new Random();

            var theProductCost = rnd.Next(1, 999) / 10m;
            var anotherProductCost = rnd.Next(1, 999) / 10m;

            // cycle adds receipt item from earlier to older date, so the first costs are the latest
            _scenarioContext[LastTheProductCost] = theProductCost;
            _scenarioContext[LastAnotherProductCost] = anotherProductCost;

            do
            {
                var theShopReceipt = new Receipt { Date = dateOfReceipt, Id = Guid.NewGuid(), Description = theShop };
                var anotherShopReceipt = new Receipt { Date = dateOfReceipt, Id = Guid.NewGuid(), Description = anotherShop };
                _context.Receipts.Add(theShopReceipt);
                _context.Receipts.Add(anotherShopReceipt);

                var theShopTheProductReceiptItem = new ReceiptItem
                {
                    Id = Guid.NewGuid(),
                    ReceiptId = theShopReceipt.Id,
                    ProductId = theProduct.Id,
                    Price = theProductCost,
                    Amount = 10,
                };

                _context.ReceiptItems.Add(theShopTheProductReceiptItem);
                var theShopAnotherProductReceiptItem = new ReceiptItem
                {
                    Id = Guid.NewGuid(),
                    ReceiptId = theShopReceipt.Id,
                    ProductId = anotherProduct.Id,
                    Price = anotherProductCost,
                    Amount = 15,
                };

                _context.ReceiptItems.Add(theShopAnotherProductReceiptItem);

                // generate new prices for another shop
                theProductCost = rnd.Next(1, 999) / 10m;
                anotherProductCost = rnd.Next(1, 999) / 10m;

                var anotherShopTheProductReceiptItem = new ReceiptItem
                {
                    Id = Guid.NewGuid(),
                    ReceiptId = anotherShopReceipt.Id,
                    ProductId = theProduct.Id,
                    Price = theProductCost,
                    Amount = 100,
                };

                _context.ReceiptItems.Add(anotherShopTheProductReceiptItem);
                var anotherShopAnotherProductReceiptItem = new ReceiptItem
                {
                    Id = Guid.NewGuid(),
                    ReceiptId = anotherShopReceipt.Id,
                    ProductId = anotherProduct.Id,
                    Price = anotherProductCost,
                    Amount = 155,
                };
                _context.ReceiptItems.Add(anotherShopAnotherProductReceiptItem);

                dateOfReceipt = dateOfReceipt.AddDays(-2);
                counter++;
                theProductCost = rnd.Next(1, 999) / 10m;
                anotherProductCost = rnd.Next(1, 999) / 10m;
            }
            while (counter < 5);

            _context.SaveChanges();
        }

        [Given(@"I have created a new receipt for the shop")]
        public void GivenIHaveCreatedANewReceiptForTheShop()
        {
            var theShop = _scenarioContext.GetValueOrDefault<string>(TheShop);
            var theShopReceipt = new Receipt { Date = DateTime.UtcNow, Id = Guid.NewGuid(), Description = theShop };
            _context.Receipts.Add(theShopReceipt);
            _context.SaveChanges();

            _scenarioContext[TheReceiptId] = theShopReceipt.Id;
        }

        [Given(@"I have selected products to add them to receipt")]
        public void GivenIHaveSelectedProductsToAddThemToReceipt()
        {
            // there is already filled ProductId and AnotherProductId
        }

        [Given(@"I want to get prices for the products of the shop to fill them for selected products")]
        public void GivenIWantToGetPricesForTheProductsOfTheShopToFillThemForSelectedProducts()
        {
            // there is already filled ProductId and AnotherProductId
        }

        [Then(@"The response should contains last product's prices of the shop")]
        public void ThenTheResponseShouldContainsLastProductsPricesOfTheShop()
        {
            var productId = _scenarioContext.GetValueOrDefault<Guid>(ProductId);
            var anotherProductId = _scenarioContext.GetValueOrDefault<Guid>(AnotherProductId);

            var lastTheProductCost = _scenarioContext.GetValueOrDefault<decimal>(LastTheProductCost);
            var lastAnotherProductCost = _scenarioContext.GetValueOrDefault<decimal>(LastAnotherProductCost);

            var response = _scenarioContext.GetDeserializedValueOrDefault<Dictionary<Guid, decimal>>(ResponseContent);
            response.Should().ContainKey(productId).WhoseValue.Should().Be(lastTheProductCost);
            response.Should().ContainKey(anotherProductId).WhoseValue.Should().Be(lastAnotherProductCost);
        }
    }
}