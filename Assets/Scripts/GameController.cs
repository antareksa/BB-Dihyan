using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float secondPerRound;
    private float time;
    [SerializeField] private bool onProgress = false;
    [SerializeField] private Text timeUI;
    [SerializeField] public int round;

    [Header("User Interface Settings")]

    [SerializeField] private GameObject gameUserInterface;

    [SerializeField] private Text blueTeamText;
    [SerializeField] private Text redTeamText;

    [SerializeField] private Text blueTeamScoreText;
    [SerializeField] private Text redTeamScoreText;

    [SerializeField] private Text roundText;

    [SerializeField] private GameObject finishUI;
    [SerializeField] private Text blueFinishText;
    [SerializeField] private Text redFinishText;

    [SerializeField] private GameObject roundFinishUI;
    [SerializeField] private Button readyButtonBlue;
    [SerializeField] private Button readyButtonRed;

    [Header("Game Object Settings")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject touchPrevent;
    

    [Header("Speed Settings")]
    [SerializeField] public float defenderSpeed;
    [SerializeField] public float attackerSpeed;
    [SerializeField] public float bringBallSpeed;
    [SerializeField] public float returnSpeed;
    [SerializeField] public float passSpeed;

    [Header("Team Settings")]
    [SerializeField] public List<GameObject> listBluePlayer = new List<GameObject>();

    [SerializeField] public List<GameObject> listRedPlayer = new List<GameObject>();

    [SerializeField] public EnergyBarController energyBlue;
    [SerializeField] public EnergyBarController energyRed;

    [SerializeField] public bool attackBlue;
    [SerializeField] public bool attackRed;

    [SerializeField] public int scoreBlue;
    [SerializeField] public int scoreRed;

    [SerializeField] private Text objectiveBlue;
    [SerializeField] private Text objectiveRed;

    [SerializeField] private Material materialBlue;
    [SerializeField] private Material materialRed;

    [SerializeField] public bool blueHaveBall;
    [SerializeField] public bool redHaveBall;

    [SerializeField] public bool blueReady;
    [SerializeField] public bool redReady;



    private void OnEnable()
    {
        round = 0;
        energyBlue.resetEnergy();
        energyRed.resetEnergy();
        time = secondPerRound;
    }

    private void OnDisable()
    {
        round = 0;
    }

    private void Start()
    {
         
        if(!attackBlue && !attackRed)
        {
            int randomAttack = Random.Range(0, 2);
            Debug.Log(randomAttack);
            if (randomAttack == 0)
            {
                attackBlue = true;
            }
            else
            {
                attackRed = true;
            }
        }

        //startGame();
        setObjective();
        onProgress = true;
    }

    void Update()
    {
        if (blueReady && redReady)
        {
            blueReady = false;
            redReady = false;

            startGame();
        }


        if (onProgress)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                DisplayTime(time);
            }
            else
            {
                Debug.Log("Time has run out!");
                time = 0;
                onProgress = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if(hitInfo.transform.name == "Blue Area")
                {

                    if((attackBlue && energyBlue.totalEnergy >= 2) || (!attackBlue && energyBlue.totalEnergy >=3))
                    {
                        Vector3 _pos = new Vector3(hitInfo.point.x, 0.5f, hitInfo.point.z);
                        GameObject newPlayer = Instantiate(playerPrefab, _pos, Quaternion.identity);
                        //newPlayer.GetComponent<MeshRenderer>().material = materialBlue;
                        newPlayer.transform.name = "Blue Player";
                        listBluePlayer.Add(newPlayer);

                        if (attackBlue)
                        {
                            energyBlue.reduceEnergy(2);
                        }
                        else
                        {
                            energyBlue.reduceEnergy(3);
                        }
                    }
                    else
                    {
                        //DONT HAVE ENERGY BLUE TEAM
                    }
                }
                if (hitInfo.transform.name == "Red Area")
                {

                    if ((attackRed && energyRed.totalEnergy >= 2) || (!attackRed && energyRed.totalEnergy >= 3))
                    {
                        Vector3 _pos = new Vector3(hitInfo.point.x, 0.5f, hitInfo.point.z);
                        GameObject newPlayer = Instantiate(playerPrefab, _pos, Quaternion.Euler(0,180,0));
                        //newPlayer.GetComponent<MeshRenderer>().material = materialBlue;
                        newPlayer.transform.name = "Red Player";
                        listRedPlayer.Add(newPlayer);

                        if (attackRed)
                        {
                            energyRed.reduceEnergy(2);
                        }
                        else
                        {
                            energyRed.reduceEnergy(3);
                        }

                    }
                    else
                    {
                        //DONT HAVE ENERGY RED TEAM
                    }
                }
            }
        }

        
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void setObjective()
    {
        if (attackRed)
        {
            attackRed = false;
            attackBlue = true;
        }
        else
        {
            attackRed = true;
            attackBlue = false;
        }


        if (attackBlue)
        {
            objectiveBlue.text = "ATTACK";
            objectiveRed.text = "DEFENSE";
        }
        else
        {
            objectiveBlue.text = "DEFENSE";
            objectiveRed.text = "ATTACK";
        }

        
    }

    public void startGame()
    {
        round++;               

        

        
        energyBlue.resetEnergy();
        energyRed.resetEnergy();

        roundText.text = "ROUND " + round;

        time = secondPerRound;
        onProgress = true;

        roundFinishUI.SetActive(false);
        touchPrevent.SetActive(false);

        blueHaveBall = false;
        redHaveBall = false;

        setObjective();

        GameObject newBall = Instantiate(ball, randomPositionBall(), Quaternion.identity);
        newBall.name = "Ball";

        
        
    }

    public Vector3 randomPositionBall()
    {
        int randomX = Random.Range(-4, 5);
        int randomZ = Random.Range(1, 10);
        if (attackRed)
        {
            randomZ = randomZ * 1;
        }
        else
        {
            randomZ = randomZ * -1;
        }

        Vector3 ballPosition = new Vector3(randomX, 0.5f, randomZ);
        return ballPosition;
    }


    public void defenderScore()
    {
        if (attackBlue)
        {
            setMessage(false);
            setScore(false);
        }
        else
        {
            setMessage(true);
            setScore(true);
        }
    }


    

    public void setMessage(bool isBlueScore)
    {
        if (isBlueScore)
        {
            blueTeamText.text = "BLUE TEAM SCORE";
            redTeamText.text = "BLUE TEAM SCORE";
        }
        else
        {
            blueTeamText.text = "RED TEAM SCORE";
            redTeamText.text = "RED TEAM SCORE";
        }
    }

    public void setMessageReady(bool isBlue)
    {
        if (isBlue)
        {
            blueTeamText.text = "WAIT FOR RED TEAM";
        }
        else
        {
            redTeamText.text = "WAIT FOR BLUE TEAM";
        }
    }


    public void setScore(bool isBlueScore)
    {
        if (isBlueScore)
        {
            scoreBlue++;
            blueTeamScoreText.text = scoreBlue.ToString();
            nextRound();
        }
        else
        {
            scoreRed++;
            redTeamScoreText.text = scoreRed.ToString();
            nextRound();
        }
    }

    public void destroyAllPlayer()
    {
        foreach (GameObject player in listBluePlayer)
        {
            Destroy(player);
        }

        foreach (GameObject player in listRedPlayer)
        {
            Destroy(player);
        }

        listBluePlayer.Clear();
        listRedPlayer.Clear();
    }


    private void nextRound()
    {
        destroyAllPlayer();

        if (round == 5)
        {
            finishUI.SetActive(true);

            if (scoreBlue > scoreRed)
            {
                blueFinishText.text = "BLUE TEAM WIN";
                redFinishText.text = "BLUE TEAM WIN";


            }
            else
            {
                blueFinishText.text = "RED TEAM WIN";
                redFinishText.text = "RED TEAM WIN";
            }

            gameUserInterface.SetActive(false);
            touchPrevent.SetActive(true);
            return;
        }

        onProgress = false;
        touchPrevent.SetActive(true);
        roundFinishUI.SetActive(true);
        readyButtonBlue.gameObject.SetActive(true);
        readyButtonRed.gameObject.SetActive(true);
    }


    public void setReady(string team)
    {
        if(team == "Blue")
        {
            blueReady = true;
            setMessageReady(true);
            readyButtonBlue.gameObject.SetActive(false);
        }
        else
        {
            redReady = true;
            setMessageReady(false);
            readyButtonRed.gameObject.SetActive(false);
        }
    }
}
