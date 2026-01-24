using Shopping.Mediator;
using Shopping.Shared.Models.Common;
using System.Text.Json.Serialization;

namespace Shopping.Shared.Requests.Bills
{
    public sealed class UpdateBillItemRate : IRequest<Either<Fail, Success>>
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid BillId { get; set; }

        public decimal Rate { get; set; }
    }
}