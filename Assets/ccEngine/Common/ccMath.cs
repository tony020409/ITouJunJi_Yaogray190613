using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ccMath
{
    //public static int f_CreateNewId()//int iNpcIndexScId, int iBuildKeyId)
    //{
    //    string ppSQL = "";
    //    int iKeyId = 0;        

    //    int index = Random.Range(0, 1000);
    //    ppSQL = System.DateTime.Now.ToString("ddhhmm") + index;
    //    iKeyId = ccMath.atoi(ppSQL);

    //    return iKeyId;

    //}

    private static int iKeyIndex = 10000;
    public static int f_CreateKeyId()
    {
        if (iKeyIndex > 99000)
        {
            iKeyIndex = 10000;
        }
        return ++iKeyIndex;

        //string ppSQL = "";
        //int iKeyId = 0;

        //int index = iKeyIndex;// Random.Range(0, 1000);
        //ppSQL = System.DateTime.Now.ToString("hhmmss") + index;
        //iKeyId = ccMath.atoi(ppSQL);

        //return iKeyId;
    }


    public static int atoi(string ppSQL)
    {
        if (ppSQL.Length == 0)
        {
            return 0;
        }     
        //int fTTT = 0;
        try
        {
			return int.Parse(ppSQL);
        }
        catch
        {
            Debug.LogError("数据转换时出错,转换数据：" + ppSQL);
            return 0;
        }
        //int iRet = 0;
        //if (fTTT < 0)
        //{
        //    return fTTT;
        //}
        return -99;
    }

    public static float atof(string ppSQL)
    {
        if (ppSQL.Length == 0)
        {
            return 0;
        }       
        float fTTT = 0;
        try
        {
            fTTT = float.Parse(ppSQL);
        }
        catch
        {
            Debug.LogError("数据转换时出错,转换数据：" + ppSQL);
            return 0;
        }
        //if (fTTT < 0)
        //{
        //    return fTTT;
        //}
        return fTTT;
    }


    public static int[] GetNextLevelData(int star, int nowLv, int nowExp)
    {
        int[] lvUpExp = new int[] { 100000, 150000, 200000, 250000, 300000 }; //升級的經驗值
        int nextLvExp = 0;

        do
        {
            nowLv++;
            nextLvExp = Mathf.RoundToInt(lvUpExp[star - 1] * Mathf.Pow((((float)nowLv - 1f) / 98f), 2.5f));
        } while (nowExp >= nextLvExp);

        return new int[] { nowLv, nextLvExp };
    }

    public static int[] f_String2ArrayInt(string ppSQL, string strSign)
    {
        string[] aData = ppSQL.Split(new string[] { strSign }, System.StringSplitOptions.None);
        int[] aRetData = new int[aData.Length];
        for (int aaa = 0; aaa < aData.Length; aaa++)
        {
            aRetData[aaa] = ccMath.atoi(aData[aaa]);
        }

        return aRetData;
    }

    public static int[] f_String2ArrayInt(string ppSQL, string[] aSign)
    {
        string[] aData = ppSQL.Split(aSign, System.StringSplitOptions.None);
        int[] aRetData = new int[aData.Length];
        for (int aaa = 0; aaa < aData.Length; aaa++)
        {
            aRetData[aaa] = ccMath.atoi(aData[aaa]);
        }

        return aRetData;
    }

    public static int[] f_String2ArrayInt(string ppSQL, char[] aSign)
    {
        string[] aData = ppSQL.Split(aSign, System.StringSplitOptions.None);
        int[] aRetData = new int[aData.Length];
        for (int aaa = 0; aaa < aData.Length; aaa++)
        {
            aRetData[aaa] = ccMath.atoi(aData[aaa]);
        }

        return aRetData;
    }


    public static float[] f_String2ArrayFloat(string ppSQL, string strSign)
    {
        string[] aData = ppSQL.Split(new string[] { strSign }, System.StringSplitOptions.None);
        float[] aRetData = new float[aData.Length];
        for (int aaa = 0; aaa < aData.Length; aaa++)
        {
            aRetData[aaa] = ccMath.atof(aData[aaa]);
        }

        return aRetData;
    }

    public static float[] f_String2ArrayFloat(string ppSQL, char[] aSign)
    {
        string[] aData = ppSQL.Split(aSign, System.StringSplitOptions.None);
        float[] aRetData = new float[aData.Length];
        for (int aaa = 0; aaa < aData.Length; aaa++)
        {
            aRetData[aaa] = ccMath.atof(aData[aaa]);
        }

        return aRetData;
    }


    public static string[] f_String2ArrayString(string ppSQL, string strSign)
    {
        return ppSQL.Split(new string[] { strSign }, System.StringSplitOptions.None);
    }

    public static string[] f_String2ArrayString(string ppSQL, char[] aSign)
    {
        return ppSQL.Split(aSign, System.StringSplitOptions.None);
    }


    public static T CloneObject<T>(T serializableObject)
    {
        object objCopy = null;

        MemoryStream stream = new MemoryStream();
        BinaryFormatter binFormatter = new BinaryFormatter();
        binFormatter.Serialize(stream, serializableObject);
        stream.Position = 0;
        objCopy = (T)binFormatter.Deserialize(stream);
        stream.Close();
        return (T)objCopy;

    }

    public static T CloneArray<T>(T RealObject)
    {
        using (Stream objectStream = new MemoryStream())
        {
            System.Runtime.Serialization.IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(objectStream, RealObject);
            objectStream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(objectStream);
        }
    }


    /// <summary>
    /// 获得0-1的随机命中
    /// </summary>
    /// <param name="fRand"></param>
    /// <returns></returns>
    public static bool f_CheckRandIsOK_0_1(float fRand)
    {
        if (fRand >= 1)
        {
            return true;
        }

        float fRnd = (float)(Random.Range(0f, 1f));

        if (fRnd < fRand)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获得0-100的随机命中
    /// </summary>
    /// <param name="iRand"></param>
    /// <returns></returns>
    public static bool f_CheckRandIsOK_0_100(int iRand)
    {
        if (iRand >= 100)
        {
            return true;
        }
        int iRnd = (Random.Range(0, 100));

        if (iRnd < iRand)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获得点Pos周围fRandRange范围内的随机坐标
    /// </summary>
    /// <param name="Pos"></param>
    /// <param name="fRandRange"></param>
    /// <returns></returns>
    //public static Vector3 f_GetRandIsPosRang(Vector3 Pos, float fRandRange, bool bNeedCheck = true)
    //{
    //    float fRndX = (float)(Random.Range(-1 * fRandRange, fRandRange));
    //    float fRndY = (float)(Random.Range(-1 * fRandRange, fRandRange));
    //    if (bNeedCheck)
    //    {
    //        if (f_CheckRandIsOK_0_1(0.5f))
    //        {
    //            Pos.x = Pos.x + fRndX;
    //            Pos.z = Pos.z + fRndY;
    //        }
    //        else
    //        {
    //            Pos.z = Pos.z + fRndX;
    //            Pos.x = Pos.x + fRndY;
    //        }
    //    }
    //    else
    //    {
    //        Pos.x = Pos.x + fRndX;
    //        Pos.z = Pos.z + fRndY;
    //    }

    //    return Pos;
    //}

    public static int f_GetRand(int iStart, int iEnd)
    {
        return Random.Range(iStart, iEnd);
    }

    public static float f_GetRand(float iStart, float iEnd)
    {
        return (float)(Random.Range(iStart, iEnd));
    }

    /// <summary>
    /// 返回一个数组内的随机元素
    /// </summary>
    /// <param name="aList"></param>
    /// <returns></returns>
    public static int f_GetRand(int[] aList)
    {
        int iPos = Random.Range(0, aList.Length);
        return aList[iPos];
    }

   

    /// <summary>
    /// 0 
    /// 1缩小
    /// 2放大
    /// </summary>
    /// <param name="oP1"></param>
    /// <param name="oP2"></param>
    /// <param name="nP1"></param>
    /// <param name="nP2"></param>
    /// <returns></returns>
    public static int isEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {
        float leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
        float leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));
        if (leng1 < leng2)
        {
            return 1;
        }
        else if (leng1 == leng2)
        {
            return 0;
        }
        else
        {
            return 2;
        }
    }

    /// <summary>
    /// 点是否在椭圆内
    /// </summary>
    /// <param name="fTestX">待检测点x坐标</param>
    /// <param name="fTestY">待检测点y坐标</param>
    /// <param name="fX">椭圆中心点x坐标</param>
    /// <param name="fY">椭圆中心点y坐标</param>
    /// <param name="fR">椭圆的最大半径(实际就是将圆沿y轴压扁变为椭圆的源圆的半径 R = 119)</param>
    /// <param name="fSy">将圆沿y轴压扁变为椭圆时候的比例(长119 高68 则比例为 68/119 = 0.75)</param>
    /// <returns></returns>
    public static bool PointIsInEllipse(float fTestX, float fTestY, float fX, float fY, float fR, float fSy)
    {
        Vector2 TestPos = new Vector2(fTestX, fTestY);
        Vector2 Pos = new Vector2(fX, fY);
        Vector2 tPos = TestPos - Pos;
        tPos.y /= fSy;
        if (tPos.magnitude < fR)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检测Pos2在Pos1的左还是右  0左 1中 2右
    /// </summary>
    /// <param name="Pos1"></param>
    /// <param name="Pos2"></param>
    /// <returns></returns>
    public static int f_CheckPointIsLeftRight(Vector3 Pos1, Vector3 Pos2)
    {
        if (Pos1.x < Pos2.x)
        {
            return 2;
        }
        else if (Pos1.x > Pos2.x)
        {
            return 0;
        }

        return 1;
    }

    /// <summary>
    /// 检测Pos2在Pos1的方向 -1错误 0右上 1右下 2左下 3左上 4右 5左 6上 7下
    /// </summary>
    /// <param name="Pos1"></param>
    /// <param name="Pos2"></param>
    /// <returns></returns>
    public static int f_CheckPointIsWay(Vector3 Pos1, Vector3 Pos2)
    {
        int i = 0;
        if (Pos1.x < Pos2.x)
        {
            i = 10;
        }
        else if (Pos1.x > Pos2.x)
        {
            i = 20;
        }
        if (Pos1.y < Pos2.y)
        {
            i = i + 1;
        }
        else if (Pos1.y > Pos2.y)
        {
            i = i + 2;
        }
        //0右上 1右下 2左下 3左上 
        if (i == 11)
        {
            return 1;
        }
        else if (i == 12)
        {
            return 0;
        }
        else if (i == 21)
        {
            return 2;
        }
        else if (i == 22)
        {
            return 3;
        }

        else if (i == 10)
        {
            return 4;
        }
        else if (i == 20)
        {
            return 5;
        }

        else if (i == 1)
        {
            return 7;
        }
        else if (i == 2)
        {
            return 6;
        }
        
        return -1;
    }

    /// <summary>
    /// 相对X轴夹角
    /// </summary>
    /// <param name="Pos"></param>
    /// <returns></returns>
    public static float f_Angle2X(Vector3 Pos)
    {
        // 計算兩者之間角度
        return Mathf.Atan2(Pos.x, Pos.z) * Mathf.Rad2Deg;
    }

    /// <summary>
    ///  计算夹角的角度 0~360  
    /// </summary>
    /// <param name="from_"></param>
    /// <param name="to_"></param>
    /// <returns></returns>
    public static float f_Angle_360(Vector3 from_, Vector3 to_)
    {
        Vector3 v3 = Vector3.Cross(from_, to_);
        //if (v3.z > 0)
        //    return Vector3.Angle(from_, to_);
        //else
        //    return 360 - Vector3.Angle(from_, to_);
        return Vector3.Angle(from_, to_);
    }

    public static System.DateTime time_t2DateTime(long iTime)
    {
        System.DateTime dt = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(iTime);

        return dt;
    }

    public static long DateTime2time_t(System.DateTime dateTime)
    {
        long time_t;
        System.DateTime dt1 = new System.DateTime(1970, 1, 1, 0, 0, 0);
        System.TimeSpan ts = dateTime - dt1;
        time_t = ts.Ticks / 10000000 - 28800;
        return time_t;
    }



    /// <summary>
    /// int秒转换成"1:10:19"
    /// </summary>
    /// <returns></returns>
    /// <param name="bAddZero">true自动补零 false不进行自动补零</param>
    /// <returns></returns>
    public static string f_Time_Int2String(int iSecTime, bool bAddZero = true)
    {
        System.TimeSpan ts;
        if (iSecTime < 0)
        {
            return "ERO";
        }
        else
        {
            ts = new System.TimeSpan(0, 0, iSecTime);
        }
        //string strPlayerTime = (int)ts.TotalHours + "小时" + ts.Minutes + "分钟" + ts.Seconds + "秒";
        string strHours = ts.Hours.ToString();
        if (bAddZero)
        {
            if (ts.Hours < 10)
            {
                strHours = "0" + ts.Hours.ToString();
            }
        }
        string strMin = ts.Minutes.ToString();
        if (bAddZero)
        {
            if (ts.Minutes < 10)
            {
                strMin = "0" + ts.Minutes.ToString();
            }
        }
        string strSec = ts.Seconds.ToString();
        if (bAddZero)
        {
            if (ts.Seconds < 10)
            {
                strSec = "0" + ts.Seconds.ToString();
            }
        }
        return strHours + ":" + strMin + ":" + strSec;
    }


    public static bool f_CheckStringHaveSign(string strScrString, string strSign)
    {
        if (-1 == strScrString.IndexOf(strSign))
        {
            return false;
        }
        return true;
    }

    public static int LimitInt(int value, int min, int max)
    {
        if (value < min) return min;
        else if (value > max) return max;
        return value;
    }

    public static float LimitFloat(float value, float min, float max)
    {
        if (value < min) return min;
        else if (value > max) return max;
        return value;
    }

    /// <summary>
    /// 去掉三维向量的Y轴，把向量投射到xz平面。
    /// </summary>
    /// <param name="vector3"></param>
    /// <returns></returns>
    public static Vector2 IgnoreYAxis(Vector3 vector3) {
        return new Vector2(vector3.x, vector3.z);
    }


    /// <summary>
    /// 求点到直线的距离，采用数学公式Ax+By+C = 0; d = A*p.x + B * p.y + C / sqrt(A^2 + B ^ 2)
    /// 此算法忽略掉三维向量的Y轴，只在XZ平面进行计算，适用于一般3D游戏。
    /// </summary>
    /// <param name="startPoint">向量起点</param>
    /// <param name="endPoint">向量终点</param>
    /// <param name="point">待求距离的点</param>
    /// <returns></returns>
    public static float DistanceOfPointToVector(Vector3 startPoint, Vector3 endPoint, Vector3 point)
    {
        Vector2 startVe2 = IgnoreYAxis(startPoint);
        Vector2 endVe2 = IgnoreYAxis(endPoint);
        float A = endVe2.y - startVe2.y;
        float B = startVe2.x - endVe2.x;
        float C = endVe2.x * startVe2.y - startVe2.x * endVe2.y;
        float denominator = Mathf.Sqrt(A * A + B * B);
        Vector2 pointVe2 = IgnoreYAxis(point);
        return Mathf.Abs((A * pointVe2.x + B * pointVe2.y + C) / denominator); ;
    }


    /// <summary>
    /// 判断目标点是否位于向量的左边
    /// </summary>
    /// <param name="startPoint">向量起点</param>
    /// <param name="endPoint">向量终点</param>
    /// <param name="point">目标点</param>
    /// <returns>True is on left, false is on right</returns>
    public static bool PointOnLeftSideOfVector(Vector3 vector3, Vector3 originPoint, Vector3 point)
    {
        Vector2 originVec2 = IgnoreYAxis(originPoint);
        Vector2 pointVec2 = (IgnoreYAxis(point) - originVec2).normalized;
        Vector2 vector2 = IgnoreYAxis(vector3);
        float verticalX = originVec2.x;
        float verticalY = (-verticalX * vector2.x) / vector2.y;
        Vector2 norVertical = (new Vector2(verticalX, verticalY)).normalized;
        float dotValue = Vector2.Dot(norVertical, pointVec2);
        return dotValue < 0f;
    }


    static System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
    public static void f_TestStart()
    {        
        sw.Start();
    }

    public static void f_TestEnd()
    {
        sw.Stop();
        Debug.Log("------------" + sw.ElapsedMilliseconds);       
    }


    public static byte[] f_Memcpy(byte[] bBuf, int iMovePos, int iDataLen)
    {
        byte[] bActivity23 = new byte[iDataLen];
        System.Array.Copy(bBuf, iMovePos, bActivity23, 0, iDataLen);
        return bActivity23;
    }


	/// <summary>
	/// 获得字符串的字节长度 
	/// </summary>
	public static int f_GetStringBytesLength(string str)
	{
		return System.Text.Encoding.Default.GetBytes(str).Length;
	}
    

}

