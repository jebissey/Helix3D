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

    private const int max = 4;
    private int counter;

    private int selectedIndex;
    public int SelectedIndex
    {
        get => selectedIndex;
        set
        {
            SetProperty(ref selectedIndex, value);

            if (selectedIndex == 0) _ = WeakReferenceMessenger.Default.Send(Gets(counter));
            else if (selectedIndex == 1) _ = WeakReferenceMessenger.Default.Send(Gets(counter + 1));
            else throw new ApplicationException();

            if (selectedIndex == 0) _ = WeakReferenceMessenger.Default.Send(new List<string>() { $"Tab1 line{counter + 1}", $"Tab1 line{counter + 2}" });
            else if (selectedIndex == 1) _ = WeakReferenceMessenger.Default.Send(new List<string>() { "Tab2 line1", "Tab2 line2" });
            else throw new ApplicationException();

            counter++;
        }
    }

    private static Visual3D Get(int index, Point3D center)
    {
        if (index % max == 0) return new CubeVisual3D() { Center = center, SideLength = 100, Fill = new SolidColorBrush(Colors.Blue) };
        if (index % max == 1) return new TorusVisual3D() { TorusDiameter = 200, TubeDiameter = 30, Fill = new SolidColorBrush(Colors.Orange) };
        if (index % max == 2) return new EllipsoidVisual3D() { Center = center, RadiusX = 20, RadiusY = 30, RadiusZ = 200, Fill = new SolidColorBrush(Colors.Green) };
        if (index % max == 3) return new TruncatedConeVisual3D() { BaseRadius = 100, Height = 60, TopRadius = 20, Fill = new SolidColorBrush(Colors.Brown) };
        throw new ApplicationException();
    }

    private static List<Visual3D> Gets(int index)
    {
        List<Visual3D> visual3Ds = new();
        for (int i = 0; i < index % max; i++)
        {
            visual3Ds.Add(Get(i, new Point3D(0, 0, 0)));
        }
        return visual3Ds;
    }
}
