using UnityEngine;
using System.Collections;

public class CompassButton : MonoBehaviour {
	
	public Main main;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown() {
		//this.gameObject.SetActive(false);
		main.teleport();
	}
	
}
