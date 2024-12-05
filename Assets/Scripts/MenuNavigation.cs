using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuNavigation : MonoBehaviour
{
    public Button[] menuButtons; // Assign your buttons in the Inspector
    private int currentIndex = 0;

    void Start()
    {
        HighlightButton(currentIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Navigate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Navigate(1);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            menuButtons[currentIndex].onClick.Invoke();
        }
    }

    void Navigate(int direction)
    {
        // Clear the current button highlight
        menuButtons[currentIndex].OnDeselect(null);

        // Update index
        currentIndex = (currentIndex + direction + menuButtons.Length) % menuButtons.Length;

        // Highlight the new button
        HighlightButton(currentIndex);
    }

    void HighlightButton(int index)
    {
        menuButtons[index].Select(); // Highlight the button
    }
}
