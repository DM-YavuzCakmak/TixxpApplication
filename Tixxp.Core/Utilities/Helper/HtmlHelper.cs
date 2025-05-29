namespace Tixxp.Core.Utilities.Helper;

public static class HtmlHelper
{
    public static string ToOpenClosed(this bool value)
    {
        return value ? "Açık" : "Kapalı";
    }
}
