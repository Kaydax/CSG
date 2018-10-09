﻿namespace CSG
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Runtime.Serialization;

    [DataContract]
    public abstract partial class Shape : IEquatable<Shape>
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Vector4 Color
        {
            get => color;
            set
            {
                if (this.color != value)
                {
                    this.color = value;
                    Invalidate();
                }
            }
        }

        public Vertex[] Vertices => vertices.ToArray();
        public ushort[] Indices => indices.ToArray();

        public ShapeCache Cache => cache ?? (cache = BuildCache()).Value;

        protected int CurrentVertex => ShapeBuilder.CurrentBuilder().Vertices.Count;

        private ShapeCache? cache = null;
        private Vector4 color = Vector4.One;
        private readonly List<Vertex> vertices = new List<Vertex>();
        private readonly List<ushort> indices = new List<ushort>();

        private ShapeCache BuildCache()
        {
            // Make sure that it is clean.
            ShapeBuilder.CurrentBuilder().Clear();

            // Build the shape.
            OnBuild();

            // Generate the shape cache.
            var cache = ShapeBuilder.CurrentBuilder().CreateCache();

            // Clear the currently builder so we dont waste memory.
            ShapeBuilder.CurrentBuilder().Clear();

            return cache;
        }

        /// <summary>
        /// Create polygons of the <see cref="Shape"/>.
        /// </summary>
        public virtual Polygon[] CreatePolygons() => Cache.CreatePolygons();

        /// <summary>
        /// Invalidates the cache. This will force it to rebuild next time it is accessed.
        /// </summary>
        public void Invalidate()
        {
            this.cache = null;
        }

        protected abstract void OnBuild();

        protected void AddVertex(Vertex vertex)
        {
            ShapeBuilder.CurrentBuilder().Vertices.Add(vertex);
        }

        protected void AddVertex(Vector3 position, Vector3 normal)
        {
            var texCoords = new Vector2(
                (float)((Math.Asin(normal.X) / Algorithms.Helpers.Pi) + 0.5),
                (float)((Math.Asin(normal.X) / Algorithms.Helpers.Pi) + 0.5));

            AddVertex(new Vertex(position, normal, texCoords, Vector4.One));
        }

        protected void AddIndex(int index)
        {
            ShapeBuilder.CurrentBuilder().Indices.Add((ushort)index);
        }

        public void Rotate(Quaternion quaternion)
        {
            throw new NotImplementedException();
            //for (int i = 0; i < vertices.Count; i++)
            //{
            //    var vertex = vertices[i];
            //    var newPosition = Vector3.Transform(vertex.Position, quaternion);
            //    vertices[i] = new Vertex(newPosition, vertex.Normal, vertex.TexCoords, vertex.Color);
            //}
        }

        public bool Equals(Shape other)
        {
            return Name == other.Name;
        }
    }
}
