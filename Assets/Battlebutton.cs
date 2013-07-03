using UnityEngine;
using System.Collections;

public class Battlebutton : MonoBehaviour {
	
	public Main main;
	
	// Use this for initialization
	void Start () {
		this.gameObject.SetActive(false);
	}	
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown() {
		this.gameObject.SetActive(false);
		main.enterBattle();
		print ("battlebutton clicked");
	}
}
