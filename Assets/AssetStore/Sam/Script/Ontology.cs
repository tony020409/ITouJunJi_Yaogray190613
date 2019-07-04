//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Ontology : MonoBehaviour {

//    public Player Aims;
//    public Missile MissilePrefab, m_Missile;
//    public float AimsDistance;

//    public float ExplosionTime, InstantiateTime, ActiveFalseTime1;

//    public Explosion ExplosionPrefab, m_Explosion;

//    public GameObject FailEffect;

//    public bool OnceTime;
//    public float ModifyHeight;
//    public string _SuccessAudioName;
//    public string _FailAudioName;
//    public bool _IsRight;
//    public bool _IsEnd;

//    void Start () {
//        Launcher._ins.initEvent += Init;

//        OnceTime = false;
//    }

//    void Update () {
//        if (Vector3.Distance(transform.position, Aims.lantern.transform.position) <= AimsDistance && m_Missile == null && m_Explosion == null && OnceTime == false) {
//            Debug.LogWarning("Ontology_碰到物品");

//            OnceTime = true;
//            if (_IsRight == true) {
//                Debug.LogWarning("Ontology_碰到對的物品");

//                Gameplay02Manager._ins.RPC_SuccessGameplay02(PhotonNetwork.playerName.Substring(0, 1), _IsEnd);
//            } else if (_IsRight == false) {
//                Debug.LogWarning("Ontology_碰到錯的物品");

//                Gameplay02Manager._ins.RPC_FailGameplay02(PhotonNetwork.playerName.Substring(0, 1));
//            }
//        }
//    }

//    void Init () {
//        OnceTime = false;
//    }

//    void OnDisable () {
//        OnceTime = false;
//    }

//    public void SuccessGameplay02 () {

//        //Debug.LogWarning(gameObject.name + _IsRight);

//        if (transform.parent.gameObject.activeSelf) {
//            if (_IsRight == false) {
//                Debug.LogWarning("Ontology_錯的物品執行事件");

//                StartCoroutine(FailEvent());
//            } else if (_IsRight == true) {
//                Debug.LogWarning("Ontology_對的物品執行事件");

//                AudioManager._ins.PlayAudioClip("Ontology", transform.position, _SuccessAudioName);
//                StartCoroutine(SuccessEvent());
//            }
//        }
//    }

//    public void FailGameplay02 () {
//        if (transform.parent.gameObject.activeSelf) {
//            AudioManager._ins.PlayAudioClip("Ontology", transform.position, _FailAudioName);
//            StartCoroutine(FailEvent());
//        }
//    }

//    IEnumerator SuccessEvent () {

//        Debug.LogWarning("Ontology_對的物品執行中");

//        Vector3 NewPos = transform.position;
//        NewPos.y += ModifyHeight;

//        yield return new WaitForSeconds(ExplosionTime);

//        m_Explosion = Instantiate(ExplosionPrefab, NewPos, transform.rotation);

//        yield return new WaitForSeconds(InstantiateTime);

//        transform.gameObject.SetActive(false);

//        if (!_IsEnd) {
//            //m_Missile = Instantiate(MissilePrefab, NewPos, Quaternion.Euler(transform.up));
//            m_Missile = Instantiate(MissilePrefab, NewPos, Quaternion.Euler(-90, 0, 0));
//            m_Missile.m_MissileStruct.m_Target = Aims.GetComponent<Player>().head;
//        }
//    }

//    IEnumerator FailEvent () {

//        Debug.LogWarning("Ontology_錯的物品執行中");

//        Vector3 NewPos = transform.position;
//        NewPos.y += ModifyHeight;

//        yield return new WaitForSeconds(ExplosionTime);

//        GameObject go = Instantiate(FailEffect, NewPos, transform.rotation);

//        yield return new WaitForSeconds(ActiveFalseTime1);

//        transform.gameObject.SetActive(false);

//    }

//}
