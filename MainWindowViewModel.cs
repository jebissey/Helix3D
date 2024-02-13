using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using HelixToolkit.Wpf;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfAppHelix3D;

internal class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel()
    {
    }

    private int selectedIndex;
    public int SelectedIndex
    {
        get => selectedIndex;
        set
        {
            SetProperty(ref selectedIndex, value);

            if (selectedIndex == 0) _ = WeakReferenceMessenger.Default.Send(new List<Visual3D>() { new CubeVisual3D() { SideLength = 100, Fill = new SolidColorBrush(Colors.Blue) } });
            else if (selectedIndex == 1) _ = WeakReferenceMessenger.Default.Send(new List<Visual3D>() { new TorusVisual3D() { TorusDiameter = 200, Fill = new SolidColorBrush(Colors.Orange) } });
            else throw new ApplicationException();

            if (selectedIndex == 0) _ = WeakReferenceMessenger.Default.Send(new List<string>() { "Tab1 line1", "Tab1 line2" });
            else if (selectedIndex == 1) _ = WeakReferenceMessenger.Default.Send(new List<string>() { "Tab2 line1", "Tab2 line2" });
            else throw new ApplicationException();
        }
    }
}
