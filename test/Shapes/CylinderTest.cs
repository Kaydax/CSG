﻿namespace CSG.Shapes
{
    using System.Numerics;
    using Xunit;

    public class CylinderTest
    {
        [Theory]
        [InlineData(ShapeOperation.Intersect)]
        [InlineData(ShapeOperation.Subtract)]
        [InlineData(ShapeOperation.Union)]
        public void ShapeOperations(ShapeOperation operation)
        {
            var shape1 = new Cylinder(new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            var shape2 = new Cylinder(new Vector3(1, 1, 0), new Vector3(0, 1, 0));
            var result = shape1.Do(operation, shape2);

            Assert.True(result.Cache.Vertices.Length > 0);
            Assert.True(result.Cache.Indices.Length > 0);
        }

        [Theory]
        [InlineData(ShapeOperation.Intersect)]
        [InlineData(ShapeOperation.Subtract)]
        [InlineData(ShapeOperation.Union)]
        public void ShapeOperationsOverlapping(ShapeOperation operation)
        {
            var shape1 = new Cylinder(new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            var shape2 = new Cylinder(new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            var result = shape1.Do(operation, shape2);

            Assert.True(result.Cache.Vertices.Length > 0);
            Assert.True(result.Cache.Indices.Length > 0);
        }
    }
}

