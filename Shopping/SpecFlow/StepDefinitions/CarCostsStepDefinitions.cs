using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Models.Domain;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;
using Shopping.SpecFlow.Extensions;
using static Shopping.SpecFlow.StepDefinitions.ScenarioContextKeys;

namespace Shopping.SpecFlow.StepDefinitions
{
    [Binding]
    public class CarCostsStepDefinitions
    {
        public const string TheCarCost = "TheCarCost";

        private readonly DateTime _startOfThisMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        private readonly ScenarioContext _scenarioContext;
        private readonly ShoppingDbContext _context;

        public CarCostsStepDefinitions(ScenarioContext scenarioContext, ShoppingDbContext context)
        {
            _scenarioContext = scenarioContext;
            _context = context;
        }

        [Given(@"I want to get car costs for current month")]
        public void GivenIWantToGetCarCostsForCurrentMonth()
        {
            _scenarioContext[CurrentMonth] = DateTime.UtcNow.Month;
        }

        [Given(@"I want to add a new car cost with")]
        public void GivenIWantToAddANewCarCost(Table table)
        {
            var request = table.GetInstance<AddCarCost>(_scenarioContext);
            request.Id = Guid.NewGuid();

            _scenarioContext[RequestContentModel] = request;
        }

        [Given(@"There is the car cost in the DB")]
        public void GivenThereIsTheCarCostInTheDB()
        {
            var carCost = new CarCost { Id = Guid.NewGuid(), Description = "Petrol A95", Price = 55.5m, Amount = 8, Date = _startOfThisMonth.AddDays(5) };

            _context.CarCosts.Add(carCost);
            _context.SaveChanges();

            _scenarioContext[Id] = carCost.Id;
        }

        [Given(@"I want to update the car cost with")]
        public void GivenIWantToUpdateTheCarCostWith(Table table)
        {
            var request = table.GetInstance<UpdateCarCost>(_scenarioContext);

            _scenarioContext[RequestContentModel] = request;
        }


        [Given(@"I want to delete the car cost")]
        public void GivenIWantToDeleteTheCarCost()
        {
        }

        [Given(@"There are car costs in the DB")]
        public void GivenThereAreCarCostsInTheDB()
        {
            var carCosts = new[]
            {
                new CarCost { Id = Guid.NewGuid(), Description = "Petrol A95", Price = 65m, Amount = 15, Date = _startOfThisMonth.AddDays(-10) },
                new CarCost { Id = Guid.NewGuid(), Description = "Petrol A95", Price = 55m, Amount = 10, Date = _startOfThisMonth.AddDays(1) },
                new CarCost { Id = Guid.NewGuid(), Description = "Petrol A95", Price = 55.5m, Amount = 8, Date = _startOfThisMonth.AddDays(5) },
            };

            _context.CarCosts.AddRange(carCosts);
            _context.SaveChanges();
        }

        [Then(@"The response should contains car costs for the current month")]
        public void ThenTheResponseShouldContainsCarCosts()
        {
            var carCostsForThisMonth = _context.CarCosts.AsNoTracking()
                .Where(cc => cc.Date > _startOfThisMonth && cc.Date < _startOfThisMonth.AddMonths(1).AddSeconds(-1));
            var expected = carCostsForThisMonth
                .Select(cc => new CarCostModel { Id = cc.Id, Description = cc.Description, Amount = cc.Amount, Price = cc.Price, Date = cc.Date })
                .ToArray();

            var carCostsResponse = _scenarioContext.GetDeserializedCollectionOrEmpty<CarCostModel>(ResponseContent);
            carCostsResponse.Should().BeEquivalentTo(expected);
        }

        [Then(@"The DB should contain a new car cost")]
        public void ThenTheDBShouldContainANewCarCost()
        {
            var addCarCost = _scenarioContext.GetValueOrDefault<AddCarCost>(RequestContentModel);
            var expected = new CarCost
            {
                Id = addCarCost.Id,
                Description = addCarCost.Description,
                Amount = addCarCost.Amount,
                Price = addCarCost.Price,
                Date = addCarCost.Date
            };

            _context.CarCosts.AsNoTracking().Should().ContainSingle(cc => cc.Id == addCarCost.Id)
                .Which.Should().BeEquivalentTo(expected);
        }

        [Then(@"The DB should contain the updated car cost")]
        public void ThenTheDBShouldContainTheUpdatedCarCost()
        {
            var id = _scenarioContext.GetValueOrDefault<Guid>(Id);
            var updateCarCost = _scenarioContext.GetValueOrDefault<UpdateCarCost>(RequestContentModel);
            var expected = new CarCost
            {
                Id = id,
                Description = updateCarCost.Description,
                Amount = updateCarCost.Amount,
                Price = updateCarCost.Price,
                Date = updateCarCost.Date
            };

            _context.CarCosts.AsNoTracking().Should().ContainSingle(cc => cc.Id == id)
                .Which.Should().BeEquivalentTo(expected);
        }

        [Then(@"The DB should not contains the car cost")]
        public void ThenTheDBShouldNotContainsTheCarCost()
        {
            var id = _scenarioContext.GetValueOrDefault<Guid>(Id);

            _context.CarCosts.AsNoTracking().SingleOrDefault(cc => cc.Id == id)
                .Should().BeNull();
        }
    }
}