using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine;

/*
  This script is responsible for intitializing Unity Gaming Services including forced anonymous authenticaion
  It ensures that every player is authenticated at start of the game to track their high scores
 */
public class InitializeUnityGamingServices : MonoBehaviour
{
    async void Start()
    {
        // Initialize Unity Gaming Services
        await UnityServices.InitializeAsync();
        await AuthenticatePlayer();
    }

    // Asynchronous method for authenticating players once game is loaded
    public async Task AuthenticatePlayer()
    {
        try
        {
            // Authenticate the player anonymously
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Signed in with Player ID: " + AuthenticationService.Instance.PlayerId);
            }
        }
        catch(System.Exception ex)
        {
            Debug.LogError($"Error signing in player: {ex.Message}");
        }
    }
}
