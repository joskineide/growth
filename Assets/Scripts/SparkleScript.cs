using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkleScript : MonoBehaviour {

    public float lifeSpawn;
    public float curLife = 0;
	public int colorId;
	public ParticleSystem sparkles;

	void Start(){
		sparkles = GetComponent<ParticleSystem>();
		var main = sparkles.main;
		
		switch(colorId){
			case 1:
				main.startColor = new Color(255f,255f,0f,1f);
				break;
			case 2:
				main.startColor = new Color(0f,255f,0f,1f);
				break;
			case 3:
				main.startColor = new Color(0f,0f,255f,1f);
				break;
			case 4:
				main.startColor = new Color(255f,0f,0f,1f);
				break;
		}
	}
	
	// Update is called once per frame
	void Update () {

        curLife += Time.deltaTime;
        if(curLife > lifeSpawn){

			Destroy(this.gameObject);
              
        }
    
	}
}

