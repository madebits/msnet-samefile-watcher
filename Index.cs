using System;
using System.Text;
using System.Collections;
using System.IO;

namespace dsw
{
	internal class Index
	{
		private Hashtable index = null;
		private long filesProcessed = 0L;
		private long dirsProcessed = 0L;
		private long fileErrors = 0L;
		private long dirErrors = 0L;
		private long repeated = 0L;
		private Hashtable dirIndex = null;
		private Hashtable reverseIndex = null;
		
		internal static bool threadOk = true;
		private Form1 handle = null;

		internal Index(Form1 handle)
		{
			this.handle = handle;
			index = new Hashtable();
			reverseIndex = new Hashtable();
			dirIndex = new Hashtable();
		}

		private void ResetStats()
		{
			filesProcessed = 0L;
			dirsProcessed = 0L;
			fileErrors = 0L;
			dirErrors = 0L;
			repeated = 0L;	
		}

		internal void PrintStats()
		{
			LogMsg(string.Empty);
			LogMsg("! Processed " + filesProcessed + " file(s) in " + dirsProcessed + " dir(s)!");
			if(fileErrors > 0) LogMsg("! File errors: " + fileErrors);
			if(dirErrors > 0) LogMsg("! Dir errors: " + dirErrors);
			LogMsg("! Repeated files: " + repeated);
			LogMsg("! File index entries: " + index.Count + Environment.NewLine);
		}

		private void LogMsg(string s)
		{
			if(handle != null) handle.LogMsg(s);
		}

		// Fill index from dir
		// Assumption index does not know anything about dir

		internal void BuildFromDir(string dir, bool processHash)
		{
			if(!threadOk) return;
			if(Utils.ContainsWCopy(dir))
			{ 
				LogMsg("# Skipped: " + dir);
				return;
			}
			dirsProcessed++;
			LogMsg("! Processing <" + dir + ">");
			
			// dir index
			string dirId = Path.GetFullPath(dir).ToLower();
			dirId = Utils.CorrectFullDirPath(dirId);
			ArrayList dd = (ArrayList)dirIndex[dirId];
			if(dd != null) throw new Exception("dir already in index");
			dd = new ArrayList(32);
			
			// files
			string[] f = Directory.GetFiles(dir);
			if((f != null) && (f.Length > 0))
			{
				for(int i = 0; i < f.Length; i++)
				{
					string lfile = Path.GetFullPath(f[i]).ToLower();
					if(!dd.Contains(lfile))
					{
						dd.Add(lfile);
					}
					try
					{
						if(!threadOk) return;
						if(processHash) BProcessFile(f[i]);
					} 
					catch(Exception ex1)
					{
						LogMsg("# Error: " + ex1.Message);
						if(Utils.Conf.PrintErrorStack)
							LogMsg(ex1.StackTrace);
						fileErrors++;
					}
				}
			}
			dirIndex[dirId] = dd;
			
			// subdirs
			if(!threadOk) return;
			f = Directory.GetDirectories(dir);
			if((f != null) && (f.Length > 0))
			{
				for(int i = 0; i < f.Length; i++)
				{
					try
					{
						if(!threadOk) return;
						BuildFromDir(f[i], processHash);
					} 
					catch(Exception ex1)
					{
						LogMsg("# Error: " + ex1.Message);
						if(Utils.Conf.PrintErrorStack)
							LogMsg(ex1.StackTrace);
						dirErrors++;
					}
				}
			}
		}

		private void BProcessFile(string file)
		{
			file = Path.GetFullPath(file);
			filesProcessed++;
			FileInfo fi = new FileInfo((string)file);
			if(fi == null)
			{ 
				throw new Exception("cannot access");
			}
			LogMsg("\t" + file + " [" + fi.Length + " byte(s)]");
			ArrayList d = AddInIndex(file);
			if(d.Count > 1)
			{ 
				LogMsg("\t!!!Already in index!!!");
				repeated++;
			}
			d = null;
		}

		private ArrayList AddInIndex(string file)
		{
			string lfile = file.ToLower();
			ArrayList d = null;
			string id = string.Empty;
			id = FPrint.Compute(file);
			if(id == null)
			{
				LogMsg("# Skipped: " + file);
				return null;
			}
			d = (ArrayList)index[id];
			if(d == null)
			{
				d = new ArrayList(5);
			}
			if(!d.Contains(lfile))
			{
				d.Add(lfile);
			}
			index[id] = d;
			reverseIndex[lfile] = id;
			return d;
		}

