using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sam;

public class Laser : MonoBehaviour
{
    public LineRenderer LR;
    public Track T1;
    void Start()
    {
        LR = GetComponent<LineRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        Ray landRay = new Ray(transform.position, transform.forward * 300);
        RaycastHit hit;
        if (Physics.Raycast(landRay, out hit, 300))
        {
            LR.enabled = true;
            LR.SetPosition(0, transform.position);
            LR.SetPosition(1, hit.point);
            if (hit.collider.GetComponent<Track>() != null)
            {
                T1 = hit.collider.GetComponent<Track>();
                T1.TracksStart();

            }
            else
            {
                if (T1 != null)
                {
                    T1.TrackStop();
                    T1 = null;
                }
            }
        }
        else
        {
            if (T1 != null)
            {
                T1.TrackStop();
                T1 = null;
            }
            LR.enabled = false;
        }
    }
}
