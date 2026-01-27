using R3;
using Unity.Properties;
using UnityEditor;

namespace Sim.Faciem.uGUI.Editor.BindingWindow
{
    public class BindingWindowViewModel : ViewModel<BindingWindowViewModel>, IBindingWindowDataContext
    {
        private MonoScript _dataSource = null;
        private string _propertyPath;
        private string _dataSourceType = string.Empty;

        [CreateProperty]
        public MonoScript DataSource
        {
            get => _dataSource;
            set => SetProperty(ref _dataSource, value);
        }

        [CreateProperty]
        public string DataSourceType
        {
            get => _dataSourceType;
            set => SetProperty(ref _dataSourceType, value);
        }

        [CreateProperty]
        public string PropertyPath
        {
            get => _propertyPath;
            set => SetProperty(ref _propertyPath, value);
        }

        public BindingWindowViewModel(IBindingManipulationProvider manipulationProvider)
        {
            Disposables.Add(Property.Observe(x => x.DataSource)
                .Subscribe(newDataSource =>
                {
                    if (manipulationProvider.BindableProperty.CurrentValue != null &&
                        (!manipulationProvider.BindableProperty.CurrentValue.BindingInfo.DataSource?.Script.Equals(newDataSource) ?? true))
                    {
                        manipulationProvider.BindableProperty.CurrentValue.BindingInfo = new SimBindingInfo
                        {
                            DataSource = new MonoScriptReference
                            {
                                Script = newDataSource,
                                TypeName = newDataSource.GetClass().AssemblyQualifiedName
                            },
                            PropertyPath = new PropertyPath()
                        };
                    
                        manipulationProvider.BindableProperty.ForceNotify();   
                    }
                    
                    DataSourceType = newDataSource?.GetInstanceID() != 0 
                        ? newDataSource?.GetClass().AssemblyQualifiedName ?? string.Empty
                        : string.Empty;
                }));
            
            Disposables.Add(Property.Observe(x => x.PropertyPath)
                .Select(newPath => new PropertyPath(newPath))
                .Where(newPath => manipulationProvider.BindableProperty.CurrentValue != null
                    && !manipulationProvider.BindableProperty.CurrentValue.BindingInfo.PropertyPath.Equals(newPath))
                .Subscribe(newPath =>
                {
                    manipulationProvider.BindableProperty.Value.BindingInfo = new SimBindingInfo
                    {
                        DataSource = manipulationProvider.BindableProperty.Value.BindingInfo.DataSource,
                        PropertyPath = newPath
                    };
                    manipulationProvider.BindableProperty.ForceNotify();
                }));
            
            Disposables.Add(manipulationProvider
                .BindableProperty
                .Subscribe(bindableProperty =>
                {
                    DataSource = bindableProperty?.BindingInfo.DataSource?.Script;
                    PropertyPath = bindableProperty?.BindingInfo.PropertyPath.ToString();
                }));
        }
    }
}