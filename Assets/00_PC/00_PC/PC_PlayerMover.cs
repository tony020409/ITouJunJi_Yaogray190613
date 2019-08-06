using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;


public class PC_PlayerMover : MonoBehaviour {

	Rigidbody rb;

    /// <summary> 玩家物件(Transform) </summary>  
    [Rename("放入玩家節點 (Player[CameraRig])")] public Transform player;
    /// <summary> 玩家攝影機 </summary> 
    [Rename("放入玩家攝影機 (Camera(eye))")] public Transform m_Camera;
    /// <summary> 播音效用的 </summary> 
    [Rename("AudioSource")] public AudioSource _audioSource;

    [Line()]
    /// <summary> 使用 Rigidbody 的移動方式 </summary> 
    [Rename("使用 Rigidbody 的移動方式")] public bool useRigidbodyMove = false;
    /// <summary> 玩家當前速度 </summary> 
    [ReadOnly("玩家當前速度 (Rigidbody)")] public float currentSpeed;
    /// <summary> 玩家最大速度 </summary> 
    [Rename("玩家最大速度")] public int maxSpeed = 5;
    /// <summary> 玩家加速度 </summary> 
    [Rename("玩家加速度")] public float accelerationSpeed = 2000.0f;
    /// <summary> 玩家減速度 </summary> 
    [Rename("玩家減速度")] public float deaccelerationSpeed = 20.0f;
    /// <summary> 水平運動方向 </summary>
    private Vector2 horizontalMovement;


    [Line()]
    /// <summary> 跑步中 </summary> 
    [ReadOnly("跑步中")] public bool isRunning = false;
    /// <summary> 當前速度 </summary> 
    [ReadOnly("當前速度")] public float moveSpeed = 0f;
    /// <summary> 走路速度 </summary> 
    [Rename("[走路速度]")] public float walkSpeed = 1.5f;
    /// <summary> 跑步速度 </summary> 
    [Rename("[跑步速度]")] public float runSpeed = 4.5f;


    [Line()]
    /// <summary> 移動時的晃動強度 </summary> 
    [Rename("移動時的晃動強度")] public float shakeForce = 0.15f;
    /// <summary> 當前腳步聲頻率 </summary> 
    [ReadOnly("當前腳步聲頻率")] public float footstepTime = 0.4f;
    /// <summary> 當前腳步聲頻率 </summary> 
    [Rename("[走路頻率]")] public float footstepTime_Walk = 0.4f;
    /// <summary> 當前腳步聲頻率 </summary> 
    [Rename("[跑步頻率]")] public float footstepTime_Run = 0.15f;
    /// <summary> 腳步聲 </summary> 
    [Rename("[腳步聲]")] public AudioEvent footstepAudio;
    /// <summary> 當下是否正在走路晃動中 </summary>
    private bool Shaking = false;


    [Line()]
    /// <summary> 玩家在地面 </summary> 
    [ReadOnly("玩家在地面")] public bool isGrounded;
    /// <summary> 玩家跳躍力道 </summary> 
    [Rename("[玩家跳躍力道]")] public float jumpForce = 300;
    /// <summary> 跳躍聲 </summary> 
    [Rename("[跳躍聲]")] public AudioEvent jumpAudio;
    /// <summary> ????? </summary>
    private Vector3 slowdownV;


    void Awake(){
        if (rb == null ) {
            rb = GetComponent<Rigidbody>();
        }
        if (rb != null) {
            rb.constraints = 
                RigidbodyConstraints.FreezeRotationX | 
                RigidbodyConstraints.FreezeRotationY |
                RigidbodyConstraints.FreezeRotationZ;
        }
        if (player == null) {
            player = this.transform;
        }
        if (_audioSource == null) {
            _audioSource = this.GetComponent<AudioSource>();
        }
        //預設狀態是走路
        moveSpeed = walkSpeed;
        footstepTime = footstepTime_Walk;
        isRunning = false;
    }


