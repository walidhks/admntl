using System;
using System.Management;
using Microsoft.Win32;

namespace BM.AdministrativeTool.Helpers
{
	// Token: 0x0200001D RID: 29
	internal class MachineNameHelper
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x000042C0 File Offset: 0x000024C0
		public static void Start()
		{
			Console.Clear();
			string text = "BMLabServer";
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("** Changes Will take effect after the next restart !! **\n");
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write("Enter new Machine Name (Default ");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write(text);
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(") :");
			string text2 = Console.ReadLine();
			Console.WriteLine("Please Wait...");
			text2 = (string.IsNullOrWhiteSpace(text2) ? text : text2);
			MachineNameHelper.SetComputerName(text2);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004348 File Offset: 0x00002548
		public static bool SetComputerName(string Name)
		{
			string text = "SYSTEM\\CurrentControlSet\\Control\\ComputerName\\ComputerName";
			try
			{
				string path = "Win32_ComputerSystem.Name='" + Environment.MachineName + "'";
				using (ManagementObject managementObject = new ManagementObject(new ManagementPath(path)))
				{
					ManagementBaseObject methodParameters = managementObject.GetMethodParameters("Rename");
					methodParameters["Name"] = Name;
					ManagementBaseObject managementBaseObject = managementObject.InvokeMethod("Rename", methodParameters, null);
					uint num = (uint)Convert.ChangeType(managementBaseObject.Properties["ReturnValue"].Value, typeof(uint));
					bool flag = num > 0U;
					if (flag)
					{
						throw new Exception("Computer could not be changed due to unknown reason.");
					}
				}
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(text);
				bool flag2 = registryKey == null;
				if (flag2)
				{
					throw new Exception("Registry location '" + text + "' is not readable.");
				}
				registryKey.Close();
				registryKey.Dispose();
			}
			catch (Exception ex)
			{
				return false;
			}
			return true;
		}
	}
}
