using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 偵測類型
/// </summary>
public enum CheckType {
    Rect_Forward, //長方形(前方偵測)
    Rect_Center,  //長方形(中心偵測)
    Sector,       //扇形
    Sphere,       //圓形
    Ring,         //環形
}


public class Check_Cube : MonoBehaviour {

    public bool _result;            //偵測結果
    public CheckType _Type;         //偵測類型
    public Transform target;        //偵測目標
    public float viewDistance = 1;  //視野距離
    [Range(0, 360)]                 //角度限制在 0~360
    public float viewAngle = 45;    //視野角度 (扇形偵測時)
    public Color tmpColor = new Color(0.07f, 1.0f, 1.0f, 0.2f); //偵測範圍顏色
    private Color dirColor  = new Color(1, 1, 1, 0.3f);         //起點顏色
    private Color nullColor = new Color(1, 1, 1, 0.3f);         //環形中間顏色
    public bool AllowY = true;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (_Type == CheckType.Rect_Forward) {
            _result = FOV_Rect(transform, target.transform, viewDistance, viewAngle);
        }
        else if (_Type == CheckType.Rect_Center) {
            _result = FOV_RectCenter(transform, target.transform, viewDistance, viewAngle);
        }
        else if (_Type == CheckType.Sector) {
            _result = FOV_Sector(transform, target.transform, viewDistance, viewAngle);
        }
        else if (_Type == CheckType.Sphere) {
            _result = FOV_Sphere(transform, target.transform, viewDistance);
        }
        else if (_Type == CheckType.Ring) {
            _result = FOV_Ring(transform, target.transform, viewDistance, viewAngle);
        }


