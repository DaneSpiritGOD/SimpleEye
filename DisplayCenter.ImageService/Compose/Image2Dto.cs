using Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisplayCenter.Core
{
    public static class Image2Dto
    {
        public static ImageDto Transform(this ImageFile item)
        {
            return new FileImageDto()
            {
                Time = item.FileTime,
                Key = item.FileName,
                Data = item.FileContent.ToByteArray(),
            };
        }        

        public static ImageDto Transform(this Image item)
        {
            return new MemoryImageDto
            {
                SolutionId = item.SolutionId,
                ErrorId = item.ErrorId,
                Time = DateTime.Now.ToString(),
                Width = item.Width,
                Height = item.Height,
                ChannelCount = item.ChannelCount,
                Data = item.Raw.ToByteArray(),
                Key = item.Key,
                Rois = item.Rois.Select(v => new Roi(v.X, v.Y, v.Width, v.Height, v.ErrorId)).ToList(),
            };
        }

        public static ImageDto Transform(this SimpleRawImage item)
        {
            throw new NotImplementedException("不支持这种类型的PB数据!");
        }
    }
}
