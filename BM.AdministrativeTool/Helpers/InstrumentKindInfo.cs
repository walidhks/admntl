using System;

namespace BM.AdministrativeTool.Helpers
{
	// Token: 0x0200001C RID: 28
	public class InstrumentKindInfo
	{
		// Token: 0x06000093 RID: 147 RVA: 0x000041D0 File Offset: 0x000023D0
		public InstrumentKindInfo(string kindName, InstrumentKindInfo info)
		{
			this.AutomateKind = info.AutomateKind;
			this.Automate = kindName;
			this.Priority = info.Priority;
			this.Code = info.Code;
			this.InstrumentBaudRate = info.InstrumentBaudRate;
			this.InstrumentDataBits = info.InstrumentDataBits;
			this.InstrumentPortName = info.InstrumentPortName;
			this.LastAnalysisId = info.LastAnalysisId;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004248 File Offset: 0x00002448
		public InstrumentKindInfo(string s)
		{
			string[] array = s.Split(new char[]
			{
				','
			});
			this.AutomateKind = (this.Automate = array[0]);
			this.Priority = array[1];
			this.Code = array[2];
			this.InstrumentBaudRate = array[3];
			this.InstrumentDataBits = array[4];
			this.InstrumentPortName = array[5];
			this.LastAnalysisId = array[6];
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00002558 File Offset: 0x00000758
		public InstrumentKindInfo(string kindName, InstrumentKindInfo info, string c, string mode) : this(kindName, info)
		{
			this.InstrumentDataBits = c;
			this.Mode = mode;
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00002575 File Offset: 0x00000775
		// (set) Token: 0x06000097 RID: 151 RVA: 0x0000257D File Offset: 0x0000077D
		public string Mode { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00002586 File Offset: 0x00000786
		// (set) Token: 0x06000099 RID: 153 RVA: 0x0000258E File Offset: 0x0000078E
		public string AutomateKind { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00002597 File Offset: 0x00000797
		// (set) Token: 0x0600009B RID: 155 RVA: 0x0000259F File Offset: 0x0000079F
		public string Automate { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600009C RID: 156 RVA: 0x000025A8 File Offset: 0x000007A8
		// (set) Token: 0x0600009D RID: 157 RVA: 0x000025B0 File Offset: 0x000007B0
		public string Priority { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600009E RID: 158 RVA: 0x000025B9 File Offset: 0x000007B9
		// (set) Token: 0x0600009F RID: 159 RVA: 0x000025C1 File Offset: 0x000007C1
		public string Code { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x000025CA File Offset: 0x000007CA
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x000025D2 File Offset: 0x000007D2
		public string InstrumentBaudRate { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x000025DB File Offset: 0x000007DB
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x000025E3 File Offset: 0x000007E3
		public string InstrumentDataBits { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x000025EC File Offset: 0x000007EC
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x000025F4 File Offset: 0x000007F4
		public string InstrumentPortName { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x000025FD File Offset: 0x000007FD
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x00002605 File Offset: 0x00000805
		public string LastAnalysisId { get; set; }
	}
}
