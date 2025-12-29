using System;
using System.IO.Ports;

namespace BM.AdministrativeTool.Model
{
	// Token: 0x02000007 RID: 7
	public class Instrument
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002131 File Offset: 0x00000331
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002139 File Offset: 0x00000339
		public int InstrumentId { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002142 File Offset: 0x00000342
		// (set) Token: 0x0600001B RID: 27 RVA: 0x0000214A File Offset: 0x0000034A
		public bool InstrumentUsed { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002153 File Offset: 0x00000353
		// (set) Token: 0x0600001D RID: 29 RVA: 0x0000215B File Offset: 0x0000035B
		public string InstrumentName { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002164 File Offset: 0x00000364
		// (set) Token: 0x0600001F RID: 31 RVA: 0x0000216C File Offset: 0x0000036C
		public string InstrumentDescription { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002175 File Offset: 0x00000375
		// (set) Token: 0x06000021 RID: 33 RVA: 0x0000217D File Offset: 0x0000037D
		public string InstrumentPortName { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002186 File Offset: 0x00000386
		// (set) Token: 0x06000023 RID: 35 RVA: 0x0000218E File Offset: 0x0000038E
		public string InstrumentBaudRate { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002197 File Offset: 0x00000397
		// (set) Token: 0x06000025 RID: 37 RVA: 0x0000219F File Offset: 0x0000039F
		public string InstrumentDataBits { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000021A8 File Offset: 0x000003A8
		// (set) Token: 0x06000027 RID: 39 RVA: 0x000021B0 File Offset: 0x000003B0
		public long LastAnalysisId { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000021B9 File Offset: 0x000003B9
		// (set) Token: 0x06000029 RID: 41 RVA: 0x000021C1 File Offset: 0x000003C1
		public StopBits? InstrumentStopBits { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000021CA File Offset: 0x000003CA
		// (set) Token: 0x0600002B RID: 43 RVA: 0x000021D2 File Offset: 0x000003D2
		public Parity? InstrumentParity { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000021DB File Offset: 0x000003DB
		// (set) Token: 0x0600002D RID: 45 RVA: 0x000021E3 File Offset: 0x000003E3
		public string InstrumentStd { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600002E RID: 46 RVA: 0x000021EC File Offset: 0x000003EC
		// (set) Token: 0x0600002F RID: 47 RVA: 0x000021F4 File Offset: 0x000003F4
		public int? ServiceId { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000021FD File Offset: 0x000003FD
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00002205 File Offset: 0x00000405
		public int Mode { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000032 RID: 50 RVA: 0x0000220E File Offset: 0x0000040E
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002216 File Offset: 0x00000416
		public long? LaboratoryUnitId { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000034 RID: 52 RVA: 0x0000221F File Offset: 0x0000041F
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002227 File Offset: 0x00000427
		public int InstrumentCode { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002230 File Offset: 0x00000430
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00002238 File Offset: 0x00000438
		public string MiddlewareServiceName { get; set; }
	}
}
