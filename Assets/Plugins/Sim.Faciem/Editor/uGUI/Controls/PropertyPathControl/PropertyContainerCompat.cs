using System;
using System.Collections.Generic;
using Unity.Properties;

namespace Sim.Faciem.uGUI.Editor.Controls
{
    internal static class PropertyContainerCompat
    {
        private static readonly PropertyPathVisitor s_propertyPathVisitor = new();
        
        public static IEnumerable<IProperty> GetProperties(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var bag = PropertyBag.GetPropertyBag(type);
            if (bag == null)
                yield break;

            var list = new List<IProperty>();
            s_propertyPathVisitor.Properties = list;
            bag.Accept(s_propertyPathVisitor);

            foreach (var property in list)
                yield return property;
        }
        
        public static bool HasProperties(Type type)
        {
            if (type == null) return false;

            // Get the property bag for the type
            var bag = PropertyBag.GetPropertyBag(type);
            if (bag == null)
                return false;

            // Use a visitor to check if the bag has any properties
            var list = new List<IProperty>();
            s_propertyPathVisitor.Properties = list;
            bag.Accept(s_propertyPathVisitor);
            
            var hasAny = list.Count > 0;
            return hasAny;
        }
    }
}