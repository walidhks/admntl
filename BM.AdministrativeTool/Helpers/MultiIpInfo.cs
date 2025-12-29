using System;

namespace BM.AdministrativeTool.Helpers
{
	// Token: 0x02000020 RID: 32
	internal class MultiIpInfo
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000BC RID: 188 RVA: 0x0000266D File Offset: 0x0000086D
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00002675 File Offset: 0x00000875
		public uint InterfaceIndex { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000BE RID: 190 RVA: 0x0000267E File Offset: 0x0000087E
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00002686 File Offset: 0x00000886
		public string IPv4Address { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x0000268F File Offset: 0x0000088F
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00002697 File Offset: 0x00000897
		public string InterfaceAlias { get; set; }
	}
}