	void FixedUpdate(){
        if (useRigidbodyMove) {
            PlayerMovementLogic();
            FootstepAudio();
        }
	}


    void Update(){
        if (!useRigidbodyMove) {
            Move();
            Run();
            FootstepAudio();
        }
        Jumping ();
		Crouching();
    }


    /// <summary>
    /// Accordingly to input adds force and if magnitude is bigger it will clamp it.
    /// If player leaves keys it will deaccelerate
    /// </summary>
    void PlayerMovementLogic(){
		currentSpeed = rb.velocity.magnitude;
		horizontalMovement = new Vector2 (rb.velocity.x, rb.velocity.z);
		if (horizontalMovement.magnitude > maxSpeed){
			horizontalMovement = horizontalMovement.normalized;
			horizontalMovement *= maxSpeed;    
		}
		rb.velocity = new Vector3 (
			horizontalMovement.x,
			rb.velocity.y,
			horizontalMovement.y
		);

		if (isGrounded){
			rb.velocity = Vector3.SmoothDamp(
              rb.velocity,
				new Vector3(0,rb.velocity.y,0),
				ref slowdownV,
				deaccelerationSpeed);
		}

		if (isGrounded) {
			rb.AddRelativeForce (
                GetAxisHorizontal() * accelerationSpeed * Time.deltaTime, 
                0, 
                GetAxisVertical() * accelerationSpeed * Time.deltaTime);
		}
        else {
			rb.AddRelativeForce (
                GetAxisHorizontal() * accelerationSpeed * Time.deltaTime, 
                0, 
                GetAxisVertical() * accelerationSpeed * Time.deltaTime);
		}


        //此處修復了滑動問題
        if (GetAxisHorizontal() != 0 || GetAxisVertical() != 0) {
			deaccelerationSpeed = 0.5f;
		} else {
			deaccelerationSpeed = 0.1f;
		}
	}



    /// <summary> 記錄水平輸入值用 </summary>
    private float AxisHorizontal = 0;
    /// <summary> 即 Input.GetAxis ("Horizontal") 返回 -1 ~ 1 的值 </summary>
    private float GetAxisHorizontal() {
        if (Input.GetKey(KeyCode.D)) {
            AxisHorizontal = Mathf.Lerp(AxisHorizontal, 1, 1f);
            return AxisHorizontal;
        }
        else if (Input.GetKey(KeyCode.A)) {
            AxisHorizontal = Mathf.Lerp(AxisHorizontal, -1, 1f);
            return AxisHorizontal;
        }
        else {
            if (AxisHorizontal != 0) {
                AxisHorizontal = Mathf.Lerp(AxisHorizontal, 0, 3);
            }
            return AxisHorizontal;
        }
    }

    /// <summary> 記錄垂直輸入值用 </summary>
    private float AxisVertical = 0;
    /// <summary> 即 Input.GetAxis ("Vertical") 返回 -1 ~ 1 的值 </summary>
    private float GetAxisVertical() {
        if (Input.GetKey(KeyCode.W)) {
            AxisVertical = Mathf.Lerp(AxisVertical, 1, 0.01f);
            return AxisVertical;
        }
        else if (Input.GetKey(KeyCode.S)) {
            AxisVertical = Mathf.Lerp(AxisVertical, -1, 0.01f);
            return AxisVertical;
        }
        else {
            if (AxisVertical != 0) {
                AxisVertical = Mathf.Lerp(AxisVertical, 0, 3);
            }
            return AxisVertical;
        }
    }


