using UnityEngine;

public class SGSecurity
{
    public delegate void EventChanges();
    public static event EventChanges TermsUseChanged = null;

    private const string TERMSUSE = "Terms Use";

    public static bool TermsUse {
        get {
            string termsUse = SGStorage.GetGenericPref(TERMSUSE);
            if (termsUse == "1")
                return true;
            else
                return false;
        }
        set {
            SGStorage.SetGenericPref(TERMSUSE, value ? "1" : "0");
            // event
            TermsUseChanged?.Invoke();
        }
    }

    public static bool CheckGenuine()
    {
        if (Application.genuineCheckAvailable)
            return Application.genuine;
        else
            return false;
    }

}
