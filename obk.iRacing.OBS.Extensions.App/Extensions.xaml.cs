using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using obk.iRacing.Service.Interfaces;
using WpfApp3.Helpers;

namespace WpfApp3;

public partial class Extensions : Window
{
    public int? Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    private Timer? _timer;

    private string FontColor { get; set; } = "#000000";

    private readonly IIRacingService _iRacingService;

    public Extensions(IIRacingService iRacingService, int id, string username, string password)
    {
        _iRacingService = iRacingService;

        if (!Directory.Exists(PathHelper.HtmlResults))
        {
            Directory.CreateDirectory(PathHelper.HtmlResults);
        }

        InitializeComponent();

        this.Icon = new BitmapImage(new Uri(PathHelper.IconPath, UriKind.Absolute));

        Id = id;
        Username = username;
        Password = password;

        if (File.Exists(PathHelper.ConfigurationsPath))
        {
            string[] lines = File.ReadAllLines(PathHelper.ConfigurationsPath);
            FontColor = lines[0];
        }

        hexColor.Text = FontColor;
        hexColorPreview.Background = FontColor.ToBrush();

        updateStatsToggleButton.Background = System.Windows.Media.Brushes.Red;
    }

    private void UpdateStatsToggleButton_OnChecked(object sender, RoutedEventArgs e)
    {
        loadingProgressBar.Visibility = Visibility.Visible;
        loadingProgressBar.Background = System.Windows.Media.Brushes.Yellow;

        updateStatsToggleButton.Background = System.Windows.Media.Brushes.Teal;

        if (_timer == null)
        {
            statsStatusText.Content = "⌛ Esperando...";
            statsStatusText.Background = System.Windows.Media.Brushes.LightYellow;
            _timer = new Timer(_ => GetInfoAndCreateOrUpdateHtml(), null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
        }
    }

    private async Task GetInfoAndCreateOrUpdateHtml()
    {
        string[] lines = { FontColor };
        File.WriteAllLines(PathHelper.ConfigurationsPath, lines);

        Dispatcher.Invoke(() =>
        {
            statsStatusText.Content = "☎️ Realizando llamada al servidor...";
            statsStatusText.Background = System.Windows.Media.Brushes.LightSalmon;
            loadingProgressBar.Visibility = Visibility.Visible;
        });

        var driverStats = await _iRacingService.GetDriverInfoAsync(Id.Value);
        var lastRaces = await _iRacingService.GetMemberRecentRacesAsync();

        var formulaLicense = driverStats.Data[0].Licenses.FirstOrDefault(x => x.Category == "formula_car");
        var formulaGroup = formulaLicense.GroupName.Contains("Class ")
            ? formulaLicense.GroupName.Substring("Class ".Length)
            : formulaLicense.GroupName;
        var formulaHtml = HtmlStrings.LicenseHtml("formula", formulaGroup, formulaLicense.IRating.ToString(),
            formulaLicense.SafetyRating.ToString("0.00"), FontColor);
        var formulaTask = CreateOrUpdateHtmlFile(PathHelper.FormulaHtmlPath, formulaHtml);

        var sportCarLicense = driverStats.Data[0].Licenses.FirstOrDefault(x => x.Category == "sports_car");
        var sportCarGroup = sportCarLicense.GroupName.Contains("Class ")
            ? sportCarLicense.GroupName.Substring("Class ".Length)
            : sportCarLicense.GroupName;
        var sportCarHtml = HtmlStrings.LicenseHtml("sportscar", sportCarGroup, sportCarLicense.IRating.ToString(),
            sportCarLicense.SafetyRating.ToString("0.00"), FontColor);
        var sportCarTask = CreateOrUpdateHtmlFile(PathHelper.SportCarHtmlPath, sportCarHtml);

        var ovalLicense = driverStats.Data[0].Licenses.FirstOrDefault(x => x.Category == "oval");
        var ovalGroup = ovalLicense.GroupName.Contains("Class ")
            ? ovalLicense.GroupName.Substring("Class ".Length)
            : ovalLicense.GroupName;
        var ovalHtml = HtmlStrings.LicenseHtml("oval", ovalGroup, ovalLicense.IRating.ToString(),
            ovalLicense.SafetyRating.ToString("0.00"), FontColor);
        var ovalTask = CreateOrUpdateHtmlFile(PathHelper.OvalHtmlPath, ovalHtml);

        var dirtRoadLicense = driverStats.Data[0].Licenses.FirstOrDefault(x => x.Category == "dirt_road");
        var dirtRoadGroup = dirtRoadLicense.GroupName.Contains("Class ")
            ? dirtRoadLicense.GroupName.Substring("Class ".Length)
            : dirtRoadLicense.GroupName;
        var dirtRoadHtml = HtmlStrings.LicenseHtml("dirtroad", dirtRoadGroup, dirtRoadLicense.IRating.ToString(),
            dirtRoadLicense.SafetyRating.ToString("0.00"), FontColor);
        var dirtRoadTask = CreateOrUpdateHtmlFile(PathHelper.DirtRoadHtmlPath, dirtRoadHtml);

        var dirtOvalLicense = driverStats.Data[0].Licenses.FirstOrDefault(x => x.Category == "dirt_oval");
        var dirtOvalGroup = dirtOvalLicense.GroupName.Contains("Class ")
            ? dirtOvalLicense.GroupName.Substring("Class ".Length)
            : dirtOvalLicense.GroupName;
        var dirtOvalHtml = HtmlStrings.LicenseHtml("dirtoval", dirtOvalGroup, dirtOvalLicense.IRating.ToString(),
            dirtOvalLicense.SafetyRating.ToString("0.00"), FontColor);
        var dirtOvalTask = CreateOrUpdateHtmlFile(PathHelper.DirtOvalHtmlPath, dirtOvalHtml);

        var recentEvent = lastRaces.Data.Races.FirstOrDefault();
        var iratingGain = recentEvent.NewiRating - recentEvent.OldiRating;
        var recentEventHtml = HtmlStrings.RecentEventHtml(recentEvent.SeriesName, recentEvent.Track.TrackName,
            recentEvent.StartPosition, recentEvent.FinishPosition, recentEvent.Incidents, iratingGain, FontColor);
        var recentEventTask = CreateOrUpdateHtmlFile(PathHelper.RecentEventHtmlPath, recentEventHtml);

        Task.WhenAll(formulaTask, sportCarTask, ovalTask, dirtRoadTask, dirtOvalTask, recentEventTask)
            .ConfigureAwait(false);

        Dispatcher.Invoke(() =>
        {
            statsStatusText.Content = "✅ Actualización exitosa";
            statsStatusText.Background = System.Windows.Media.Brushes.LightSeaGreen;
            loadingProgressBar.Visibility = Visibility.Visible;
        });

        await Task.Delay(2000);

        Dispatcher.Invoke(() =>
        {
            statsStatusText.Content = "⌛ Esperando...";
            statsStatusText.Background = System.Windows.Media.Brushes.LightYellow;
            loadingProgressBar.Visibility = Visibility.Visible;
        });
    }

    private async Task CreateOrUpdateHtmlFile(string filePath, string html)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        using (var sw = new StreamWriter(filePath))
        {
            await sw.WriteAsync(html);
        }
    }

    private void UpdateStatsToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
    {
        loadingProgressBar.Visibility = Visibility.Hidden;

        updateStatsToggleButton.Background = System.Windows.Media.Brushes.OrangeRed;

        if (_timer != null)
        {
            _timer.Dispose();
            _timer = null;

            statsStatusText.Content = "❌ Actualización detenida";
            statsStatusText.Background = System.Windows.Media.Brushes.LightCoral;
        }
    }

    private void ChangedHexText(object sender, TextChangedEventArgs e)
    {
        if (hexColor.Text.Length != 7 || !hexColor.Text.StartsWith('#'))
            FontColor = "#000000";
        else
            FontColor = hexColor.Text;

        try
        {
            hexColorPreview.Background = FontColor.ToBrush();
        }
        catch
        {
            hexColorPreview.Background = Brushes.Black;
        }
    }
}