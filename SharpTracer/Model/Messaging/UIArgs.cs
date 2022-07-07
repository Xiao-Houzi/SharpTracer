namespace SharpTracer.Model.Base.Messaging
{
    public class UIArgs
    {

        public EventReason Reason
        {
            get;
        }
        public object DataObject
        {
            get;
        }

        public UIArgs(EventReason reason, object dataObject)
        {
            Reason = reason;
            DataObject = dataObject;
        }
    }
}