using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DisplayCenter.Core
{
    public abstract class ImageDto
    {
        public virtual Memory<byte> Data { get; set; }
        public virtual string Time { get; set; }
        public virtual string Key { get; set; }
    }

    public sealed class FileImageDto : ImageDto { }

    public sealed class MemoryImageDto : ImageDto
    {
        public int SolutionId { get; set; }
        public int ErrorId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int ChannelCount { get; set; }
        public IEnumerable<Roi> Rois { get; set; }
    }

    public struct Roi
    {
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }
        public int ErrorId { get; }

        public Roi(int x, int y, int width, int height, int errId)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            ErrorId = errId;
        }
    }
}
