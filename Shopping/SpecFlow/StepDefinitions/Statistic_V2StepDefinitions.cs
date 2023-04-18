using Shopping.DataAccess;
using Shopping.Shared.Models.Results;
using Shopping.SpecFlow.Contexts;
using Shopping.SpecFlow.Extensions;
using System.Collections.ObjectModel;
using TechTalk.SpecFlow.Assist;
using static Shopping.SpecFlow.StepDefinitions.ScenarioContextKeys;

namespace Shopping.SpecFlow.StepDefinitions
{
    [Binding]
    public class Statistic_V2StepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        public ShoppingDbContext _context { get; }

        public Statistic_V2StepDefinitions(ScenarioContext scenarioContext, ShoppingDbContext context)
        {
            _scenarioContext = scenarioContext;
            _context = context;
        }

        [Then(@"The response should be Dictionary<string,decimal> with the following data")]
        public void ThenTheResponseShouldBeDictionaryStringDecimalWithTheFollowingData(Table table)
        {
            var expexted = table.Rows.ToDictionary(
                r => r[0],
                r => decimal.TryParse(r[1], out var v) ? v
                : throw new InvalidOperationException($"The value {r[1]} is not valid decimal"));

            var response = _scenarioContext.GetDeserializedValueOrDefault<Dictionary<string, decimal>>(ResponseContent);
            response.Should().BeEquivalentTo(expexted);
        }

        [Then(@"The response should be Dictionary<int,decimal> with the following data")]
        public void ThenTheResponseShouldBeDictionaryIntDecimalWithTheFollowingData(Table table)
        {
            int GetValue(string value)
            {
                if (value.StartsWith("current"))
                {
                    return DateTime.UtcNow.Month;
                }
                else if (value.StartsWith("previous"))
                {
                    return DateTime.UtcNow.AddMonths(-1).Month;
                }
                throw new NotSupportedException();
            }

            var expexted = table.Rows.ToDictionary(
                r => GetValue(r[0]),
                r => decimal.TryParse(r[1], out var v) ? v
                : throw new InvalidOperationException($"The value {r[1]} is not valid decimal"));

            var response = _scenarioContext.GetDeserializedValueOrDefault<Dictionary<int, decimal>>(ResponseContent);
            response.Should().BeEquivalentTo(expexted);
        }

        [Then(@"The response should be collection of ProductCostChange with the following data")]
        public void ThenTheResponseShouldBeCollectionOfProductCostChangeWithTheFollowingData(Table table)
        {
            var expectedCollection = new Collection<ProductCostChange>();
            foreach (var row in table.Rows)
            {
                var previousCost = decimal.TryParse(row["PreviousCost"], out var previousCostValue) ? previousCostValue
                    : throw new InvalidOperationException($"The PreviousCost {row["PreviousCost"]} is not valid");
                var lastCost = decimal.TryParse(row["LastCost"], out var lastCostValue) ? lastCostValue
                    : throw new InvalidOperationException($"The LastCost {row["LastCost"]} is not valid");
                var changePercent = decimal.TryParse(row["ChangePercent"], out var changePercentValue) ? changePercentValue
                    : throw new InvalidOperationException($"The ChangePercent {row["ChangePercent"]} is not valid");

                var item = new ProductCostChange
                {
                    Name = row["Product Name"],
                    Kind = row["Kind Name"],
                    Shop = row["Shop"],
                    PreviousCost = previousCost,
                    LastCost = lastCost,
                    ChangePercent = changePercent
                };

                expectedCollection.Add(item);
            }

            var response = _scenarioContext.GetDeserializedCollectionOrEmpty<ProductCostChange>(ResponseContent);
            response.Should().BeEquivalentTo(expectedCollection);
        }

        [Then(@"The response should collection of ProductExpensesDetail with the following data")]
        public void ThenTheResponseShouldCollectionOfProductExpensesDetailWithTheFollowingData(Table table)
        {
            var expectedCollection = table.CreateSet<ProductExpensesDetail>(); new Collection<ProductExpensesDetail>();
            //foreach (var row in table.Rows)
            //{
            //    var spentOn = DateTime.TryParse(row["SpentOn"], out var spentOnValue) ? spentOnValue
            //        : throw new InvalidOperationException($"The SpentOn {row["SpentOn"]} is not valid");

            //    var price = decimal.TryParse(row["Price"], out var priceValue) ? priceValue
            //        : throw new InvalidOperationException($"The Price {row["Price"]} is not valid");
            //    var amount = decimal.TryParse(row["Amount"], out var amountValue) ? amountValue
            //        : throw new InvalidOperationException($"The Amount {row["Amount"]} is not valid");

            //    var item = new ProductExpensesDetail
            //    {
            //       ShopName = row["ShopName"],
            //       Amount = amount,
            //       Price = price,
            //       SpentOn = spentOn,
            //    };

            //    expectedCollection.Add(item);
            //}

            var response = _scenarioContext.GetDeserializedCollectionOrEmpty<ProductExpensesDetail>(ResponseContent);
            response.Should().BeEquivalentTo(expectedCollection);
        }
    }
}