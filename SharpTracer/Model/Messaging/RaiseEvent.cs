using System;
using System.Runtime.CompilerServices;
using static SharpTracer.Model.Base.Messaging.LogArgs;

namespace SharpTracer.Model.Base.Messaging
{
    public class RaiseEvent
    {
        private static UInt64 _ID = 0;
        private static object _IDLock = new object();
        public object Sender { get; }
        public object Args { get; }
        public static UInt64 ID { get; private set; }

        protected RaiseEvent(object sender, object args)
        {
            lock (_IDLock)
            {
                ID = _ID++;
            }
            Sender = sender;
            Args = args;
        }

        public static void UI(object sender, EventReason eventReason, object dataObject)
        {
#pragma warning disable CS0618 // Type or member is obsolete
			Messenger.Send(new RaiseEvent(sender, new UIArgs(eventReason, dataObject)));
#pragma warning restore CS0618 // Type or member is obsolete
		}

        public static void Model(object sender, EventReason eventReason, object dataObject)
        {
#pragma warning disable CS0618 // Type or member is obsolete
			Messenger.Send(new RaiseEvent(sender, new ModelArgs(eventReason, dataObject)));
#pragma warning restore CS0618 // Type or member is obsolete
		}



        public static void Network(object sender, string reason, string message)
        {
            Messenger.Send(new RaiseEvent(sender, new NetworkArgs(reason, message)));
        }

        public static void Progress(object sender, int progressID, string message, Status status, int max = 0, string caption = "")
        {
            Messenger.Send(new RaiseEvent(sender, new ProgressArgs(progressID, ID, caption, message, status, max)));
        }

        public static void Log(object sender, DebugLevel level, string message, Exception exception = null)
        {
            Messenger.Send(new RaiseEvent(sender, new LogArgs(message, exception, level, LogDestination.Console)));
        }

        public static void Console(object sender, string message, Exception exception = null)
        {
            Messenger.Send(new RaiseEvent(sender, new LogArgs(message, exception, DebugLevel.Information, LogDestination.Console)));
        }

        public static void Exception(object sender, DebugLevel level, string message, Exception exception = null,
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string caller = "",
            [CallerFilePath] string callingFilePath = "")
        {
            message = string.Format("ERROR::\t in file {0} in function {1} at line {2}\n\t Message: {3}\n",
                    callingFilePath, caller, lineNumber, message);
            Messenger.Send(new RaiseEvent(sender, new LogArgs(message, exception, level, LogDestination.Console)));
        }

    }
}