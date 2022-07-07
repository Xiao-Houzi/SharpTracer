namespace SharpTracer.Model.Base.Messaging
{
    public class ModelArgs
    {
        public EventReason Reason
        {
            get;
        }
        public object DataObject
        {
            get;
        }

        public ModelArgs(EventReason reason, object dataObject)
        {
            Reason = reason;
            DataObject = dataObject;
        }
    }
}