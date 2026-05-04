using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Infrastructure.EventBus.Options
{
    public class RabbitMqOptions
    {
        public string HostName { get; init; }
        public string VirtualHost { get; init; }
        public string UserName { get; init; }
        public string Password { get; init; }
    }
}
