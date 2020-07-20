using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermaRotation : MonoBehaviour
{
    public float force = 1.0f;
    public Vector3 axis = Vector3.up;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis, force);
    }
}
