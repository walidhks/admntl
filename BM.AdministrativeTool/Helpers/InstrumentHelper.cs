using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Configuration.Install;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Xml;
using BM.AdministrativeTool.Model;

namespace BM.AdministrativeTool.Helpers
{
    internal class InstrumentHelper
    {
        public static void StartInstallWizard()
        {
            Console.Clear();
            using (LaboContext laboContext = new LaboContext())
            {
                List<Instrument> instruments = (from i in laboContext.Instrument
                                                where i.InstrumentUsed
                                                select i).ToList<Instrument>();
                int maxNameLength = instruments.Max((Instrument i) => i.InstrumentName.Length);
                instruments.ForEach(delegate (Instrument i)
                {
                    Console.WriteLine(string.Format("{0}-\t{1}\t{2}   {3}", new object[]
                    {
                        instruments.IndexOf(i),
                        i.InstrumentId,
                        MultiIpHelper.ToMaxLength(i.InstrumentName, maxNameLength),
                        i.MiddlewareServiceName
                    }));
                });
                Console.WriteLine("\nSelect instrument :");
                int index = int.Parse(Console.ReadLine());
                Instrument instrument = instruments[index];
                ServiceController serviceController = ServiceController.GetServices().FirstOrDefault((ServiceController s) => s.ServiceName == instrument.MiddlewareServiceName);
                bool flag = serviceController != null;
                if (flag)
                {
                    InstrumentHelper.Uninstall(instrument.MiddlewareServiceName);
                }
                InstrumentHelper.DoInstall(instrument, laboContext);
                Console.WriteLine("\nPress any key to continue ...");
            }
        }

        private static void DoInstall(Instrument instrument, LaboContext db)
        {
            Console.WriteLine("Start install service :" + instrument.InstrumentName);
            string text = "BM." + instrument.InstrumentName;
            text = text.Replace("/", "-");
            string text2 = "C:\\BM";
            string servicePath = InstrumentHelper.CopyServicesFiles(text2, text);
            InstrumentHelper.ApplyConfigurationChanges(Path.Combine(text2, text, "GbService.exe.config"), (long)instrument.InstrumentId, "127.0.0.1");
            InstrumentHelper.Install(text, servicePath);
            instrument.MiddlewareServiceName = text;
            db.SaveChanges();
        }

