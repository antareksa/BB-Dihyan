using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBarController : MonoBehaviour
{
    [SerializeField] private Slider energyBar;
    [SerializeField] public float totalEnergy;
    [SerializeField] private GameObject[] fillIndicator;
    [SerializeField] private Color filledColor;

    [SerializeField] private float fillRate = 0.5f;

    private float current;




    void Start()
    {

    }

    void Update()
    {
        if(totalEnergy < 6)
        {
            fillEnergy();
        }

        

        energyBar.value = totalEnergy;
    }

    void fillEnergy()
    {
        totalEnergy += fillRate * Time.deltaTime;

        current = energyBar.GetComponent<Slider>().value;
        
        if(totalEnergy > 1)
        {
            for(int i = 0; i < (int)totalEnergy; i++)
            {
                fillIndicator[i].GetComponent<Image>().enabled = false;
            }
        }
    }

    public void reduceEnergy(int minus)
    {
        if((totalEnergy - minus) > 0)
        {
            totalEnergy -= minus;
            Debug.Log("ini current: " + (int)current);
            for (int i = (int)current - 1; i >= 0; i--)
            {
                if ((int)current == 5)
                {
                    fillIndicator[i + 1].GetComponent<Image>().enabled = true;
                }
                else
                {
                    fillIndicator[i].GetComponent<Image>().enabled = true;
                    Debug.Log("MASUK");
                }


            }
        }
        else
        {
            foreach(GameObject indicator in fillIndicator)
            {
                indicator.GetComponent<Image>().enabled = true;
            }
        }
        
    }

    public void resetEnergy()
    {
        foreach (GameObject indicator in fillIndicator)
        {
            indicator.GetComponent<Image>().enabled = true;
        }
        reduceEnergy(6);
    }
}
