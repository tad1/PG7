using System.Reactive.Subjects;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FieldCelluarAutomaton.Core;

namespace FieldCelluarAutomaton.ViewModels;

public partial class ToolboxViewModel : ObservableObject
{
    private IBus _bus;
    [ObservableProperty] private bool isPlaying = false;
    
    private string selectedRule = string.Empty;
    
    [ObservableProperty]
    private float playSpeed = 1.0f;
    DispatcherTimer _dispatcherTimer = new DispatcherTimer();
    
    public ToolboxViewModel(IBus bus)
    {
        _bus = bus;
        bus.Subscribe<bool>("playing", b =>
        {
            isPlaying = b;
            OnPropertyChanged(nameof(isPlaying));
        });
        
        bus.Subscribe<string>("selectedRule", str =>
        {
            selectedRule = str;
        });
    }

    [RelayCommand]
    void Play()
    {
        _bus.Publish<bool>("playing", true);
    }
}