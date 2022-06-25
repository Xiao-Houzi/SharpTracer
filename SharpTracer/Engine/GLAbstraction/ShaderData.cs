namespace SharpTracer.Engine.Graphics
{
	public class ShaderData
	{
		public int ModelMatrix;
		public int ViewlMatrix;
		public int ProjlMatrix;
		public int DataMatrix;

		public uint ProgramID;

		public ShaderData(uint programBuffer)
		{
			ProgramID = programBuffer;
		}
	}
}