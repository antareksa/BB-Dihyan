using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public GameController game;

    public Text blueTeamText;
    public Text redTeamText;

    public bool blueReady;
    public bool redReady;

    public GameObject gameUI;
    public GameObject finishUI;

    void Start()
    {
        
    }

    void Update()
    {
        if(blueReady && redReady)
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
    }

    public void setReady(bool isBlue)
    {
        if (isBlue)
        {
            blueTeamText.text = "WAIT FOR RED TEAM";
            blueReady = true;
        }
        else
        {
            redTeamText.text = "WAIT FOR BLUE TEAM";
            redReady = true;
        }
    }
}
