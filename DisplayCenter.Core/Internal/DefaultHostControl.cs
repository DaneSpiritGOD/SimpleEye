using DisplayCenter.Core.Control;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DisplayCenter.Core.Internal
{
    public class DefaultHostControl : IHostControl
    {
        private readonly ILogger<DefaultHostControl> _logger;

        public DefaultHostControl(ILogger<DefaultHostControl> logger)
        {
            _logger = NamedNullException.Assert(logger, nameof(logger));
        }

        public Task RestartAllAsync()
        {
            _logger.LogInformation("你正在尝试重启，但是目前还没有具体实现！");
            return Task.CompletedTask;
        }

        public Task ShutdownAllAsync()
        {
            _logger.LogInformation("你正在尝试重启，但是目前还没有具体实现！");
            return Task.CompletedTask;
        }
    }
}
