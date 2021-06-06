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
    }
}