using Shopping.DataAccess;
using Shopping.SpecFlow.StepDefinitions;
using Xunit.Abstractions;

namespace Shopping.SpecFlow.Hooks
{
    [Binding]
    public class ScenarioHooks
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly ScenarioContext _scenarioContext;
        private readonly ShoppingDbContext _context;

        public ScenarioHooks(ITestOutputHelper outputHelper, ScenarioContext scenarioContext, ShoppingDbContext context)
        {
            _outputHelper = outputHelper;
            _scenarioContext = scenarioContext;
            _context = context;
        }

        [AfterScenario]
        public void AfterScenario(ShoppingDbContext context)
        {
            context.ReceiptItems.RemoveRange(context.ReceiptItems);
            context.SaveChanges();
        }

        [AfterStep]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            if (scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.TestError
                && scenarioContext.Keys.Contains(ScenarioContextKeys.ResponseContent))
            {
                var error = scenarioContext[ScenarioContextKeys.ResponseContent];
                _outputHelper.WriteLine($"Error Info: {error}");
            }
        }
    }
}