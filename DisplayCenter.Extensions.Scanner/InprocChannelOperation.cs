using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using Archiving.Core.Operation;
using Archiving.Entity;
using DisplayCenter.Core;

namespace DisplayCenter.Extensions.Scanner
{
    internal class InprocChannelOperation : IObserverOperation
    {
        private readonly ChannelWriter<ImageDto> _writer;

        public InprocChannelOperation(ChannelWriter<ImageDto> writer)
        {
            _writer = writer;
        }

        public void Dispose() { }

        public void Handle(FileEntity entity)
        {
            _writer.WriteAsync(entity.Transform()).GetAwaiter().GetResult();
        }
    }
}
