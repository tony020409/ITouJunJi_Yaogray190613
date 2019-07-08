using UnityEngine;
using System.Collections;

namespace y_Network
{

    public class NetWork_ObservedCamera : MonoBehaviour
    {

        Transform target;
        // The distance in the x-z plane to the target
        public float distance = 1;
        // the height we want the camera to be above the target
        public float height = 5;
        // How much we 
        public float heightDamping = 3;
        public float rotationDamping = 3;
        public static int type = 0;
        GameObject playerEye;
        GameObject[] playerList;
        public bool isFristPersion;

        // Update is called once per frame
        void Update()
        {

            target = CameraManager.inst.target;


            if (isFristPersion)
            {
                FirstPersonCamera();
            }
            else
            {
                ThirdPersonCamera();
            }
         
        }

        void FirstPersonCamera()
        {
           
            if (target)
            {
                transform.position = Vector3.Lerp(transform.position, target.position, 5 * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, 5 * Time.deltaTime);
            }
        }

        void ThirdPersonCamera()
        {

            if (target)
            {
                // Calculate the current rotation angles
                float wantedRotationAngle = target.eulerAngles.y;
                float wantedHeight = target.position.y + height;

                float currentRotationAngle = transform.eulerAngles.y;
                float currentHeight = transform.position.y;

                // Damp the rotation around the y-axis
                currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

                // Damp the height
                currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

                // Convert the angle into a rotation
                Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

                // Set the position of the camera on the x-z plane to:
                // distance meters behind the target

                Vector3 pos = target.position;
                pos -= currentRotation * Vector3.forward * distance;
                pos.y = currentHeight;
                transform.position = pos;


                // Always look at the target
                transform.LookAt(target);
            }
        }
    }
}
