using System;
using Mpga.ImageSearchEngine;

namespace Gochiusearch.Mac
{
    internal class XMImageSearch : ImageSearch
    {
        public XMImageSearch()
        {
        }

        protected override byte[] GetSmallImageData(string targetFile, int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}

