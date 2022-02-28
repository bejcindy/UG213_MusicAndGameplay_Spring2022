using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScript : MonoBehaviour
{
    SpriteRenderer r;
    public bool hit;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<SpriteRenderer>();
        r.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hit)
        {
            r.color += new Color(0, 0, 0, .4f);
            r.enabled = true;
            hit = false;
        }
        else
        {
           r.color -= new Color(0, 0, 0, Time.deltaTime*1.5f);
            if (r.color.a <= 0)
            {
                r.enabled = false;
                r.color += new Color(0, 0, 0, .4f);
                
            }
        }
        
    }
}
