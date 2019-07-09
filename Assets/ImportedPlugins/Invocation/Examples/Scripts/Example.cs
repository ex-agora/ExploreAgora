
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Example : MonoBehaviour
{
	//------------------------------------------------------------------------------------------------------------	
    private bool m_Animate = false;
	
	//------------------------------------------------------------------------------------------------------------	
    public void SetAnimate(bool lAnimate)
	{
		m_Animate = lAnimate;
	}
	
	//------------------------------------------------------------------------------------------------------------	
    private void Update()
	{
		if (m_Animate)
		{
			var lPosition = transform.localPosition;
			lPosition.x = Mathf.Sin(Time.time) * 2f;
			transform.localPosition = lPosition;
		}		
	}
}