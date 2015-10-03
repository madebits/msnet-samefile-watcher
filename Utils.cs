using System;
using System.Text;
using System.Collections;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace dsw
{
	/// <summary>
	/// 
	/// </summary>
	internal class Utils
	{
		internal static Config Conf = new Config();

		internal static bool ContainsWCopy(string s)
		{
			if(s == null) return true;
			if(s.ToUpper().IndexOf(Config.WCOPY) >= 0) return true;
			return false;
		}
		
		internal static string CorrectDirPath(string dir)
		{
			return CorrectFullDirPath(Path.GetFullPath(dir));
		}

		internal static string CorrectFullDirPath(string dir)
		{
			if(!dir.EndsWith("\\"))
			{
				dir += "\\";
			}
			return dir;
		}
		
		internal static string ArrayList2String(string prefix, ArrayList d)
		{
			if(d == null) return string.Empty;
			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < d.Count; i++)
			{
				sb.Append(prefix).Append((string)d[i]).Append(Environment.NewLine);
			}
			return sb.ToString();
		}

	}//EOC

	internal class Config
	{
		private bool logEvents = false;
		private bool processEvents = true;

		private bool logDSWSEvents = false;
		private bool printErrorStack = false;
		private bool logFileReindex = false;


		internal static readonly string WCOPY = "_DSWRCOPY";

		internal bool LogFileReindex
		{
			get{ return logFileReindex; }
			set { logFileReindex = value; }
		}

		internal bool LogEvents
		{
			get{ return logEvents; }
			set { logEvents = value; }
		}

		internal bool ProcessEvents
		{
			get{ return processEvents; }
			set { processEvents = value; }
		}

		internal bool LogDSWSEvents
		{
			get{ return logDSWSEvents; }
			set { logDSWSEvents = value; }
		}

		internal bool PrintErrorStack
		{
			get{ return printErrorStack; }
			set { printErrorStack = value; }
		}

	}//EOC	

	internal class Log
	{
		private System.Windows.Forms.RichTextBox txtLog;
		private Queue logMsgs = null;
		private AutoResetEvent logEvent = null;
		private System.Int32 count = 0;
		private StreamWriter logFile = null;
		private bool countOn = false;

		internal Log(System.Windows.Forms.RichTextBox t)
		{
			txtLog = t;
			InitLog();
		}

		internal bool CountOn
		{
			get { return countOn; }
			set
			{ 
				if(value) count = 0;
				countOn = value;
			}
		}

		private void InitLog()
		{
			if(logMsgs == null)
			{
				logMsgs = new Queue();
				logEvent = new AutoResetEvent(false);
				Thread th = new Thread(new ThreadStart(ProcessLogEvents));
				th.IsBackground = true;
				th.Start();
			}
		}

		internal void SetFileLog(bool on)
		{
			string lf = GetLogFile();
			if(on)
			{
				logFile = new StreamWriter(lf, true);
				logFile.WriteLine(DateTime.Now.ToString("#DSW-FILELOG @ yyyy-MM-dd HH:mm:ss"));
			}
			else
			{
				if(logFile != null)
				{
					logFile.Close();
					logFile = null;
				}
			}
		}

		internal static string GetLogFile()
		{
			return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "dswlog.txt");
		}

		internal void LogMsg(string s)
		{
			if(CountOn)
			{
				Interlocked.Increment(ref count);
				if(count > Int32.MaxValue) count = 1;
				AddLogEvent("" + count + "  " + s);
			}
			else AddLogEvent(s);
		}

		private void AddLogEvent(string s)
		{
			lock(logMsgs.SyncRoot)
			{
				logMsgs.Enqueue(s);
			}
			logEvent.Set();
		}

		private string GetLogEvent()
		{
			string s = null;
			while(true)
			{
				s = null;
				lock(logMsgs.SyncRoot)
				{
					if(logMsgs.Count > 0)
						s = (string)logMsgs.Dequeue();
				}
				if(s != null) return s;
				else logEvent.WaitOne();
			}
		}

		private void ProcessLogEvents()
		{
			string s = null;
			while(true)
			{
				s = GetLogEvent(); // block until avaiable
				try
				{
					ProcessLogEvent(s); 
				} 
				catch//(Exception ex1)
				{
					//ShowErr(ex1.Message);
				}
			}
		}

		private void ProcessLogEvent(string s)
		{
			try
			{
				if(logFile != null)
				{
					logFile.WriteLine(s);
					logFile.Flush();
				}
			} 
			catch//(Exception ex1)
			{
				//ShowErr(ex1.Message);
			}
			try
			{
				if(txtLog == null) return;
				if(txtLog.Text.Length > 10 * 1024 * 1024) txtLog.Text = "...\r\n";
				txtLog.AppendText(s + Environment.NewLine);
			} 
			catch {}
			//this.Invoke(new LogHandler(_LogMsg), new object[]{s});
		}

	} //EOC

	internal class DSWEvents
	{
		private Form1 handle = null;
		private Queue events = null;
		private AutoResetEvent getEvent = null;
		private Thread th = null;
		private Optimizer optimizer = null;

		internal DSWEvents(Form1 handle)
		{
			this.handle = handle;
			InitEvents();
		}

		private void InitEvents()
		{
			optimizer = new Optimizer();
			events = new Queue();
			getEvent = new AutoResetEvent(false);
			th = new Thread(new ThreadStart(ProcessEvents));
			th.IsBackground = true;
			th.Start();
		}

		internal void Stop()
		{
			/*
			try
			{
				if(th != null) th.Abort();
			} 
			catch{}
			*/
		}

		internal void AddEvent(WEvent we)
		{
			//LogMsg(" -> AddEvent " + we.ToString());
			lock(events.SyncRoot)
			{
				events.Enqueue(we);
			}
			getEvent.Set();
			//LogMsg(" <- AddEvent");
		}

		private WEvent GetEvent()
		{
			//LogMsg(" -> GetEvent");
			WEvent we = null;
			while(true)
			{
				we = null;
				lock(events.SyncRoot)
				{
					if(events.Count > 0)
						we = (WEvent)events.Dequeue();
				}
				if(we != null) return we;
				else getEvent.WaitOne();
			}
		}

		private void ProcessEvents()
		{
			WEvent we = null;
			while(true)
			{
				we = GetEvent(); // block until avaiable
				try
				{
					if((we.type != WEventType.WatchEvent) || optimizer.CanProcessEvent(we)) //handle.CanProcessEvent(we.file, we.watchType))
					{ 
						handle.ProcessEvent(we); 
					}
				} 
				catch(Exception ex1)
				{
					handle.LogMsg("# Error: " + ex1.Message + " @ " + we.ToString());
					if(Utils.Conf.PrintErrorStack)
						handle.LogMsg(ex1.StackTrace);
				}
			}
		}

	}//EOC

	internal class Help
	{
		internal static readonly string help = string.Empty
			+ "DSW watches a directory for changes and finds files that are the same (same hash).\r\n"
			+ "When such a file is found it can be logged, user can be notified with a dialog, and it can be moved to a subdir (" + Config.WCOPY + ").\r\n"
			+ Environment.NewLine
			+ "When a directory is watched an index is build. You can export it and import it latter.\r\n"
			+ "This allow you to watch a dir without building its file index first, you can import it at any time.\r\n"
			+ "You can import also indices of unrelated directories. To the DSW it will look like these files (virtual entries) are in the directory being watched.\r\n"
			+ "Index operations are valid only after you start watching a directory.\r\n"
			+ Environment.NewLine
			+ "An index of the directory is kept in non-optimized memory. It consumes as mutch as nearly 0.6 (virtual entry) to 1 Kb (real entry) for every file.\r\n"
			+ "As a consequence for directories with many entries you can run out of memory.\r\n"
			+ "Zero length files are not included in watch events but they are part of the index.\r\n"
			+ Environment.NewLine
			+ "File System Events are cached as soon as they arrive. They are processed latter in the order they arrive (they cannot be processed in parallel).\r\n"
			+ "This means that if we receive a file create and a delete event, when file create is processed it may have been already deleted.\r\n"
			+ "In this case an error message file cannot be found will be printed. Usually these messages can be ignored.\r\n"
			+ Environment.NewLine
			+ "In the case of directory changes, directories are re-indexed. This can take some time for long directories.\r\n"
			+ Environment.NewLine
			+ "Screen logging is limited in length (10Mb). Log to file (dswlog.txt in dsw.exe dir) is you want all logs.\r\n"
			+ "Logs are appended so this file length can grow large.\r\n"
			+ Environment.NewLine
			+ "Todo: Apply heuristics not to process all events.\r\n"
			+ Environment.NewLine
			;
	}//EOC

}
