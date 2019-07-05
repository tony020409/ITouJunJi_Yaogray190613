// http://wiki.unity3d.com/index.php/OpenInFileBrowser
// CC BY-SA 3.0 http://creativecommons.org/licenses/by-sa/3.0/



/// <summary>
/// 資料夾瀏覽器
/// </summary>
public static class OpenInFileBrowser {


    public static bool IsInMacOS {
        get {
            return UnityEngine.SystemInfo.operatingSystem.IndexOf("Mac OS") != -1;
        }
    }

    public static bool IsInWinOS {
        get {
            return UnityEngine.SystemInfo.operatingSystem.IndexOf("Windows") != -1;
        }
    }


    public static void OpenInMac(string path) {
        bool openInsidesOfFolder = false;

        // try mac
        string macPath = path.Replace("\\", "/"); // mac finder doesn't like backward slashes

        // if path requested is a folder, automatically open insides of that folder
        if (System.IO.Directory.Exists(macPath))  {
            openInsidesOfFolder = true;
        }

        if (!macPath.StartsWith("\"")) {
            macPath = "\"" + macPath;
        }

        if (!macPath.EndsWith("\"")) {
            macPath = macPath + "\"";
        }

        string arguments = (openInsidesOfFolder ? "" : "-R ") + macPath;

        try {
            System.Diagnostics.Process.Start("open", arguments);
        }
        catch (System.ComponentModel.Win32Exception e) {
            // tried to open mac finder in windows
            // just silently skip error
            // we currently have no platform define for the current OS we are in, so we resort to this
            e.HelpLink = ""; // do anything with this variable to silence warning about not using it
        }
    }



    public static void OpenInWin(string path) {
        bool openInsidesOfFolder = false;

        // try windows
        string winPath = path.Replace("/", "\\"); // windows explorer doesn't like forward slashes

        // if path requested is a folder, automatically open insides of that folder
        if (System.IO.Directory.Exists(winPath))  {
            openInsidesOfFolder = true;
        } else {
            System.IO.Directory.CreateDirectory(winPath);
        }

        try {
            System.Diagnostics.Process.Start("explorer.exe", (openInsidesOfFolder ? "/root," : "/select,") + winPath);
        }
        catch (System.ComponentModel.Win32Exception e) {
            // tried to open win explorer in mac
            // just silently skip error
            // we currently have no platform define for the current OS we are in, so we resort to this
            e.HelpLink = ""; // do anything with this variable to silence warning about not using it
        }
    }



    public static void OpenFilePath(string path) {
        if (IsInWinOS) {
            OpenInWin(path);
        }
        else if (IsInMacOS) {
            OpenInMac(path);
        }
        // couldn't determine OS
        else {
            OpenInWin(path);
            OpenInMac(path);
        }
    }
}