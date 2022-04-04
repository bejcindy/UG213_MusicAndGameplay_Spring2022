using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOController : MonoBehaviour
{
    Animator a;
    public GameObject explode;

    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<Animator>();
        explode.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoMiddle()
    {
        a.SetBool("isMiddle", true);
    }
    public void GoIdle()
    {
        a.SetBool("isMiddle", false);
    }

    public void Die()
    {
        explode.SetActive(true);
    }
}
