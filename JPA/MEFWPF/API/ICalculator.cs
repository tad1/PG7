namespace API;
using System.Windows.Controls;

public interface ICalculator
{
    string Name { get; set; }
    UserControl GetUserControl();
}