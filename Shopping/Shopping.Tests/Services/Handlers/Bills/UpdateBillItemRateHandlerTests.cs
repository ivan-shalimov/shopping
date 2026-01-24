using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Shopping.Models.Requests;
using Shopping.Services.Handlers.Bills;
using Shopping.Services.Interfaces;
using Shopping.Shared.Requests.Bills;
using Shopping.Tests.TestingUtilities;

namespace Shopping.Tests.Services.Handlers.Bills
{
    public class UpdateBillItemRateHandlerTests : ContextBase
    {
        private static readonly DateTime BillDate = new DateTime(2022, 1, 1);

        private const string BillKey = "TestBill";

        private const string NotQuantifiableTariffKey = "NotQuantifiableTariff";
        private const string VariableRateTariffKey = "VariableRateTariff";

        private readonly IBackgroundMediatorRequestHandler _backgroundRequestHandler = Substitute.For<IBackgroundMediatorRequestHandler>();

        private readonly BillDomainHelper _billDomainHelper = new BillDomainHelper();
        private UpdateBillItemRateHandler _handler;

        public UpdateBillItemRateHandlerTests() : base()
        {
            _handler = new UpdateBillItemRateHandler(Context, _backgroundRequestHandler);
            Setup();
        }

        private UpdateBillItemRate GetConfiguredRequest(Action<UpdateBillItemRate> config = null)
        {
            var request = new UpdateBillItemRate
            {
                Id = _billDomainHelper.GetBillItem(BillKey, VariableRateTariffKey).Id,
                BillId = _billDomainHelper.GetBill(BillKey).Id,
                Rate = 2.50m,
            };

            config?.Invoke(request);
            return request;
        }

        private void Setup()
        {
            _billDomainHelper.AddNotQuantifiableTariff(NotQuantifiableTariffKey);
            _billDomainHelper.AddTariffPeriod(NotQuantifiableTariffKey, "single", BillDate.AddYears(-1));

            _billDomainHelper.AddVariableTariff(VariableRateTariffKey, BillDate.AddDays(-1));

            _billDomainHelper.AddBill(BillKey, BillDate);
            _billDomainHelper.AddNotQuantifiableBillItem(BillKey, NotQuantifiableTariffKey);
            _billDomainHelper.AddVariableBillItem(BillKey, VariableRateTariffKey, 1.25m);

            _billDomainHelper.Seed(Context);
        }

        [Fact]
        public async Task Handle_Result_ShouldBeSuccess()
        {
            // Arrange
            var request = GetConfiguredRequest();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeSuccess();
        }

        [Fact]
        public async Task Handle_Result_ShouldSaveChanges()
        {
            // Arrange
            var request = GetConfiguredRequest();

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            Context.ChangeTracker.HasChanges().Should().BeFalse();
        }

        [Fact]
        public async Task Handle_Context_ShouldContainUpdatedBillItem()
        {
            // Arrange
            var request = GetConfiguredRequest();
            var expected = new
            {
                Id = request.Id,
                Rate = request.Rate,
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Context.BillItems.Should().ContainSingle(b => b.Id == expected.Id)
                .Which.Should().BeEquivalentTo(expected, config => config.ExcludingMissingMembers());
        }

        [Fact]
        public async Task Handle_ShouldCallExecuteInBackground_WithUpdateBillTotal()
        {
            // Arrange
            var request = GetConfiguredRequest();
            var expected = new
            {
                Id = request.Id,
                Rate = request.Rate,
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            await _backgroundRequestHandler
                .Received(1)
                .ExecuteInBackground(Arg.Is<UpdateBillTotal>(e => e.Id == request.BillId));
        }
    }
}