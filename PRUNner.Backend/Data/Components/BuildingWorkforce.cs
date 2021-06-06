using FIOImport.POCOs.Buildings;

namespace PRUNner.Backend.Data.Components
{
    public class BuildingWorkforce
    {
        public static readonly BuildingWorkforce Zero = new(0, 0,0,0,0);
        
        public int Pioneers { get; }
        public int Settlers { get; }
        public int Technicians { get; }
        public int Engineers { get; }
        public int Scientists { get; }

        public BuildingWorkforce(FioBuilding poco)
        {
            Pioneers = poco.Pioneers;
            Settlers = poco.Settlers;
            Technicians = poco.Technicians;
            Engineers = poco.Engineers;
            Scientists = poco.Scientists;
        }        
        
        public BuildingWorkforce(int pioneers, int settlers, int technicians, int engineers, int scientists)
        {
            Pioneers = pioneers;
            Settlers = settlers;
            Technicians = technicians;
            Engineers = engineers;
            Scientists = scientists;
        }
    }
}