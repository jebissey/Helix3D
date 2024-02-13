using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace WpfAppHelix3D;

internal class SharedViewModel
{
    public SharedViewModel()
    {
        WeakReferenceMessenger.Default.Register<List<string>>(this, (_, lines) => SetLines(lines));
    }

    public ObservableCollection<string> ListItemsSource { get; set; } = new() { "init value 1", "init value 2" };

    private void SetLines(List<string> lines)
    {
        ListItemsSource.Clear();
        lines.ForEach(x => ListItemsSource.Add(x));
    }
}
