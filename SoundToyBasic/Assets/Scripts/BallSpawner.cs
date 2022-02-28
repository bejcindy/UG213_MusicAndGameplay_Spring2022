using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour {

	public GameObject model;
	public float rate = 1.0f;

    //this is public so we can tell the Platform Creator that we're using our mouse for something else, if 
    public bool mouseOver = false;


	float progress = 0.0f;

	//GameObject[] balls;

	void Spawn()
	{
        GameObject next = Instantiate(model, transform.position, transform.rotation);
		next.transform.localScale = new Vector3(3, 3, 1);
		next.transform.parent = transform;
		next.transform.localPosition = Vector3.zero;
	}

	void Update()
	{
		//balls=GetComponent<>
		progress += rate * Time.deltaTime;
		if (progress >= 1.0f && transform.childCount<1)
		{
			Spawn();
			float newX = Random.Range(-20, 20);
			transform.position = new Vector2(newX, transform.position.y);
			progress -= 1.0f;
		}
	}

    //we could use this method if we want the player to be able to move the spawner around
    private void OnMouseOver()
    { 
        
    }

}
