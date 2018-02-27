namespace Unscientificlab.Logging
{
    public static class Logger
    {
        public static IDebug Instance = new NullDebug ();
    }
}
