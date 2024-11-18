using System.Collections.ObjectModel;
using System.Numerics;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComplexMathLibrary;
using FieldCelluarAutomaton.Components;
using FieldCelluarAutomaton.Core;
using FieldCelluarAutomaton.Models;

namespace FieldCelluarAutomaton.ViewModels;

public partial class GridViewModel : ObservableObject
{
    private IBus _bus;

    [ObservableProperty] 
    ObservableCollection<CellModel> gameGrid;

    [ObservableProperty]
    private int gridHeight;
    [ObservableProperty]
    private int gridWidth;
    
    [ObservableProperty]
    private double zoom = 1;

    [ObservableProperty] 
    private bool isPlaying;
    
    [ObservableProperty]
    private CellDisplay cellDisplay = CellDisplay.Number;

    [ObservableProperty] private double offsetX;
    [ObservableProperty] private double offsetY;
    
    private Complex selectedValue;
    private (int, int)? selectedCell;
    private bool _isDrawing;

    private CellModel[] GetCellData()
    {
            return ComplexMathLibrary.Public.grid.array.Cast<Complex>().Select(
                (complex, i) =>
                {
                    (int, int) pos = (i % GridWidth, i / GridWidth);
                    return new CellModel
                    {
                        ColumnIdx = pos.Item1,
                        RowIdx = pos.Item2,
                        Value = complex,
                        Angle = -double.RadiansToDegrees(complex.Phase),
                        BoundedMagnitude = 1.5 - 1.5 / (5*complex.Magnitude + 1),
                        IsSelected = selectedCell?.Item1 == pos.Item1 && selectedCell?.Item2 == pos.Item2
                    };
                }
            ).ToArray();
    }
    
    [RelayCommand]
    private void CellClick(CellModel cell)
    {
        _bus.Publish<Complex>("selectedCell", new Complex(cell.ColumnIdx, cell.RowIdx));
        if (_isDrawing && selectedCell != null)
        {
            Public.grid.array[selectedCell.Value.Item1, selectedCell.Value.Item2] = selectedValue;
            _bus.Publish<Event>("grid_updated", new Event());
        }

        if (!_isDrawing)
        {
            _bus.Publish<Complex>("selectedValue", cell.Value);
        }
    }

    public GridViewModel(IBus bus)
    {
        this.selectedCell = null;
        _bus = bus;
        _bus.RegisterSubscriptions(this);
        _bus.Subscribe<bool>("isDrawing", isDrawing => _isDrawing = isDrawing);
        _bus.Subscribe<CellDisplay>("cellDisplay", cellDisplay =>
        {
            this.cellDisplay = cellDisplay;
            OnPropertyChanged(nameof(CellDisplay));
        });
        
        gridHeight = ComplexMathLibrary.Public.grid.array.GetLength(0);
        gridWidth = ComplexMathLibrary.Public.grid.array.GetLength(1);
        gameGrid = new ObservableCollection<CellModel>(GetCellData());
        
    }

    [Subscribe("selectedValue")]
    private void UpdateSelectedValue(Complex complex)
    {
        selectedValue = complex;
    }

    [Subscribe("selectedCell")]
    private void UpdateSelectedCell(Complex complex)
    {
        if (selectedCell is not null)
        {
                
            var pos = selectedCell.Value.Item1 + selectedCell.Value.Item2 * gridWidth;
            var element = GameGrid[pos];
            var cellModel = new CellModel()
            {
                ColumnIdx = element.ColumnIdx,
                RowIdx = element.RowIdx,
                Angle = element.Angle,
                Value = element.Value,
                BoundedMagnitude = element.BoundedMagnitude,
                IsSelected = false
            };
            GameGrid[pos] = cellModel;
        }
        {
            var pos = (int)(complex.Real + complex.Imaginary * gridWidth);
            var element = GameGrid[pos];
            var cellModel = new CellModel()
            {
                ColumnIdx = element.ColumnIdx,
                RowIdx = element.RowIdx,
                Angle = element.Angle,
                Value = element.Value,
                BoundedMagnitude = element.BoundedMagnitude,
                IsSelected = true
            };
            GameGrid[pos] = cellModel;
            selectedCell = ((int)complex.Real, (int)complex.Imaginary);
        }
        OnPropertyChanged(nameof(GameGrid));
    }

    [Subscribe("grid_updated")]
    private void OnGridUpdated(Event _)
    {
        var array = ComplexMathLibrary.Public.grid.array;
        for (var i = 0; i < array.Length; i++)
        {
            var pos = (i % GridWidth, i / GridWidth);
            var element = array[pos.Item1, pos.Item2];
            gameGrid[i] = new CellModel
            {
                ColumnIdx = pos.Item1,
                RowIdx = pos.Item2,
                Value = element,
                Angle = -double.RadiansToDegrees(element.Phase),
                BoundedMagnitude = 1.5 - 1.5 / (5*element.Magnitude + 1),
                IsSelected = selectedCell?.Item1 == pos.Item1 && selectedCell?.Item2 == pos.Item2
            };
        }
    }

    [Subscribe("gridSize")]
    private void UpdateGridSize((int, int) tuple)
    {
        GridWidth = tuple.Item1;
        GridHeight = tuple.Item2;
        GameGrid = new ObservableCollection<CellModel>(new CellModel[GridWidth * GridHeight]);
        selectedCell = null;
    }

    [Subscribe("isPlaying")]
    private void UpdateIsPlaying(bool value)
    {
        IsPlaying = value;
        OnPropertyChanged(nameof(IsPlaying));
    }

    [Subscribe("zoom")]
    private void UpdateZoom(ZoomModel zoom)
    {
        this.Zoom = zoom._zoom;
        this.OffsetX = zoom.offsetX;
        this.OffsetY = zoom.offsetY;
        OnPropertyChanged(nameof(this.Zoom));
    }
}