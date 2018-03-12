namespace Unscientific.Logging
{
	public class NullDebug: IDebug
	{
		public LogLevel Level {
			get;
			set;
		}

	    public void Log (LogLevel level, string format)
	    {
	    }

	    public void Log<T1> (LogLevel level, string format, T1 arg1)
	    {
	    }

	    public void Log<T1, T2> (LogLevel level, string format, T1 arg1, T2 arg2)
	    {
	    }

	    public void Log<T1, T2, T3> (LogLevel level, string format, T1 arg1, T2 arg2, T3 arg3)
	    {
	    }

	    public void Log<T1, T2, T3, T4> (LogLevel level, string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
	    {
	    }

	}
}