        private static void ApplyConfigurationChanges(string filePath, long instrumentId, string socketIpAddress)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filePath);
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/configuration/applicationSettings/GbService.Properties.Settings/setting");
            foreach (object obj in xmlNodeList)
            {
                XmlNode xmlNode = (XmlNode)obj;
                XmlAttribute xmlAttribute = xmlNode.Attributes["name"];
                bool flag = xmlAttribute.Value == "ID";
                if (flag)
                {
                    XmlNode xmlNode2 = xmlNode.ChildNodes.Item(0);
                    xmlNode2.InnerText = instrumentId.ToString();
                }
            }
            xmlDocument.Save(filePath);
        }

        private static string CopyServicesFiles(string rootFolder, string serviceFolder)
        {
            bool flag = !Directory.Exists(rootFolder);
            if (flag)
            {
                Directory.CreateDirectory(rootFolder);
            }
            string text = Path.Combine(rootFolder, serviceFolder);
            bool flag2 = !Directory.Exists(text);
            if (flag2)
            {
                Directory.CreateDirectory(text);
            }
            foreach (FileInfo fileInfo in new DirectoryInfo("ServiceFiles").GetFiles())
            {
                File.Copy(fileInfo.FullName, Path.Combine(text, fileInfo.Name), true);
            }
            return Path.Combine(text, "GbService.exe");
        }

        public static void Install(string serviceName, string servicePath = null)
        {
            InstrumentHelper.CreateInstaller(serviceName, servicePath).Install(new Hashtable());
            ServiceController serviceController = ServiceController.GetServices().FirstOrDefault((ServiceController s) => s.ServiceName == serviceName);
            if (serviceController != null)
            {
                serviceController.Start();
            }
        }

        public static void Uninstall(string serviceName)
        {
            InstrumentHelper.CreateInstaller(serviceName, null).Uninstall(null);
        }

        private static Installer CreateInstaller(string serviceName, string servicePath = null)
        {
            TransactedInstaller transactedInstaller = new TransactedInstaller();
            transactedInstaller.Installers.Add(new ServiceInstaller
            {
                ServiceName = serviceName,
                DisplayName = serviceName,
                StartType = ServiceStartMode.Automatic,
                DelayedAutoStart = true
            });
            transactedInstaller.Installers.Add(new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            });
            InstallContext installContext = new InstallContext(serviceName + ".install.log", null);
            installContext.Parameters["assemblypath"] = (string.IsNullOrEmpty(servicePath) ? Assembly.GetEntryAssembly().Location : Path.Combine(new string[]
            {
                servicePath
            }));
            transactedInstaller.Context = installContext;
            return transactedInstaller;
        }

        public static void StartReplaceServerIpAddress()
        {
            Console.Clear();
            using (LaboContext laboContext = new LaboContext())
            {
                List<Instrument> list = (from i in laboContext.Instrument
                                         where i.InstrumentUsed
                                         select i).ToList<Instrument>();
                List<string> usedIPs = (from i in list
                                        select string.IsNullOrEmpty(i.InstrumentPortName) ? null : Regex.Match(i.InstrumentPortName, "192\\.168\\.\\d+\\.\\d+").Value into e
                                        where !string.IsNullOrWhiteSpace(e)
                                        select e).Distinct<string>().ToList<string>();
                usedIPs.ForEach(delegate (string i)
                {
                    Console.WriteLine(string.Format("{0}-\t{1}", usedIPs.IndexOf(i), i));
                });
                Console.WriteLine("\nSelect Address to replace :");
                int index = int.Parse(Console.ReadLine());
                string addressToReplace = usedIPs[index];
                List<MultiIpInfo> list2 = MultiIpHelper.GetInterfacesIPs().Where(delegate (MultiIpInfo i)
                {
                    string ipv4Address = i.IPv4Address;
                    return ipv4Address != null && ipv4Address.StartsWith("192.168");
                }).ToList<MultiIpInfo>();
                int maxLength = list2.Max((MultiIpInfo p) => p.InterfaceAlias.Length);
                MultiIpInfo multiIpInfo = list2.FirstOrDefault(delegate (MultiIpInfo p)
                {
                    string ipv4Address = p.IPv4Address;
                    return ipv4Address != null && ipv4Address.Contains("192.168");
                });
                foreach (MultiIpInfo multiIpInfo2 in list2)
                {
                    Console.WriteLine(string.Format("{0}\t{1}  {2}", list2.IndexOf(multiIpInfo2), MultiIpHelper.ToMaxLength(multiIpInfo2.InterfaceAlias, maxLength), multiIpInfo2.IPv4Address));
                }
                Console.Write("Select a new IP address :");
                index = int.Parse(Console.ReadLine());
                string newIpAddress = list2[index].IPv4Address;
                list.ForEach(delegate (Instrument i)
                {
                    string instrumentPortName = i.InstrumentPortName;
                    i.InstrumentPortName = ((instrumentPortName != null) ? instrumentPortName.Replace(addressToReplace, newIpAddress) : null);
                });
                laboContext.SaveChanges();
                Console.WriteLine("Press any Key to continue ...");
                Console.ReadKey();
            }
        }

        public static void StartUninstallWizard()
        {
            Console.Clear();
            using (LaboContext laboContext = new LaboContext())
            {
                List<Instrument> instruments = (from i in laboContext.Instrument
                                                where i.InstrumentUsed
                                                select i).ToList<Instrument>();
                int maxNameLength = instruments.Max((Instrument i) => i.InstrumentName.Length);
                instruments.ForEach(delegate (Instrument i)
                {
                    Console.WriteLine(string.Format("{0}-\t{1}\t{2} {3}", new object[]
                    {
                        instruments.IndexOf(i),
                        i.InstrumentId,
                        MultiIpHelper.ToMaxLength(i.InstrumentName, maxNameLength),
                        i.MiddlewareServiceName
                    }));
                });
                Console.WriteLine("\nSelect instrument :");
                int index = int.Parse(Console.ReadLine());
                Instrument instrument = instruments[index];
                ServiceController serviceController = ServiceController.GetServices().FirstOrDefault((ServiceController s) => s.ServiceName == instrument.MiddlewareServiceName);
                bool flag = serviceController != null;
                if (flag)
                {
                    InstrumentHelper.Uninstall(instrument.MiddlewareServiceName);
                }
                instrument.MiddlewareServiceName = null;
                laboContext.SaveChanges();
                Console.WriteLine("\nPress any key to continue ...");
            }
        }

        public static void StartInstallAllUsedInstrumentsServicesWizard()
        {
            Console.Clear();
            using (LaboContext db = new LaboContext())
            {
                List<Instrument> source = (from i in db.Instrument
                                           where i.InstrumentUsed
                                           select i).ToList<Instrument>();
                int num = source.Max((Instrument i) => i.InstrumentName.Length);
                (from i in source
                 where i.InstrumentUsed && string.IsNullOrEmpty(i.MiddlewareServiceName)
                 select i).ToList<Instrument>().ForEach(delegate (Instrument instrument)
                 {
                     bool flag = ServiceController.GetServices().Any((ServiceController s) => s.ServiceName == instrument.MiddlewareServiceName);
                     if (!flag)
                     {
                         InstrumentHelper.DoInstall(instrument, db);
                     }
                 });
                Console.WriteLine("\nPress any key to continue ...");
            }
        }

        public static void StartInstall_ByInstrumentKind_Wizard()
        {
            LaboContext laboContext = new LaboContext();
            Console.Clear();
            Console.WriteLine("\nSelect instrument Kind Id:\n");

            List<InstrumentKindInfo> infos = (from s in File.ReadAllLines("InstrumentKindsInfo.txt").Skip(1)
                                              where !string.IsNullOrWhiteSpace(s)
                                              select s).Select(s => new InstrumentKindInfo(s)).ToList();

            List<string> list = File.ReadAllLines("KindsToMap.txt").Skip(1).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            foreach (string text in list)
            {
                string[] sp = text.Split(new char[]
                {
                    ','
                });
                InstrumentKindInfo kindName = infos.First((InstrumentKindInfo i) => i.Code == sp[0]);
                string c = sp[1];
                string mode = sp[2];
                (from s in sp.Skip(3)
                 where !string.IsNullOrWhiteSpace(s)
                 select s).ToList<string>().ForEach(delegate (string s)
                 {
                     infos.Add(new InstrumentKindInfo(s, kindName, c, mode));
                 });
            }
            Console.WriteLine(string.Join("\n", infos.Select((InstrumentKindInfo i, int j) => string.Format("{0}-\t\t{1}\t\t{2}", j, i.Code, i.Automate))) + "\n");
            InstrumentKindInfo instrumentKindInfo = infos.ElementAt(int.Parse(Console.ReadLine()));
            int num = int.Parse(instrumentKindInfo.Code);
            InstrumentKind instrumentKind;
            if ((instrumentKind = laboContext.InstrumentKind.Find(new object[]
            {
                num
            })) == null)
            {
                instrumentKind = InstrumentHelper.InsertInstrumentKind(laboContext, new InstrumentKind
                {
                    InstrumentKindId = num,
                    InstrumentKindName = instrumentKindInfo.AutomateKind
                });
            }
            InstrumentKind kind = instrumentKind;

            // =========================================================
            // GetInstrument inserts the data into DB (that part is done)
            // =========================================================
            Instrument instrument = InstrumentHelper.GetInstrument(laboContext, kind, instrumentKindInfo);

            // =========================================================
            // [OPTIMIZATION] If Serial (0), STOP HERE. 
            // Dont check service, dont uninstall, dont install.
            // =========================================================
            if (instrument.InstrumentBaudRate != null && instrument.InstrumentBaudRate.Trim() == "0")
            {
                Console.WriteLine("\n[INFO] Serial Instrument (BaudRate 0) - Database updated.");
                Console.WriteLine("[INFO] Skipping Service Installation as requested.");
            }
            else
            {
                // NORMAL LOGIC for Network Instruments
                ServiceController serviceController = ServiceController.GetServices().FirstOrDefault((ServiceController s) => s.ServiceName == instrument.MiddlewareServiceName);
                bool flag = serviceController != null;
                if (flag)
                {
                    InstrumentHelper.Uninstall(instrument.MiddlewareServiceName);
                }
                InstrumentHelper.DoInstall(instrument, laboContext);
            }

            Console.WriteLine("\n\n\npress any key to continue\n");
            Console.ReadKey();
        }

        private static InstrumentKind InsertInstrumentKind(LaboContext db, InstrumentKind kind)
        {
            string sql = string.Format("\r\nset identity_insert InstrumentKind on\r\ninsert into InstrumentKind(InstrumentKindId,InstrumentKindName) values({0},'{1}')\r\nset identity_insert InstrumentKind off \r\nselect 1", kind.InstrumentKindId, kind.InstrumentKindName);
            int num = db.Database.SqlQuery<int>(sql, new object[0]).First<int>();
            kind = db.InstrumentKind.Find(new object[]
            {
                kind.InstrumentKindId
            });
            return kind;
        }

        private static Instrument InsertInstrument(LaboContext db, InstrumentKind kind, InstrumentKindInfo info)
        {
            string instrumentName = info.Automate;
            int num = 1;
            while (db.Instrument.Any((Instrument i) => i.InstrumentName == instrumentName))
            {
                instrumentName = info.Automate + "_" + num++.ToString();
            }

            // Defaults
            System.IO.Ports.Parity? parity = System.IO.Ports.Parity.None;
            if (!string.IsNullOrEmpty(info.Parity) && Enum.TryParse(info.Parity, out System.IO.Ports.Parity p))
            {
                parity = p;
            }

            long? ParseLongOrNull(string val)
            {
                if (long.TryParse(val, out long result)) return result;
                return null;
            }

            bool? ParseBoolOrNull(string val)
            {
                if (string.IsNullOrWhiteSpace(val)) return null;
                if (val == "1" || val.Equals("true", StringComparison.OrdinalIgnoreCase)) return true;
                if (val == "0" || val.Equals("false", StringComparison.OrdinalIgnoreCase)) return false;
                return null;
            }

            Instrument instrument = new Instrument
            {
                InstrumentCode = kind.InstrumentKindId,
                InstrumentName = instrumentName,
                InstrumentUsed = true,
                InstrumentBaudRate = info.InstrumentBaudRate,
                InstrumentDataBits = string.IsNullOrWhiteSpace(info.InstrumentDataBits) ? null : info.InstrumentDataBits,
                InstrumentPortName = InstrumentHelper.GetInstrumentPortName(info),
                Mode = (string.IsNullOrWhiteSpace(info.Mode) ? 0 : int.Parse(info.Mode)),
                InstrumentParity = parity,
                L1 = ParseLongOrNull(info.L1) ?? 0,
                L2 = ParseLongOrNull(info.L2) ?? 0,
                L3 = ParseLongOrNull(info.L3) ?? 0,
                B1 = ParseBoolOrNull(info.B1) ?? false
            };
            db.Instrument.Add(instrument);
            db.SaveChanges();
            return instrument;
        }

        private static string GetInstrumentPortName(InstrumentKindInfo info)
        {
            string type = info.InstrumentBaudRate.Trim();

            // 1. SERIAL (BaudRate 0) -> DEFAULT VALUE
            if (type == "0")
            {
                if (!string.IsNullOrWhiteSpace(info.InstrumentPortName))
                    return info.InstrumentPortName;

                return "9600,None,One,8,COM1";
            }

            // 2. CLIENT (BaudRate 2) -> Use Default IP
            if (type == "2")
            {
                return "192.168.1.222," + InstrumentHelper.GetInstrumentPort(info);
            }

            // 3. SERVER (BaudRate 1) -> Ask for IP
            return InstrumentHelper.GetInstrumentIpAddress() + "," + InstrumentHelper.GetInstrumentPort(info);
        }

        private static string GetInstrumentPort(InstrumentKindInfo info)
        {
            LaboContext laboContext = new LaboContext();
            bool flag = !string.IsNullOrWhiteSpace(info.InstrumentPortName);
            string text;
            if (flag)
            {
                text = info.InstrumentPortName;
            }
            else
            {
                List<string> source = (from i in laboContext.Instrument.ToList<Instrument>()
                                       where !string.IsNullOrEmpty(i.InstrumentPortName) && Regex.IsMatch(i.InstrumentPortName, "^\\d+\\.\\d+\\.\\d+\\.\\d+,\\d+$")
                                       select i.InstrumentPortName).ToList<string>();
                List<string> list = (from i in source
                                     select i.Split(new char[]
                                     {
                    ','
                                     })[1]).ToList<string>();
                list.Add("30000");
                int num = list.Max(new Func<string, int>(int.Parse));
                text = (num + 1).ToString();
            }
            InstrumentHelper.OpenPort(text, info.Automate);
            return text;
        }

        private static void OpenPort(string port, string description)
        {
            PowerShell powerShell = PowerShell.Create();
            string script = string.Concat(new string[]
            {
                "New-NetFirewallRule -DisplayName \"",
                description,
                "\" -Direction Inbound -LocalPort ",
                port,
                " -Protocol TCP -Action Allow"
            });
            powerShell.Commands.AddScript(script);
            Collection<PSObject> collection = powerShell.Invoke();
        }

        private static string GetInstrumentIpAddress()
        {
            Console.WriteLine("Enter instrument Ip address:\n");
            return Console.ReadLine();
        }

        private static Instrument GetInstrument(LaboContext db, InstrumentKind kind, InstrumentKindInfo info)
        {
            List<Instrument> list = (from i in db.Instrument
                                     where i.InstrumentCode == kind.InstrumentKindId
                                     select i).ToList<Instrument>();
            bool flag = list.Count == 0;
            Instrument result;
            if (flag)
            {
                result = InstrumentHelper.InsertInstrument(db, kind, info);
            }
            else
            {
                Console.WriteLine("Please select an instrument:\n  N\t\t 'new one'\n" + string.Join("\n", list.Select((Instrument i, int j) => string.Format("  {0}\t\t{1}\t{2}", j, i.InstrumentId, i.InstrumentName))) + "\n");
                string text = Console.ReadLine();
                bool flag2 = text.ToLower() == "n";
                if (flag2)
                {
                    result = InstrumentHelper.InsertInstrument(db, kind, info);
                }
                else
                {
                    int index = int.Parse(text);
                    result = list.ElementAt(index);
                }
            }
            return result;
        }

        public static void DeleteNotUsedInstruments()
        {
            bool thereIsAnError = false;
            SqlHelper.TryBackupBmlims();
            LaboContext db = new LaboContext();
            (from i in db.Instrument
             where !i.InstrumentUsed
             select i).ToList<Instrument>().ForEach(delegate (Instrument i)
             {
                 try
                 {
                     db.Instrument.Remove(i);
                 }
                 catch (Exception)
                 {
                     thereIsAnError = true;
                 }
             });
            bool thereIsAnError2 = thereIsAnError;
            if (thereIsAnError2)
            {
                Console.WriteLine("Some instrument not deleted !!");
            }
            Console.ReadLine();
        }

        public static void SetDefaultInstrument_ByAtMapping()
        {
            using (LaboContext laboContext = new LaboContext())
            {
                List<Instrument> instruments = (from i in laboContext.Instrument
                                                where i.InstrumentUsed
                                                select i).ToList<Instrument>();
                int maxNameLength = instruments.Max((Instrument i) => i.InstrumentName.Length);
                instruments.ForEach(delegate (Instrument i)
                {
                    Console.WriteLine(string.Format("{0}-\t{1}\t{2} {3}", new object[]
                    {
                        instruments.IndexOf(i),
                        i.InstrumentId,
                        MultiIpHelper.ToMaxLength(i.InstrumentName, maxNameLength),
                        i.MiddlewareServiceName
                    }));
                });
                Console.WriteLine("\nSelect instrument :");
                int index = int.Parse(Console.ReadLine());
                Instrument defaultInstrument_ByAtMapping = instruments[index];
                InstrumentHelper.SetDefaultInstrument_ByAtMapping(defaultInstrument_ByAtMapping);
                Console.WriteLine("\nWork done !!");
                Console.ReadKey();
            }
        }

        public static void SetDefaultInstrument_ByAtMapping(Instrument instrument)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ClinModel"].ConnectionString + "Application Name=MyApp";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                string cmdText = string.Format("update AnalysisType\r\nset InstrumentId = {0}\r\nwhere AnalysisTypeId in (select AnalysisTypeId from AnalysisTypeInstrumentMapping m where m.InstrumentId = {1})", instrument.InstrumentId, instrument.InstrumentCode);
                SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}