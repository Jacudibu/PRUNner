namespace PRUNner.Backend.Data.Components
{
    public class BuildingWorkforceRatio
    {
        public readonly double Pioneers;
        public readonly double Settlers;
        public readonly double Technicians;
        public readonly double Engineers;
        public readonly double Scientists;

        public BuildingWorkforceRatio(BuildingWorkforce workforce)
        {
            double totalWorkforce = workforce.Pioneers + workforce.Settlers + workforce.Technicians + workforce.Engineers + workforce.Scientists;
            if (totalWorkforce == 0)
            {
                return;
            }
            
            Pioneers = workforce.Pioneers / totalWorkforce;
            Settlers = workforce.Settlers / totalWorkforce;
            Technicians = workforce.Technicians / totalWorkforce;
            Engineers = workforce.Engineers / totalWorkforce;
            Scientists = workforce.Scientists / totalWorkforce;
        }
    }
}