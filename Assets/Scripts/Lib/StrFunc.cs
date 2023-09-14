/// <summary>
/// A Libary of generic string functions that are either more efficient than or unavailable in standard C#.
/// </summary>
///

public static class StrFunc
{
    /// <summary>
    /// Returns whether one string param Ends with the other.  More efficient than String.EndsWith().
    /// Case sensitive.
    /// From https://docs.unity3d.com/2020.3/Documentation/Manual/BestPracticeUnderstandingPerformanceInUnity5.html
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool EndsWith(this string a, string b)
    {
        int ap = a.Length - 1;
        int bp = b.Length - 1;

        while (ap >= 0 && bp >= 0 && a[ap] == b[bp])
        {
            ap--;
            bp--;
        }

        return (bp < 0);
    }

    /// <summary>
    /// Returns whether one string param Starts with the other.  More efficient than String.StartsWith().
    /// Case sensitive.
    /// From https://docs.unity3d.com/2020.3/Documentation/Manual/BestPracticeUnderstandingPerformanceInUnity5.html
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool StartsWith(this string a, string b)
    {
        int aLen = a.Length;
        int bLen = b.Length;

        int ap = 0; int bp = 0;

        while (ap < aLen && bp < bLen && a[ap] == b[bp])
        {
            ap++;
            bp++;
        }

        return (bp == bLen);
    }
}



