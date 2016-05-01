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
        protected override byte[] GetSmallImageData(string targetFile, int width, int height)
        {
            byte[] bmp32Data;
            int sourceWidth;
            int sourceHeight;
            using (var imgRef = NSImageRep.ImageRepFromFile(targetFile).CGImage)
            using (var dataProvider = imgRef.DataProvider)
            using (var data = dataProvider.CopyData())
            {
                var source = data.Bytes;
                bmp32Data = new byte[data.Length];
                sourceHeight = (int)imgRef.Height;
                sourceWidth = (int)imgRef.Width;
                Marshal.Copy(source, bmp32Data, 0, bmp32Data.Length);
            }

            var result = new byte[width * height * 4];
            int s = 12; // s*s = サンプリング数
            int pos = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int srcX0 = x * sourceWidth / width;
                    int srcY0 = y * sourceHeight / height;

                    int r = 0;
                    int g = 0;
                    int b = 0;
                    int a = 0;

                    // 縮小した画素に対して縮小元から s * s 画素を取得し
                    // 平均値を計算
                    for (int yy = 0; yy < s; yy++)
                    {
                        for (int xx = 0; xx < s; xx++)
                        {
                            int dx = xx * sourceWidth / width / s;
                            int dy = yy * sourceHeight / height / s;
                            int p = ((srcX0 + dx) + (srcY0 + dy) * sourceWidth) * 4;
                            r += bmp32Data[p];
                            g += bmp32Data[p + 1];
                            b += bmp32Data[p + 2];
                            a += bmp32Data[p + 3];
                        }
                    }

                    result[pos++] = (byte)(r / s / s);
                    result[pos++] = (byte)(g / s / s);
                    result[pos++] = (byte)(b / s / s);
                    result[pos++] = (byte)(a / s / s);
                }
            }
            return result;
        }
    }
}

