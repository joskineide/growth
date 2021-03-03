using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkleScript : MonoBehaviour {

    [SerializeField] private float lifeSpawn;
    [SerializeField] private float curLife = 0;
	private int colorId;
	private ParticleSystem sparkles;
	[SerializeField] Texture2D[] particlesTextures;

	public void setColorId(int colorId){
		this.colorId = colorId;
	}

	void Start(){
		sparkles = GetComponent<ParticleSystem>();
		
		ParticleSystem.MainModule main = sparkles.main;
		
		switch(colorId){
			case 1:
			
				main.startColor = new Color(255f,255f,0f,1f); break;
			case 2:
				main.startColor = new Color(0f,255f,0f,1f); break;
			case 3:
				main.startColor = new Color(0f,0f,255f,1f); break;
			case 4:
				main.startColor = new Color(255f,0f,0f,1f); break;
		}

		Renderer mat = GetComponent<Renderer>();
		mat.material.mainTexture = particlesTextures[colorId - 1];
		
	}
	
	// Update is called once per frame
	void Update () {
        curLife += Time.deltaTime;
        if(curLife > lifeSpawn) Destroy(this.gameObject);    
	}
}

