using System;
using System.Diagnostics.CodeAnalysis;
using PRUNner.Backend.Data;

namespace PRUNner.Backend.BasePlanner
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class WorkforceConsumptionFactors
    {
        public static readonly WorkforceConsumptionFactors Pioneers = new(4, rat: 4, cof: 0.5, ove: 0.5 , pwo: 0.2 );
        public static readonly WorkforceConsumptionFactors Settlers = new(5, rat: 6, kom: 1, exo: 0.5, rep: 0.2, pt: 0.5);
        public static readonly WorkforceConsumptionFactors Technicians = new(7.5, rat: 7, ale: 1, med: 0.5, sc: 0.1, hms: 0.5, scn: 0.1);
        public static readonly WorkforceConsumptionFactors Engineers = new(10, med: 0.5, gin: 1, fim: 7, vg: 0.2, hss: 0.2, pda: 0.1);
        public static readonly WorkforceConsumptionFactors Scientists = new(10, med: 0.5, win: 1, mea: 7, nst: 0.1, lc: 0.2, ws: 0.1);
        
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
        
        private WorkforceConsumptionFactors(double dw, double rat = 0, double ove = 0, double exo = 0, double pt = 0, double med = 0, double hms = 0, double scn = 0, double fim = 0, double hss = 0, double pda = 0, double mea = 0, double lc = 0, double ws = 0, double cof = 0, double pwo = 0, double kom = 0, double rep = 0, double ale = 0, double sc = 0, double gin = 0, double vg = 0, double win = 0, double nst = 0)
        {
            DW = dw / 100;
            RAT = rat / 100;
            OVE = ove / 100;
            EXO = exo / 100;
            PT = pt / 100;
            MED = med / 100;
            HMS = hms / 100;
            SCN = scn / 100;
            FIM = fim / 100;
            HSS = hss / 100;
            PDA = pda / 100;
            MEA = mea / 100;
            LC = lc / 100;
            WS = ws / 100;
            COF = cof / 100;
            PWO = pwo / 100;
            KOM = kom / 100;
            REP = rep / 100;
            ALE = ale / 100;
            SC = sc / 100;
            GIN = gin / 100;
            VG = vg / 100;
            WIN = win / 100;
            NST = nst / 100;
        }

        public void AddUsedConsumables(ProvidedConsumables consumables, int workforceRequired, int workforceCapacity, Action<MaterialData, double> addConsumableCallback)
        {
            var workforce = Math.Min(workforceRequired, workforceCapacity);

            if (workforce == 0)
            {
                return;
            }
            
            if (DW > 0 && consumables.DW) addConsumableCallback.Invoke(MaterialData.GetOrThrow("DW"), DW * workforce);
            if (RAT > 0 && consumables.RAT) addConsumableCallback.Invoke(MaterialData.GetOrThrow("RAT"), RAT * workforce);
            if (OVE > 0 && consumables.OVE) addConsumableCallback.Invoke(MaterialData.GetOrThrow("OVE"), OVE * workforce);
            if (EXO > 0 && consumables.EXO) addConsumableCallback.Invoke(MaterialData.GetOrThrow("EXO"), EXO * workforce);
            if (PT > 0 && consumables.PT) addConsumableCallback.Invoke(MaterialData.GetOrThrow("PT"), PT * workforce);
            if (MED > 0 && consumables.MED) addConsumableCallback.Invoke(MaterialData.GetOrThrow("MED"), MED * workforce);
            if (HMS > 0 && consumables.HMS) addConsumableCallback.Invoke(MaterialData.GetOrThrow("HMS"), HMS * workforce);
            if (SCN > 0 && consumables.SCN) addConsumableCallback.Invoke(MaterialData.GetOrThrow("SCN"), SCN * workforce);
            if (FIM > 0 && consumables.FIM) addConsumableCallback.Invoke(MaterialData.GetOrThrow("FIM"), FIM * workforce);
            if (HSS > 0 && consumables.HSS) addConsumableCallback.Invoke(MaterialData.GetOrThrow("HSS"), HSS * workforce);
            if (PDA > 0 && consumables.PDA) addConsumableCallback.Invoke(MaterialData.GetOrThrow("PDA"), PDA * workforce);
            if (MEA > 0 && consumables.MEA) addConsumableCallback.Invoke(MaterialData.GetOrThrow("MEA"), MEA * workforce);
            if (LC > 0 && consumables.LC) addConsumableCallback.Invoke(MaterialData.GetOrThrow("LC"), LC * workforce);
            if (WS > 0 && consumables.WS) addConsumableCallback.Invoke(MaterialData.GetOrThrow("WS"), WS * workforce);
            if (COF > 0 && consumables.COF) addConsumableCallback.Invoke(MaterialData.GetOrThrow("COF"), COF * workforce);
            if (PWO > 0 && consumables.PWO) addConsumableCallback.Invoke(MaterialData.GetOrThrow("PWO"), PWO * workforce);
            if (KOM > 0 && consumables.KOM) addConsumableCallback.Invoke(MaterialData.GetOrThrow("KOM"), KOM * workforce);
            if (REP > 0 && consumables.REP) addConsumableCallback.Invoke(MaterialData.GetOrThrow("REP"), REP * workforce);
            if (ALE > 0 && consumables.ALE) addConsumableCallback.Invoke(MaterialData.GetOrThrow("ALE"), ALE * workforce);
            if (SC > 0 && consumables.SC) addConsumableCallback.Invoke(MaterialData.GetOrThrow("SC"), SC * workforce);
            if (GIN > 0 && consumables.GIN) addConsumableCallback.Invoke(MaterialData.GetOrThrow("GIN"), GIN * workforce);
            if (VG > 0 && consumables.VG) addConsumableCallback.Invoke(MaterialData.GetOrThrow("VG"), VG * workforce);
            if (WIN > 0 && consumables.WIN) addConsumableCallback.Invoke(MaterialData.GetOrThrow("WIN"), WIN * workforce);
            if (NST > 0 && consumables.NST) addConsumableCallback.Invoke(MaterialData.GetOrThrow("NST"), NST * workforce);
        }

        public double CalculateCosts(PlanetaryBase planetaryBase)
        {
            var consumables = planetaryBase.ProvidedConsumables;

            var result = 0d;
            
            if (DW > 0 && consumables.DW) result += DW * MaterialData.GetOrThrow("DW").PriceData.GetPrice(planetaryBase);
            if (RAT > 0 && consumables.RAT) result += RAT * MaterialData.GetOrThrow("RAT").PriceData.GetPrice(planetaryBase);
            if (OVE > 0 && consumables.OVE) result += OVE * MaterialData.GetOrThrow("OVE").PriceData.GetPrice(planetaryBase);
            if (EXO > 0 && consumables.EXO) result += EXO * MaterialData.GetOrThrow("EXO").PriceData.GetPrice(planetaryBase);
            if (PT > 0 && consumables.PT) result += PT * MaterialData.GetOrThrow("PT").PriceData.GetPrice(planetaryBase);
            if (MED > 0 && consumables.MED) result += MED * MaterialData.GetOrThrow("MED").PriceData.GetPrice(planetaryBase);
            if (HMS > 0 && consumables.HMS) result += HMS * MaterialData.GetOrThrow("HMS").PriceData.GetPrice(planetaryBase);
            if (SCN > 0 && consumables.SCN) result += SCN * MaterialData.GetOrThrow("SCN").PriceData.GetPrice(planetaryBase);
            if (FIM > 0 && consumables.FIM) result += FIM * MaterialData.GetOrThrow("FIM").PriceData.GetPrice(planetaryBase);
            if (HSS > 0 && consumables.HSS) result += HSS * MaterialData.GetOrThrow("HSS").PriceData.GetPrice(planetaryBase);
            if (PDA > 0 && consumables.PDA) result += PDA * MaterialData.GetOrThrow("PDA").PriceData.GetPrice(planetaryBase);
            if (MEA > 0 && consumables.MEA) result += MEA * MaterialData.GetOrThrow("MEA").PriceData.GetPrice(planetaryBase);
            if (LC > 0 && consumables.LC) result += LC * MaterialData.GetOrThrow("LC").PriceData.GetPrice(planetaryBase);
            if (WS > 0 && consumables.WS) result += WS * MaterialData.GetOrThrow("WS").PriceData.GetPrice(planetaryBase);
            if (COF > 0 && consumables.COF) result += COF * MaterialData.GetOrThrow("COF").PriceData.GetPrice(planetaryBase);
            if (PWO > 0 && consumables.PWO) result += PWO * MaterialData.GetOrThrow("PWO").PriceData.GetPrice(planetaryBase);
            if (KOM > 0 && consumables.KOM) result += KOM * MaterialData.GetOrThrow("KOM").PriceData.GetPrice(planetaryBase);
            if (REP > 0 && consumables.REP) result += REP * MaterialData.GetOrThrow("REP").PriceData.GetPrice(planetaryBase);
            if (ALE > 0 && consumables.ALE) result += ALE * MaterialData.GetOrThrow("ALE").PriceData.GetPrice(planetaryBase);
            if (SC > 0 && consumables.SC) result += SC * MaterialData.GetOrThrow("SC").PriceData.GetPrice(planetaryBase);
            if (GIN > 0 && consumables.GIN) result += GIN * MaterialData.GetOrThrow("GIN").PriceData.GetPrice(planetaryBase);
            if (VG > 0 && consumables.VG) result += VG * MaterialData.GetOrThrow("VG").PriceData.GetPrice(planetaryBase);
            if (WIN > 0 && consumables.WIN) result += WIN * MaterialData.GetOrThrow("WIN").PriceData.GetPrice(planetaryBase);
            if (NST > 0 && consumables.NST) result += NST * MaterialData.GetOrThrow("NST").PriceData.GetPrice(planetaryBase);

            return result;
        }
    }
}