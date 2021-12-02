using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerBehaviour : MonoBehaviour
{
    
    [SerializeField] private GameController gameController;
    [SerializeField] private string teamColor;
    [SerializeField] public bool isAttack;
    [SerializeField] private bool isDisable;
    [SerializeField] private bool isAppear;

    [SerializeField] private bool haveBall;
    [SerializeField] private bool teamHaveBall;

    //[SerializeField] SkinnedMeshRenderer skinMeshRenderer;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material blueTeamMaterial;
    [SerializeField] Material blueTeamDisableMaterial;
    [SerializeField] Material redTeamMaterial;
    [SerializeField] Material redTeamDisableMaterial;

    [SerializeField] GameObject rangeDetection;
    [SerializeField] float rangeDetectionDistance;


    [SerializeField] float speedMovement;
    [SerializeField] float speedRotation;
    [SerializeField] public float defenderSpeed;
    [SerializeField] public float attackerSpeed;
    [SerializeField] public float bringBallSpeed;
    [SerializeField] public float returnSpeed;
    [SerializeField] public float passSpeed;


    [SerializeField] Vector3 originalPosition;
    [SerializeField] Transform ballPosition;

    //[SerializeField] Animator animator;

    //State Player
    [SerializeField] private bool isDie = false;


    private float timeDissolve;
    private float timeAppear = 1;
    private GameObject ball;
    public GameObject goalBlue;
    public GameObject goalRed;
    public NavMeshAgent agent;
    
    void Start()
    {

        ball = GameObject.Find("Ball");
        goalBlue = GameObject.Find("Goal Blue");
        goalRed = GameObject.Find("Goal Red");

        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();

        defenderSpeed = gameController.defenderSpeed;
        attackerSpeed = gameController.attackerSpeed;
        bringBallSpeed = gameController.bringBallSpeed;
        returnSpeed = gameController.returnSpeed;

        agent = this.GetComponent<NavMeshAgent>();

        originalPosition = this.transform.position;
        

        if(this.transform.name == "Blue Player" )
        {
            teamColor = "Blue";
            //skinMeshRenderer.material = blueTeamMaterial;
            meshRenderer.material = blueTeamMaterial;
            if (gameController.attackBlue)
            {
                isAttack = true;
                teamHaveBall = gameController.blueHaveBall;
            }
            else
            {
                isAttack = false;
            }

        }
        else
        {
            teamColor = "Red";
            //skinMeshRenderer.material = redTeamMaterial;
            meshRenderer.material = redTeamMaterial;
            if (gameController.attackRed)
            {
                isAttack = true;
                teamHaveBall = gameController.redHaveBall;
            }
            else
            {
                isAttack = false;
            }
        }

        //line = gameObject.GetComponent<LineRenderer>();



        //speedRotation = 1.5f;
        speedMovement = 1.5f;


        this.GetComponent<Renderer>().material.SetFloat("Vector1_A8DBC554", 1);
        //appear();
        isAppear = true;
    }

    
    void Update()
    {
        if (isAppear)
        {
            appear();
        }

        if (isAttack)
        {
            attacking();
            
        }
        else if (isDie)
        {
            die();
        }
        else
        {
            defending();
        }

        if (isDisable)
        {
            disablePlayer();
        }

        
    }

   

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("MASUK");
    //    if(collision.transform.name == "Ball")
    //    {
    //        if(teamColor == "Blue")
    //        {
    //            gameController.blueHaveBall = true;
    //            foreach (GameObject player in gameController.listBluePlayer)
    //            {
    //                player.GetComponent<PlayerBehaviour>().teamHaveBall = true;
    //            }
    //        }
    //        else
    //        {
    //            gameController.redHaveBall = true;
    //            foreach (GameObject player in gameController.listRedPlayer)
    //            {
    //                player.GetComponent<PlayerBehaviour>().teamHaveBall = true;
    //            }
    //        }

    //        haveBall = true;

    //        Debug.Log("get ball");

    //        Destroy(ball);
            
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Ball" && isAttack)
        {
            if (teamColor == "Blue")
            {
                gameController.blueHaveBall = true;
                foreach (GameObject player in gameController.listBluePlayer)
                {
                    player.GetComponent<PlayerBehaviour>().teamHaveBall = true;
                }
            }
            else
            {
                gameController.redHaveBall = true;
                foreach (GameObject player in gameController.listRedPlayer)
                {
                    player.GetComponent<PlayerBehaviour>().teamHaveBall = true;
                }
            }
            haveBall = true;


            ball.transform.SetParent(this.transform);
            ball.transform.position = ballPosition.transform.position;

        }


        if(other.transform.name == "Fence Red" && teamColor == "Blue")
        {
            isDie = true;
            isAttack = false;
        }

        if (other.transform.name == "Fence Blue" && teamColor == "Red")
        {
            isDie = true;
            isAttack = false;
        }


        if(other.transform.CompareTag("Player") && other.GetComponent<PlayerBehaviour>().teamColor != this.teamColor)
        {
            if (haveBall && isAttack)
            {
                isDisable = true;
                Debug.Log("HOI");
            }

            if(!haveBall && !isAttack)
            {
                if (other.GetComponent<PlayerBehaviour>().haveBall == true)
                {
                    isDisable = true;
                    Debug.Log(this.gameObject.name + " HOII");
                }
                
            }

        }

        if(other == goalRed && this.teamColor == "Red")
        {
            Debug.Log("BLUE SCORE 1");
        }
       
    }


    public void attacking()
    {
        //animator.SetBool("isRun", true);
        //animator.SetTrigger("makeRun");
        rangeDetection.SetActive(false);

        if (isDisable)
        {
            return;
        }
        if (!teamHaveBall)
        {
            //agent.speed = 1.5f * Time.deltaTime;
            //agent.SetDestination(ball.transform.position);

            if (ball)
            {
                //Rotate object menuju bola
                Vector3 targetDirection = ball.transform.position - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speedRotation * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);

                transform.position = Vector3.MoveTowards(transform.position, ball.transform.position, attackerSpeed * Time.deltaTime);
            }
        }
        else
        {
            if(teamColor == "Blue") //TEAM BLUE
            {
                if (haveBall)
                {
                    //Rotate object menuju gawang musuh
                    Vector3 targetDirection = goalRed.transform.position - transform.position;
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speedRotation * Time.deltaTime, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDirection);

                    //Gerak menuju gawang musuh
                    transform.position = Vector3.MoveTowards(transform.position, goalRed.transform.position, bringBallSpeed * Time.deltaTime);
                }
                else
                {
                    //Rotate object menuju arah field lawan
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), speedRotation * Time.deltaTime);
                    transform.Translate(Vector3.forward * attackerSpeed * Time.deltaTime);
                }
                
            }
            else //TEAM RED
            {
                if (haveBall)
                {
                    //Rotate object menuju gawang musuh
                    Vector3 targetDirection = goalBlue.transform.position - transform.position;
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speedRotation * Time.deltaTime, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDirection);

                    //Gerak menuju gawang musuh
                    transform.position = Vector3.MoveTowards(transform.position, goalBlue.transform.position, bringBallSpeed * Time.deltaTime);
                }
                else
                {
                    //Rotate object menuju arah field lawan
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180, 0), speedRotation * Time.deltaTime);
                    transform.Translate(Vector3.forward * attackerSpeed * Time.deltaTime);
                }

            }
        }
    }

    public void defending()
    {
        if (isDisable)
        {
            return;
        }

        rangeDetection.SetActive(true);

        if (teamColor == "Red")
        {
            for(int i = 0; i < gameController.listBluePlayer.Count; i++)
            {
                GameObject currentTarget = gameController.listBluePlayer[i];

                float distance = Vector3.Distance(this.transform.position, currentTarget.transform.position);

                if(distance <= rangeDetectionDistance && currentTarget.GetComponent<PlayerBehaviour>().haveBall == true)
                {
                    rangeDetection.SetActive(false);

                    Vector3 targetDirection = currentTarget.transform.position - transform.position;
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speedRotation * Time.deltaTime, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDirection);

                    transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, defenderSpeed * Time.deltaTime);
                }
            }
        }

        if (teamColor == "Blue")
        {
            for (int i = 0; i < gameController.listRedPlayer.Count; i++)
            {
                GameObject currentTarget = gameController.listRedPlayer[i];

                float distance = Vector3.Distance(this.transform.position, currentTarget.transform.position);

                if (distance <= 3 && currentTarget.GetComponent<PlayerBehaviour>().haveBall == true)
                {
                    Vector3 targetDirection = currentTarget.transform.position - transform.position;
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speedRotation * Time.deltaTime, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDirection);

                    transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, defenderSpeed * Time.deltaTime);
                }
            }
        }
    }

    public void die()
    {
        transform.position = transform.position;
        timeDissolve += 1 * Time.deltaTime;
        this.GetComponent<Renderer>().material.SetFloat("Vector1_A8DBC554", timeDissolve);

        if (this.teamColor == "Blue")
        {
            gameController.listBluePlayer.Remove(this.gameObject);
        }
        if (this.teamColor == "Red")
        {
            gameController.listRedPlayer.Remove(this.gameObject);
        }

        Destroy(this.gameObject, 3);
    }

    public void appear()
    {
        timeAppear -= 1 * Time.deltaTime;
        this.GetComponent<Renderer>().material.SetFloat("Vector1_A8DBC554", timeAppear);

        if(timeAppear < 0)
        {
            timeAppear = 0;
            isAppear = false;
        }
    }

    public void disablePlayer()
    {
        //transform.rotation = transform.rotation;

        

        this.GetComponent<CapsuleCollider>().enabled = false;

        if (teamColor == "Blue")
        {
            meshRenderer.material = blueTeamDisableMaterial;
        }
        else
        {
            meshRenderer.material = redTeamDisableMaterial;
        }

        transform.position = transform.position;
        if (isAttack)
        {
            //ATTACK DISABLE
            if (teamColor == "Blue")
            {
                if (gameController.listBluePlayer.Count == 1)
                {
                    Debug.Log("defender goal");
                    gameController.defenderScore();
                    gameController.destroyAllPlayer();
                }
                else
                {
                    if (haveBall)
                    {
                        bool canPass = false;

                        foreach(GameObject player in gameController.listBluePlayer)
                        {
                            if(player.GetComponent<PlayerBehaviour>().isDisable == false)
                            {
                                canPass = true;
                            }
                        }

                        if (canPass)
                        {
                            GameObject nearest = findNearestActivePlayer();
                            passBall(ball, nearest);
                        }
                        else
                        {
                            gameController.defenderScore();
                            Debug.Log("defender goal");
                            gameController.destroyAllPlayer();
                        }

                    }
                }
            }
            else
            {
                if (gameController.listRedPlayer.Count == 1)
                {
                    gameController.defenderScore();
                    Debug.Log("defender goal");
                    gameController.destroyAllPlayer();
                }
                else
                {
                    if (haveBall)
                    {
                        bool canPass = false;

                        foreach (GameObject player in gameController.listRedPlayer)
                        {
                            if (player.GetComponent<PlayerBehaviour>().isDisable == false)
                            {
                                canPass = true;
                            }
                        }


                        if (canPass)
                        {
                            GameObject nearest = findNearestActivePlayer();
                            passBall(ball, nearest);
                        }
                        else
                        {
                            gameController.defenderScore();
                            Debug.Log("defender goal");
                            gameController.destroyAllPlayer();
                        }
                    }
                }
            }
        }


        if (!isAttack)
        {
            Vector3 targetDirection = originalPosition - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speedRotation * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);

            transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnSpeed * Time.deltaTime);
        }


        if(teamColor == "Blue")
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), speedRotation * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180, 0), speedRotation * Time.deltaTime);
        }

        StartCoroutine(cooldownDisable(2));
    }


    public GameObject findNearestActivePlayer()
    {
        GameObject nearest = null;
        float nearestDistance = 0;

        if (teamColor == "Blue")
        {
            foreach (GameObject currentCheckDistance in gameController.listBluePlayer)
            {
                if (currentCheckDistance == this.gameObject)
                {
                    continue;
                }

                if (currentCheckDistance.GetComponent<PlayerBehaviour>().isDisable)
                {
                    continue;
                }

                if (nearest == null)
                {
                    nearest = currentCheckDistance;
                    nearestDistance = Vector3.Distance(this.transform.position, nearest.transform.position);
                }
                else
                {
                    float checkDistance = Vector3.Distance(this.transform.position, currentCheckDistance.transform.position);

                    if (checkDistance < nearestDistance)
                    {
                        nearest = currentCheckDistance;
                        nearestDistance = checkDistance;
                    }
                }

            } 
        }

        if (teamColor == "Red")
        {
            foreach (GameObject currentCheckDistance in gameController.listRedPlayer)
            {
                if (currentCheckDistance == this.gameObject)
                {
                    continue;
                }

                if (currentCheckDistance.GetComponent<PlayerBehaviour>().isDisable)
                {
                    continue;
                }

                if (nearest == null)
                {
                    nearest = currentCheckDistance;
                    nearestDistance = Vector3.Distance(this.transform.position, nearest.transform.position);
                }
                else
                {


                    float checkDistance = Vector3.Distance(this.transform.position, currentCheckDistance.transform.position);

                    if (checkDistance < nearestDistance)
                    {
                        nearest = currentCheckDistance;
                        nearestDistance = checkDistance;
                    }
                }

            }
        }

        return nearest;
    }


    public void passBall(GameObject ball, GameObject targetPass)
    {
        Vector3 targetPassBallPos = targetPass.transform.Find("Ball Pos").transform.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetPassBallPos, speedRotation * Time.deltaTime, 0.0f);

        ball.GetComponent<BallBehaviour>().setMove(targetPass);

        //Vector3 targetDirection = originalPosition - transform.position;
        //Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speedRotation * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);


        //transform.rotation = Quaternion.LookRotation(targetPassBallPos);

        ball.transform.position = Vector3.MoveTowards(ball.transform.position, targetPass.transform.position, passSpeed * Time.deltaTime);
        ball.transform.SetParent(targetPass.transform);
        targetPass.transform.GetComponent<PlayerBehaviour>().haveBall = true;
        haveBall = false;
    }


    IEnumerator cooldownDisable(float cooldownTime)
    {
        isDisable = true;
        yield return new WaitForSeconds(cooldownTime);

        this.GetComponent<CapsuleCollider>().enabled = true;

        if (teamColor == "Blue")
        {
            meshRenderer.material = blueTeamMaterial;
        }
        else
        {
            meshRenderer.material = redTeamMaterial;
        }


        isDisable = false;

    }
}
