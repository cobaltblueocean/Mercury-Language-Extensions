using System;
using System.Collections.Generic;

namespace System
{
    public static class TypeExtension
    {
        /// <summary>
        /// Get all base type of the Type that given by this parameter
        /// </summary>
        /// <param name="self"></param>
        /// <returns>All Type</returns>
        public static IEnumerable<Type> GetBaseTypes(this Type self)
        {
            for (var baseType = self.BaseType; null != baseType; baseType = baseType.BaseType)
            {
                yield return baseType;
            }
        }

        public static Boolean  IsNullable(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return true;
            else
                return false;
        }
    }
}
