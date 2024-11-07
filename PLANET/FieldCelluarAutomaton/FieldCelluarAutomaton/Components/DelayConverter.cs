using System.Globalization;
using System.Windows.Data;

namespace FieldCelluarAutomaton.Components;

public class DelayConverter : IValueConverter
{
    private double _previousValue = 0;
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        double currentAngle = (double)value;
        double returnValue = _previousValue;
        // Store the current value as the previous value for the next conversion
        _previousValue = currentAngle;
        return 0.0;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}