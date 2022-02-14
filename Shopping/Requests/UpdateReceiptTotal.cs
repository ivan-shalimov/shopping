using MediatR;

namespace Shopping.Requests
{
    public sealed class UpdateReceiptTotal : IRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}