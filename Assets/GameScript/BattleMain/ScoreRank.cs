using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// 排行榜模式
/// </summary>
public enum EM_RankType{

    [Rename("全球 (尚未完成)")]
    /// <summary> 全球 </summary>
    Gloabl,

    [Rename("單場")]
    /// <summary> 單場 </summary>
    Single,
}



/// <summary>
/// 排行榜
/// </summary>
public class ScoreRank : MonoBehaviour {

    [Rename("是否顯示排行榜")]
    public bool useRank;

    [Rename("排行榜類型")]
    public EM_RankType _RankType;

    [Rename("全域排行榜")]
    public GameObject RankRoot_Global;

    [Rename("單場排行榜")]
    public GameObject RankRoot_Single;

    [Line()]
    [Rename(new string[] {
        "第一名序號",
        "第一名擊殺",
        "第一名命中率",
        "第一名遊戲時間" })]
    public Text[] RankUI_1 = new Text[4];

    [Rename(new string[] {
        "第二名序號",
        "第二名擊殺",
        "第二名命中率",
        "第二名遊戲時間" })]
    public Text[] RankUI_2 = new Text[4];

    [Rename(new string[] {
        "第三名序號",
        "第三名擊殺",
        "第三名命中率",
        "第三名遊戲時間" })]
    public Text[] RankUI_3 = new Text[4];

    [Rename(new string[] {
        "第四名序號",
        "第四名擊殺",
        "第四名擊殺",
        "第四名遊戲時間" })]
    public Text[] RankUI_4 = new Text[4];

    [Rename(new string[] {
        "第五名序號",
        "第五名擊殺",
        "第五名命中率",
        "第五名遊戲時間" })]
    public Text[] RankUI_5 = new Text[4];

    [Rename(new string[] {
        "自己的名次",
        "自己的序號",
        "自己的擊殺",
        "自己的命中率",
        "自己的遊戲時間" })]
    public Text[] UI_Self = new Text[5];

    [Line()]
    [Rename(new string[] {
        "玩家0 的名次",
        "玩家0 的序號",
        "玩家0 的擊殺",
        "玩家0 的命中率",
        "玩家0 的遊戲時間" })]
    public Text[] UI_Player_0 = new Text[5];


    [Rename(new string[] {
        "玩家1 的名次",
        "玩家1 的序號",
        "玩家1 的擊殺",
        "玩家1 的命中率",
        "玩家1 的遊戲時間" })]
    public Text[] UI_Player_1 = new Text[5];


    /// <summary>
    /// 記錄名次和對應的分數
    /// </summary>
    private Dictionary<float, int> _dSort = new Dictionary<float, int>();

    /// <summary>
    /// 記錄命中率用
    /// </summary>
    private float[] _aHitRate = new float[2];



    //讓其他地方可以呼叫排行榜
    private static ScoreRank _Instance = null;
    public static ScoreRank GetInstance() {
        return _Instance;
    }


    void Start () {
        _Instance = this;
        RankRoot_Single.SetActive(false);
        RankRoot_Single.SetActive(false);
    }


    /// <summary>
    /// 從 BattleMain 接收排行榜資料
    /// </summary>
    public void Receive_GameOverData(CTS_GameOver tCTS_GameOver)
    {
        //List<PlayerDT> aPlayer = Data_Pool.m_PlayerPool.f_GetAll();
        //MessageBox.DEBUG(
        //    "成績結算：\n" +
        //    "玩家人數共 " + aPlayer.Count + "名\n" +
        //    "花費" + tCTS_GameOver.m_iTime + "秒\n" +
        //    "所有玩家成績如下：");
        //
        //for (int i = 0; i < aPlayer.Count; i++) {
        //    MessageBox.DEBUG("玩家 [" + i + "]： / " +
        //    "發射了 " + tCTS_GameOver.m_aScore[i].m_iShot + "顆子彈  / " +
        //    "殺死了 " + tCTS_GameOver.m_aScore[i].m_iShotDie + "個敵人" +
        //    "命中 " + tCTS_GameOver.m_aScore[i].m_iShotHit + "次 / " +
        //    "其中頭部 " + tCTS_GameOver.m_aScore[i].m_iHeadShotDie + "次 \n");
        //}


        //
        if (_RankType == EM_RankType.Gloabl) {
            Rank_Global(tCTS_GameOver);
        }
        else if(_RankType == EM_RankType.Single) {
            Rank_Single(tCTS_GameOver);
        }

    }


    /// <summary>
    /// 全球排行榜
    /// </summary>
    /// <param name="tCTS_GameOver"> 資料 </param>
    public void Rank_Global(CTS_GameOver tCTS_GameOver) {

        //全球排行榜目前強制關閉
        useRank = false;

        //顯示排行榜
        if (useRank) {
            RankRoot_Global.SetActive(true);
        }
    }



