public static class ExtensionMethods
{
    public static List<T> Shuffle<T>(this IEnumerable<T> source)
    {
        Random rng = new Random();
        List<T> list = source.ToList();

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }
}
