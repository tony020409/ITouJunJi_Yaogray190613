using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionAims : MonoBehaviour
{
    public Enemy target;
    public Enemy Nowtarget;
    public GameObject UI;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Nowtarget != null && Nowtarget != target)
        {
            Nowtarget.Arrow.SetActive(false);

        }
        if (target != null)
        {
            float distance = Vector3.Distance(target.Arrow.gameObject.transform.position, transform.position);
            Quaternion right = transform.rotation * Quaternion.AngleAxis(30, Vector3.up);
            Quaternion left = transform.rotation * Quaternion.AngleAxis(30, Vector3.down);

            Vector3 n = transform.position + (Vector3.forward * distance);
            Vector3 leftPoint = left * n;
            Vector3 rightPoint = right * n;

            Debug.DrawLine(transform.position, leftPoint, Color.red);
            Debug.DrawLine(transform.position, rightPoint, Color.red);
            Vector3 targetDir = target.Arrow.gameObject.transform.position - transform.position;
            Vector3 forward = transform.forward;
            float angle = Vector3.Angle(targetDir, forward);
            if (angle < 30)
            {
                target.Arrow.SetActive(true);
                target.Arrow.transform.LookAt(transform);
                target.SC.center = target.Arrow.transform.localPosition;
                UI.SetActive(!target.Arrow.activeSelf);
                Nowtarget = target;
            }
            else
            {

                UI.transform.LookAt(target.Arrow.gameObject.transform);
                target.Arrow.SetActive(false);
                UI.SetActive(!target.Arrow.activeSelf);
            }
        }
    }
}
