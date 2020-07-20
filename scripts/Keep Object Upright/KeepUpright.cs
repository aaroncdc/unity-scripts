using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepUpright : MonoBehaviour
{
    public Vector3 upAxis = new Vector3(.0f, .0f, .0f);
    public bool lookAtCamera = false;
    public GameObject cameraObject;
    public Vector3 cameraPositionVector = Vector3.forward;
    public Vector3 cameraRotationVector = Vector3.up;

    Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(upAxis);
        if (lookAtCamera)
        {
            /*
             * If the axes were right, this would be:
             * transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward, m_Camera.transform.rotation * Vector3.up);
             */
            transform.LookAt(transform.position + cameraObject.transform.rotation * cameraPositionVector, cameraObject.transform.rotation * cameraRotationVector);
        }

    }
}
