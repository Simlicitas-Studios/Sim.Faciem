using Cysharp.Threading.Tasks;

namespace Sim.Faciem
{
    public interface INavigationService
    {
        UniTask NavigateTo(RegionManager regionManager, ViewId viewId, RegionName regionName);
    }
}