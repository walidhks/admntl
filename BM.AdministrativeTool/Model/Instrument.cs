using System;
using System.IO.Ports;

namespace BM.AdministrativeTool.Model
{
    public class Instrument
    {
        public int InstrumentId { get; set; }
        public bool InstrumentUsed { get; set; }
        public string InstrumentName { get; set; }
        public string InstrumentDescription { get; set; }
        public string InstrumentPortName { get; set; }
        public string InstrumentBaudRate { get; set; }
        public string InstrumentDataBits { get; set; }
        public long LastAnalysisId { get; set; }
        public StopBits? InstrumentStopBits { get; set; }
        public Parity? InstrumentParity { get; set; }
        public string InstrumentStd { get; set; }
        public int? ServiceId { get; set; }
        public int Mode { get; set; }
        public long? LaboratoryUnitId { get; set; }
        public int InstrumentCode { get; set; }
        public string MiddlewareServiceName { get; set; }

        public long? L1 { get; set; }
        public long? L2 { get; set; }
        public long? L3 { get; set; }
        public bool? B1 { get; set; }
    }
}