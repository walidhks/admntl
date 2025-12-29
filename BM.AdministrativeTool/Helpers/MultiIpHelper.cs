using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text.RegularExpressions;

namespace BM.AdministrativeTool.Helpers
{
	// Token: 0x0200001E RID: 30
	internal class MultiIpHelper
	{
		// Token: 0x060000AB RID: 171 RVA: 0x00004464 File Offset: 0x00002664
		public static void Start()
		{
			Console.Clear();
			List<MultiIpInfo> interfacesIPs = MultiIpHelper.GetInterfacesIPs();
			int maxLength = interfacesIPs.Max((MultiIpInfo p) => p.InterfaceAlias.Length);
			MultiIpInfo multiIpInfo = interfacesIPs.FirstOrDefault(delegate(MultiIpInfo p)
			{
				string ipv4Address = p.IPv4Address;
				return ipv4Address != null && ipv4Address.Contains("192.168");
			});
			foreach (MultiIpInfo multiIpInfo2 in interfacesIPs)
			{
				Console.ForegroundColor = ((multiIpInfo2 == multiIpInfo) ? ConsoleColor.Yellow : ConsoleColor.White);
				Console.WriteLine(string.Format("{0}\t{1}\t{2}  {3}", new object[]
				{
					interfacesIPs.IndexOf(multiIpInfo2),
					multiIpInfo2.InterfaceIndex,
					MultiIpHelper.ToMaxLength(multiIpInfo2.InterfaceAlias, maxLength),
					multiIpInfo2.IPv4Address
				}));
			}
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write("Select a network Interface (default ");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write(interfacesIPs.IndexOf(multiIpInfo));
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(") :");
			string text = Console.ReadLine();
			text = (string.IsNullOrWhiteSpace(text) ? interfacesIPs.IndexOf(multiIpInfo).ToString() : text);
			multiIpInfo = interfacesIPs[int.Parse(text)];
			string text2 = "220,221,222,223,224,225";
			Console.Write("\nEnter Address to add (default ");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write(text2);
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(") :");
			string text3 = Console.ReadLine();
			Console.WriteLine("Please Wait...");
			text2 = (string.IsNullOrWhiteSpace(text3) ? text2 : text3);
			foreach (string replacement in text2.Split(new char[]
			{
				','
			}, StringSplitOptions.RemoveEmptyEntries))
			{
				PowerShell powerShell = PowerShell.Create();
				powerShell.AddCommand("New-NetIPAddress");
				powerShell.Commands.AddParameter("IPAddress", Regex.Replace(multiIpInfo.IPv4Address, "\\d+$", replacement));
				powerShell.Commands.AddParameter("PrefixLength", "24");
				powerShell.Commands.AddParameter("InterfaceAlias", multiIpInfo.InterfaceAlias);
				powerShell.Commands.AddParameter("SkipAsSource", false);
				powerShell.Invoke();
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000046EC File Offset: 0x000028EC
		public static List<MultiIpInfo> GetInterfacesIPs()
		{
			PowerShell powerShell = PowerShell.Create();
			powerShell.AddCommand("Get-NetIPAddress");
			return (from p in powerShell.Invoke().Select(delegate(PSObject psObject)
			{
				MultiIpInfo multiIpInfo = new MultiIpInfo();
				PSPropertyInfo pspropertyInfo = psObject.Properties.FirstOrDefault((PSPropertyInfo p) => p.Name == "InterfaceIndex");
				multiIpInfo.InterfaceIndex = (uint)((pspropertyInfo != null) ? pspropertyInfo.Value : null);
				PSPropertyInfo pspropertyInfo2 = psObject.Properties.FirstOrDefault((PSPropertyInfo p) => p.Name == "IPv4Address");
				multiIpInfo.IPv4Address = (string)((pspropertyInfo2 != null) ? pspropertyInfo2.Value : null);
				PSPropertyInfo pspropertyInfo3 = psObject.Properties.FirstOrDefault((PSPropertyInfo p) => p.Name == "InterfaceAlias");
				multiIpInfo.InterfaceAlias = (string)((pspropertyInfo3 != null) ? pspropertyInfo3.Value : null);
				return multiIpInfo;
			})
			orderby p.InterfaceIndex
			select p).ToList<MultiIpInfo>();
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004764 File Offset: 0x00002964
		public static string ToMaxLength(string input, int maxLength)
		{
			string str = new string(' ', maxLength - input.Length);
			return input + str;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004790 File Offset: 0x00002990
		public static void ChangePowerPlanHighPerformance()
		{
			Console.Write("Old Power plan :");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(MultiIpHelper.GetCurrentPowerPlan());
			Console.ForegroundColor = ConsoleColor.White;
			PowerShell powerShell = PowerShell.Create();
			powerShell.AddScript("Scripts\\PS1\\ChangePowerPlan.ps1");
			Collection<PSObject> collection = powerShell.Invoke();
			Console.Write("New Power plan :");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(MultiIpHelper.GetCurrentPowerPlan());
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("Press any Key to continue ...");
			Console.ReadKey();
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004814 File Offset: 0x00002A14
		public static string GetCurrentPowerPlan()
		{
			PowerShell powerShell = PowerShell.Create();
			powerShell.Commands.AddScript("PowerCfg.exe -LIST");
			Collection<PSObject> source = powerShell.Invoke();
			string text = (from e in source
			select e.ToString()).FirstOrDefault((string e) => e.EndsWith("*"));
			bool flag = string.IsNullOrWhiteSpace(text);
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = Regex.Match(text, "\\((.+)\\)").Groups[1].Value;
			}
			return result;
		}
	}
}
