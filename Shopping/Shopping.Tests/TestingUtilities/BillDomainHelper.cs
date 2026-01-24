using Shopping.DataAccess;
using Shopping.Models.Domain;

namespace Shopping.Tests.TestingUtilities
{
    internal sealed class BillDomainHelper
    {
        private static int _incrementingId = 1;

        private readonly Dictionary<string, Tariff> _tariffs = new();
        private readonly Dictionary<string, TariffPeriod> _tariffPeriods = new();

        private readonly Dictionary<string, Bill> _bills = new();
        private readonly Dictionary<string, BillItem> _billItems = new();

        internal Tariff GetTariff(string identifier) => _tariffs[identifier];

        internal TariffPeriod GetTariffPeriod(string tariffidentifier, string identifier) => _tariffPeriods[$"{tariffidentifier}_{identifier}"];

        internal Bill GetBill(string identifier) => _bills[identifier];

        internal BillItem GetBillItem(string billidentifier, string tariffIdentifier) => _billItems[$"{billidentifier}_{tariffIdentifier}"];

        internal void Seed(ShoppingDbContext context)
        {
            context.Tariffs.AddRange(_tariffs.Values);
            context.TariffPeriods.AddRange(_tariffPeriods.Values);
            context.Bills.AddRange(_bills.Values);
            context.BillItems.AddRange(_billItems.Values);
            context.SaveChanges();
        }

        internal Tariff AddQuantifiableTariff(string identifier)
        {
            _tariffs[identifier] = new Tariff
            {
                Id = (++_incrementingId).ToGuid(),
                GroupName = identifier,
                Description = $"{identifier} Description",
                RateType = RateType.Constant,
                Quantifiable = true,
            };

            return _tariffs[identifier];
        }

        internal Tariff AddNotQuantifiableTariff(string identifier)
        {
            _tariffs[identifier] = new Tariff
            {
                Id = (++_incrementingId).ToGuid(),
                GroupName = identifier,
                Description = $"{identifier} Description",
                RateType = RateType.Constant,
                Quantifiable = false,
            };

            return _tariffs[identifier];
        }

        internal Tariff AddVariableTariff(string identifier, DateTime startDate)
        {
            _tariffs[identifier] = new Tariff
            {
                Id = (++_incrementingId).ToGuid(),
                GroupName = identifier,
                Description = $"{identifier} Description",
                RateType = RateType.Variable,
                Quantifiable = false,
            };
            AddTariffPeriod(identifier, identifier + "_start", startDate);

            return _tariffs[identifier];
        }

        internal TariffPeriod AddTariffPeriod(string tariffidentifier, string identifier, DateTime startDate, DateTime? endDate = null)
        {
            _tariffPeriods[$"{tariffidentifier}_{identifier}"] = new TariffPeriod
            {
                Id = (++_incrementingId).ToGuid(),
                TariffId = _tariffs[tariffidentifier].Id,
                Tariff = _tariffs[tariffidentifier],
                Rate = _incrementingId / 100m,
                StartOn = startDate,
                EndOn = endDate
            };

            return _tariffPeriods[$"{tariffidentifier}_{identifier}"];
        }

        internal Bill AddBill(string identifier, DateTime billedOn)
        {
            _bills[identifier] = new Bill
            {
                Id = (++_incrementingId).ToGuid(),
                CreatedOn = billedOn,
                Description = $"{identifier} Description",
            };

            return _bills[identifier];
        }

        internal BillItem AddQuantifiableBillItem(string billidentifier, string tariffIdentifier, int quantity)
        {
            var tarifid = _tariffs[tariffIdentifier].Id;
            var billDate = _bills[billidentifier].CreatedOn;

            var previousBillItem = _billItems
                .Select(v => v.Value)
                .Where(bi => bi.TariffId == tarifid)
                .OrderByDescending(bi => _bills.Values.First(b => b.Id == bi.BillId).CreatedOn)
                .FirstOrDefault(bi => _bills.Values.First(b => b.Id == bi.BillId).CreatedOn < billDate);
            var previousValue = previousBillItem == null ? 0 : previousBillItem.PreviousValue + previousBillItem.Quantity;

            var tarifPeriod = _tariffPeriods
                .Select(v => v.Value)
                .First(tp => tp.TariffId == tarifid && tp.StartOn <= billDate && (!tp.EndOn.HasValue || billDate <= tp.EndOn));

            return AddBillItem(billidentifier, tariffIdentifier, tarifPeriod.Rate, previousValue, quantity);
        }

        internal BillItem AddNotQuantifiableBillItem(string billidentifier, string tariffIdentifier)
        {
            var tarifid = _tariffs[tariffIdentifier].Id;
            var billDate = _bills[billidentifier].CreatedOn;

            var tarifPeriod = _tariffPeriods
                .Select(v => v.Value)
                .First(tp => tp.TariffId == tarifid && tp.StartOn <= billDate && (!tp.EndOn.HasValue || billDate <= tp.EndOn));

            return AddBillItem(billidentifier, tariffIdentifier, tarifPeriod.Rate, 1, 1);
        }

        internal BillItem AddVariableBillItem(string billidentifier, string tariffIdentifier, decimal rate)
        {
            return AddBillItem(billidentifier, tariffIdentifier, rate, 1, 1);
        }

        private BillItem AddBillItem(string billidentifier, string tariffIdentifier, decimal rate, int previousValue, int quantity)
        {
            var tarifid = _tariffs[tariffIdentifier].Id;

            var identifier = $"{billidentifier}_{tariffIdentifier}";
            _billItems[identifier] = new BillItem
            {
                Id = (++_incrementingId).ToGuid(),
                BillId = _bills[billidentifier].Id,
                TariffId = tarifid,
                Rate = rate,
                PreviousValue = previousValue,
                Quantity = quantity,
            };

            return _billItems[identifier];
        }
    }
}