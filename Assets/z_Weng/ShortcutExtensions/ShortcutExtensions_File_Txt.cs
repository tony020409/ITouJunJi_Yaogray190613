
using System.IO;                      //外部讀取檔案用
using System.Text.RegularExpressions; //解析字元用


public static class ShortcutExtensions_TXT
{

    /// <summary>
    /// 獲取txt文本 指定某行的內容
    /// </summary>
    /// <param name="PathName"> 路徑名+檔案名 </param>
    /// <param name="lineNumber"> 第幾行 </param>
    public static string ReadFileLine(this string PathName, int lineNumber){
        string[] strs = File.ReadAllLines(PathName);
        if (lineNumber == 0){
            return "";
        } else {
            return strs[lineNumber - 1];   //返回第linenumber行内容
        }
    }

    /// <summary>
    /// 以特殊字元對文本進行拆分 (「姓名：小王」，以(value =：)拆分後，得到「姓名」和「小王」)
    /// </summary>
    /// <param name="target"> 要分析的內容(string) </param>
    /// <param name="value"> 特殊字元 </param>
    /// <returns></returns>
    public static string[] SplitText(this string target, string value){
        string[] tmpContent = Regex.Split(target, value, RegexOptions.IgnoreCase); //根據換行符劃分出多個行文本
        return tmpContent;                                                         //回傳解析結果
    }

    /// <summary>
    /// 取得 以特殊字元對文本進行拆分後 第 Index項的內容
    /// </summary>
    /// <param name="target"> 要分析的內容(string) </param>
    /// <param name="value"> 特殊字元</param>
    /// <param name="index"> 拆分後的第幾項 </param>
    public static string Get_SplitTextIndex(this string target, string value, int index){
        string[] tmpContent = Regex.Split(target, value, RegexOptions.IgnoreCase); //根據換行符劃分出多個行文本
        return tmpContent[index];                                                   //回傳解析結果
    }


    /// <summary>
    /// String 轉換成 int
    /// </summary>
    /// <param name="target"> 要轉換的內容(string) </param>
    public static int ParsetoInt(this string target){
        int tmpInt = int.Parse(target);
        return tmpInt;
    }


    /// <summary>
    /// String 轉換成 float
    /// </summary>
    /// <param name="target"> 要轉換的內容(string) </param>
    public static float ParsetoFloat(this string target){
        float tmpFloat = float.Parse(target);
        return tmpFloat;
    }


    /// <summary>
    /// 在指定路徑創建檔案 (這個路徑需包含檔案名稱和格式)
    /// </summary>
    /// <param name="PathName"> 包含檔案名稱和格式的路徑 </param>
    public static string CreateFile(this string PathName){

        //如果檔案路徑不存在，就創建檔案
        if (File.Exists(PathName) == false){
            FileStream fr = File.Open(PathName, FileMode.OpenOrCreate);
            fr.Close();
        }
        return string.Empty;
    }


    /// <summary>
    /// 在指定路徑創建指定檔案
    /// </summary>
    /// <param name="PathName"> 路徑(不含檔案名稱與格式) </param>
    /// <param name="FileName"> 檔案名稱(含格式，如：.txt) </param>
    public static string CreateFile(this string PathName, string FileName){

        //如果檔案的位置資料夾不存在，就在指定的路徑中創建資料夾。
        if (Directory.Exists(PathName) == false){
            DirectoryInfo di = Directory.CreateDirectory(PathName);
            FileStream fr = File.Open(FileName, FileMode.OpenOrCreate);
            fr.Close();
            return string.Empty;
        }

        //如果檔案路徑不存在，就創建路徑資料夾和檔案
        if (File.Exists(PathName + FileName) == false) {
            FileStream fr = File.Open(PathName + FileName, FileMode.OpenOrCreate);
            fr.Close();
        }
        return string.Empty;
    }



    //預計添加功能
    //02. 修改並儲存txt

}





