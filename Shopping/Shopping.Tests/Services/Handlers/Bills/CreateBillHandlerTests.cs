using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using Shopping.Models.Domain;
using Shopping.Services.Handlers.Bills;
using Shopping.Shared.Requests.Bills;
using Shopping.Tests.TestingUtilities;

namespace Shopping.Tests.Services.Handlers.Bills
{
    public class CreateBillHandlerTests : ContextBase
    {
        private static readonly DateTime BillDate = new DateTime(2022, 1, 1);

        private const string November2021BillKey = "November2021Bill";
        private const string December2021BillKey = "December2021Bill";

        private const string QuantifiableTariffKey = "QuantifiableTariff";
        private const string NotQuantifiableTariffKey = "NotQuantifiableTariff";
        private const string VariableRateTariffKey = "VariableRateTariff";

        private const string OldTariffPeriodKey = "OldTariffPeriod";
        private const string CurrentTariffPeriodKey = "CurrentTariffPeriod";

        private readonly BillDomainHelper _billDomainHelper = new BillDomainHelper();
        private CreateBillHandler _handler;

        public CreateBillHandlerTests() : base()
        {
            _handler = new CreateBillHandler(Context);
            Setup();
        }

        private CreateBill GetConfiguredRequest(Action<CreateBill> config = null)
        {
            var request = new CreateBill
            {
                Id = 202201.ToGuid(),
                Description = "January 2022 Bill",
                Month = BillDate.Month,
                Year = BillDate.Year,
            };

            config?.Invoke(request);
            return request;
        }

