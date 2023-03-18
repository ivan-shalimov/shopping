using Shopping.DataAccess;
using Shopping.Models.Domain;
using Shopping.Shared.Models.Results;
using Shopping.SpecFlow.Extensions;
using static Shopping.SpecFlow.StepDefinitions.ScenarioContextKeys;

namespace Shopping.SpecFlow.StepDefinitions
{
    [Binding]
    public class StatisticStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        public ShoppingDbContext _context { get; }

        public StatisticStepDefinitions(ScenarioContext scenarioContext, ShoppingDbContext context)
        {
            _scenarioContext = scenarioContext;
            _context = context;

            _scenarioContext[TheShop] = "Market";
            _scenarioContext[AnotherShop] = "ATB";

            _scenarioContext[PreviousMonthTheProductCost] = 150.55m;
            _scenarioContext[CurrentMonthTheProductCost] = 125.00m;
            _scenarioContext[PreviousMonthAnotherProductCost] = 130.45m;
            _scenarioContext[CurrentMonthAnotherProductCost] = 200.55m;
        }

        [Given(@"The DB has the set receipt with items created in this month")]
        public void GivenTheDBHasTheSetReceiptWithItemsCreatedInThisMonth()
        {
            var now = DateTime.UtcNow;
            var dateOfReceipt = new DateTime(now.Year, now.Month, 1, now.Hour, now.Minute, 0);

            var theProduct = _scenarioContext.GetValueOrDefault<Product>(TheProduct);
            var anotherProduct = _scenarioContext.GetValueOrDefault<Product>(AnotherProduct);

            var theShop = _scenarioContext.GetValueOrDefault<string>(TheShop);
            var anotherShop = _scenarioContext.GetValueOrDefault<string>(AnotherShop);

            var currentMonthTheProductCost = _scenarioContext.GetValueOrDefault<decimal>(CurrentMonthTheProductCost);
            var currentMonthAnotherProductCost = _scenarioContext.GetValueOrDefault<decimal>(CurrentMonthAnotherProductCost);

            var currentMonthExpensesByTheProductKind = 0m;
            var currentMonthExpensesByAnotherProductKind = 0m;
            var theShopCurrentMonthExpenses = 0m;
            var anotherShopCurrentMonthExpenses = 0m;
            var theProductCurrentMonthExpenses = 0m;
            var theProductExpensesDetails = new List<ProductExpensesDetail>();

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
                    Price = currentMonthTheProductCost,
                    Amount = 10,
                };
                currentMonthExpensesByTheProductKind += currentMonthTheProductCost * 10;
                theProductCurrentMonthExpenses += currentMonthTheProductCost * 10;
                theShopCurrentMonthExpenses += currentMonthTheProductCost * 10;
                theProductExpensesDetails.Add(new ProductExpensesDetail
                {
                    Amount = (int)theShopTheProductReceiptItem.Amount,
                    Price = theShopTheProductReceiptItem.Price,
                    ShopName = theShopReceipt.Description,
                    SpentOn = theShopReceipt.Date,
                });
                _context.ReceiptItems.Add(theShopTheProductReceiptItem);
                var theShopAnotherProductReceiptItem = new ReceiptItem
                {
                    Id = Guid.NewGuid(),
                    ReceiptId = theShopReceipt.Id,
                    ProductId = anotherProduct.Id,
                    Price = currentMonthAnotherProductCost,
                    Amount = 15,
                };
                currentMonthExpensesByAnotherProductKind += currentMonthAnotherProductCost * 15;
                theShopCurrentMonthExpenses += currentMonthAnotherProductCost * 15;
                _context.ReceiptItems.Add(theShopAnotherProductReceiptItem);

