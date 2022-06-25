using SharpTracer.Base.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTracer.Model.Events
{
    class SharpMessenger : Messenger
    {
        public new delegate void UIMessage( object sender, SharpTracerUIArgs args );
        public new delegate void ModelMessage( object sender, SharpTracerModelArgs args );

        public new static event UIMessage UIEvent;
        public new static event ModelMessage ModelEvent;

        public override void SendMessage( RaiseEvent m )
        {
            base.SendMessage( m );
            string type = m.Args.GetType().Name;

            switch (type)
            {
                case nameof( SharpTracerUIArgs ):
                    UIEvent?.Invoke( m.Sender, (SharpTracerUIArgs)m.Args );
                    break;

                case nameof( SharpTracerModelArgs ):
                    ModelEvent?.Invoke( m.Sender, (SharpTracerModelArgs )m.Args );
                    break;
            }
        }
    }
}
