using MediatR;

namespace Shopping.Requests
{
    public sealed class DeleteReceiptItem : IRequest
    {
        public Guid Id { get; set; }
    }
}