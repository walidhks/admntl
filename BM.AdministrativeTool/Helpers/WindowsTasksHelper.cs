using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Win32.TaskScheduler;

namespace BM.AdministrativeTool.Helpers
{
	// Token: 0x02000024 RID: 36
	internal class WindowsTasksHelper
	{
		// Token: 0x060000DC RID: 220 RVA: 0x00005774 File Offset: 0x00003974
		public static void CreateCheckQuartzStateTask()
		{
			using (TaskService taskService = new TaskService())
			{
				Console.Clear();
				bool flag = !Directory.Exists("C:\\Scripts");
				if (flag)
				{
					Directory.CreateDirectory("C:\\Scripts");
				}
				File.Copy("Scripts\\BAT\\quartz_chek.bat", "C:\\Scripts\\quartz_chek.bat", true);
				TaskDefinition taskDefinition = taskService.NewTask();
				taskDefinition.Settings.MultipleInstances = TaskInstancesPolicy.IgnoreNew;
				taskDefinition.RegistrationInfo.Description = "Check Quartz State";
				TimeTrigger timeTrigger = new TimeTrigger();
				timeTrigger.Repetition.Interval = TimeSpan.FromMinutes(30.0);
				taskDefinition.Triggers.Add(timeTrigger);
				taskDefinition.Actions.Add(new ExecAction("C:\\Scripts\\quartz_chek.bat", null, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
				taskService.RootFolder.RegisterTaskDefinition("Check Quartz State", taskDefinition);
				bool flag2 = taskService.RootFolder.AllTasks.Any((Task t) => t.Name == "Check Quartz State");
				if (flag2)
				{
					Console.WriteLine("Task Created, press any key to continue ...");
					Console.ReadLine();
				}
			}
		}
	}
}
