namespace Shopping.Shared.Models.Common
{
    public sealed record Success
    {
        public static readonly Success Instance = new Success();

        private Success()
        { }
    }
}