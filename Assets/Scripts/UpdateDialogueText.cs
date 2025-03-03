using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
  This is a simple script for updating the dialogue text along with dialogue audio during gameplay
  The Master Timeline emitters invokes this script at specific points on the timeline
 */
public class UpdateDialogueText : MonoBehaviour
{
    [SerializeField] TMP_Text dialogueText; // reference to the dialogue text UI element
    [SerializeField] string[] dialogueLines; // reference to array of strings storing the dialogue text

    private int currentDialogueLine = 0; // start at string 0 in array


    // function for loading next dialogue line. invoked by signal emitter / receivers from Master Timeline
    public void LoadNextDialogueLine()
    {
        currentDialogueLine++; // increment currentIndex

        dialogueText.text = dialogueLines[currentDialogueLine];  // update dialogue text to display

    }
}
