using UnityEngine;
using System.Collections;

public class Cannonball : MonoBehaviour {
	
	private float starttime;
	private GameObject explosion;
	private GameObject water;
	
	// Use this for initialization
	void Start () {
		explosion = GameObject.Find("Explosion");
		water = GameObject.Find("water");
		
		starttime = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Time.timeSinceLevelLoad - starttime > 10) {
			Remove();
		}
		
		//water splash 
		if (transform.position.y < water.transform.position.y) {
			
			showExplosion();
		}
		
	}
	
	public void Remove() {
			this.enabled = false;
			this.gameObject.SetActive(false);
			Destroy(this.gameObject);		
	}
	
	
	void OnTriggerEnter(Collider collider) {
		if (collider.tag.Equals("enemy")) {
			showExplosion();
		}
	}
	
	void showExplosion() {
		
		explosion.GetComponent<ParticleSystem>().Clear();
		explosion.GetComponent<ParticleSystem>().Play();
		explosion.transform.position = transform.position;
		Remove();
	}
	
	
}
