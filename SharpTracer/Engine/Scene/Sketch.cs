using SharpEngine.Engine.Graphics;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTracer.Engine.Scene
{
    public class Sketch
    {
        public Entity Parent
        { get; set; }

        List<IDrawInstruction> _drawingInstructions;

        public Sketch()
        {
            _drawingInstructions = new List<IDrawInstruction>();
        }

        public virtual void Render(OpenGL gl)
        {
            //  Reset the modelview Matrix44.
            gl.LoadIdentity();

            //  Move the geometry into a fairly central position.
            gl.Translate(Parent.Transform.Translation.x, Parent.Transform.Translation.y, Parent.Transform.Translation.z);

            //  Draw a pyramid. First, rotate the modelview Matrix44.
            gl.Rotate(Parent.Transform.Orientation.x, Parent.Transform.Orientation.y, Parent.Transform.Orientation.z);

            gl.Scale(Parent.Transform.Scale.x, Parent.Transform.Scale.y, Parent.Transform.Scale.z);

            foreach (IDrawInstruction instruction in _drawingInstructions)
                instruction.Execute(gl);

            gl.Flush();
        }

        public void AddInstruction(IDrawInstruction instruction)
        {
            _drawingInstructions.Add(instruction);
        }

        void Initialise()
        {

        }
    }
}
