using System;
using System.Collections.Generic;
using System.Linq;
using MvcContrib.FluentHtml.Attributes;
using MvcContrib.FluentHtml.Behaviors;

namespace MvcContrib.FluentHtml.Binders
{
    /// <summary>
    /// This cache is used to both improve performance of the derived type model binder
    /// on cases where a binding type has already been identified.
    /// </summary>
    public static class DerivedTypeModelBinderCache
    {
        private static ThreadSafeDictionary<Type, IEnumerable<Type>> typeCache =
            new ThreadSafeDictionary<Type, IEnumerable<Type>>();

        /// <summary>
        /// Registers the attached set of derived types by the indicated base type
        /// </summary>
        /// <param name="baseType">base type that will be encountered by the binder where an alternate value should be used</param>
        /// <param name="derivedTypes">an enumerable set of types to be considered for binding</param>
        public static bool RegisterDerivedTypes(Type baseType, IEnumerable<Type> derivedTypes)
        {
            try
            {
                typeCache.Add(baseType, derivedTypes);

                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        /// <summary>
        /// Searches for the requested base type in the cache
        /// </summary>
        /// <param name="baseType">type used to located the set of registered alternate types</param>
        /// <returns>the set of matching alternate types or null when a set is not found</returns>
        public static IEnumerable<Type> GetDerivedTypes(Type baseType)
        {
            if( typeCache.ContainsKey(baseType))
                    return typeCache[baseType];

            // next we'll search for the derived type aware attributes on the type.
            // letting go of the lock as this operation can be longer lived

            var attributes =
                baseType.GetCustomAttributes(typeof(DerivedTypeBinderAwareAttribute), true) as
                DerivedTypeBinderAwareAttribute[];

            var types = from a in attributes select a.DerivedType;

            RegisterDerivedTypes(baseType, types);

            return types;
        }

        /// <summary>
        /// removes all items from the cache
        /// </summary>
        public static void Reset()
        {
            typeCache.Clear();
        }
    }
}
