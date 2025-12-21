using Reqnroll.BoDi;
using Shopping.SpecFlow.Infrastructure;
using Shopping.SpecFlow.StepDefinitions;
using Shopping.SpecFlow.ValueRetrievers;

namespace Shopping.SpecFlow.Hooks
{
    [Binding]
    public class RootHooks
    {
        public static ShoppingWebApplicationFactory ShoppingWebApplicationFactory = new ShoppingWebApplicationFactory();

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Reqnroll.Assist.Service.Instance.ValueRetrievers.Register(new DateTimeValueRetriever());
            Reqnroll.Assist.Service.Instance.ValueRetrievers.Register(new MonthNumberValueRetriever());
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            ShoppingWebApplicationFactory.Dispose();
        }

        [BeforeScenario(Order = 0)]
        public void BeforeScenario(IObjectContainer objectContainer)
        {
            objectContainer.RegisterInstanceAs(ShoppingWebApplicationFactory.GetClient());
            objectContainer.RegisterFactoryAs(_ => ShoppingWebApplicationFactory.GetContext());
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            scenarioContext[ScenarioContextKeys.RandomId] = Guid.NewGuid();
            scenarioContext[ScenarioContextKeys.EmptyId] = Guid.Empty;
            scenarioContext[ScenarioContextKeys.EmptyString] = string.Empty;

            var now = DateTime.UtcNow;
            scenarioContext[ScenarioContextKeys.UtcNow] = now;
            scenarioContext[ScenarioContextKeys.StartOfCurrentMonth] = new DateTime(now.Year, now.Month, 1);
            scenarioContext[ScenarioContextKeys.EndOfCurrentMonth] = new DateTime(now.Year, now.Month, 1).AddMonths(1).AddSeconds(-1);
        }
    }
}