        if (AllowY) {
            if (target.transform.position.y > (transform.position.y + viewDistance)) {
                _result = false;
            } else if (target.transform.position.y < (transform.position.y - viewDistance)){
                _result = false;
            }
        }
    }


    /// <summary>
    /// 判斷目標是否在長方形內 (攻擊點前方的長方形範圍)
    /// </summary>
    /// <param name="attacker"     > 攻擊產生點 </param>
    /// <param name="target"       > 被攻擊的對象位置 </param>
    /// <param name="viewDistance" > 矩形前方範圍 </param>
    /// <param name="rightDistance"> 矩形寬度/2 </param>          
    public bool FOV_Rect(Transform attacker, Transform target, float viewDistance, float rightDistance) {
        Vector3 deltaA = target.position - attacker.position;      //攻擊點到目標的向量
        float forwardDotA = Vector3.Dot(attacker.forward, deltaA); //「攻擊點前方向量」與「攻擊點到目標的向量」的????，其值等於兩點距離，且含有方向資訊(以 attacker.forward 為主的訊息↓)
        if (forwardDotA > 0) {                                     //「Dot > 0」表示目標在攻擊點前方(夾角小於90)，「= 0」表示目標在左右邊(兩者垂直)， 「< 0」表示目標在後面((夾角大於90))
            if (forwardDotA <= viewDistance) {                     //??
                if (Mathf.Abs(Vector3.Dot(attacker.right, deltaA)) < rightDistance) { //Mathf.Abs 是絕對值，??
                    return true;
                }
            }
        }
        return false;
    }


    /// <summary>
    /// 判斷目標是否在長方形內 (以攻擊點為中心的長方形範圍)
    /// </summary>
    /// <param name="attacker"     > 攻擊產生點 </param>
    /// <param name="target"       > 被攻擊的對象位置 </param>
    /// <param name="viewDistance" > 矩形前方範圍 </param>
    /// <param name="rightDistance"> 矩形寬度/2 </param>          
    public bool FOV_RectCenter(Transform attacker, Transform target, float viewDistance, float rightDistance) {
        Vector3 deltaA = target.position - attacker.position;                 //攻擊點到目標的向量
        float forwardDotA = Mathf.Abs(Vector3.Dot(attacker.forward, deltaA)); //「攻擊點前方向量」與「攻擊點到目標的向量」的????，其值等於兩點距離，且含有方向資訊(以 attacker.forward 為主的訊息↓)
        float rightDotA = Mathf.Abs(Vector3.Dot(attacker.right, deltaA));     //「攻擊點右方向量」與「攻擊點到目標的向量」的????，其值等於兩點距離，且含有方向資訊(以 attacker.forward 為主的訊息↓)
        if (forwardDotA <= viewDistance) {
            if (rightDotA < rightDistance) {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 目標在左邊還是右邊 (這個未測試過)
    /// </summary>
    /// <param name="attacker"> 自己 </param>
    /// <param name="target"  > 目標位置 </param>
    public bool Check_InRight(Transform attacker, Transform target) {
        Vector3 deltaA = target.position - attacker.position; //攻擊點到目標的向量
        if (Vector3.Cross(attacker.forward, deltaA).y > 0) {  //正值為右方，負值為左方
            return true;
        }
        return false;
    }


    /// <summary>
    /// 判斷目標是否在扇形內
    /// </summary>
    /// <param name="attacker"    > 攻擊產生點 </param>
    /// <param name="target"      > 被攻擊的對象位置 </param>
    /// <param name="viewDistance"> 視野距離 </param>
    /// <param name="viewAngle"   > 視野角度 </param>
    public bool FOV_Sector(Transform attacker, Transform target, float viewDistance, float viewAngle) {
        Vector3 tmpPos = target.position;
        tmpPos.y = attacker.position.y;
        if (Vector3.Distance(attacker.position, tmpPos) <= viewDistance) { //計算目標是否在視野距離內
            Vector3 _Dir = (tmpPos - attacker.position).normalized;        //到目標物的朝向
            if (Vector3.Angle(attacker.forward, _Dir) < viewAngle / 2) {   //計算目標是否在視野角度內
                return true;                                               //角度內
            }
        }
        return false;
    }



    /// <summary>
    /// 判斷目標是否在圓形內
    /// </summary>
    /// <param name="attacker"    > 攻擊產生點 </param>
    /// <param name="target"      > 被攻擊的對象位置 </param>
    /// <param name="viewDistance"> 視野距離 </param>
    public bool FOV_Sphere(Transform attacker, Transform target, float viewDistance) {
        Vector3 tmpPos = target.position;
        if (!AllowY) {
            tmpPos.y = attacker.position.y;
        }
        if (Vector3.Distance(attacker.position, tmpPos) <= viewDistance) { //計算目標是否在視野距離內
            return true;
        }
        return false;
    }


    /// <summary>
    /// 判斷目標是否在圓環內
    /// </summary>
    /// <param name="attacker"    > 攻擊產生點 </param>
    /// <param name="target"      > 被攻擊的對象位置 </param>
    /// <param name="viewDistance"> 視野最外圍距離   </param>
    /// <param name="viewAngle"   > 視野中心死角距離 </param>
    public bool FOV_Ring(Transform attacker, Transform target, float viewDistance, float viewAngle) {
        Vector3 tmpPos = target.position;
        if (!AllowY) {
            tmpPos.y = attacker.position.y;
        }
        if (Vector3.Distance(attacker.position, tmpPos) <= viewDistance) {  //計算目標是否在視野距離內
            if (Vector3.Distance(attacker.position, tmpPos) >= viewAngle) { //計算目標是否在中心死角外
                return true;
            }
        }
        return false;
    }



    /// <summary>
    /// 判斷目標是否在橢圓內 (沒測試過)
    /// </summary>
    /// <param name="fR"  >椭圆的最大半径(实际就是将圆沿y轴压扁变为椭圆的源圆的半径 R = 119)</param>
    /// <param name="fSy" >将圆沿y轴压扁变为椭圆时候的比例(长119 高68 则比例为 68/119 = 0.75)</param>
    public bool FOV_Ellipse(Transform attacker, Transform target, float fR, float fSy) {
        Vector2 targetPos = new Vector2(target.position.x, target.position.y);
        Vector2 attackerPos = new Vector2(attacker.position.x, attacker.position.y);
        Vector2 tPos = targetPos - attackerPos;
        tPos.y /= fSy;
        if (tPos.magnitude < fR) {
            return true;
        }
        return false;
    }


#if UNITY_EDITOR
    /// <summary>
    /// 顯示範圍
    /// </summary>
    private void OnDrawGizmos() {
        if (!enabled) {
            return;
        }
        if (_Type == CheckType.Rect_Forward) {
            GizmosRectangle();
        }
        else if (_Type == CheckType.Rect_Center) {
            GizmosRectCenter();
        }
        else if (_Type == CheckType.Sector) {
            GizmosSector();
        }
        else if (_Type == CheckType.Sphere) {
            GizmosSphere();
        }
        else if (_Type == CheckType.Ring) {
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
    /// 畫長方形 (描繪於前方)
    /// </summary>
    void GizmosRectangle() {
        Vector3 posR = transform.right * viewAngle;
        Vector3 posF = transform.forward * viewDistance;
        Vector3[] verts = new Vector3[4]{
            transform.position + posR,
            transform.position + posR + posF,
            transform.position + (posR * -1) + posF,
            transform.position + (posR * -1),
        };
        UnityEditor.Handles.color = tmpColor;
        UnityEditor.Handles.DrawSolidRectangleWithOutline(verts, tmpColor, tmpColor);

        if (AllowY){
            Vector3[] vertsUp = new Vector3[4] { verts[0], verts[1], verts[2], verts[3] };
            for (int i = 0; i < 4; i++) { vertsUp[i].y += viewDistance; }
            UnityEditor.Handles.DrawSolidRectangleWithOutline(vertsUp, tmpColor, tmpColor);
            Vector3[] vertsBottom = new Vector3[4] { verts[0], verts[1], verts[2], verts[3] };
            for (int i = 0; i < 4; i++) { vertsBottom[i].y -= viewDistance; }
            UnityEditor.Handles.DrawSolidRectangleWithOutline(vertsBottom, tmpColor, tmpColor);
            UnityEditor.Handles.DrawLine(vertsUp[0], vertsBottom[0]);
            UnityEditor.Handles.DrawLine(vertsUp[1], vertsBottom[1]);
            UnityEditor.Handles.DrawLine(vertsUp[2], vertsBottom[2]);
            UnityEditor.Handles.DrawLine(vertsUp[3], vertsBottom[3]);
        }

    }


    /// <summary>
    /// 畫長方形 (以中心描繪)
    /// </summary>
    void GizmosRectCenter() {
        Vector3 posR = transform.right   * viewAngle;
        Vector3 posF = transform.forward * viewDistance;
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
            for (int i = 0; i < 4; i++) { vertsUp[i].y += viewDistance; }
            UnityEditor.Handles.DrawSolidRectangleWithOutline(vertsUp, tmpColor, tmpColor);
            Vector3[] vertsBottom = new Vector3[4] { verts[0], verts[1], verts[2], verts[3] };
            for (int i = 0; i < 4; i++) { vertsBottom[i].y -= viewDistance; }
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
        UnityEditor.Handles.DrawSolidArc(this.transform.position, Vector3.up * 0.01f, rotatedForward, viewAngle, viewDistance);

        if (AllowY) {
            Vector3 upPos     = new Vector3(transform.position.x, transform.position.y + viewDistance, transform.position.z);
            Vector3 bottomPos = new Vector3(transform.position.x, transform.position.y - viewDistance, transform.position.z);
            UnityEditor.Handles.DrawSolidArc(upPos,     Vector3.up * 0.01f, rotatedForward, viewAngle, viewDistance);
            UnityEditor.Handles.DrawSolidArc(bottomPos, Vector3.up * 0.01f, rotatedForward, viewAngle, viewDistance);
            UnityEditor.Handles.DrawWireArc(upPos     - new Vector3(0, viewDistance / 2, 0), Vector3.up * 0.01f, rotatedForward, viewAngle, viewDistance);
            UnityEditor.Handles.DrawWireArc(bottomPos + new Vector3(0, viewDistance / 2, 0), Vector3.up * 0.01f, rotatedForward, viewAngle, viewDistance);
            UnityEditor.Handles.DrawLine(upPos, bottomPos);
            UnityEditor.Handles.DrawLine(upPos + (transform.forward * viewDistance), bottomPos + (transform.forward * viewDistance));
        }
    }


    /// <summary>
    /// 畫圓形
    /// </summary>
    void GizmosSphere() {
        UnityEditor.Handles.color = tmpColor;
        UnityEditor.Handles.DrawSolidDisc(transform.position + new Vector3(0, 0.05f, 0), Vector3.up, viewDistance);

        if (AllowY) {
            UnityEditor.Handles.DrawWireDisc(transform.position + new Vector3(0, 0.05f, 0), Vector3.up, viewDistance);
            UnityEditor.Handles.DrawWireDisc(transform.position + new Vector3(0, 0.05f, 0), Vector3.right, viewDistance);
            UnityEditor.Handles.DrawWireDisc(transform.position + new Vector3(0, 0.05f, 0), Vector3.forward, viewDistance);
            UnityEditor.Handles.DrawWireDisc(transform.position + new Vector3(0, 0.05f, 0), (transform.forward - transform.right).normalized, viewDistance);
            UnityEditor.Handles.DrawWireDisc(transform.position + new Vector3(0, 0.05f, 0), (transform.forward + transform.right).normalized, viewDistance);
            UnityEditor.Handles.DrawLine(transform.position + transform.up, transform.position - transform.up);
        }
       
    }


    /// <summary>
    /// 畫環形
    /// </summary>
    void GizmosRing() {
        Vector3 rotatedForward = Quaternion.Euler(0, -viewAngle * 0.5f, 0) * this.transform.forward;
        UnityEditor.Handles.color = tmpColor;
        UnityEditor.Handles.DrawSolidArc(transform.position + new Vector3(0, 0.05f, 0), Vector3.up, Vector3.forward, 360, viewDistance);
        UnityEditor.Handles.color = nullColor;
        UnityEditor.Handles.DrawSolidArc(transform.position + new Vector3(0, 0.05f, 0), Vector3.up, Vector3.forward, 360, viewAngle);
    }




#endif





}
