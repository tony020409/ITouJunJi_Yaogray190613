using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;



public class ParabolicPointer : MonoBehaviour
{

    [Header("拋物線彈道")]
    [Tooltip("拋物線的初始速度 (Local space)")]
    public Vector3 InitialVelocity = Vector3.forward * 10f;
    [Tooltip("拋物線 WorldSpace中的加速度，這會影響曲線的衰減。")]
    public Vector3 Acceleration = Vector3.up * -9.8f;

    [Header("拋物線的 網格屬性(Mesh)")]
    [Tooltip("拋物線網格上的點數。 數值越高網格越平滑。")]
    public int PointCount = 10;
    [Tooltip("拋物線上每個點之間的間距。")]
    public float PointSpacing = 0.5f;
    [Tooltip("拋物線網的厚度")]
    public float GraphicThickness = 0.2f;
    [Tooltip("用於渲染拋物線網格的材質")]
    public Material GraphicMaterial;

    [Header("選擇 Pad 屬性")]
    [SerializeField]
    [Tooltip("當玩家指向能夠傳送的表面時，會出現的Prefab。")]
    private GameObject SelectionPadPrefab;
    [SerializeField]
    [Tooltip("當玩家指向能夠無法移動的表面時，會出現的Prefab。")]
    private GameObject InvalidPadPrefab;


    public Vector3 SelectedPoint { get; private set; }
    public Vector3 CurrentPointVector { get; private set; }
    public bool PointOnNavMesh { get; private set; }
    public float CurrentParabolaAngleY { get; private set; }

    private GameObject SelectionPadObject;
    private GameObject InvalidPadObject;

    private Mesh ParabolaMesh;
    private List<Vector3> ParabolaPoints;


    [Header("槍口")]
    public Transform GunStartPosition; //槍口

    [Header("Raycast偵測的Layer")]
    public LayerMask teleportMask; //允許瞄準的目標Layer層
    private RaycastHit hit;        //Raycast擊中的點


    void Start() {
        ParabolaPoints = new List<Vector3>(PointCount);
        ParabolaMesh = new Mesh();
        ParabolaMesh.MarkDynamic();
        ParabolaMesh.name = "Parabolic Pointer";
        ParabolaMesh.vertices = new Vector3[0];
        ParabolaMesh.triangles = new int[0];

        if (SelectionPadPrefab != null) {
            SelectionPadObject = Instantiate<GameObject>(SelectionPadPrefab);
            SelectionPadObject.SetActive(false);
        }

        if (InvalidPadPrefab != null) {
            InvalidPadObject = Instantiate<GameObject>(InvalidPadPrefab);
            InvalidPadObject.SetActive(false);
        }
    }




