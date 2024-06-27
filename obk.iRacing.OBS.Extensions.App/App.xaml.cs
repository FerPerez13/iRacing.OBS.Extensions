using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Aydsko.iRacingData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using obk.iRacing.Service;
using obk.iRacing.Service.Interfaces;

namespace WpfApp3;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;
    
    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                ConfigureServices(services);
            })
            .Build();
        
        // Inicializa los servicios
        _host.Start();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Aquí puedes agregar tus servicios
        services.AddSingleton<MainWindow>();
        services.AddSingleton<Extensions>();
        services.AddIRacingDataApi();
        services.AddTransient<IIRacingService, IRacingService>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        // Obtén la ruta del directorio de documentos del usuario
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string targetPath = Path.Combine(documentsPath, "iracingextensions", "resources");

        // Comprueba si existe el directorio "iracingextensions/resources"
        if (!Directory.Exists(targetPath))
        {
            // Obtén la ruta del directorio del proyecto
            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            string sourcePath = Path.Combine(projectPath, "resources");

            // Comprueba si existe el directorio "resources" en el directorio del proyecto
            if (Directory.Exists(sourcePath))
            {
                // Copia el directorio "resources" al directorio "iracingextensions"
                Directory.CreateDirectory(targetPath);
                foreach (var file in Directory.GetFiles(sourcePath))
                {
                    File.Copy(file, Path.Combine(targetPath, Path.GetFileName(file)));
                }
            }
        }

        // Resuelve una instancia de MainWindow y la muestra
        _host.Services.GetRequiredService<MainWindow>().Show();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        // Cierra el host cuando la aplicación se cierra
        await _host.StopAsync(TimeSpan.FromSeconds(5));
        _host.Dispose();
        base.OnExit(e);
    }
}