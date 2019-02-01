namespace CSG
{
    using System;
    using Veldrid;

    /// <summary>
    /// Interface for a 3D geometry made up of triangles.
    /// </summary>
    public interface IGeometry
    {
        /// <summary>
        /// Gets the triangle vertices of the target geometry.
        /// </summary>
        /// <param name="vertices">Output vertex buffer</param>
        /// <param name="indices">Output index buffer</param>
        /// <returns>
        /// Returns whether the result contains any triangles.
        /// </returns>
        // bool TryGetTriangles(out Vector3[] vertices, out ushort[] indices);
    }

    public class Geometry : IGeometry
    {
        private Vertex[] vertices;
        private ushort[] indices;
        private uint vertexCount;
        
        private DeviceBuffer vertexBuffer;
        private DeviceBuffer indexBuffer;

        private readonly DrawingContext drawingContext;

        public Geometry(DrawingContext drawingContext,
                        Vertex[] vertices, ushort[] indices) 
        {
            this.drawingContext = drawingContext;

            CreateBuffers((uint)vertices.Length, (uint)indices.Length);
            Update(vertices, indices);
        }

        public void Update(Vertex[] vertices, ushort[] indices)
        {
            this.vertices = vertices;
            this.vertexCount = (uint)vertices.Length;
            this.indices = indices;

            drawingContext.GraphicsDevice.UpdateBuffer(vertexBuffer, 0, vertices);

            if (indexBuffer != null)
            {
                drawingContext.GraphicsDevice.UpdateBuffer(indexBuffer, 0, indices);
            }
        }

        public void Draw(CommandList commandList) 
        {
            commandList.SetVertexBuffer(0, vertexBuffer);

            if (indices.Length > 0) 
            {
                commandList.SetIndexBuffer(indexBuffer, IndexFormat.UInt16);
                commandList.DrawIndexed(vertexCount, 1, 0, 0, 0);
            }
        }

        private void CreateBuffers(uint vertexCount, uint indexCount)
        {
            if (vertexCount <= 0) 
            {
                // throw new RuntimeException($"{vertexCount} must be higher than 1");
            }

            var vBufferDesc = new BufferDescription((uint)(Vertex.SizeInBytes * vertexCount), BufferUsage.VertexBuffer);
            vertexBuffer = drawingContext.ResourceFactory.CreateBuffer(vBufferDesc);

            if (indexCount > 0)
            {
                var iBufferDesc = new BufferDescription(sizeof(ushort) * indexCount, BufferUsage.IndexBuffer);
                indexBuffer = drawingContext.ResourceFactory.CreateBuffer(iBufferDesc);
            }
        }
    }
}
