using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Security.Cryptography;

namespace DSV
{
	public sealed class DirSelect
	{
		DirectorySelect ds;

		internal DirSelect() 
		{
				ds = new DirectorySelect();
		}

		internal string GetDir(){
			ds.ShowDialog();
			return ds.ReturnPath;
		}

	}//EOC

	internal class DirectorySelect : FolderNameEditor 
	{
		private static FolderBrowser m_fb = null;
		private string m_description = "Select Folder";
		private string m_returnPath = string.Empty;

		internal DirectorySelect() 
		{
		}

		internal string Description 
		{
			get 
			{
				return m_description;
			}
			set 
			{
				m_description = value;
			}
		}

		internal string ReturnPath 
		{
			get 
			{
				return m_returnPath;
			}
		}

		internal DialogResult RunDialog() 
		{
			if(m_fb == null){
				m_fb = new FolderBrowser();
				m_fb.Style = FolderBrowserStyles.RestrictToFilesystem;
				m_fb.Description = m_description;
				m_fb.StartLocation = FolderBrowserFolder.Desktop;
			}
			return m_fb.ShowDialog();
		}

		internal DialogResult ShowDialog() 
		{
			DialogResult dr = DialogResult.None;
			//m_fb.DirectoryPath = DUtils.lastFolder;
			dr = RunDialog();
            if(dr == DialogResult.OK) 
			{
				m_returnPath = m_fb.DirectoryPath;
			} 
			else 
			{
				m_returnPath = string.Empty;
			}
			return dr;
		}

	}


}
