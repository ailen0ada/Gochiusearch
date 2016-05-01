using System;
using Mpga.ImageSearchEngine;
using AppKit;
using System.IO;
using CoreGraphics;
using System.Runtime.InteropServices;
using ModelIO;

namespace Gochiusearch.Mac
{
    internal class XMImageSearch : ImageSearch
    {
        public XMImageSearch()
        {
        }

        protected override byte[] GetSmallImageData(string targetFile, int width, int height)
        {
            using (var imgRep = NSImageRep.ImageRepFromFile(targetFile))
            using (var cgImage = imgRep.CGImage)
            using (var cs = CGColorSpace.CreateDeviceRGB())
            using (var context = new CGBitmapContext(null, width, height, cgImage.BitsPerComponent, cgImage.BytesPerRow, cs, CGImageAlphaInfo.PremultipliedLast))
            {
                var rect = new CGRect(0, 0, width, height);
                context.DrawImage(rect, cgImage);

                using (var newImageRef = context.ToImage())
                using (var dataProvider = newImageRef.DataProvider)
                using (var data = dataProvider.CopyData())
                {
                    var source = data.Bytes;
                    var bytes = new byte[data.Length];
                    Marshal.Copy(source, bytes, 0, bytes.Length);
                    return bytes;
                }
            }
        }
    }
}

