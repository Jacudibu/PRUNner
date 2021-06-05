using System.Collections.Generic;
using System.Linq;
using FIOImport.Data;

namespace PRUNner.Backend
{
    public static class SystemPathFinder
    {
        public static List<SystemData> FindShortestPath(PlanetData from, PlanetData to)
        {
            return FindShortestPath(from.System, to.System);
        }
        
        public static List<SystemData> FindShortestPath(SystemData from, SystemData to)
        {
            if (from == to)
            {
                return new List<SystemData>();
            }
            
            List<SystemData> visited = new() {from};
            Queue<Path> paths = new();
            foreach (var connection in from.Connections)
            {
                paths.Enqueue(new Path(connection, new List<SystemData>()));
            }
            
            while (paths.All(x => x.Position != to) && paths.Count > 0)
            {
                var path = paths.Dequeue();
                var unvisitedNeighbors = path.Position.Connections
                    .Where(x => !visited.Contains(x))
                    .ToList();

                if (unvisitedNeighbors.Count == 0)
                {
                    continue;
                }
                
                foreach (var nextSystem in unvisitedNeighbors)
                {
                    paths.Enqueue(new Path(nextSystem, path.UsedPath));
                    visited.Add(nextSystem);
                }
            }

            return paths.First(x => x.Position == to).UsedPath;
        }

        private class Path
        {
            public readonly SystemData Position;
            public readonly List<SystemData> UsedPath;

            public Path(SystemData position, IEnumerable<SystemData> path)
            {
                UsedPath = path.ToList();
                UsedPath.Add(position);

                Position = position;
            }

            public override string ToString()
            {
                return Position.Name;
            }
        }
    }
}