using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {
	
	//private Main mainscript;
	public GameObject shipdummy;
	public GUITexture battlebutton;
	public ParticleSystem starparticles;
	public Main main;
	private int damage = 0;
	private int maxdamage = 3;
	
	// Use this for initialization
	void Start () {
			
		starparticles.Pause();
		//gameObject.transform.position = 50*shipdummy.transform.forward + 20*Random.insideUnitSphere;
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (damage<maxdamage) {
			gameObject.transform.LookAt(shipdummy.transform,Vector3.up);
			gameObject.transform.position = new Vector3(gameObject.transform.position.x,4f*Mathf.Sin(0.02f*Time.frameCount),gameObject.transform.position.z);
		}
		//destroyed, sinking
		else {
			gameObject.transform.Translate(0,-0.1f,0);
			gameObject.transform.Rotate(0,5,0);
			if (gameObject.transform.position.y < -10) {
				damage = 0;
				gameObject.transform.position = 30*shipdummy.transform.forward + 20*Random.insideUnitSphere;
				
								
			}
		}
		
		if (Vector3.Distance(this.gameObject.transform.position,shipdummy.transform.position) < 30 ) {
			//print ("monster in range");
			if(main.gameMode.Equals(Main.MODE_SHIP)) battlebutton.gameObject.SetActive(true);
		}
		else {
			//print ("monster out of range");
			battlebutton.gameObject.SetActive(false);
		}
		
	}
	
	
	
	
	void OnTriggerEnter(Collider collider) {
		
		if (collider.tag.Equals("cannonball")) {
			damage++;	
			starparticles.Clear();
			starparticles.Play();
			collider.gameObject.GetComponent<Cannonball>().Remove();	
			if (damage==maxdamage) {
				revealTreasure();
			}
		}
			
	}
	
	void revealTreasure() {
		GameObject tr = Resources.Load("treasurechest-pf") as GameObject;
		GameObject freshloot = Instantiate(tr, this.gameObject.transform.position,Quaternion.identity) as GameObject;
		freshloot.transform.position = new Vector3(freshloot.transform.position.x,main.water.transform.position.y+4,freshloot.transform.position.z);
		//freshloot.transform.Translate(0,2,0);
		//iTween.MoveFrom(freshloot,iTween.Hash("y",0,"time",5,"easetype",iTween.EaseType.easeOutElastic));
		freshloot.AddComponent("Treasure");
		freshloot.GetComponent<Treasure>().isJunk = false;
		freshloot.GetComponent<Treasure>().isFromIsland = false;
		main.curtreasure = freshloot;
		freshloot.collider.enabled = true;
	}

}
