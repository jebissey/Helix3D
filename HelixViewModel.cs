using CommunityToolkit.Mvvm.Messaging;
using HelixToolkit.Wpf;
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;

namespace WpfAppHelix3D;

internal class HelixViewModel
{
    public ObservableCollection<Visual3D> Objects { get; } = new ObservableCollection<Visual3D>();

    public HelixViewModel()
    {
        WeakReferenceMessenger.Default.Register<List<Visual3D>>(this, (_, visual3Ds) => SetObjects(visual3Ds));
    }

    private void SetObjects(List<Visual3D> visual3Ds)
    {
        Objects.Clear();
        Objects.Add(new DefaultLights());
        visual3Ds.ForEach(o => Objects.Add(o));
    }
}
