using System;
using MediatR;
using PortalApi.Common;

namespace PortalApi.Service
{
    public class PingHandler : IRequestHandler<PingRequest, string>
    {
        public string Handle(PingRequest message)
        {
            return "PortalApi " + App.Configuration.Environment + " @" + Environment.MachineName;
        }
    }
}
