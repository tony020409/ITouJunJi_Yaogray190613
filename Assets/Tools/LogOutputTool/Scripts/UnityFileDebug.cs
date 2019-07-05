using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

//修改自：AssetStore免費插件
//https://assetstore.unity.com/packages/tools/utilities/unity-file-debug-72250
//修改內容：
// 1. 這個插件原本有一個覆寫原生 Debug 的程式，因為會與其他插件的 Debug.Log 衝突，所以修改後將那個程式拔除了
// 2. 相對路徑原本是固定在 Application.persistentDataPath，修改版可以額外設定相對路徑的類型，並且在相對路徑再增設其他資料夾
// 3. 修正輸出後的文件中文會亂碼的問題 (修正方式：改用 Encoding.UTF8 輸出)
// 4. 原本的路徑類型是以絕對路徑決定，修改版的以相對路徑決定
// 5. 修復 Log內容第一個字被吃掉、Log內容有 <color> 、 [] 的字時，記錄會出錯的問題  
//    (修正方式：原本 HandleLog() 裡會對師道的數據做處理，砍掉變成直接單純的 output.l = logString;)
// 6. 添加一個可以透過讀取外部文件，決定是否要輸出log文本的機制。
//    (外部文件是寫死在 StreamingAssets裡的 Config_LogOutput.txt， 以「：」後的字做判斷  1= 啟用；0 = 關閉；如果沒有這個文件默認關閉)
// 7. 參數的中文化。


namespace UnityFileDebug {

    #region　相關類型
    /// <summary>
    /// 輸出格式
    /// 短鍵名(short keynames)用於使json輸出變小
    /// </summary>
    [System.Serializable]
    public class LogOutput {
        /// <summary>
        /// Log 類型(type)
        /// </summary>
        public string t;

        /// <summary>
        /// Log 出現時間(time)
        /// </summary>
        public string tm;

        /// <summary>
        /// Log 內容(log)
        /// </summary>
        public string l;

        /// <summary>
        /// 相關的程式堆疊(stack)
        /// </summary>
        public string s;
    }


    /// <summary>
    /// Log 輸出類型
    /// </summary>
    public enum DebugOutputType {
        CSV,
        JSON,
        TSV,
        TXT
    }


    /// <summary>
    /// Log 輸出類型
    /// </summary>
    public enum DebugOutputRelativePath {

        /// <summary>
        /// Asset 資料夾
        /// </summary>
        dataPath,

        /// <summary>
        /// Asset 資料夾的上一層
        /// </summary>
        dataPathIndex,

        persistentDataPath,
        streamingAssetsPath,
    }
    #endregion

    [ExecuteInEditMode]
    public class UnityFileDebug : MonoBehaviour {


        #region　參數
        /// <summary>
        /// 是否輸出 Log文本的開關 (由外部文件開關)
        /// </summary>
        public bool useLogOutput = false;

        /// <summary>
        /// 輸出檔名
        /// </summary>
        public string fileName = "MyGame";

        /// <summary>
        /// 使用相對路徑 (=false 表示使用絕對路徑(像是網址(?)))
        /// </summary>
        public bool useRelativePath = true;

        /// <summary>
        /// 輸出類型
        /// </summary>
        public DebugOutputType fileType = DebugOutputType.TXT;

        /// <summary>
        /// 絕對路徑 (網址?)
        /// </summary>
        public string absolutePath = "c:\\";

        /// <summary>
        /// 相對路徑類型
        /// </summary>
        public DebugOutputRelativePath relativePathType = DebugOutputRelativePath.dataPathIndex;

        /// <summary>
        /// 相對路徑時的資料夾
        /// </summary>
        public string relativeFolder = "LogOutput";


        /// <summary>
        /// 輸出路徑
        /// </summary>
        public string filePath;

        /// <summary>
        /// 完整的輸出路徑 (含檔案名稱與格式)
        /// </summary>
        public string filePathFull;

        /// <summary>
        /// 行數
        /// </summary>
        public int count = 0;

