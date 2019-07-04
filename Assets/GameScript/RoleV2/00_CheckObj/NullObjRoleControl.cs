using ccU3DEngine;
using UnityEngine;

public class NullObjRoleControl : BaseRoleControllV2
{

   
    [HelpBox("沒功能的物件，作為偵測點之類用", HelpBoxType.Info)]

    //接收動畫事件用
    private ccCallback _CallBack_RecvAnimatorEvent = null;

    //[Rename("在角色表是設定成隱形的")]
    //public bool isIgnore;


    //初始化
    public override void f_Init(int iId, BaseActionController tBaseActionController, GameEM.TeamType tTeamType, CharacterDT tCharacterDT, TileNode tTileNode, float fHeight = 1, bool bUpdatePos = true) {
        base.f_Init(iId, tBaseActionController, tTeamType, tCharacterDT, tTileNode, fHeight, bUpdatePos = true);
        //if (f_CheckIsNoFind()) {
        //    isIgnore = true;
        //} else {
        //    isIgnore = false;
        //}
    }


    //跳過需要把模型放置在RoleModel上的步驟
    protected override Transform GetRoleModel()  {
        return transform;
    }

}
