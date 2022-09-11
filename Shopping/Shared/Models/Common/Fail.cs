namespace Shopping.Shared.Models.Common
{
    public sealed class Fail
    {
        public FailType FailType { get; }
        public IEnumerable<string> ErrorMessages { get; }

        public Fail(FailType failType)
        {
            FailType = failType;
            ErrorMessages = new string[] { "Entity is not found." };
        }

        public Fail(FailType failType, IEnumerable<string> errorMessages)
        {
            FailType = failType;
            ErrorMessages = errorMessages.ToArray();
        }
    }
}