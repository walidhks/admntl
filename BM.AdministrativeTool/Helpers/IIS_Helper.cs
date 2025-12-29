using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace BM.AdministrativeTool.Helpers
{
	// Token: 0x0200000C RID: 12
	internal class IIS_Helper
	{
		// Token: 0x06000048 RID: 72 RVA: 0x000022B3 File Offset: 0x000004B3
		public static void CreateSite(string name, int port, string userName, string password)
		{
			IIS_Helper.CreateAppPool(name, userName, password);
			IIS_Helper.CreateSite(name, port);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002BC8 File Offset: 0x00000DC8
		public static void CreateAppPool(string name, string userName, string password)
		{
			PowerShell powerShell = PowerShell.Create();
			powerShell.Commands.AddScript("Import-Module WebAdministration\r\nNew-WebAppPool –Name \"" + name + "\"");
			powerShell.Commands.AddScript("$AppPool = Get-Item \"IIS:\\AppPools\\" + name + "\"");
			powerShell.Commands.AddScript("$AppPool | Stop-WebAppPool ");
			powerShell.Commands.AddScript("$AppPool.enable32BitAppOnWin64 = $True");
			powerShell.Commands.AddScript("$AppPool.ProcessModel.IdentityType =\"SpecificUser\"");
			powerShell.Commands.AddScript("$AppPool.ProcessModel.Username =\"" + userName + "\"");
			powerShell.Commands.AddScript("$AppPool.ProcessModel.Password =\"" + password + "\"");
			powerShell.Commands.AddScript("$AppPool | Set-Item");
			powerShell.Commands.AddScript("$AppPool | Start-WebAppPool");
			Collection<PSObject> collection = powerShell.Invoke();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002CA8 File Offset: 0x00000EA8
		public static void CreateSite(string name, int port)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo("C:\\").GetDirectories().FirstOrDefault((DirectoryInfo d) => d.FullName.ToLower().Contains("websites"));
			string text = ((directoryInfo != null) ? directoryInfo.FullName : null) ?? "C:\\_WebSites";
			PowerShell powerShell = PowerShell.Create();
			powerShell.Commands.AddScript(string.Format("Import-Module WebAdministration\r\nNew-Website -Name \"{0}\" -Port {1} -PhysicalPath \"{2}\\{3}\" -ApplicationPool \"{4}\" -Force", new object[]
			{
				name,
				port,
				text,
				name,
				name
			}));
			Collection<PSObject> collection = powerShell.Invoke();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002D44 File Offset: 0x00000F44
		public static void StartCreateSiteWizard()
		{
			Console.Clear();
			Console.WriteLine("Enter Site name :");
			string name = Console.ReadLine();
			Console.WriteLine("Enter Site port :");
			int port = int.Parse(Console.ReadLine());
			IIS_Helper.CreateSite(name, port, ".\\administrator", "Slimani*1920");
		}
	}
}
