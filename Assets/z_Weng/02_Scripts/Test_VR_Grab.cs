using UnityEngine;

//手把要加上：碰撞體(IsTrigger要打勾)、Rigibody(IsKineMatic要打勾)
//要被抓取的物件要加上：碰撞體(IsTrigger不用勾)、Rigibody(IsKineMatic不用勾)

public class Test_VR_Grab : MonoBehaviour {

    /// <summary>
    /// 手部射線起點
    /// </summary>
    public Transform HandTrigger;


    /// <summary>
    /// 偵測長度
    /// </summary>
    public float rayLength;

    /// <summary>
    /// 射線偵測的Layer
    /// </summary>
    public LayerMask EnemyLayer;

    RaycastHit hit;
    Vector3 fwd;

    /// <summary>
    /// 抓住的東西
    /// </summary>
    private GameObject objectInHand;

    /// <summary>
    /// 當前碰到的東西
    /// </summary>
    public GameObject collidingObject;

    /// <summary>
    /// 手把控制器
    /// </summary>
    //private SteamVR_TrackedObject trackedObj;
    

    //private SteamVR_Controller.Device Controller{
    //    get { return SteamVR_Controller.Input((int)trackedObj.index); }
    //}



    void Awake(){
        //trackedObj = GetComponent<SteamVR_TrackedObject>();
    }


    void Update() {
        Debug.DrawRay(HandTrigger.position, fwd * rayLength, Color.green);

        //if (Controller.GetHairTriggerDown()) {
        //    //如果有碰到東西就抓起來
        //    if (collidingObject) {
        //        GrabObject();
        //    }
        //}
        //
        //if (Controller.GetHairTriggerUp()) {
        //    //如果手上有東西就放掉
        //    if (objectInHand) {
        //        ReleaseObject();
        //    }
        //}
    }


    private void FixedUpdate() {
        fwd = HandTrigger.TransformDirection(Vector3.forward);
        if (Physics.Raycast(HandTrigger.position, fwd, out hit, rayLength, EnemyLayer)){
            SetCollidingObject(hit); //設定抓到的東西
        }  else {
            if (!collidingObject) {
                return;
            }
            collidingObject = null; //表明沒碰到東西
        }
    }


    // 碰撞檢測
    //public void OnTriggerEnter(Collider other){
    //    SetCollidingObject(other); //設定抓到的東西
    //}
    //
    //public void OnTriggerStay(Collider other){
    //    SetCollidingObject(other); //設定抓到的東西
    //}
    //
    //public void OnTriggerExit(Collider other){
    //    if (!collidingObject){
    //        return;
    //    }
    //    collidingObject = null; //表明沒碰到東西
    //}


    /// <summary>
    /// 設置抓取的東西
    /// </summary>
    /// <param name="col"></param>
    private void SetCollidingObject(RaycastHit tmpHit){
        if (collidingObject || !tmpHit.collider.GetComponent<Rigidbody>()){
            return;
        }
        collidingObject = tmpHit.transform.gameObject;
    }

    /// <summary>
    /// 抓取物件
    /// </summary>
    private void GrabObject(){
        objectInHand = collidingObject;
        collidingObject = null;
        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.breakForce = 20000;
        joint.breakTorque = 20000;
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }


    /// <summary>
    /// 釋放物件
    /// </summary>
    private void ReleaseObject(){
        if (GetComponent<FixedJoint>()){
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            //objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity; //丟出去的力道
            //objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
        }
        objectInHand = null;
    }


}
