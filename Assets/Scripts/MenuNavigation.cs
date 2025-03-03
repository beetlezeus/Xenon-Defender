using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
  This script handles menu navigation using the keyboard keys instead of a mouse
  This is important Since this Arcade game should be completely playable without using a mouse
 */
public class MenuNavigation : MonoBehaviour
{
    public Button[] menuButtons; // Reference to assign buttons in the Inspector
    private int currentIndex = 0; // index for starting button to be highlighted

    void Start()
    {
        HighlightButton(currentIndex); // first button is highlighted 
    }

    void Update()
    {
        // pressing up arrow or left arrow navigates to previous button
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Navigate(-1);
        }
        // down arrow or right arrow navigates to next button
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Navigate(1);
        }
        // return clicks the selected button
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            menuButtons[currentIndex].onClick.Invoke();
        }
    }

    // function for navigating to previous / next button
    void Navigate(int direction)
    {
        // Clear the current button highlight
        menuButtons[currentIndex].OnDeselect(null);

        // Update index
        currentIndex = (currentIndex + direction + menuButtons.Length) % menuButtons.Length;

        // Highlight the new button
        HighlightButton(currentIndex);
    }

    // function for highlighting correct button with navigation
    void HighlightButton(int index)
    {
        menuButtons[index].Select(); // Highlight the button
    }
}
