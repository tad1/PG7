using Application;
using Infrastructure;
using Terminal.Gui;

DomainContext _context = new DomainContext();
People _people = new People(_context);
Employments _employments = new Employments(_context);
Companies _companies = new Companies(_context);

Terminal.Gui.Application.Init();

Terminal.Gui.Application.Run<MainWindow>().Dispose();

Terminal.Gui.Application.Shutdown();

public sealed class MainWindow : Window
{
    public static string Command;

    public MainWindow()
    {
        Title = "UI";
        
        var cli = new TextField()
        {
            Width = Dim.Fill(),
        };
        Add(cli);
    }
}