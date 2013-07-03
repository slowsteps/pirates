using UnityEngine;
using System.Collections;

public class Parrot : MonoBehaviour {
	
	public bool isflying = false;

	
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Animator>().SetBool(0,false);
	}
	
	// Update is called once per frame
	void Update () {
		if (isflying) {
			
		
			//leftwing.transform.Rotate(60,0,0);
			
		}
	}
}
