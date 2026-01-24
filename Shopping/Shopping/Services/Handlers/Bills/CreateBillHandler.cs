using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Mediator;
using Shopping.Models.Domain;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests.Bills;

namespace Shopping.Services.Handlers.Bills
{
    public sealed class CreateBillHandler : IRequestHandler<CreateBill, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;
        private const int DEFAULT_QUANTITY_OF_NOT_QUANTIFIABLE = 1;
        private const int DEFAULT_PREVIOUS_VALUE_OF_NOT_QUANTIFIABLE = 1;

        public CreateBillHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, Success>> Handle(CreateBill request, CancellationToken cancellationToken)
        {
            var bill = new Bill
            {
                Id = request.Id,
                Description = request.Description,
                CreatedOn = new DateTime(request.Year, request.Month, 1),
            };
            var previousBill = await _context.Bills
                .Where(e => e.CreatedOn < bill.CreatedOn)
                .OrderByDescending(b => b.CreatedOn)
                .FirstOrDefaultAsync(cancellationToken);

            var previousBillDataByTariff = previousBill == null ? new Dictionary<Guid, (int PreviousValue, decimal Rate)>()
                : await _context.BillItems
                .Where(e => e.BillId == previousBill.Id)
                .ToDictionaryAsync(e => e.TariffId, e => (PreviousValue: e.PreviousValue + e.Quantity, e.Rate), cancellationToken);

            var billItems = new List<BillItem>();

            var tariffs = await _context.TariffPeriods
                .Where(e => e.StartOn <= bill.CreatedOn && (!e.EndOn.HasValue || bill.CreatedOn < e.EndOn))
                .Select(e => new { e.Id, TariffId = e.TariffId, e.Rate, e.Tariff.Quantifiable, e.Tariff.RateType })
                .ToArrayAsync(cancellationToken: cancellationToken);

            foreach (var tariff in tariffs)
            {
                var previousBillData = previousBillDataByTariff.TryGetValue(tariff.TariffId, out var v) ? v : (PreviousValue: 0, Rate: 0m);
                var item = new BillItem
                {
                    Id = Guid.NewGuid(),
                    BillId = bill.Id,
                    TariffId = tariff.TariffId,
                    Rate = tariff.RateType == RateType.Constant ? tariff.Rate : previousBillData.Rate,
                    Quantity = tariff.Quantifiable ? 0 : DEFAULT_QUANTITY_OF_NOT_QUANTIFIABLE,
                    PreviousValue = tariff.Quantifiable ? previousBillData.PreviousValue : DEFAULT_PREVIOUS_VALUE_OF_NOT_QUANTIFIABLE,
                };
                billItems.Add(item);
                bill.Total += (int)(item.Rate * item.Quantity * 100);
            }

            _context.Add(bill);
            _context.AddRange(billItems);
            await _context.SaveChangesAsync();

            return Success.Instance;
        }
    }
}