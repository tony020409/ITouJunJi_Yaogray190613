/*  This file is part of the "Simple Waypoint System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;


namespace PathTool
{

    public class GetPath_and_Move : MonoBehaviour
    {

        #region 參數宣告 ================================================================
        //路徑本身
        [Header("當前路徑")]
        public Path_Stantard Path;

        [Header("起始節點")]
        public int startPoint = 0;      // Waypoint index where this object should start its path.
        [HideInInspector]
        public int currentPoint = 0;    // 當前節點

        [Header("狀態監控")]
        public bool onStart = false;    // Whether this object should start its movement at game launch.
        public bool reverse = false;    // Reverse the movement direction on the path, typically used for "pingPong" behavior.
        public bool moveToPath = false; // Whether this object should walk to the first waypoint or spawn there.

        [Header("移動選項")]
        public bool closeLoop = false;  // Option for closing the path on the "loop" looptype.
        public bool local = false;      // Whether local positioning should be used when tweening this object.
        public float lookAhead = 0;     // Value to look ahead on the path when orientToPath is enabled (0-1). (數字越小，移動轉向越自然的樣子，1表示不轉向)
        public float speed = 5;         // Speed or time value depending on the selected TimeValue type.

        [Header("選擇基於速度的移動 或時間( xx秒到達(?)")]
        public TimeValue timeValue = TimeValue.speed; 
        public enum TimeValue{
            time,
            speed
        }

        /// <summary>
        /// 獲取路徑的所有航點的引用。
        /// <summary>
        [HideInInspector]
        public Vector3[] waypoints;


        [Header("DoTween客製化項目")]
        /// <summary>
        /// Custom curve when AnimationCurve has been selected as easeType.
        /// <summary>
        public AnimationCurve animEaseType;

        /// <summary>
        /// Supported movement looptypes when moving on the path. 
        /// <summary>
        public LoopType loopType = LoopType.none;
        public enum LoopType{
            none,
            loop,
            pingPong,
            random,
            yoyo
        }


        /// <summary>
        /// List of Unity Events invoked when reaching waypoints.
        /// <summary>
        [HideInInspector]
        public List<UnityEvent> events = new List<UnityEvent>();
        public DG.Tweening.PathType pathType = DG.Tweening.PathType.CatmullRom; // Animation path type, linear or curved.
        public DG.Tweening.PathMode pathMode = DG.Tweening.PathMode.Full3D;     // Whether this object should orient itself to a different Unity axis.
        public DG.Tweening.Ease easeType = DG.Tweening.Ease.Linear;             // Animation easetype on TimeValue type time.
        public DG.Tweening.AxisConstraint lockPosition = DG.Tweening.AxisConstraint.None; // Option for locking a position axis.
        public DG.Tweening.AxisConstraint lockRotation = DG.Tweening.AxisConstraint.None; // Option for locking a rotation axis with orientToPath enabled.

        /// <summary>
        /// Whether to lerp this target from one waypoint rotation to the next,
        /// effectively overwriting the pathMode setting for all or one axis only. (純位移還是物件朝向要跟著轉?)
        /// </summary>
		public RotationType waypointRotation = RotationType.none;
        public enum RotationType{
            none,
            all
        }

        /// <summary>
        /// The target transform to rotate using waypoint rotation, if selected.
        /// This should be a child object with (0,0,0) rotation that gets overridden.
        /// </summary>
        public Transform rotationTarget;

        //---DOTween animation helper variables---
        [HideInInspector]
        public Tweener tween;
        private Vector3[] wpPos;      //array of modified waypoint positions for the tween
        private float originSpeed;    //original speed when changing the tween's speed
        private Quaternion originRot; //original rotation when rotating to first waypoint on moveToPath
        private System.Random rand = new System.Random(); //looptype random generator
        private int[] rndArray;                           //looptype random waypoint index array
        #endregion


        //check for automatic initialization
        void Start(){
            if (onStart)
                StartMove();
        }

        // 將 Start要執行的東西放在 StartMove()，使其可以從其他腳本調用、允許啟動延遲。
        public void StartMove(){
            
            //檢查路徑是否存在
            if (Path == null){
                Debug.LogWarning(gameObject.name + "沒有選擇路徑!");
                return;
            }

            waypoints = Path.GetPathPoints(local); //獲取所有航點的位置
            originSpeed = speed;                   //緩存原始速度 (如果之後有變動速度，這裡可以回復速度)
            originRot = transform.rotation;        //緩存原始朝向 (如果啟用了航點轉動，這裡可以回復朝向)

            //獲取起始節點
            startPoint = Mathf.Clamp(startPoint, 0, waypoints.Length - 1); //限制起始節點的編號在航點範圍內
            int index = startPoint;                                        //設定起始節點
            if (reverse){                                                  //如果是反向移動
                Array.Reverse(waypoints);
                index = waypoints.Length - 1 - index;
            }
            Initialize(index);

            Stop();
            CreateTween();
        }


        //initialize or update modified waypoint positions
        //fills array with original positions and adds custom height
        //check for message count and reinitialize if necessary
        private void Initialize(int startAt = 0){
            if (!moveToPath) startAt = 0;
            wpPos = new Vector3[waypoints.Length - startAt];
            for (int i = 0; i < wpPos.Length; i++)
                wpPos[i] = waypoints[i + startAt];

            //message count is smaller than waypoint count,
            //add empty message per waypoint
            for (int i = events.Count; i <= Path.GetWaypointCount() - 1; i++)
                events.Add(new UnityEvent());
        }


        //creates a new tween with given arguments that moves along the path
        private void CreateTween(){
            //prepare DOTween's parameters, you can look them up here
            //http://dotween.demigiant.com/documentation.php

            TweenParams parms = new TweenParams();
            //differ between speed or time based tweening
            if (timeValue == TimeValue.speed)
                parms.SetSpeedBased();
            if (loopType == LoopType.yoyo)
                parms.SetLoops(-1, DG.Tweening.LoopType.Yoyo);

            //apply ease type or animation curve
            if (easeType == DG.Tweening.Ease.Unset)
                parms.SetEase(animEaseType);
            else
                parms.SetEase(easeType);

            if (moveToPath)
                parms.OnWaypointChange(OnWaypointReached);
            else{
                //on looptype random initialize random order of waypoints
                if (loopType == LoopType.random)
                    RandomizeWaypoints();
                else if (loopType == LoopType.yoyo)
                    parms.OnStepComplete(ReachedEnd);

                Vector3 startPos = wpPos[0];
                if (local) startPos = Path.transform.TransformPoint(startPos);
                transform.position = startPos;

                parms.OnWaypointChange(OnWaypointChange);
                parms.OnComplete(ReachedEnd);
            }

            if (pathMode == DG.Tweening.PathMode.Ignore && waypointRotation != RotationType.none) {
                if (rotationTarget == null)
                    rotationTarget = transform;
                parms.OnUpdate(OnWaypointRotation);
            }

            if (local){
                tween = transform.DOLocalPath(wpPos, originSpeed, pathType, pathMode)
                                 .SetAs(parms)
                                 .SetOptions(closeLoop, lockPosition, lockRotation)
                                 .SetLookAt(lookAhead);
            }
            else{
                tween = transform.DOPath(wpPos, originSpeed, pathType, pathMode)
                                 .SetAs(parms)
                                 .SetOptions(closeLoop, lockPosition, lockRotation)
                                 .SetLookAt(lookAhead);
            }

            if (!moveToPath && startPoint > 0){
                GoToWaypoint(startPoint);
                startPoint = 0;
            }

            //continue new tween with adjusted speed if it was changed before
            if (originSpeed != speed)
                ChangeSpeed(speed);
        }


        #region 路徑設置 ==============================================================
        /// <summary>
        /// 更改此對象的當前路徑並開始移動。
        /// <summary>
        public void SetPath(Path_Stantard newPath){
            Stop();         //禁用任何運行的移動方法
            Path = newPath; //設定新路徑
            StartMove();    //重新開始移動
        }


        /// <summary>
        /// 禁用任何正在運行的移動協程。
        /// <summary>
        public void Stop(){
            StopAllCoroutines();
            if (tween != null)
                tween.Kill();
            tween = null;
        }


        /// <summary>
        /// 停止移動並重置到第一個路標。
        /// <summary>
        public void ResetToStart(){
            Stop();
            currentPoint = 0;
            if (Path){
                transform.position = Path.waypoints[currentPoint].position;
            }
        }
        #endregion


        // moveToPath完成時調用
        private void OnWaypointReached(int index){
            if (index <= 0)
                return;
            Stop();
            moveToPath = false;
            Initialize();
            CreateTween();
        }


        //called at every waypoint to invoke events
        private void OnWaypointChange(int index){
            index = Path.GetWaypointIndex(index);
            if (index == -1) return;
            if (loopType != LoopType.yoyo && reverse)
                index = waypoints.Length - 1 - index;
            if (loopType == LoopType.random)
                index = rndArray[index];

            currentPoint = index;

            if (events == null || events.Count - 1 < index || events[index] == null
                || loopType == LoopType.random && index == rndArray[rndArray.Length - 1])
                return;

            events[index].Invoke();
        }


        //EXPERIMENTAL
        //called on every tween update for lerping rotation between waypoints
        private void OnWaypointRotation(){ 
            int lookPoint = currentPoint;
            lookPoint = Mathf.Clamp(Path.GetWaypointIndex(currentPoint), 0, Path.GetWaypointCount());

            if (!tween.IsInitialized() || tween.IsComplete()){
                rotationTarget.rotation = Path.GetWaypoint(lookPoint).rotation;
                return;
            }

            TweenerCore<Vector3, Path, PathOptions> tweenPath = tween as TweenerCore<Vector3, Path, PathOptions>;
            float currentDist = tweenPath.PathLength() * tweenPath.ElapsedPercentage();
            float pathLength = 0f;
            float currentPerc = 0f;
            int targetPoint = currentPoint;

            if (moveToPath){
                pathLength = tweenPath.changeValue.wpLengths[1];
                currentPerc = currentDist / pathLength;
                rotationTarget.rotation = Quaternion.Lerp(originRot, Path.GetWaypoint(currentPoint).rotation, currentPerc);
                return;
            }

            if (Path is Path_Bezier) {
                 Path_Bezier bPath = Path as Path_Bezier;
                int curPoint = currentPoint;

                if (reverse){
                    targetPoint = bPath.GetWaypointCount() - 2 - (waypoints.Length - currentPoint - 1);
                    curPoint = (bPath.bPoints.Count - 2) - targetPoint;
                }

                int prevPoints = (int)(curPoint * bPath.pathDetail * 10);

                if (bPath.customDetail){
                    prevPoints = 0;
                    for (int i = 0; i < targetPoint; i++)
                        prevPoints += (int)(bPath.segmentDetail[i] * 10);
                }

                if (reverse){
                    for (int i = 0; i <= curPoint * 10; i++)
                        currentDist -= tweenPath.changeValue.wpLengths[i];
                }
                else{
                    for (int i = 0; i <= prevPoints; i++)
                        currentDist -= tweenPath.changeValue.wpLengths[i];
                }

                if (bPath.customDetail){
                    for (int i = prevPoints + 1; i <= prevPoints + bPath.segmentDetail[currentPoint] * 10; i++)
                        pathLength += tweenPath.changeValue.wpLengths[i];
                }
                else {
                    for (int i = prevPoints + 1; i <= prevPoints + 10; i++)
                        pathLength += tweenPath.changeValue.wpLengths[i];
                }
            }
            else{
                if (reverse) targetPoint = waypoints.Length - currentPoint - 1;

                for (int i = 0; i <= targetPoint; i++)
                    currentDist -= tweenPath.changeValue.wpLengths[i];

                pathLength = tweenPath.changeValue.wpLengths[targetPoint + 1];
            }

            currentPerc = currentDist / pathLength;
            if (Path is Path_Bezier){
                lookPoint = targetPoint;
                if (reverse) lookPoint++;
            }

            currentPerc = Mathf.Clamp01(currentPerc);
            rotationTarget.rotation = Quaternion.Lerp(Path.GetWaypoint(lookPoint).rotation, 
                Path.GetWaypoint(reverse ? lookPoint - 1 : lookPoint + 1).rotation, currentPerc);
        }


        private void ReachedEnd()
        {
            //each looptype has specific properties
            switch (loopType){
                //none means the tween ends here
                case LoopType.none:
                    return;

                //in a loop we start from the beginning
                case LoopType.loop:
                    currentPoint = 0;
                    CreateTween();
                    break;

                //reversing waypoints then moving again
                case LoopType.pingPong:
                    reverse = !reverse;
                    Array.Reverse(waypoints);
                    Initialize();

                    CreateTween();
                    break;

                //indicate backwards direction
                case LoopType.yoyo:
                    reverse = !reverse;
                    break;

                //randomize waypoints to new order
                case LoopType.random:
                    RandomizeWaypoints();
                    CreateTween();
                    break;
            }
        }



        private void RandomizeWaypoints(){
            Initialize();
            //create array with ongoing index numbers to keep them in mind,
            //this gets shuffled with all waypoint positions at the next step 
            rndArray = new int[wpPos.Length];
            for (int i = 0; i < rndArray.Length; i++){
                rndArray[i] = i;
            }

            //get total array length
            int n = wpPos.Length;
            //shuffle wpPos and rndArray
            while (n > 1){
                int k = rand.Next(n--);
                Vector3 temp = wpPos[n];
                wpPos[n] = wpPos[k];
                wpPos[k] = temp;

                int tmpI = rndArray[n];
                rndArray[n] = rndArray[k];
                rndArray[k] = tmpI;
            }

            //since all waypoints are shuffled the first waypoint does not
            //correspond with the actual current position, so we have to
            //swap the first waypoint with the actual waypoint.
            //start by caching the first waypoint position and number
            Vector3 first = wpPos[0];
            int rndFirst = rndArray[0];
            //loop through wpPos array and find corresponding waypoint
            for (int i = 0; i < wpPos.Length; i++){
                //currentPoint is equal to this waypoint number
                if (rndArray[i] == currentPoint){
                    //swap rnd index number and waypoint positions
                    rndArray[i] = rndFirst;
                    wpPos[0] = wpPos[i];
                    wpPos[i] = first;
                }
            }
            //set current rnd index number to the actual current point
            rndArray[0] = currentPoint;
        }


        /// <summary>
        /// Teleports to the defined waypoint index on the path.
        /// </summary>
        public void GoToWaypoint(int index){
            if (tween == null)
                return;

            if (reverse) index = waypoints.Length - 1 - index;

            tween.ForceInit();
            tween.GotoWaypoint(index, true);
        }


        /// <summary>
        /// Pauses the current movement routine for a defined amount of time.
        /// <summary>
        public void Pause(float seconds = 0f){
            StopCoroutine(Wait());
            if (tween != null)
                tween.Pause();

            if (seconds > 0)
                StartCoroutine(Wait(seconds));
        }
        //waiting routine
        private IEnumerator Wait(float secs = 0f){
            yield return new WaitForSeconds(secs);
            Resume();
        }
        /// <summary>
        /// 恢復當前暫停的移動程序。
        /// <summary>
        public void Resume(){
            StopCoroutine(Wait());
            if (tween != null)
                tween.Play();
        }


        /// <summary>
        /// 任何時候都可以反向移動。
        /// <summary>
        public void Reverse(){
            //inverse direction toggle
            reverse = !reverse;
            //calculate opposite remaining path time i.e. if we're at 80% progress in one direction,
            //this then returns 20% time value when starting from the opposite direction and so on
            float timeRemaining = 0f;
            if (tween != null)
                timeRemaining = 1 - tween.ElapsedPercentage(false);

            //invert starting point from current waypoint
            startPoint = waypoints.Length - 1 - currentPoint;
            StartMove();
            tween.ForceInit();
            //set moving object to the reversed time progress
            tween.fullPosition = tween.Duration(false) * timeRemaining;
        }



        /// <summary>
        /// 變更速度
        /// <summary>
        public void ChangeSpeed(float value){
            //calulate new timeScale value based on original speed
            float newValue;
            if (timeValue == TimeValue.speed)
                newValue = value / originSpeed;
            else
                newValue = originSpeed / value;

            //set speed, change timeScale percentually
            speed = value;
            if (tween != null)
                tween.timeScale = newValue;
        }
    }
}