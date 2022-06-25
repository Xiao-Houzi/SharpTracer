using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharpTracer.Base.Messaging
{
    public class Messenger
    {
        private bool _running = true;
        private int _processDeliveryInterval = 50;
        private static List<RaiseEvent> _messages = new List<RaiseEvent>();

        public delegate void LogMessage(object sender, LogArgs args);
        public delegate void NetworkMessage(object sender, NetworkArgs args);
#pragma warning disable CS0618 // Type or member is obsolete
		public delegate void UIMessage(object sender,UIArgs args);
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning disable CS0618 // Type or member is obsolete
		public delegate void ModelMessage(object sender, ModelArgs args);
#pragma warning restore CS0618 // Type or member is obsolete
		public delegate void DataOperationMessage(object sender, DataOperationArgs args);
		public delegate void ProgressMessage(object sender, ProgressArgs args);

		public static event LogMessage LogEvent;
        public static event NetworkMessage NetworkEvent;
        public static event UIMessage UIEvent;
        public static event ModelMessage ModelEvent;
        public static event DataOperationMessage DataOperationEvent;
		public static event ProgressMessage ProgressEvent;


        public Messenger()
        {
            Process();
        }

        public static void Send(RaiseEvent message)
        {
			lock (_messages) { _messages.Add(message);}
        }

        public void Process()
        {
            Task.Run(() =>
            {
                while (_running)
                {
                    var deliverTask = new Task(Deliver);
                    deliverTask.Start();
                    deliverTask.Wait();
                    Thread.Sleep(_processDeliveryInterval);
                }
            });
        }

        private void Deliver()
        {
            lock (_messages)
            {
                _processDeliveryInterval = 50 / (_messages.Count+1);
                var messages = (from x in _messages select x).ToList();
				foreach (RaiseEvent m in messages)
                {
                    _messages.Remove(m);
                    Task.Run(() =>SendMessage(m));
                }
            }
        }

        public virtual void SendMessage(RaiseEvent m)
        {
            string type = m.Args.GetType().Name;

			switch (type)
            {
                case nameof(LogArgs):
                    LogEvent?.Invoke(m.Sender, (LogArgs)m.Args);
                    break;

                case nameof(NetworkArgs):
                    NetworkEvent?.Invoke(m.Sender, (NetworkArgs)m.Args);
					break;

#pragma warning disable CS0618 // Type or member is obsolete
				case nameof(UIArgs):
					UIEvent?.Invoke(m.Sender, (UIArgs)m.Args);
					break;
#pragma warning restore CS0618 // Type or member is obsolete

#pragma warning disable CS0618 // Type or member is obsolete
				case nameof(ModelArgs):
					ModelEvent?.Invoke(m.Sender, (ModelArgs)m.Args);
					break;
#pragma warning restore CS0618 // Type or member is obsolete

                case nameof(ProgressArgs):
                    ProgressEvent?.Invoke(m.Sender, (ProgressArgs)m.Args);
					break;

				case nameof(DataOperationArgs):
					DataOperationEvent?.Invoke(m.Sender, (DataOperationArgs)m.Args);
					break;
			}
        }


        public void Start()
        {
            _running = true;
        }

        public void Stop()
        {
            _running = false;
        }
    }
}
