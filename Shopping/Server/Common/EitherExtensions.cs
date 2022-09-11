using Microsoft.AspNetCore.Mvc;
using Shopping.Shared.Models.Common;

namespace Shopping.Server.Common
{
    public static class EitherExtensions
    {
        public static T Reduce<T>(this Either<T, T> either)
        {
            return either.ReduceTo(r => r);
        }

        public static IActionResult Reduce<T>(this Either<Fail, T> either)
        {
            return either
                .MapLeft<IActionResult>(f =>
                {
                    switch (f.FailType)
                    {
                        case FailType.Validation:
                            return new BadRequestObjectResult(f.ErrorMessages);

                        case FailType.None:
                        default:
                            return new BadRequestObjectResult("Something go wrong.");
                    }
                })
                .MapRight<IActionResult>(r => new OkObjectResult(r))
                .Reduce();
        }
    }
}