using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverallGameManager : MonoBehaviour {

    [SerializeField] private bool canInteractMenu;
    private bool isMute;
    private bool isColorBlind;
    private static OverallGameManager ogm;
    [SerializeField] private int nodeGroupsPlaced = 0;

    [SerializeField] private List<ShapeOdd> shapeOdds; 

    [System.Serializable]private class ShapeOdd{
        private float curWeight;
        [SerializeField] private string name;
        private int shapeId;
        [SerializeField] private float initialWeight;
        [SerializeField] private float dificultyMultiplier;

        public float getCurWeight(int nodeGroupsPlaced){
            return initialWeight + (dificultyMultiplier * nodeGroupsPlaced);
        }

        public void setShapeId(int id){
            this.shapeId = id;
        }
        public int getShapeId(){
            return this.shapeId;
        }
    }

    public int getNodeGroupsPlaced(){
        return this.nodeGroupsPlaced;
    }

    public void setNodeGroupsPlaced(int nodeGroupsPlaced){
        this.nodeGroupsPlaced = nodeGroupsPlaced;
    }

    public int generateRandomNodeShapeId(){
        this.nodeGroupsPlaced ++;

        float totalWeight = 0;
        shapeOdds.ForEach(delegate(ShapeOdd shape){
            totalWeight += shape.getCurWeight(nodeGroupsPlaced);
        });
        float shapeSeed = Random.Range(0, totalWeight);
        float curWeight = 0;

        foreach(ShapeOdd shape in shapeOdds){
            curWeight += shape.getCurWeight(nodeGroupsPlaced);
            if(curWeight >= shapeSeed){
                Debug.Log("NodeGroupsPlaced: " + nodeGroupsPlaced + " Total Weight: " + totalWeight + " ShapeSeed: " + shapeSeed + " CurWeight: " + curWeight + " ChosenShape: " + shape.getShapeId());
                return shape.getShapeId();
            } 
        }
        Debug.Log("Something went wrong in ");

        return shapeOdds[shapeOdds.Count - 1].getShapeId();
    }

    public bool checkMute(){
        return this.isMute;
    }

    public void setMute(bool mute){
        this.isMute = mute;
    }

    public bool checkColorBlind(){
        return this.isColorBlind;
    }

    public void setColorBlind(bool colorBlind){
        this.isColorBlind = colorBlind;
    }

    public bool checkInteractMenu(){
        return this.canInteractMenu;
    }

    public void setInteractMenu(bool interactMenu){
        this.canInteractMenu = interactMenu;
    }

    public void toggleMute(){
        this.isMute = !this.isMute;
    }

    public void toggleColorblind(){
        this.isColorBlind = !this.isColorBlind;
    }

    private void Awake()
    {
        if (ogm == null)
		{
			DontDestroyOnLoad (gameObject);
			ogm = this;
		}
		else if (ogm != this)
		{
			Destroy (gameObject);
		}
    }

    private void setupShapeIds(){
        for(int i = 0 ;  i < shapeOdds.Count ; i++){
            shapeOdds[i].setShapeId(i);
        }
    }

    private void Start(){
        setupShapeIds();
    }

	void Update () {
        AudioListener.volume = isMute ? 0 : 1;	
	}

    public void backToMainMenu(){
        Time.timeScale = 1f;
        Debug.Log("Going back to main menu");
        SceneManager.LoadScene(0);
    }
}

public static class Enums {
    public enum TileColor{
        Empty,
        Yellow,
        Green,
        Blue,
        Red,
        White
    }
} 
