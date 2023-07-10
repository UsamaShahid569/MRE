using MRE.Contracts.Dtos;
using MediatR;
using FluentValidation;


namespace MRE.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : CqrsResponse, new()
    {
        private readonly IValidator<TRequest> _validator;


        public ValidationBehavior()
        {
        }

        public ValidationBehavior(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (_validator == null)
            {
                return await next();
            }
            var result = await _validator.ValidateAsync(request);
            if (!result.IsValid)
            {
                return new TResponse { 
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = result.Errors[0].ErrorMessage
                };
            }

            return await next();
        }
    }

}
