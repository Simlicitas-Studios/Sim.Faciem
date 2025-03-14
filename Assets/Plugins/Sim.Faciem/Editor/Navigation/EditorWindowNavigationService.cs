using Cysharp.Threading.Tasks;
using Sim.Faciem;

namespace Plugins.Sim.Faciem.Editor.Navigation
{
    public class EditorWindowNavigationService : IEditorWindowNavigationService
    {
        private readonly FaciemEditorWindow _editorWindow;
        private readonly INavigationService _navigationService;

        public EditorWindowNavigationService(FaciemEditorWindow editorWindow, INavigationService navigationService)
        {
            _editorWindow = editorWindow;
            _navigationService = navigationService;
        }
        
        public UniTask Navigate(ViewId viewId, RegionName region)
        {
            return _navigationService.NavigateTo(_editorWindow.RegionManager, viewId, region);
        }
    }
}