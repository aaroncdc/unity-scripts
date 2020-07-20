using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceGame : MonoBehaviour
{
    public GameObject[] dices = new GameObject[3];
    public GameObject[] texts = new GameObject[4];

    public int rolls = 3;

    int rolling = 0;
    int[] roundScore = new int[3];
    int round = 1;
    bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < 3; x++)
            roundScore[x] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!stop)
        { 
            foreach(GameObject dice in dices)
            {
                if(dice.GetComponent<DiceController>().rolled())
                {
                    rolling++;
                    roundScore[round-1] += dice.GetComponent<DiceController>().getFace();
                }
            }

            if(rolling == 3)
            {
                    round++;
                    foreach (GameObject dice in dices)
                        dice.GetComponent<DiceController>().reset();
            }

            if(round == 4)
            {
                if (roundScore[0] == 7 && roundScore[1] == 7 && roundScore[2] == 7)
                {
                    stop = true;
                    foreach (GameObject dice in dices)
                        dice.GetComponent<DiceController>().reset();
                    print("CONGRATULATIONS! YOU WON!");
                    // Congratulations!! You won!! Stop the game here. Add a button to reset the game.
                }
                else
                {
                    round = 1;
                }
            }

            texts[0].GetComponent<Text>().text = roundScore[0].ToString();
            texts[1].GetComponent<Text>().text = roundScore[1].ToString();
            texts[2].GetComponent<Text>().text = roundScore[2].ToString();

            texts[3].GetComponent<Text>().text = "Round: " + round.ToString() + "/3";

            rolling = 0;
            roundScore[round - 1] = 0;
        }
    }
}
