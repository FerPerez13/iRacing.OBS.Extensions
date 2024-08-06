using System.Windows.Media;

namespace WpfApp3.Helpers;

// Convertir una cadena hexadecimal en un pincel
public static class BrushExtension
{
    public static Brush ToBrush(this string hex)
    {
        var converter = new BrushConverter();
        return (Brush)converter.ConvertFromString(hex);
    }
}