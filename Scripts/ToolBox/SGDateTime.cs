using UnityEngine;

public class SGDateTime
{
    /// <summary>
    /// Use to countdown seconds in the same scene
    /// </summary>
    public static bool Chronometer(ref float chronometer)
    {
        chronometer -= Time.deltaTime;

        if (chronometer <= 0)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Get the timestamp
    /// </summary>
    public static int IntegerDateTime()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        return cur_time;
    }

    /// <summary>
    /// Use only in FixedUpdate
    /// Get the elapsed time, in format 00:00:00
    /// </summary>
    public static string TimeElapsedUpdate(ref float timeElapsed)
    {
        float timeLeftCurrent;
        int seconds = 0;
        int minutes = 0;
        int hour = 0;

        timeElapsed = timeElapsed + Time.deltaTime;
        timeLeftCurrent = timeElapsed;
        hour = (int)timeLeftCurrent / 60 / 60;
        timeLeftCurrent = timeLeftCurrent - hour * 60 * 60;
        minutes = (int)timeLeftCurrent / 60;
        timeLeftCurrent = timeLeftCurrent - minutes * 60;
        seconds = (int)timeLeftCurrent;
        if (hour > 0)
            return hour.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
        else if (minutes > 0)
            return minutes.ToString("D2") + ":" + seconds.ToString("D2");
        else
            return seconds.ToString("D2");
    }

    /// <summary>
    /// Use only in FixedUpdate
    /// Get the left time, in format 00:00:00
    /// </summary>
    public static string TimeLeftUpdate(ref float timeLeft)
    {
        float timeLeftCurrent;
        int seconds = 0;
        int minutes = 0;
        int hour = 0;

        if (timeLeft > 0)
        {
            timeLeft = timeLeft - Time.deltaTime;
            timeLeftCurrent = timeLeft;
            if (timeLeftCurrent > 0)
            {
                hour = (int)timeLeftCurrent / 60 / 60;
                timeLeftCurrent = timeLeftCurrent - hour * 60 * 60;
                minutes = (int)timeLeftCurrent / 60;
                timeLeftCurrent = timeLeftCurrent - minutes * 60;
                seconds = (int)timeLeftCurrent;
                if (hour > 0)
                    return hour.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
                else if (minutes > 0)
                    return minutes.ToString("D2") + ":" + seconds.ToString("D2");
                else
                    return seconds.ToString("D2");
            }
            else
            {
                return "0";
                // call action time is over!
            }
        }

        return "";
    }

    public static System.DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        System.DateTime datetime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
        datetime = datetime.AddMilliseconds(unixTimeStamp);
        return datetime;
    }

}
