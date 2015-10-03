using System;
using System.Text;
using System.IO;
using System.Security;
using System.Security.Cryptography;

namespace dsw
{
	internal sealed class FPrint
	{
		private static SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
		
		internal static string Compute(Stream s)
		{
			byte[] b = null;
			//BufferedStream bs = new BufferedStream(s, 10 * 1024 * 1024);
			StringBuilder sb = null;
			try
			{
				//if(!File.Exists(file)) throw new Exception("File does not exists!\n" + file);
				if(s == null) throw new Exception("Cannot read file!");
				//SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
				b = sha1.ComputeHash(s);
				s.Close();
				s = null;
				if(b == null) throw new Exception("Hash failed!");
				sb = new StringBuilder(500);
				for(int i = 0; i < b.Length; i++)
				{
					sb.Append(b[i]).Append(' ');
				}
			} 
			finally
			{
				if(s != null) s.Close();
			}
			if(sb != null) return sb.ToString();
			return null;
		}

		internal static string Compute(string file)
		{
			string id = null;
			Stream s = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 10 * 1024 * 1024);
			id = Compute(s);
			return id;
		}
	} //EOC
}
