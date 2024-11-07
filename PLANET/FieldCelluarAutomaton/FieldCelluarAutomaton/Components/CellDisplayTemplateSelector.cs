using System.Windows;
using System.Windows.Controls;

namespace FieldCelluarAutomaton.Components;

public class CellDisplayTemplateSelector : DataTemplateSelector
{
    public DataTemplate NumberTemplate { get; set; }
    public DataTemplate BooleanTemplate { get; set; }
    public DataTemplate ArrowTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is CellDisplay display)
        {
            return display switch
            {
                CellDisplay.Number => NumberTemplate,
                CellDisplay.Boolean => BooleanTemplate,
                CellDisplay.Arrow => ArrowTemplate,
                _ => ArrowTemplate
            };
        }
        return ArrowTemplate;
    }
    
}