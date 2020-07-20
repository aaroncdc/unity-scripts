using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIRaycast : MonoBehaviour
{
    //public GameObject debugRaycastHit;
    public GameObject canvas;
    public EventSystem eSys;
    public GameObject sceneRoot;

    //GraphicRaycaster gRay;
    GraphicRaycaster[] raycasters = new GraphicRaycaster[0];
    RaycastHit rHit;
    PointerEventData pEvent;
    List<RaycastResult> previous = null;

    public void updateRaycasterList()
    {
        // Call this function whenever you dynamically add a new canvas with a graphic raycaster component
        if (sceneRoot == null)
        {
            if (GameObject.Find("Root") == null)
            {
                throw new System.Exception("You didn't set a scene root! Add a single root object for your whole scene and then set it as the root on this script.");
            }
            else
            {
                sceneRoot = GameObject.Find("Root");
            }
        }

        raycasters = sceneRoot.transform.GetComponentsInChildren<GraphicRaycaster>();
        print("Found " + raycasters.Length + " raycasters");
    }

    // Start is called before the first frame update
    void Start()
    {
        updateRaycasterList();
    }

    // Update is called once per frame
    void Update()
    {
        if(raycasters.Length > 0)
        {
            foreach(GraphicRaycaster gRay in raycasters)
            {
                pEvent = new PointerEventData(eSys);
                pEvent.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();

                gRay.Raycast(pEvent, results);

                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.transform.parent.GetComponent<Canvas>() != null)
                    {
                        if (result.gameObject.transform.GetComponent<Button>() != null)
                        {
                            result.gameObject.transform.GetComponent<Button>().OnPointerEnter(pEvent);
                        }
                    }
                }

                if (previous != null)
                {
                    foreach (RaycastResult result in previous)
                    {
                        if (!results.Contains(result))
                        {
                            if (result.gameObject.transform.GetComponent<Button>() != null)
                            {
                                result.gameObject.transform.GetComponent<Button>().OnPointerExit(pEvent);
                            }
                        }
                    }
                }

                previous = new List<RaycastResult>(results);

                if (Input.GetKey(KeyCode.Mouse0))
                {
                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject.transform.parent.GetComponent<Canvas>() != null)
                        {
                            if (result.gameObject.transform.GetComponent<Button>() != null)
                            {
                                result.gameObject.transform.GetComponent<Button>().onClick.Invoke();
                            }
                            print("Hit: " + result.gameObject.name);
                        }

                    }
                }
            }
        }

        //Debug.DrawRay(transform.position, transform.forward * 1000.0f, Color.red);

        /*if(Physics.Raycast(transform.position, transform.forward, out rHit, Mathf.Infinity, int.MaxValue))
        {
            if(rHit.distance > 1.0f)
            {
                debugRaycastHit.SetActive(true);
            }
            else
            {
                debugRaycastHit.SetActive(false);
            }
            debugRaycastHit.transform.position = rHit.point;
            //print(rHit.collider.gameObject.name + " at " + rHit.distance + " units");
        }
        else
        {
            debugRaycastHit.SetActive(false);
        }*/
    }
}
