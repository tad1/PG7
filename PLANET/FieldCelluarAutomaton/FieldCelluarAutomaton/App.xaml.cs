using System.Configuration;
using System.Data;
using System.Numerics;
using System.Windows;
using Dark.Net;

namespace FieldCelluarAutomaton;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        DarkNet.Instance.SetCurrentProcessTheme(Theme.Dark);
    }
}