    /// <summary>
    /// 單人排行榜
    /// </summary>
    /// <param name="tCTS_GameOver"> 資料 </param>
    public void Rank_Single(CTS_GameOver tCTS_GameOver) {

        MessageBox.DEBUG("此台電腦的玩家編號是：" + tCTS_GameOver.m_iUserId); //tCTS_GameOver.m_aScore[1].m_iGamePlayerId

        //玩家編號
        if (StaticValue.m_UserDataUnit.m_PlayerDT.m_iId == 0) {
            UI_Player_0[1].text = "0 (你)";
        }
        else if (StaticValue.m_UserDataUnit.m_PlayerDT.m_iId == 1) {
            UI_Player_1[1].text = "1 (你)";
        }


        //玩家0 的分數細項
        _aHitRate[0] = f_HitRate(tCTS_GameOver.m_aScore[0].m_iShotHit, tCTS_GameOver.m_aScore[0].m_iShot); //記錄命中率
        UI_Player_0 [2].text = tCTS_GameOver.m_aScore[0].m_iShotDie.ToString();                            //擊殺
        UI_Player_0 [3].text = _aHitRate[0].ToString() + "%";                                              //命中率
        UI_Player_0 [4].text = f_Time0000(tCTS_GameOver.m_iTime);                                          //遊戲時間


        //玩家1 的分數細項
        if (tCTS_GameOver.m_iPlayerNum >= 2) {
            _aHitRate[1] = f_HitRate(tCTS_GameOver.m_aScore[1].m_iShotHit, tCTS_GameOver.m_aScore[1].m_iShot); //記錄命中率
            UI_Player_1 [2].text = tCTS_GameOver.m_aScore[1].m_iShotDie.ToString();                            //擊殺
            UI_Player_1 [3].text = _aHitRate[1].ToString() + "%";                                              //命中率
            UI_Player_1 [4].text = f_Time0000(tCTS_GameOver.m_iTime);                                          //遊戲時間
        } else {
            UI_Player_1[0].text = " - ";
            UI_Player_1[1].text = " - ";
            UI_Player_1[2].text = " - ";
            UI_Player_1[3].text = " - ";
            UI_Player_1[4].text = " - ";
        }


        //名次
        //if (tCTS_GameOver.m_iPlayerNum >= 2) {
        //    f_SortRank(_aHitRate);
        //    UI_Player_0[0].text = f_GetSort(_aHitRate[0]).ToString();
        //    UI_Player_1[0].text = f_GetSort(_aHitRate[1]).ToString();
        //} else {
        //    UI_Player_0[0].text = "1";
        //    UI_Player_1[0].text = "-";
        //}

        //單人排行榜不管名次
        UI_Player_0[0].text = "-";
        UI_Player_1[0].text = "-";

        //顯示排行榜
        if (useRank) {
            RankRoot_Single.SetActive(true);
        }
    }


    /// <summary>
    /// 將時間換算成 分+秒 顯示
    /// </summary>
    /// <param name="m_iTime"> 總秒數 </param>
    public string f_Time0000(int m_iTime) {
        return (m_iTime/60).ToString("00") + "分" + (m_iTime%60).ToString("00") + "秒";
    }


    /// <summary>
    /// 命中率
    /// </summary>
    /// <param name="_iShotHit"> 命中數 </param>
    /// <param name="_iShot"   > 總射擊量 </param>
    public float f_HitRate(int _iShotHit, int _iShot) {
        if (_iShot == 0) {
            return 0;
        }
        //計算命中率
        float tmp = (((float)_iShotHit / _iShot) * 100);

        //命中率如果有小數只取小數點後兩位來顯示
        if (tmp.ToString().Contains(".")) {
            tmp = float.Parse(tmp.ToString("0.00"));
        }

        return tmp;
    }



    /// <summary>
    /// 排名
    /// </summary>
    /// <param name="basis"  > 判斷標準 </param>
    public void f_SortRank(float[] basis) {
        float[] _tmpSort = new float[basis.Length];
        _tmpSort = basis;
        for (int i = 0; i < _tmpSort.Length - 1; i++) {
            for (int j = i + 1; j < _tmpSort.Length; j++){
                //若第i個的分數比第j個大，則兩筆資料調換 
                if (_tmpSort[i] > _tmpSort[j]) { 
                    float temp = _tmpSort[i];
                    _tmpSort[i] = _tmpSort[j];
                    _tmpSort[j] = temp;
                }
            }
        }

        for (int i = 0; i < _tmpSort.Length; i++) {
            if (!_dSort.ContainsKey(i)) {
                _dSort.Add(_tmpSort[i], i);
            }
        }
    }


    /// <summary>
    /// 取得排名
    /// </summary>
    /// <param name="mySorce"> 自己分數 </param>
    /// <returns></returns>
    public int f_GetSort(float mySorce) {
        if (_dSort.ContainsKey(mySorce)) {
            return _dSort[mySorce]+1;
        }
        return -99;
    }



}
