using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.FCGames.Catalogo.Api.Controllers.Shared
{
    public abstract class ApiControllerBase<T> : ControllerBase where T : class
    {
        protected readonly ILogger<T> _logger;
        protected readonly ISender _sender;
        protected ApiControllerBase(ISender sender, ILogger<T> logger)
        {
            _sender = sender;
            _logger = logger;
        }
    }
}
