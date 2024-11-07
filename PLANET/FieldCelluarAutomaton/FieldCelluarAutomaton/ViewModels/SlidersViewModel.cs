using CommunityToolkit.Mvvm.ComponentModel;
using FieldCelluarAutomaton.Core;
using FieldCelluarAutomaton.Models;

namespace FieldCelluarAutomaton.ViewModels;

public partial class SlidersViewModel : ObservableObject
{
    private IBus _bus;

    private ZoomModel _zoom;
    private float _speed;

    public double Zoom
    {
        get => _zoom._zoom; 
        set {
            _zoom._zoom = value;
            _bus.Publish<ZoomModel>("zoom",_zoom); 
        }
    }
    public float Speed
    {
        get => _speed;
        set => _bus.Publish<float>("tickSpeedMs", 1000/value); 
    }

    public SlidersViewModel(IBus bus)
    {
        _bus = bus;
        _bus.Subscribe<ZoomModel>("zoom", s =>
        {
            _zoom = s;
            OnPropertyChanged(nameof(Zoom));
        });
        _bus.Subscribe<float>("tickSpeedMs", s =>
        {
            _speed = 1000/s;
            OnPropertyChanged(nameof(Speed));
        });
    }
}