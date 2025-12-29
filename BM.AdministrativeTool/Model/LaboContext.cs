using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BM.AdministrativeTool.Model
{
	// Token: 0x02000005 RID: 5
	public class LaboContext : BaseContext<LaboContext>
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000020BF File Offset: 0x000002BF
		public LaboContext()
		{
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000020C9 File Offset: 0x000002C9
		public LaboContext(string nameOrConnectionString) : base(nameOrConnectionString)
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000020D4 File Offset: 0x000002D4
		// (set) Token: 0x0600000F RID: 15 RVA: 0x000020DC File Offset: 0x000002DC
		public virtual DbSet<Instrument> Instrument { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000020E5 File Offset: 0x000002E5
		// (set) Token: 0x06000011 RID: 17 RVA: 0x000020ED File Offset: 0x000002ED
		public virtual DbSet<InstrumentKind> InstrumentKind { get; set; }

		// Token: 0x06000012 RID: 18 RVA: 0x000020F6 File Offset: 0x000002F6
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}
	}
}
