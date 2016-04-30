using System;
using Mpga.ImageSearchEngine;
using System.IO;
namespace ImageSearchEngine.Portable
{
    public class ImageSearch
    {
        private readonly string _basePath;

        private readonly ImageInfo[] _info;

        public ImageSearch()
        {
        }

        public ImageSearch(ImageInfo[] info)
        {
            _info = info;
        }

        public ImageSearch(string basePath)
        {
            _basePath = basePath;
        }

        public ImageInfo[] LoadFromDb(string dbFileName)
        {
        }

        public static string GetDirectory(string path)
        {

        }
    }
}

