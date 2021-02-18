using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    [SerializeField] private GroupAdder[] activeGroups;
    [SerializeField] private int comboScore, score, highScore;
    [SerializeField] private TextMesh comboScoreText, scoreText, highScoreText;
    [SerializeField] private int[] boardSize, dIsSatisfied;
    [SerializeField] private bool dSatisfiedCount, gameOver, debugTextOver, 
        canSave, isTutorial, showClusterText;
    [SerializeField] private GameObject gameOverText, sparkEffect, scoreObjects;
    [SerializeField] private AudioSource audioSrc;
    [SerializeField] private AudioClip placeSound, finishScoreSound;
    [SerializeField] private AudioClip[] scoreSound;

    private NodeScript[,] nodes, clusters, adjacentClusters;
    private NodeScript[] pointNodes;
    private int[] adjacentNodesWeight;
    private int nodeReq, pointNodeAmmount, pointMultiplier, 
        fromCluster, clusterCount, addNodeAt;
    [SerializeField] private GameObject initialSpace;
    private string debugText;
    [SerializeField] private float pointTimer, pointMaxTimer;
    private bool isScoring;

    public bool checkTutorial(){
        return isTutorial;
    }

    public int getBoardSizeX(){
        return boardSize[0];
    }

    public int getBoardSizeY(){
        return boardSize[1];
    }

    public NodeScript getNode(int x, int y){
        return nodes[x,y];
    }

    public int getNodeId(int x, int y){
        return getNode(x, y).getCurId();
    }

    public void setNode(int x, int y, NodeScript newNode){
        this.nodes[x,y] = newNode;
    }

    public bool checkScoring(){
        return isScoring;
    }

    public void addScore(int score){
        this.score += score;
    }

    public void playPlaceSound(){
        audioSrc.PlayOneShot(placeSound);
    }
    
    public bool checkGameOver(){
        return this.gameOver;
    }

    void Awake()
    {
        gameOverText.SetActive(false);

        highScore = PlayerPrefs.HasKey("highScore") ? PlayerPrefs.GetInt("highScore") : 0;

        score = 0;
        comboScore = 0;
        nodes = new NodeScript[getBoardSizeX(), getBoardSizeY()];
        for (int i = 0; i < getBoardSizeX(); i++)
        {
            for (int j = 0; j < getBoardSizeY(); j++)
            {
                Instantiate(initialSpace, 
                    new Vector3(i - (getBoardSizeX() / 2), 
                                j - (getBoardSizeY() / 2), 
                                0), Quaternion.identity);
            }
        }
    }
    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        if (!isTutorial)
        {
            if (PlayerPrefs.GetInt("CurScore") != 0)
            {
                LoadGame();
                CheckEnd();
            }
            if (gameOver) RestartBoard();
        }
        else
        {
            SetupTutorialBoard();
        }

    }
    public void CheckEnd()
    {
        if (isTutorial) return;
        int curPosCheck = 0;
        bool isOcupied = false;
        dIsSatisfied = new int[3];
        dSatisfiedCount = false;
        for (int i = 0; i < getBoardSizeX(); i++)
        {
            for (int j = 0; j < getBoardSizeY(); j++)
            {
                for (int s = 0; s < 3; s++)
                {
                    int curShape = activeGroups[s].getDice();
                    dIsSatisfied[s] = 0;
                    curPosCheck = 0;
                    for (int k = 0; k < 4; k++)
                    {
                        for (int l = 0; l < 4; l++)
                        {
                            if (i + k < getBoardSizeX() && j + l < getBoardSizeY())
                            {
                                isOcupied = (getNodeId(i + k, j + l) != 0);
                            }
                            else isOcupied = true;

                            
                            if (isOcupied)
                            {
                                switch (curPosCheck)
                                {
                                    case 0:
                                        if (curShape != 4 && curShape != 13 && curShape != 18 && curShape != 21 && curShape != 22 && curShape != 25 && curShape != 26)
                                        {
                                            dIsSatisfied[s] = -1;
                                        }
                                        break;
                                    case 1:
                                        if (curShape != 0 && curShape != 1 && curShape != 3 && curShape != 7 && curShape != 10 && curShape != 14 && curShape != 18
                                            && curShape != 19 && curShape != 20 && curShape != 24)
                                            dIsSatisfied[s] = -1;
                                        break;
                                    case 2:
                                        if (curShape == 8 || curShape == 11 || curShape == 16 || curShape == 17 || curShape == 18 || curShape == 21 || curShape == 27)
                                            dIsSatisfied[s] = -1;
                                        break;
                                    case 3:
                                        if (curShape == 11)
                                            dIsSatisfied[s] = -1;
                                        break;
                                    case 4:
                                        if (curShape != 0 && curShape != 2 && curShape != 5 && curShape != 8 && curShape != 11 && curShape != 12 && curShape != 13
                                            && curShape != 17 && curShape != 23 && curShape != 27)
                                            dIsSatisfied[s] = -1;
                                        break;
                                    case 5:
                                        if (curShape == 3 || curShape == 4 || curShape == 5 || curShape == 9 || curShape == 12 || curShape == 13 || curShape == 18 || curShape == 19 ||
                                            curShape == 20 || curShape == 21 || curShape == 22 || curShape == 23 || curShape == 24 || curShape == 25 || curShape == 26 || curShape == 27)
                                            dIsSatisfied[s] = -1;
                                        break;
                                    case 6:
                                        if (curShape == 17 || curShape == 18 || curShape == 19 || curShape == 23 || curShape == 26)
                                            dIsSatisfied[s] = -1;
                                        break;
                                    case 8:
                                        if (curShape == 7 || curShape == 10 || curShape == 13 || curShape == 14 || curShape == 15 || curShape == 22 || curShape == 24)
                                            dIsSatisfied[s] = -1;
                                        break;
                                    case 9:
                                        if (curShape == 12 || curShape == 13 || curShape == 14 || curShape == 20 || curShape == 25)
                                            dIsSatisfied[s] = -1;
                                        break;
                                    case 12:
                                        if (curShape == 10)
                                            dIsSatisfied[s] = -1;
                                        break;
                                }
                            }
                            if (dIsSatisfied[s] == 0)
                            {
                                switch (curPosCheck)
                                {
                                    case 0:
                                        if (curShape == 0)
                                        {
                                            dSatisfiedCount = true;
                                        }
                                        break;
                                    case 1:
                                        if (curShape == 2)
                                        {
                                            dSatisfiedCount = true;
                                        }
                                        break;
                                    case 2:
                                        if (curShape == 8)
                                        {
                                            dSatisfiedCount = true;
                                        }
                                        break;
                                    case 3:
                                        if (curShape == 11)
                                        {
                                            dSatisfiedCount = true;
                                        }
                                        break;
                                    case 4:
                                        if (curShape == 1 || curShape == 6 || curShape == 16)
                                        {
                                            dSatisfiedCount = true;
                                        }
                                        break;
                                    case 5:
                                        if (curShape == 3 || curShape == 4 || curShape == 5 || curShape == 9 || curShape == 21 || curShape == 27)
                                        {
                                            dSatisfiedCount = true;
                                        }
                                        break;
                                    case 6:
                                        if (curShape == 17 || curShape == 18 || curShape == 19 || curShape == 23 || curShape == 26)
                                        {
                                            dSatisfiedCount = true;
                                        }
                                        break;
                                    case 8:
                                        if (curShape == 7 || curShape == 15 || curShape == 22 || curShape == 24)
                                        {
                                            dSatisfiedCount = true;
                                        }
                                        break;
                                    case 9:
                                        if (curShape == 12 || curShape == 13 || curShape == 14 || curShape == 20 || curShape == 25)
                                        {
                                            dSatisfiedCount = true;
                                        }
                                        break;
                                    case 12:
                                        if (curShape == 10)
                                        {
                                            dSatisfiedCount = true;
                                        }
                                        break;
                                }
                            }
                            curPosCheck++;
                            if (dSatisfiedCount) break;
                            if (dIsSatisfied[s] != 0) break;
                        }
                        if (dSatisfiedCount) break;
                        if (dIsSatisfied[s] != 0) break;
                    }
                    if (dSatisfiedCount) break;
                }
                if (dSatisfiedCount) break;
            }
            if (dSatisfiedCount) break;
        }
        if (!dSatisfiedCount)
        {
            gameOver = true;
            gameOverText.SetActive(true);
            Debug.Log("GAME OVER");
        }
    }
    void SaveGame()
    {
        for (int i = 0; i < activeGroups.Length; i++)
        {
            PlayerPrefs.SetInt("GroupID" + i, activeGroups[i].getDice());
            for (int j = 0; j < activeGroups[i].getActiveGroup().getNodesAmount(); j++)
            {
                PlayerPrefs.SetInt("G" + i + "Node" + j, activeGroups[i].getActiveGroup().getNodeToPlace(j).getNodeId());
            }
        }
        for (int i = 0; i < getBoardSizeX(); i++)
        {
            for (int j = 0; j < getBoardSizeY(); j++)
            {
                PlayerPrefs.SetInt("Node" + i + "x" + j + "y", getNodeId(i, j));
            }
        }
        PlayerPrefs.SetInt("CurScore", score);
        Debug.Log("Saving...");
    }
    void LoadGame()
    {
        for (int i = 0; i < activeGroups.Length; i++)
        {
            activeGroups[i].setLoading(true);
            activeGroups[i].setDice(PlayerPrefs.GetInt("GroupID" + i));
        }
        for (int i = 0; i < getBoardSizeX(); i++)
        {
            for (int j = 0; j < getBoardSizeY(); j++)
            {
                getNode(i, j).ChangeTo((Enums.TileColor)PlayerPrefs.GetInt("Node" + i + "x" + j + "y"));
            }
        }
        score = PlayerPrefs.GetInt("CurScore");
        Debug.Log("Loading...");
    }
    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (canSave)
            {
                SaveGame();
                canSave = false;
            }
        }
        else if (!canSave) canSave = true;
    }
    void Update()
    {
        //Debug
        if (Input.GetKeyDown(KeyCode.I))
        {
            Instantiate(sparkEffect, new Vector3(5 - (getBoardSizeX() / 2), 5 - (getBoardSizeY() / 2), -1), Quaternion.identity);
        }
        //Debug
        if (Input.GetKeyDown(KeyCode.K))
        {
            debugTextOver = true;
            CheckEnd();
        }
        scoreText.text = score.ToString();
        comboScoreText.text = "C.Score: " + comboScore;
        highScoreText.text = highScore.ToString();
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScore", highScore);
        }
        //Debug
        if (!isScoring && Input.GetKeyDown(KeyCode.R))
        {
            for (int i = 0; i < getBoardSizeX(); i++)
            {
                for (int j = 0; j < getBoardSizeY(); j++)
                {
                    int randNode = Random.Range(0, 5);
                    getNode(i, j).ChangeTo((Enums.TileColor) randNode);
                    CheckBoard();
                }
            }
        }
        //Debug
        if (Input.GetKeyDown(KeyCode.O)) RestartBoard();

        //Debug
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckBoard();
            CheckClusterCanScore();
        }
        //Apenas um delay em checks do "CheckClusterCanScoreOneMore" para ver melhor oq está acontecendo e como está acontecendo
        if (isScoring)
        {
            pointTimer += Time.deltaTime;
            if (pointTimer > pointMaxTimer) CheckClusterCanScoreOneMore();
        }
    }
    public void RestartBoard()
    {
        PlayerPrefs.SetInt("CurScore", 0);
        score = 0;
        comboScore = 0;
        for (int i = 0; i < getBoardSizeX(); i++)
        {
            for (int j = 0; j < getBoardSizeY(); j++)
            {
                getNode(i,j).resetNode();
            }
        }
        for (int i = 0; i < activeGroups.Length; i++) activeGroups[i].destroyActiveGroup();

        gameOver = false;
        gameOverText.SetActive(false);
    }
    void AddNode(int i, int j, int clusterID, int clusterPos)
    {
        clusters[clusterID, clusterPos] = getNode(i, j);
        addNodeAt++;
    }
    bool CheckRegistered(int i, int j)
    {
        for (int k = 0; k <= clusterCount; k++)
        {
            for (int l = 0; l < getBoardSizeX() * getBoardSizeY(); l++)
            {
                //Tentei colocar com &&, mas por algum motivo o c# está execultando todas as condicionais
                if (clusters[k, l] != null)
                {
                    if (clusters[k, l] == getNode(i, j))
                    {
                        fromCluster = k;
                        return false;
                    }
                }
                else break;
            }
        }
        return true;
    }
    void CheckNodeBorders(int i, int j)
    {
        if (i >= 0 && i < getBoardSizeX() && j >= 0 && j < getBoardSizeY())
        {
            if (i < getBoardSizeX() - 1 && getNodeId(i, j) == getNodeId(i + 1, j)) CheckSide(i + 1, j);
            if (i > 0 && getNodeId(i, j) == getNodeId(i - 1, j)) CheckSide(i - 1, j);
            if (j < getBoardSizeY() - 1 && getNodeId(i, j) == getNodeId(i, j + 1)) CheckSide(i, j + 1);
            if (j > 0 && getNodeId(i, j) == getNodeId(i, j - 1)) CheckSide(i, j - 1);
        }
    }
    void CheckSide(int i, int j)
    {
        if (CheckRegistered(i, j))
        {
            AddNode(i, j, clusterCount, addNodeAt);
            CheckNodeBorders(i, j);
        }
    }
    public void CheckBoard()
    {
        clusters = new NodeScript[getBoardSizeX() * getBoardSizeY(), getBoardSizeX() * getBoardSizeY()];
        clusterCount = 0;
        addNodeAt = 0;
        nodeReq = 4;
        for (int i = 0; i < getBoardSizeX(); i++)
        {
            for (int j = 0; j < getBoardSizeY(); j++)
            {
                if (getNodeId(i, j) > 0 && CheckRegistered(i, j))
                {
                    AddNode(i, j, clusterCount, addNodeAt);
                    CheckNodeBorders(i, j);
                    clusterCount++;
                }


                addNodeAt = 0;
            }
        }
        if (showClusterText)
        {
            for (int c = 0; c < clusterCount; c++)
            {
                debugText += "Cluster " + c + ": ";
                for (int n = 0; n < getBoardSizeX() * getBoardSizeY(); n++)
                {
                    if (clusters[c, n] != null)
                    {
                        debugText += clusters[c, n].getPosX() + "x " + clusters[c, n].getPosY() + "y / ";
                    }
                    else break;
                }
                debugText += ";\n";
            }
            Debug.Log(debugText);
            debugText = "";
        }
    }
    public void CheckClusterCanScore()
    {

        pointNodes = new NodeScript[getBoardSizeX() * getBoardSizeY()];
        pointMultiplier = 0;
        pointNodeAmmount = 0;
        float red;
        float green;
        float blue;
        float yellow;
        for (int c = 0; c < clusterCount; c++)
        {
            addNodeAt = 0;
            red = 0;
            green = 0;
            blue = 0;
            yellow = 0;

            for (int n = 0; n < getBoardSizeX() * getBoardSizeY(); n++)
            {
                //Debug.Log(clusters);
                if (clusters[c, n] != null)
                {
                    switch (clusters[c, n].getCurId()) {
                        case 1: yellow++; break;
                        case 2: green++; break;
                        case 3: blue++; break;
                        case 4: red++; break;
                    }
                    addNodeAt++;
                }
                else break;
            }
            if (addNodeAt >= nodeReq)
            {
                GameObject curScoreText = Instantiate(scoreObjects, this.transform.position, this.transform.rotation);
                curScoreText.GetComponent<ScoreTextScript>().Setup(red, green, blue, yellow, 0);
                audioSrc.PlayOneShot(scoreSound[0]);
                pointMultiplier++;
                isScoring = true;
                ChangingToPointNodes(c);
            }
        }
    }
    void CheckClusterCanScoreOneMore()
    {
        isScoring = false;
        pointTimer = 0;
        adjacentClusters = new NodeScript[4, getBoardSizeX() * getBoardSizeY()];
        nodeReq++;
        CheckPointBorders();

        float red;
        float green;
        float blue;
        float yellow;

        for (int c = 0; c < 4; c++)
        {
            addNodeAt = 0;
            red = 0;
            green = 0;
            blue = 0;
            yellow = 0;

            for (int n = 0; n < getBoardSizeX() * getBoardSizeY(); n++)
            {
                if (adjacentClusters[c, n] != null)
                {
                    switch (adjacentClusters[c, n].getCurId())
                    {
                        case 1: yellow++; break;
                        case 2: green++; break;
                        case 3: blue++; break;
                        case 4: red++; break;
                    }
                    addNodeAt++;
                }
                else break; 
            }
            if (addNodeAt >= nodeReq)
            {
                if (pointMultiplier < scoreSound.Length) 
                    audioSrc.PlayOneShot(scoreSound[pointMultiplier]);
                else 
                    audioSrc.PlayOneShot(scoreSound[scoreSound.Length - 1]);
                    
                GameObject curScoreText = Instantiate(scoreObjects, this.transform.position, this.transform.rotation);
                curScoreText.GetComponent<ScoreTextScript>().Setup(red, green, blue, yellow, pointMultiplier);
                pointMultiplier++;
                isScoring = true;
                ChangingToPointNodesOneMore(c);
            }
        }
        if (isScoring)
        {
            comboScore = 0;
            for (int d = 0; d < pointNodes.Length; d++)
            {
                if (pointNodes[d] != null) addComboScore();
                else break; 
            }
        }
        if (!isScoring)
        {
            comboScore = 0;
            for (int d = 0; d < pointNodes.Length; d++)
            {
                if (pointNodes[d] != null)
                {
                    addComboScore();
                    GameObject spark = Instantiate(sparkEffect, 
                        new Vector3(pointNodes[d].getPosX() - (getBoardSizeX() / 2), 
                                    pointNodes[d].getPosY() - (getBoardSizeY() / 2),
                                    0), Quaternion.identity);
                    SparkleScript sparkS = spark.GetComponent<SparkleScript>();
                    sparkS.setColorId(pointNodes[d].getLastId());
                    pointNodes[d].ChangeTo(0);
                    pointNodes[d] = null;
                }
                else { break; }
            }
            score += comboScore;
            CheckEnd();
            audioSrc.PlayOneShot(finishScoreSound);
            Debug.Log("CASHING THE FUCK OUT");
        }
    }
    void CheckPointBorders()
    {
        adjacentNodesWeight = new int[4];
        for (int i = 0; i < pointNodes.Length; i++)
        {
            if (pointNodes[i] != null)
            {
                CheckPointBorderOneMore(pointNodes[i].getPosX() + 1, pointNodes[i].getPosY());
                CheckPointBorderOneMore(pointNodes[i].getPosX() - 1, pointNodes[i].getPosY());
                CheckPointBorderOneMore(pointNodes[i].getPosX(), pointNodes[i].getPosY() + 1);
                CheckPointBorderOneMore(pointNodes[i].getPosX(), pointNodes[i].getPosY() - 1);
            }
            else break;
        }
    }
    bool CanAdd(int i, int j)
    {
        for (int l = 0; l < adjacentNodesWeight[getNodeId(i, j) - 1]; l++)
        {
            if (adjacentClusters[getNodeId(i, j) - 1, l] == getNode(i, j)) return false;
        }
        return true;
    }
    void CheckPointBorderOneMore(int i, int j)
    {
        if (i >= 0 && 
            j >= 0 && 
            i < getBoardSizeX() && 
            j < getBoardSizeY() && 
            getNodeId(i, j) > 0 && 
            getNodeId(i, j) < 5 && 
            !CheckRegistered(i, j) &&
            CanAdd(i, j))
        {
            for (int y = 0; y < getBoardSizeX() * getBoardSizeY(); y++)
            {
                if (clusters[fromCluster, y] != null)
                {
                    adjacentClusters[getNodeId(i, j) - 1, adjacentNodesWeight[getNodeId(i, j) - 1]] = clusters[fromCluster, y];
                    adjacentNodesWeight[getNodeId(i, j) - 1]++;
                }
                else break;
            }
        }
    }
    void ChangingToPointNodesOneMore(int c)
    {
        for (int n = 0; n < getBoardSizeX() * getBoardSizeY(); n++)
        {
            if (adjacentClusters[c, n] != null)
            {
                adjacentClusters[c, n].ChangeTo(Enums.TileColor.White);
                pointNodes[pointNodeAmmount] = adjacentClusters[c, n];
                pointNodeAmmount++;
            }
            else break;
        }
    }
    void ChangingToPointNodes(int c)
    {
        for (int n = 0; n < getBoardSizeX() * getBoardSizeY(); n++)
        {
            if (clusters[c, n] != null)
            {
                clusters[c, n].ChangeTo(Enums.TileColor.White);
                pointNodes[pointNodeAmmount] = clusters[c, n];
                pointNodeAmmount++;
            }
            else break;
        }
        if (isScoring)
        {
            comboScore = 0;
            for (int d = 0; d < pointNodes.Length; d++)
            {
                if(pointNodes[d] != null) addComboScore();
                else break;
            }
        }
    }

    private void addComboScore(){
        comboScore += 20 * pointMultiplier;
    }

    private void alterNode(int x, int y, Enums.TileColor target){
        getNode(x, y).ChangeTo(target);
    }

    void SetupTutorialBoard()
    {
        //#!!#
        alterNode(1, 8, Enums.TileColor.Yellow);
        alterNode(4, 8, Enums.TileColor.Yellow);
        alterNode(7, 9, Enums.TileColor.Yellow);
        alterNode(8, 9, Enums.TileColor.Yellow);
        alterNode(9, 8, Enums.TileColor.Yellow);
        alterNode(7, 6, Enums.TileColor.Yellow);
        alterNode(9, 0, Enums.TileColor.Yellow);
        alterNode(8, 1, Enums.TileColor.Yellow);
        alterNode(7, 2, Enums.TileColor.Yellow);
        alterNode(8, 2, Enums.TileColor.Yellow);
        alterNode(3, 2, Enums.TileColor.Yellow);
        alterNode(5, 3, Enums.TileColor.Yellow);
        alterNode(6, 3, Enums.TileColor.Yellow);
        alterNode(7, 2, Enums.TileColor.Yellow);

        alterNode(5, 2, Enums.TileColor.Green);
        alterNode(7, 7, Enums.TileColor.Green);
        alterNode(2, 0, Enums.TileColor.Green);
        alterNode(8, 0, Enums.TileColor.Green);
        alterNode(3, 1, Enums.TileColor.Green);
        alterNode(7, 1, Enums.TileColor.Green);

        alterNode(6, 1, Enums.TileColor.Blue);
        alterNode(7, 0, Enums.TileColor.Blue);
        alterNode(6, 2, Enums.TileColor.Blue);

        alterNode(5, 7, Enums.TileColor.Red);
        alterNode(6, 7, Enums.TileColor.Red);
        alterNode(8, 6, Enums.TileColor.Red);
        alterNode(8, 5, Enums.TileColor.Red);
        alterNode(8, 4, Enums.TileColor.Red);
        alterNode(0, 0, Enums.TileColor.Red);
        alterNode(1, 0, Enums.TileColor.Red);
        alterNode(5, 0, Enums.TileColor.Red);
        alterNode(6, 0, Enums.TileColor.Red);
        alterNode(5, 1, Enums.TileColor.Red);
        alterNode(9, 1, Enums.TileColor.Red);
        alterNode(2, 2, Enums.TileColor.Red);
        alterNode(9, 2, Enums.TileColor.Red);
        alterNode(2, 3, Enums.TileColor.Red);
        alterNode(9, 3, Enums.TileColor.Red);
        alterNode(2, 4, Enums.TileColor.Red);
        alterNode(5, 4, Enums.TileColor.Red);
        alterNode(4, 5, Enums.TileColor.Red);
        alterNode(5, 5, Enums.TileColor.Red);
    }
}
