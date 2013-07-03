using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	
	public GameObject shipdummy;
	GameObject pirate;
	GameObject rightboot;
	
	//islands
	public GameObject island;
	public ArrayList islands;
	float savedIslandDist = 9999;
	GameObject closestIsland;
	GameObject diggingspot;
	bool newdig = true;
	
	GameObject rudder;
	GameObject cross;
	
	GameObject treasure;
	GameObject chest;
	GameObject boot;
	GameObject parrot;
	ArrayList treasures;
	public GameObject curtreasure;
	public int treasuresfound=0;
	private bool treasureToBeStored = false;
	GameObject diggingparticles;
	float diggingpower = 0 ;
	float diggingtime = 0;
	public GameObject cannonball;
	public Material highlight;
	
	Camera holdcamera;
	Camera maincamera;
	
	
	//camera viewpoints
	GameObject behindviewpoint;
	GameObject defaultviewpoint;
	GameObject orbitdummy;
	GameObject crowsnestdummy;
	GameObject pirate_ship_pos;
	
	GameObject cannonviewpoint;
	GameObject cannonlookat;
	GameObject cannon;
	
	
	//
	GameObject campos;
	//ship helper dummies
	GameObject lookatdummy;
	public GameObject curlookat;
	GameObject shipanimdummy;
	GameObject steerdummy;
	GameObject tapdummy;
	public GameObject water;
	public GameObject map;
	
	//UI
	public GUIText message;
	public GameObject backbutton;
	public GameObject islandbutton;
	
	//
	private GameObject anchor;
	GameObject nozzle;
	public GameObject monster;
	GameObject bell;
	GameObject sails; //model
	GameObject saildummy; //collision dummy
	GameObject saildowndummy; //collision dummy
	GameObject boomup;
	GameObject boomdown;
	GameObject sailsdown;
	GameObject turret;
	GameObject hook;
	GameObject hatch;
	GameObject hold;
	GameObject cargohold;
	GameObject spade;
	GameObject islandhole;
	public GameObject islandflag;
	
	Vector3 spadesavedpos;
	GameObject linedummy;
	
	
	private float rudderspeed;
	private float thrust = 3;
	private string cameraMode;
	private const string CAM_FPS = "fps";
	private const string CAM_TURRET = "turret";
	private const string CAM_FOLLOW = "follow";
	private const string CAM_ORBIT = "orbit";
	private const string CAM_ISO = "iso";
	private const string CAM_ORTHO = "ortho";
	
	
	public const string MODE_ISLAND = "island mode";
	public const string MODE_SHIP = "ship mode";
	public const string MODE_BATTLE = "battle mode";
	public const string MODE_HOLD = "cargo hold mode";    
	
	public string gameMode = MODE_SHIP;
	
	private Vector3 lastmousepos;
	private float horswipe;
	private float vertswipe;
	private float totalswipelength=0;
	private bool steering = false;
	
	private bool hookdown = false;
	private bool iswaterswipe = false;
	private Vector3 savedCannonBallpos;
	private Vector3 savedWaterTouchPos;
	
	
	
	private Material savedmaterial;
	private GameObject savedgameobject;
	
	
	// Use this for initialization
	void Start () {
		
		//dummy helpers
		shipdummy = GameObject.Find("shipdummy");
		shipanimdummy = GameObject.Find("shipanimdummy");
		crowsnestdummy = GameObject.Find("crowsnestdummy");
		pirate_ship_pos = GameObject.Find ("pirate_shipposition_dummy");

		cannonviewpoint = GameObject.Find("cannonviewpoint");
		defaultviewpoint = GameObject.Find("defaultviewpoint");
		behindviewpoint = GameObject.Find ("behindviewpoint");
		cannonlookat = GameObject.Find("cannonlookat");
		cross = GameObject.Find("crossdummy");
		
		nozzle = GameObject.Find("nozzle");
		sails = GameObject.Find("sails");
		saildummy = GameObject.Find("saildummy");
		saildowndummy = GameObject.Find("saildowndummy");
		sailsdown = GameObject.Find("sailsdown");
		boomdown = GameObject.Find("boomdown");
		boomup = GameObject.Find("boomup");
		steerdummy = GameObject.Find("steerdummy");
		tapdummy = GameObject.Find("tapdummy");
		parrot = GameObject.Find("parrot pf");
		parrot.AddComponent("BoxCollider");
		parrot.collider.isTrigger = true;

		water = GameObject.Find("water");
		backbutton = GameObject.Find("backbutton");
		backbutton.SetActive(false);
		islandbutton = GameObject.Find("islandbutton");
		islandbutton.SetActive(false);
		
		nozzle.particleSystem.Pause();
		
		diggingparticles = GameObject.Find("diggingparticles");
		diggingparticles.SetActive(false);
		
		
		//treasures
		treasure = GameObject.Find("treasuredummy");
		chest = GameObject.Find("treasurechest");
		boot = GameObject.Find("boot");
		
		holdcamera = GameObject.Find("HoldCamera").camera;
		maincamera = GameObject.Find("Main Camera").camera;
		holdcamera.camera.enabled = false;
		maincamera.camera.enabled = true;
		
		
		//objects in model
		pirate = GameObject.Find("piratedummy");
		rightboot = GameObject.Find("boot_r1");
		spade = GameObject.Find("spade");
		islandhole = GameObject.Find("islandhole");
	
		
		
		
		island = GameObject.Find("islanddummy");
		rudder = GameObject.Find("rudder1");
		anchor = GameObject.Find("anchor");
		monster = GameObject.Find("monsterdummy");
		bell = GameObject.Find("HingeBellDummy");
		cannon = GameObject.Find("cannongroup");
		
		turret = GameObject.Find("turretdummy");
		turret.AddComponent("SphereCollider");
		turret.collider.isTrigger = false;
		turret.GetComponent<SphereCollider>().radius = 0.5f;
		
		hatch = GameObject.Find("hatch");
		hatch.AddComponent("BoxCollider");
		hatch.collider.isTrigger = true;
		cargohold = GameObject.Find("cargohold"); //collection shelf
		cargohold.SetActive(false);
		

		linedummy = GameObject.Find("linedummy");
		
		hook = GameObject.Find("hook");
		hook.AddComponent("SphereCollider");
		SphereCollider hookcollider = hook.GetComponent<SphereCollider>() as SphereCollider;
		hookcollider.radius = 1f;
		hook.collider.isTrigger = true;
		
		cannon.transform.parent = turret.transform;
	
		
		
		
		//add some collision models for mousepicking
		anchor.AddComponent("SphereCollider");
		anchor.collider.isTrigger = true;		
		
		//ship center
		lookatdummy = GameObject.Find("lookatdummy");
		curlookat = lookatdummy;
		
		//position behind the ship
			
		orbitdummy = new GameObject();
				
		lastmousepos = new Vector3(0,0,1);
		savedWaterTouchPos = new Vector3();
		
		lowerSails();
		toggleDefault("start");
		setupIslands();
		setupTreasures();
		map.SetActive(false);
		
	}
	

	
	// Update is called once per frame
	void Update () {
		
		steerShip();
		checkMouseClicks();
		updateDigging();
		findIslandInRange();
		
	}
	

	
	float inertiaswipe = 0;
	
	void trackCamera() {
		
		if (cameraMode.Equals(CAM_FOLLOW)) {
			Camera.mainCamera.transform.LookAt(curlookat.transform);	
		}
		else if (cameraMode.Equals(CAM_FPS)) {
			Camera.mainCamera.transform.RotateAround(Vector3.up,-0.01f*horswipe);
		}
		else if (cameraMode.Equals(CAM_TURRET)) {
			Camera.mainCamera.transform.LookAt(curlookat.transform);
			//shipdummy.transform.RotateAround(Vector3.up,0.005f*horswipe);
			inertiaswipe = inertiaswipe + 0.0002f*horswipe;
			inertiaswipe = 0.9f*inertiaswipe;
			//shipdummy.transform.RotateAround(Vector3.up,inertiaswipe);
			turret.transform.RotateAround(Vector3.up,inertiaswipe);
			if (Input.GetMouseButtonUp(0)) fireCannon();
			Camera.mainCamera.transform.parent = shipdummy.transform;
		}
		else if (cameraMode.Equals(CAM_ORBIT)) {
			Camera.mainCamera.transform.LookAt(curlookat.transform);	
			campos = orbitdummy;
			orbitdummy.transform.position = curlookat.transform.position;
			orbitdummy.transform.RotateAround(Vector3.up,0.002f*horswipe);
			orbitdummy.transform.Translate(0,2.5f,-4,Space.Self);
		}
		else if (cameraMode.Equals(CAM_ISO)) {
			Camera.mainCamera.transform.LookAt(curlookat.transform);	
			campos = orbitdummy;
			orbitdummy.transform.position = curlookat.transform.position;
			orbitdummy.transform.Translate(7,5,-7,Space.Self);
		}
		else if (cameraMode.Equals(CAM_ORTHO)) {
			//
		}


		
		//interpolate towards viewpoint dummy
		if (!cameraMode.Equals(CAM_ORTHO) ) {
			Camera.mainCamera.transform.position = Vector3.Slerp(Camera.mainCamera.transform.position,campos.transform.position,0.05f);	
		}
	}
	
	void FixedUpdate() {
		
		trackCamera();
		
	
	}
	

	
	void steerShip() {
		
		
		//behind the rudder		
		if (steering) {
			float steerX = Camera.mainCamera.WorldToScreenPoint(rudder.transform.position).x;
			float steerY = Camera.mainCamera.WorldToScreenPoint(rudder.transform.position).y;
			
			
			//reverse turning for touch below wheel center
			float signx;
			float signy;
			if (steerX - Input.mousePosition.x > 0) signx = 1;
			else signx = -1;
			if (steerY - Input.mousePosition.y < 0) signy = 1;
			else signy = -1;
			
			
			//forwards
			shipdummy.rigidbody.AddForce(thrust*shipdummy.transform.forward);
			if (Mathf.Abs(rudderspeed) < 3) {
				rudderspeed = rudderspeed + signy*0.1f*horswipe;
				rudderspeed = rudderspeed + signx*0.1f*vertswipe;
			}
			
			//turn the wheel
			rudder.transform.Rotate(0,0,-0.5f*rudderspeed);
			rudderspeed = rudderspeed*0.9f;
			//turn the boat
			shipdummy.rigidbody.AddTorque(0,03f*rudderspeed,0);
			
		}
		
		
	}
	
	void checkMouseClicks() { 
		
		//mouseclick
		if (Input.GetMouseButtonUp(0) && totalswipelength<10)  {
			onMousePick(Input.mousePosition);
			totalswipelength = 0;
			lastmousepos = new Vector3(0,0,1); //HACK
			horswipe = 0;
			vertswipe = 0;
			
		}
		//end of swipe
		else if (Input.GetMouseButtonUp(0)) {
			totalswipelength = 0;
			horswipe = 0;
			vertswipe = 0;
			iswaterswipe = false;
			lastmousepos = new Vector3(0,0,1); //HACK should be bool flag
			
		}
		
		//mouse still down
		if (Input.GetMouseButton(0)) {
			//swipe per frame
			
			if (lastmousepos.z != 1) {//HACK should be bool flag indicating first frame
				horswipe = Input.mousePosition.x - lastmousepos.x;
				vertswipe = Input.mousePosition.y - lastmousepos.y;
				totalswipelength = totalswipelength + Mathf.Abs(horswipe);
			}
			lastmousepos = Input.mousePosition;
			
			//god hand control
			onGodHand();
			//
			
		}
		else {
			tapdummy.particleSystem.emissionRate = 0;
		}
			
				
	}
	
	//POINT AND CLICK STEERING
	void onGodHand() {
		
		if (gameMode.Equals(CAM_ORTHO)) return;		
		//continous raycasting
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo)) {	
			
			//WATER FINGER
			if (hitInfo.collider.name.Equals("water")) {
			    
				 
				
				Vector3 tappos = hitInfo.point;
				
				//Vector3 waterswipe =  hitInfo.point - savedWaterTouchPos;
				Vector3 waterswipe =  tappos - savedWaterTouchPos;
				if (waterswipe.magnitude>0.1f) waterswipe = 50f*Vector3.Normalize(waterswipe);
				
				
				tapdummy.particleSystem.emissionRate = 15;
				tapdummy.transform.position = hitInfo.point;
				
				if (!steering && cameraMode.Equals(CAM_ISO)) {
					//sail towards finger when not steering
					if (iswaterswipe) {
						//Debug.DrawRay(hitInfo.point,10*waterswipe);
						
						float normalF = Vector3.Dot(shipdummy.transform.right,waterswipe);
						float tangF = Vector3.Dot(shipdummy.transform.forward,waterswipe);
						
						Vector3 delta = hitInfo.point - shipdummy.transform.position;
						if (Vector3.Dot(delta,shipdummy.transform.forward)<0) normalF = -normalF;  //swipe back of the boat
						
						
						shipdummy.rigidbody.AddTorque(0,normalF,0);
						shipdummy.rigidbody.AddForce(0.4f*tangF*shipdummy.transform.forward);
					}
						
					iswaterswipe = true;
				}
				
				
				//savedWaterTouchPos = hitInfo.point;
				savedWaterTouchPos = tappos;
				
			}
			
			
		}
		
		
	}
	
	
	void onMousePick(Vector3 cursorpos) {
		
		Ray ray = Camera.main.ScreenPointToRay(cursorpos);
		RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo)) {
			
			
				message.text = hitInfo.collider.name + " " + hitInfo.collider.tag;
				if (hitInfo.collider.name.Equals("anchor")) toggleAnchor();	
				else if (hitInfo.collider.name.Equals("steerdummy")) toggleRudder();
				else if (hitInfo.collider.name.Equals("crowsnest 1")) toggleCrowsnest();	
				//else if (hitInfo.collider.name.Equals("deckdummy")) toggleDeck(hitInfo);
				//else if (hitInfo.collider.name.Equals("islanddummy")) toggleIsland();
				//else if (hitInfo.collider.name.Equals("crossdummy")) digForTreasure();
				else if (hitInfo.collider.tag.Equals("diggingspot")) digForTreasure();
				//else if (hitInfo.collider.name.Equals("treasuredummy")) toggleTreasure();
				//TODO tag
				else if (hitInfo.collider.name.Equals("treasurechest")) toggleTreasure();
				else if (hitInfo.collider.name.Equals("boot")) toggleTreasure();
				else if (hitInfo.collider.name.Equals("treasurechest-pf(Clone)")) toggleTreasure();
				//end TODO
				else if (hitInfo.collider.name.Equals("cannondummy")) toggleCannon();
				//else if (hitInfo.collider.name.Equals("monsterdummy")) toggleCannon();
				else if (hitInfo.collider.name.Equals("saildummy")) toggleSail();
				else if (hitInfo.collider.name.Equals("saildowndummy")) toggleRudder();
				else if (hitInfo.collider.name.Equals("HingeBellDummy")) toggleBell();
				else if (hitInfo.collider.name.Equals("hook")) toggleHook();
				else if (hitInfo.collider.name.Equals("hatch")) showHold();
				else if (hitInfo.collider.name.Equals("turretdummy")) fireCannon();
				else if (hitInfo.collider.name.Equals("parrot pf")) toggleParrot();
				
				

			}
			
	
	}
	
	
	void lowerSails() {
		sails.SetActive(false);
		saildummy.SetActive(false);
		saildowndummy.SetActive(true);
		anchor.SetActive(false);
		steering = false;
		//toggleDefault("lower sails");
		boomup.SetActive(false);
		boomdown.SetActive(true);
		sailsdown.SetActive(true);
		
	}
	
	void raiseSails() {
		
		boomup.SetActive(true);
		sails.SetActive(true);
		saildummy.SetActive(true);
		saildowndummy.SetActive(false);
		anchor.SetActive(true);
		boomdown.SetActive(false);
		sailsdown.SetActive(false);
		steering = true;
		
	}
	
	void toggleAnchor() {
		lowerSails();
		anchor.SetActive(false);
	}
	
	void toggleRudder() {
		Camera.mainCamera.fieldOfView = 90;
		cameraMode = CAM_FOLLOW;
		campos = behindviewpoint;
		curlookat = lookatdummy;
		steering = true;
		raiseSails();
		backbutton.SetActive(true);
	}
	
	void toggleCrowsnest() {
		lowerSails();
		cameraMode = CAM_FPS;
		campos = crowsnestdummy;
		backbutton.SetActive(true);
		steering = false;
		
	}
	

	
	public void toggleDefault(string reason) {
		print ("toggle default, reason: " + reason);
		cargohold.SetActive(false);
		holdcamera.camera.enabled = false;
		maincamera.camera.enabled = true;
		cameraMode = CAM_ISO;
		campos = defaultviewpoint;
		curlookat = shipdummy;
		steering = false;
		Camera.mainCamera.fieldOfView = 70;
		
		piratesToShip();
		
		gameMode = MODE_SHIP;
		
	}
	
	public void enterIsland() {
		//place pirate on island when closeby
			gameMode = MODE_ISLAND;
			cameraMode = CAM_ORBIT;
			//curlookat = pirate;
			diggingspot = closestIsland.GetComponent<Island>().digginglocation;
			diggingtime = 0;
			newdig = true;
			curlookat = diggingspot;
			//curlookat = cross;
			steering = false;
			pirate.transform.parent = null;
			
			pirate.transform.position = diggingspot.transform.position;
			//pirate.transform.position = islandviewpoint.transform.position;
			backbutton.SetActive(true);
	}
	
	public void enterShip() {
			
	}
	
	void updateDigging() {
		if (gameMode.Equals(MODE_ISLAND)) {
			//message.text = "diggingtime: " + diggingtime.ToString();
			diggingparticles.particleSystem.emissionRate = diggingpower;
			
			
			if (diggingpower > 0 ) {
				spade.transform.position = new Vector3(spade.transform.position.x, diggingspot.transform.position.y + 0.7f + 0.2f*Mathf.Sin(diggingpower*0.001f*Time.frameCount),spade.transform.position.z);
				diggingpower--;
				diggingtime++;
				islandhole.transform.localScale = new Vector3(0.01f*diggingtime,0.01f*diggingtime,0.01f*diggingtime);
				if (diggingtime == 200) {
					revealTreasure();
					spade.SetActive(false);
				}
			}
			else {
			spade.transform.position = new Vector3(spade.transform.position.x, diggingspot.transform.position.y + 0.7f,spade.transform.position.z);
			}

		}
	}
	
	void digForTreasure() {
		
		//print ("newdig: " + newdig.ToString() );
		
		if (newdig) {
			steering = false;
			diggingparticles.transform.position = diggingspot.transform.position;
			diggingparticles.SetActive(true);
			islandhole.SetActive(true);
			islandhole.transform.position = diggingspot.transform.position;
			islandhole.transform.position = diggingspot.transform.position;
			islandhole.transform.localScale = new Vector3(1,1,1);
			diggingpower = 30;
			newdig = false;
		}
		else {
			diggingpower = 30;
		}
				
	}
	
	void setupIslands() {
		islands = new ArrayList();
		islands.Add(island);
		islands.Add(GameObject.Find("towerisland pf"));
	}
	
	void setupTreasures() {
		treasures = new ArrayList();
		
		treasures.Add(boot);
		boot.AddComponent("Treasure");
		boot.GetComponent<Treasure>().isJunk = true;
		
		treasures.Add(chest);
		chest.AddComponent("Treasure");
		chest.GetComponent<Treasure>().isJunk = false;
				
		foreach (GameObject g in treasures) {
			g.SetActive(false);
		}
		
		
		islandhole.SetActive(false);
		

		
		
		
	}
	
	void revealTreasure() {
		
		//treasure is the dummy container object
		
		closestIsland.GetComponent<Island>().treasureFound = true;
	
		
		curtreasure = treasures[Random.Range(0,treasures.Count)] as GameObject;
		curtreasure.SetActive(true);
		
		curtreasure.transform.position = treasure.transform.position;
		curtreasure.transform.parent = treasure.transform;
		
		treasure.transform.position = pirate.transform.position + 2*pirate.transform.forward;
		print ("tween treasure");
		iTween.MoveFrom(treasure,iTween.Hash("y",10,"easetype",iTween.EaseType.easeOutBounce,"time",2));
		
		curtreasure.collider.enabled = true;
		
		
		
	}
	
	void toggleTreasure() {
	
		print ("toggle treasure");
		
		//curtreasure = treasures[1] as GameObject;
		//plant a flag to mark this island as explored
		
		if (curtreasure.GetComponent<Treasure>().isFromIsland) {
			print ("toggle treasure - is from island");
			print ("islandflag: " + islandflag.name);
			GameObject.Instantiate(islandflag ,pirate.transform.position - 2*pirate.transform.forward,Quaternion.identity);
			//GameObject.Instantiate(islandflag);
			print ("post instantiate");
			//iTween.MoveFrom(anIslandflag,iTween.Hash("y",10,"time",0.4f,"delay",0));
		}
		
		
		if (curtreasure.GetComponent<Treasure>().isJunk) {
			print ("Junk treasure");
			
			//iTween.RotateTo(rightboot,iTween.Hash("x",-50));
			curtreasure.transform.Translate(0,1,0);
			curtreasure.AddComponent("Rigidbody");
			curtreasure.collider.isTrigger = false;
			curtreasure.rigidbody.AddForce(300*pirate.transform.forward);
			curtreasure.rigidbody.AddForce(0,400,0);
			curtreasure.rigidbody.AddTorque(20,0,10);
			
			curlookat = curtreasure;
			
		}
		else {
			print ("good treasure");
			//gameMode = MODE_HOLD;
			curtreasure.transform.position = hook.transform.position;
			curtreasure.transform.rotation = hook.transform.rotation;
			curtreasure.transform.Translate(0,-1.5f,0);
			curtreasure.transform.parent = hook.transform;
			curtreasure.collider.enabled = false;
			
			treasureToBeStored = true;
			
			
			toggleDefault("treasure clicked");
		}
	}
	
	void toggleSail() {
		if (steering) lowerSails();
		else raiseSails();
	}
	
	void toggleBell() {
		bell.rigidbody.AddTorque(0,0,-300);
	}
	
	void toggleHook() {
		//lower the hook
		print ("toggleHook");
		
		if (!hookdown && treasureToBeStored) {
			gameMode = MODE_HOLD;
			hookdown = true;
			iTween.MoveBy(hook,iTween.Hash("y",-4.5,"time",4,"oncomplete","onHookComplete","oncompletetarget",this.gameObject));
			iTween.RotateAdd(hatch,iTween.Hash("z",80,"time",3));
			drawHookLine(-2);
			
		}
		
	}
	
	void piratesToShip() {
		pirate.transform.position = pirate_ship_pos.transform.position;
		pirate.transform.parent = shipanimdummy.transform;
	}
	
	void drawHookLine(float amount) {
		LineRenderer lr = linedummy.GetComponent<LineRenderer>();
		lr.SetPosition(1,new Vector3(0,amount,0));
	}
	
	void onHookComplete() {
		showHold();
		addTreasureToHold();
		hook.transform.Translate(0,4.5f,0);
		hatch.transform.Rotate(0,0,-80);
		drawHookLine(1.5f);
		curtreasure.SetActive(false);
		hookdown = false;
	}
	
	void showHold() {
		print("showing hold");
		Camera.mainCamera.enabled = false;
		holdcamera.enabled = true;
		cameraMode = CAM_ORTHO;
		
		cargohold.SetActive(true);
		cargohold.transform.position = holdcamera.transform.position + 5*holdcamera.transform.forward;
		cargohold.transform.rotation = Quaternion.Euler(0,-90,0);
	
		backbutton.SetActive(true);
		
	}
	
	void addTreasureToHold() {
		print ("add treasure to hold");
		GameObject tr = Resources.Load("treasurechest-pf") as GameObject;
		GameObject freshloot = Instantiate(tr,cargohold.transform.position,Quaternion.identity) as GameObject;
		
		freshloot.transform.Translate(2*treasuresfound,0,0);
		
		iTween.MoveFrom(freshloot,iTween.Hash("y",0));
		
		treasuresfound++;
		treasureToBeStored = false;
		monster.SetActive(true);		
	}
	
	void toggleCannon() {
		
		//first click, lookat the monster
		if (!cameraMode.Equals(CAM_TURRET)) {
			lowerSails();
			turret.transform.LookAt(monster.transform.position,Vector3.up);
			curlookat = cannonlookat;
			cannonlookat.transform.position = monster.transform.position;
			cameraMode = CAM_TURRET;
			campos = cannonviewpoint;	
			steering = false;
			backbutton.SetActive(true);
		}
	
	}
	
	public void enterBattle() {
		print(MODE_BATTLE);
		toggleCannon();
		gameMode = MODE_BATTLE;
	}
	
	
	void fireCannon() {
			//fire new cannonball
			GameObject newball = Instantiate(cannonball,nozzle.transform.position,Quaternion.identity) as GameObject;
			nozzle.particleSystem.Emit(5);
			newball.AddComponent("SphereCollider");
			newball.AddComponent("Rigidbody");
		
			
			newball.rigidbody.AddForce(900*nozzle.transform.right);
	}
	
	
	public void teleport() {
		shipdummy.rigidbody.isKinematic = true;
		shipdummy.transform.position = island.transform.position;
		shipdummy.transform.Translate(0,3.5f,-20);
		map.SetActive(true);
		map.transform.position = new Vector3(0.5f,0.5f,0);
		iTween.MoveTo(map,iTween.Hash("x",-0.5f,"time",1,"delay",1,"easetype",iTween.EaseType.easeInCubic));
		iTween.MoveFrom(shipdummy,iTween.Hash("y",30,"time",3,"delay",1,"easetype",iTween.EaseType.easeOutElastic,"oncomplete","onTeleportComplete","oncompletetarget",this.gameObject));
		
	}
	
	void onTeleportComplete() {
		shipdummy.rigidbody.isKinematic = false;
		map.SetActive(false);
	}
	
	void toggleParrot() {
		Parrot parrotscript = parrot.GetComponent<Parrot>();
		message.text = parrotscript.isflying.ToString();
		if (parrotscript.isflying) {
			iTween.MoveBy(parrot,iTween.Hash("y",4));
			parrot.GetComponent<Parrot>().isflying = false;
			parrot.GetComponent<Animator>().SetBool("isflying",true);
			parrot.transform.parent = null;
		}
		else {
			
			parrot.transform.parent = shipdummy.transform;
			iTween.MoveTo(parrot,iTween.Hash("position",pirate.transform));
			parrot.GetComponent<Parrot>().isflying = true;
			parrot.GetComponent<Animator>().SetBool("isflying",false);			
		}
	}
	
	void findIslandInRange() {
		
		
		foreach(GameObject anIsland in islands) {
			print(anIsland.GetComponent<Island>().treasureFound);
			float dist = Vector3.Distance(anIsland.transform.position,shipdummy.transform.position);
			if (dist<savedIslandDist) {
				closestIsland = anIsland;
				savedIslandDist = dist;
			}
			
		}
	}
	
}
