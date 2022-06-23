


namespace SharpTracer.Base.Messaging
{
    public class NetworkArgs
    {
        public string Reason { get; }
        public string Message { get; }

        public NetworkArgs(string reason, string message)
        {
            Reason = reason;
            Message = message;
        }
    }
}