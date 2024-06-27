using System.IO;

namespace WpfApp3.Helpers;

public static class HtmlStrings
{
    public static string LicenseHtml(string type, string license, string irating, string sr)
    {
        return
            $"<head><meta http-equiv=\"refresh\" content=\"1\"></head><html><table><tr><td><img src=\"{PathHelper.ResourcesPath}/{type.ToLower()}_{license.ToLower()}.svg\"></td><td><div style=\"font-size:125px;color:#fff;font-weight:700;font-family:verdana\">{sr}</div><div style=\"font-size:125px;color:#fff;font-weight:700;font-family:verdana\">{irating}</div></td></tr></table></html>";
    }
    
    public static string RecentEventHtml(string eventName, string carName, string trackName, int startingPosition, int finishPosition)
    {
        return
            $"<meta content=1 http-equiv=refresh><div style=font-size:125px;color:#fff;font-weight:700;font-family:verdana>{eventName}</div><div style=font-size:125px;color:#fff;font-weight:700;font-family:verdana>{carName}</div><div style=font-size:125px;color:#fff;font-weight:700;font-family:verdana>{trackName}</div><table><tr><td><div style=font-size:125px;color:#fff;font-weight:700;font-family:verdana>De: {startingPosition} a:</div><td><div style=font-size:125px;color:#fff;font-weight:700;font-family:verdana>{finishPosition}</div></table>";
    }
}