using DisplayCenter.Core;
using DisplayCenter.Core.Options;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NanomsgPlus;
using NanomsgPlus.Business;
using Suites.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DisplayCenter.ImageService
{
    public class HostedImageService : BackgroundService
    {
        private readonly IImageDtoTransformer _dataTransformer;
        private readonly ChannelReader<ImageDto> _channelReader;
        private readonly ILogger<HostedImageService> _logger;
        private readonly IMediator _mediator;
        private IEnumerable<NanomsgSocketBase> _sockets;
        private readonly IEnumerable<ServiceEndpoint> _endpoints;
        private readonly int _pollTimeout;

        public HostedImageService(
            IImageDtoTransformer dataTransformer,
            Channel<ImageDto> channel,
            IMediator mediator,
            IOptions<ImageServiceOptions> options,
            ILogger<HostedImageService> logger)
        {
            _dataTransformer = NamedNullException.Assert(dataTransformer, nameof(dataTransformer));
            _channelReader = NamedNullException.Assert(channel, nameof(channel)).Reader;
            _logger = NamedNullException.Assert(logger, nameof(logger));
            _mediator = NamedNullException.Assert(mediator, nameof(mediator));

            var opt = NamedNullException.Assert(options, nameof(options)).Value;
            NotTrueException.Assert(opt.Endpoints.Count() > 0, nameof(opt.Endpoints));
            _endpoints = opt.Endpoints;
            _pollTimeout = opt.PollTimeout;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _sockets = _endpoints.Select(x => x.Address.CreateHugeBufferPipelineSocket()).ToList();

            var fds = _sockets.Select(x => x.SocketID).ToArray();
            Task.Run(
                async () =>
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        try
                        {
                            var mems = fds.Poll(_pollTimeout);
                            foreach (var mem in mems)
                            {
                                if (mem.IsEmpty)
                                {
                                    break;
                                }

                                var dto = _dataTransformer.TransformFrom(mem);
                                await _mediator.Publish(new SimpleNotification<ImageDto>(dto));
                            }
                        }
                        catch (Exception) when (stoppingToken.IsCancellationRequested)
                        {
                            _logger.LogInformation("Stopping image service...");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogCritical(ex, "严重未知错误！");
                        }
                    }
                });

            Task.Run(
                async () =>
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var dto = await _channelReader.ReadAsync();
                        await _mediator.Publish(new SimpleNotification<ImageDto>(dto));
                    }
                });

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (var socket in _sockets)
            {
                socket.Dispose();
            }
        }
    }
}
