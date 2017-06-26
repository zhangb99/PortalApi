using System.Linq;
using MediatR;
using NPoco;
using Fasterflect;
using PortalApi.Common;

namespace PortalApi.Service
{
    public class TransactionRequestDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _innerHander;
        private readonly IDatabase _database;

        public TransactionRequestDecorator(IRequestHandler<TRequest, TResponse> innerHandler, IDatabase database)
        {
            _innerHander = innerHandler;
            _database = database;
        }

        public TResponse Handle(TRequest message)
        {
            var attributes = message.GetType().Attributes();

            if (attributes.Any(x => x is TransactionAttribute))
            {
                _database.BeginTransaction();
            }

            var result = _innerHander.Handle(message);

            if (_database.Transaction != null)
            {
                _database.CompleteTransaction();
            }

            return result;
        }
    }
}