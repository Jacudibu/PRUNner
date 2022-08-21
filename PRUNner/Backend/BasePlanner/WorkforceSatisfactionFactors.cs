using System.Diagnostics.CodeAnalysis;

namespace PRUNner.Backend.BasePlanner
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class WorkforceSatisfactionFactors
    {
        public static readonly WorkforceSatisfactionFactors Pioneers = new(0.02,  10/3d, 4, ove: 5/6d, pwo: 1/11d, cof: 2/13d);
        public static readonly WorkforceSatisfactionFactors Settlers = new(0.01, 10/3d, 4, exo: 1, pt: 5/6d, rep: 1/11d, kom: 2/13d);
        public static readonly WorkforceSatisfactionFactors Technicians = new(0.005, 10/3d, 4, med: 5/6d, hms: 1, scn: 1, sc: 1/11d, ale: 2/13d);
        public static readonly WorkforceSatisfactionFactors Engineers = new(0.005, 10/3d, 0, fim: 4, med: 5/6d, hss: 1, pda: 1, vg: 1/11d, gin: 2/13d);
        public static readonly WorkforceSatisfactionFactors Scientists = new(0.005, 10/3d, 0, mea: 4, med: 5/6d, lc: 1, ws: 1, nst: 1/11d, win: 2/13d);
        
        private readonly double _baseFactor; 
        private readonly double DW; 
        private readonly double RAT; 
        private readonly double OVE; 
        private readonly double EXO; 
        private readonly double PT; 
        private readonly double MED; 
        private readonly double HMS; 
        private readonly double SCN; 
        private readonly double FIM; 
        private readonly double HSS;
        private readonly double PDA; 
        private readonly double MEA; 
        private readonly double LC; 
        private readonly double WS; 
        private readonly double COF; 
        private readonly double PWO; 
        private readonly double KOM; 
        private readonly double REP; 
        private readonly double ALE; 
        private readonly double SC; 
        private readonly double GIN; 
        private readonly double VG; 
        private readonly double WIN; 
        private readonly double NST;
        
        private WorkforceSatisfactionFactors(double baseFactor, double dw, double rat, double ove = 0, double exo = 0, double pt = 0, double med = 0, double hms = 0, double scn = 0, double fim = 0, double hss = 0, double pda = 0, double mea = 0, double lc = 0, double ws = 0, double cof = 0, double pwo = 0, double kom = 0, double rep = 0, double ale = 0, double sc = 0, double gin = 0, double vg = 0, double win = 0, double nst = 0)
        {
            _baseFactor = baseFactor;
            DW = 1 + dw;
            RAT = 1 + rat;
            OVE = 1 + ove;
            EXO = 1 + exo;
            PT = 1 + pt;
            MED = 1 + med;
            HMS = 1 + hms;
            SCN = 1 + scn;
            FIM = 1 + fim;
            HSS = 1 + hss;
            PDA = 1 + pda;
            MEA = 1 + mea;
            LC = 1 + lc;
            WS = 1 + ws;
            COF = 1 + cof;
            PWO = 1 + pwo;
            KOM = 1 + kom;
            REP = 1 + rep;
            ALE = 1 + ale;
            SC = 1 + sc;
            GIN = 1 + gin;
            VG = 1 + vg;
            WIN = 1 + win;
            NST = 1 + nst;
        }

        [SuppressMessage("ReSharper", "CyclomaticComplexity")]
        public double Calculate(ProvidedConsumables consumables)
        {
            var result = _baseFactor;
            if (consumables.DW) result *= DW;
            if (consumables.RAT) result *= RAT;
            if (consumables.OVE) result *= OVE;
            if (consumables.EXO) result *= EXO;
            if (consumables.PT) result *= PT;
            if (consumables.MED) result *= MED;
            if (consumables.HMS) result *= HMS;
            if (consumables.SCN) result *= SCN;
            if (consumables.FIM) result *= FIM;
            if (consumables.HSS) result *= HSS;
            if (consumables.PDA) result *= PDA;
            if (consumables.MEA) result *= MEA;
            if (consumables.LC) result *= LC;
            if (consumables.WS) result *= WS;
            if (consumables.COF) result *= COF;
            if (consumables.PWO) result *= PWO;
            if (consumables.KOM) result *= KOM;
            if (consumables.REP) result *= REP;
            if (consumables.ALE) result *= ALE;
            if (consumables.SC) result *= SC;
            if (consumables.GIN) result *= GIN;
            if (consumables.VG) result *= VG;
            if (consumables.WIN) result *= WIN;
            if (consumables.NST) result *= NST;
            return result;
        }
    }
}