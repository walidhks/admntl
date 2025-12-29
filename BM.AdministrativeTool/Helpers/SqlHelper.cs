using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BM.AdministrativeTool.Model;
using Ionic.Zip;
using Ionic.Zlib;

namespace BM.AdministrativeTool.Helpers
{
	// Token: 0x02000021 RID: 33
	internal class SqlHelper
	{
		// Token: 0x060000C3 RID: 195 RVA: 0x00004994 File Offset: 0x00002B94
		public static void DeleteDataBase(SqlConnection connection, string bmLabName)
		{
			string cmdText = string.Concat(new string[]
			{
				"use[master]ALTER DATABASE ",
				bmLabName,
				" SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE ",
				bmLabName,
				" ;"
			});
			using (SqlCommand sqlCommand = new SqlCommand(cmdText, connection))
			{
				sqlCommand.ExecuteNonQuery();
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000049FC File Offset: 0x00002BFC
		public static int RestoreDataBase(SqlConnection conn, string oldDbName, string backUpName, string newDbName, string oldDbLog = null)
		{
			SqlCommand sqlCommand = new SqlCommand("USE [master] select DB_ID('" + newDbName + "') id", conn);
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
			sqlDataReader.Read();
			int num;
			bool flag = int.TryParse(sqlDataReader["id"].ToString(), out num);
			int result;
			if (flag)
			{
				result = -999;
			}
			else
			{
				string sqlServerDataPAth = SqlHelper.GetSqlServerDataPAth(conn);
				string cmdText = "USE [master] \r\nRESTORE DATABASE @newDbName FROM  DISK = @backUpName WITH  FILE = 1,MOVE @oldDbName TO @a,MOVE @oldDbName_log TO @b,  NOUNLOAD,  STATS = 5";
				sqlCommand = new SqlCommand(cmdText, conn);
				string str = Path.Combine(sqlServerDataPAth, newDbName);
				string value = oldDbLog ?? (oldDbName + "_log");
				sqlCommand.Parameters.Add(new SqlParameter
				{
					ParameterName = "@newDbName",
					Value = newDbName,
					Size = 500
				});
				sqlCommand.Parameters.Add(new SqlParameter
				{
					ParameterName = "@backUpName",
					Value = backUpName,
					Size = 500
				});
				sqlCommand.Parameters.Add(new SqlParameter
				{
					ParameterName = "@oldDbName",
					Value = oldDbName,
					Size = 500
				});
				sqlCommand.Parameters.Add(new SqlParameter
				{
					ParameterName = "@oldDbName_log",
					Value = value,
					Size = 500
				});
				sqlCommand.Parameters.Add(new SqlParameter
				{
					ParameterName = "@a",
					Value = str + ".mdf",
					Size = 500
				});
				sqlCommand.Parameters.Add(new SqlParameter
				{
					ParameterName = "@b",
					Value = str + "_log.mdf",
					Size = 500
				});
				result = sqlCommand.ExecuteNonQuery();
			}
			return result;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004BD4 File Offset: 0x00002DD4
		public static int RestoreDataBase(SqlConnection conn, string workDir, string oldDbName, string backUpName, string newDbName, string oldDbLog = null)
		{
			SqlCommand sqlCommand = new SqlCommand("select DB_ID('" + newDbName + "') id", conn);
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
			sqlDataReader.Read();
			int num;
			bool flag = int.TryParse(sqlDataReader["id"].ToString(), out num);
			int result;
			if (flag)
			{
				result = -999;
			}
			else
			{
				string cmdText = "USE [master] \r\nRESTORE DATABASE @newDbName FROM  DISK = @backUpName WITH  FILE = 1,MOVE @oldDbName TO @a,MOVE @oldDbName_log TO @b,  NOUNLOAD,  STATS = 5";
				sqlCommand = new SqlCommand(cmdText, conn);
				bool flag2 = !Directory.Exists(Path.Combine(workDir, "DATA"));
				if (flag2)
				{
					Directory.CreateDirectory(Path.Combine(workDir, "DATA"));
				}
				string str = Path.Combine(workDir, "DATA", newDbName);
				string value = oldDbLog ?? (oldDbName + "_log");
				sqlCommand.Parameters.Add(new SqlParameter
				{
					ParameterName = "@newDbName",
					Value = newDbName,
					Size = 500
				});
				sqlCommand.Parameters.Add(new SqlParameter
				{
					ParameterName = "@backUpName",
					Value = Path.Combine(workDir, backUpName),
					Size = 500
				});
				sqlCommand.Parameters.Add(new SqlParameter
				{
					ParameterName = "@oldDbName",
					Value = oldDbName,
					Size = 500
				});
				sqlCommand.Parameters.Add(new SqlParameter
				{
					ParameterName = "@oldDbName_log",
					Value = value,
					Size = 500
				});
				sqlCommand.Parameters.Add(new SqlParameter
				{
					ParameterName = "@a",
					Value = str + ".mdf",
					Size = 500
				});
				sqlCommand.Parameters.Add(new SqlParameter
				{
					ParameterName = "@b",
					Value = str + "_log.mdf",
					Size = 500
				});
				result = sqlCommand.ExecuteNonQuery();
			}
			return result;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004DDC File Offset: 0x00002FDC
		private static string GetSqlServerDataPAth(SqlConnection conn)
		{
			string cmdText = "select SERVERPROPERTY('InstanceDefaultDataPath') AS InstanceDefaultDataPath";
			SqlCommand sqlCommand = new SqlCommand(cmdText, conn);
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
			sqlDataReader.Read();
			return sqlDataReader["InstanceDefaultDataPath"].ToString();
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004E1C File Offset: 0x0000301C
		private static string GetSqlServerBackupPAth(SqlConnection conn)
		{
			string cmdText = "EXEC master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'Software\\Microsoft\\MSSQLServer\\MSSQLServer',N'BackupDirectory'";
			SqlCommand sqlCommand = new SqlCommand(cmdText, conn);
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
			sqlDataReader.Read();
			return sqlDataReader["Data"].ToString();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004E5C File Offset: 0x0000305C
		public static void ShrinkDataBase(SqlConnection connection, string bmLabName)
		{
			string cmdText = "use[master]DBCC SHRINKDATABASE (" + bmLabName + ", 0) ;";
			using (SqlCommand sqlCommand = new SqlCommand(cmdText, connection))
			{
				sqlCommand.ExecuteNonQuery();
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004EA8 File Offset: 0x000030A8
		public static void CheckSp_BackupDatabases(SqlConnection connection)
		{
			SqlCommand sqlCommand = new SqlCommand("use [Master]IF object_id('sp_BackupDatabases') IS NOT NULL select 1 id else select 0 id", connection);
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
			sqlDataReader.Read();
			bool flag = int.Parse(sqlDataReader["id"].ToString()) > 0;
			if (!flag)
			{
				string[] array = sp_BackupDatabases.sp_Backup.Split(new string[]
				{
					"go",
					"GO"
				}, StringSplitOptions.None);
				foreach (string cmdText in array)
				{
					try
					{
						using (SqlCommand sqlCommand2 = new SqlCommand(cmdText, connection))
						{
							sqlCommand2.ExecuteNonQuery();
						}
					}
					catch (Exception arg)
					{
						throw new Exception(string.Format("Can't Run Script !!\n file ='sp_BackupDatabases'\n error ='{0}'\n", arg));
					}
				}
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004F88 File Offset: 0x00003188
		public static void CreateDataBaseBackup(SqlConnection connection, string dbName, string backDirectoryPath, string backupType)
		{
			string cmdText = string.Concat(new string[]
			{
				"EXEC sp_BackupDatabases @backupLocation='",
				backDirectoryPath,
				"\\', @databaseName='",
				dbName,
				"', @backupType='",
				backupType,
				"'"
			});
			using (SqlCommand sqlCommand = new SqlCommand(cmdText, connection))
			{
				sqlCommand.ExecuteNonQuery();
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004FFC File Offset: 0x000031FC
		public static void RestoreBmLimsDb()
		{
			try
			{
				Console.WriteLine("please select your backup file:");
				OpenFileDialog openFileDialog = new OpenFileDialog();
				bool flag = openFileDialog.ShowDialog() != DialogResult.OK;
				if (flag)
				{
					throw new Exception("no File selected !!");
				}
				Console.WriteLine(openFileDialog.FileName);
				string connectionString = SqlHelper.ReplaceInitialCatalog(ConfigurationManager.ConnectionStrings["ClinModel"].ConnectionString + "Application Name=MyApp", "Master");
				using (SqlConnection sqlConnection = new SqlConnection(connectionString))
				{
					sqlConnection.Open();
					bool flag2 = SqlHelper.TryBackupBmlims(sqlConnection) == 1;
					if (flag2)
					{
						SqlHelper.DeleteDataBase(sqlConnection, "BMlims");
					}
					SqlHelper.RestoreDataBase(sqlConnection, "BMlims", openFileDialog.FileName, "BMlims", null);
					SqlHelper.RunScriptOnDataBase(sqlConnection, null, "Scripts\\SQL\\RestoreDbPostScript\\01 Enable broker.sql", true);
					SqlHelper.RunScriptOnDataBase(sqlConnection, null, "Scripts\\SQL\\RestoreDbPostScript\\02 Create login.sql", true);
					SqlHelper.RunScriptOnDataBase(sqlConnection, null, "Scripts\\SQL\\RestoreDbPostScript\\03 Create User.sql", true);
					SqlHelper.RunScriptOnDataBase(sqlConnection, null, "Scripts\\SQL\\RestoreDbPostScript\\04 User permission.sql", true);
					SqlHelper.RunScriptOnDataBase(sqlConnection, null, "Scripts\\SQL\\RestoreDbPostScript\\05 Drop Trigger.sql", true);
					SqlHelper.RunScriptOnDataBase(sqlConnection, null, "Scripts\\SQL\\RestoreDbPostScript\\06 Create Trigger.sql", false);
					SqlHelper.RunScriptOnDataBase(sqlConnection, null, "Scripts\\SQL\\RestoreDbPostScript\\07 CHECK_POLICY.sql", true);
					sqlConnection.Close();
				}
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
			Console.WriteLine("Work done !!");
			Console.ReadLine();
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000517C File Offset: 0x0000337C
		public static int TryBackupBmlims()
		{
			int result;
			using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ClinModel"].ConnectionString + "Application Name=MyApp"))
			{
				sqlConnection.Open();
				int num = SqlHelper.TryBackupBmlims(sqlConnection);
				sqlConnection.Close();
				result = num;
			}
			return result;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000051E4 File Offset: 0x000033E4
		public static int TryBackupBmlims(SqlConnection sc)
		{
			SqlCommand sqlCommand = new SqlCommand("select DB_ID('BMlims') id", sc);
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
			sqlDataReader.Read();
			int num;
			bool flag = !int.TryParse(sqlDataReader["id"].ToString(), out num);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				SqlHelper.CheckSp_BackupDatabases(sc);
				string backupDir = SqlHelper.GetBackupDir();
				SqlHelper.CreateDataBaseBackup(sc, "BMlims", backupDir, "F");
				result = 1;
			}
			return result;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005258 File Offset: 0x00003458
		private static string GetBackupDir()
		{
			bool flag = Directory.Exists("D:\\Backup");
			string result;
			if (flag)
			{
				result = "D:\\Backup";
			}
			else
			{
				Console.WriteLine("Select database backup folder:");
				FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
				bool flag2 = folderBrowserDialog.ShowDialog() != DialogResult.OK;
				if (flag2)
				{
					throw new Exception("no File selected !!");
				}
				result = folderBrowserDialog.SelectedPath;
			}
			return result;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000052B4 File Offset: 0x000034B4
		public static string GetEmptyActiveDataBase(string dbName, string connString)
		{
			string result;
			using (SqlConnection sqlConnection = new SqlConnection(connString))
			{
				sqlConnection.Open();
				SqlHelper.CheckSp_BackupDatabases(sqlConnection);
				string text = Path.Combine(SqlHelper.GetSqlServerBackupPAth(sqlConnection), "EmptyActiveDataBases");
				bool flag = !Directory.Exists(text);
				if (flag)
				{
					Directory.CreateDirectory(text);
				}
				string path = SqlHelper.CreateDbBackup(sqlConnection, dbName, text, true);
				string fileName = Path.GetFileName(path);
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
				string text2 = string.Format("Test_{0}_{1:ddMMyyyy_HHmmss}", dbName, DateTime.Now);
				int num = SqlHelper.RestoreDataBase(sqlConnection, text, fileNameWithoutExtension, fileName, text2, null);
				File.Delete(path);
				string connectionString = SqlHelper.ReplaceInitialCatalog(connString, text2);
				using (SqlConnection sqlConnection2 = new SqlConnection(connectionString))
				{
					sqlConnection2.Open();
					Console.WriteLine("run: trancate");
					SqlHelper.RunScriptOnDataBase(sqlConnection2, null, "Scripts\\SQL\\trancate.sql", true);
					Console.WriteLine("run: Empty DB 1");
					SqlHelper.RunScriptOnDataBase(sqlConnection2, null, "Scripts\\SQL\\Empty DB 1.sql", true);
					Console.WriteLine("run: InitParamDict");
					SqlHelper.RunScriptOnDataBase(sqlConnection2, null, "Scripts\\SQL\\InitParamDict.sql", true);
					Console.WriteLine("run: DeleteUsers");
					SqlHelper.RunScriptOnDataBase(sqlConnection2, null, "Scripts\\SQL\\DeleteUsers.sql", true);
					Console.WriteLine("run: Enable All constraints");
					SqlHelper.RunScriptOnDataBase(sqlConnection2, null, "Scripts\\SQL\\Enable All constraints.sql", true);
					sqlConnection2.Close();
				}
				SqlHelper.ShrinkDataBase(sqlConnection, text2);
				string text3 = SqlHelper.CreateDbBackup(sqlConnection, text2, text, true);
				SqlHelper.DeleteDataBase(sqlConnection, text2);
				string text4 = SqlHelper.CompressFile(text3);
				File.Delete(text3);
				sqlConnection.Close();
				result = text4;
			}
			return result;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005478 File Offset: 0x00003678
		public static string CreateDbBackup(SqlConnection conn, string dbName, string backDirectoryPath, bool isFullBackup)
		{
			string cmdText = string.Concat(new string[]
			{
				"EXEC sp_BackupDatabases @backupLocation='",
				backDirectoryPath,
				"\\', @databaseName='",
				dbName,
				"', @backupType='",
				isFullBackup ? "F" : "D",
				"'"
			});
			string result;
			using (SqlCommand sqlCommand = new SqlCommand(cmdText, conn))
			{
				sqlCommand.ExecuteNonQuery();
				DirectoryInfo directoryInfo = new DirectoryInfo(backDirectoryPath);
				FileInfo fileInfo = (from f in directoryInfo.GetFiles()
				orderby f.LastWriteTime descending
				select f).First<FileInfo>();
				string text = Path.Combine(backDirectoryPath, dbName + ".bak");
				bool flag = File.Exists(text);
				if (flag)
				{
					File.Delete(text);
				}
				File.Move(fileInfo.FullName, text);
				result = text;
			}
			return result;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000556C File Offset: 0x0000376C
		public static void RunScriptOnDataBase(SqlConnection connection, string dbName, string scriptPath, bool separateBlocks = true)
		{
			List<string> list;
			if (!separateBlocks)
			{
				(list = new List<string>()).Add(File.ReadAllText(scriptPath));
			}
			else
			{
				list = File.ReadAllText(scriptPath).Split(new string[]
				{
					"go",
					"GO"
				}, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
			}
			List<string> list2 = list;
			list2.ForEach(delegate(string sqlScript)
			{
				sqlScript = (string.IsNullOrWhiteSpace(dbName) ? "" : ("use[" + dbName + "]")) + sqlScript;
				using (SqlCommand sqlCommand = new SqlCommand(sqlScript, connection))
				{
					sqlCommand.CommandTimeout = 0;
					sqlCommand.ExecuteNonQuery();
				}
			});
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000055E0 File Offset: 0x000037E0
		private static string CompressFile(string filePath)
		{
			string result;
			using (ZipFile zipFile = new ZipFile())
			{
				zipFile.UseZip64WhenSaving = Zip64Option.Always;
				zipFile.AddFile(filePath, "");
				zipFile.Comment = "This zip was created at " + DateTime.Now.ToString("G");
				zipFile.CompressionLevel = CompressionLevel.BestCompression;
				string text = Path.ChangeExtension(filePath, ".zip");
				zipFile.Save(Path.Combine(new string[]
				{
					text
				}));
				result = text;
			}
			return result;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000026A0 File Offset: 0x000008A0
		public static string GetInitialCatalog(string cnc)
		{
			return Regex.Match(cnc, "(initial catalog=)([^;]+)(;)").Groups[2].Value;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000026BD File Offset: 0x000008BD
		public static string ReplaceInitialCatalog(string cnc, string initialCatalog)
		{
			return Regex.Replace(cnc, "(initial catalog=)([^;]+)(;)", "$1" + initialCatalog + "$3");
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005678 File Offset: 0x00003878
		public static void RemoveTestDataForLancemant()
		{
			string connectionString = ConfigurationManager.ConnectionStrings["ClinModel"].ConnectionString + "Application Name=MyApp";
			using (SqlConnection sqlConnection = new SqlConnection(connectionString))
			{
				sqlConnection.Open();
				SqlHelper.RunScriptOnDataBase(sqlConnection, null, "Scripts\\SQL\\trancate.sql", true);
				SqlHelper.RunScriptOnDataBase(sqlConnection, null, "Scripts\\SQL\\SuppTestPourLancement.sql", true);
				sqlConnection.Close();
			}
		}
	}
}
