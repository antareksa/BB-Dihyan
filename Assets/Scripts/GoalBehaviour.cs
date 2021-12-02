using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehaviour : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] string goalColor;
    [SerializeField] GameObject goalBlue;
    [SerializeField] GameObject goalRed;
    [SerializeField] float distanceBlue;
    [SerializeField] float distanceRed;

    void Start()
    {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();

        if (goalColor == null)
        {
            goalColor = this.gameObject.name.Substring(6,9);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Ball")
        {
            if(goalColor == "Red")
            {
                gameController.scoreBlue++;
            }
            if (goalColor == "Blue")
            {
                gameController.scoreRed++;
            }
        }
    }
}
