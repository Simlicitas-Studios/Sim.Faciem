
# Sim.Faciem

Sim.Faciem is a framework designed to simplify working with Unity's UI Toolkit by providing a natural MVVM (Model-View-ViewModel) pattern for both editor and runtime environments.  
Unity has done a fantastic job introducing the UI Toolkit, but it lacks a comprehensive framework to fully utilize its potential. Sim.Faciem aims to bridge that gap by making it easier for developers to build complex and dynamic UIs.  

One of the core challenges developers face is connecting data from the GameObject world to the UI Toolkit world. Typically, developers end up using singleton patterns or manipulating UI controls directly from MonoBehaviours, which can lead to tightly coupled and hard-to-maintain code.  
Sim.Faciem addresses this by introducing a lightweight DI framework that acts as a starting point. However, it is recommended to replace it with a more advanced DI framework of your choice by implementing the provided interfaces.  

To keep it simple and efficient, Sim.Faciem leverages as many built-in Unity features as possible.  
The library fully integrates [UniTask](https://github.com/Cysharp/UniTask) and [R3](https://github.com/Cysharp/R3) from Cysharp as required dependencies. Additionally, the library requires Unity's Addressable system.  

## Features

1. **MVVM Architecture**: Provides a structured approach to separate presentation logic from business logic.
2. **ViewId System**: Manages views and their associated view models through assets.
3. **Region System**: Inspired by Prism and Angular's router-outlet, allowing dynamic view placement and nesting.
4. **Navigation System**: Handles view transitions and manages region contexts.
5. **Property Change Notifications**: Uses Unity's built-in binding system to update views automatically.
6. **Lightweight DI Support**: A minimal DI framework that can be replaced by any DI solution.
7. **Editor Integration**: Seamlessly works with editor windows and provides a lightweight DI container for editor use.

---

## Region System

### Concept
The Region System in Sim.Faciem enables developers to dynamically compose and display views within specified areas of the UI. This concept is inspired by:  
- [Prism Framework](https://github.com/PrismLibrary/Prism)  
- Angular's `router-outlet` feature  

#### Illustration
Imagine a UI layout where you have multiple regions defined:  
```
+-----------------------+
|      Header Region    |
+-----------------------+
|                       |
|     Main Content      |
|     +-------------+   |
|     |  Sub Region |   |
|     +-------------+   |
|                       |
+-----------------------+
|      Footer Region    |
+-----------------------+
```
Each of these regions can dynamically host different views based on navigation requests. Regions can also be nested to achieve highly modular UI designs.

### Implementation
A region is represented by a `RegionNameDefinition` (a scriptable object) that holds a unique `RegionName`. This makes it easy to define and manage region placements.  
Regions can be nested, and each region is associated with a `RegionManager` that manages the active view within that region.  
Navigation requires providing the current `RegionManager`, the target `ViewId`, and the `RegionName`. The system will locate the appropriate region control and either create a new instance or reactivate an existing one.

---

## ViewId System

### Concept
The **ViewId System** in Sim.Faciem links a View (UXML Document) with its corresponding ViewModel, enabling efficient management of UI elements and their data contexts.  

### Implementation
To define a View and its ViewModel, create a `ViewIdAsset`, which holds:  
- A unique name (struct `ViewId`).  
- A reference to the UXML document.  
- Two MonoScript references:  
  - An interface inheriting from `IDataContext`.  
  - A class inheriting from `BaseViewModel` (usually the `ViewModel` class).  

The `ViewIdAsset` can also auto-generate a static property in a user-defined script to simplify referencing the view.

---

## Navigation System

The **Navigation System** ties the Region and ViewId systems together, allowing you to control which view appears in a given region.  
As regions can be deeply nested, each region is managed by a `RegionManager`, and each view holds its own `RegionManager`. RegionManagers also know their parent, forming a hierarchy.  

When performing navigation, the system checks the current `RegionManager`, the target `ViewId`, and the `RegionName`.  
If a matching region is found, it either instantiates a new view or reactivates an existing one.

---

## ViewModel

### Lifecycle Methods
ViewModels are created via the dependency container, allowing you to resolve services in the constructor.  
They provide two lifecycle methods for handling navigation events:  
- `UniTask OnNavigateTo()` - Called when the view becomes active.  
- `UniTask OnNavigateAway()` - Called when the view is deactivated.  

### Navigation and Commands
The base ViewModel class exposes the `Navigation` property (type `IViewModelNavigationService`) to perform navigation:  
- `UniTask Navigate(ViewId, RegionName)`  
- `UniTask Clear(RegionName)`  

It also exposes the `Command` property (type `ICommandBuilderFactory`) to create commands:  
- `CommandBuilder Execute(Action)`  
- `AsyncCommandBuilder ExecuteAsync(Func<CancellationToken, UniTask>)`  

### Property Change Notifications
The ViewModel base class offers a protected `Observe` property of type `IViewModelPropertyObserver`.  
To observe a property, use:  
```csharp
public interface IViewModelPropertyObserver<T> where T : ViewModel<T>
{
    Observable<TProperty> Observe<TProperty>(Expression<Func<T, TProperty>> propertyExpression);
}
```
To trigger notifications, use the `SetProperty<T>(ref T field, T newValue)` method within the setter.

---

## How to Get Started

### Runtime
**(TBD)**  

### Editor Window

#### Lightweight DI Container
Sim.Faciem uses a lightweight DI container for editor integration.  
To add services, create a class that inherits from `EditorServiceInstaller` and implements:  
```csharp
public abstract void Install(IEditorInjector injector);
```  
After creating your scriptable object instance, the services are registered.

#### Editor ViewId
The `EditorViewIdAsset` is a special version of `ViewIdAsset` for editor-only views, maintaining a clear separation between runtime and editor components.

#### Creating an Editor Window
To create an editor window, inherit from `FaciemEditorWindow` and override the lifecycle methods:  
```csharp
using Cysharp.Threading.Tasks;
using Plugins.Sim.Faciem.Editor;
using UnityEditor;

public class MyEditorWindow : FaciemEditorWindow
{
    [MenuItem("Window/My Editor Window")]
    public static void ShowWindow()
    {
        GetWindow<MyEditorWindow>("My Editor Window");
    }

    protected override async UniTask NavigateTo()
    {
        await base.NavigateTo();
        Debug.Log("Navigated to MyEditorWindow");
    }

    protected override async UniTask NavigateAway()
    {
        await base.NavigateAway();
        Debug.Log("Navigated away from MyEditorWindow");
    }
}
```

---

## License
Sim.Faciem is licensed under the MIT License.