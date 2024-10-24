using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.IO;
using System.Windows.Threading;
using API;
using Path = System.IO.Path;

namespace MEFWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    FileSystemWatcher watcher;
    private string pluginPath;
    
    DirectoryCatalog catalog;
    CompositionContainer _container;
    
    public MainWindow()
    {
        InitializeComponent();

        pluginPath = ConfigurationManager.AppSettings["PluginsPath"];
        if (!Directory.Exists(pluginPath)) throw new FileNotFoundException("PluginsPath not found");
        Console.WriteLine("plugin dir path: "+Path.GetFullPath(pluginPath));
        
        catalog = new DirectoryCatalog(pluginPath);
        _container = new CompositionContainer(catalog);
        
        ReloadPlugins(null, null);

        watcher = new FileSystemWatcher();
        watcher.Path = pluginPath;
        watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
        watcher.Filter = "*.dll";
        watcher.Changed += ReloadPlugins;
        watcher.Deleted += ReloadPlugins;
        watcher.EnableRaisingEvents = true;
    }
    
    private void ReloadPlugins(object sender, FileSystemEventArgs e)
    {
        
        Application.Current.Dispatcher.Invoke((Action)(() =>
        {
            catalog.Refresh();
            _container = new CompositionContainer(catalog);
            var values = _container.GetExportedValues<ICalculator>();
            
            TabControl.Items.Clear();
            foreach (var plugin in values)
            {
                var tab = new TabItem();
                tab.Header = plugin.Name;
                tab.Content = plugin.GetUserControl();
                TabControl.Items.Add(tab);
            }
        }));
    }

}