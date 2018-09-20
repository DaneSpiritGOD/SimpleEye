using Basket;
using DisplayCenter.Core;
using Google.Protobuf;
using System;

namespace DisplayCenter.Core
{
    public class ImageDtoFactory : IImageDtoTransformer
    {
        public ImageDtoFactory() { }

        public ImageDto TransformFrom(Memory<byte> data)
        {
            var mix = new Mix();
            mix.MergeFrom(data.ToArray());
            switch (mix.FormatOneOfCase)
            {
                case Mix.FormatOneOfOneofCase.ImageFile:
                    return mix.ImageFile.Transform();
                case Mix.FormatOneOfOneofCase.Image:
                    return mix.Image.Transform();
                default:
                    throw new RuntimeException("无法找到配对的pb格式，请确认发送端和接收端pb的匹配！");
            }
        }
    }
}
