using DisplayCenter.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DisplayCenter.Core
{
    public interface IImageDtoTransformer
    {
        ImageDto TransformFrom(Memory<byte> data);
    }
}