        /// <summary>
        /// 導出用
        /// </summary>
        System.IO.StreamWriter fileWriter;

        /// <summary>
        /// 外部開關功能文檔的路徑 (寫死，含檔名)
        /// </summary>
        private string ConfigPathName;


        /// <summary>
        /// 取得檔案格式
        /// </summary>
        /// <param name="type"> 輸出格式 </param>
        string FileExtensionFromType(DebugOutputType type) {
            switch (type) {
                case DebugOutputType.JSON:
                    return ".json";
                case DebugOutputType.CSV:
                    return ".csv";
                case DebugOutputType.TSV:
                    return ".tsv";
                case DebugOutputType.TXT:
                    return ".txt";
                default:
                    return ".txt";
            }
        }
        #endregion

        void OnEnable()  {

            //外讀讀取一個叫 Config_LogOutput.txt的文件決定要不要保留輸出日誌
            ConfigPathName = Application.streamingAssetsPath + "/" + "Config_LogOutput.txt";
            useLogOutput = CheckOutputDebug(ConfigPathName, "保留輸出", "1");
            if (!useLogOutput) {
                Debug.LogWarning("不保留 Log日誌");
                return;
            } else {
                Debug.LogWarning("保留 Log日誌");
            }

            UpdateFilePath();
            if (Application.isPlaying) {
                count = 0;
                fileWriter = new System.IO.StreamWriter(filePathFull, false, System.Text.Encoding.UTF8);
                fileWriter.AutoFlush = true;
                switch (fileType) {
                    case DebugOutputType.CSV:
                        fileWriter.WriteLine("type,time,log,stack");
                        break;
                    case DebugOutputType.JSON:
                        fileWriter.WriteLine("[");
                        break;
                    case DebugOutputType.TSV:
                        fileWriter.WriteLine("type\ttime\tlog\tstack");
                        break;
                }
                Application.logMessageReceived += HandleLog;
            }
        }


        /// <summary>
        /// 取得檔案路徑
        /// </summary>
        public void UpdateFilePath() {
            if ( !useRelativePath) {
                filePath = absolutePath;
            }
            else {
                filePath = GetRelativePath(relativePathType);
                if (!Directory.Exists(filePath)) {
                    Directory.CreateDirectory(filePath);
                }
            }
            filePathFull = System.IO.Path.Combine(filePath, fileName + "_" + System.DateTime.Now.ToString("yyyy.MM.dd__HH.mm.ss") + FileExtensionFromType(fileType));
        }


        void OnDisable() {

            if (!useLogOutput) {
                return;
            }

            if (Application.isPlaying) {
                Application.logMessageReceived -= HandleLog;

                switch (fileType) {
                    case DebugOutputType.JSON:
                        fileWriter.WriteLine("\n]");
                        break;
                    case DebugOutputType.CSV:
                        break;
                    case DebugOutputType.TSV:
                        break;
                    default:
                        break;
                }
                fileWriter.Close();
            }
        }


        /// <summary>
        /// 處理 Log 文本的輸出內容
        /// </summary>
        /// <param name="logString" > Log內容 </param>
        /// <param name="stackTrace"> 相關的程式堆疊 </param>
        /// <param name="type"      > Log類型 </param>
        void HandleLog(string logString, string stackTrace, LogType type) {
            LogOutput output = new LogOutput();
            output.t = GetLogType(type);
            output.l = logString;
            output.s = stackTrace;
            output.tm = System.DateTime.Now.ToString("yyyy-MM-dd    HH：mm：ss");

            switch (fileType) {
                case DebugOutputType.CSV:
                    fileWriter.WriteLine(output.t + "," + output.tm + "," + output.l.Replace(",", " ").Replace("\n", "") + "," + output.s.Replace(",", " ").Replace("\n", ""));
                    break;
                case DebugOutputType.JSON:
                    fileWriter.Write((count == 0 ? "" : ",\n") + JsonUtility.ToJson(output));
                    break;
                case DebugOutputType.TSV:
                    fileWriter.WriteLine(output.t + "\t" + output.tm + "\t" + output.l.Replace("\t", " ").Replace("\n", "") + "\t" + output.s.Replace("\t", " ").Replace("\n", ""));
                    break;
                case DebugOutputType.TXT:
                    fileWriter.WriteLine("Type  : " + output.t);
                    fileWriter.WriteLine("Time  : " + output.tm);
                    fileWriter.WriteLine("Log   : " + output.l);
                    fileWriter.WriteLine("Stack : " + output.s + "\n");
                    break;
            }

            count++;
        }