    #region 拋物線公式
    //拋物線運動方程式： y = p0 + v0*t + 1/2at^2
    private static float ParabolicCurve(float p0, float v0, float a, float t) {
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







    void Update()
    {
        // 1. 計算拋物線點數
        Vector3 velocity = transform.TransformDirection(InitialVelocity);
        Vector3 velocity_normalized;
        CurrentParabolaAngleY = ClampInitialVelocity(ref velocity, out velocity_normalized);
        CurrentPointVector = velocity_normalized;

        Vector3 normal;
        PointOnNavMesh = CalculateParabolicCurve(
            transform.position,
            velocity,
            Acceleration, PointSpacing, PointCount,
            ParabolaPoints,
            out normal);

        SelectedPoint = ParabolaPoints[ParabolaPoints.Count - 1];

        // 2. Render Parabola graphics
        if (SelectionPadObject != null) {
            SelectionPadObject.SetActive(PointOnNavMesh);
            SelectionPadObject.transform.position = SelectedPoint + Vector3.one * 0.005f;
            if (PointOnNavMesh) {
                SelectionPadObject.transform.rotation = Quaternion.LookRotation(normal);
                SelectionPadObject.transform.Rotate(90, 0, 0);
            }
        }
        if (InvalidPadObject != null) {
            InvalidPadObject.SetActive(!PointOnNavMesh);
            InvalidPadObject.transform.position = SelectedPoint + Vector3.one * 0.005f;
            if (!PointOnNavMesh)  {
                InvalidPadObject.transform.rotation = Quaternion.LookRotation(normal);
                InvalidPadObject.transform.Rotate(90, 0, 0);
            }
        }

        // Draw parabola (BEFORE the outside faces of the selection pad, to avoid depth issues)
        GenerateMesh(ref ParabolaMesh, ParabolaPoints, velocity, Time.time % 1);
        Graphics.DrawMesh(ParabolaMesh, Matrix4x4.identity, GraphicMaterial, gameObject.layer);
    }





    /// <summary>
    /// 取樣拋物線曲線的一堆點，直到你擊中gnd。 那時停止拋物線
    /// </summary>
    /// <param name="p0"> 拋物線的起點 </param>
    /// <param name="v0"> 拋物線的初始速度 </param>
    /// <param name="a" > 拋物線的初始加速度 </param>
    /// <param name="dist"  > 採樣點之間的距離 </param>
    /// <param name="points"> 採樣點的數量 </param>
    /// <param name="outPts"> 採樣點的列表 </param>
    /// <param name="normal"> 擊中點的法線 </param>
    private bool CalculateParabolicCurve(Vector3 p0, Vector3 v0, Vector3 a, float dist, int points, List<Vector3> outPts, out Vector3 normal)
    {
        outPts.Clear();
        outPts.Add(p0);

        Vector3 last = p0;
        float t = 0;

        for (int i = 0; i < points; i++)
        {
            t += dist / ParabolicCurveDeriv(v0, a, t).magnitude;
            Vector3 next = ParabolicCurve(p0, v0, a, t);

            Vector3 castHit;
            Vector3 norm;
            bool endOnNavmesh;

            //bool cast = nav.Linecast(last, next, out endOnNavmesh, out castHit, out norm);
            //if (cast)
            //{
            //    outPts.Add(castHit);
            //    normal = norm;
            //    return endOnNavmesh;
            //}
            //else {
            //    outPts.Add(next);
            //}
            //last = next;
        }

        normal = Vector3.up;
        return false;
    }


    /// <summary>
    /// 產生拋物線模型
    /// </summary>
    /// <param name="m"></param>
    /// <param name="points"></param>
    /// <param name="fwd"></param>
    /// <param name="uvoffset"></param>
    private void GenerateMesh(ref Mesh m, List<Vector3> points, Vector3 fwd, float uvoffset)
    {
        Vector3 right = Vector3.Cross(fwd, Vector3.up).normalized;
        Vector3[] verts = new Vector3[points.Count * 2];
        Vector2[] uv = new Vector2[points.Count * 2];
       
        for (int x = 0; x < points.Count; x++) {
            verts[2 * x] = points[x] - right * GraphicThickness / 2;
            verts[2 * x + 1] = points[x] + right * GraphicThickness / 2;

            float uvoffset_mod = uvoffset;
            if (x == points.Count - 1 && x > 1) {
                float dist_last = (points[x - 2] - points[x - 1]).magnitude;
                float dist_cur = (points[x] - points[x - 1]).magnitude;
                uvoffset_mod += 1 - dist_cur / dist_last;
            }

            uv[2 * x] = new Vector2(0, x - uvoffset_mod);
            uv[2 * x + 1] = new Vector2(1, x - uvoffset_mod);
        }

        int[] indices = new int[2 * 3 * (verts.Length - 2)];
        for (int x = 0; x < verts.Length / 2 - 1; x++) {
            int p1 = 2 * x;
            int p2 = 2 * x + 1;
            int p3 = 2 * x + 2;
            int p4 = 2 * x + 3;

            indices[12 * x] = p1;
            indices[12 * x + 1] = p2;
            indices[12 * x + 2] = p3;
            indices[12 * x + 3] = p3;
            indices[12 * x + 4] = p2;
            indices[12 * x + 5] = p4;

            indices[12 * x + 6] = p3;
            indices[12 * x + 7] = p2;
            indices[12 * x + 8] = p1;
            indices[12 * x + 9] = p4;
            indices[12 * x + 10] = p2;
            indices[12 * x + 11] = p3;
        }
        m.Clear();
        m.vertices = verts;
        m.uv = uv;
        m.triangles = indices;
        m.RecalculateBounds();
        m.RecalculateNormals();
    }



    //void OnDisable() {
    //    if (SelectionPadObject != null)
    //        SelectionPadObject.SetActive(false);
    //    if (InvalidPadObject != null)
    //        InvalidPadObject.SetActive(false);
    //}

  

    // 當您無法依賴 Update()時，使用這個來自動更新 CurrentParabolaAngle
    // (for example, directly after enabling the component)
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
        if (angle > 45){
            velocity = Vector3.Slerp(velocity_fwd, velocity, 45f / angle);
            velocity /= velocity.magnitude;
            velocity_normalized = velocity;
            velocity *= InitialVelocity.magnitude;
            angle = 45;
        }
        else {
            velocity_normalized = velocity.normalized;
        }
        return angle;
    }


    #if UNITY_EDITOR
    private List<Vector3> ParabolaPoints_Gizmo;
    void OnDrawGizmos() {
        if (Application.isPlaying) // Otherwise the parabola can show in the game view
            return;

        if (ParabolaPoints_Gizmo == null)
            ParabolaPoints_Gizmo = new List<Vector3>(PointCount);

        Vector3 velocity = transform.TransformDirection(InitialVelocity);
        Vector3 velocity_normalized;
        CurrentParabolaAngleY = ClampInitialVelocity(ref velocity, out velocity_normalized);

        Vector3 normal;
        bool didHit = CalculateParabolicCurve(
            transform.position,
            velocity,
            Acceleration, PointSpacing, PointCount,
            //NavMesh,
            ParabolaPoints_Gizmo, out normal);

        Gizmos.color = Color.blue;
        for (int x = 0; x < ParabolaPoints_Gizmo.Count - 1; x++)
            Gizmos.DrawLine(ParabolaPoints_Gizmo[x], ParabolaPoints_Gizmo[x + 1]);
        Gizmos.color = Color.green;

        if (didHit)
            Gizmos.DrawSphere(ParabolaPoints_Gizmo[ParabolaPoints_Gizmo.Count - 1], 0.2f);
    }
    #endif


}
