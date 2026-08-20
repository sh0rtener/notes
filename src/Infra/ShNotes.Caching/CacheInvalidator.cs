namespace ShNotes.Caching;

public class CacheInvalidator
{
    private static CacheInvalidator instance;
    private static object syncRoot = new Object();
    public CancellationTokenSource AddCacheCts { get; set; } = new();

    private CacheInvalidator() { }

    public static CacheInvalidator GetInstance()
    {
        if (instance == null)
        {
            lock (syncRoot)
            {
                if (instance == null)
                {
                    instance = new CacheInvalidator();
                }
            }
        }

        return instance;
    }
}
