using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float emitTime, Destroytime;

    /// <summary>
    /// 特效
    /// </summary>
    public EllipsoidParticleEmitter[] m_EllipsoidParticleEmitter;

    void Start () {
        StartCoroutine(DestroyTime());
    }

    public IEnumerator DestroyTime () {

        yield return new WaitForSeconds(emitTime);

        for (int i = 0; i < m_EllipsoidParticleEmitter.Length; i++) {
            m_EllipsoidParticleEmitter[i].emit = false;
        }

        yield return new WaitForSeconds(Destroytime);

        Destroy(gameObject);

    }

}
