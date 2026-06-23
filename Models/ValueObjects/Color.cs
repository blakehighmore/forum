using System.Text.RegularExpressions;


namespace backend.Models.ValueObjects;

public record Color
{
    public string Value { get; }

    private Color(string value)
    {
        Value = value;
    }

    public static Color Create(string colorHex)
    {
        if (!Regex.IsMatch(colorHex, "^#[0-9A-Fa-f]{6}$"))
            throw new ArgumentException("Неправильный hex", nameof(colorHex));
        Color color = new(colorHex.ToUpper());

        return color;
    }
}