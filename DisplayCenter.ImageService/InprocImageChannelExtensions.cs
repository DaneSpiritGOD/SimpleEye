using DisplayCenter.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace DisplayCenter.ImageService
{
    public static class InprocImageChannelExtensions
    {
        private static Channel<T> createImageBounded<T>(int capacity) =>
            Channel.CreateBounded<T>(
                new BoundedChannelOptions(capacity)
                {
                    SingleReader = true,
                    SingleWriter = false,
                    AllowSynchronousContinuations = false,
                    FullMode = BoundedChannelFullMode.DropOldest
                });

        public static IServiceCollection AddImageChannel(this IServiceCollection services, int capacity)
        {
            return services.AddSingleton(createImageBounded<ImageDto>(capacity));
        }
    }
}
