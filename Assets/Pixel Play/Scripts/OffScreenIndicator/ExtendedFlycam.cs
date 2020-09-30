using UnityEngine;

/*
FEATURES
    WASD/Arrows:    Movement
    Q:    Climb
    E:    Drop
    Shift:    Move faster
    Control:    Move slower
*/
namespace Pixel_Play.Scripts.OffScreenIndicator
{
    public class ExtendedFlycam : MonoBehaviour
    {
        public float cameraSensitivity = 90;
        public float climbSpeed = 4;
        public float normalMoveSpeed = 10;
        public float slowMoveFactor = 0.25f;
        public float fastMoveFactor = 3;

        private float rotationX = 0.0f;
        private float rotationY = 0.0f;

        private void Update()
        {
            rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, -90, 90);

            var localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
            localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
            transform.localRotation = localRotation;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                var transform1 = transform;
                var position = transform1.position;
                position += transform1.forward *
                            (normalMoveSpeed * fastMoveFactor * Time.deltaTime * Input.GetAxis("Vertical"));
                position += transform.right *
                            (normalMoveSpeed * fastMoveFactor * Time.deltaTime * Input.GetAxis("Horizontal"));
                transform.position = position;
            }
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                Transform transform1;
                (transform1 = transform).position += transform.forward * (normalMoveSpeed * slowMoveFactor * Input.GetAxis("Vertical") * Time.deltaTime);
                transform.position += transform1.right * (normalMoveSpeed * slowMoveFactor * Input.GetAxis("Horizontal") * Time.deltaTime);
            }
            else
            {
                Transform transform1;
                (transform1 = transform).position += transform.forward * (normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
                transform.position += transform1.right * (normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);
            }


            if (Input.GetKey(KeyCode.Q))
            {
                var transform1 = transform;
                transform1.position += transform1.up * (climbSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.E))
            {
                var transform1 = transform;
                transform1.position -= transform1.up * (climbSpeed * Time.deltaTime);
            }
        }
    }
}