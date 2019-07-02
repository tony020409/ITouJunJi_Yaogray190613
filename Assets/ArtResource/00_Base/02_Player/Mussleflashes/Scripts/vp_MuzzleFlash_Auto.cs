using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vp_MuzzleFlash_Auto : MonoBehaviour {




    protected float m_FadeSpeed = 0.075f;               // amount of alpha to be deducted each frame
    protected bool m_ForceShow = false;                 // used to set the muzzleflash 'always on' in the editor
    protected Color m_Color = new Color(1, 1, 1, 0.0f);

    protected Transform m_Transform = null;
    public float FadeSpeed { get { return m_FadeSpeed; } set { m_FadeSpeed = value; } }
    public bool ForceShow { get { return m_ForceShow; } set { m_ForceShow = value; } }

    protected Light m_Light = null;
    protected float m_LightIntensity = 0.0f;

    protected Renderer m_Renderer = null;
    protected Material m_Material = null;


    void Awake()
    {

        m_Transform = transform;
        m_ForceShow = false;

        // if a light is present in the prefab we will cache and use it
        m_Light = GetComponent<Light>();
        if (m_Light != null) {
            m_LightIntensity = m_Light.intensity;
            m_Light.intensity = 0.0f;
        }

        m_Renderer = GetComponent<Renderer>();
        if (m_Renderer != null) {
            m_Material = m_Renderer.material;
            if (m_Material != null) {
                // the muzzleflash is meant to use the 'Particles/Additive'
                // (unity default) shader which has the 'TintColor' property
                m_Color = m_Material.GetColor("_TintColor");
                m_Color.a = 0.0f;
            }
        }

    }


    void Start() {
       ShootInternal(true);
    }


    void Update()
    {

        //自動開啟
        ShootInternal(true);

        // editor force show
        if (m_ForceShow) {
            Show();
        }  
        else {
            // always fade out muzzleflash if it is visible
            if (m_Color.a > 0.0f)  {
                m_Color.a -= m_FadeSpeed * (Time.deltaTime * 60.0f);
                if (m_Light != null)
                    m_Light.intensity = m_LightIntensity * (m_Color.a * 5.0f);  // sync light intensity to muzzleflash alpha
            }
        }

        if (m_Material != null)
            m_Material.SetColor("_TintColor", m_Color); // TODO: will affect all muzzleflashes using this material in the scene (!)

        if (m_Color.a < 0.01f) {
            m_Renderer.enabled = false;
            if (m_Light != null)
                m_Light.enabled = false;
        }
    }


    /// <summary>
    /// makes the muzzleflash show for editing purposes
    /// </summary>
    public void Show() {
        m_Renderer.enabled = true;
        if (m_Light != null) {
            m_Light.enabled = true;
            m_Light.intensity = m_LightIntensity;
        }
        m_Color.a = 0.5f;   // the default alpha value for the 'Particles/Additive' shader is 0.5
    }



    /// <summary>
    /// shows and rotates the muzzleflash for when firing a shot
    /// </summary>
    public void ShootInternal(bool showMesh)
    {
        m_Color.a = 0.5f;   // the default alpha value for the 'Particles/Additive' shader is 0.5
        if (showMesh) {
            if (m_Transform == null)  {
                return;
            }
            m_Transform.Rotate(0, 0, Random.Range(0, 360)); // rotate randomly 360 degrees around z
            m_Renderer.enabled = true;
        }

        if (m_Light != null) {
            m_Light.enabled = true;
            m_Light.intensity = m_LightIntensity;
        }

    }



}
