using UnityEngine;
using Firebase.Crashlytics;

public class CrashlyticsTester : MonoBehaviour
{
    int updatesBeforeException;

    // Use this for initialization
    void Start()
    {
        updatesBeforeException = 0;
        SGFirebase.Setup();
        Crashlytics.SetCustomKey("chave", "valor");
        Crashlytics.Log("Log de erros aparece aqui");
    }

    // Update is called once per frame
    void Update()
    {
        // Call the exception-throwing method here so that it's run
        // every frame update
        throwExceptionEvery60Updates();
    }

    // A method that tests your Crashlytics implementation by throwing an
    // exception every 60 frame updates. You should see non-fatal errors in the
    // Firebase console a few minutes after running your app with this method.
    void throwExceptionEvery60Updates()
    {
        if (updatesBeforeException > 0)
        {
            updatesBeforeException--;
        }
        else
        {
            // Set the counter to 60 updates
            updatesBeforeException = 60;

            // Throw an exception to test your Crashlytics implementation
            throw new System.Exception("test exception please ignore");
        }
    }
}