                var anotherShopTheProductReceiptItem = new ReceiptItem
                {
                    Id = Guid.NewGuid(),
                    ReceiptId = anotherShopReceipt.Id,
                    ProductId = theProduct.Id,
                    Price = currentMonthTheProductCost,
                    Amount = 100,
                };
                currentMonthExpensesByTheProductKind += currentMonthTheProductCost * 100;
                theProductCurrentMonthExpenses += currentMonthTheProductCost * 100;
                anotherShopCurrentMonthExpenses += currentMonthTheProductCost * 100;
                theProductExpensesDetails.Add(new ProductExpensesDetail
                {
                    Amount = (int)anotherShopTheProductReceiptItem.Amount,
                    Price = anotherShopTheProductReceiptItem.Price,
                    ShopName = anotherShopReceipt.Description,
                    SpentOn = anotherShopReceipt.Date,
                });
                _context.ReceiptItems.Add(anotherShopTheProductReceiptItem);
                var anotherShopAnotherProductReceiptItem = new ReceiptItem
                {
                    Id = Guid.NewGuid(),
                    ReceiptId = anotherShopReceipt.Id,
                    ProductId = anotherProduct.Id,
                    Price = currentMonthAnotherProductCost,
                    Amount = 155,
                };
                currentMonthExpensesByAnotherProductKind += currentMonthAnotherProductCost * 155;
                anotherShopCurrentMonthExpenses += currentMonthAnotherProductCost * 155;
                _context.ReceiptItems.Add(anotherShopAnotherProductReceiptItem);

