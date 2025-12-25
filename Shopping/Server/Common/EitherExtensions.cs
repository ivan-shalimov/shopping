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

        public static IActionResult ReduceToAction<T>(this Either<Fail, T> either)
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

        public static IResult Reduce<T>(this Either<Fail, T> either)
        {
            return either
                .MapLeft(f =>
                {
                    switch (f.FailType)
                    {
                        case FailType.Validation:
                            return Results.BadRequest(f.ErrorMessages);

                        case FailType.None:
                        default:
                            return Results.BadRequest("Something go wrong.");
                    }
                })
                .MapRight(r => Results.Ok(r))
                .Reduce();
        }
    }
}