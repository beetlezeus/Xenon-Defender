using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine;

public class InitializeUnityGamingServices : MonoBehaviour
{
    async void Start()
    {
        // Initialize Unity Gaming Services
        await UnityServices.InitializeAsync();
        await AuthenticatePlayer();
    }

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
