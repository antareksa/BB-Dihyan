using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ARSwitch : MonoBehaviour
{
    public bool isAR;
    public GameObject AR;
    public GameObject notAR;

    public GameObject buttonPlay;
    public GameObject buttonPlayAR;


    void Start()
    {
        Debug.Log("why");
    }


    void Update()
    {
        if (isAR)
        {
            AR.SetActive(true);
            notAR.SetActive(false);

            buttonPlay.SetActive(false);
            buttonPlayAR.SetActive(true);
        }
        else
        {
            AR.SetActive(false);
            notAR.SetActive(true);

            buttonPlay.SetActive(true);
            buttonPlayAR.SetActive(false);
        }
    }

    public void switchAR()
    {
        if (!isAR)
        {
            isAR = true;
        }
        else
        {
            isAR = false;
        }
    }

    public void loadPlay()
    {
        SceneManager.LoadScene("Main");
    }

    public void loadPlayAR()
    {
        SceneManager.LoadScene("AR");
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
