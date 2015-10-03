using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

namespace dsw
{
	/// <summary>
	/// Summary description for FindText.
	/// </summary>
	public class FindText : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtText;
		private System.Windows.Forms.Button btnFind;
		private System.Windows.Forms.CheckBox chkCase;
		private System.Windows.Forms.CheckBox chkWord;
		private System.Windows.Forms.Button bntClose;
		private System.Windows.Forms.TextBox txtPos;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FindText()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.Closing += new CancelEventHandler(this_Closing);
			Text = "Find Text - " + Form1.APP_NAME;
			chkCase.Checked = mcase;
			chkWord.Checked = wword;

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private void this_Closing(object sender, CancelEventArgs e)
		{
			lastString = txtText.Text;
			handle.FindTextDone();
		}

		private FText handle = null;
		private RichTextBox t = null;
		private int lastSearch = 0;
		private static bool mcase = false, wword = false;
		private static string lastString = string.Empty;


		internal void Find(FText h, RichTextBox t)
		{
			this.handle = h;
			this.t = t;
			if(t.SelectedText.Length > 0)
			{
				string[] lines = t.SelectedText.Split(new char[] {'\r', '\n'});
				if(lines != null && lines.Length > 0)
					txtText.Text = lines[0];
				lastSearch = t.SelectionStart;
				txtPos.Text = lastSearch.ToString();
				lastSearch++;
			}
			else
			{
				txtText.Text = lastString;
			}
			Thread th = new Thread(new ThreadStart(DoWork));
			th.Start();
			Show();
		}

		private void DoWork()
		{
			Thread.Sleep(50);
			Invoke(new MethodInvoker(SetFocus));
		}

		private void SetFocus()
		{
			txtText.Focus();
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
			this.txtText = new System.Windows.Forms.TextBox();
			this.btnFind = new System.Windows.Forms.Button();
			this.chkCase = new System.Windows.Forms.CheckBox();
			this.chkWord = new System.Windows.Forms.CheckBox();
			this.bntClose = new System.Windows.Forms.Button();
			this.txtPos = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtText
			// 
			this.txtText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtText.Location = new System.Drawing.Point(8, 8);
			this.txtText.Name = "txtText";
			this.txtText.Size = new System.Drawing.Size(240, 20);
			this.txtText.TabIndex = 0;
			this.txtText.Text = "";
			// 
			// btnFind
			// 
			this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnFind.Location = new System.Drawing.Point(256, 8);
			this.btnFind.Name = "btnFind";
			this.btnFind.Size = new System.Drawing.Size(48, 20);
			this.btnFind.TabIndex = 1;
			this.btnFind.Text = "Find";
			this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
			// 
			// chkCase
			// 
			this.chkCase.Location = new System.Drawing.Point(8, 32);
			this.chkCase.Name = "chkCase";
			this.chkCase.Size = new System.Drawing.Size(88, 16);
			this.chkCase.TabIndex = 2;
			this.chkCase.Text = "Match case";
			this.chkCase.CheckedChanged += new System.EventHandler(this.chkCase_CheckedChanged);
			// 
			// chkWord
			// 
			this.chkWord.Location = new System.Drawing.Point(88, 32);
			this.chkWord.Name = "chkWord";
			this.chkWord.Size = new System.Drawing.Size(88, 16);
			this.chkWord.TabIndex = 3;
			this.chkWord.Text = "Whole word";
			this.chkWord.CheckedChanged += new System.EventHandler(this.chkWord_CheckedChanged);
			// 
			// bntClose
			// 
			this.bntClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bntClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.bntClose.Location = new System.Drawing.Point(256, 32);
			this.bntClose.Name = "bntClose";
			this.bntClose.Size = new System.Drawing.Size(48, 20);
			this.bntClose.TabIndex = 6;
			this.bntClose.Text = "Close";
			this.bntClose.Click += new System.EventHandler(this.bntClose_Click);
			// 
			// txtPos
			// 
			this.txtPos.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtPos.Location = new System.Drawing.Point(176, 32);
			this.txtPos.Name = "txtPos";
			this.txtPos.ReadOnly = true;
			this.txtPos.Size = new System.Drawing.Size(72, 13);
			this.txtPos.TabIndex = 7;
			this.txtPos.TabStop = false;
			this.txtPos.Text = "";
			this.txtPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// FindText
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(306, 63);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.txtPos,
																		  this.bntClose,
																		  this.chkWord,
																		  this.chkCase,
																		  this.btnFind,
																		  this.txtText});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FindText";
			this.ShowInTaskbar = false;
			this.Text = "Find Text";
			this.TopMost = true;
			this.ResumeLayout(false);

		}
		#endregion

		

		private void btnFind_Click(object sender, System.EventArgs e)
		{
			if(t.Text.Length <= 0) return;
			if(txtText.Text.Length <= 0) return;
			if(lastSearch < 0)
			{ 
				lastSearch = 0;
			}
			if(lastSearch >= t.Text.Length)
			{
				lastSearch = t.Text.Length - 1;
			}

			RichTextBoxFinds tf = RichTextBoxFinds.None;
			if(chkCase.Checked) tf = tf | RichTextBoxFinds.MatchCase;
			if(chkWord.Checked) tf = tf | RichTextBoxFinds.WholeWord;
			//if(radUp.Checked) tf = tf | RichTextBoxFinds.Reverse;
		
			lastSearch = t.Find(txtText.Text, lastSearch, tf);

			if(lastSearch >= 0)
			{
				txtPos.Text = lastSearch.ToString();
				t.Select(lastSearch, txtText.Text.Length);
				lastSearch += txtText.Text.Length;
			}
			else
			{
				MessageBox.Show(this, "Text not found!", Form1.APP_NAME);
			}
		}

		private void bntClose_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
				case Keys.Escape:
					Close();
					break;
				case Keys.X:
					//if(e.Control) break;
					if(e.Alt) Application.Exit();
					break;
			}
		}

		private void chkCase_CheckedChanged(object sender, System.EventArgs e)
		{
			mcase = chkCase.Checked;
		}

		private void chkWord_CheckedChanged(object sender, System.EventArgs e)
		{
			wword = chkWord.Checked;
		}

	}
}
