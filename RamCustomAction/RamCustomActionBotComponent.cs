using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs.Declarative;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RamCustomAction.Clients;

namespace RamCustomAction
{
    public class RamCustomActionBotComponent : BotComponent
    {
        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<DeclarativeType>(sp =>
                new DeclarativeType<RamCustomAction>(RamCustomAction.Kind)); 
        }
    }
}
