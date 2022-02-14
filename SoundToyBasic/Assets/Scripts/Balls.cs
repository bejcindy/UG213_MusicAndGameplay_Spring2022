using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Balls : MonoBehaviour
{

    public pxStrax s;

    public PositionToPitch posToP;

    public AudioSource a;

    Renderer r;
    Rigidbody2D rb;

    //public pxStrax subtractiveSynth;
    public float minSynthParam = 100;
    public float maxSynthParam = 12000;
    public AnimationCurve mappingCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        s = GetComponent<pxStrax>();
        posToP = GetComponent<PositionToPitch>();
        s.sustain = true;
        //s.KeyOn(posToP.PitchMapping(transform.position));
        r = GetComponent<Renderer>();
        a = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        //s.WaveShape(1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!r.isVisible)
        {
            //Destroy(gameObject);
            a.volume -= Time.deltaTime;
            if (a.volume <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            s.KeyOn(posToP.PitchMapping(transform.position));
            a.volume = Mathf.Abs(rb.velocity.y);
        }
    }
    private void OnDestroy()
    {
        s.KeyOff();
        StopAllCoroutines();
    }
}
