using System;
using System.Collections.Generic;
using System.Text;

namespace DisplayCenter.Core.Options
{
    public class ImageServiceOptions
    {
        public int PollTimeout { get; set; } = 20;
        public IEnumerable<ServiceEndpoint> Endpoints { get; set; }
    }
}
