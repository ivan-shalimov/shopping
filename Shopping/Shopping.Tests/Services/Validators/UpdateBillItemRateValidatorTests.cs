using AwesomeAssertions;
using Shopping.Services.Handlers.Bills;
using Shopping.Services.Validators;
using Shopping.Shared.Requests.Bills;
using Shopping.Tests.TestingUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping.Tests.Services.Validators
{
    public class UpdateBillItemRateValidatorTests : ContextBase
    {
        private static readonly DateTime BillDate = new DateTime(2022, 1, 1);

        private const string BillKey = "TestBill";

        private const string NotQuantifiableTariffKey = "NotQuantifiableTariff";
        private const string VariableRateTariffKey = "VariableRateTariff";

        private readonly BillDomainHelper _billDomainHelper = new BillDomainHelper();
        private UpdateBillItemRateValidator _validator;

        public UpdateBillItemRateValidatorTests() : base()
        {
            _validator = new UpdateBillItemRateValidator(Context);
            Setup();
        }

        private UpdateBillItemRate GetConfiguredRequest(Action<UpdateBillItemRate> config = null)
        {
            var request = new UpdateBillItemRate
            {
                Id = _billDomainHelper.GetBillItem(BillKey, VariableRateTariffKey).Id,
                BillId = _billDomainHelper.GetBill(BillKey).Id,
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
        public async Task ValidateAsync_Result_ShouldBeValid()
        {
            // Arrange
            var request = GetConfiguredRequest();

            // Act
            var result = await _validator.ValidateAsync(request, CancellationToken.None);

            // Assert
            result.Should().BeValid();
        }

        [Fact]
        public async Task ValidateAsync_Result_ShouldBeNotValid_WhenIdIsNotVariableBillItemId()
        {
            // Arrange
            var notVariableBillItem = _billDomainHelper.GetBillItem(BillKey, NotQuantifiableTariffKey);
            var request = GetConfiguredRequest(r => r.Id = notVariableBillItem.Id);
            var expectedMessage = "The Rate can be updated only for the Variable rate type.";

            // Act
            var result = await _validator.ValidateAsync(request, CancellationToken.None);

            // Assert
            result.Should().BeNotValid()
                .Which.Errors.Should().Contain(e => e.ErrorMessage.Contains(expectedMessage));
        }

        [Fact]
        public async Task ValidateAsync_Result_ShouldBeNotValid_WhenIdIsEmpty()
        {
            // Arrange
            var request = GetConfiguredRequest(r => r.Id = Guid.Empty);
            var expectedMessage = "Id is required.";

            // Act
            var result = await _validator.ValidateAsync(request, CancellationToken.None);

            // Assert
            result.Should().BeNotValid()
                .Which.Errors.Should().Contain(e => e.ErrorMessage.Contains(expectedMessage));
        }

        [Fact]
        public async Task ValidateAsync_Result_ShouldBeNotValid_WhenBillIdIsEmpty()
        {
            // Arrange
            var request = GetConfiguredRequest(r => r.BillId = Guid.Empty);
            var expectedMessage = "BillId is required.";

            // Act
            var result = await _validator.ValidateAsync(request, CancellationToken.None);

            // Assert
            result.Should().BeNotValid()
                .Which.Errors.Should().Contain(e => e.ErrorMessage.Contains(expectedMessage));
        }

        [Fact]
        public async Task ValidateAsync_Result_ShouldBeNotValid_WhenRateIsNegative()
        {
            // Arrange
            var request = GetConfiguredRequest(r => r.Rate = -1);
            var expectedMessage = "The Rate can not be negative.";

            // Act
            var result = await _validator.ValidateAsync(request, CancellationToken.None);

            // Assert
            result.Should().BeNotValid()
                .Which.Errors.Should().Contain(e => e.ErrorMessage.Contains(expectedMessage));
        }

        [Fact]
        public async Task ValidateAsync_Result_ShouldBeNotValid_WhenBillItemNotFound()
        {
            // Arrange
            var request = GetConfiguredRequest(r => r.Id = Guid.NewGuid());
            var expectedMessage = "Bill item not found for the provided Id and BillId.";

            // Act
            var result = await _validator.ValidateAsync(request, CancellationToken.None);

            // Assert
            result.Should().BeNotValid()
                .Which.Errors.Should().Contain(e => e.ErrorMessage.Contains(expectedMessage));
        }
    }
}