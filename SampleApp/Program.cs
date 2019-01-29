﻿using DEM.Net.Lib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Net;
using System.IO.Compression;
using System.Xml;
using System.Diagnostics;
using System.Windows.Media;
using System.Configuration;
using DEM.Net.glTF;
using AssetGenerator.Runtime;
using AssetGenerator;
using System.Numerics;
using Newtonsoft.Json;
using GeoAPI.Geometries;

namespace SampleApp
{
    class Program
    {
        static string _RasterDataDirectory;
        static string _OutputDataDirectory;

        [STAThread]
        static void Main(string[] args)
        {

            Logger.StartPerf("Main cold start");


            _RasterDataDirectory = GetDataDirectory();
            _OutputDataDirectory = @"..\..\..\Data";


            // Initialize raster service and Elevation service
            IRasterService rasterService = new RasterService(_RasterDataDirectory);
            IElevationService elevationService = new ElevationService(rasterService);

            //rasterService.GenerateDirectoryMetadata(DEMDataSet.AW3D30, false, true, true);
            //rasterService.GenerateDirectoryMetadata(DEMDataSet.SRTM_GL3, false, true, true);
            
           
            TextureSamples textureSamples = new TextureSamples(elevationService, _RasterDataDirectory, _OutputDataDirectory);
            textureSamples.Run();
            textureSamples.RunImagery(true);

            ReprojectionSamples reprojSamples = new ReprojectionSamples(elevationService, _OutputDataDirectory, @"..\..\..\Data\GPX\Vernet-les-bains-Canigou-34km.gpx");
            reprojSamples.Run();


            GpxSamples gpxSamples = new GpxSamples(elevationService, _OutputDataDirectory, @"..\..\..\Data\GPX\Bouleternere-Denivele_de_Noel_2017.gpx");
            gpxSamples.Run();

            OldSamples oldSamples = new OldSamples(rasterService, elevationService, _OutputDataDirectory);
            oldSamples.Run();


            Logger.StopPerf("Main cold start", true);
            Console.Write("Press any key to exit...");
            Console.ReadLine();

        }

        private static string GetDataDirectory()
        {
            string dataDir = ConfigurationManager.AppSettings["DataDir"];
            if (dataDir == null)
            {
                dataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DEM.Net");
            }
            Logger.Info($"Data directory : {dataDir}.");
            return dataDir;
        }

   
    }
}