using Shopping.Shared.Models.Common;

namespace Shopping.Server.Common
{
    public static class EitherExtensions
    {
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
                .ReduceTo(r => r);
        }
    }
}