using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTextScript : MonoBehaviour
{

    [SerializeField]
    private float growSpeed;
    [SerializeField]
    private float fadeSpeed;
    private float curFade = 2;
    [SerializeField]
    private float riseSpeed;
    [SerializeField]
    private Sprite[] sprites;
    private float curRise;
    private SpriteRenderer actualSprite;
    private float redWeight = 1;
    private float blueWeight = 1;
    private float greenWeight = 1;

    void Start()
    {
        actualSprite = GetComponent<SpriteRenderer>();
        curRise = this.transform.position.y;
        fadeSpeed /= 1000;
        growSpeed /= 1000;
        riseSpeed /= 1000;
    }
    //!!// DEIXAR POR SPRITE NO GAMEBOARD, NÃO POR GAMEOBJECT

    public void Setup(float red, float green, float blue, float yellow, int curPos) {

        // Debug.Log(curPos);
        // Debug.Log(sprites.Length);
        actualSprite = GetComponent<SpriteRenderer>();
        // Debug.Log(actualSprite);

        actualSprite.sprite = curPos >= sprites.Length ? sprites[sprites.Length - 1] : sprites[curPos];
    
        redWeight = red + (yellow / 2);
        blueWeight = blue;
        greenWeight = green + (yellow/2);

        float dominantWeight = 1;

        if (redWeight >= blueWeight && redWeight >= greenWeight)
        {
            dominantWeight = redWeight;
        }
        if (greenWeight >= redWeight && greenWeight >= blueWeight)
        {
            dominantWeight = greenWeight;
        }
        if (blueWeight >= redWeight && blueWeight >= greenWeight)
        {
            dominantWeight = blueWeight;    
        }
        
        if(redWeight != 0)
            redWeight = redWeight / dominantWeight;
        if (greenWeight != 0)
            greenWeight = greenWeight / dominantWeight;
        if (blueWeight != 0)
            blueWeight = blueWeight / dominantWeight;

        // Debug.Log("Red 2: " + redWeight);
        // Debug.Log("Green 2: " + greenWeight);
        // Debug.Log("Blue 2: " + blueWeight);

    }
    void Update(){ 
        curFade -= fadeSpeed;
        curRise += riseSpeed;
        this.transform.position = new Vector3(this.transform.position.x,curRise, this.transform.position.z);
        this.transform.localScale += new Vector3(growSpeed, growSpeed, growSpeed);
        actualSprite.color = new Color(redWeight, greenWeight, blueWeight, curFade);
        if (curFade <= 0) Destroy(gameObject);
    }
}
