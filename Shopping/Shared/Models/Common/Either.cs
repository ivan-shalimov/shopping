namespace Shopping.Shared.Models.Common
{
    public sealed class Either<L, R>
    {
        private readonly R Right;
        private readonly L Left;

        public bool IsRight { get; }

        private Either(L left, R right, bool isRight)
        {
            Left = left;
            Right = right;
            IsRight = isRight;
        }

        public static implicit operator Either<L, R>(L value)
        {
            return new Either<L, R>(value, default, false);
        }

        public static implicit operator Either<L, R>(R value)
        {
            return new Either<L, R>(default, value, true);
        }

        public Either<TNewLeft, R> MapLeft<TNewLeft>(Func<L, TNewLeft> mapping) => new Either<TNewLeft, R>(IsRight ? default : mapping(Left), Right, IsRight);

        public Either<L, TNewRight> MapRight<TNewRight>(Func<R, TNewRight> mapping) => new Either<L, TNewRight>(Left, IsRight ? mapping(Right) : default, IsRight);

        public L ReduceTo(Func<R, L> mapping) => IsRight ? mapping(Right) : Left;
    }
}