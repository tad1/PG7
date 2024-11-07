using System.Numerics;
using ComplexMathLibrary;

namespace FieldCelluarAutomaton.Core;

public class EngineBridge
{
    IBus _bus;

    private ulong _tickNumber = 0;
    private string _selectedRule = string.Empty;

    public EngineBridge(IBus bus)
    {
        _bus = bus;
        _bus.Subscribe<string>("selectedRule", s =>
        {
            _selectedRule = s;
        });
        _bus.Subscribe<ulong>("tickNumber", s =>
        {
            _tickNumber = s;
        });
    }

    public void CreateRule(string rule)
    {
        Public.add_rule(rule);
        _bus.Publish<Event>("ruleset_updated", new Event());
    }
    
    public void ApplyRule(string rule)
    {
        ComplexMathLibrary.Public.apply(ComplexMathLibrary.Public.ruleset[rule]);
        _bus.Publish<Event>("grid_updated", new Event());
    }

    public void Advance()
    {
        ApplyRule(_selectedRule);
        _bus.Publish<ulong>("tickNumber", ++_tickNumber);
    }

    public void New(int width, int height)
    {
        Public.create_grid(width, height);
        _bus.Publish<(int, int)>("gridSize", (width, height));
        _bus.Publish<Event>("grid_updated", new Event());
    }

    public void Load(Complex[,] grid)
    {
        Public.grid = new Public.CheckedGrid(grid);
        _bus.Publish<(int, int)>("gridSize", (grid.GetLength(0), grid.GetLength(1)));
        _bus.Publish<Event>("grid_updated", new Event());
    }
}