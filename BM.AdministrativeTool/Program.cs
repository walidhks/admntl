using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using BM.AdministrativeTool.Helpers;
using BM.AdministrativeTool.Model;

namespace BM.AdministrativeTool
{
	// Token: 0x02000002 RID: 2
	internal class Program
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		[STAThread]
		public static void Main()
		{
			Program.Connection(3);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000270C File Offset: 0x0000090C
		public static bool Do()
		{
			Console.Clear();
			foreach (object obj in Enum.GetValues(typeof(MainMenuOptions)))
			{
				MainMenuOptions mainMenuOptions = (MainMenuOptions)obj;
				Console.WriteLine(string.Format("{0}-\t{1}", (int)mainMenuOptions, Program.GetDisplay(mainMenuOptions)));
			}
			int num = int.Parse(Console.ReadLine());
			bool result;
			if (!Program.CheckConnection())
			{
				Program.Connection(3);
				result = false;
			}
			else
			{
				switch (num)
				{
				case 0:
					MultiIpHelper.Start();
					break;
				case 1:
					MachineNameHelper.Start();
					break;
				case 2:
					MultiIpHelper.ChangePowerPlanHighPerformance();
					break;
				case 3:
					WindowsTasksHelper.CreateCheckQuartzStateTask();
					break;
				case 4:
					InstrumentHelper.StartInstallAllUsedInstrumentsServicesWizard();
					break;
				case 5:
					InstrumentHelper.StartInstallWizard();
					break;
				case 6:
					InstrumentHelper.StartInstall_ByInstrumentKind_Wizard();
					break;
				case 7:
					InstrumentHelper.StartUninstallWizard();
					break;
				case 8:
					InstrumentHelper.StartReplaceServerIpAddress();
					break;
				case 9:
					InstrumentHelper.DeleteNotUsedInstruments();
					break;
				case 10:
					SqlTriggersHelper.ChangeSampleCodeTrigger();
					break;
				case 11:
					SqlHelper.RestoreBmLimsDb();
					break;
				case 12:
				{
					string text = ConfigurationManager.ConnectionStrings["ClinModel"].ConnectionString + "Application Name=MyApp";
					string emptyActiveDataBase = SqlHelper.GetEmptyActiveDataBase(SqlHelper.GetInitialCatalog(text), text);
					Console.WriteLine(emptyActiveDataBase);
					if (File.Exists(emptyActiveDataBase))
					{
						Process.Start("explorer.exe", "/select, " + emptyActiveDataBase);
					}
					Console.ReadKey();
					break;
				}
				case 13:
					SqlHelper.RemoveTestDataForLancemant();
					break;
				case 14:
					InstrumentHelper.SetDefaultInstrument_ByAtMapping();
					break;
				default:
					return false;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000028D8 File Offset: 0x00000AD8
		public static string GetDisplay(Enum value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			DisplayAttribute[] array = (DisplayAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false);
			string result;
			if (array.Length != 1)
			{
				result = value.ToString();
			}
			else
			{
				result = array[0].Name;
			}
			return result;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002934 File Offset: 0x00000B34
		public static bool CheckConnection()
		{
			bool result;
			if (!(DateTime.Now < Program.DisconnectTime))
			{
				result = false;
			}
			else
			{
				Program.DisconnectTime = DateTime.Now.AddMinutes(60.0);
				result = true;
			}
			return result;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002974 File Offset: 0x00000B74
		public static void Connection(int maxTry)
		{
			Console.Clear();
			Console.Write("********************************** LOGIN *****************************\n");
			Console.Write("\nUser name :SU\n");
			Program.DisconnectTime = DateTime.Now.AddMinutes(60.0);
			while (Program.Do())
			{
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000029BC File Offset: 0x00000BBC
		private static string ReadPassword()
		{
			string text = null;
			for (;;)
			{
				ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
				if (consoleKeyInfo.Key == ConsoleKey.Enter)
				{
					break;
				}
				if (consoleKeyInfo.Key == ConsoleKey.Backspace)
				{
					if (text != null && text.Length > 0)
					{
						Console.Write("\b \b");
						text = text.Remove(text.Length - 1);
					}
				}
				else
				{
					Console.Write("*");
					text += consoleKeyInfo.KeyChar.ToString();
				}
			}
			return text;
		}

		// Token: 0x04000001 RID: 1
		public static DateTime DisconnectTime = DateTime.MaxValue;
	}
}
