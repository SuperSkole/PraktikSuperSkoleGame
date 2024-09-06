using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;

public class CloudLoginManager : MonoBehaviour
{
    [SerializeField] private Text logTxt;
     
    async void Start() 
    {
        await UnityServices.InitializeAsync();
        SignIn();
    }
    
    
    public async void SignIn() 
    {
        await signInAnonymous();
    }
    
    async Task signInAnonymous()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
    
            print("Sign in Success");
            print("Player Id:" + AuthenticationService.Instance.PlayerId);
                
            // logTxt.text = "Player id:" + AuthenticationService.Instance.PlayerId;
        }
        catch (AuthenticationException ex)
        {
            print("Sign in failed!!");
            Debug.LogException(ex);
        }
        
    }
}