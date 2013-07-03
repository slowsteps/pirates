using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour {
	
	public bool isJunk = false;
	public bool isCargo = false;
	public bool isFromIsland = true;
	private GameObject water;
	private GameObject explosion;
	private Main main;
	
	// Use this for initialization
	void Start () {
		main = GameObject.Find("Maindummy").GetComponent<Main>();
	}
	
	void Awake() {
		print ("new treasure " + this.name);
		explosion = GameObject.Find("Explosion");
		water = GameObject.Find("water");
		this.gameObject.AddComponent("SphereCollider");
		SphereCollider sphereCollider = gameObject.GetComponent<SphereCollider>();
		sphereCollider.radius = 1f;
		this.gameObject.collider.isTrigger = true;
		this.gameObject.collider.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
			if (isJunk && (transform.position.y < water.transform.position.y)) {
			this.gameObject.SetActive(false);
			print ("splash");
			showExplosion();
		}
	}
	
	
	void showExplosion() {
		
		main.monster.SetActive(true);
		explosion.GetComponent<ParticleSystem>().Clear();
		explosion.GetComponent<ParticleSystem>().Play();
		explosion.transform.position = transform.position;
		//Remove();
	}
	
	
	
}
