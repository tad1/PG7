using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using FieldCelluarAutomaton.ViewModels;

namespace FieldCelluarAutomaton.Views;

public partial class SavesView : UserControl
{

    public SavesView()
    {
        InitializeComponent();

        
    }
    

    private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var viewModel = (SavesViewModel)DataContext;
        var path = (string)((UserControl)sender).Tag;
        viewModel.Load(path);
    }
}