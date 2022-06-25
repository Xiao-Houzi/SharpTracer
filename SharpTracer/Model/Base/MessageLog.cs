using SharpTracer;
using System.Collections.Generic;


namespace SharpTracer.Base
{
    public class MessageLog
    {
        public List<ErrorLogItem> LogItems { get; set; }

        public MessageLog()
        {
            LogItems = new List<ErrorLogItem>();
        }
    }
}
