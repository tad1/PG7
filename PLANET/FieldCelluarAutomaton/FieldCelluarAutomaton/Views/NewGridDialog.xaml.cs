using System.ComponentModel;
using System.Windows;
using Dark.Net;
using Dark.Net.Wpf;

namespace FieldCelluarAutomaton.Views;

public partial class NewGridDialog : Window
{
    public NewGridDialog()
    {
        InitializeComponent();
        DarkNet.Instance.SetWindowThemeWpf(this, Theme.Dark);
        SkinManager skinManager = (SkinManager)FindResource("skinManager");
        skinManager.RegisterSkins(new Uri("Skins/Skin.Light.xaml", UriKind.Relative), new Uri("Skins/Skin.Dark.xaml", UriKind.Relative));
    }

    private void Window_Closing(object? sender, CancelEventArgs e)
    {
        DialogResult = false;
    }

    private void okButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }
}