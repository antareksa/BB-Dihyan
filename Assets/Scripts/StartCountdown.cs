using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCountdown : MonoBehaviour
{
    public GameController game;
    public EnergyBarController blueEnergy;
    public EnergyBarController redEnergy;

    public GameObject countdownUI;
    public GameObject gameUI;

    public Text blueCountdown;
    public Text redCountdown;

    public float time;

    public AudioSource audioSource;
    public AudioClip clip;

    public bool isAudio;

    void Start()
    {
        game.enabled = false;
        blueEnergy.enabled = false;
        redEnergy.enabled = false;
        gameUI.SetActive(false);

        StartCoroutine(Sound());

    }

    

    void Update()
    {

        if (time > 1)
        {
            time -= Time.deltaTime;


            blueCountdown.text = Mathf.FloorToInt(time).ToString();
            redCountdown.text = Mathf.FloorToInt(time).ToString();



        }
        else
        {
            game.enabled = true;
            blueEnergy.enabled = true;
            redEnergy.enabled = true;

            countdownUI.SetActive(false);
            gameUI.SetActive(true);


            Debug.Log("Time has run out!");
            time = 1;

            this.enabled = false;

            
        }


    }

    IEnumerator Sound()
    {
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(1);
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(1);
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(1);
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(1);
    }
}
