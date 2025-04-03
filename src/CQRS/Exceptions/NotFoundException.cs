namespace CQRS.Exceptions
{
    internal class NotFoundException : Exception
    {
        public NotFoundException(Type requestType)
            : base($"Handler not found for {requestType.FullName}")
        {
            RequestType = requestType;
        }

        public Type RequestType { get; }
    }
}
