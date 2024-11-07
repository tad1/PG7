using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using FieldCelluarAutomaton.Models;

namespace FieldCelluarAutomaton.Components;

public partial class Cell : UserControl
{
    public Cell()
    {
        InitializeComponent();
    }
    
    public static readonly DependencyProperty DisplayTypeProperty = 
        DependencyProperty.Register("DisplayType", typeof(CellDisplay), typeof(Cell),
            new PropertyMetadata(CellDisplay.Arrow, null));

    public CellDisplay DisplayType
    {
        get => (CellDisplay)GetValue(DisplayTypeProperty);
        set => SetValue(DisplayTypeProperty, value);
    }

}