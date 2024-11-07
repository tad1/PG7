using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FieldCelluarAutomaton.Models;
using FieldCelluarAutomaton.ViewModels;

namespace FieldCelluarAutomaton.Views;

public partial class GridView : UserControl
{
    
    public GridView()
    {
        InitializeComponent();
    }

    private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is FrameworkElement frameworkElement)
        {
            if (frameworkElement.DataContext is CellModel cell)
            {
                var viewModel = (GridViewModel)DataContext;
                viewModel.CellClickCommand.Execute(cell);
            }
        }
    }
}