using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeNavigation_OtherPlayer : MonoBehaviour {

    [Rename("當前導航區域 (監測用)")]
    public NodeNavigation currentNode;

    [Rename("所有導航區域")]
    private List<NodeNavigation> nodeList = new List<NodeNavigation>();

    [Rename("箭頭物件")]
    public GameObject GuideObj;


    /// <summary>
    /// 原本的箭頭物件
    /// </summary>
    private GameObject _tObj;


    /// <summary>
    /// 因為在 Start()裡抓不到 BattleMain.GetInstance()，所以用一個布爾值去控制初始化
    /// </summary>
    private bool Init = false;


    // Update is called once per frame
    void Update () {

        //因為在 Start()裡抓不到 BattleMain.GetInstance()，所以用一個布爾值去抓
        if (!Init) {
            if (!StaticValue.m_bIsMaster) {
                nodeList = BattleMain.GetInstance().NodeNavigatioList;
            }
            Init = true;
            return;
        } 

        CheckNode();
    }


    /// <summary>
    /// 檢查導航區域
    /// </summary>
    void CheckNode() {
        for (int i=0; i<nodeList.Count; i++) {

            //空值排除
            if (nodeList[i] == null) {
                continue;
            }

            //檢查觸發的導航區域
            if (Vector3.Distance(transform.position, nodeList[i].transform.position) < nodeList[i].StartTriggerDistance) {
                currentNode = nodeList[i];

                //替換箭頭物件
                if (GuideObj != null) {
                    if (!StaticValue.m_bIsMaster) {
                        _tObj = currentNode.GuideObj;
                        currentNode.GuideObj = this.GuideObj;
                    }
                }

            }
        }


        //檢查是否離開導航區域
        if (currentNode != null) {
            if (Vector3.Distance(transform.position, currentNode.transform.position) > currentNode.StartTriggerDistance) {
                currentNode.GuideObj = _tObj;
                currentNode = null;
            }
        }

    }







}
