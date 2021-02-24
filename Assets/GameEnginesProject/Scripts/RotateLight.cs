﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  It rotates the directional light
 * 
 */
public class RotateLight : MonoBehaviour
{

    public float speed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, speed * Time.deltaTime, 0.0f, Space.World);
    }
}