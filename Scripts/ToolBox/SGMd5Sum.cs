
public class SGMd5Sum
{
    private static string[] hashSpearator = { "@=>" };

    /// <summary>
    /// Get the MD5 hash for the string
    /// </summary>
    public static string Sum(string strToHash)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToHash);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

    public static string Sign(string data)
    {
        string sign;
        string hash;

        hash = Sum(data);
        sign = hash + hashSpearator[0] + data;

        return sign;
    }

    public static string Validate(string dataSign)
    {
        string[] split;
        string hash;
        string data;

        if (string.IsNullOrEmpty(dataSign))
            return "";

        split = dataSign.Split(hashSpearator, System.StringSplitOptions.RemoveEmptyEntries);
        data = split[1];
        hash = Sum(data);
        if (hash != split[0])
            return "";

        return data;
    }
}
