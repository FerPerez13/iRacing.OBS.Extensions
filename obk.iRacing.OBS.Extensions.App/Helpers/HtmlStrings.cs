using System.IO;

namespace WpfApp3.Helpers;

public static class HtmlStrings
{
    public static string LicenseHtml(string type, string license, string irating, string sr)
    {
        return
            $"<head><meta http-equiv=\"refresh\" content=\"1\"></head><html><table><tr><td><img src=\"..\\resources\\{type.ToLower()}_{license.ToLower()}.svg\"></td><td><div style=\"font-size:125px;color:#fff;font-weight:700;font-family:verdana\">{sr}</div><div style=\"font-size:125px;color:#fff;font-weight:700;font-family:verdana\">{irating}</div></td></tr></table></html>";
    }
    
    public static string RecentEventHtml(string eventName, string trackName, int startingPosition, int finishPosition, int incidents, int gain)
    {
        return
            $"<meta content=\"1\" http-equiv=\"refresh\"><div style=\"font-size:150px;color:#fff;font-weight:700;font-family:verdana\">Resultado carrera anterior</div><div style=\"font-size:125px;color:#fff;font-weight:500;font-family:verdana\">{eventName}</div><div style=\"font-size:125px;color:#fff;font-weight:500;font-family:verdana\">{trackName}</div><table><tr><td><div style=\"font-size:125px;color:#fff;font-weight:700;font-family:verdana;margin:50px\"><img src=\"..\\resources\\green-flag.png\" style=\"width:200;height:200px\"> {startingPosition}</div></td><td><div style=\"font-size:125px;color:#fff;font-weight:700;font-family:verdana;margin:50px\"><img src=\"..\\resources\\check-flag.png\" style=\"width:200px;height:200px\"> {finishPosition}</div></td><td><div style=\"font-size:125px;color:#fff;font-weight:700;font-family:verdana;margin:50px\"><img src=\"..\\resources\\incident.png\" style=\"width:200px;height:200px\"> {incidents}</div></td><td><div style=\"font-size:125px;color:#fff;font-weight:700;font-family:verdana;margin:50px\"><img src=\"..\\resources\\gain.png\" style=\"width:200px;height:200px\"> {gain}</div></td></tr></table>";
    }
}