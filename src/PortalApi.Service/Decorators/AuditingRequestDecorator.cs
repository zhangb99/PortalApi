using MediatR;
using NPoco;

namespace PortalApi.Service
{
    public class AuditingRequestDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _innerHander;
        private readonly IDatabase _database;

        public AuditingRequestDecorator(IRequestHandler<TRequest, TResponse> innerHandler, IDatabase database)
        {
            _innerHander = innerHandler;
            _database = database;
        }

        public TResponse Handle(TRequest request)
        {
            var audit = request.CreateAudit();

            var result = _innerHander.Handle(request);

            if (audit != null)
            {
                audit.AppendPropertyFrom(request, result);
                _database.Insert(audit);
            }

            return result;
        }
    }
}