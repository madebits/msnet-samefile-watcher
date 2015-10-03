using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

namespace dsw
{
	/// <summary>
	/// Summary description for FText.
	/// </summary>
	public class FText : System.Windows.Forms.Form
	{
		private System.Windows.Forms.RichTextBox txtText;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FText()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.Closed += new EventHandler(OnClose);
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private void OnClose(object sender, EventArgs e)
		{
			RemovevMatchWindow(this);
			if(ft != null) ft.Close();
		}

		// ----

		private FindText ft = null;

		private void findText()
		{
			if(ft == null)
			{
				ft = new FindText();
				ft.Find(this, txtText);
			}
			else
			{
				ft.Activate();
			}
		}
        
		internal void FindTextDone()
		{
			ft = null;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
				case Keys.F:
					//if(e.Control) break;
					if(e.Control) findText();
					break;
			}
		}

		// ---

		private static ArrayList matchWindows = null;

		private static void AddMatchWindow(FText w)
		{
			if(matchWindows == null) matchWindows = new ArrayList();
			if(!matchWindows.Contains(w)) matchWindows.Add(w);
		}

		private static void RemovevMatchWindow(FText w)
		{
			if((matchWindows != null) && (matchWindows.Contains(w)))
			{
				if(matchWindows.Contains(w)) matchWindows.Remove(w);
			}
		}

		internal static void CloseMatchWindows()
		{
			if(matchWindows == null) return;
			while(matchWindows.Count > 0)
			{
				FText w = (FText)matchWindows[0];
				w.Close();
			}
			/*
			for(int i = 0; i < matchWindows.Count; i++)
			{
				FText w = (FText)matchWindows[i];
				w.Close();
			}
			matchWindows.Clear();
			*/
			matchWindows = null;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FText));
			this.txtText = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// txtText
			// 
			this.txtText.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtText.HideSelection = false;
			this.txtText.Name = "txtText";
			this.txtText.ReadOnly = true;
			this.txtText.Size = new System.Drawing.Size(512, 136);
			this.txtText.TabIndex = 0;
			this.txtText.Text = "Please wait ...";
			this.txtText.WordWrap = false;
			// 
			// FText
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(512, 133);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.txtText});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MinimumSize = new System.Drawing.Size(300, 150);
			this.Name = "FText";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "DSW";
			this.ResumeLayout(false);

		}
		#endregion

		internal DialogResult ShowText(IWin32Window parent, string title, string msg)
		{
			txtText.Text = msg;
			SetTitle(title);
			return ShowDialog(parent);
		}

		internal void SetText(string t)
		{
			txtText.Text = t;
		}

		internal void ShowNotifyMsg(string title, string msg)
		{
			SetTitle(title);
			txtText.Text = msg;
			AddMatchWindow(this);
			Show();
		}

		private void SetTitle(string title)
		{
			this.Text = "DSW: " + title;
		}
	}
}
