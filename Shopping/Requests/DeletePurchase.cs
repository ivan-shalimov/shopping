using MediatR;

namespace Shopping.Requests
{
    public sealed class DeletePurchase : IRequest
    {
        public Guid Id { get; set; }
    }
}