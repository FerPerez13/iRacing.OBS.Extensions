using Aydsko.iRacingData;
using Microsoft.Extensions.DependencyInjection;
using obk.iRacing.Service;
using obk.iRacing.Service.Interfaces;

namespace WpfApp3;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddIRacingDataApi();
        services.AddTransient<IIRacingService, IRacingService>();
    }
}