    /// <summary> 移動 </summary>
    private void Move() {
        if (m_Camera == null) {
            return;
        }
        if (player == null) {
            return;
        }
        Vector3 dirVector = m_Camera.transform.forward;
        dirVector.y = 0;
        dirVector = dirVector.normalized * moveSpeed;

        // WASD 前進
        if (Input.GetKey(KeyCode.W)) {
            player.transform.position += dirVector * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A)){
            player.transform.position += (Quaternion.AngleAxis(-90, Vector3.up) * dirVector) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S)) {
            player.transform.position += (Quaternion.AngleAxis(180, Vector3.up) * dirVector) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D)) {
            player.transform.position += (Quaternion.AngleAxis(90, Vector3.up) * dirVector) * Time.deltaTime;
        }
    }


    /// <summary> 處理跑步 </summary>
    void Run() {    
        // Shift切換移動速度、腳步聲頻率
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            moveSpeed = runSpeed;
            footstepTime = footstepTime_Run;
            isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            moveSpeed = walkSpeed;
            footstepTime = footstepTime_Walk;
            isRunning = false;
        }
        //editor模式下支援即時變更走路速度
        #if UNITY_EDITOR
        if (!isRunning) {
            if (moveSpeed == walkSpeed) {
                return;
            }
            moveSpeed = walkSpeed;
            footstepTime = footstepTime_Walk;
            isRunning = false;
        }
        #endif
    }


    /// <summary> 處理走路晃動效果、腳步聲 </summary>
    void FootstepAudio() {
        if (Input.GetKey(KeyCode.W)) {
            CameraShake_Start();
        }
        if (Input.GetKey(KeyCode.A)){
            CameraShake_Start();
        }
        if (Input.GetKey(KeyCode.S)) {
            CameraShake_Start();
        }
        if (Input.GetKey(KeyCode.D)) {
            CameraShake_Start();
        }
    }



    /// <summary> 處理跳躍 </summary>
	void Jumping(){
		if (Input.GetKeyDown (KeyCode.Space) && isGrounded) {
			rb.AddRelativeForce (Vector3.up * jumpForce);
            if (jumpAudio != null) {
                jumpAudio.PlayOneShot(_audioSource);
            }
		}
	}


    /// <summary> 處理蹲下 </summary>
    void Crouching(){
        if (player == null) {
            return;
        }
		if(Input.GetKey(KeyCode.LeftControl)){
            player.transform.localScale = Vector3.Lerp(player.transform.localScale, new Vector3(1,0.6f,1), Time.deltaTime * 15);
		}
		else{
            player.transform.localScale = Vector3.Lerp(player.transform.localScale, new Vector3(1,1,1), Time.deltaTime * 15);
		}
	}



    /// <summary>
    /// 檢查玩家如果以小於 60度的角度接觸地面，則將 isGrounded 設為 true
    /// </summary>
	void OnCollisionStay(Collision other){
		foreach(ContactPoint contact in other.contacts){
            if (Vector2.Angle(contact.normal,Vector3.up) < 60){
				isGrounded = true;
			} 
		}
	}


    /// <summary>
    /// 玩家沒有碰撞到東西表示 離開地面
    /// </summary>
	void OnCollisionExit (){
		isGrounded = false;
	}

      
    /// <summary> 走路晃動效果向上 </summary>
    void CameraShake_Start() {
        if (Shaking) {
            return;
        }
        if (m_Camera == null) {
            return;
        }
        Shaking = true;
        m_Camera.DOLocalMoveY(m_Camera.localPosition.y + shakeForce, footstepTime).OnComplete(CameraShake_Down);
    }

    /// <summary> 走路晃動效果向下 </summary>
    void CameraShake_Down() {
        if (m_Camera == null){
            return;
        }
        //播放腳步聲
        if (footstepAudio != null) {
            if (isGrounded) {
                footstepAudio.PlayOneShot(_audioSource);
            }
        }
        m_Camera.DOLocalMoveY(m_Camera.localPosition.y - shakeForce, footstepTime).OnComplete(CameraShake_End);
    }

    /// <summary> 走路晃動效果著地 </summary>
    void CameraShake_End() {
        Shaking = false;
    }


}

