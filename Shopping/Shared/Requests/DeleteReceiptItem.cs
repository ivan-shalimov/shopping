using MediatR;

namespace Shopping.Shared.Requests
{
    public sealed class DeleteReceiptItem : IRequest
    {
        public Guid Id { get; set; }
    }
}