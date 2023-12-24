using MediatR;
using Shopping.Shared.Models.Common;

namespace Shopping.Shared.Requests
{
    public sealed class DeleteReceiptItem : IRequest<Success>
    {
        public Guid Id { get; set; }
    }
}