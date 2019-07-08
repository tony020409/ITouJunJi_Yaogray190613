using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBoxes1 : MonoBehaviour {


    public bool LetBoxesDone = false;
    bool LetChangShader = true;
    bool OKfadeout = false;
    private int fadeout = 0;
    private int st = 0;
    private int ft = 0;
    //public GameObject CheckBoxes;
    Renderer[] BoxMaterial;
    Shader BumpedDiffuseSader;
    // Use this for initialization
    void Start () {

        BumpedDiffuseSader = Shader.Find("Transparent/Bumped Diffuse");
        BoxMaterial = GetComponents<Renderer>();
    }


    void Update()
    {
        if (LetBoxesDone == true && BoxMaterial != null)
        {
            if(LetChangShader == true)
            {
                //fadeout = ccEngine.ccTimeEvent.Instance.f_RegEvent(1f, ChangShader);
                //ccEngine.ccTimeEvent.Instance.f_SetTimeEventExcuteTime(fadeout, 7.0f);


            }

            if (OKfadeout == true)
            {
                for (int i = 0; i < BoxMaterial.Length; i++)
                {
                    Color c = BoxMaterial[i].materials[0].color;
                    c.a -= 0.01f;
                    BoxMaterial[i].materials[0].color = c;

                }
                //if(PhotonNetwork.isMasterClient)
                //{
                    Invoke("FadeOut",2f);
               // }
            }
        }
    }

 

    void ChangShader(object date)
    {
        if (BoxMaterial != null  && LetBoxesDone == true )
        {
            for (int i = 0; i < BoxMaterial.Length; i++)
            {
                BoxMaterial[i].materials[0].shader = BumpedDiffuseSader;

            }

            LetChangShader = false;
            OKfadeout = true;

        }
    }


 
    public void FadeOut()
    {
        // ccEngine.ccTimeEvent.Instance.f_UnRegEvent(fadeout);
        Destroy(gameObject);
        //PhotonNetwork.Destroy(gameObject);

    }



}
