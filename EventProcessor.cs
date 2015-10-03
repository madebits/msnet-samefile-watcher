using System;
using System.Text;
using System.Collections;
using System.IO;
using System.Threading;

namespace dsw
{
	internal enum WEventType { WatchEvent, IndexDump, IndexView, Export, Import}

	internal class WEvent
	{
		internal long timestamp = System.DateTime.Now.Ticks;
		internal WEventType type = WEventType.WatchEvent;
		internal string file = null;
		internal string oldFile = null;
		internal WatcherChangeTypes watchType = WatcherChangeTypes.All;
		internal object data = null;
		
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("DSW-E: ").Append(timestamp.ToString("X")); //.Append(new DateTime(timestamp).ToString("(yyyy-MM-dd HH:mm:ss fffffff)"));
			sb.Append(" ").Append(type);
			if(watchType != WatcherChangeTypes.All)
				sb.Append(" <").Append(watchType).Append(">");
			if(file != null)
			{
				sb.Append(" [").Append(file).Append("]");
			}
			if(oldFile != null)
			{
				sb.Append(" [").Append(oldFile).Append("]");
			}
			if(data != null)
			{
				sb.Append(" [...data...]");
			}
			return sb.ToString();
		}
	} //EOC
}
