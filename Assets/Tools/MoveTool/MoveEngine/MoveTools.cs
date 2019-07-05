using System.Collections.Generic;
using UnityEngine;

class TargetObj
{
    public BaseRoleControllV2 tarObj;
    public float t;
    public float step;
    public MathClasses.DelGetPoint GetPoint, GetVelocity;
    public MoveDinfs.FinishEvent UpdateCallback;
    public MoveDinfs.FinishEvent FinishEvent;
}

public class MoveTools : MonoBehaviour
{
    private bool _bRun = false;
    TargetObj tarObjs;

    const float angle = 4f;
    Quaternion q = new Quaternion(0, 0, 0, Mathf.Cos(angle));
    float sin = Mathf.Sin(angle);

    #region 移动相关
    /// <summary>
    /// 根据移动类型计算出二边间移动需要的时间
    /// </summary>
    /// <param name="tEM_MoveType"></param>
    /// <param name="StartPos"></param>
    /// <param name="EndPos"></param>
    /// <returns></returns>
    public static float f_CalcMoveTime(BaseRoleControllV2 tBaseRoleControl, Vector3 endPos, Vector3 endDir = default(Vector3), float speed = 15)
    {
        if (tBaseRoleControl == null || speed <= 0) return -1;

        if (endDir == default(Vector3))
        {
            endDir = endPos - tBaseRoleControl.transform.position;
        }

        MoveTools tMoveTools = tBaseRoleControl.gameObject.GetComponent<MoveTools>();
        if (tMoveTools == null)
        {
            tMoveTools = tBaseRoleControl.gameObject.AddComponent<MoveTools>();
        }

        MathClasses.Bezier curve = new MathClasses.Bezier();
        curve.AssignPoints(tMoveTools.AutoBezierPoints(tBaseRoleControl.transform.position, endPos, tBaseRoleControl.transform.forward, endDir));

        return tMoveTools.CalcFinishTime(speed, curve.GetLength());
    }

    /// <summary>
    /// 移動一個指定的物件
    /// </summary>
    /// <param name="targetObj">需要被移動的物件</param>
    /// <param name="endPos">目標點</param>
    /// <param name="finishSec">指定的完成路徑時間</param>
    /// <param name="endDir">到達目標時要面向的方向</param>
    /// <param name="updateCallback">移動期間會不斷的呼叫這個函數</param>
    /// <param name="finishCallback">完成路徑後的 Callback 函數</param>
    public static void f_Moving(BaseRoleControllV2 tBaseRoleControl, Vector3 endPos, float finishSec, Vector3 endDir = default(Vector3),
        MoveDinfs.FinishEvent updateCallback = null, MoveDinfs.FinishEvent finishCallback = null)
    {

        if (tBaseRoleControl == null) return;


        if (endDir == default(Vector3))
        {
            endDir = endPos - tBaseRoleControl.transform.position;
        }

        MoveTools tMoveTools = tBaseRoleControl.gameObject.GetComponent<MoveTools>();
        if (tMoveTools == null)
        {
            tMoveTools = tBaseRoleControl.gameObject.AddComponent<MoveTools>();
        }

        MathClasses.Bezier curve = new MathClasses.Bezier();
        curve.AssignPoints(tMoveTools.AutoBezierPoints(tBaseRoleControl.transform.position, endPos, tBaseRoleControl.transform.forward, endDir));

        if (finishSec <= 0)
        {                           // 指定的時間 <= 0 直接移動到終點
            tBaseRoleControl.transform.position = curve.GetPoint(1);
            tBaseRoleControl.transform.forward = curve.GetVelocity(1);
            return;
        }

        TargetObj tObj = new TargetObj
        {
            tarObj = tBaseRoleControl,
            t = 0,
            step = tMoveTools.CalcMoveStep(finishSec),
            GetPoint = curve.GetPoint,
            GetVelocity = curve.GetVelocity,
            UpdateCallback = updateCallback,
            FinishEvent = finishCallback
        };

        tMoveTools.tarObjs = tObj;

        tMoveTools._bRun = true;

    }
    /// <summary>
    /// 立即停止物件移動
    /// </summary>
    /// <param name="targetObj">需要被停止的物件</param>
    /// <param name="callFinishEvent">停止後是否要呼叫之前註冊的完成事件</param>
    public static void f_StopMoving(Transform targetObj, bool callFinishEvent = false)
    {
        MoveTools tMoveTools = targetObj.GetComponent<MoveTools>();
        if (tMoveTools == null)
        {
            tMoveTools = targetObj.gameObject.AddComponent<MoveTools>();
        }
        tMoveTools._bRun = false;
        //      if (!tMoveTools.indexDict.ContainsKey(targetObj)) return;

        //int index = tMoveTools.indexDict[targetObj];

        //if (callFinishEvent) {
        //	if (tMoveTools.tarObjs[index].FinishEvent != null) tMoveTools.tarObjs[index].FinishEvent();
        //}
        //      tMoveTools.tarObjs.RemoveAt(index);

        //Debug.LogError("MoveTool22: " + tMoveTools.gameObject.name + " t: " + tMoveTools.tarObjs.t);
        tMoveTools.tarObjs = null;
        //tMoveTools.tarObjs.Clear();
    }
    #endregion

