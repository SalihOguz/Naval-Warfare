using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	
	float maxSpeed=10f;
	private float speed=7f;//acceleration
	private float reverse=2f;
	private float turning= 0f;
	private float tra=0f;
	private float change=0f;

	public float firedelay = 100;//ms

	private bool left_cannons;
	private bool right_cannons;
	private bool forward_cannons;

	
	private float t=0;
	private float maxHeight=14.0f;
	private float maxDist=10.0f;
	private float maxHeightM=8.0f;
	private float maxDistM=8.0f;
	
	private int rf;
	private int lf;
	private int ff;

	private string rn;
	private string ln;
	private string fn;
	private float opt=0;

	private float traRot=0;


	public int numberCannons;//number of cannons per side,this depends on ship - big one 16, small one -4
	public bool is_big; //if true then it has forward cannons as well
	
	
	
	void  OnGUI (){
		
		//GUI.Label( new Rect(10,60,200,90), "Move - Arrows");
	
		//GUI.Label( new Rect(10,140,200,90), "Camera Height - Mouse Wheel");
		
	}
	
	void  Start (){
		InvokeRepeating("FireL",0f,firedelay/1000f);
		InvokeRepeating("FireR",0f,firedelay/1000f);
		if (is_big)
			InvokeRepeating("FireF",0f,firedelay/1000f);
	}

	void FireL(){
		if (left_cannons){
			Transform pos;
			//choosing a random cannon from the left side
			lf = Random.Range(0,numberCannons);// we choose random from our cannons
			
			ln = "spawn" + lf+"l"; // finding a corresponding spawn point
			if (ln=="spawn0l"){ln="spawnl";}
			print("ln_"+ln);// debug
			pos = this.transform.Find("SpawnPoints").Find(ln).transform; //assign the spawn position

			//shooting code here

			left_cannons = false;
		}

	}
	void FireR(){
		if (right_cannons){
			Transform pos;
			//shot is done from a random right side cannon
			rf = Random.Range(0,numberCannons);
			
			rn = "spawn" + rf;//name of random cannon (you can find spawn points in prefab)
			if (rn=="spawn0"){rn="spawn";}
			print("rn_"+rn);// debug
			pos = this.transform.Find("SpawnPoints").Find(rn).transform;

			//shooting code here
			right_cannons = false;
		}
	}

	void FireF(){
		if (forward_cannons){
			Transform pos;
			//shot is done from a random forward cannon
			ff = Random.Range(1,3);
			
			fn = "spawn" + ff+"f";//name of random cannon (you can find spawn points in prefab)
			if (rn=="spawn0"){rn="spawn";}
			print("fn_"+fn);// debug
			pos = this.transform.Find("SpawnPoints").Find(fn).transform;
			
			//shooting code here
			forward_cannons = false;
		}
	}
	
	
	void  Update (){
		
		if (Input.GetKey(KeyCode.UpArrow)){
			

			if (GetComponent<Rigidbody>().velocity.magnitude<maxSpeed){//max speed
				GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed*Time.deltaTime*60);
			}
		} 
		
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)){
			turning=1;
			if (GetComponent<Rigidbody>().angularVelocity.magnitude<1){
				
				if (transform.rotation.eulerAngles.z>=0 && transform.rotation.eulerAngles.z<120){
					opt=(transform.rotation.eulerAngles.z+10)/10f;
				}
				if (transform.rotation.eulerAngles.z<=360 && transform.rotation.eulerAngles.z>120){
					opt=(370-(transform.rotation.eulerAngles.z))/10f;
				}
				transform.eulerAngles+=new Vector3(0f,0f,Input.GetAxis("Horizontal")*GetComponent<Rigidbody>().velocity.magnitude*Time.deltaTime*10f/opt);// when turning left or right 
				//this tilts a ship a little
				GetComponent<Rigidbody>().AddRelativeTorque(Vector3.up * Input.GetAxis("Horizontal")*Time.deltaTime*turning*30f*(GetComponent<Rigidbody>().velocity.magnitude+1));// the rotation
				//of the ship with the respect to axis Y
				
				
				
			}
			
		}
		
		
		
		//transform.position=new Vector3(transform.position.x,7.13f,transform.position.z); // this is a constant y position. This prevent the ship from sinking\flying.
		
		transform.eulerAngles=new Vector3(0f,transform.eulerAngles.y,transform.eulerAngles.z);// the rotation of the ship with the respect to this axis is equal to 0
		if (transform.rotation.eulerAngles.z>=0 && transform.rotation.eulerAngles.z<120){
			tra=transform.rotation.eulerAngles.z+180;
		}
		if (transform.rotation.eulerAngles.z<=360 && transform.rotation.eulerAngles.z>120){
			tra=transform.rotation.eulerAngles.z-180;
		}
		change=(180-tra)+Mathf.Sin(t)*(GetComponent<Rigidbody>().velocity.magnitude+1)*3f;
		transform.eulerAngles+=new Vector3(0f,0f,Time.deltaTime*change); // wiggling of the ship
		t+=Time.deltaTime;
		
		
		

		
		if (Input.GetKey(KeyCode.C) )
		{
			right_cannons = true;
			
		}
		
		if (Input.GetKey(KeyCode.X) )
		{
			forward_cannons = true;
			
		}


		if (Input.GetKey(KeyCode.Z))
		{
			
			left_cannons = true;
			
		}

		if (Input.GetKey(KeyCode.F))
		{
			FireR();
			FireL();
		}
		
		//Camera adjusting
		adjust_camera();
		
	}
	void adjust_camera(){
		
		if ((maxHeight > maxHeightM && maxDist > maxDistM || Input.GetAxis("Mouse ScrollWheel")<=0) && (maxHeight < maxHeightM*2 && maxDist < maxDistM*2 || Input.GetAxis("Mouse ScrollWheel")>=0)){
			maxHeight += -(Input.GetAxis("Mouse ScrollWheel")*8);
			maxDist  += -(Input.GetAxis("Mouse ScrollWheel")*2);
		}
		FollowShip.height-=GetComponent<Rigidbody>().velocity.magnitude*(FollowShip.height-maxHeight/2)/3;
		FollowShip.distance-=GetComponent<Rigidbody>().velocity.magnitude*(FollowShip.distance-maxDist/2)/15;
		FollowShip.height+=(maxHeight-FollowShip.height)/10;
		FollowShip.distance+=(maxDist-FollowShip.distance)/5;

	}
	
}