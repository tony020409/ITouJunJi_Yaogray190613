using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 範圍類型
/// </summary>
public enum RangeType {
    [Rename("正方體")] Cube,        //正方體
    [Rename("長方形")] Rect_Center, //長方形(中心偵測)
    [Rename("扇形")] Sector,        //扇形
    [Rename("圓形")] Sphere,        //圓形
    [Rename("環形")] Ring,          //環形
}



/// <summary>
/// 方便觀測範圍用的
/// </summary>
public class RangeInfo : MonoBehaviour {

    [Rename("範圍 (長度)")] public RangeType _Type = RangeType.Cube; //範圍類型
    [Rename("範圍 (長度)")] public float _fRange = 1;                //基本範圍大小
    [Range(0, 360)]                                          //視野角度 (限制在 0~360)
    [Rename("範圍 (角度或寬)")] public float viewAngle = 2;  //視野角度 (扇形、矩形用)

    [Rename("顯示顏色")]
    public  Color tmpColor  = new Color(0.07f, 1.0f, 1.0f, 0.2f); //基礎顏色
    private Color dirColor  = new Color(1, 1, 1, 0.3f);           //起點顏色
    private Color nullColor = new Color(1, 1, 1, 0.3f);           //環形中間顏色

    [Rename("顯示高度")]
    public bool AllowY = false;

    // Use this for initialization
    void Start() { }



