using UnityEngine;
using System.Collections;



/// <summary>
/// 游戏数据数据结构
/// </summary>
public class Data_Pool
{



#region POOL


    public static TeamPool m_TeamPool = new TeamPool();
    public static PlayerPool m_PlayerPool = new PlayerPool();

    #endregion



    public static void f_InitPool()
    {

        m_PlayerPool.f_InitPool();


        //----------------------------------------------------------------------------------



    }

   
}
