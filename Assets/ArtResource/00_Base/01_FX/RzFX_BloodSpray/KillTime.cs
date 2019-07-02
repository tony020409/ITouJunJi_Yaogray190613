using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 銷毀
/// </summary>
public class KillTime : MonoBehaviour {

    [Rename("[粒子限定] 啟用粒子特效結束後自動銷毀")]
    public bool AutoParticleKill;

    [Rename("銷毀時間")]
    public float tTime = 1;

    /// <summary>
    /// 銷毀類型
    /// </summary>
    public enum KillType {

        [Rename("直接銷毀")]
        Destroy = 1,

        [Rename("(物件池回收) 粒子")]
        ParticlePool = 2,

    }

    [Rename("回收類型")]
    public KillType _type = KillType.Destroy;


    private void Start() {
        if (BattleMain.GetInstance() == null) {
            _type = KillType.Destroy;
            return;
        }
        if (_type == KillType.ParticlePool) {
            if (BattleMain.GetInstance().m_ParticlePool == null) {
                _type = KillType.Destroy;
            }
        }


    }


    private void OnEnable() {
        if (!this.enabled) {
            return;
        }

        if (AutoParticleKill) {
            if (this.GetComponent<ParticleSystem>() != null) {
                StartCoroutine(f_Kill_Particle(this.GetComponent<ParticleSystem>()));
            } else {
                StartCoroutine(f_Kill_Normal(tTime));
            }
        }
        else {
            StartCoroutine(f_Kill_Normal(tTime));
        }
    }

    /// <summary>
    /// 回收or銷毀物件
    /// </summary>
    /// <param name="tTime"  > 多久後回收 </param>
    IEnumerator f_Kill_Normal (float tTime) {
        yield return new WaitForSeconds(tTime);
        f_Kill();
    }


    /// <summary>
    /// 粒子系統播放結束後自動回收
    /// </summary>
    IEnumerator f_Kill_Particle (ParticleSystem tmp) {
        while (true)  {
            yield return new WaitForSeconds(0.5f);
            if (!tmp.IsAlive(true)) {
                f_Kill();
            }
        }
    }



    void f_Kill() {
        if (_type == KillType.Destroy) {
            Destroy(this.gameObject);
        }
        else if (_type == KillType.ParticlePool) {
            BattleMain.GetInstance().m_ParticlePool.Despawn(this.transform);
        }
    }



}
