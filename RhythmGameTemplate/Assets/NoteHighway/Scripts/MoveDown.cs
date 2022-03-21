using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    float speed = .05f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= speed * new Vector3(1, 1, 0);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
