using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sura.Areas.Admin.Infrastructure
{
    public class Helper
    {
        public static List<string> ConvertToTagList(string tags)
        {
            var list = new List<string>();

            if (string.IsNullOrWhiteSpace(tags))
                return list;

            foreach (var tag in tags.Split(','))
            {
                tag.Trim();
                list.Add(ConvertToSlug(tag));
            }

            return list;
        }

        public static string ConvertToSlug(string text)
        {
            text = RemoveDiacritics(text);
            text = text.ToLowerInvariant();
            text = ReplaceNonWordWithDashes(text);
            text = text.Trim(' ', '-');

            return text;
        }

        private static string RemoveDiacritics(string value)
        {
            var stFormD = value.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in stFormD.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
                sb.Append(c);

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        private static string ReplaceNonWordWithDashes(string title)
        {
            title = Regex.Replace(title, "[?'??\"&]{1,}", "", RegexOptions.None);

            var builder = new StringBuilder();
            foreach (var c in title)
                builder.Append(char.IsLetterOrDigit(c) ? c : ' ');

            title = builder.ToString();
            title = Regex.Replace(title, "[ ]{1,}", "-", RegexOptions.None);

            return title;
        } 
    }
}