using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info_Check : MonoBehaviour {


    [Rename("範圍")] public float _fRang;
    [Rename("隊伍")] public GameEM.TeamType _TeamType;
    [Rename("顯示顏色")] public Color tmpColor = new Color(0.07f, 1.0f, 1.0f, 0.2f);
    [Line()]
    [Rename("搜尋到的人數")] public int _iPeopleCount;
    [Rename("搜尋到的清單")] public List<BaseRoleControllV2> aData;

     #if UNITY_EDITOR
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        aData = BattleMain.GetInstance().m_BattleRolePool.f_FindTargetFriendAll2(_TeamType, transform.position, _fRang, true);
        _iPeopleCount = aData.Count;
    }


    /// <summary>
    /// 顯示躲避點偵測範圍
    /// </summary>
    private void OnDrawGizmos() {
        if (!enabled) {
            return;
        }
        UnityEditor.Handles.color = tmpColor;
        UnityEditor.Handles.DrawSolidArc(transform.position + new Vector3(0, 0.05f, 0), Vector3.up, Vector3.forward, 360, _fRang);
    }

    #endif
}
