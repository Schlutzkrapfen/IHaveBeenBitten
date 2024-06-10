using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossTalking : MonoBehaviour
{


    public float waitTimeForNextSentences =1f;
    public float waitForNextCharacter= 0.02f;
    private float waitTimeCharacter;
    private float waitTimeSentences;
    private TextMeshProUGUI textMesh;
    private string[] textBoss =
    {
        "What are you doing here? Go back to work",
        "Did you become a Zombie?",
        "Do you really think that just because you\n became a Zombie that you can Stop Working",
        "Death doesn't dismiss you, I do!!!",
        "If you want promotion you can forget it!"
    };

    private int characters;

    private int sentences;
    // Start is called before the first frame update
    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = "";
    }

    // Update is called once per frame
    void Update()
    {

        waitTimeCharacter += Time.deltaTime;
        while (waitTimeCharacter > waitForNextCharacter)
        {
            waitTimeCharacter -= waitForNextCharacter;
            textMesh.text += textBoss[sentences][characters];
            characters++;

        }

        if (characters >= textBoss[sentences].Length)
        {
            waitTimeSentences += Time.deltaTime;
           
        }

        if (waitTimeSentences > waitTimeForNextSentences)
        {
            waitTimeCharacter = 0;
             characters = 0;
             textMesh.text = "";
            sentences= (sentences +1) % textBoss.Length;
              waitTimeSentences = 0;
        }
    }
}
