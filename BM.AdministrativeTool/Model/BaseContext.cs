using System;
using System.Configuration;
using System.Data.Entity;

namespace BM.AdministrativeTool.Model
{
	// Token: 0x02000004 RID: 4
	public class BaseContext<TContext> : DbContext where TContext : DbContext
	{
		// Token: 0x06000009 RID: 9 RVA: 0x0000206C File Offset: 0x0000026C
		static BaseContext()
		{
			Database.SetInitializer<TContext>(null);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002076 File Offset: 0x00000276
		protected BaseContext() : base(ConfigurationManager.ConnectionStrings["ClinModel"].ConnectionString + "Application Name=MyApp")
		{
			base.Database.CommandTimeout = new int?(600);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000020B4 File Offset: 0x000002B4
		public BaseContext(string nameOrConnectionString) : base(nameOrConnectionString)
		{
		}
	}
}
