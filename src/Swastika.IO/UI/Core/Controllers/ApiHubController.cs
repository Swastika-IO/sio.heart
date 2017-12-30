using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Swastika.Api.Controllers;
using Swastika.IO.UI.Core.SignalR;

namespace Swastika.IO.UI.Core.Controllers
{
    public abstract class ApiHubController<THub, TDbContext, TModel>
        : BaseApiController<TDbContext, TModel>
        where TDbContext : DbContext
        where TModel : class
        where THub : BaseSignalRHub
    {
        private readonly IHubContext<THub> _hub;
        public IHubClients Clients { get; private set; }
        public IGroupManager Groups { get; private set; }
        protected ApiHubController(IHubContext<THub> hub)
        {
            _hub = hub;
            Clients = _hub.Clients;
            Groups = _hub.Groups;
        }
    }

    public abstract class ApiHubController<THub, TDbContext, TModel, TView> 
        : BaseApiController<TDbContext, TModel, TView>
        where TDbContext : DbContext
        where TModel : class
        where TView : Swastika.Domain.Data.ViewModels.ViewModelBase<TDbContext, TModel, TView>
        where THub : BaseSignalRHub
    {
        private readonly IHubContext<THub> _hub;
        public IHubClients Clients { get; private set; }
        public IGroupManager Groups { get; private set; }
        protected ApiHubController(IHubContext<THub> hub)
        {
            _hub = hub;
            Clients = _hub.Clients;
            Groups = _hub.Groups;
        }
    }
}
