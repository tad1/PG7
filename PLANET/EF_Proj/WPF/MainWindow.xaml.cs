using System.CommandLine;
using System.CommandLine.Completions;
using System.CommandLine.Parsing;
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
using Application;
using Domain;
using Infrastructure;

namespace WPF;

enum Table
{
    People,
    Employments,
}

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    DomainContext _context = new DomainContext();
    People _people;
    Employments _employments;
    Generator _generator;
    Queries _queries;

    private Command _parser;
    string currentCommand = "";
    private int completionNo = 0;
    
    Table _currentTable = Table.People;
    
    public MainWindow()
    {
        _context.Database.EnsureCreated();
        _people = new People(_context);
        _employments = new Employments(_context);
        _generator = new Generator(_context);
        _queries = new Queries(_context);
        
        InitializeComponent();

        var tableSelect = new Command("table");
        var tableArgument = new Argument<Table>("table_name");
        tableSelect.AddArgument(tableArgument);
        tableSelect.SetHandler(SelectTable, tableArgument);
        var generate = new Command("generate");
        {
            var nPeople = new Argument<int>("nPeople");
            var nCompanies = new Argument<int>("nCompanies");
            generate.AddArgument(nPeople);
            generate.AddArgument(nCompanies);
            generate.SetHandler((n_people, n_employments) =>
            {
                _generator.Generate(n_people, n_employments);
                DisplayTable(_currentTable);
            }, nPeople, nCompanies);
        }
        var clear = new Command("clear");
        clear.SetHandler(() =>
        {
            _generator.Clean();
            DisplayTable(_currentTable);
        });
        var delete = new Command("delete");
        var guidArg = new Argument<string>("guid");
        delete.AddArgument(tableArgument);
        delete.AddArgument(guidArg);
        delete.SetHandler((table, guid) =>
        {
            switch (table)
            {
                case Table.People:
                    _people.DeletePerson(Guid.Parse(guid));
                    break;
                case Table.Employments:
                    _employments.RemoveEmployment(Guid.Parse(guid));
                    break;
            }
            DisplayTable(_currentTable);
        },tableArgument, guidArg);
        
        var query = new Command("query");
        var mostGrandChildren = new Command("mostGrandChildren");
        var genderArgument = new Argument<Gender>("gender");

        mostGrandChildren.SetHandler((WithMostGrandChildren), genderArgument);
        mostGrandChildren.AddArgument(genderArgument);
        
        var averageSalaryPerType = new Command("averageSalaryPerType");
        averageSalaryPerType.SetHandler((GetAvgCompanies));
        
        var personWithRichestFamily = new Command("personWithRichestFamily");
        personWithRichestFamily.SetHandler((GetPersonWithRichestFamily));
        
        query.AddCommand(mostGrandChildren);
        query.AddCommand(averageSalaryPerType);
        query.AddCommand(personWithRichestFamily);
        
        
        _parser = new Command("app");
        _parser.AddCommand(tableSelect);
        _parser.AddCommand(generate);
        _parser.AddCommand(clear);
        _parser.AddCommand(query);
        _parser.AddCommand(delete);
        
        DisplayTable(_currentTable);
    }

    private void SelectTable(Table obj)
    {
        _currentTable = obj;
        DisplayTable(_currentTable);
    }

    private void DisplayTable(Table table)
    {
        DataGrid.ItemsSource = table switch
        {
            Table.People => _people.GetPeople()
                .Select(person => new
                {
                    GUID = person.Id,
                    Name = person.Name,
                    Surname = person.Surname,
                    PhoneNumbers =
                        person.PhoneNumbers.Aggregate("", (current, phoneNumber) => current + phoneNumber.Number) +
                        ", ",
                    Employments = person.Employments,
                    Fortune = (person as PublicPerson)?.Fortune ?? 0,
                    FatherID = person.Father?.Id,
                    MotherID = person.Mother?.Id,
                    SpouseID = person.Spouse?.Id,
                }),
            Table.Employments => _employments.GetEmployments()
                .Select(employment => new
                {
                    GUID = employment.Id,
                    EmploymentType = employment.EmploymentType.Name,
                    Salary = employment.Salary,
                    CompanyName = employment.CompanyName,
                    PersonID = employment.Person.Id,
                }),
            _ => DataGrid.ItemsSource
        };
    }
    
    private void GetPersonWithRichestFamily()
    {
        DataGrid.ItemsSource = _queries.GetPersonWithRichestFamily();
    }

    private void GetAvgCompanies()
    {
        DataGrid.ItemsSource = _queries.GetAvgCompanies();
    }

    private void WithMostGrandChildren(Gender gender)
    {
        DataGrid.ItemsSource = (dynamic[])[_queries.WithMostGrandChildren(gender)];
    }

    private string RemoveLastWord(string input)
    {
        return input.Trim(' ').Remove(input.LastIndexOf(' ') + 1);
    }
    private void HandleAutoComplete()
    {
        ParseResult result = null;
        var position = CommandLine.CaretIndex;
        var parseResult = _parser.Parse(CommandLine.Text);
        var completions = parseResult.GetCompletions(position);
        
        
        if (completions.Count() == 1)
        {
            var newStr = RemoveLastWord(CommandLine.Text) + completions.First().Label;
            CommandLine.Text = newStr;
            CommandLine.CaretIndex = newStr.Length;
        }
        foreach (var completionItem in completions)
        {
            Console.WriteLine(completionItem);
        }
    }
    
    private void CommandLine_OnKeyDown(object sender, KeyEventArgs e)
    {
        var args = CommandLine.Text.Split(' ');
        if (e.Key == Key.Tab)
        {
            HandleAutoComplete();
            e.Handled = true;
        }
        if (e.Key != Key.Enter) return;

        _parser.Invoke(args);
        
        CommandLine.Text = "";
    }

    private void DataGrid_OnCopyingRowClipboardContent(object? sender, DataGridRowClipboardEventArgs e)
    {
        var currentCell = e.ClipboardRowContent[ DataGrid.CurrentCell.Column.DisplayIndex];
        e.ClipboardRowContent.Clear();
        e.ClipboardRowContent.Add( currentCell );
    }
}