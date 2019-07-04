
using ccU3DEngine;
using UnityEngine;


public class DoorRoleControl : BaseRoleControllV2
{


    [Header("---------常用音效----------------------")]
    [Rename("開門音效")] public AudioClip Clip_Open;
    [Rename("關門音效")] public AudioClip Clip_Close;
    private AudioSource audioOne;  //播放聲音用

    //接收動畫事件用
    private ccCallback _CallBack_RecvAnimatorEvent = null;


    
    //初始化2 (物件池重複利用)
    public override void f_Init(int iId, BaseActionController tBaseActionController, GameEM.TeamType tTeamType, CharacterDT tCharacterDT, TileNode tTileNode, float fHeight = 1, bool bUpdatePos = true) {
        base.f_Init(iId, tBaseActionController, tTeamType, tCharacterDT, tTileNode, fHeight, bUpdatePos = true);
    }


    //跳過需要把模型放置在RoleModel上的步驟 ======================================
    protected override Transform GetRoleModel(){
        return transform;
    }



    //播放常用聲音 ===============================================================    
    public void SoundDoor(string ClipName)  {
        if (ClipName == "OPEN") {
            audioOne.PlayOneShot(Clip_Open);
        }
        else if (ClipName == "Open") {
            audioOne.PlayOneShot(Clip_Open);
        }
        else if (ClipName == "CLOSE") {
            audioOne.PlayOneShot(Clip_Close);
        }
        else if (ClipName == "Close") {
            audioOne.PlayOneShot(Clip_Close);
        }
    }


}
