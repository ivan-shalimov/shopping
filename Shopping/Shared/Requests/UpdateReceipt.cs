using MediatR;

namespace Shopping.Shared.Requests
{
    public sealed class UpdateReceipt : IRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; }
    }
}