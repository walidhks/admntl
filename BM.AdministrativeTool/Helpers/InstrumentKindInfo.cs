using System;

namespace BM.AdministrativeTool.Helpers
{
    public class InstrumentKindInfo
    {
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

            this.Parity = info.Parity;
            this.L1 = info.L1;
            this.L2 = info.L2;
            this.L3 = info.L3;
            this.B1 = info.B1;
            this.Mode = info.Mode;
        }

        public InstrumentKindInfo(string s)
        {
            string[] array = s.Split(new char[]
            {
                ','
            });

            // Safety Check for standard fields
            if (array.Length < 7)
            {
                // Initialize empty to prevent null ref later, or handle error
                this.AutomateKind = this.Automate = "INVALID";
                this.Priority = this.Code = this.InstrumentBaudRate = "";
                return;
            }

            this.AutomateKind = (this.Automate = array[0]);
            this.Priority = array[1];
            this.Code = array[2];
            this.InstrumentBaudRate = array[3];
            this.InstrumentDataBits = array[4];
            this.InstrumentPortName = array[5];
            this.LastAnalysisId = array[6];

            // Extended Fields (Only read if they exist)
            if (array.Length > 7) this.Parity = array[7];
            if (array.Length > 8) this.L1 = array[8];
            if (array.Length > 9) this.L2 = array[9];
            if (array.Length > 10) this.L3 = array[10];
            if (array.Length > 11) this.B1 = array[11];
            if (array.Length > 12) this.Mode = array[12];
        }

        public InstrumentKindInfo(string kindName, InstrumentKindInfo info, string c, string mode) : this(kindName, info)
        {
            this.InstrumentDataBits = c;
            this.Mode = mode;
        }

        public string Mode { get; set; }
        public string AutomateKind { get; set; }
        public string Automate { get; set; }
        public string Priority { get; set; }
        public string Code { get; set; }
        public string InstrumentBaudRate { get; set; }
        public string InstrumentDataBits { get; set; }
        public string InstrumentPortName { get; set; }
        public string LastAnalysisId { get; set; }

        // New Properties
        public string Parity { get; set; }
        public string L1 { get; set; }
        public string L2 { get; set; }
        public string L3 { get; set; }
        public string B1 { get; set; }
    }
}