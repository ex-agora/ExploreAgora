using UnityEngine;

namespace SIS
{
    /// <summary>
    /// Simple VFX script for rotating a transform.
    /// </summary>
	public class Spin : MonoBehaviour
	{
        /// <summary>
        /// The rotation speed per second.
        /// </summary>
		public float rotationsPerSecond = 0.1f;


        //rotating per frame
		void Update ()
		{
			Vector3 euler = transform.localEulerAngles;
			euler.z -= rotationsPerSecond * 360f * Time.deltaTime;
			transform.localEulerAngles = euler;
		}
	}
}
