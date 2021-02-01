public class SGLanguage
{
    public delegate void EventChanges();
    public static event EventChanges LanguageChanged = null;

    public static string fileXML { get; set; } = null;
    private static string language = null;

    public static string Language {
        get {
            return language;
        }
        set {
            language = value;
            // event
            LanguageChanged?.Invoke();
        }
    }
}
