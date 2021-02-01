public class SGStatus
{
    public delegate void EventChanges();
    public static event EventChanges PauseChanged = null;

    private static bool pause = false;
 
    /// <summary>
    /// 
    /// </summary>
    public static bool Pause {
        get {
            return pause;
        }
        set {
            pause = value;
            // event
            PauseChanged?.Invoke();
        }
    }
}
