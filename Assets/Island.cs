using UnityEngine;
using System.Collections;

public class Island : MonoBehaviour {
	
	public Main main;
	public bool treasureFound = false;
	public GameObject digginglocation;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Vector3.Distance(this.gameObject.transform.position,main.shipdummy.transform.position) < 30 ) {
			if(!treasureFound && main.gameMode.Equals(Main.MODE_SHIP)) {
				main.islandbutton.gameObject.SetActive(true);
			}
			else {
				main.islandbutton.gameObject.SetActive(false);
				
			}
			
		}
	
		
	}


}
