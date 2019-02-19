﻿namespace CSG.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.Serialization;

    public static class SerializerHelper
    {
        public static IEnumerable<Type> DependencyTypes()
        {
            return new Type[] {
                typeof(System.Numerics.Vector2),
                typeof(System.Numerics.Vector3),
                typeof(System.Numerics.Vector4),
            };
        }

        public static IEnumerable<Type> GetShapeTypes(Assembly assembly = null)
        {
            return (assembly ?? ThisAssembly()).FindDerivedTypes<Shape>();
        }

        public static IEnumerable<Type> GetSerializableTypes(Assembly assembly = null)
        {
            return (assembly ?? ThisAssembly()).FindByAttribute<SerializableAttribute>();
        }

        internal static Assembly ThisAssembly() => Assembly.GetAssembly(typeof(Shape));
    }
}
