using UnityEngine;
using System.Collections;

public class Islandbutton : MonoBehaviour {
	
	public Main main;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown() {
		this.gameObject.SetActive(false);
		main.enterIsland();
		
	}
	
}
