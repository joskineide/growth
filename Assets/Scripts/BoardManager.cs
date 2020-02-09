using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    public GroupAdder[] activeGroups;
    public int comboScore, score, highScore;
    public TextMesh comboScoreText, scoreText, highScoreText;
    public int[] boardSize;
    public bool showClusterText;
    [HideInInspector]
    public NodeScript[,] nodes, clusters, adjacentClusters;
    [HideInInspector]
    public NodeScript[] pointNodes;
    [HideInInspector]
    public int[] adjacentNodesWeight;
    [HideInInspector]
    public int nodeReq, pointNodeAmmount, pointMultiplier, fromCluster;
    [HideInInspector]
    public GameObject initialSpace;
    [HideInInspector]
    public int clusterCount;
    [HideInInspector]
    public int addNodeAt;
    [HideInInspector]
    public string debugText;
    [HideInInspector]
    public float pointTimer, pointMaxTimer;
    [HideInInspector]
    public bool isScoring;
    public int[] dIsSatisfied;
    public bool dSatisfiedCount, gameOver, debugTextOver, canSave, isTutorial;
    public GameObject gameOverText, sparkEffect;
    public AudioSource audioSrc;
    public AudioClip placeSound, finishScoreSound;
    public AudioClip[] scoreSound;
    public GameObject scoreObjects;

    void Awake()
    {
        gameOverText.SetActive(false);
        if (PlayerPrefs.HasKey("highScore"))
        {
            highScore = PlayerPrefs.GetInt("highScore");
        }
        else
            highScore = 0;
        score = 0;
        comboScore = 0;
        nodes = new NodeScript[boardSize[0], boardSize[1]];
        for (int i = 0; i < boardSize[0]; i++)
        {
            for (int j = 0; j < boardSize[1]; j++)
            {
                Instantiate(initialSpace, new Vector3(i - (boardSize[0] / 2), j - (boardSize[1] / 2), 0), Quaternion.identity);
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
            if (gameOver)
            {
                RestartBoard();
            }
        }
        else
        {
            SetupTutorialBoard();
        }

    }
    public void CheckEnd()
    {
        if (isTutorial)
        {
            return;
        }
        int curPosCheck = 0;
        bool isOcupied = false;
        dIsSatisfied = new int[3];
        dSatisfiedCount = false;
        for (int i = 0; i < boardSize[0]; i++)
        {
            for (int j = 0; j < boardSize[1]; j++)
            {
                for (int s = 0; s < 3; s++)
                {
                    int curShape = activeGroups[s].dice;
                    dIsSatisfied[s] = 0;
                    curPosCheck = 0;
                    for (int k = 0; k < 4; k++)
                    {
                        for (int l = 0; l < 4; l++)
                        {
                            if (i + k < boardSize[0] && j + l < boardSize[1])
                            {
                                isOcupied = (nodes[i + k, j + l].curId != 0);
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
            PlayerPrefs.SetInt("GroupID" + i, activeGroups[i].dice);
            for (int j = 0; j < activeGroups[i].activeGroup.GetComponent<GroupNoteToPlace>().ntp.Length; j++)
            {
                PlayerPrefs.SetInt("G" + i + "Node" + j, activeGroups[i].activeGroup.GetComponent<GroupNoteToPlace>().ntp[j].nodeID);
            }
        }
        for (int i = 0; i < boardSize[0]; i++)
        {
            for (int j = 0; j < boardSize[1]; j++)
            {
                PlayerPrefs.SetInt("Node" + i + "x" + j + "y", nodes[i, j].curId);
            }
        }
        PlayerPrefs.SetInt("CurScore", score);
        Debug.Log("Saving...");
    }
    void LoadGame()
    {
        for (int i = 0; i < activeGroups.Length; i++)
        {
            activeGroups[i].isLoading = true;
            activeGroups[i].dice = PlayerPrefs.GetInt("GroupID" + i);
        }
        for (int i = 0; i < boardSize[0]; i++)
        {
            for (int j = 0; j < boardSize[1]; j++)
            {
                nodes[i, j].ChangeTo(PlayerPrefs.GetInt("Node" + i + "x" + j + "y"));
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
        else if (!canSave)
        {
            canSave = true;
        }
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            Instantiate(sparkEffect, new Vector3(5 - (boardSize[0] / 2), 5 - (boardSize[1] / 2), -1), Quaternion.identity);
        }

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
        if (!isScoring && Input.GetKeyDown(KeyCode.R))
        {
            for (int i = 0; i < boardSize[0]; i++)
            {
                for (int j = 0; j < boardSize[1]; j++)
                {
                    int randNode = Random.Range(0, 5);
                    nodes[i, j].ChangeTo(randNode);
                    CheckBoard();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            RestartBoard();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckBoard();
            CheckClusterCanScore();
        }
        //Apenas um delay em checks do "CheckClusterCanScoreOneMore" para ver melhor oq está acontecendo e como está acontecendo
        if (isScoring)
        {
            pointTimer += Time.deltaTime;
            if (pointTimer > pointMaxTimer)
            {
                CheckClusterCanScoreOneMore();
            }
        }
    }
    public void RestartBoard()
    {
        PlayerPrefs.SetInt("CurScore", 0);
        score = 0;
        comboScore = 0;
        for (int i = 0; i < boardSize[0]; i++)
        {
            for (int j = 0; j < boardSize[1]; j++)
            {
                nodes[i, j].ChangeTo(0);
                nodes[i, j].anim.SetBool("hardResset", true);
            }
        }
        for (int i = 0; i < activeGroups.Length; i++)
        {
            Destroy(activeGroups[i].activeGroup.gameObject);
        }
        gameOver = false;
        gameOverText.SetActive(false);
    }
    void AddNode(int i, int j, int clusterID, int clusterPos)
    {
        clusters[clusterID, clusterPos] = nodes[i, j];
        addNodeAt++;
    }
    bool CheckRegistered(int i, int j)
    {
        for (int k = 0; k <= clusterCount; k++)
        {
            for (int l = 0; l < boardSize[0] * boardSize[1]; l++)
            {
                if (clusters[k, l] != null)
                {
                    if (clusters[k, l] == nodes[i, j])
                    {
                        fromCluster = k;
                        return false;
                    }
                }
                else { break; }
            }
        }
        return true;
    }
    void CheckNodeBorders(int i, int j)
    {
        if (i >= 0 && i < boardSize[0] && j >= 0 && j < boardSize[1])
        {
            if (i < boardSize[0] - 1 && nodes[i, j].curId == nodes[i + 1, j].curId)
            {
                CheckSide(i + 1, j);
            }
            if (i > 0 && nodes[i, j].curId == nodes[i - 1, j].curId)
            {
                CheckSide(i - 1, j);
            }
            if (j < boardSize[1] - 1 && nodes[i, j].curId == nodes[i, j + 1].curId)
            {
                CheckSide(i, j + 1);
            }
            if (j > 0 && nodes[i, j].curId == nodes[i, j - 1].curId)
            {
                CheckSide(i, j - 1);
            }
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
        clusters = new NodeScript[boardSize[0] * boardSize[1], boardSize[0] * boardSize[1]];
        clusterCount = 0;
        addNodeAt = 0;
        nodeReq = 4;
        for (int i = 0; i < boardSize[0]; i++)
        {
            for (int j = 0; j < boardSize[1]; j++)
            {
                if (nodes[i, j].curId > 0)
                {
                    if (CheckRegistered(i, j))
                    {
                        AddNode(i, j, clusterCount, addNodeAt);
                        CheckNodeBorders(i, j);
                        clusterCount++;
                    }
                }
                addNodeAt = 0;
            }
        }
        if (showClusterText)
        {
            for (int c = 0; c < clusterCount; c++)
            {
                debugText += "Cluster " + c + ": ";
                for (int n = 0; n < boardSize[0] * boardSize[1]; n++)
                {
                    if (clusters[c, n] != null)
                    {
                        debugText += clusters[c, n].nodePos[0] + "x " + clusters[c, n].nodePos[1] + "y / ";
                    }
                    else { break; }
                }
                debugText += ";\n";
            }
            Debug.Log(debugText);
            debugText = "";
        }
    }
    public void CheckClusterCanScore()
    {

        pointNodes = new NodeScript[boardSize[0] * boardSize[1]];
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

            for (int n = 0; n < boardSize[0] * boardSize[1]; n++)
            {
                //Debug.Log(clusters);
                if (clusters[c, n] != null)
                {
                    switch (clusters[c, n].curId) {
                        case 1: yellow++; break;
                        case 2: green++; break;
                        case 3: blue++; break;
                        case 4: red++; break;
                    }
                    addNodeAt++;
                }
                else { break; }
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
        adjacentClusters = new NodeScript[4, boardSize[0] * boardSize[1]];
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

            for (int n = 0; n < boardSize[0] * boardSize[1]; n++)
            {
                if (adjacentClusters[c, n] != null)
                {
                    switch (adjacentClusters[c, n].curId)
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
                if (pointNodes[d] != null)
                {
                    comboScore += 20 * pointMultiplier;
                }
                else { break; }
            }
        }
        if (!isScoring)
        {
            comboScore = 0;
            //Fazer toda a parte de score
            for (int d = 0; d < pointNodes.Length; d++)
            {
                if (pointNodes[d] != null)
                {
                    comboScore += 20 * pointMultiplier;
                    //Debug.Log("Posição x:"+pointNodes[d].nodePos[0]+" y:"+pointNodes[d].nodePos[1]);
                    //Debug.Log("Id atual: "+pointNodes[d].curId+" Id passado:"+pointNodes[d].lastId);
                    GameObject spark = Instantiate(sparkEffect, new Vector3(pointNodes[d].nodePos[0] - (boardSize[0] / 2), pointNodes[d].nodePos[1] - (boardSize[1] / 2), 0), Quaternion.identity);
                    SparkleScript sparkS = spark.GetComponent<SparkleScript>();
                    sparkS.colorId = pointNodes[d].lastId;
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
                CheckPointBorderOneMore(pointNodes[i].nodePos[0] + 1, pointNodes[i].nodePos[1]);
                CheckPointBorderOneMore(pointNodes[i].nodePos[0] - 1, pointNodes[i].nodePos[1]);
                CheckPointBorderOneMore(pointNodes[i].nodePos[0], pointNodes[i].nodePos[1] + 1);
                CheckPointBorderOneMore(pointNodes[i].nodePos[0], pointNodes[i].nodePos[1] - 1);
            }
            else { break; }
        }
    }
    bool CanAdd(int i, int j)
    {
        for (int l = 0; l < adjacentNodesWeight[nodes[i, j].curId - 1]; l++)
        {
            if (adjacentClusters[nodes[i, j].curId - 1, l] == nodes[i, j])
            {
                return false;
            }
        }
        return true;
    }
    void CheckPointBorderOneMore(int i, int j)
    {
        if (i >= 0 && j >= 0 && i < boardSize[0] && j < boardSize[1] && nodes[i, j].curId > 0 && nodes[i, j].curId < 5)
        {
            if (!CheckRegistered(i, j))
            {
                if (CanAdd(i, j))
                {
                    for (int y = 0; y < boardSize[0] * boardSize[1]; y++)
                    {
                        if (clusters[fromCluster, y] != null)
                        {
                            adjacentClusters[nodes[i, j].curId - 1, adjacentNodesWeight[nodes[i, j].curId - 1]] = clusters[fromCluster, y];
                            adjacentNodesWeight[nodes[i, j].curId - 1]++;
                        }
                        else { break; }
                    }
                }
            }
        }
    }
    void ChangingToPointNodesOneMore(int c)
    {
        for (int n = 0; n < boardSize[0] * boardSize[1]; n++)
        {
            if (adjacentClusters[c, n] != null)
            {
                adjacentClusters[c, n].ChangeTo(5);
                pointNodes[pointNodeAmmount] = adjacentClusters[c, n];
                pointNodeAmmount++;
            }
            else { break; }
        }
    }
    void ChangingToPointNodes(int c)
    {
        for (int n = 0; n < boardSize[0] * boardSize[1]; n++)
        {
            if (clusters[c, n] != null)
            {
                clusters[c, n].ChangeTo(5);
                pointNodes[pointNodeAmmount] = clusters[c, n];
                pointNodeAmmount++;
            }
            else { break; }
        }
        if (isScoring)
        {
            comboScore = 0;
            for (int d = 0; d < pointNodes.Length; d++)
            {
                if (pointNodes[d] != null)
                {
                    comboScore += 20 * pointMultiplier;
                }
                else { break; }
            }
        }
    }

    void SetupTutorialBoard()
    {
        //#!!#
        nodes[1, 8].ChangeTo(1);
        nodes[4, 8].ChangeTo(1);
        nodes[7, 9].ChangeTo(1);
        nodes[8, 9].ChangeTo(1);
        nodes[9, 8].ChangeTo(1);
        nodes[7, 6].ChangeTo(1);
        nodes[7, 7].ChangeTo(2);
        nodes[5, 7].ChangeTo(4);
        nodes[6, 7].ChangeTo(4);
        nodes[8, 6].ChangeTo(4);
        nodes[8, 5].ChangeTo(4);
        nodes[8, 4].ChangeTo(4);
        nodes[0, 0].ChangeTo(4);
        nodes[1, 0].ChangeTo(4);
        nodes[2, 0].ChangeTo(2);
        nodes[5, 0].ChangeTo(4);
        nodes[6, 0].ChangeTo(4);
        nodes[7, 0].ChangeTo(3);
        nodes[8, 0].ChangeTo(2);
        nodes[9, 0].ChangeTo(1);
        nodes[3, 1].ChangeTo(2);
        nodes[5, 1].ChangeTo(4);
        nodes[6, 1].ChangeTo(3);
        nodes[7, 1].ChangeTo(2);
        nodes[8, 1].ChangeTo(1);
        nodes[9, 1].ChangeTo(4);
        nodes[2, 2].ChangeTo(4);
        nodes[3, 2].ChangeTo(1);
        nodes[5, 2].ChangeTo(2);
        nodes[6, 2].ChangeTo(3);
        nodes[7, 2].ChangeTo(1);
        nodes[8, 2].ChangeTo(1);
        nodes[9, 2].ChangeTo(4);
        nodes[2, 3].ChangeTo(4);
        nodes[5, 3].ChangeTo(1);
        nodes[6, 3].ChangeTo(1);
        nodes[9, 3].ChangeTo(4);
        nodes[2, 4].ChangeTo(4);
        nodes[5, 4].ChangeTo(4);
        nodes[4, 5].ChangeTo(4);
        nodes[5, 5].ChangeTo(4);

    }
}
