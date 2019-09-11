using System;


//This is not my code I was just trying to make sure I understood it.
public static class Extensions
{
    /// <summary>
    /// Get next value in enum.
    /// </summary>
    public static T Next<T>(this T src) where T : struct
    {
        //Check if value is enum.
        if (!typeof(T).IsEnum) throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

        //Get enum values based on enum.
        T[] Arr = (T[])Enum.GetValues(src.GetType());
        //Find the current index of the enum based on value and add it by 1.
        int j = Array.IndexOf(Arr, src) + 1;
        //Check if out of range, if true, return index 0.
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }

    /// <summary>
    /// Get previous value in enum.
    /// </summary>
    public static T Previous<T>(this T src) where T : struct
    {
        //Check if value is enum.
        if (!typeof(T).IsEnum) throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));

        //Get enum values based on enum.
        T[] Arr = (T[])Enum.GetValues(src.GetType());
        //Find the current index of the enum based on value and subtract it by 1.
        int j = Array.IndexOf(Arr, src) - 1;
        //Check if out of range, if true, return highest index.
        return (j < 0) ? Arr[Arr.Length - 1] : Arr[j];
    }
}