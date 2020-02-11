using UnityEngine;
using System.Collections;

// Cartoon FX  - (c) 2015 Jean Moreno

// Indefinitely rotates an object at a constant speed

public class CFX_AutoRotate : MonoBehaviour
{
    #region Fields
    // Rotation speed & axis
    public Vector3 rotation;
	
	// Rotation space
	public Space space = Space.Self;
    #endregion Fields

    #region Methods
    void Update()
	{
		this.transform.Rotate(rotation * Time.deltaTime, space);
    }
    #endregion Methods
}
