﻿namespace CSG.Viewer
{
    using CSG.Shapes;
    using CSG.Viewer.Framework;
    using CSG.Viewer.Framework.Materials;
    using System.Numerics;
    using Veldrid;

    class App : Application
    {
        private CommandList commandList;
        private BasicMaterial basicMaterial;
        private ShapeGeometry shapeGeometry;

        private float _ticks;

        protected override void CreateResources(ResourceFactory factory)
        {
            var texture = TextureLoader.Load("v:checker").GetAwaiter().GetResult();
            basicMaterial = new BasicMaterial(DrawingContext, texture);

            // var shape1 = new Cube(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
            // var shape2 = new Cube(new Vector3(0.8f, 0.8f, 0), new Vector3(1, 1, 1));
            // var shape = shape1.Do(ShapeOperation.Intersect, shape2);

            var shape = new Teapot();

            shapeGeometry = new ShapeGeometry(DrawingContext, shape);

            commandList = factory.CreateCommandList();
        }

        protected override void Draw(float deltaSeconds)
        {
            _ticks += deltaSeconds * 1000f;
            commandList.Begin();

            basicMaterial.Projection = Matrix4x4.CreatePerspectiveFieldOfView(1.0f, HostAspectRatio, 0.5f, 100f);
            basicMaterial.View = Matrix4x4.CreateLookAt(Vector3.UnitZ * 2.5f, Vector3.Zero, Vector3.UnitY);
            basicMaterial.World = Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, (_ticks / 1000f)) *
                                  Matrix4x4.CreateFromAxisAngle(Vector3.UnitX, (_ticks / 3000f));

            commandList.SetFramebuffer(DrawingContext.MainSwapchain.Framebuffer);
            commandList.ClearColorTarget(0, RgbaFloat.CornflowerBlue);
            commandList.ClearDepthStencil(1f);

            basicMaterial.Apply(commandList);
            shapeGeometry.Draw(commandList);

            commandList.End();

            DrawingContext.GraphicsDevice.SubmitCommands(commandList);
            DrawingContext.GraphicsDevice.SwapBuffers(DrawingContext.MainSwapchain);
            DrawingContext.GraphicsDevice.WaitForIdle();
        }
    }
}