        /// <summary>
        /// 返回類型 string
        /// </summary>
        /// <param name="type"> Log類型 </param>
        private string GetLogType(LogType type) {
            switch (type) {
                case LogType.Assert:
                    return " Assert";
                case LogType.Error:
                    return " Error";
                case LogType.Exception:
                    return " Exception";
                case LogType.Log:
                    return " Log";
                case LogType.Warning:
                    return " Warning";
                default:
                    return " - ";
            }
        }



        /// <summary>
        /// 返回相對路徑類型
        /// </summary>
        /// <param name="type"> Log類型 </param>
        public string GetRelativePath(DebugOutputRelativePath type) {
            switch (type) {
                //系統資料夾
                case DebugOutputRelativePath.persistentDataPath:
                    return Application.persistentDataPath  + "/" + relativeFolder;

                //  Asset 資料夾下
                case DebugOutputRelativePath.dataPath:
                    return Application.dataPath + "/" + relativeFolder;

                //  Asset 資料夾的上一層資料夾
                case DebugOutputRelativePath.dataPathIndex:
                    return GetParentDirectoryPath (Application.dataPath, 1) + "/" + relativeFolder;

                // Asset 資料夾下的 StreamingAsset資料夾
                case DebugOutputRelativePath.streamingAssetsPath:
                    return Application.streamingAssetsPath + "/" + relativeFolder;

                //預設路徑
                default:
                    return GetParentDirectoryPath(Application.dataPath, 1) + "/" + relativeFolder;
            }
        }

        /// <summary>
        /// 取得某目錄的上幾層的目錄路徑
        /// </summary>
        /// <param name="folderPath"> 目錄路徑 </param>
        /// <param name="levels"    > 要往上幾層 </param>
        public string GetParentDirectoryPath(string folderPath, int levels) {
            string result = folderPath;
            for (int i = 0; i < levels; i++) {
                if (Directory.GetParent(result) != null) {
                    result = Directory.GetParent(result).FullName;
                }
                else {
                    return result;
                }
            }
            return result;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"> 檔案路徑(含檔名與.txt) </param>
        /// <param name="tmp"     > 參考字 </param>
        /// <param name="value"   > 決定項 </param>
        bool CheckOutputDebug(string filePath, string tmp, string value) {
            if (File.Exists(filePath)) {
                string[] strs = File.ReadAllLines(filePath);
                for (int i = 0; i < strs.Length; i++) {
                    if (strs[i].Contains(tmp)) {
                        if (f_GetSplitTextIndex(strs[i], "：", 1) == value) {
                            return true;
                        }
                        else {
                            continue;
                        }
                    }
                }
            }
            //找不到文檔預設關閉不輸出
            return false;
        }



        /// <summary>
        /// 取得某段文字以特殊字元拆分後 第 Index項的內容
        /// </summary>
        /// <param name="tmp"> 要分析的內容(string) </param>
        /// <param name="value" > 特殊字元 </param>
        /// <param name="index" > 拆分後的第幾項 </param>
        public string f_GetSplitTextIndex(string tmp, string value, int index) {
            string[] tmpContent = Regex.Split(tmp, value, RegexOptions.IgnoreCase); //根據換行符劃分出多個行文本
            if (tmpContent.Length == 1) {
                return "-99";
            }
            return tmpContent[index];                                                   //回傳解析結果
        }


    }
}