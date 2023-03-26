using Shopping.Models.Domain;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;
using Shopping.SpecFlow.Extensions;
using static Shopping.SpecFlow.StepDefinitions.ScenarioContextKeys;

namespace Shopping.SpecFlow.StepDefinitions
{
    [Binding]
    public class ReceiptStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        private Receipt GetTheReceipt() => _scenarioContext.GetValueOrDefault<Receipt>(TheReceipt);

        private ReceiptItem GetTheReceiptItem() => _scenarioContext.GetValueOrDefault<ReceiptItem>(TheReceiptItem);

        private Product GetTheProduct() => _scenarioContext.GetValueOrDefault<Product>(TheProduct);

        public ReceiptStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I want to get receipts for current month")]
        public void GivenIWantToGetReceiptsForCurrentMonth()
        {
            _scenarioContext[CurrentMonth] = DateTime.UtcNow.Month;
        }

        [Given(@"I want to add a new receipt with")]
        public void GivenIWantToAddANewReceiptWith(Table table)
        {
            var request = table.GetInstance<AddReceipt>(_scenarioContext);

            _scenarioContext[TheReceipt] = new Receipt { Id = request.Id, Description = request.Description, Date = request.Date, Total = 0 };
            _scenarioContext[RequestContentModel] = request;
        }

        [Given(@"I want to update the receipt with")]
        public void GivenIWantToUpdateTheReceiptWith(Table table)
        {
            var request = table.GetInstance<UpdateReceipt>(_scenarioContext);
            var receipt = GetTheReceipt();
            request.Id = receipt.Id;
            _scenarioContext[TheReceiptId] = receipt.Id;
            _scenarioContext[TheReceipt] = new Receipt { Id = request.Id, Description = request.Description, Date = request.Date, Total = 0 };
            _scenarioContext[RequestContentModel] = request;
        }

        [Given(@"I want to get items of the receipt")]
        public void GivenIWantToGetItemsOfTheReceipt()
        {
            var receipt = GetTheReceipt();
            _scenarioContext[TheReceiptId] = receipt.Id;
        }

        [Given(@"I want to add new item to the receipt with")]
        public void GivenIWantToAddNewItemToTheReceiptWith(Table table)
        {
            var receipt = GetTheReceipt();
            var request = table.GetInstance<AddReceiptItem>(_scenarioContext);
            request.ReceiptId = receipt.Id;
            request.Id = Guid.NewGuid();

            _scenarioContext[TheReceiptItem] = new ReceiptItem
            {
                Id = request.Id,
                ReceiptId = request.ReceiptId,
                ProductId = request.ProductId,

                Amount = request.Amount,
                Price = request.Price,
            };
            _scenarioContext[RequestContentModel] = request;
        }

        [Given(@"I want to update the item of the receipt")]
        public void GivenIWantToUpdateTheItemOfTheReceipt(Table table)
        {
            var receipt = GetTheReceipt();
            var receiptItem = GetTheReceiptItem();
            var request = table.GetInstance<UpdateReceiptItem>(_scenarioContext);
            request.ReceiptId = receipt.Id;
            request.Id = receiptItem.Id;

            _scenarioContext[TheReceiptItem] = new ReceiptItem
            {
                Id = receiptItem.Id,
                ReceiptId = receiptItem.ReceiptId,
                ProductId = receiptItem.ProductId,

                Amount = request.Amount,
                Price = request.Price,
            };
            _scenarioContext[RequestContentModel] = request;
        }

        [Given(@"I want to delete the item of the receipt")]
        public void GivenIWantToDeleteTheItemOfTheReceipt()
        {
            var receipt = GetTheReceipt();
            var receiptItem = GetTheReceiptItem();
            _scenarioContext[TheReceiptId] = receipt.Id;
            _scenarioContext[TheReceiptItemId] = receiptItem.Id;
        }

        [Then(@"The response should contains the receipt")]
        public void ThenTheResponseShouldContainsTheReceipt()
        {
            var receipt = GetTheReceipt();

            var expectedReceipt = new ReceiptModel { Id = receipt.Id, Description = receipt.Description, Date = receipt.Date, Total = receipt.Total };

            var receiptCollection = _scenarioContext.GetDeserializedCollectionOrEmpty<ReceiptModel>(ResponseContent);
            receiptCollection.Should().ContainEquivalentOf(expectedReceipt, config => config.ExcludingMissingMembers());
        }

        [Then(@"The response should contains the item with the product of the receipt")]
        public void ThenTheResponseShouldContainsTheItemOfTheReceipt()
        {
            var receiptItem = GetTheReceiptItem();
            var theProductKind = _scenarioContext.GetValueOrDefault<ProductKind>(TheProductKind);
            var product = GetTheProduct();

            var expectedReceiptItem = new ReceiptItemModel
            {
                Id = receiptItem.Id,
                ProductId = product.Id,
                ProductName = product.Name,
                ProductKind = theProductKind.Name,
                Amount = receiptItem.Amount,
                Price = receiptItem.Price
            };

            var productKindCollection = _scenarioContext.GetDeserializedCollectionOrEmpty<ReceiptItemModel>(ResponseContent);
            productKindCollection.Should().ContainEquivalentOf(expectedReceiptItem, config => config.ExcludingMissingMembers());
        }
    }
}