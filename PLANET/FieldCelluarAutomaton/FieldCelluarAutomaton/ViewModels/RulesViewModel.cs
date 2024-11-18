using System.Numerics;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComplexMathLibrary;
using FieldCelluarAutomaton.Core;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;

namespace FieldCelluarAutomaton.ViewModels;

public partial class RulesViewModel : ObservableObject
{
    IBus _bus;

    [ObservableProperty]
    public FSharpMap<string, Public.RuleInfo> rules;

    private string _selectedRule;
    
    //TODO: foolproof this
    public Public.RuleInfo SelectedRule
    {
        get => rules.GetValueOrDefault(_selectedRule, null);
        set => _bus.Publish<string>("selectedRule", value.RuleId);
    }

    public RulesViewModel(IBus bus)
    {
        _bus = bus;
        _bus.Subscribe<string>("selectedRule", s =>
        {
            _selectedRule = s;
            OnPropertyChanged(nameof(SelectedRule));
        });
        
        rules = ComplexMathLibrary.Public.rulesetInfo;
        _bus.Subscribe<Event>("ruleset_updated", s =>
        {
            rules = ComplexMathLibrary.Public.rulesetInfo;
            OnPropertyChanged(nameof(rules));
        });
    }

    public void ApplySelectedRule()
    {
        //TODO: QoL, apply selected rule on double click
    }
    
    [RelayCommand]
    void Delete(string key)
    {
        ComplexMathLibrary.Public.ruleset.Remove(key);
    }
}