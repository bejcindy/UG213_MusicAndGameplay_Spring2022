using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform left, mid, right;

    public GemGenerator gemGenerator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(gemGenerator.fallingGemR.playerInput))
        {
            transform.position = left.position;
        }
        if (Input.GetButtonDown(gemGenerator.fallingGemB.playerInput))
        {
            transform.position = mid.position;
        }
        if (Input.GetButtonDown(gemGenerator.fallingGemG.playerInput))
        {
            transform.position = right.position;
        }
    }
}
