using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BambooController : MonoBehaviour
{
    Animator a;
    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<Animator>();
        a.SetBool("died", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Sliced()
    {
        a.SetBool("died", true);
    }
    public void Grow()
    {
        a.SetBool("died", false);
    }
    public void Reset()
    {
        a.SetBool("failed", false);
    }
    public void Failed()
    {
        a.SetBool("failed", true);
    }
}
