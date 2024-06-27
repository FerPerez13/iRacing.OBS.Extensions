using System.IO;

namespace WpfApp3.Helpers;

public static class PathHelper
{
    public static string DocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    public static string IracingExtensionsPath = Path.Combine(DocumentsPath, "iracingextensions");
    public static string ResourcesPath = Path.Combine(IracingExtensionsPath, "resources");
    public static string HtmlResults = Path.Combine(IracingExtensionsPath, "HtmlResults");
    
    public static string CredentialsPath = Path.Combine(IracingExtensionsPath, "credentials.txt");
    public static string IconPath = Path.Combine(ResourcesPath, "appicon.ico"); 
    public static string LoginImagePath = Path.Combine(ResourcesPath, "iracing.png");
    
    public static string FormulaHtmlPath = Path.Combine(HtmlResults, "formula_info.html");
    public static string SportCarHtmlPath = Path.Combine(HtmlResults, "sport_car_info.html");
    public static string OvalHtmlPath = Path.Combine(HtmlResults, "oval_info.html");
    public static string DirtRoadHtmlPath = Path.Combine(HtmlResults, "dirt_road_info.html");
    public static string DirtOvalHtmlPath = Path.Combine(HtmlResults, "dirt_oval_info.html");
    public static string RecentEventHtmlPath = Path.Combine(HtmlResults, "recent_event.html");
    

}