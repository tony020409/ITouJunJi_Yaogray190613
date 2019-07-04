//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;



///// <summary>
///// 飛彈
///// </summary>
//public class Missile : Photon.PunBehaviour {

//    public MissileStruct m_MissileStruct;

//    public float AimsDistance;

//    public bool OnceTime;

//    public bool Audio_IO = true;
//    public float Audio_Timer;
//    public float Audio_CDTime;
//    public string _AudioName1;
//    public string _AudioName2;

//    void Update () {

//        if (Audio_IO) {
//            Audio_Timer += Time.deltaTime;

//            if (Audio_Timer >= Audio_CDTime) {
//                Audio_Timer = 0f;

//                AudioManager._ins.PlayAudioClip("Missile", transform.position, _AudioName1);
//            }
//        }

//        if (m_MissileStruct.m_uptimer < m_MissileStruct.TrackTime && m_MissileStruct.m_MissileEnum == MissileEnum.StraightLine) {

//            m_MissileStruct.m_uptimer += Time.deltaTime;
//            transform.position += transform.forward * m_MissileStruct.m_speed * Time.deltaTime;

//        } else if (m_MissileStruct.m_MissileEnum != MissileEnum.Stop) {

//            // 開始追蹤敵人
//            Vector3 target = (m_MissileStruct.m_Target.transform.position - transform.position).normalized;

//            float a = Vector3.Angle(transform.forward, target) / m_MissileStruct.m_RotSpeed;

//            if (a > 0.1f || a < -0.1f)
//                transform.forward = Vector3.Slerp(transform.forward, target, Time.deltaTime / a).normalized;
//            else {
//                m_MissileStruct.m_speed += 40 * Time.deltaTime;
//                transform.forward = Vector3.Slerp(transform.forward, target, 1).normalized;
//            }

//            transform.position += transform.forward * m_MissileStruct.m_Trackspeed * Time.deltaTime;
//        }

//        if (Vector3.Distance(transform.position, m_MissileStruct.m_Target.transform.position) <= AimsDistance && OnceTime == false) {

//            OnceTime = true;
//            Audio_IO = false;

//            m_MissileStruct.m_speed = 0;
//            m_MissileStruct.m_MissileEnum = MissileEnum.Stop;

//            AudioManager._ins.PlayAudioClip("Missile", transform.position, _AudioName2);
//            TeamManager._ins.UpdateLifeRings(PhotonNetwork.playerName.Substring(0, 1));

//            StartCoroutine(DestroyTime(3));
//        }
//    }

//    public IEnumerator DestroyTime (int k) {

//        for (int i = 0; i < m_MissileStruct.m_EllipsoidParticleEmitter.Length; i++) {
//            m_MissileStruct.m_EllipsoidParticleEmitter[i].emit = false;
//        }

//        if (m_MissileStruct.m_Explosion == null) {
//            m_MissileStruct.m_Explosion = Instantiate(m_MissileStruct.ExplosionPrefab, transform.position, transform.rotation);
//        }

//        yield return new WaitForSeconds(k);

//        Destroy(gameObject);
//    }
//}
