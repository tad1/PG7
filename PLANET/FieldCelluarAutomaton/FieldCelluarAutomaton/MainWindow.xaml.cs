using System.Configuration;
using System.IO;
using System.Media;
using System.Net;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using ComplexMathLibrary;
using Dark.Net;
using Dark.Net.Wpf;
using FieldCelluarAutomaton.Components;
using FieldCelluarAutomaton.Core;
using FieldCelluarAutomaton.Models;
using FieldCelluarAutomaton.ViewModels;
using FieldCelluarAutomaton.Views;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;


namespace FieldCelluarAutomaton;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, ILoad
{
    private PersistantBus _bus = new();
    private const float ZOOM_SPEED = 4.0f;
    DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Normal);
    IConfiguration Configuration { get; set; }
    (CellDisplay type, MenuItem item)[] DisplayMenuItems;
    private string saveDir;
    private string SaveDirFullPath => $@"{Directory.GetCurrentDirectory()}\{saveDir}";
    private string DefaultSavePath => $@"{Directory.GetCurrentDirectory()}\{saveDir}\quicksave.json";

    //state
    private string selectedRule = string.Empty;
    private Public.GridType selectedBoardType = Public.GridType.Checked;
    private int historyElement = 0;
    private string currentCommand = "";
    private ulong TickNumber = 0;
    
    private bool isPlaying = false;
    private (int width, int height) GridSize { get; set; } = (10, 10);
    private List<string> commandHistory = new();
    private ZoomModel zoom = new ZoomModel()
    {
        _zoom = 1,
        offsetX = 0,
        offsetY = 0 
    };
    
    //models
    private RulesViewModel rulesViewModel;
    private GridViewModel gridViewModel;
    private CellInfoViewModel cellInfoViewModel;
    private SlidersViewModel slidersViewModel;
    private StatusViewModel statusViewModel;
    private SavesViewModel savesViewModel;
    
    private EngineBridge engineBridge;
    
    
    public MainWindow()
    {
        timer.Interval = TimeSpan.FromMilliseconds(500);
        timer.Tick += Timer_Tick;
        
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        
        Configuration = builder.Build();
        saveDir = Configuration["AppSettings:SaveDirectory"] ?? "saves";
        
        InitializeComponent();
        DisplayMenuItems = new[]
        {
            (CellDisplay.Arrow, DisplayArrowsCheck),
            (CellDisplay.Number, DisplayNumbersCheck),
            (CellDisplay.Boolean, DisplayBooleanCheck),
        };
        
        rulesViewModel = new RulesViewModel(_bus);
        RulesView.DataContext = rulesViewModel;
        gridViewModel = new GridViewModel(_bus);
        GridView.DataContext = gridViewModel;
        cellInfoViewModel = new CellInfoViewModel(_bus);
        CellInfoView.DataContext = cellInfoViewModel;
        slidersViewModel = new SlidersViewModel(_bus);
        SlidersView.DataContext = slidersViewModel;
        statusViewModel = new StatusViewModel(_bus);
        StatusView.DataContext = statusViewModel;
        engineBridge = new EngineBridge(_bus);
        savesViewModel = new SavesViewModel(this, saveDir);
        SavesView.DataContext = savesViewModel;
        
        
        DarkNet.Instance.SetWindowThemeWpf(this, Theme.Dark);
        SkinManager skinManager = (SkinManager)FindResource("skinManager");
        skinManager.RegisterSkins(new Uri("Skins/Skin.Light.xaml", UriKind.Relative), new Uri("Skins/Skin.Dark.xaml", UriKind.Relative));
        
        _bus.RegisterSubscriptions(this);
        _bus.Publish<(int, int)>("gridSize", (10,10));
        _bus.Publish<Public.GridType>("board_type", Public.GridType.Checked);

        _bus.Subscribe<string>("selectedRule", s => selectedRule = s);
        _bus.Subscribe<ZoomModel>("zoom", s => zoom = s);
        _bus.Subscribe<float>("tickSpeedMs", s => timer.Interval = TimeSpan.FromMilliseconds(s));
        _bus.Subscribe<List<string>>("commandHistory", s => commandHistory = s);
        _bus.Subscribe<CellDisplay>("cellDisplay", s =>
        {
            foreach (var tuple in DisplayMenuItems)
            {
                tuple.item.IsChecked = tuple.type == s;
            }
        });
        _bus.Subscribe<(int,int)>("gridSize", tuple =>
        {
            GridSize = tuple;
        });
    }

    [Subscribe("board_type")]
    private void UpdateBoardType(Public.GridType gridType)
    {
        selectedBoardType = gridType;
    }
    
    public void Save(string filePath)
    {
        var saveData = new SaveData()
        {
            grid = Public.grid.array,
            rules = Public.rulesetInfo.Select(pair => (pair.Value.RuleId, pair.Value.RuleStr)).ToArray(),
            state = _bus.State.Select(pair => (pair.Key.Item1.AssemblyQualifiedName, pair.Key.Item2, JsonConvert.SerializeObject(pair.Value))).ToArray()!,
        };
        var jsonString = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        if (!Directory.Exists(SaveDirFullPath))
        {
            Directory.CreateDirectory(SaveDirFullPath);
        }
        File.WriteAllText(filePath, jsonString);
    }

    public void Load(string filePath)
    {
        var obj = System.Configuration.ConfigurationManager.AppSettings;
        var jsonString = File.ReadAllText(filePath);
        var saveData = JsonConvert.DeserializeObject<SaveData>(jsonString);
        engineBridge.Load(saveData.grid);
        foreach (var (id, rule) in saveData.rules)
        {
            engineBridge.CreateRule(rule);
        }
        
        var dictionary = saveData.state.ToDictionary(tuple => (Type.GetType(tuple.type)!, tuple.name), tuple => JsonConvert.DeserializeObject(tuple.value, Type.GetType(tuple.type)!));
        _bus.Load(dictionary!);
    }
    
    private void Timer_Tick(object sender, object e)
    {
        engineBridge.Advance();
    }

    private void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            string input = CommandInterface.Text;
            var args = input.Split(' ');
            switch (input.Split(' ')[0])
            {
                case "step" or "s":
                    engineBridge.Advance();
                    break;
                case "cell" or "c":
                    if (args.Length >= 3)
                    {
                        _bus.Publish<Complex>("selectedCell", new Complex(int.Parse(args[1]), int.Parse(args[2])));
                    }
                    break;
                case "rule" or "r":
                    if (args.Length >= 2)
                    {
                        _bus.Publish<string>("selectedRule",args[1]);
                    }
                    break;
                case "new":
                    (int width, int height) size;
                    if(args.Length >= 3) size = (int.Parse(args[1]), int.Parse(args[2]));
                    else if(args.Length >= 2) size = (int.Parse(args[1]), int.Parse(args[1]));
                    else size = (GridSize.width, GridSize.height);
                    engineBridge.New(size.width, size.height);
                    break;
                case "jump":
                    Storyboard staticStoryboard = (Storyboard)FindResource("JumpAnimationStoryboard");
                    Storyboard storyboard = staticStoryboard.Clone();
                    Storyboard.SetTarget(storyboard, this);
                    var animation = (DoubleAnimationUsingKeyFrames)storyboard.Children[0];
                    foreach (var keyFrame in animation.KeyFrames)
                    {
                        if (keyFrame is DoubleKeyFrame doubleKeyFrame)
                        {
                            doubleKeyFrame.Value += this.Top;
                        }
                    }
                    storyboard.Begin();
                    break;
                case "display" or "disp":
                    if(args.Length == 0) break;
                    switch (args[1])
                    {
                        case "number" or "n":
                            _bus.Publish<CellDisplay>("cellDisplay", CellDisplay.Number);
                            break;
                        case "boolean" or "b":
                            _bus.Publish<CellDisplay>("cellDisplay", CellDisplay.Boolean);
                            break;
                        case "arrow" or "a":
                            _bus.Publish<CellDisplay>("cellDisplay", CellDisplay.Arrow);
                            break;
                    }
                    break;
                case "type" or "t":
                    if(args.Length == 0) break;
                    switch (args[1])
                    {
                        case "checked" or "chk" or "c":
                            _bus.Publish<Public.GridType>("board_type", Public.GridType.Checked);
                            break;
                        case "wrapped" or "wrp" or "w":
                            _bus.Publish<Public.GridType>("board_type", Public.GridType.Wrapped);
                            break;
                    }
                    break;
                case "apply" or "a":
                    if(args.Length >= 2 && ComplexMathLibrary.Public.ruleset.Keys.Contains(args[1]))
                        engineBridge.ApplyRule(args[1]);
                    break;
                case "save":
                    if (args.Length >= 2)
                    {
                        Save(ShortNameToPath(args[1]));
                    }
                    else
                    {
                        Save(DefaultSavePath);
                    }
                    break;
                case "load":
                    if (args.Length >= 2)
                    {
                        Load(ShortNameToPath(args[1]));
                    }
                    else
                    {
                        Load(DefaultSavePath);
                    }
                    break;
                case "play" or "start" or "p":
                    Play();
                    break;
                case "stop" or "stp":
                    Stop();
                    break;
                case "clear" or "clr" or "cls":
                    engineBridge.ClearRules();
                    engineBridge.New(GridSize.width, GridSize.height);
                    break;
                case "exit":
                    this.Close();
                    break;
                default:
                    engineBridge.CreateRule(input);
                    break;
            }
            saveToHistory(CommandInterface.Text);
            CommandInterface.Text = "";
            historyElement = 0;
        }



        if (e.Key == Key.Escape)
        {
            Keyboard.ClearFocus();
            // Keyboard.Focus(ZoomViewBox);ZoomViewBox
            ZoomViewBox.Focus();
        }
        
    }

    private string ShortNameToPath(string shortName)
    {
        return $@"{SaveDirFullPath}\{shortName}.json";
    }
    
    private void ToggleStart()
    {
        if(isPlaying) Stop();
        else Play();
    }
    
    private void Play()
    {
        timer.Start();
        isPlaying = true;
        StopPlayMenuItem.Header = "_Stop";
        _bus.Publish<bool>("isPlaying", true);
    }
    
    private void Stop()
    {
        timer.Stop();
        isPlaying = false;
        StopPlayMenuItem.Header = "_Start";
        _bus.Publish<bool>("isPlaying", false);
    }

    private void saveToHistory(string command)
    {
        commandHistory.Insert(0, command);
        commandHistory = commandHistory.TakeLast(100).ToList();
        _bus.Publish<List<string>>("commandHistory", commandHistory);
    }

    private void Step_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = selectedRule != string.Empty;
    }
    private void Step_Execute(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
    {
        engineBridge.Advance();
    }
    
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        ZoomModel transformation = new ZoomModel()
        {
            _zoom = 0,
            offsetX = 0,
            offsetY = 0
        };
        switch (e.Key)
        {
            case Key.Up:
                transformation.offsetY += 1;
                break;
            case Key.Down:
                transformation.offsetY -= 1;
                break;
            case Key.Left:
                transformation.offsetX += 1;
                break;
            case Key.Right:
                transformation.offsetX -= 1;
                break;
            case Key.Escape:
                Keyboard.ClearFocus();
                return;
            default:
                return;
        }
        ZoomModel newZoom = new ZoomModel()
        {
            _zoom = zoom._zoom + transformation._zoom,
            offsetX = zoom.offsetX + transformation.offsetX * ZOOM_SPEED * zoom._zoom,
            offsetY = zoom.offsetY + transformation.offsetY * ZOOM_SPEED * zoom._zoom
        };
        _bus.Publish<ZoomModel>("zoom", newZoom);
        e.Handled = true;
    }

    private void ZoomSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        _bus.Publish<ZoomModel>("zoom", new ZoomModel()
        {
            _zoom = e.NewValue,
            offsetX = 0,
            offsetY = 0
        });
    }

    private void IncreaseZoom(object sender, ExecutedRoutedEventArgs e)
    {
        _bus.Publish<ZoomModel>("zoom", zoom with { _zoom = zoom._zoom + 0.3 });
    }

    private void DecreaseZoom(object sender, ExecutedRoutedEventArgs e)
    {
        _bus.Publish<ZoomModel>("zoom", zoom with { _zoom = zoom._zoom - 0.3 });
    }

    private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        Keyboard.ClearFocus();
        ZoomViewBox.Focus();
        // Keyboard.Focus(sender as UIElement);
    }

    private void FocusCommand(object sender, ExecutedRoutedEventArgs e)
    {
        Keyboard.ClearFocus();
        CommandInterface.Focus();
    }

    private void OnSave(object sender, ExecutedRoutedEventArgs e)
    {
        this.Save(DefaultSavePath);
    }

    private void OnLoad(object sender, ExecutedRoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.DefaultExt = ".json";
        dialog.Filter = "save file (*.json)|*.json";
        dialog.InitialDirectory = SaveDirFullPath;
        dialog.RestoreDirectory = true;
        var res = dialog.ShowDialog();
        if (res == System.Windows.Forms.DialogResult.OK)
        {
            Load(dialog.FileName);
        }
    }


    
    private void CommandInterface_OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Up)
        {
            OlderCommandFromHistory();
        }

        if (e.Key == Key.Down)
        {
            NewerCommandFromHistory();
        }
    }

    private void NewerCommandFromHistory()
    {
        if(commandHistory.Count == 0) return;
        historyElement--;
        if (historyElement <= 0)
        {
            historyElement = 0;
            CommandInterface.Text = currentCommand;
        }
        else
        {
            CommandInterface.Text = commandHistory[historyElement-1];
        }
    }

    private void OlderCommandFromHistory()
    {
        if(commandHistory.Count == 0) return;
        if (historyElement == 0) currentCommand = CommandInterface.Text;
        historyElement++;
        if(historyElement >= commandHistory.Count) historyElement = commandHistory.Count;
        CommandInterface.Text = commandHistory[historyElement-1];
    }

    private void UpdateMenuDisplayCheck()
    {
        
    }
    
    private void DisplayNumbersCheck_OnClick(object sender, RoutedEventArgs e)
    {
        _bus.Publish<CellDisplay>("cellDisplay", CellDisplay.Number);
    }

    private void DisplayArrowsCheck_OnClick(object sender, RoutedEventArgs e)
    {
       _bus.Publish<CellDisplay>("cellDisplay", CellDisplay.Arrow);
    }

    private void DisplayBooleanCheck_OnClick(object sender, RoutedEventArgs e)
    {
        _bus.Publish<CellDisplay>("cellDisplay", CellDisplay.Boolean);
    }

    private void NewCommand(object sender, ExecutedRoutedEventArgs e)
    {
        Public.GridType[] gridTypes = new [] //NOTE: coupling with dialog window
        {
            Public.GridType.Checked,
            Public.GridType.Wrapped,
        };
        var dialog = new NewGridDialog
        {
            Width =
            {
                Text = (GridSize.width).ToString()
            },
            Height =
            {
                Text = (GridSize.height).ToString()
            },
            GridType =
            {
                SelectedIndex = selectedBoardType.Equals(Public.GridType.Checked) ? 0 : 1,
            }
        };
        dialog.ShowDialog();
        var size = (int.Parse(dialog.Width.Text), int.Parse(dialog.Height.Text));
        var type = gridTypes[dialog.GridType.SelectedIndex];
        _bus.Publish<Public.GridType>("board_type", type);
        engineBridge.New(size.Item1, size.Item2);
    }

    private void PlayExecute(object sender, ExecutedRoutedEventArgs e)
    {
        ToggleStart();
    }

    private void SaveAs(object sender, ExecutedRoutedEventArgs e)
    {
        var dialog = new SaveFileDialog();
        dialog.DefaultExt = ".json";
        dialog.Filter = "save file (*.json)|*.json";
        dialog.InitialDirectory = SaveDirFullPath;
        dialog.RestoreDirectory = true;
        var res = dialog.ShowDialog();
        if (res == System.Windows.Forms.DialogResult.OK)
        {
            Save(dialog.FileName);
        }
    }
}