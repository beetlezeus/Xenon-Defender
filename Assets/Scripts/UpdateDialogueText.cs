using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateDialogueText : MonoBehaviour
{
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] string[] dialogueLines;

    private int currentDialogueLine = 0;


    public void LoadNextDialogueLine()
    {
        currentDialogueLine++;

        dialogueText.text = dialogueLines[currentDialogueLine];

    }
}
