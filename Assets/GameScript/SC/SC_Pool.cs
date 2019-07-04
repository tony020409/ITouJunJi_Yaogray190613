using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

/// <summary>
/// 脚本管理器
/// </summary>
public class SC_Pool
{
    private bool _bLoadSuc;

    public CharacterSC m_CharacterSC = new CharacterSC();
    public GunSC m_GunSC = new GunSC();

    public GameControll_ParameterSC m_GameControll_ParameterSC = new GameControll_ParameterSC();
    public GameControll_ConditionSC m_GameControll_ConditionSC = new GameControll_ConditionSC();
    public GameControllSC m_GameControllSC = new GameControllSC();
    public CharacterAISC m_CharacterAISC = new CharacterAISC();
    public CharacterAIConditionSC m_CharacterAIConditionSC = new CharacterAIConditionSC();
    public CharacterAIRunSC m_CharacterAIRunSC = new CharacterAIRunSC();
    public BulletSC m_BulletSC = new BulletSC();

    List<NBaseSC> _aSCList = new List<NBaseSC>();
    public void f_LoadSC(byte[] bData)
    {
        _bLoadSuc = false;

        ///////////////////////////////////////////////////////////////

        _aSCList.Add(m_CharacterSC);
        _aSCList.Add(m_GunSC);
        _aSCList.Add(m_GameControll_ParameterSC);
        _aSCList.Add(m_GameControll_ConditionSC);
        _aSCList.Add(m_GameControllSC);
        _aSCList.Add(m_CharacterAISC);
        _aSCList.Add(m_CharacterAIConditionSC);
        _aSCList.Add(m_CharacterAIRunSC);
        _aSCList.Add(m_BulletSC);

        ///////////////////////////////////////////////////////////////

        int i = 0;
        MessageBox.DEBUG("解析脚本");

        string ppSQL;
        byte[] b = new byte[512];
        System.Array.Copy(bData, b, 5);
        int iHeadLen = int.Parse(System.Text.Encoding.UTF8.GetString(b));
        System.Array.Copy(bData, 5, b, 0, iHeadLen);
        string strHeadData = System.Text.Encoding.UTF8.GetString(b);
        string[] ttt = strHeadData.Split(new string[] { "," }, System.StringSplitOptions.None);
        int iMovePos = iHeadLen + 5;

        for (i = 0; i < _aSCList.Count; i++)
        {
            //yield return new WaitForSeconds(4.5f/_aSCList.Count);
            MessageBox.DEBUG("SC " + i + " " + _aSCList[i].m_strRegDTName);
            int iDataLen = int.Parse(ttt[i]);
            ppSQL = ZipTools.aaa556(bData, iMovePos, iDataLen);
            _aSCList[i].f_LoadSCForData(ppSQL);
            iMovePos = iMovePos + iDataLen;
        }

        _bLoadSuc = true;


        MessageBox.DEBUG("解析脚本成功");
    }



    public bool f_CheckLoadSuc()
    {
        return _bLoadSuc;
    }

}
