using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTracer.Model.Base.Messaging
{
    public enum EventReason
    {
        CommandAbout,
        CommandSettings,
        CommandCloseApp,
        CommandSearch,
        SearchTextChanged,

        CommandOpenProject,
        CommandCloseProject,
        CommandSaveProject,
        CommandRender,
        CommandResetViewCamera,
        CommandClear,

        RenderStarted,
        RenderUpdated,
        RenderEnded,

        EntitySelected,
        AddedCamera,
        AddedLight,
        AddedEntity,
        RemovedCamera,
        RemovedLight,
        RemovedEntity,
        ChangedTransform,
        ChangedMaterial,
        ChangedGeometry,
        ChangedViewCamera,
        GLIsNull,
        GLAcquired,
        AcquireGL,
        ProjectLoaded,
        SafeToClose,
        SaveProject,
        LoadProject,
        ResetViewCamera,
        RendererInitialised,
        CommandAddEntity,
    }
    internal class Event : RaiseEvent
    {
        protected Event(object sender, object args) : base(sender, args) { }

        public static void UI(object sender, EventReason eventReason, object dataObject)
        {
            Messenger.Send(new Event(sender, new UIArgs(eventReason, dataObject)));
        }

        public static void Model(object sender, EventReason eventReason, object dataObject)
        {
            Messenger.Send(new Event(sender, new ModelArgs(eventReason, dataObject)));
        }
    }
}
