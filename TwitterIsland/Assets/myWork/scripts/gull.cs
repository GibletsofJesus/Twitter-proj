﻿using UnityEngine;
using System.Collections;

public class gull : MonoBehaviour {

    private float angle = 0;
    public float radius = 10;
    public float offset;

	// Update is called once per frame
	void Update () {
        float x = 0;
        float y = 0;
        
        x = radius * Mathf.Cos(angle+(Mathf.Deg2Rad*offset));
        y = radius * Mathf.Sin(angle+ (Mathf.Deg2Rad * offset));
        
        transform.localPosition = new Vector3(x, -13,y);
        angle += (.1f/radius) * Mathf.Rad2Deg * Time.deltaTime;
        transform.rotation = Quaternion.Euler(15,(-angle* Mathf.Rad2Deg)-90-offset, 0);
    }
}
