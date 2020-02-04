using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSparkle : MonoBehaviour
{
    // Start is called before the first frame update
    public float time;
    void Start()
    {
        StartCoroutine(waitForIt());
    }

    IEnumerator waitForIt()
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);  
    }
}
