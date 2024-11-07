using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using FieldCelluarAutomaton.Core;

namespace FieldCelluarAutomaton.ViewModels;

public partial class SavesViewModel : ObservableObject
{
    [ObservableProperty] private string[] saves = new string[]{};
    private ILoad _model;

    FileSystemWatcher watcher;
    
    public SavesViewModel(ILoad model, string path)
    {
        this._model = model;
     
        watcher = new FileSystemWatcher();
        watcher.Path = path;
        watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
        watcher.Filter = "*.json";
        watcher.Created += WatcherOnCreated; 
        watcher.Deleted += WatcherOnCreated;
        watcher.EnableRaisingEvents = true;
        UpdateFiles();
    }

    public void Load(string path)
    {
        _model.Load(path);
    }

    private void UpdateFiles()
    {
        Saves = Directory.GetFiles(watcher.Path).Select(s => Path.GetFileNameWithoutExtension(s)).ToArray();
    }
    private void WatcherOnCreated(object sender, FileSystemEventArgs e)
    {
        UpdateFiles();
    }
}