    #region ============== 內部使用函數 ===============
    private void FixedUpdate()
    {
        if (!_bRun)
        {
            return;
        }

        if (tarObjs == null) return;

        float t = tarObjs.t + tarObjs.step;
        if (t > 1) t = 1;
        tarObjs.t = t;

        if (tarObjs.GetPoint != null) tarObjs.tarObj.transform.position = tarObjs.GetPoint(t);
        if (tarObjs.GetVelocity != null) tarObjs.tarObj.transform.forward = tarObjs.GetVelocity(t);
        if (tarObjs.UpdateCallback != null) tarObjs.UpdateCallback();

        // 扺達
        if (t >= 1)
        {
            try
            {
                MoveDinfs.FinishEvent f = tarObjs.FinishEvent;
                tarObjs = null;
                f(); // 呼叫 CallBack 函數
            }
            catch { }
        }
    }

    float CalcMoveStep(float finishSec)
    {
        if (finishSec <= 0) return 1;

        return Time.fixedDeltaTime / finishSec;
    }

    float CalcFinishTime(float speed, float curveLen)
    {
        if (speed <= 0) return 0;
        return curveLen / speed;
    }

    List<Vector3> AutoBezierPoints(Vector3 startPos, Vector3 endPos, Vector3 startDir, Vector3 targetDir)
    {
        Vector3 distVector = endPos - startPos;                     // Calculate distance between the two
        float factor = distVector.magnitude;
        distVector /= factor;                                       // Normalize distance vector
        factor /= 1.7f;

        Vector3[] pos = new Vector3[]                              // Two Ends
			{ startPos, endPos };
        Vector3[] dirs = new Vector3[]                              // Two Directions
			{ startDir.normalized, targetDir.normalized };
        Vector3[] ctrlP = new Vector3[]                             // Two Direction Control Points
			{startPos + dirs[0] * factor,  endPos + dirs[1] * -factor};

        List<Vector3> ctrlPoints = new List<Vector3>();

        #region ========================== Assign Points for this bezier curve ======================
        float meanY = (startPos.y + endPos.y) * 0.5f;
        float dot;
        ctrlPoints.Add(startPos);                                      // --- 1. Assign Start Point
        ctrlPoints.Add(ctrlP[0]);                                   // --- 2. Assign Start Direction Point

        for (int i = 0; i < 2; i++)
        {
            dot = Vector3.Dot(distVector, dirs[i]);
            if (dot >= -1f && dot < -0.1f)
            {
                //Vector3 v = new Vector3(dirs[i].z, 0, -dirs[i].x);      // 與 myDir 垂直的向量
                Vector3 v = Vector3.Cross(distVector, dirs[i]).normalized * sin;
                q.x = v.x;
                q.y = v.y;
                q.z = v.z;
                v = q * dirs[i];

                if ((i % 2) == 0) v = -v;

                //dot = Vector3.Dot(v, distVector);
                //print(i+ ": " +  dot);
                //if ((i % 2) == 1) dot = -dot;
                //print(i + ": " + dot);
                //if (dot < -0.1f) v = -v;
                //if (Vector3.Dot(-v, distVector) > dot) {
                //	v = -v;
                //}
                v = pos[i] + v * factor * 2f;
                v.y = meanY;
                ctrlPoints.Add(v);              // --- 3 4. Assign Start Direction Adjust Points
            }
        }

        ctrlPoints.Add(ctrlP[1]);                                      // --- 5. Assign End Direction Point
        ctrlPoints.Add(endPos);                              // --- 6. Assign End Direction Adjust Point
        #endregion ====================================================================================
        return ctrlPoints;
    }
    #endregion ============================================
}
