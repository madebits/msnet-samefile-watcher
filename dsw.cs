using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Threading;

namespace dsw
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtDir;
		private System.Windows.Forms.Button btnDir;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.CheckBox chkLog;
		private System.Windows.Forms.CheckBox chkNotify;
		private System.Windows.Forms.CheckBox chkMove;
		private System.Windows.Forms.RichTextBox txtLog;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem mnuFile;
		private System.Windows.Forms.MenuItem mnuExit;
		private System.Windows.Forms.MenuItem mnuTools;
		private System.Windows.Forms.MenuItem mnuIView;
		private System.Windows.Forms.MenuItem mnuIExport;
		private System.Windows.Forms.MenuItem mnuIImport;
		private System.Windows.Forms.MenuItem mnuHelp;
		private System.Windows.Forms.MenuItem mnuAbout;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem mnuBuildIndex;
		private System.Windows.Forms.MenuItem mnuLogEvents;
		private System.Windows.Forms.PictureBox pBoxState;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem mnuCloseMatchWindows;
		private System.Windows.Forms.MenuItem mnuDebug;
		private System.Windows.Forms.MenuItem mnuViewAllIndices;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem mnuProcessEvents;
		private System.Windows.Forms.MenuItem mnuEnableWatch;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.Label txtStatus;
		private System.Windows.Forms.MenuItem mnuDSWEvents;
		private System.Windows.Forms.MenuItem mnuShowErrorStack;
		private System.Windows.Forms.MenuItem mnuFileLog;
		private System.Windows.Forms.MenuItem mnuClearLogWin;
		private System.Windows.Forms.MenuItem mnuUsage;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem mnuViewLog;
		private System.Windows.Forms.MenuItem mnuLogReindexFiles;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			InitFromConfig();
			chkMove.Text = "Move to " + Config.WCOPY + " (subdir)";
			//this.Closed += new EventHandler(OnClose);
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			string[] args = Environment.GetCommandLineArgs();
			if((args.Length > 1) && Directory.Exists(args[1]))
			{
				txtDir.Text = args[1];
			}
			else txtDir.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			//txtLog.Text = "This software comes without any warranty!";
			log = new Log(txtLog);
		}

		private void InitFromConfig()
		{
			mnuLogEvents.Checked = Utils.Conf.LogEvents;
			mnuDSWEvents.Checked = Utils.Conf.LogDSWSEvents;
			mnuShowErrorStack.Checked = Utils.Conf.PrintErrorStack;
			mnuProcessEvents.Checked = Utils.Conf.ProcessEvents;
		}

		internal readonly static string APP_NAME = "DSW";

		private void OnClose(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.txtDir = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnDir = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.chkLog = new System.Windows.Forms.CheckBox();
			this.chkNotify = new System.Windows.Forms.CheckBox();
			this.chkMove = new System.Windows.Forms.CheckBox();
			this.txtLog = new System.Windows.Forms.RichTextBox();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.mnuFile = new System.Windows.Forms.MenuItem();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.mnuEnableWatch = new System.Windows.Forms.MenuItem();
			this.mnuLogEvents = new System.Windows.Forms.MenuItem();
			this.mnuProcessEvents = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.mnuBuildIndex = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.mnuFileLog = new System.Windows.Forms.MenuItem();
			this.mnuViewLog = new System.Windows.Forms.MenuItem();
			this.mnuClearLogWin = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.mnuCloseMatchWindows = new System.Windows.Forms.MenuItem();
			this.mnuTools = new System.Windows.Forms.MenuItem();
			this.mnuIView = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mnuIExport = new System.Windows.Forms.MenuItem();
			this.mnuIImport = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.mnuDebug = new System.Windows.Forms.MenuItem();
			this.mnuViewAllIndices = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.mnuDSWEvents = new System.Windows.Forms.MenuItem();
			this.mnuLogReindexFiles = new System.Windows.Forms.MenuItem();
			this.mnuShowErrorStack = new System.Windows.Forms.MenuItem();
			this.mnuHelp = new System.Windows.Forms.MenuItem();
			this.mnuUsage = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.mnuAbout = new System.Windows.Forms.MenuItem();
			this.pBoxState = new System.Windows.Forms.PictureBox();
			this.txtStatus = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtDir
			// 
			this.txtDir.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtDir.Location = new System.Drawing.Point(8, 16);
			this.txtDir.Name = "txtDir";
			this.txtDir.Size = new System.Drawing.Size(352, 20);
			this.txtDir.TabIndex = 1;
			this.txtDir.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(232, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Directory to watch (0 length files are ignored)";
			// 
			// btnDir
			// 
			this.btnDir.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnDir.Location = new System.Drawing.Point(368, 16);
			this.btnDir.Name = "btnDir";
			this.btnDir.Size = new System.Drawing.Size(40, 20);
			this.btnDir.TabIndex = 2;
			this.btnDir.Text = "...";
			this.btnDir.Click += new System.EventHandler(this.btnDir_Click);
			// 
			// btnStart
			// 
			this.btnStart.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnStart.Location = new System.Drawing.Point(368, 48);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(40, 20);
			this.btnStart.TabIndex = 7;
			this.btnStart.Text = "Start";
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(208, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "What to do if a file that exists is created?";
			// 
			// chkLog
			// 
			this.chkLog.Checked = true;
			this.chkLog.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkLog.Location = new System.Drawing.Point(8, 72);
			this.chkLog.Name = "chkLog";
			this.chkLog.Size = new System.Drawing.Size(48, 16);
			this.chkLog.TabIndex = 4;
			this.chkLog.Text = "Log";
			// 
			// chkNotify
			// 
			this.chkNotify.Location = new System.Drawing.Point(64, 72);
			this.chkNotify.Name = "chkNotify";
			this.chkNotify.Size = new System.Drawing.Size(56, 16);
			this.chkNotify.TabIndex = 5;
			this.chkNotify.Text = "Notify";
			// 
			// chkMove
			// 
			this.chkMove.Checked = true;
			this.chkMove.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMove.Location = new System.Drawing.Point(128, 72);
			this.chkMove.Name = "chkMove";
			this.chkMove.Size = new System.Drawing.Size(192, 16);
			this.chkMove.TabIndex = 6;
			this.chkMove.Text = "Move";
			// 
			// txtLog
			// 
			this.txtLog.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtLog.HideSelection = false;
			this.txtLog.Location = new System.Drawing.Point(0, 96);
			this.txtLog.Name = "txtLog";
			this.txtLog.ReadOnly = true;
			this.txtLog.Size = new System.Drawing.Size(416, 120);
			this.txtLog.TabIndex = 0;
			this.txtLog.Text = "";
			this.txtLog.WordWrap = false;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuFile,
																					  this.menuItem2,
																					  this.mnuTools,
																					  this.mnuHelp});
			// 
			// mnuFile
			// 
			this.mnuFile.Index = 0;
			this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuExit});
			this.mnuFile.Text = "File";
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 0;
			this.mnuExit.Text = "Exit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuEnableWatch,
																					  this.mnuLogEvents,
																					  this.mnuProcessEvents,
																					  this.menuItem3,
																					  this.mnuBuildIndex,
																					  this.menuItem5,
																					  this.mnuFileLog,
																					  this.mnuViewLog,
																					  this.mnuClearLogWin,
																					  this.menuItem4,
																					  this.mnuCloseMatchWindows});
			this.menuItem2.Text = "Edit";
			// 
			// mnuEnableWatch
			// 
			this.mnuEnableWatch.Checked = true;
			this.mnuEnableWatch.Enabled = false;
			this.mnuEnableWatch.Index = 0;
			this.mnuEnableWatch.Text = "Enable Watching";
			this.mnuEnableWatch.Click += new System.EventHandler(this.mnuEnableWatch_Click);
			// 
			// mnuLogEvents
			// 
			this.mnuLogEvents.Index = 1;
			this.mnuLogEvents.Text = "Log Events";
			this.mnuLogEvents.Click += new System.EventHandler(this.mnuLogEvents_Click);
			// 
			// mnuProcessEvents
			// 
			this.mnuProcessEvents.Checked = true;
			this.mnuProcessEvents.Index = 2;
			this.mnuProcessEvents.Text = "Process Events";
			this.mnuProcessEvents.Click += new System.EventHandler(this.mnuProcessEvents_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 3;
			this.menuItem3.Text = "-";
			// 
			// mnuBuildIndex
			// 
			this.mnuBuildIndex.Checked = true;
			this.mnuBuildIndex.Index = 4;
			this.mnuBuildIndex.Text = "Build File Index on Start";
			this.mnuBuildIndex.Click += new System.EventHandler(this.mnuBuildIndex_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 5;
			this.menuItem5.Text = "-";
			// 
			// mnuFileLog
			// 
			this.mnuFileLog.Index = 6;
			this.mnuFileLog.Text = "Log to File (dswlog.txt)";
			this.mnuFileLog.Click += new System.EventHandler(this.mnuFileLog_Click);
			// 
			// mnuViewLog
			// 
			this.mnuViewLog.Index = 7;
			this.mnuViewLog.Text = "View Log File";
			this.mnuViewLog.Click += new System.EventHandler(this.mnuViewLog_Click);
			// 
			// mnuClearLogWin
			// 
			this.mnuClearLogWin.Index = 8;
			this.mnuClearLogWin.Text = "Clear Log Window";
			this.mnuClearLogWin.Click += new System.EventHandler(this.mnuClearLogWin_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 9;
			this.menuItem4.Text = "-";
			// 
			// mnuCloseMatchWindows
			// 
			this.mnuCloseMatchWindows.Index = 10;
			this.mnuCloseMatchWindows.Text = "Close All Open Notify Windows";
			this.mnuCloseMatchWindows.Click += new System.EventHandler(this.mnuCloseMatchWindows_Click);
			// 
			// mnuTools
			// 
			this.mnuTools.Enabled = false;
			this.mnuTools.Index = 2;
			this.mnuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuIView,
																					 this.menuItem1,
																					 this.mnuIExport,
																					 this.mnuIImport,
																					 this.menuItem6,
																					 this.mnuDebug});
			this.mnuTools.Text = "Index";
			// 
			// mnuIView
			// 
			this.mnuIView.Index = 0;
			this.mnuIView.Text = "View Repeated";
			this.mnuIView.Click += new System.EventHandler(this.mnuIView_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 1;
			this.menuItem1.Text = "-";
			// 
			// mnuIExport
			// 
			this.mnuIExport.Index = 2;
			this.mnuIExport.Text = "Export";
			this.mnuIExport.Click += new System.EventHandler(this.mnuIExport_Click);
			// 
			// mnuIImport
			// 
			this.mnuIImport.Index = 3;
			this.mnuIImport.Text = "Import";
			this.mnuIImport.Click += new System.EventHandler(this.mnuIImport_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 4;
			this.menuItem6.Text = "-";
			// 
			// mnuDebug
			// 
			this.mnuDebug.Index = 5;
			this.mnuDebug.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuViewAllIndices,
																					 this.menuItem7,
																					 this.mnuDSWEvents,
																					 this.mnuLogReindexFiles,
																					 this.mnuShowErrorStack});
			this.mnuDebug.Text = "Debug";
			// 
			// mnuViewAllIndices
			// 
			this.mnuViewAllIndices.Index = 0;
			this.mnuViewAllIndices.Text = "Index Dump";
			this.mnuViewAllIndices.Click += new System.EventHandler(this.mnuViewAllIndices_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 1;
			this.menuItem7.Text = "-";
			// 
			// mnuDSWEvents
			// 
			this.mnuDSWEvents.Index = 2;
			this.mnuDSWEvents.Text = "Log DSW Events";
			this.mnuDSWEvents.Click += new System.EventHandler(this.mnuDSWEvents_Click);
			// 
			// mnuLogReindexFiles
			// 
			this.mnuLogReindexFiles.Index = 3;
			this.mnuLogReindexFiles.Text = "Log Reindexed Files";
			this.mnuLogReindexFiles.Click += new System.EventHandler(this.mnuLogReindexFiles_Click);
			// 
			// mnuShowErrorStack
			// 
			this.mnuShowErrorStack.Index = 4;
			this.mnuShowErrorStack.Text = "Print Error Stacktrace";
			this.mnuShowErrorStack.Click += new System.EventHandler(this.mnuShowErrorStack_Click);
			// 
			// mnuHelp
			// 
			this.mnuHelp.Index = 3;
			this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuUsage,
																					this.menuItem8,
																					this.mnuAbout});
			this.mnuHelp.Text = "Help";
			// 
			// mnuUsage
			// 
			this.mnuUsage.Index = 0;
			this.mnuUsage.Text = "Usage Hints";
			this.mnuUsage.Click += new System.EventHandler(this.mnuUsage_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 1;
			this.menuItem8.Text = "-";
			// 
			// mnuAbout
			// 
			this.mnuAbout.Index = 2;
			this.mnuAbout.Text = "About DSW";
			this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
			// 
			// pBoxState
			// 
			this.pBoxState.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.pBoxState.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.pBoxState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pBoxState.Location = new System.Drawing.Point(368, 72);
			this.pBoxState.Name = "pBoxState";
			this.pBoxState.Size = new System.Drawing.Size(16, 16);
			this.pBoxState.TabIndex = 11;
			this.pBoxState.TabStop = false;
			// 
			// txtStatus
			// 
			this.txtStatus.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.txtStatus.BackColor = System.Drawing.Color.Aqua;
			this.txtStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtStatus.ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtStatus.Location = new System.Drawing.Point(383, 72);
			this.txtStatus.Name = "txtStatus";
			this.txtStatus.Size = new System.Drawing.Size(25, 16);
			this.txtStatus.TabIndex = 9;
			this.txtStatus.Text = "L P ";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(416, 213);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.pBoxState,
																		  this.txtLog,
																		  this.chkMove,
																		  this.chkNotify,
																		  this.chkLog,
																		  this.label2,
																		  this.btnStart,
																		  this.btnDir,
																		  this.label1,
																		  this.txtDir,
																		  this.txtStatus});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.Text = "DSW: Same File Created Watcher";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		internal void LogMsg(string s)
		{
			log.LogMsg(s);
		}

		private void ShowErr(string err)
		{
			MessageBox.Show(this, err, "Error - " + APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		internal string TopDir
		{
			get { return topDir; }
		}

		// ---

		private const string STOP = "Stop";
		private string topDir = string.Empty;
		private Thread th = null;
		private string lastFolder = null;
		//
		private Index index = null;
		private FileSystemWatcher watcher = null;
		private Log log = null;
		private DSWEvents wEvents = null;
		
		private void btnStart_Click(object sender, System.EventArgs e)
		{
			if(btnStart.Text.Equals(STOP))
			{
				Index.threadOk = false;
				if(th != null)
				{
					try
					{
						th.Abort();
					} 
					catch{}
					th = null;
				}
				StopWorkEnd();
			}
			else
			{
				string dir = txtDir.Text.Trim();
				if(!Directory.Exists(dir))
				{
					ShowErr("Invalid directory!");
					return;
				}
				dir = Utils.CorrectDirPath(dir);
				if(Utils.ContainsWCopy(dir))
				{ 
					ShowErr("Sorry: Cannot watch directories that contain the string '" + Config.WCOPY + "' in path!");
					return;
				}
				txtDir.Text = dir;
				topDir = dir;
				btnStart.Text = STOP;
				mnuBuildIndex.Enabled = false;
				mnuEnableWatch.Enabled = true;
				Index.threadOk = true;
				Thread th = new Thread(new ThreadStart(DoStartWork));
				th.Start();
			}
		}

		private void DoStartWork()
		{
			try
			{
				BuildIndex(topDir);
				StartWorkEnd();
			} 
			catch(Exception ex1)
			{
				StopWorkEnd();
				LogMsg("# Error: " + ex1.Message);
			}
		}
		
		private void BuildIndex(string dir)
		{
			LogMsg("! Building index " + DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ..."));
			index = new Index(this);
			index.BuildFromDir(dir, mnuBuildIndex.Checked);
			index.PrintStats();
		}

		private void StartWorkEnd()
		{
			th = null;
			if(!Index.threadOk) throw new Exception("user aborted");
			StartWatch(topDir);
			mnuTools.Enabled = true;
			pBoxState.BackColor = Color.Green;
		}

		private void StopWorkEnd()
		{
			StopWatch();
			btnStart.Text = "Start";
			mnuTools.Enabled = false;
			mnuBuildIndex.Enabled = true;
			mnuEnableWatch.Enabled = false;
			pBoxState.BackColor = Color.Orange;
		}
	
		private void StartWatch(string dir)
		{
			StopWatch();
			watcher = new FileSystemWatcher();
			watcher.Path = dir;
			watcher.IncludeSubdirectories = true;
			watcher.NotifyFilter = NotifyFilters.LastWrite 
				| NotifyFilters.FileName | NotifyFilters.DirectoryName;
			watcher.Created += new FileSystemEventHandler(OnCreated);
			watcher.Changed += new FileSystemEventHandler(OnCreated);
			watcher.Deleted += new FileSystemEventHandler(OnCreated); //OnDeleted);
			watcher.Renamed += new RenamedEventHandler(OnRenamed);
			LogMsg("! Starting watching ... ");
			wEvents = new DSWEvents(this);
			this.Text = "DSW: Started <" + topDir + ">";
			EnableWatch(true);
		}

		private void StopWatch()
		{
			if(watcher != null)
			{
				watcher.EnableRaisingEvents = false;
				watcher.Dispose();
				watcher = null;
				LogMsg("! Stopped.");
				this.Text = "DSW: Stopped";
				wEvents.Stop();
				wEvents = null;
			}
		}

		private void EnableWatch(bool on)
		{
			if((watcher != null) && (watcher.EnableRaisingEvents != on))
			{
				watcher.EnableRaisingEvents = on;
				if(on)
				{ 
					pBoxState.BackColor = Color.Green;
					LogMsg("! Watching enabled.");
				}
				else
				{ 
					pBoxState.BackColor = Color.Red;
					LogMsg("! Watching dissabled.");
				}
			}
		}

		private void OnCreated(object source, FileSystemEventArgs e)
		{
			RaiseDSWEvent(e.ChangeType, e.FullPath, null);
		}

		private void OnRenamed(object source, RenamedEventArgs e)
		{
			RaiseDSWEvent(e.ChangeType, e.FullPath, e.OldFullPath);
		}

		private void RaiseDSWEvent(WatcherChangeTypes t, string file, string oldFile)
		{
			try
			{
				WEvent we = new WEvent();
				LogEvent(file, t, we.timestamp);
				we.file = file;
				we.watchType = t;
				we.oldFile = oldFile;
				wEvents.AddEvent(we);
			}
			catch(Exception ex1)
			{
				LogMsg("# Error: (" + file + ") -> " + ex1.Message);
				if(Utils.Conf.PrintErrorStack)
					LogMsg(ex1.StackTrace);
			}
		}

		private void LogEvent(string file, WatcherChangeTypes t, long time)
		{
			if(Utils.Conf.LogEvents)
				LogMsg("\tEVENT: " + time.ToString("X") //new DateTime(time).ToString("(yyyy-MM-dd HH:mm:ss fffffff)")
					+ " <" + t + "> "  + file);
		}

		// ---- dsw event processing

		internal void ProcessEvent(WEvent we)
		{
			if(Utils.Conf.LogDSWSEvents) LogMsg("\t" + we.ToString());
			switch(we.type)
			{
				case WEventType.WatchEvent:
					ProcessWatchEvent(we);
					break;
				case WEventType.IndexView:
					EPViewIndex();
					break;
				case WEventType.IndexDump:
					EPIndexDump();
					break;
				case WEventType.Export:
					EPExport(we);
					break;
				case WEventType.Import:
					EPImport(we);
					break;
			}
		}

		// ------------------

		private void ProcessWatchEvent(WEvent we)
		{
			if(we.type != WEventType.WatchEvent) return;
			switch(we.watchType)
			{
				case WatcherChangeTypes.Deleted:
					if(index.IsFileInIndex(we.file))
					{
						lock(index)
						{
							index.ProcessFileDeleted(we.file);
						}
					}
					else if(index.IsDirInIndex(we.file))
					{
						lock(index)
						{
							index.ProcessDirDelete(we.file);
						}
					}
					// do nothing if neither
					break;
				case WatcherChangeTypes.Renamed:
					if(Directory.Exists(we.file))
					{
						lock(index)
						{
							index.ProcessDirRenamed(we.file, we.oldFile);
						}
					}
					else // file
					{
						lock(index)
						{
							index.ProcessFileRenamed(we.file, we.oldFile);
						}
					}

					break;
				default: // create/ changes
					if(Directory.Exists(we.file))
					{
						if(we.watchType == WatcherChangeTypes.Changed)
						{
							Thread.Sleep(5); // wait until system copies file
						}
						lock(index)
						{
							index.ProcessDirCreatedChanged(we.file, we.watchType);
						}
					}
					else // file
					{
						if(we.watchType == WatcherChangeTypes.Changed)
						{
							Thread.Sleep(5); // wait until system copies file
						}
						lock(index)
						{
							index.ProcessFileCreatedChanged(we.file, we.watchType);
						}
					}
					break;
			}
		}

		internal void FileExists(string file, string id, ArrayList d, WatcherChangeTypes t)
		{
			if(d != null && d.Count > 0)
			{
				string msg = "! Exists: " +  file + " <" + t.ToString() + "> is equal with:" + Environment.NewLine;
				msg += Environment.NewLine + Utils.ArrayList2String("\t", d);

				if(chkLog.Checked)
				{
					LogMsg(msg);
				}
				if(chkNotify.Checked)
				{
					ShowNotify(msg);
				}
				if(chkMove.Checked)
				{
					try
					{
						MoveRepeated(file);
					} 
					catch(Exception ex)
					{
						//LogMsg("#Error: " + ex.Message);
						if(ex.Message.StartsWith("Could not find file")) throw ex;
						else
						{
							LogMsg("# Error: " + ex.Message);
							LogMsg("# Cannot move file! Trying to update index!");
							index.UpdateAddFileNew(file, id, d, t);
						}
					}
				}
			}
			else
			{
				index.UpdateAddFileNew(file, id, d, t);
				LogMsg("! Added: " + file);
			}
		}

		private string _msg = null;

		private void ShowNotify(string msg)
		{
			_msg = msg;
			this.Invoke(new MethodInvoker(DoShowNotify));
			//this.Invoke(new NotifyDelegate(DoShowNotify), new object[]{msg});
		}

		//delegate void NotifyDelegate(string msg);

		private void DoShowNotify() //string _msg)
		{
			FText ft = new FText();
			ft.Size = new Size(550, 200);
			ft.ShowInTaskbar = true;
			ft.ShowNotifyMsg("Match !!!", _msg);
			ft.Activate();
			ft.Focus();
		}

		private void MoveRepeated(string file)
		{
			string moveDir = topDir + Config.WCOPY + "\\";
			if(!Directory.Exists(moveDir))
			{
				Directory.CreateDirectory(moveDir);
			}
			string newFile = Path.Combine(moveDir, Path.GetFileName(file));
			if(File.Exists(newFile))
			{
				int i = 0;
				do
				{
					newFile = Path.Combine(moveDir, "R" + i + "-" + Path.GetFileName(file));
					i++;
				} while(File.Exists(newFile));
			}
			File.Move(file, newFile);
			LogMsg("! Moved [" + file + "] to [" + newFile + "]");
		}

		//-----------------------------

		private void btnDir_Click(object sender, System.EventArgs e)
		{
			DSV.DirSelect ds = new DSV.DirSelect();
			string dir = ds.GetDir();
			if((dir != null) && (dir.Trim().Length > 0))
			{
				txtDir.Text = dir;
			}
		}

		private void mnuExit_Click(object sender, System.EventArgs e)
		{
			StopWatch();
			Application.Exit();
		}

		private void mnuIView_Click(object sender, System.EventArgs e)
		{
			WEvent we = new WEvent();
			we.type = WEventType.IndexView;
			wEvents.AddEvent(we);
		}

		private void EPViewIndex()
		{
			string msg = "Index in empty!";
			if(index != null)
			{ 
				lock(index)
				{
					msg = index.GetRepeatedIndex();
				}
			}
			FText ft = new FText();
			ft.ShowText(this, "Repeated Files", msg);
		}
		
		private void mnuIExport_Click(object sender, System.EventArgs e)
		{
			SaveFileDialog sf = null;
			try
			{
				if(lastFolder == null) lastFolder = topDir;
				sf = new SaveFileDialog();
				sf.Title = "Export Current DSW File Index";
				sf.Filter = "DSW Index Files (*.vidx)|*.vidx|All files (*.*)|*.*";
				sf.InitialDirectory = topDir;
				sf.FileName = "i" + (DateTime.Now.ToString("yyyyMMdd-HHmmss")) + ".vidx";
				if(sf.ShowDialog() == DialogResult.OK)
				{
					string fileName = sf.FileName;
					if(fileName == null) return;
					WEvent we = new WEvent();
					we.type = WEventType.Export;
					we.file = fileName;
					wEvents.AddEvent(we);
					lastFolder = Path.GetDirectoryName(fileName);
				}
			} 
			catch(Exception ex1)
			{
				ShowErr(ex1.Message);
			}
			finally 
			{
				if(sf != null) sf.Dispose();
			}
		}

		private void EPExport(WEvent we)
		{
			if(index != null)
			{
				lock(index)
				{
					index.ExportIndex(we.file);
				}
			}
		}

		private void mnuIImport_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog ofd = null;
			try
			{
				if(lastFolder == null) lastFolder = topDir;
				ofd = new OpenFileDialog();
				ofd.Title = "Import DSW File Index File(s)";
				ofd.InitialDirectory = lastFolder;
				ofd.Filter = "DSW Index Files (*.vidx)|*.vidx|All files (*.*)|*.*";
				ofd.Multiselect = true;
				if (ofd.ShowDialog () == DialogResult.OK) 
				{
					if((ofd.FileNames != null) && (ofd.FileNames.Length > 0))
					{
						for(int i = 0; i < ofd.FileNames.Length; i++)
						{
							WEvent we = new WEvent();
							we.type = WEventType.Import;
							we.file = ofd.FileNames[i];
							wEvents.AddEvent(we);
							lastFolder = Path.GetDirectoryName(ofd.FileNames[i]);
						}
					}
				}
			} 
			catch(Exception ex1)
			{
				ShowErr(ex1.Message);
			}
			finally 
			{
				if(ofd != null) ofd.Dispose();
			}
		}

		private void EPImport(WEvent we)
		{
			if(index != null)
			{
				lock(index)
				{
					index.ImportIndex(we.file);
				}
			}
		}

		private void mnuAbout_Click(object sender, System.EventArgs e)
		{
			FText ft = new FText();
			string msg = Environment.NewLine;
			msg += "\tDSW: Same File Created Watcher 0.9.0" + Environment.NewLine;
			msg += Environment.NewLine;
			msg += "\t(c) 2003 by Vasian Cepa" + Environment.NewLine;
			msg += "\t/" + Environment.NewLine;
			msg += Environment.NewLine;
			msg += "\tThis software comes without any warranty!" + Environment.NewLine;
			ft.Size = new Size(200, 150);
			ft.MaximizeBox = false;
			ft.FormBorderStyle = FormBorderStyle.FixedDialog;
			ft.ShowText(this, "About", msg);
		}

		private void mnuBuildIndex_Click(object sender, System.EventArgs e)
		{
			mnuBuildIndex.Checked = !mnuBuildIndex.Checked;
		}

		private void mnuLogEvents_Click(object sender, System.EventArgs e)
		{
			mnuLogEvents.Checked = !mnuLogEvents.Checked;
			Utils.Conf.LogEvents = mnuLogEvents.Checked;
			SetStatus();
		}

		private void mnuProcessEvents_Click(object sender, System.EventArgs e)
		{
			mnuProcessEvents.Checked = !mnuProcessEvents.Checked;
			Utils.Conf.ProcessEvents = mnuProcessEvents.Checked;
			SetStatus();
		}

		private void SetStatus()
		{
			if(!Utils.Conf.LogEvents && !Utils.Conf.ProcessEvents)
			{
				txtStatus.BackColor = Color.Red;
			}
			else txtStatus.BackColor = Color.Aqua;
			string status = string.Empty;
			if(Utils.Conf.LogEvents) status += "L ";
			else status += "_ ";
			if(Utils.Conf.ProcessEvents) status += "P ";
			else status += "_ ";
			txtStatus.Text = status;
		}

		private void mnuEnableWatch_Click(object sender, System.EventArgs e)
		{
			mnuEnableWatch.Checked = !mnuEnableWatch.Checked;
			EnableWatch(mnuEnableWatch.Checked);
		}

		private void mnuCloseMatchWindows_Click(object sender, System.EventArgs e)
		{
			FText.CloseMatchWindows();
		}

		private void mnuViewAllIndices_Click(object sender, System.EventArgs e)
		{
			WEvent we = new WEvent();
			we.type = WEventType.IndexDump;
			wEvents.AddEvent(we);
		}

		private void EPIndexDump()
		{
			string msg = "Index in empty!";
			if(index != null)
			{ 
				lock(index)
				{
					msg = index.GetIndexDump();
				}
			}
			FText ft = new FText();
			ft.ShowText(this, "Index Dump", msg);
		
		}

		private void mnuDSWEvents_Click(object sender, System.EventArgs e)
		{
			mnuDSWEvents.Checked = !mnuDSWEvents.Checked;
			Utils.Conf.LogDSWSEvents = mnuDSWEvents.Checked;
		}

		private void mnuShowErrorStack_Click(object sender, System.EventArgs e)
		{
			mnuShowErrorStack.Checked = !mnuShowErrorStack.Checked;
			Utils.Conf.PrintErrorStack = mnuShowErrorStack.Checked;
		}

		private void mnuFileLog_Click(object sender, System.EventArgs e)
		{
			mnuFileLog.Checked = !mnuFileLog.Checked;
			try
			{
				log.SetFileLog(mnuFileLog.Checked);
			}
			catch(Exception ex1)
			{
				ShowErr(ex1.Message);
			}
		}

		private void mnuClearLogWin_Click(object sender, System.EventArgs e)
		{
			txtLog.Clear();
			txtLog.Focus();
		}

		private void mnuUsage_Click(object sender, System.EventArgs e)
		{
			FText ft = new FText();
			ft.ShowText(this, "DSW Help", Help.help);
		}

		private void mnuViewLog_Click(object sender, System.EventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start(Log.GetLogFile());
			} 
			catch(Exception ex)
			{
				ShowErr(ex.Message + " <" + Log.GetLogFile() + ">");
			}
		}

		private void mnuLogReindexFiles_Click(object sender, System.EventArgs e)
		{
			mnuLogReindexFiles.Checked = !mnuLogReindexFiles.Checked;
			Utils.Conf.LogFileReindex = mnuLogReindexFiles.Checked;
		}

	} //EOC
}
