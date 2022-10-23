using BoDi;
using Shopping.SpecFlow.Infrastructure;

namespace Shopping.SpecFlow.Hooks
{
    [Binding]
    public class RootHooks
    {
        public static ShoppingWebApplicationFactory ShoppingWebApplicationFactory = new ShoppingWebApplicationFactory();

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
    }
}