public class SGTemplateModels
{
    public delegate void EventChanges();
    public static event EventChanges SomethingChanged = null;

    // data
    private static float privateData = 0f;

    /// <summary>
    /// Event implementation for changes on data
    /// </summary>
    public static float Data {
        get {
            return privateData;
        }
        set {
            privateData = value;
            // event
            SomethingChanged?.Invoke();
        }
    }
}
