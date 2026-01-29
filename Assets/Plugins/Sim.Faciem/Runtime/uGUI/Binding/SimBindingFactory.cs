using R3;
using Unity.Properties;
using UnityEngine;

namespace Sim.Faciem.uGUI.Binding
{
    internal static class SimBindingFactory
    {
        private static readonly SimObservablePropertyPathVisitor s_observablePathVisitor = new();
        private static readonly SimPropertyPathVisitor s_propertyPathVisitor = new();
        
        public static SimDataBinding<T> CreateBinding<T>(SimBindingInfo bindingInfo)
        {
            if (bindingInfo.BindingType == BindingType.BindToUI)
            {
                var propertyPath = bindingInfo.PropertyPath;

                var propertyPathParts = propertyPath.ExtractPaths();

                Observable<object> propertyChanges = null;
                
                foreach (var simPropertyPathPart in propertyPathParts)
                {
                    if (simPropertyPathPart.Kind == SimPropertyPathPartKind.Observable)
                    {
                        if (propertyChanges == null)
                        {
                            s_observablePathVisitor.Path = simPropertyPathPart.Path;
                            if (!PropertyContainer.TryAccept(s_observablePathVisitor, ref bindingInfo.DataSource))
                            {
                                Debug.LogError("Could not evaluate observable property path");
                                return null;
                            }
                            
                            propertyChanges = s_observablePathVisitor.PropertyObservable;
                        }
                        else
                        {
                            propertyChanges = propertyChanges
                                .Select(subDataSource =>
                                {
                                    s_observablePathVisitor.Path = simPropertyPathPart.Path;
                                    if (!PropertyContainer.TryAccept(s_observablePathVisitor, ref subDataSource))
                                    {
                                        Debug.LogError("Could not evaluate sub-dataContext-property path");
                                        return Observable.Never<object>();
                                    }

                                    return s_observablePathVisitor.PropertyObservable;
                                })
                                .Switch();
                        }
                    }

                    if (simPropertyPathPart.Kind == SimPropertyPathPartKind.Property)
                    {
                        if (simPropertyPathPart.Path.IsEmpty)
                        {
                            continue;
                        }
                        
                        if (propertyChanges != null)
                        {
                            propertyChanges = propertyChanges
                                .Where(dataSource => dataSource != null)
                                .Select(dataSource =>
                                {
                                    s_propertyPathVisitor.Path = simPropertyPathPart.Path;
                                    if (!PropertyContainer.TryAccept(s_propertyPathVisitor, ref dataSource))
                                    {
                                        Debug.LogError("Could not evaluate property path");
                                        return null;
                                    }

                                    var value =  s_propertyPathVisitor.Value;
                                    s_propertyPathVisitor.Reset();
                                    return value;
                                });
                        }
                    }
                }

                if (propertyChanges == null)
                {
                    return new SimOneWayToUIBinding<T>(Observable.Never<T>());
                }
                
                var obs = propertyChanges
                    .OfType<object, T>();
                
                return new SimOneWayToUIBinding<T>(obs);
            }
            
            return null;
        }
    }
}