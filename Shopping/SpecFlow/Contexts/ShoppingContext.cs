namespace Shopping.SpecFlow.Contexts
{
    public sealed class ShoppingContext
    {
        public Dictionary<string, Guid> Kinds { get; } = new Dictionary<string, Guid>();

        public Dictionary<string, Guid> Prodcuts { get; } = new Dictionary<string, Guid>();
    }
}