using System.ComponentModel.Composition;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using API;

namespace CalcSet;
[Export(typeof(ICalculator))]
public class Calculator : ICalculator
{
    private UserControl userControl = new UserControl();
    public string Name { get; set; } = "Arithmetic Calculator";
    public UserControl GetUserControl()
    {
        return userControl;
    }
}
/// <summary>
/// Interaction logic for UserControl1.xaml
/// </summary>
public partial class UserControl1 : UserControl
{
    public UserControl1()
    {
        InitializeComponent();
    }
}