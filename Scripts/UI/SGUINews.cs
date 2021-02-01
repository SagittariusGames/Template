using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;

public class SGUINews : MonoBehaviour
{
    public Text textNews = null;
    //public string dataseURL = "";
    public string referenceKey = "news";
    public string recordSetKey = "";
    public string placeholderTitle = "{title}";
    public string placeholderDesc = "{description}";
    public string placeholderDate = "{date}";
    public string placeholderAuthor = "{author}";

    private DatabaseReference referenceRTD = null;

    private string textOriginal = "";

    private bool firstAttempt = true;

    // Start is called before the first frame update
    void Start()
    {
        textOriginal = textNews.text;
        textNews.text = "";
    }

    void Update()
    {
        if (firstAttempt && SGFirebase.SetupReady)
        {
            Setup();
            firstAttempt = false;
        }
    }

    void OnDisable()
    {
        if (referenceRTD != null)
            referenceRTD.ValueChanged -= ValueChanged;
    }

    private void Setup()
    {
        if (string.IsNullOrEmpty(recordSetKey))
            referenceRTD = SGFirebase.RealTimeDatabase(referenceKey);
        else
            referenceRTD = SGFirebase.RealTimeDatabase(referenceKey + "/" + recordSetKey);
        if (referenceRTD == null)
            return;

        referenceRTD.ValueChanged += ValueChanged;
    }

    private void ValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            SGDebug.LogWarning("Realtime Database: " + args.DatabaseError.Message);
            return;
        }

        UpdateValues(args.Snapshot);
    }

    private void UpdateValues (DataSnapshot snapshot)
    {
        double timestamp;

        textNews.text = textOriginal;

        foreach (var childSnapshot in snapshot.Children)
        {
            switch (childSnapshot.Key.ToString())
            {
                case "title":
                    textNews.text = Placeholder(textNews.text, childSnapshot.Value.ToString(), placeholderTitle);
                    break;
                case "description":
                    textNews.text = Placeholder(textNews.text, childSnapshot.Value.ToString(), placeholderDesc);
                    break;
                case "author":
                    textNews.text = Placeholder(textNews.text, childSnapshot.Value.ToString(), placeholderAuthor);
                    break;
                case "timestamp":
                    if (double.TryParse(childSnapshot.Value.ToString(), out timestamp))
                    {
                        System.DateTime datetime = SGDateTime.UnixTimeStampToDateTime(timestamp);
                        textNews.text = Placeholder(textNews.text, datetime.ToString(System.Globalization.CultureInfo.InvariantCulture), placeholderDate);
                    }
                    break;
            }
        }
    }

    private string Placeholder(string oldText, string newText, string placeholder)
    {
        string text = oldText;

        if (placeholder.Length > 0)
        {
            text = oldText.Replace(placeholder, newText);
        }

        return text;
    }
}
