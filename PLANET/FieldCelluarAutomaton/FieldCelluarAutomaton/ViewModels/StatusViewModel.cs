using System.Numerics;
using CommunityToolkit.Mvvm.ComponentModel;
using ComplexMathLibrary;
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
    [ObservableProperty] private string boardType;

    public StatusViewModel(IBus bus)
    {
        _bus = bus;
        _bus.RegisterSubscriptions(this);
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

    [Subscribe("board_type")]
    void UpdateBoardType(Public.GridType boardType)
    {
        BoardType = boardType.ToString();
        OnPropertyChanged(nameof(BoardType));
    }

    [Subscribe("gridSize")]
    private void UpdateGridSize((int, int) tuple)
    {
        GridSize = new Complex(tuple.Item1, tuple.Item2);
        OnPropertyChanged(nameof(GridSize));
    }
}