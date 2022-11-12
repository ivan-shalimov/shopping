using MediatR;
using Shopping.Shared.Models.Common;

namespace Shopping.Shared.Requests
{
    public sealed class UpdateReceipt : IRequest<Either<Fail, Success>>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; }
    }
}