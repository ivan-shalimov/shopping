using MediatR;

namespace Shopping.Shared.Requests
{
    public sealed class UpdateReceiptTotal : IRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}