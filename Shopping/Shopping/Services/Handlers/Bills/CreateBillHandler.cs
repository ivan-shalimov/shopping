using Shopping.Mediator;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Models.Domain;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests.Bills;

namespace Shopping.Services.Handlers.CarCosts
{
    public sealed class CreateBillHandler : IRequestHandler<CreateBill, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;

        public CreateBillHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, Success>> Handle(CreateBill request, CancellationToken cancellationToken)
        {
            var bill = new Bill
            {
                Id = Guid.NewGuid(),
                Description = request.Description,
                CreatedOn = new DateTime(request.Year, request.Month, 1),
            };
            var previousBill = await _context.Bills
                .Where(e => e.CreatedOn < bill.CreatedOn)
                .OrderByDescending(bill => bill.CreatedOn)
                .FirstOrDefaultAsync();

            var previousValuesByTariff = previousBill == null ? new Dictionary<Guid, int>()
                : await _context.BillItems
                .Where(e => e.BillId == previousBill.Id)
                .ToDictionaryAsync(e => e.TariffId, e => e.PreviousValue + e.Quantity, cancellationToken);

            var billItems = new List<BillItem>();

            var tariffs = await _context.Tariffs
                .Where(e => e.StartOn < bill.CreatedOn && (!e.EndOn.HasValue || bill.CreatedOn > e.EndOn))
                .Select(e => new { TariffId = e.Id, e.Quantifiable })
                .ToArrayAsync(cancellationToken: cancellationToken);

            foreach (var tariff in tariffs)
            {
                var item = new BillItem
                {
                    Id = Guid.NewGuid(),
                    BillId = bill.Id,
                    TariffId = tariff.TariffId,
                    Quantity = tariff.Quantifiable ? 0 : 1,
                    PreviousValue = previousValuesByTariff.TryGetValue(tariff.TariffId, out var previousValue) ? previousValue : 0,
                };
                billItems.Add(item);
            }

            _context.Add(bill);
            _context.AddRange(billItems);
            await _context.SaveChangesAsync();

            return Success.Instance;
        }
    }
}