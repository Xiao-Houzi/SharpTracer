using SharpTracer.ViewModels;
using System.Linq;
using System.Collections.Generic;
using System;
using SharpEngine.Engine;
using SharpEngine.Engine.Graphics;
using GlmSharp;
using SharpTracer.RendererScripting.States;

namespace SharpTracer.RendererScripting
{
    public class ViewRenderer : Renderer
    {
        public MainWindowVM VM
        {
            get;
            private set;
        }

        private SharpTracerModel _model;

        public ViewRenderer(SharpTracerModel model)
        {
            _model = model;
            LoadShader("Default");
            LoadShader("Texture");
            LoadShader("Hex");
        }

        public override void RendererUpdate()
        {
            foreach (Layer layer in _stateMachine.CurrentState.Layers.Values)
            {
                foreach (Entity entity in layer.Entities)
                {

                    if (entity == SharpTracerModel.SelectedEntity)
                    {
                        //entity.Orientation = new quat(0, 0, 0, 1);
                        //entity.Translation = new vec3(0);
                    }

                }
            }
            if (_stateMachine.CurrentState.Camera.IsChanged)
                Update();
        }

        public override void Update()
        {
            VM.UpdateControls();
            base.Update();
        }

        internal void SetVM(MainWindowVM mainWindowVM)
        {
            VM = mainWindowVM;
        }

        internal bool HasState(Type type)
        {
            return _stateMachine.States.ContainsKey(type);
        }
    }
}