		// ---

		private void ReindexDir(string dir)
		{
			if(!threadOk) return;
			if(Utils.ContainsWCopy(dir))
			{ 
				LogMsg("# Skipped: " + dir);
				return;
			}
			dirsProcessed++;
			LogMsg("! Processing <" + dir + ">");
	
			// dir index
			string dirId = Path.GetFullPath(dir).ToLower();
			dirId = Utils.CorrectFullDirPath(dirId);
			ArrayList dd = (ArrayList)dirIndex[dirId];
			if(dd == null) dd = new ArrayList(32);
			else dd.Clear();
			// files
			string[] f = Directory.GetFiles(dir);
			if((f != null) && (f.Length > 0))
			{
				for(int i = 0; i < f.Length; i++)
				{
					string lfile = Path.GetFullPath(f[i]).ToLower();
					try
					{
						if(!threadOk) return;
						if(ReProcessFile(f[i]))
						{
							if(!dd.Contains(lfile))
							{
								dd.Add(lfile);
							}
						}
					} 
					catch(Exception ex1)
					{
						LogMsg("# Error: " + ex1.Message);
						if(Utils.Conf.PrintErrorStack)
							LogMsg(ex1.StackTrace);
						fileErrors++;
					}
				}
			}
			dirIndex[dirId] = dd;
			
			// subdirs
			if(!threadOk) return;
			f = Directory.GetDirectories(dir);
			if((f != null) && (f.Length > 0))
			{
				for(int i = 0; i < f.Length; i++)
				{
					try
					{
						if(!threadOk) return;
						ReindexDir(f[i]);
					} 
					catch(Exception ex1)
					{
						LogMsg("# Error: " + ex1.Message);
						if(Utils.Conf.PrintErrorStack)
							LogMsg(ex1.StackTrace);
						dirErrors++;
					}
				}
			}
		}

		private bool ReProcessFile(string file)
		{
			file = Path.GetFullPath(file);
			filesProcessed++;
			if(Utils.Conf.LogFileReindex)
			{
				FileInfo fi = new FileInfo((string)file);
				if(fi == null)
				{ 
					throw new Exception("cannot access");
				}
				LogMsg("\t" + file + " [" + fi.Length + " byte(s)]");
			}
			//
			string lfile = file.ToLower();
			string id = FPrint.Compute(file);
			if(id == null)
			{
				LogMsg("# Skipped: " + file);
				return false;
			}
			ArrayList d = (ArrayList)index[id];
			if(d != null)
			{
				// file exists
				repeated++;
				// file could match itself, in this case skip
				if(d.Contains(file.ToLower()))
				{ 
					return true;
				}
				handle.FileExists(file, id, d, WatcherChangeTypes.Created);
				return false;
			}
			// if new add in index
			d = new ArrayList(5);
			if(!d.Contains(lfile))
			{
				d.Add(lfile);
			}
			index[id] = d;
			reverseIndex[lfile] = id;
			return true;
		}

		// -------------- START FILE operations 

		internal void ProcessFileCreatedChanged(string file, WatcherChangeTypes t)
		{
			//we check the main index only
			// if file is in main index it exists
			// call Form1 process exists
			// Form1 process exists may decide to accept file
			// so we have to have a way to add the file without check
			// if file is not in the index
			// add it without check and update all indexes
			string id = FPrint.Compute(file);
			if(id == null)
			{
				LogMsg("# Skipped: " + file);
				return;
			}
			ArrayList d = (ArrayList)index[id];
			if((d != null) && (d.Count > 0))
			{
				// file could match itself, in this case skip
				if(d.Contains(file.ToLower())) return;
				handle.FileExists(file, id, d, t);
			}
			else //file does not exist
			{
				UpdateAddFileNew(file, id, d, t);
			}
		}

