using System.Numerics;
using CommunityToolkit.Mvvm.ComponentModel;
using ComplexMathLibrary;
using FieldCelluarAutomaton.Core;

namespace FieldCelluarAutomaton.ViewModels;

public partial class CellInfoViewModel : ObservableObject
{
    private IBus _bus;

    [ObservableProperty]
    private Complex selectedCell;

    
    private Complex _selectedValue;
    public double Imaginary
    {
        get => _selectedValue.Imaginary;
        set
        {
            _selectedValue = new Complex(_selectedValue.Real, value);
            _bus.Publish<Complex>("selectedValue", _selectedValue);
        }
    }
    
    public double Real
    {
        get => _selectedValue.Real;
        set
        {
            _selectedValue = new Complex(value, _selectedValue.Imaginary);
            _bus.Publish<Complex>("selectedValue", _selectedValue);
        }
    }

    private bool _isDrawing;
    public bool IsDrawing { 
        get => _isDrawing; 
        set {
            _isDrawing = value;
            _bus.Publish<bool>("isDrawing",_isDrawing); 
        } 
    }
    
    public CellInfoViewModel(IBus bus)
    {
        _bus = bus;
        _bus.Subscribe<Complex>("selectedCell", complex =>
        {
            selectedCell = complex;
            var value = ComplexMathLibrary.Public.grid.array[(int)complex.Real, (int)complex.Imaginary];
            if (!_isDrawing)
            {
                _selectedValue = value;
                OnPropertyChanged(nameof(Imaginary));
                OnPropertyChanged(nameof(Real));
            }
            OnPropertyChanged(nameof(selectedCell));
        });
        _bus.Subscribe<bool>("isDrawing", b =>
        {
            _isDrawing = b;
            OnPropertyChanged(nameof(IsDrawing));
        }); 
    }
}