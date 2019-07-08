using UnityEngine;
using System.Collections;

public class ParabolaTest : MonoBehaviour
{

    public float ShotSpeed = 10; // 抛出的速度
    public Transform pointA;     // 起点
    public Transform pointB;     // 终点

    private float g = -9.8f;     // 重力加速度
    private float time;          // A-B的时间
    private Vector3 speed;       // 初速度向量
    private Vector3 Gravity;     // 重力向量

    private LineRenderer lineRenderer;
    public Material lineMat;
    public float startWidth = 0.21f;
    public float endWidth = 0.21f;
    public Color lineColor = Color.green;
    public int lineVertex = 20;

    [Tooltip("拋物線的初始速度 (Local space)")]
    public Vector3 InitialVelocity = Vector3.forward * 10f;
    [Tooltip("拋物線 WorldSpace中的加速度，這會影響曲線的衰減。")]
    public Vector3 Acceleration = Vector3.up * -9.8f;
    [Tooltip("拋物線上每個點之間的間距。")]
    public float PointSpacing = 0.5f;
    public Vector3 CurrentPointVector { get; private set; }
    public float CurrentParabolaAngleY { get; private set; }



    void Start(){
        InitLineRenderer();
    }


    //手動模擬測試用
    private void Update(){
        if (Input.GetKey(KeyCode.A)){
            //GenerateLineRenderer();
            Vector3 velocity = transform.TransformDirection(InitialVelocity);
            CalculateParabolicCurve( pointA.position, velocity, Acceleration);
        }
    }


    /// <summary>
    /// 初始化 LineRenderer參數
    /// </summary>
    void InitLineRenderer(){
        if (GetComponent<LineRenderer>()){
            lineRenderer = GetComponent<LineRenderer>();
        }
        else{
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.enabled = false;
        lineRenderer.alignment = LineAlignment.Local;
        lineRenderer.positionCount = lineVertex;
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
        lineRenderer.material = lineMat;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
    }


    /// <summary>
    /// 產生拋物線 (算拋物線上的點，然後用 LineRenderer連接)
    /// </summary>
    private void GenerateLineRenderer(){
        lineRenderer.positionCount = lineVertex;
        g = -2 * (pointA.position.y - pointB.position.y);
        Vector3[] linePoints = new Vector3[lineVertex + 1];
        for (int i = 0; i < lineVertex; i++){
            float x = pointA.position.x + (pointB.position.x - pointA.position.x) / lineVertex * i;
            float y = pointA.position.y + 0.5f * g * ((float)i / lineVertex) * ((float)i / lineVertex);
            float z = pointA.position.z + (pointB.position.z - pointA.position.z) / lineVertex * i;
            linePoints[i] = new Vector3(x, y, z);
        }
        lineRenderer.enabled = true;
        lineRenderer.SetPositions(linePoints);
    }




    #region 拋物線公式
    //拋物線運動方程式： y = p0 + v0*t + 1/2at^2
    private static float ParabolicCurve(float p0, float v0, float a, float t){
        return p0 + v0 * t + 0.5f * a * t * t;
    }

    // 拋物線運動方程式 (應用於3維)：
    private static Vector3 ParabolicCurve(Vector3 p0, Vector3 v0, Vector3 a, float t){
        Vector3 ret = new Vector3();
        for (int x = 0; x < 3; x++)
            ret[x] = ParabolicCurve(p0[x], v0[x], a[x], t);
        return ret;
    }

    //拋物運動方程式的導數
    private static float ParabolicCurveDeriv(float v0, float a, float t){
        return v0 + a * t;
    }

    //拋物運動方程式的導數 (應用於3維)
    private static Vector3 ParabolicCurveDeriv(Vector3 v0, Vector3 a, float t){
        Vector3 ret = new Vector3();
        for (int i = 0; i < 3; i++)
            ret[i] = ParabolicCurveDeriv(v0[i], a[i], t);
        return ret;
    }
    #endregion



    /// <summary>
    /// 取樣拋物線曲線的一堆點，直到你擊中gnd。 那時停止拋物線
    /// </summary>
    /// <param name="p0"> 拋物線的起點 </param>
    /// <param name="v0"> 拋物線的初始速度 </param>
    /// <param name="a" > 拋物線的初始加速度 </param>
    /// <param name="dist"  > 採樣點之間的距離 </param>
    private void CalculateParabolicCurve(Vector3 p0, Vector3 v0, Vector3 a){
        float t = 0;
        float dist = Vector3.Distance(pointA.position, pointB.position) / lineVertex;
        lineRenderer.positionCount = lineVertex;
        Vector3[] linePoints = new Vector3[lineVertex + 1];
        linePoints[0] = p0;
        linePoints[lineVertex] = pointB.position;
        for (int i = 1; i < lineVertex; i++){
            t += dist / ParabolicCurveDeriv(v0, a, t).magnitude;
            Vector3 next = ParabolicCurve(p0, v0, a, t);
            linePoints[i] = new Vector3(next.x, next.y, next.z);
        }
        lineRenderer.enabled = true;
        lineRenderer.SetPositions(linePoints);
    }



    // 當您無法依賴 Update()時，使用這個來自動更新 CurrentParabolaAngle
    public void ForceUpdateCurrentAngle(){
        Vector3 velocity = transform.TransformDirection(InitialVelocity);
        Vector3 d;
        CurrentParabolaAngleY = ClampInitialVelocity(ref velocity, out d);
        CurrentPointVector = d;
    }


    //牽制給定的速度矢量，使其不超過水平方向45度。
    //這樣做是為了更容易利用拋物線運動的最大距離（在45度角）。
    //返回參考XZ平面的角度
    private float ClampInitialVelocity(ref Vector3 velocity, out Vector3 velocity_normalized){

        Vector3 d = Vector3.Project(velocity, Vector3.up.normalized);       //將初始速度投影到XZ平面上。
        Vector3 velocity_fwd = velocity - d;                                //取得"forward"的方向
        float angle = Vector3.Angle(velocity_fwd, velocity);                //找到XZ平面和速度之間的角度
        Vector3 right = Vector3.Cross(Vector3.up, velocity_fwd);            //使用叉積計算角度的 積極性/消極性(positivity/negativity)
        if (Vector3.Dot(right, Vector3.Cross(velocity_fwd, velocity)) > 0){ //如果「forward和速度的交叉乘積」與右側的方向相同，則我們低於垂直方向
            angle *= -1;
        }

        // 如果角度大於45度，則牽制角度
        if (angle > 45) {
            velocity = Vector3.Slerp(velocity_fwd, velocity, 45f / angle);
            velocity /= velocity.magnitude;
            velocity_normalized = velocity;
            velocity *= InitialVelocity.magnitude;
            angle = 45;
        } else {
            velocity_normalized = velocity.normalized;
        }
        return angle;
    }


}