		internal void UpdateAddFileNew(string file, string id, ArrayList d, WatcherChangeTypes t)
		{
			string lfile = file.ToLower();
			if(d == null) d = new ArrayList(4);
			if(t == WatcherChangeTypes.Created)
			{
				// update index
				if(!d.Contains(lfile)) d.Add(lfile);
				index[id] = d;
				// update revIndex
				reverseIndex[lfile] = id;
				// update dirIndex
				string dir = Path.GetDirectoryName(lfile);
				ArrayList dd = (ArrayList)dirIndex[dir];
				if(dd == null)
				{
					dd = new ArrayList(32);
				}
				if(!dd.Contains(lfile.ToLower())) dd.Add(lfile);
				dirIndex[dir] = dd;
			}
			else // change
			{
				// id1 is the old id
				string id1 = (string)reverseIndex[lfile];
				if(id1 != null)
				{
					// updateIndex
					// file was in index before under another id1
					ArrayList d1 = (ArrayList)index[id1];
					if(d1 != null)
					{
						if(d1.Count > 0) d1.Remove(lfile);
						if(d1.Count == 0) index.Remove(d1);
					}
				}
				// update index with new id
				if(!d.Contains(lfile)) d.Add(lfile);
				index[id] = d;
				// update reverseIndex
				reverseIndex[lfile] = id;
				// dirIndex does not change
			}
		}

		internal void ProcessFileRenamed(string file, string oldFile)
		{
			// check if file is in reverseIndex
			string lfile = file.ToLower();
			string lofile = oldFile.ToLower();
			string id = (string)reverseIndex[lofile];
			if(id == null)
			{
				// if it is not there, we should try to add the file
				ProcessFileCreatedChanged(file, WatcherChangeTypes.Created); 
			} 
			else
			{
				reverseIndex.Remove(lofile);
				// we do not check if file is in index, os handles dublicate file names
				reverseIndex[lfile] = id;
				// update index
				ArrayList d = (ArrayList)index[id];
				if(d == null) d = new ArrayList(4);
				if(d.Contains(lofile)) d.Remove(lofile);
				if(!d.Contains(lfile)) d.Add(lfile);
				index[id] = d;
			}
			// update dirIndex
			string dir = Utils.CorrectFullDirPath(Path.GetDirectoryName(lofile));
			ArrayList dd = (ArrayList)dirIndex[dir];
			if(dd == null) dd = new ArrayList(32);
			if(dd.Contains(lofile)) dd.Remove(lofile);
			if(!dd.Contains(lfile)) dd.Add(lfile);
			dirIndex[dir] = dd;
		}

		internal void ProcessFileDeleted(string file)
		{
			// during delete we cannot access file
			string lfile = file.ToLower();
			string id = (string)reverseIndex[lfile];
			if(id == null)
			{
				// do nothing
			}
			else
			{
				reverseIndex.Remove(lfile);
				// update index
				ArrayList d = (ArrayList)index[id];
				if(d == null) d = new ArrayList(4);
				if(d.Contains(lfile)) d.Remove(lfile);
				if(d.Count <= 0) index.Remove(d);
				else index[id] = d;
			}
			// update dirIndex
			string dir = Utils.CorrectFullDirPath(Path.GetDirectoryName(lfile));
			ArrayList dd = (ArrayList)dirIndex[dir];
			if(dd == null) dd = new ArrayList(32);
			if(dd.Contains(lfile)) dd.Remove(lfile);
			if(dd.Count <= 0) dirIndex.Remove(dd);
			else dirIndex[dir] = dd;
		}

		// --- utils

		internal bool IsDirInIndex(string dir)
		{
			ArrayList dd = (ArrayList)dirIndex[Utils.CorrectFullDirPath(dir.ToLower())];
			if(dd == null) return false;
			return true;
		}

		internal bool IsFileInIndex(string file)
		{
			string id = (string)reverseIndex[file.ToLower()];
			if(id == null) return false;
			return true;
		}

		// --- dirs

		internal void ProcessDirCreatedChanged(string dir, WatcherChangeTypes t)
		{
			// dir and subdirs must be indexed
			// all match files must be removed
			ResetStats();
			ReindexDir(dir);
			PrintStats();
		}