    #if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (!enabled) {
            return;
        }
        if (_Type == RangeType.Cube) {
            GizmosCube();
        }
        else if (_Type == RangeType.Rect_Center) {
            GizmosRectCenter();
        }
        else if (_Type == RangeType.Sector) {
            GizmosSector();
        }
        else if (_Type == RangeType.Sphere) {
            GizmosSphere();
        }
        else if (_Type == RangeType.Ring) {
            GizmosRing();
        }
        GizmosCenter();
    }


    /// <summary>
    /// 畫原點和朝向
    /// </summary>
    void GizmosCenter() {
        Vector3 pos = transform.position;
        Gizmos.color = dirColor;
        Gizmos.DrawSphere(pos, 0.02f);
        UnityEditor.Handles.color = dirColor;
        UnityEditor.Handles.ArrowHandleCap(GUIUtility.GetControlID(FocusType.Passive), pos, transform.rotation, 0.1f, EventType.Repaint);
    }


    /// <summary>
    /// 畫長方形 (以中心描繪)
    /// </summary>
    void GizmosRectCenter() {
        Vector3 posR = transform.right * viewAngle;
        Vector3 posF = transform.forward * _fRange;
        Vector3[] verts = new Vector3[4]{
            transform.position + posR - posF,
            transform.position + posR + posF,
            transform.position + (posR * -1) + posF,
            transform.position + (posR * -1) - posF,
        };
        UnityEditor.Handles.color = tmpColor;
        UnityEditor.Handles.DrawSolidRectangleWithOutline(verts, tmpColor, tmpColor);

        if (AllowY) {
            Vector3[] vertsUp = new Vector3[4] { verts[0], verts[1], verts[2], verts[3] };
            for (int i = 0; i < 4; i++) { vertsUp[i].y += _fRange; }
            UnityEditor.Handles.DrawSolidRectangleWithOutline(vertsUp, tmpColor, tmpColor);
            Vector3[] vertsBottom = new Vector3[4] { verts[0], verts[1], verts[2], verts[3] };
            for (int i = 0; i < 4; i++) { vertsBottom[i].y -= _fRange; }
            UnityEditor.Handles.DrawSolidRectangleWithOutline(vertsBottom, tmpColor, tmpColor);
            UnityEditor.Handles.DrawLine(vertsUp[0], vertsBottom[0]);
            UnityEditor.Handles.DrawLine(vertsUp[1], vertsBottom[1]);
            UnityEditor.Handles.DrawLine(vertsUp[2], vertsBottom[2]);
            UnityEditor.Handles.DrawLine(vertsUp[3], vertsBottom[3]);
        }
    }



    /// <summary>
    /// 畫扇形
    /// </summary>
    void GizmosSector() {
        Vector3 rotatedForward = Quaternion.Euler(0, -viewAngle * 0.5f, 0) * this.transform.forward;
        UnityEditor.Handles.color = tmpColor;
        UnityEditor.Handles.DrawSolidArc(this.transform.position, Vector3.up * 0.01f, rotatedForward, viewAngle, _fRange);

        if (AllowY) {
            Vector3 upPos = new Vector3(transform.position.x, transform.position.y + _fRange, transform.position.z);
            Vector3 bottomPos = new Vector3(transform.position.x, transform.position.y - _fRange, transform.position.z);
            UnityEditor.Handles.DrawSolidArc(upPos, Vector3.up * 0.01f, rotatedForward, viewAngle, _fRange);
            UnityEditor.Handles.DrawSolidArc(bottomPos, Vector3.up * 0.01f, rotatedForward, viewAngle, _fRange);
            UnityEditor.Handles.DrawWireArc(upPos - new Vector3(0, _fRange / 2, 0), Vector3.up * 0.01f, rotatedForward, viewAngle, _fRange);
            UnityEditor.Handles.DrawWireArc(bottomPos + new Vector3(0, _fRange / 2, 0), Vector3.up * 0.01f, rotatedForward, viewAngle, _fRange);
            UnityEditor.Handles.DrawLine(upPos, bottomPos);
            UnityEditor.Handles.DrawLine(upPos + (transform.forward * _fRange), bottomPos + (transform.forward * _fRange));
        }
    }



    /// <summary>
    /// 畫圓形或球體
    /// </summary>
    void GizmosSphere() {
        UnityEditor.Handles.color = tmpColor;
        UnityEditor.Handles.DrawSolidDisc(transform.position + new Vector3(0, 0.05f, 0), Vector3.up, _fRange);

        if (AllowY) {
            UnityEditor.Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), transform.position, transform.rotation, _fRange*2, EventType.Repaint);
            //註解的地方是畫線
            //UnityEditor.Handles.DrawWireDisc(transform.position + new Vector3(0, 0.05f, 0), Vector3.up, _fRange);
            //UnityEditor.Handles.DrawWireDisc(transform.position + new Vector3(0, 0.05f, 0), Vector3.right, _fRange);
            //UnityEditor.Handles.DrawWireDisc(transform.position + new Vector3(0, 0.05f, 0), Vector3.forward, _fRange);
            //UnityEditor.Handles.DrawWireDisc(transform.position + new Vector3(0, 0.05f, 0), (transform.forward - transform.right).normalized, _fRange);
            //UnityEditor.Handles.DrawWireDisc(transform.position + new Vector3(0, 0.05f, 0), (transform.forward + transform.right).normalized, _fRange);
            //UnityEditor.Handles.DrawLine(transform.position + transform.up, transform.position - transform.up);
        }
    }

    /// <summary>
    /// 畫環形
    /// </summary>
    void GizmosRing() {
        Vector3 rotatedForward = Quaternion.Euler(0, -viewAngle * 0.5f, 0) * this.transform.forward;
        UnityEditor.Handles.color = tmpColor;
        UnityEditor.Handles.DrawSolidArc(transform.position + new Vector3(0, 0.05f, 0), Vector3.up, Vector3.forward, 360, _fRange);
        UnityEditor.Handles.color = nullColor;
        UnityEditor.Handles.DrawSolidArc(transform.position + new Vector3(0, 0.05f, 0), Vector3.up, Vector3.forward, 360, viewAngle);
    }


    /// <summary>
    /// 畫立方體
    /// </summary>
    void GizmosCube() {
        Gizmos.color = tmpColor;
        Gizmos.DrawCube(transform.position, Vector3.one * _fRange);
    }

    #endif


}
