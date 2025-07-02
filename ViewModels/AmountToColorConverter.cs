using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace BudzetDomowyMinimal.ViewModels;

public class AmountToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is decimal amount)
        {
            if (amount > 0) return Brushes.Green;
            if (amount < 0) return Brushes.Red;
        }
        return Brushes.Black;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
} 