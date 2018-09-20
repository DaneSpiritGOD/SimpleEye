using DisplayCenter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using sw = System.Windows;

namespace DisplayCenter.UI.Solution
{
    internal static class ImageDtoExtensions
    {
        public static BitmapSource GetImage(this ImageDto dto)
        {
            switch (dto)
            {
                case FileImageDto fdto:
                    return getImage(fdto);
                case MemoryImageDto mdto:
                    return drawRoi(getImage(mdto), mdto.Rois);
                default:
                    throw new NotSupportedException("未知数据类型!");
            }
        }

        private static BitmapSource getImage(ImageDto dto)
        {
            switch (dto)
            {
                case FileImageDto fdto:
                    return fdto.Data.CreateFromFileContent();
                case MemoryImageDto mdto:
                    return BitmapSource.Create(
                        mdto.Width,
                        mdto.Height,
                        96, 96,
                        mdto.ChannelCount.CalcPixelFormat(),
                        null,
                        mdto.Data.ToArray(),
                        mdto.Width * mdto.ChannelCount);
                default:
                    throw new NotSupportedException("未知数据类型!");
            }
        }

        private static BitmapSource drawRoi(BitmapSource bitmapSource, IEnumerable<Roi> rois)
        {
            if (rois == default || !rois.Any())
            {
                return bitmapSource;
            }

            return bitmapSource.ShowROI(rois.Select(x => new sw.Rect(x.X, x.Y, x.Width, x.Height)).ToArray());
        }
    }
}
