using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{

    [SerializeField] string currentTeamHoldingBall;
    [SerializeField] bool isHoldPlayer;
    [SerializeField] GameController gameController;

    [SerializeField] GameObject goalBlue;
    [SerializeField] GameObject goalRed;
    [SerializeField] float distanceBlue;
    [SerializeField] float distanceRed;

    public bool isMove;
    public GameObject target;
    
    void Start()
    {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();

        goalBlue = GameObject.Find("Goal Blue");
        goalRed = GameObject.Find("Goal Red");
    }

    // Update is called once per frame
    void Update()
    {
        distanceBlue = Vector3.Distance(this.transform.position, goalBlue.transform.position);
        distanceRed = Vector3.Distance(this.transform.position, goalRed.transform.position);


        if (distanceBlue < 0.1f)
        {
            Debug.Log("goal");
        }

        if (isMove)
        {
            moveBall(target);
        }

        scoreGoal();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Blue Player")
        {
            Debug.Log("BLUE TEAM GOT BALL");
            currentTeamHoldingBall = "Blue";
            isHoldPlayer = true;
            this.GetComponent<SphereCollider>().enabled = false;
            //Destroy(this.gameObject);
        }
        if (other.transform.name == "Red Player")
        {
            Debug.Log("RED TEAM GOT BALL");
            currentTeamHoldingBall = "Red";
            isHoldPlayer = true;
            this.GetComponent<SphereCollider>().enabled = false;
            //Destroy(this.gameObject);
        }
    }

    public void scoreGoal()
    {
        if (distanceBlue < 0.1f)
        {
            gameController.setMessage(false);
            gameController.setScore(false);
        }
        if (distanceRed < 0.1f)
        {
            gameController.setMessage(true);
            gameController.setScore(true);
        }
    }


    public void moveBall(GameObject targetPass)
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPass.transform.position, gameController.passSpeed * Time.deltaTime);

        if(this.transform.position == targetPass.transform.position)
        {
            this.transform.position = targetPass.transform.Find("Ball Pos").position;

            isMove = false;
        }
    }

    public void setMove(GameObject targetPass)
    {
        target = targetPass;
        isMove = true;
    }
}
