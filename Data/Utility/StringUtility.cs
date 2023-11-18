using System.Text.RegularExpressions;

namespace Data.Utility;

public static class StringUtility
{
    public static string En2Fa(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        return input
            .Replace('0', '۰')
            .Replace('1', '۱')
            .Replace('2', '۲')
            .Replace('3', '۳')
            .Replace('4', '۴')
            .Replace('5', '۵')
            .Replace('6', '۶')
            .Replace('7', '۷')
            .Replace('8', '۸')
            .Replace('9', '۹');
    }

    public static string Fa2En(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        return input
            .Replace('۰', '0')
            .Replace('۱', '1')
            .Replace('۲', '2')
            .Replace('۳', '3')
            .Replace('۴', '4')
            .Replace('۵', '5')
            .Replace('۶', '6')
            .Replace('۷', '7')
            .Replace('۸', '8')
            .Replace('۹', '9')

            .Replace('٠', '0')
            .Replace('١', '1')
            .Replace('٢', '2')
            .Replace('٣', '3')
            .Replace('٤', '4')
            .Replace('٥', '5')
            .Replace('٦', '6')
            .Replace('٧', '7')
            .Replace('٨', '8')
            .Replace('٩', '9');
    }

    public static string FixPersianChars(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        return input
            .Replace('ك', 'ک')  // تصحیح ک
            .Replace('ي', 'ی')  // تصحیح ی
            .Replace('ة', 'ه'); // تصحیح ه
    }

    public static string CleanString(this string input)
    {
        input = input.Trim();
        input = FixPersianChars(input);
        input = Fa2En(input);

        return input;
    }

    public static string RemoveDulpcateCharacter(string input, char str)
    {
        if (!input.Contains($"{str}{str}"))
        {
            return input;
        }
        else
        {
            input = input.Replace($"{str}{str}", $"{str}");
            return RemoveDulpcateCharacter(input, str);
        }
    }

    public static string TrimFileName(string filename)
    {
        if (string.IsNullOrEmpty(filename))
        {
            return null;
        }

        var trimedName = filename
            .ToLowerInvariant()
            .CleanString()
            .Replace(' ', '-')
            .Replace('‌', '-')
            .Replace('/', '-')
            .Replace('[', '-')
            .Replace(']', '-')
            .Replace('{', '-')
            .Replace('}', '-')
            .Replace('(', '-')
            .Replace(')', '-')
            .Replace('!', '-')
            .Replace(':', '-')
            .Replace('*', '-')
            .Replace('?', '-')
            .Replace('<', '-')
            .Replace('>', '-')
            .Replace('|', '-')
            .Replace('%', '-')
            .Replace('~', '-')
            .Replace('!', '-')
            .Replace('@', '-')
            .Replace('#', '-')
            .Replace('$', '-')
            .Replace('^', '-')
            .Replace('&', '-')
            .Replace('+', '-')
            .Replace('=', '-')
            .Replace(',', '-')
            .Replace(';', '-')
            .Replace('`', '-')
            .Replace('\\', '-')
            .Replace('\"', '-')
            .Replace('\'', '-');

        return RemoveDulpcateCharacter(trimedName, '-').TrimEnd('-');
    }

    public static string TrimUrl(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return null;
        }

        var trimedName = input
            .ToLowerInvariant()
            .CleanString()
            .Replace(' ', '-')
            .Replace('‌', '-')
            .Replace('[', '-')
            .Replace(']', '-')
            .Replace('{', '-')
            .Replace('}', '-')
            .Replace('(', '-')
            .Replace(')', '-')
            .Replace('!', '-')
            .Replace('<', '-')
            .Replace('>', '-')
            .Replace('|', '-')
            .Replace('%', '-')
            .Replace('~', '-')
            .Replace('!', '-')
            .Replace('^', '-')
            .Replace('+', '-')
            .Replace('`', '-')
            .Replace('\\', '-')
            .Replace('\"', '-')
            .Replace('\'', '-');

        return RemoveDulpcateCharacter(trimedName, '-').TrimEnd('-');
    }

    public static string SanitizeHtmlTags(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return null;
        }

        return Regex.Replace(input, "<.*?>", string.Empty);
    }

    public static string SanitizeHtmlCharacters(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return null;
        }

        var trimedInput = input
            .Replace("&zwnj;", "‌")
            .Replace("&nbsp;", " ")
            .Replace("&raquo;", "»")
            .Replace("&laquo;", "«");

        return trimedInput;
    }

    public static string Summary(string input, int lenght = 160)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        input = SanitizeHtmlTags(input);

        if (input.Length < lenght)
        {
            return input;
        }
        else
        {
            var result = input[..lenght];
            return result + " ...";
        }
    }
}
