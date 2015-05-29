using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace SVNUpdateAllDir
{
	class Program
	{
		static void Main(string[] args)
		{
			SVN_Update_All_Sub_Dir();
		}

		static void SVN_Update_All_Sub_Dir()
		{
			string strCurrent = Directory.GetCurrentDirectory();
			DirectoryInfo dirCurrent = new DirectoryInfo(strCurrent);
			if (dirCurrent != null)
			{
				System.Diagnostics.Process proUpdate = new System.Diagnostics.Process();
				proUpdate.StartInfo.FileName = "cmd.exe";
				proUpdate.StartInfo.UseShellExecute = false;
				proUpdate.StartInfo.RedirectStandardInput = true;
				proUpdate.Start();
				
				string strSVNCmd = "svn update ";
				string strLogFile = " SVNUpdateAllDir.log";
				
				DateTime dtStart = DateTime.Now;
				string strOutput = "ECHO Now update start at " + dtStart.ToString("u") + " > " + strLogFile;
				proUpdate.StandardInput.WriteLine(strOutput);
				proUpdate.StandardInput.WriteLine("ECHO **********" + " >> " + strLogFile);

				int nCount = 0;
				DirectoryInfo[] subDirectories = dirCurrent.GetDirectories();
				foreach (DirectoryInfo dirInfo in subDirectories)
				{
					string strDirName = dirInfo.Name.ToString();
					DirectoryInfo[] subDir2 = dirInfo.GetDirectories();
					foreach (DirectoryInfo dir2 in subDir2)
					{
						string strTempDir = dir2.Name.ToString();
						if (IsIgnoreDir(strTempDir))
						{
							proUpdate.StandardInput.WriteLine("ECHO Ignore " + strDirName + "\\" + strTempDir + " >> " + strLogFile);
							continue;
						}
						string strCMD = strSVNCmd + strDirName;
						strCMD += "\\" + strTempDir;
						strCMD += " >> ";
						strCMD += strLogFile;
						proUpdate.StandardInput.WriteLine(strCMD);

						nCount++;
					}
				}

				proUpdate.StandardInput.WriteLine("ECHO **********" + " >> " + strLogFile);
				DateTime dtEnd = DateTime.Now;
				strOutput = "ECHO Update " + nCount.ToString() + " Directories.";
				//strOutput += "Finished at " + dtEnd.ToString("u");
				strOutput += " >> " + strLogFile;
				proUpdate.StandardInput.WriteLine(strOutput);

				proUpdate.StandardInput.WriteLine("exit");
				proUpdate.WaitForExit();
				proUpdate.Close();
			}
		}
		
		static List<string> listIgnoreDir = new List<string>();
		static bool IsIgnoreDir(string strDir)
		{
			if (0 == listIgnoreDir.Count)
			{
				listIgnoreDir.Add(".svn");
			}
			foreach (string sIgnoreDir in listIgnoreDir)
			{
				if (0 == strDir.CompareTo(sIgnoreDir))
				{
					return true;
				}
			}

			return false;
		}

	}
}
