using System.IO;
using NLog;

namespace FIOImport
{
    public static class FioImporter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public const string CacheFolder = "FIOCache/";

        public static RawData LoadAllFromCache()
        {
            if (Directory.Exists(CacheFolder))
            {
                return new RawData(FioBuildingDownloader.Instance.LoadFromCacheOrDownload(),
                    FioMaterialDownloader.Instance.LoadFromCacheOrDownload(), 
                    FioPlanetDownloader.Instance.LoadFromCacheOrDownload(), 
                    FioSystemDownloader.Instance.LoadFromCacheOrDownload(),
                    FioCommodityExchangeDownloader.Instance.LoadFromCacheOrDownload(),
                    FioExchangeDataDownloader.Instance.LoadFromCacheOrDownload());
            }

            Logger.Info("Unable to find Cache folder, downloading all data from fio instead. This might take a little while.");
            return ImportAll();
        }

        private static RawData ImportAll()
        {
            Directory.CreateDirectory(CacheFolder);

            return new RawData(FioBuildingDownloader.Instance.LoadFromCacheOrDownload(),
                FioMaterialDownloader.Instance.LoadFromCacheOrDownload(), 
                FioPlanetDownloader.Instance.LoadFromCacheOrDownload(),
                FioSystemDownloader.Instance.LoadFromCacheOrDownload(),
                FioCommodityExchangeDownloader.Instance.LoadFromCacheOrDownload(),
                FioExchangeDataDownloader.Instance.LoadFromCacheOrDownload());
        }
    }
}