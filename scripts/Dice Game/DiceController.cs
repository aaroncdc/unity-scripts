using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceController : MonoBehaviour
{
    ConstantForce propeller;
    Button diceButton;
    Text numberText;

    public float faceCheckTreshold = 0.5f;
    public float rollTorque = 150.0f;
    public float minJumpForce = 6.0f;
    public float maxJumpForce = 14.0f;

    private int face = 3;
    private bool isRolling = false;
    private bool speedReached = false;
    private float force = .0f;

    bool isUpright()
    {
        return transform.up.y > faceCheckTreshold;
    }

    public bool rolled()
    {
        // TODO: return true only when it's been rolled and the motion is about 0

        if (transform.GetComponent<Rigidbody>().velocity.magnitude < 0.01f)
        {
            return diceButton.enabled == false;
        }
        else
        {
            return false;
        }
    }

    public bool reset()
    {
        diceButton.enabled = true;
        return diceButton.enabled;
    }

    int checkFace()
    {
        int face = 1;
        bool uR = isUpright();

        Vector3 rotation = transform.localEulerAngles;

        if (isUpright())
        {
            return 3;
        }

        if (transform.forward.y > faceCheckTreshold)
        {
            return 1;
        }

        if (transform.forward.y < -faceCheckTreshold)
        {
            return 2;
        }

        if (transform.right.y > faceCheckTreshold)
        {
            return 4;
        }

        if(transform.right.y < -faceCheckTreshold)
        {
            return 5;
        }

        if (transform.up.z < faceCheckTreshold)
        {
            return 6;
        }

        return face;
    }

    void Roll()
    {
        diceButton.enabled = false;
        diceButton.transform.Find("Text").GetComponent<Text>().text = "WAIT";

        force = Random.Range(minJumpForce, maxJumpForce);
        propeller.relativeForce.Set(.0f, force, .0f);
        propeller.relativeTorque.Set(Random.Range(-rollTorque, rollTorque), Random.Range(-rollTorque, rollTorque), Random.Range(-rollTorque, rollTorque));

        propeller.enabled = true;
        isRolling = true;
    }

    void Start()
    {
        numberText = transform.Find("HUDPosition").Find("Canvas").Find("NumberHUD").GetComponent<Text>();
        diceButton = transform.Find("HUDPosition").Find("Canvas").Find("Button").GetComponent<Button>();
        propeller = transform.GetComponent<ConstantForce>();
        propeller.enabled = false;
        diceButton.onClick.AddListener(delegate { Roll(); });
    }

    // Update is called once per frame
    void Update()
    {
        face = checkFace();
        numberText.text = face.ToString();
        //print(transform.localEulerAngles + " " + checkFace() + " " + transform.right.ToString() + " " + transform.up.ToString() + " " + transform.forward.ToString());
        //print(isUpright() + " " + isRightUp() + " " + isFrontUp());
        //print("Right " + transform.right.ToString() + " Up " + transform.up.ToString() + " Forward " + transform.forward.ToString());
        if (isRolling && transform.GetComponent<Rigidbody>().velocity.magnitude >= force)
        {
            speedReached = true;
            propeller.enabled = false;
        }
        else if(isRolling && speedReached && transform.GetComponent<Rigidbody>().velocity.magnitude < 0.01f)
        {
            speedReached = false;
            isRolling = false;
            propeller.enabled = false;
            //diceButton.enabled = true;
            diceButton.transform.Find("Text").GetComponent<Text>().text = "Roll!";
            print("You rolled the dice and got a " + face + "!");
        }
    }

    public int getFace()
    {
        return face;
    }
}
