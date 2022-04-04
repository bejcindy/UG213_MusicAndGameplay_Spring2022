using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodController : MonoBehaviour
{
    public GameObject ufo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void End()
    {
        ufo.SetActive(false);
        gameObject.SetActive(false);
    }
}