                dateOfReceipt = dateOfReceipt.AddDays(3);
            }
            while (dateOfReceipt.Month == now.Month);

            _context.SaveChanges();

            _scenarioContext[CurrentMonthExpenses] = (currentMonthExpensesByTheProductKind + currentMonthExpensesByAnotherProductKind);
            _scenarioContext[CurrentMonthExpensesByTheProductKind] =currentMonthExpensesByTheProductKind;
            _scenarioContext[CurrentMonthExpensesByAnotherProductKind] = currentMonthExpensesByAnotherProductKind;
            _scenarioContext[TheShopCurrentMonthExpenses] =theShopCurrentMonthExpenses;
            _scenarioContext[TheProductCurrentMonthExpenses] = theProductCurrentMonthExpenses;
            _scenarioContext[AnotherShopCurrentMonthExpenses] = anotherShopCurrentMonthExpenses;
            _scenarioContext[TheProductExpensesDetails] = theProductExpensesDetails.ToArray();
        }

        [Given(@"The DB has the set receipt with items created in previous month")]
        public void GivenTheDBHasTheSetReceiptWithItemsCreatedInPreviousMonth()
        {
            var now = DateTime.UtcNow.AddMonths(-1);
            var dateOfReceipt = new DateTime(now.Year, now.Month, 1, now.Hour, now.Minute, 0);

            var theProduct = _scenarioContext.GetValueOrDefault<Product>(TheProduct);
            var anotherProduct = _scenarioContext.GetValueOrDefault<Product>(AnotherProduct);

            var theShop = _scenarioContext.GetValueOrDefault<string>(TheShop);
            var anotherShop = _scenarioContext.GetValueOrDefault<string>(AnotherShop);

            var previousMonthTheProductCost = _scenarioContext.GetValueOrDefault<decimal>(PreviousMonthTheProductCost);
            var previousMonthAnotherProductCost = _scenarioContext.GetValueOrDefault<decimal>(PreviousMonthAnotherProductCost);

            var previousMonthExpenses = 0m;

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
                    Price = previousMonthTheProductCost,
                    Amount = 10,
                };
                previousMonthExpenses += previousMonthTheProductCost * 10;
                _context.ReceiptItems.Add(theShopTheProductReceiptItem);
                var theShopAnotherProductReceiptItem = new ReceiptItem
                {
                    Id = Guid.NewGuid(),
                    ReceiptId = theShopReceipt.Id,
                    ProductId = anotherProduct.Id,
                    Price = previousMonthAnotherProductCost,
                    Amount = 15,
                };
                previousMonthExpenses += previousMonthAnotherProductCost * 15;
                _context.ReceiptItems.Add(theShopAnotherProductReceiptItem);

                var anotherShopTheProductReceiptItem = new ReceiptItem
                {
                    Id = Guid.NewGuid(),
                    ReceiptId = anotherShopReceipt.Id,
                    ProductId = theProduct.Id,
                    Price = previousMonthTheProductCost,
                    Amount = 100,
                };
                previousMonthExpenses += previousMonthTheProductCost * 100;
                _context.ReceiptItems.Add(anotherShopTheProductReceiptItem);
                var anotherShopAnotherProductReceiptItem = new ReceiptItem
                {
                    Id = Guid.NewGuid(),
                    ReceiptId = anotherShopReceipt.Id,
                    ProductId = anotherProduct.Id,
                    Price = previousMonthAnotherProductCost,
                    Amount = 155,
                };
                previousMonthExpenses += previousMonthAnotherProductCost * 155;
                _context.ReceiptItems.Add(anotherShopAnotherProductReceiptItem);

                dateOfReceipt = dateOfReceipt.AddDays(3);
            }
            while (dateOfReceipt.Month == now.Month);

            _context.SaveChanges();

            _scenarioContext[PreviousMonthExpenses] = previousMonthExpenses;
        }

        [Then(@"The response should contains expenses of current month grouped by kind")]
        public void ThenTheResponseShouldContainsExpensesOfCurrentMonthGroupedByKind()
        {
            var productKind = _scenarioContext.GetValueOrDefault<ProductKind>(TheProductKind);
            var anotherProductKind = _scenarioContext.GetValueOrDefault<ProductKind>(AnotherProductKind);

            var currentMonthExpensesByTheProductKind = _scenarioContext.GetValueOrDefault<decimal>(CurrentMonthExpensesByTheProductKind);
            var currentMonthExpensesByAnotherProductKind = _scenarioContext.GetValueOrDefault<decimal>(CurrentMonthExpensesByAnotherProductKind);

            var response = _scenarioContext.GetDeserializedValueOrDefault<Dictionary<string, decimal>>(ResponseContent);
            response.Should().ContainKey(productKind.Name).WhoseValue.Should().Be(currentMonthExpensesByTheProductKind);
            response.Should().ContainKey(anotherProductKind.Name).WhoseValue.Should().Be(currentMonthExpensesByAnotherProductKind);
        }

        [Then(@"The response should contains expenses of this year by month")]
        public void ThenTheResponseShouldContainsExpensesOfThisYearByMonth()
        {
            var currentMonth = DateTime.UtcNow.Month;
            var currentMonthExpenses = _scenarioContext.GetValueOrDefault<decimal>(CurrentMonthExpenses);
            var previousMonth = DateTime.UtcNow.AddMonths(-1).Month;
            var previousMonthExpenses = _scenarioContext.GetValueOrDefault<decimal>(PreviousMonthExpenses);

            var response = _scenarioContext.GetDeserializedValueOrDefault<Dictionary<int, decimal>>(ResponseContent);
            response.Should().ContainKey(currentMonth).WhoseValue.Should().Be(currentMonthExpenses);
            response.Should().ContainKey(previousMonth).WhoseValue.Should().Be(previousMonthExpenses);
        }

        [Then(@"The response should contains expenses of current month grouped by shop")]
        public void ThenTheResponseShouldContainsExpensesOfCurrentMonthGroupedByShop()
        {
            var theShop = _scenarioContext.GetValueOrDefault<string>(TheShop);
            var theShopCurrentMonthExpenses = _scenarioContext.GetValueOrDefault<decimal>(TheShopCurrentMonthExpenses);
            var anotherShop = _scenarioContext.GetValueOrDefault<string>(AnotherShop);
            var anotherShopCurrentMonthExpenses = _scenarioContext.GetValueOrDefault<decimal>(AnotherShopCurrentMonthExpenses);

            var response = _scenarioContext.GetDeserializedValueOrDefault<Dictionary<string, decimal>>(ResponseContent);
            response.Should().ContainKey(theShop).WhoseValue.Should().Be(theShopCurrentMonthExpenses);
            response.Should().ContainKey(anotherShop).WhoseValue.Should().Be(anotherShopCurrentMonthExpenses);
        }

        [Then(@"The response should contains product cost change")]
        public void ThenTheResponseShouldContainsProductCostChange()
        {
            var productKind = _scenarioContext.GetValueOrDefault<ProductKind>(TheProductKind);
            var anotherProductKind = _scenarioContext.GetValueOrDefault<ProductKind>(AnotherProductKind);
            var theProduct = _scenarioContext.GetValueOrDefault<Product>(TheProduct);
            var anotherProduct = _scenarioContext.GetValueOrDefault<Product>(AnotherProduct);

            var theShop = _scenarioContext.GetValueOrDefault<string>(TheShop);
            var anotherShop = _scenarioContext.GetValueOrDefault<string>(AnotherShop);

            var previousMonthTheProductCost = _scenarioContext.GetValueOrDefault<decimal>(PreviousMonthTheProductCost);
            var currentMonthTheProductCost = _scenarioContext.GetValueOrDefault<decimal>(CurrentMonthTheProductCost);

            var previousMonthAnotherProductCost = _scenarioContext.GetValueOrDefault<decimal>(PreviousMonthAnotherProductCost);
            var currentMonthAnotherProductCost = _scenarioContext.GetValueOrDefault<decimal>(CurrentMonthAnotherProductCost);

            var theProdcutCostChange = new ProductCostChange
            {
                Name = theProduct.Name,
                Kind = productKind.Name,
                Shop = theShop,
                PreviousCost = previousMonthTheProductCost,
                LastCost = currentMonthTheProductCost,
                ChangePercent = (currentMonthTheProductCost - previousMonthTheProductCost) / previousMonthTheProductCost,
            };

            var anotherProdcutCostChange = new ProductCostChange
            {
                Name = anotherProduct.Name,
                Kind = anotherProductKind.Name,
                Shop = anotherShop,
                PreviousCost = previousMonthAnotherProductCost,
                LastCost = currentMonthAnotherProductCost,
                ChangePercent = (currentMonthAnotherProductCost - previousMonthAnotherProductCost) / previousMonthAnotherProductCost,
            };

            var response = _scenarioContext.GetDeserializedCollectionOrEmpty<ProductCostChange>(ResponseContent);
            response.Should().ContainEquivalentOf(theProdcutCostChange);
            response.Should().ContainEquivalentOf(anotherProdcutCostChange);
        }

        [Then(@"The response should contains expenses for the kind of the current month grouped by product")]
        public void ThenTheResponseShouldContainsExpensesForTheKindOfTheCurrentMonthGroupedByProduct()
        {
            var theProductCurrentMonthExpenses = _scenarioContext.GetValueOrDefault<decimal>(TheProductCurrentMonthExpenses);
            var theProduct = _scenarioContext.GetValueOrDefault<Product>(TheProduct);

            var response = _scenarioContext.GetDeserializedValueOrDefault<Dictionary<string, decimal>>(ResponseContent);
            response.Should().ContainKey(theProduct.Name).WhoseValue.Should().Be(theProductCurrentMonthExpenses);
        }

        [Then(@"The response should contains details of expenses for the product of the current month")]
        public void ThenTheResponseShouldContainsDetailsOfExpensesForTheProductOfTheCurrentMonth()
        {
            var expectedDetails = _scenarioContext.GetValueOrDefault<ProductExpensesDetail[]>(TheProductExpensesDetails);

            var response = _scenarioContext.GetDeserializedCollectionOrEmpty<ProductExpensesDetail>(ResponseContent);

            response.Should().BeEquivalentTo(expectedDetails);
        }
    }
}