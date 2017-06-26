using MediatR;

namespace PortalApi.Common
{
    public class PingRequest : IRequest<string>
    {
        public string Message { get; set; }
    }
}