		internal void ProcessDirRenamed(string dir, string oldDir)
		{
			string ldir = Utils.CorrectDirPath(dir).ToLower();
			string loldDir = Utils.CorrectDirPath(oldDir).ToLower();
			//
			ArrayList dd = (ArrayList)dirIndex[loldDir];
			if(dd == null)
			{
				// no such dir, then add the new one
				ProcessDirCreatedChanged(dir, WatcherChangeTypes.Created);
				return;
			}
			ArrayList newDD = new ArrayList(dd.Count);
			// in dd are the files, whose paths need to be changed
			for(int i = 0; i < dd.Count; i++)
			{
				string file = (string)dd[i];
				string id = (string)reverseIndex[file];
				if(id == null)
				{
					//file is not in revindex, add it
					ProcessFileCreatedChanged(file, WatcherChangeTypes.Created);
					return;
				}
				string newFile = Path.Combine(ldir, Path.GetFileName(file));
				// index
				ArrayList d = (ArrayList)index[id];
				if(d == null) continue; // silently fail
				if(d.Contains(file)) d.Remove(file);
				if(!d.Contains(newFile)) d.Add(newFile);
				if(d.Count <= 0) index.Remove(d);
				else index[id] = d;
				// rev
				reverseIndex.Remove(file);
				reverseIndex[newFile] = id;
				// dir
				if(!newDD.Contains(newFile))
					newDD.Add(newFile);
			}
			// now update dirIndex
			dirIndex.Remove(loldDir);
			dirIndex[ldir] = newDD;
		}

		internal void ProcessDirDelete(string dir)
		{
			string ldir = Utils.CorrectDirPath(dir).ToLower();
			ArrayList dd = (ArrayList)dirIndex[ldir];
			if(dd == null) return; // silently fail
			if(dd != null) // dir is in dirIndex
			{
				for(int i = 0; i < dd.Count; i++)
				{
					string id = (string)reverseIndex[(string)dd[i]];
					if(id == null) continue;
					ArrayList d = (ArrayList)index[id];
					if((d != null) && (d.Count > 0))
					{
						d.Remove((string)dd[i]);
						if(d.Count <= 0)
						{
							index.Remove(id);
						}
						else index[id] = d;
					}
					reverseIndex.Remove((string)dd[i]);
				}
				dd = null;
			}
			dirIndex.Remove(ldir);
		}

		// TODO
		// Updates index when file is added
		// Assumes that index is created and does not contain file


		private void UpdateFileAdd(string file)
		{
			file = Path.GetFullPath(file);
			string id = (string)reverseIndex[file];
			if(id != null) // file exists in reverse index
			{ 
				//handle.FileExists(file, (ArrayList)index[id]);
			}
			else
			{
				ArrayList d = AddInIndex(file);
			}
		}

		// -------------- START DIR operations

		// Updates index when dir is deleted
		// Assumes that index is created and contain dir

		

		// ---- EXPORT / IMPORT

		internal void ExportIndex(string file)
		{
			StreamWriter sw = null;
			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("# DSW Index File ").Append(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).Append(Environment.NewLine);
				sb.Append("# ").Append(handle.TopDir).Append(Environment.NewLine);
				sb.Append("# Entries ").Append(index.Count.ToString()).Append(Environment.NewLine);
				foreach(string id in index.Keys)
				{
					ArrayList d = (ArrayList)index[id];
					if((d != null) && (d.Count > 0))
					{
						sb.Append("?").Append(id).Append(Environment.NewLine);
						sb.Append(Utils.ArrayList2String(string.Empty, d).Trim(new char[]{'\r', '\n'})).Append(Environment.NewLine);
						sb.Append("*").Append(Environment.NewLine);
					}
				}
				sw = new StreamWriter(file);
				sw.Write(sb.ToString());
				sw.Close();
				sw = null;
				LogMsg("! Exported.");
			}
			finally
			{
				if(sw != null) sw.Close();
			}
		}

		internal void ImportIndex(string file)
		{
			StreamReader sw = null;
			try
			{
				LogMsg("! Importing from " + file);
				LogMsg("! Index before import " + index.Count);
				if(index == null) index = new Hashtable();
				StringBuilder sb = new StringBuilder();
				sw = new StreamReader(file);
				string line = null;
				int lineCount = 0;
				while(true)
				{
					line = sw.ReadLine();
					lineCount++;
					if(line == null) break;
					line = line.Trim(new char[]{' ', '\t'});
					if(line.Length <= 0) continue;
					if(line.StartsWith("#"))
					{ 
						if(lineCount == 2)
						{
							LogMsg("\t" + line);
						}
						continue;
					}
					if(line.StartsWith("?"))
					{
						string id = line.Substring(1, line.Length - 1).Trim(new char[]{'\r', '\n', ' ', '\t'});
						id += " "; //correct
						ArrayList d = (ArrayList)index[id];
						if(d == null)
						{ 
							d = new ArrayList(32);
						}
						//LogMsg("ID [" + id + "]     ["  + id.GetHashCode() + "] ---> COUNT0 " + d.Count + "\r\n" +  ArrayList2String("\t", d));
						while(true)
						{
							line = sw.ReadLine();
							lineCount++;
							if(line == null) break;
							if(line.StartsWith("*"))
							{
								if(d.Count > 0)
								{
									index[id] = d;
								}
								break;
							}
							line = line.ToLower();
							if(!d.Contains(line))
							{
								d.Add(line);
								//LogMsg("\tAdded: " + line);
								sb.Append("\tAdded: ").Append(line).Append(Environment.NewLine);
							}
							if(!reverseIndex.Contains(line))
								reverseIndex[line] = id;
						}
						//LogMsg("ID [" + id + "]     ["  + id.GetHashCode() + "] ---> COUNT1 " + d.Count);
					}
				}
				sw.Close();
				LogMsg(sb.ToString());
				sw = null;
				LogMsg("! Index after import " + index.Count);
			}
			finally
			{
				if(sw != null) sw.Close();
			}
		}

