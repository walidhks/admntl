using System;
using System.Collections.Generic;

namespace BM.AdministrativeTool.Helpers
{
	// Token: 0x0200000B RID: 11
	public class AuditEntry
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600003F RID: 63 RVA: 0x0000226F File Offset: 0x0000046F
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00002277 File Offset: 0x00000477
		public long AuditEntryId { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002280 File Offset: 0x00000480
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00002288 File Offset: 0x00000488
		public string Json { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002291 File Offset: 0x00000491
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002299 File Offset: 0x00000499
		public long UserId { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000045 RID: 69 RVA: 0x000022A2 File Offset: 0x000004A2
		// (set) Token: 0x06000046 RID: 70 RVA: 0x000022AA File Offset: 0x000004AA
		public string UserLib { get; set; }

		// Token: 0x0400002C RID: 44
		private readonly Dictionary<string, string> _actions = new Dictionary<string, string>
		{
			{
				"Insert",
				"Ajout"
			},
			{
				"Update",
				"Modification"
			},
			{
				"Delete",
				"Supprission"
			}
		};
	}
}
