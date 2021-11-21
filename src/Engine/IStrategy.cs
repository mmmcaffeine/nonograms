namespace Dgt.Nonograms.Engine;

public interface IStrategy
{
    Line Execute(string hint, string line)
    {
        Hint parsedHint;
        Line parsedLine;

        try
        {
            parsedHint = Hint.Parse(hint);
        }
        catch (FormatException fe)
        {
            throw CreateParsingException(nameof(hint), hint, fe);
        }

        try
        {
            parsedLine = Line.Parse(line);
        }
        catch (FormatException fe)
        {
            throw CreateParsingException(nameof(line), line, fe);
        }

        return Execute(parsedHint, parsedLine);
    }

    private static Exception CreateParsingException(string paramName, string paramValue, FormatException innerException)
    {
        var message = $"Value could not be parsed into a {paramName}. Actual value was '{paramValue}'.";

        return new ArgumentException(message, paramName, innerException)
        {
            Data = { { paramName, paramValue } }
        };
    }
    
    Line Execute(Hint hint, Line line);
}
