using System.Numerics;
using CommunityToolkit.Mvvm.ComponentModel;
using FieldCelluarAutomaton.Core;

namespace FieldCelluarAutomaton.ViewModels;

public partial class StatusViewModel : ObservableObject
{
    IBus _bus;
    [ObservableProperty] bool isPlaying = false;

    [ObservableProperty] private Complex position;
    [ObservableProperty] private Complex gridSize;
    [ObservableProperty] private ulong tickNumber;
    [ObservableProperty] private ulong rulesApplied;
    [ObservableProperty] private ulong deadCells;

    public StatusViewModel(IBus bus)
    {
        _bus = bus;
        _bus.Subscribe<bool>("isPlaying", value =>
        {
            IsPlaying = value;
            OnPropertyChanged(nameof(IsPlaying));
        });
        _bus.Subscribe<ulong>("tickNumber", value =>
        {
            TickNumber = value;
            OnPropertyChanged(nameof(TickNumber));
        });
        _bus.Subscribe<Complex>("selectedCell", value => Position = value);
    }
}