using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace BM.AdministrativeTool.Helpers
{
	// Token: 0x02000009 RID: 9
	internal class SqlTriggersHelper
	{
		// Token: 0x0600003B RID: 59 RVA: 0x00002A34 File Offset: 0x00000C34
		public static void ChangeSampleCodeTrigger()
		{
			List<string> triggers = Directory.GetFiles("Scripts/SampleCodeTriggers/Triggers").ToList<string>().Select(new Func<string, string>(Path.GetFileName)).ToList<string>();
			Console.WriteLine("Select Trigger to apply:\n");
			Console.WriteLine(string.Join("\n", from s in triggers
			select string.Format("{0} _ \t{1}", triggers.IndexOf(s), s)));
			int index = int.Parse(Console.ReadLine());
			string connectionString = ConfigurationManager.ConnectionStrings["ClinModel"].ConnectionString + "Application Name=MyApp";
			using (SqlConnection sqlConnection = new SqlConnection(connectionString))
			{
				sqlConnection.Open();
				string cmdText = File.ReadAllText("Scripts/SampleCodeTriggers/Triggers/" + triggers[index]);
				SqlCommand sqlCommand = new SqlCommand(cmdText, sqlConnection);
				sqlCommand.ExecuteNonQuery();
				string cmdText2 = File.ReadAllText("Scripts/SampleCodeTriggers/ReginerateQueries/" + triggers[index]);
				SqlCommand sqlCommand2 = new SqlCommand(cmdText2, sqlConnection);
				sqlCommand2.ExecuteNonQuery();
				sqlConnection.Close();
			}
			Console.WriteLine("\n\nPress any key to continue\n");
			Console.ReadKey();
		}
	}
}
