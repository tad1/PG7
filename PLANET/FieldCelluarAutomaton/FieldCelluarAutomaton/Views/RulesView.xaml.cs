using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FieldCelluarAutomaton.ViewModels;

namespace FieldCelluarAutomaton.Views;

public partial class RulesView : UserControl
{
    public RulesView()
    {
        InitializeComponent();
    }

    private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var viewModel = (RulesViewModel)DataContext;
        viewModel.ApplySelectedRule();
    }
}