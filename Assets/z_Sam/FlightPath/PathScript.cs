using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EN_Path
{
    CirclingPath,
    Attack,
}


public class PathScript : MonoBehaviour
{
    [HideInInspector]
    public MoveOnpathScript moveOnpathScript;

    [Header("路徑模式、大小")]
    public EN_Path en;
    public float SphereSize;
   
    [Header("是否顯示路徑")]
    public bool b1;
    public float Gizmos_Size = 0.3f;
    public Color Gizmos_Color;

    [Header("其它")]
    public GameObject Pathpoint;
    public List<Transform> path_objs = new List<Transform>();
    Transform[] theArray;

    void Start()
    {

        #if UNITY_EDITOR
        if (en == EN_Path.Attack)       //直線預設用青色顯示
            Gizmos_Color = new Color(0f, 1f, 1f, 1f);
        if (en == EN_Path.CirclingPath) //盤旋路線預設用紅色顯示
            Gizmos_Color = new Color(1f, 0f, 0f, 1f);
        #endif

        if (path_objs.Count == 0)
        {
            CreateNode();
        }
    }


    //產生路徑與節點 ==========================================================================
    public void CreateNode()
    {
        //建立直線 ------------------------------------------------------
        if (en == EN_Path.Attack){
            if (path_objs.Count > 0){
                ClearNode();
            }
            for (int i = 0; i < 3; i++){
                Pathpoint g1 = Instantiate(Pathpoint, transform.position, Quaternion.identity).GetComponent<Pathpoint>();
                g1.name = i.ToString();
                g1.transform.parent = transform.transform;
                g1.g1.transform.position = new Vector3(g1.transform.position.x + i * SphereSize, g1.transform.position.y, g1.transform.position.z);

            }
        }//Atttack

        //建立圓形線 ------------------------------------------------------
        if (en == EN_Path.CirclingPath){
            if (path_objs.Count > 0){
                ClearNode();
            }
            for (int i = 0; i < 72; i++){
                Pathpoint g1 = Instantiate(Pathpoint, transform.position, Quaternion.identity).GetComponent<Pathpoint>();
                g1.name = i.ToString();
                g1.g1.transform.position = new Vector3(-SphereSize, g1.transform.position.y, g1.transform.position.z);
                g1.transform.parent = transform;
                g1.transform.eulerAngles = new Vector3(0, i * 5, 0);
            }
        }//Cycle
    }

    //清除路徑與節點 ========================================================================
    public void ClearNode(){
        for (int i = 0; i < path_objs.Count; i++){
            Destroy(path_objs[i].gameObject);
        }
        path_objs.Clear();
    }


    //顯示路徑與節點 ========================================================================
    private void OnDrawGizmos()
    {
        if (!b1) return;

        Gizmos.color = Gizmos_Color;
        theArray = GetComponentsInChildren<Transform>();
        path_objs.Clear();
        foreach (Transform path_obj in theArray)
        {
            if (path_obj != this.transform)
            {
                path_objs.Add(path_obj);
            }
        }
        for (int i = 0; i < path_objs.Count; i++)
        {
            Vector3 position = path_objs[i].position;
            if (i == 0)
            {
                Gizmos.color = Color.yellow; //起點使用黃色顯示
                Vector3 previous = path_objs[0].position;
                Gizmos.DrawCube(position, Vector3.one * Gizmos_Size * 2);
            }
            else if (i > 0)
            {
                Gizmos.color = Gizmos_Color;
                Vector3 previous = path_objs[i - 1].position;
                Gizmos.DrawLine(previous, position);
                Gizmos.DrawSphere(position, Gizmos_Size);
            }
        }
    }
}
