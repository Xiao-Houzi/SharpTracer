namespace SharpTracer.Model.Events
{
	public class SharpTracerUIArgs
	{
		public enum EventReason
		{
			CommandCloseProject,
			CommandAbout,
			CommandTextChanged,
			SearchTextChanged,
			CommandSearch,

			SelectedFileChanged,
			CommandCloseApp,
			CommandSettings,
			CommandCleanupProject,
			CommandExportProject,
			CommandOpenProjectPTX,
			CommandClear,
			NoSelectedProjectFile,
			UpdateAxes,
			AdjustmentMatrixChanged,
			CommandOpenProjectBinary,
			CommandRender,
			ConvertDI,
			EntitySelected,
			AdjustmentAdjustChanged,
		}
		public EventReason Reason
		{
			get;
		}
		public object DataObject
		{
			get;
		}

		public SharpTracerUIArgs( EventReason reason, object dataObject )
		{
			Reason = reason;
			DataObject = dataObject;
		}
	}
}