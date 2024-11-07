using System.Numerics;

namespace FieldCelluarAutomaton.Models;

public struct CellModel
{
    public int ColumnIdx;
    public int RowIdx;
    public bool IsSelected { get; set; }
    public Complex Value { get; set; }
    public double Angle { get; set; }
}