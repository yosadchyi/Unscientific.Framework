namespace Unscientific.Logging
{
	public enum LogLevel
	{
		All,
		Trace,
		Debug,
		Info,
		Warning,
		Error,
		Critical,
		Fatal,
		None
	}

	public interface IDebug
	{
		LogLevel Level {
			get;
			set;
		}

		void Log (LogLevel level, string format);
	    void Log<T1> (LogLevel level, string format, T1 arg1);
	    void Log<T1, T2> (LogLevel level, string format, T1 arg1, T2 arg2);
	    void Log<T1, T2, T3> (LogLevel level, string format, T1 arg1, T2 arg2, T3 arg3);
	    void Log<T1, T2, T3, T4> (LogLevel level, string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
	}
}

