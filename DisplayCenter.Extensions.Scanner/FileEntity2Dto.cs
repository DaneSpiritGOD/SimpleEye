using Archiving.Entity;
using DisplayCenter.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DisplayCenter.Extensions.Scanner
{
    internal static class FileEntity2Dto
    {
        public static ImageDto Transform(this FileEntity entity)
        {
            return new FileImageDto()
            {
                Time = entity.NewTime.ToString(),
                Key = entity.FileName,
                Data = File.ReadAllBytes(entity.FullPath),
            };
        }
    }
}
