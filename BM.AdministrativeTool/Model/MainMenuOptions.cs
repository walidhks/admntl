using System;
using System.ComponentModel.DataAnnotations;

namespace BM.AdministrativeTool.Model
{
	// Token: 0x02000003 RID: 3
	public enum MainMenuOptions
	{
		// Token: 0x04000003 RID: 3
		[Display(Name = "Add Mutli-IP Address.")]
		AddMultiIpAddress,
		// Token: 0x04000004 RID: 4
		[Display(Name = "Change Machine Name.")]
		ChangeMachineName,
		// Token: 0x04000005 RID: 5
		[Display(Name = "Change Power Plan (High performance).")]
		ChangePowerPlanHighPerformance,
		// Token: 0x04000006 RID: 6
		[Display(Name = "Schedule quartz check status.")]
		ScheduleQuartzCheckStatus,
		// Token: 0x04000007 RID: 7
		[Display(Name = "Install All Used Instruments Services.")]
		InstallAllUsedInstrumentsServices,
		// Token: 0x04000008 RID: 8
		[Display(Name = "Install Instrument Service.")]
		InstallInstrumentService,
		// Token: 0x04000009 RID: 9
		[Display(Name = "Install Service by Instrument Kind Id.")]
		InstallInstrumentKindService,
		// Token: 0x0400000A RID: 10
		[Display(Name = "Uninstall Instrument Service.")]
		UninstallInstrumentService,
		// Token: 0x0400000B RID: 11
		[Display(Name = "Replace Instrument Server Ip Address.")]
		ReplaceInstrumentServerIpAddress,
		// Token: 0x0400000C RID: 12
		[Display(Name = "Delete Not Used Instruments.")]
		DeleteNotUsedInstruments,
		// Token: 0x0400000D RID: 13
		[Display(Name = "Change Sample Code Trigger.")]
		ChangeSampleCodeTrigger,
		// Token: 0x0400000E RID: 14
		[Display(Name = "Restaurer la base de données 'BMlims'.")]
		RestoreBmLimsDb,
		// Token: 0x0400000F RID: 15
		[Display(Name = "Extraire une base de données vide en conservant le parametrage.")]
		GetEmptyActiveDataBase,
		// Token: 0x04000010 RID: 16
		[Display(Name = "Supprimer les données de testes pour Lancemant.")]
		RemoveTestDataForLancemant,
		// Token: 0x04000011 RID: 17
		[Display(Name = "spicifie l'automate par default par 'Mapping'.")]
		SetDefaultInstrument_ByAtMapping,
		// Token: 0x04000012 RID: 18
		[Display(Name = "Exit.")]
		Exit
	}
}
