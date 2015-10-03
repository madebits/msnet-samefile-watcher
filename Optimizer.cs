using System;
using System.IO;
using System.Collections;

namespace dsw
{

	internal class Optimizer
	{
		internal Optimizer()
		{
		}

		internal bool CanProcessEvent(WEvent we)
		{
			if(!Utils.Conf.ProcessEvents) return false;
			// skip WCOPY move events
			if(Utils.ContainsWCopy(we.file)) return false;
			//skip zero size files
			if(File.Exists(we.file))
			{
				FileInfo fi = new FileInfo(we.file);
				if(fi == null)
				{ 
					throw new Exception("cannot access");
				}
				if(fi.Length <= 0) return false;
			}
			return true;
		}

	}//EOC
}