        private void Setup()
        {
            _billDomainHelper.AddQuantifiableTariff(QuantifiableTariffKey);
            _billDomainHelper.AddTariffPeriod(QuantifiableTariffKey, OldTariffPeriodKey, BillDate.AddYears(-1), BillDate.AddDays(-1));
            _billDomainHelper.AddTariffPeriod(QuantifiableTariffKey, CurrentTariffPeriodKey, BillDate.AddDays(-1));

            _billDomainHelper.AddNotQuantifiableTariff(NotQuantifiableTariffKey);
            _billDomainHelper.AddTariffPeriod(NotQuantifiableTariffKey, OldTariffPeriodKey, BillDate.AddYears(-1), BillDate.AddDays(-1));
            _billDomainHelper.AddTariffPeriod(NotQuantifiableTariffKey, CurrentTariffPeriodKey, BillDate.AddDays(-1));

            _billDomainHelper.AddVariableTariff(VariableRateTariffKey, BillDate.AddDays(-1));

            _billDomainHelper.AddBill(November2021BillKey, new DateTime(2021, 11, 1));
            _billDomainHelper.AddQuantifiableBillItem(November2021BillKey, QuantifiableTariffKey, 50);
            _billDomainHelper.AddNotQuantifiableBillItem(November2021BillKey, NotQuantifiableTariffKey);
            _billDomainHelper.AddVariableBillItem(November2021BillKey, VariableRateTariffKey, 1.25m);

            _billDomainHelper.AddBill(December2021BillKey, new DateTime(2021, 12, 1));
            _billDomainHelper.AddQuantifiableBillItem(December2021BillKey, QuantifiableTariffKey, 70);
            _billDomainHelper.AddNotQuantifiableBillItem(December2021BillKey, NotQuantifiableTariffKey);
            _billDomainHelper.AddVariableBillItem(December2021BillKey, VariableRateTariffKey, 1.75m);

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
        public async Task Handle_Context_ShouldContainBill()
        {
            // Arrange
            var request = GetConfiguredRequest();

            var tariffPeriod = _billDomainHelper.GetTariffPeriod(NotQuantifiableTariffKey, CurrentTariffPeriodKey);
            var prevousBillItem = _billDomainHelper.GetBillItem(December2021BillKey, VariableRateTariffKey);
            var expected = new
            {
                Id = request.Id,
                Description = request.Description,
                CreatedOn = new DateTime(request.Year, request.Month, 1),
                Total = (tariffPeriod.Rate + prevousBillItem.Rate) * 100, // calculate initial total based on expected bill items
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Context.Bills.Should().ContainSingle(b => b.Id == expected.Id)
                .Which.Should().BeEquivalentTo(expected, config => config.ExcludingMissingMembers());
        }

        [Fact]
        public async Task Handle_Context_ShouldContainBillItemsOnlyForExpectedTariffs()
        {
            // Arrange
            var request = GetConfiguredRequest();
            var expectedTarifIds = new Guid[]
            {
                _billDomainHelper.GetTariff( QuantifiableTariffKey).Id,
                _billDomainHelper.GetTariff( NotQuantifiableTariffKey).Id,
                _billDomainHelper.GetTariff( VariableRateTariffKey).Id,
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Context.BillItems.AsNoTracking().Where(e => e.BillId == request.Id)
                .Should().HaveCount(expectedTarifIds.Length)
                .And.Contain(b => expectedTarifIds.Contains(b.TariffId));
        }

        [Fact]
        public async Task Handle_Context_ShouldContainBillItemsForQuantifiableTariff()
        {
            // Arrange
            var request = GetConfiguredRequest();
            var prevousBillItem = _billDomainHelper.GetBillItem(December2021BillKey, QuantifiableTariffKey);
            var tariffPeriod = _billDomainHelper.GetTariffPeriod(QuantifiableTariffKey, CurrentTariffPeriodKey);
            var expected = new
            {
                Quantity = 0,
                PreviousValue = prevousBillItem.PreviousValue + prevousBillItem.Quantity,
                Rate = tariffPeriod.Rate,
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Context.BillItems.AsNoTracking()
                .Should().ContainSingle(b => b.BillId == request.Id && b.TariffId == tariffPeriod.TariffId)
                .Which
                .Should().BeEquivalentTo(expected, config => config.ExcludingMissingMembers());
        }

        [Fact]
        public async Task Handle_Context_ShouldContainBillItemsForNotQuantifiableTariff()
        {
            // Arrange
            var request = GetConfiguredRequest();
            var tariffPeriod = _billDomainHelper.GetTariffPeriod(NotQuantifiableTariffKey, CurrentTariffPeriodKey);
            var expected = new
            {
                Quantity = 1,
                PreviousValue = 1,
                Rate = tariffPeriod.Rate,
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Context.BillItems.AsNoTracking()
                .Should().ContainSingle(b => b.BillId == request.Id && b.TariffId == tariffPeriod.TariffId)
                .Which
                .Should().BeEquivalentTo(expected, config => config.ExcludingMissingMembers());
        }

        [Fact]
        public async Task Handle_Context_ShouldContainBillItemsForVariableRateTariff()
        {
            // Arrange
            var request = GetConfiguredRequest();
            var prevousBillItem = _billDomainHelper.GetBillItem(December2021BillKey, VariableRateTariffKey);
            var expected = new
            {
                Quantity = 1,
                PreviousValue = 1,
                Rate = prevousBillItem.Rate,
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Context.BillItems.AsNoTracking()
                .Should().ContainSingle(b => b.BillId == request.Id && b.TariffId == prevousBillItem.TariffId)
                .Which
                .Should().BeEquivalentTo(expected, config => config.ExcludingMissingMembers());
        }

        [Fact]
        public async Task Handle_Result_ShouldBeSuccess_WhenNoPreviousBill()
        {
            // Arrange
            Context.Bills.RemoveRange(Context.Bills);
            Context.BillItems.RemoveRange(Context.BillItems);
            Context.SaveChanges();

            var request = GetConfiguredRequest();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeSuccess();
        }

        [Fact]
        public async Task Handle_Context_ShouldContainBill_WhenNoPreviousBill()
        {
            // Arrange
            Context.Bills.RemoveRange(Context.Bills);
            Context.BillItems.RemoveRange(Context.BillItems);
            Context.SaveChanges();

            var request = GetConfiguredRequest();
            var expected = new
            {
                Id = request.Id,
                Description = request.Description,
                CreatedOn = new DateTime(request.Year, request.Month, 1),
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Context.Bills.Should().ContainSingle(b => b.Id == expected.Id)
                .Which.Should().BeEquivalentTo(expected, config => config.ExcludingMissingMembers());
        }

        [Fact]
        public async Task Handle_Context_QuantifiableTariffShouldHaveZeroPreviousValue_WhenNoPreviousBill()
        {
            // Arrange
            Context.Bills.RemoveRange(Context.Bills);
            Context.BillItems.RemoveRange(Context.BillItems);
            Context.SaveChanges();
            var quantifiableTariff = _billDomainHelper.GetTariff(QuantifiableTariffKey);

            var request = GetConfiguredRequest();
            var expected = new
            {
                Quantity = 0,
                PreviousValue = 0,
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Context.BillItems.AsNoTracking()
                .Should().ContainSingle(b => b.BillId == request.Id && b.TariffId == quantifiableTariff.Id)
                .Which
                .Should().BeEquivalentTo(expected, config => config.ExcludingMissingMembers());
        }

        [Fact]
        public async Task Handle_Context_VariableRateTariffShouldHaveZeroRate_WhenNoPreviousBill()
        {
            // Arrange
            Context.Bills.RemoveRange(Context.Bills);
            Context.BillItems.RemoveRange(Context.BillItems);
            Context.SaveChanges();
            var variableRateTariff = _billDomainHelper.GetTariff(VariableRateTariffKey);

            var request = GetConfiguredRequest();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Context.BillItems.AsNoTracking()
                .Should().ContainSingle(b => b.BillId == request.Id && b.TariffId == variableRateTariff.Id)
                .Which.Rate
                .Should().Be(0);
        }

        [Fact]
        public async Task Handle_Context_WhenPreviousBillMissesTariff_ShouldUseDefaultValues()
        {
            // Arrange
            var newTariff = new Tariff
            {
                Id = 7.ToGuid(),
                RateType = RateType.Constant,
                Description = "New Tariff",
                GroupName = "New",
                Quantifiable = true
            };
            var newTariffPeriod = new TariffPeriod
            {
                Id = 27.ToGuid(),
                TariffId = newTariff.Id,
                StartOn = BillDate,
                Rate = 5.0m
            };
            Context.Add(newTariff);
            Context.Add(newTariffPeriod);
            Context.SaveChanges();

            var request = GetConfiguredRequest();
            var expected = new
            {
                Quantity = 0,
                PreviousValue = 0,
                Rate = newTariffPeriod.Rate,
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Context.BillItems.AsNoTracking()
                .Should().ContainSingle(b => b.BillId == request.Id && b.TariffId == newTariff.Id)
                .Which
                .Should().BeEquivalentTo(expected, config => config.ExcludingMissingMembers());
        }

        [Fact]
        public async Task Handle_Context_ShouldUseRateFromTariffPeriodForConstantRateType()
        {
            // Arrange
            var request = GetConfiguredRequest();
            var quantifiableTariffPeriod = _billDomainHelper.GetTariffPeriod(QuantifiableTariffKey, CurrentTariffPeriodKey);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Context.BillItems.AsNoTracking()
                .Should().ContainSingle(b => b.BillId == request.Id && b.TariffId == quantifiableTariffPeriod.TariffId)
                .Which.Rate.Should().Be(quantifiableTariffPeriod.Rate);
        }

        [Fact]
        public async Task Handle_Context_ShouldUseRateFromPreviousBillForVariableRateType()
        {
            // Arrange
            var request = GetConfiguredRequest();
            var previousBillItem = _billDomainHelper.GetBillItem(December2021BillKey, VariableRateTariffKey);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Context.BillItems.AsNoTracking()
                .Should().ContainSingle(b => b.BillId == request.Id && b.TariffId == previousBillItem.TariffId)
                .Which.Rate.Should().Be(previousBillItem.Rate);
        }
    }
}