using System.Diagnostics.CodeAnalysis;
using PRUNner.Backend.Data;
using PRUNner.Backend.Data.Enums;

namespace PRUNner.Backend.PlanetFinder
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class FilterCriteria
    {
        public string? Name;
        public bool ExcludeInfertile;
        public bool ExcludeRocky;
        public bool ExcludeGaseous;
        public bool ExcludeLowGravity;
        public bool ExcludeLowPressure;
        public bool ExcludeLowTemperature;
        public bool ExcludeHighGravity;
        public bool ExcludeHighPressure;
        public bool ExcludeHighTemperature;

        public bool DoesPlanetFitCriteria(PlanetData planet)
        {
            if (!string.IsNullOrEmpty(Name) && !planet.Name.Contains(Name, System.StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            if (ExcludeInfertile && !planet.IsFertile())
            {
                return false;
            }
                
            if (ExcludeRocky && planet.Type == PlanetType.Rocky)
            {
                return false;
            }

            if (ExcludeGaseous && planet.Type == PlanetType.Gaseous)
            {
                return false;
            }

            if (ExcludeLowGravity && planet.IsLowGravity())
            {
                return false;
            }
            
            if (ExcludeHighGravity && planet.IsHighGravity())
            {
                return false;
            }
            
            if (ExcludeLowPressure && planet.IsLowPressure())
            {
                return false;
            }
            
            if (ExcludeHighPressure && planet.IsHighPressure())
            {
                return false;
            }
            
            if (ExcludeLowTemperature && planet.IsLowTemperature())
            {
                return false;
            }
            
            if (ExcludeHighTemperature && planet.IsHighTemperature())
            {
                return false;
            }
                
            return true;
        }
            
        public FilterCriteria Combine(FilterCriteria other)
        {
            return new()
            {
                Name = Name ?? other.Name,
                ExcludeInfertile = ExcludeInfertile || other.ExcludeInfertile,
                ExcludeRocky = ExcludeRocky || other.ExcludeRocky,
                ExcludeGaseous = ExcludeGaseous || other.ExcludeGaseous,
                ExcludeLowGravity = ExcludeLowGravity || other.ExcludeLowGravity,
                ExcludeLowPressure = ExcludeLowPressure || other.ExcludeLowPressure,
                ExcludeLowTemperature = ExcludeLowTemperature || other.ExcludeLowTemperature,
                ExcludeHighGravity = ExcludeHighGravity || other.ExcludeHighGravity,
                ExcludeHighPressure = ExcludeHighPressure || other.ExcludeHighPressure,
                ExcludeHighTemperature = ExcludeHighTemperature || other.ExcludeHighTemperature,
            };
        }
            
        public static readonly FilterCriteria T1Criteria = new() 
        {
            ExcludeGaseous = true, 
            ExcludeLowGravity = true, ExcludeHighGravity = true,
            ExcludeLowPressure = true, ExcludeHighPressure = true,
            ExcludeLowTemperature = true, ExcludeHighTemperature = true
        };

        public static readonly FilterCriteria None = new();
    }
}