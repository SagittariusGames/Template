using UnityEngine;

public class SGUICopyPaste : MonoBehaviour
{
    public void Copy (string message)
    {
        Copy(message, 0, message.Length);
        Debug.LogWarning(message);
    }
    public void Copy(string message, int firstCharacterIndex, int characterCount)
    {
        // clipboard
        SGEnvironment.SetClipboard(message);
    }
}
