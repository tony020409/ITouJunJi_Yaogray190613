using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsMove : MonoBehaviour 
{

	Rigidbody rb;   

	void Start () 
	{   
		rb = GetComponent<Rigidbody> ();    

		StartCoroutine("Rotate");    
		 
		StartCoroutine("RecoverGravity");  
	}    
		
	void FixedUpdate ()
	{   
		rb.AddForce (-transform.right * 5f);   

		rb.AddForce (transform.up * 8f);  
	}   

	IEnumerator Rotate()
	{   
		while (true) 
		{       
			yield return new WaitForSeconds (0.1f);    

			transform.eulerAngles += new Vector3 (Random.Range (-360f, 360f), Random.Range (-360f, 360f),Random.Range(-360f,360f));      
		}    
	}   

	IEnumerator RecoverGravity()
	{    
		yield return new WaitForSeconds (0.2f);   

		rb.useGravity = true;  
	} 
}  