		internal string GetRepeatedIndex()
		{
			if((index == null) || (index.Count == 0))
			{
				return "Index is empty!" + Environment.NewLine;;
			}
			StringBuilder sb = new StringBuilder();
			ArrayList repList = new ArrayList();
			sb.Append("! Index of <").Append(handle.TopDir).Append(">").Append(Environment.NewLine);
			sb.Append(Environment.NewLine);
			foreach(string id in index.Keys)
			{
				ArrayList d = (ArrayList)index[id];
				if((d != null) && (d.Count > 1))
				{
					sb.Append("? ID [" + id + "]").Append(Environment.NewLine);
					sb.Append(Utils.ArrayList2String("\t", d));
					repList.AddRange(d.GetRange(1, d.Count - 1));
				}
			}
			sb.Append(Environment.NewLine);
			if(repList.Count > 0)
			{
				
				sb.Append("! " + repList.Count + " repeated files").Append(Environment.NewLine);;
				sb.Append(Environment.NewLine);
				sb.Append(Utils.ArrayList2String(string.Empty, repList)).Append(Environment.NewLine);;
				sb.Append("! Use:\r\n\tCtrl+A - select all\r\n\tCtrl+C - copy selected\r\n\tCtrl+F - find text").Append(Environment.NewLine);;
			}
			else
			{ 
				sb.Append("! All files are different!");
				sb.Append(Environment.NewLine);
			}
			return sb.ToString();
		} 

		internal string GetIndexDump()
		{
			long i = 0L;
			StringBuilder sb = new StringBuilder();
			sb.Append("# File Index").Append(Environment.NewLine);
			sb.Append(Environment.NewLine);
			i = 0L;
			foreach(string id in index.Keys)
			{
				sb.Append("[").Append(++i).Append("] ").Append("?").Append(id).Append(Environment.NewLine);
				ArrayList d = (ArrayList)index[id];
				if((d != null) && (d.Count > 0))
					sb.Append(Utils.ArrayList2String("\t", d).Trim(new char[]{'\r', '\n'})).Append(Environment.NewLine);
				sb.Append("\t").Append("*").Append(Environment.NewLine);
			}
			sb.Append(Environment.NewLine);
			sb.Append(Environment.NewLine);
			sb.Append("# Reverse File Index").Append(Environment.NewLine);
			sb.Append(Environment.NewLine);
			i = 0L;
			foreach(string file in reverseIndex.Keys)
			{
				sb.Append("[").Append(++i).Append("] ").Append(file).Append("\r\n\t-> ?").Append((string)reverseIndex[file]).Append(Environment.NewLine);
			}
			sb.Append(Environment.NewLine);
			sb.Append(Environment.NewLine);
			sb.Append("# Directory Index").Append(Environment.NewLine);
			sb.Append(Environment.NewLine);
			i = 0L;
			foreach(string dir in dirIndex.Keys)
			{
				sb.Append("[").Append(++i).Append("] ").Append(dir).Append(Environment.NewLine);
				ArrayList dd = (ArrayList)dirIndex[dir];
				if((dd != null) && (dd.Count > 0))
					sb.Append(Utils.ArrayList2String("\t", dd).Trim(new char[]{'\r', '\n'})).Append(Environment.NewLine);
				sb.Append("\t").Append("*").Append(Environment.NewLine);
			}
			sb.Append(Environment.NewLine);
			sb.Append(Environment.NewLine);
			sb.Append("END").Append(Environment.NewLine);
			return sb.ToString();
		}

	} //EOC
}
