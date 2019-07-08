using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBoxes : MonoBehaviour {


    public bool BoxesIsDestroy = false;
    bool IsDestroy = false;
    Renderer[] BoxMaterial;
    Shader BumpedDiffuseSader;
    // Use this for initialization
    void Start () {

        BumpedDiffuseSader = Shader.Find("Transparent/Bumped Diffuse");
        BoxMaterial = GetComponents<Renderer>();
    }




    void FixedUpdate()
    {
        if (IsDestroy)
        {
            for (int i = 0; i < BoxMaterial.Length; i++)
            {
                Color c = BoxMaterial[i].materials[0].color;
                c.a -= 0.01f;
                BoxMaterial[i].materials[0].color = c;
            }

       
            
        }
        
    }

    public void Destroyy()
    {

        Invoke("DoDestroy", 7f);
    }

    public void DoDestroy()
    {

        IsDestroy = true;

        for (int i = 0; i < BoxMaterial.Length; i++)
        {
            BoxMaterial[i].materials[0].shader = BumpedDiffuseSader;
        }

        //if (PhotonNetwork.isMasterClient)
        //{

            Invoke("DestroyGo", 2f);
        //}
    }

    void DestroyGo()
    {
        Destroy(gameObject);
        //PhotonNetwork.Destroy(gameObject);
    }

}
