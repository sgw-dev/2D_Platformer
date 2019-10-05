using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsTEXT : MonoBehaviour
{
    private bool setActiveWindow = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IsActive()
    {
        if (setActiveWindow)
        {
            setActiveWindow = false;
        }
        else
        {
            setActiveWindow = true;
        }
        gameObject.SetActive(setActiveWindow);
    }


}
