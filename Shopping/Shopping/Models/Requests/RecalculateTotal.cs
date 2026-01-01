using Shopping.Mediator;
using Shopping.Shared.Models.Common;

namespace Shopping.Models.Requests
{
    public sealed class RecalculateTotal : IRequest<Either<Fail, Success>>
    {
        public string Type { get; set; }

        public int Year { get; set; }
    }
}