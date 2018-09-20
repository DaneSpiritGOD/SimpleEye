using System;
using System.Collections.Generic;
using System.Text;

namespace DisplayCenter.Core.Options
{
    public class ServiceEndpoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Port { get; set; } = -1;
        public string Type { get; set; } = ServiceEndpointTypeDefinations.NanoPipeline;
    }

    public class ServiceEndpointTypeDefinations
    {
        public const string NanoPipeline = "nano:pipe";
        public const string Socket = "socket";
    }
}
