using Shopping.Mediator;
using Shopping.Shared.Models.Common;

namespace Shopping.Shared.Requests
{
    public sealed class AddCarCost : IRequest<Either<Fail, Success>>
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }
}