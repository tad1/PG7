using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Policy;
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

namespace CalcArithmetic;
[Export(typeof(ICalculator))]
public class Calculator : ICalculator
{
    private UserControl userControl = new UserControl1();
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
        ButtonSum.Click += ButtonSumOnClick;
        ButtonMulti.Click += ButtonMultiOnClick;
    }

    private void ButtonMultiOnClick(object sender, RoutedEventArgs e)
    {
        var (a, b) = GetValues();
        Result.Text = (a*b).ToString();
    }

    private void ButtonSumOnClick(object sender, RoutedEventArgs e)
    {
        var (a, b) = GetValues();
        Result.Text = (a+b).ToString();
    }

    private (int, int) GetValues()
    {
        return (int.Parse(ValueA.Text), int.Parse(ValueB.Text));
    }

}