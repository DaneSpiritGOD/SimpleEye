using Archiving.Core.Operation;
using Archiving.Core.Options;
using DisplayCenter.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace DisplayCenter.Extensions.Scanner
{
    public class InprocOperationFactoryExtension : IObserverOperationFactoryExtension
    {
        private readonly ChannelWriter<ImageDto> _writer;

        private const string Key = nameof(ImageDto);
        public InprocOperationFactoryExtension(Channel<ImageDto> channel)
        {
            _writer = NamedNullException.Assert(channel, nameof(channel)).Writer;
        }

        public IObserverOperation Create(string groupName, OperationOptions oo)
        {
            if (oo.DoInProcessTransfer && StringComparer.OrdinalIgnoreCase.Equals(oo.InProcessTransferKey, Key))
                return new InprocChannelOperation(_writer);

            return default;
        }
    }
}