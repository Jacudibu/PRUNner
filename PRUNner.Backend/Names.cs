using System.Diagnostics.CodeAnalysis;

namespace PRUNner.Backend
{
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class Names
    {
        public static class Systems
        {
            public const string AntaresI = "Antares I";
            public const string Benten = nameof(Benten);
            public const string Hortus = nameof(Hortus);
            public const string Moria = nameof(Moria);
        }

        public static class Planets
        {
            public const string Katoa = nameof(Katoa);
            public const string Montem = nameof(Montem);
            public const string Pyrgos = nameof(Pyrgos);
            public const string Promitor = nameof(Promitor);
            public const string Umbra = nameof(Umbra);
            public const string Vallis = nameof(Vallis);
        }
        
        public static class Materials
        {
            public const string FEO = nameof(FEO);
            public const string LST = nameof(LST);
        }

        public static class Buildings
        {
            public const string CM = nameof(CM);
            public const string HB1 = nameof(HB1);
            public const string HB2 = nameof(HB2);
            public const string HB3 = nameof(HB3);
            public const string HB4 = nameof(HB4);
            public const string HB5 = nameof(HB5);
            public const string HBB = nameof(HBB);
            public const string HBC = nameof(HBC);
            public const string HBM = nameof(HBM);
            public const string HBL = nameof(HBL);
            public const string STO = nameof(STO);
        }
    }
}