using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float delay;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
