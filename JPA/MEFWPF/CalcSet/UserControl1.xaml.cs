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

namespace CalcSet;
[Export(typeof(ICalculator))]
public class Calculator : ICalculator
{
    private UserControl userControl = new UserControl1();
    public string Name { get; set; } = "Set Calculator";
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
        var (setA, setB) = GetSetValues();
        setA.IntersectWith(setB);
        Result.Text = setA.Aggregate("", (a, b) => a + " " + b);  
    }

    private void ButtonSumOnClick(object sender, RoutedEventArgs e)
    {
        var (setA, setB) = GetSetValues();
        setA.UnionWith(setB);
        Result.Text = setA.Aggregate("", (a, b) => a + " " + b);  
    }

    private (HashSet<string>,HashSet<string>) GetSetValues()
    {
        var valA = ValueA.Text.Split(' ');
        var setA = new HashSet<string>();
        foreach (var s in valA)
        {
            setA.Add(s);
        }
        var valB = ValueB.Text.Split(' ');
        var setB = new HashSet<string>();
        foreach (var s in valB)
        {
            setB.Add(s);
        }
        return (setA, setB);
    }

}