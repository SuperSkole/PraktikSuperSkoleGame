using System;
using System.Text.RegularExpressions;
using CORE;
using TMPro;
using UI.Scripts;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes._02_LoginScene.Scripts
{
    public enum AuthenticationMethod
    {
        Anonymous,
        UsernamePassword,
        Google,
        Facebook
    }
    
    /// <summary>
    /// Manages the UI interactions for the login and registration screens.
    /// </summary>
    public class UILoginSceneManager : MonoBehaviour
    {
        [SerializeField] private GameObject loginScreen;
        [SerializeField] private Image panel;
        [SerializeField] private Image loginButton;
        [SerializeField] private Image registerButton;
        [SerializeField] private Toggle anonLoginToggle;
        [SerializeField] private TMP_InputField usernameInput;
        [SerializeField] private TMP_InputField passwordInput;
        
        private IAuthenticationService authService;

        private bool isLoginButtonInteractable;
        private bool isRegisterButtonInteractable;

        // Regex for validating password complexity
        // (min 8 chars, at least one uppercase, one lowercase, one number, and one special character)
        private const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,30}$";
        private const string UsernamePattern = @"^(?i)[a-z\d.\-@_]{3,20}$";

        private HoverEffectUI loginButtonHoverEffect;
        private BlinkEffectUI panelBlinkEffect;

        /// <summary>
        /// Initializes the login screen and sets up the initial state of the UI elements.
        /// </summary>
        private async void Awake()
        {
            Debug.Log("Initializing Unity services");
            await UnityServices.InitializeAsync();
            
            loginScreen.SetActive(true);

            panelBlinkEffect = panel.GetComponent<BlinkEffectUI>();
            loginButtonHoverEffect = loginButton.GetComponent<HoverEffectUI>();

            ToggleButtonVisibility(loginButton, false);
            ToggleButtonVisibility(registerButton, false);
        }

        /// <summary>
        /// Attaches listeners to input fields to validate user input in real-time.
        /// </summary>
        private void Start()
        {
#if UNITY_EDITOR
            anonLoginToggle.gameObject.SetActive(true);
            anonLoginToggle.onValueChanged.AddListener(OnAnonLoginToggleChanged);
#else
            anonLoginToggle.gameObject.SetActive(false);
#endif
            usernameInput.onValueChanged.AddListener(delegate { ValidateInput(); });
            passwordInput.onValueChanged.AddListener(delegate { ValidateInput(); });
        }

        /// <summary>
        /// Validates the input fields and updates the button interactivity accordingly.
        /// </summary>
        private void ValidateInput()
        {
            string username = usernameInput.text.Trim();
            string password = passwordInput.text.Trim();

#if UNITY_EDITOR
            // In the Unity editor, the login button should be interactable if the anonymous toggle is on
            // OR if both username and password are valid for Username/Password login
            isLoginButtonInteractable = anonLoginToggle.isOn || (IsUsernameValid(username) && IsPasswordValid(password));
#else
    // In build mode, the login button is interactable if either:
    // 1. There is a valid username (for anonymous login), or
    // 2. Both username and password are valid (for Username/Password login)
    isLoginButtonInteractable = IsUsernameValid(username) || (IsUsernameValid(username) && IsPasswordValid(password));
#endif

            // Validate password complexity for registration.
            isRegisterButtonInteractable = (IsUsernameValid(username) && IsPasswordValid(password));

            // Update visibility of login and register buttons based on their interactable states.
            ToggleButtonVisibility(loginButton, isLoginButtonInteractable);
            ToggleButtonVisibility(registerButton, isRegisterButtonInteractable);
        }


        /// <summary>
        /// Validates the password based on the given complexity rules.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <returns>True if the password meets the criteria, otherwise false.</returns>
        private bool IsPasswordValid(string password)
        {
            return Regex.IsMatch(password, PasswordPattern);
        }

        private bool IsUsernameValid(string username)
        {
            return Regex.IsMatch(username, UsernamePattern);
        }

        /// <summary>
        /// Adjusts login settings when the anonymous login toggle is changed.
        /// </summary>
        /// <param name="isAnonLogin">Indicates if anonymous login is enabled.</param>
        private void OnAnonLoginToggleChanged(bool isAnonLogin)
        {
            ValidateInput(); // Revalidate inputs after toggle state change
        }

        /// <summary>
        /// Toggles the visibility of a button and triggers hover effects if applicable.
        /// </summary>
        /// <param name="buttonImage">The button image to toggle.</param>
        /// <param name="isVisible">Visibility state of the button.</param>
        private void ToggleButtonVisibility(Image buttonImage, bool isVisible)
        {
            buttonImage.enabled = isVisible;

            if (isVisible)
            {
                loginButtonHoverEffect.HoverEnter();
                loginButtonHoverEffect.HoverExit();
            }
        }
        
        private AuthenticationMethod GetAuthenticationMethod(string username, string password)
        {
            // Handle different scenarios based on the platform and inputs
#if UNITY_EDITOR
            // In the editor, use the anonymous toggle or provided username for anon login
            if (anonLoginToggle.isOn)
            {
                return AuthenticationMethod.Anonymous;
            }

            // If only username is provided (no password), use anonymous login with that username
            if (IsUsernameValid(username))
            {
                return AuthenticationMethod.Anonymous;
            }

            // If both username and password are provided, use Username/Password login
            if (IsUsernameValid(username) && IsPasswordValid(password))
            {
                return AuthenticationMethod.UsernamePassword;
            }

#else
    // In the build version:
    // If only username is provided, use anonymous login with that username
    if (IsUsernameValid(username))
    {
        return AuthenticationMethod.Anonymous;
    }

    // If both username and password are provided, use Username/Password login
    if (IsUsernameValid(username) && IsPasswordValid(password))
    {
        return AuthenticationMethod.UsernamePassword;
    }
#endif

            // Default to anonymous if no conditions match
            return AuthenticationMethod.Anonymous;
        }


        private void AssignAuthService(string username = "", string password = "")
        {
            AuthenticationMethod method = GetAuthenticationMethod(username, password);

            switch (method)
            {
                case AuthenticationMethod.Anonymous:
                    authService = new AnonymousAuthenticationService();
                    break;
                case AuthenticationMethod.UsernamePassword:
                    authService = new UserPasswordAuthenticationService(username, password);
                    break;
                case AuthenticationMethod.Google:
                    // authService = new GoogleAuthenticationService(); // To be implemented later
                    break;
                case AuthenticationMethod.Facebook:
                    // authService = new FacebookAuthenticationService(); // To be implemented later
                    break;
                default:
                    authService = new AnonymousAuthenticationService(); // Fallback to anonymous  
                    break;
            }
        }

        public async void TryLogin()
        {
            string username = usernameInput.text.Trim();
            string password = passwordInput.text.Trim();

            // Assign the appropriate authentication service
            AssignAuthService(username, password);

            if (authService != null)
            {
                try
                {
                    // Use the assigned service to log in
                    await authService.SignInAsync();
                    Debug.Log("Login successful.");
                    GameManager.Instance.CurrentUser = string.IsNullOrEmpty(username) ? "TEST" : username;
                    SceneManager.LoadScene(SceneNames.Start); 
                }
                catch (Exception ex)
                {
                    Debug.LogError("Login failed: " + ex.Message);
            
                    // Indicate failure
                    panelBlinkEffect.TriggerBlink(Color.red);
                }
            }
        }

        public async void TryRegister()
        {
            string username = usernameInput.text.Trim();
            string password = passwordInput.text.Trim();

            // Validate that both username and password are provided
            if (IsUsernameValid(username) && IsPasswordValid(password))
            {
                // Directly assign UserPasswordAuthenticationService for registration
                var passwordService = new UserPasswordAuthenticationService(username, password);

                try
                {
                    bool success = await passwordService.SignUpWithUsernamePasswordAsync();
                    if (success)
                    {
                        Debug.Log("Registration successful.");
                        ClearInputFields();  // Clear the input fields after successful registration 
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Registration failed: " + ex.Message);
                    panelBlinkEffect.TriggerBlink(Color.red);  // Indicate failure
                }
            }
            else
            {
                Debug.LogError("Invalid username or password format.");
            }
        }



//         /// <summary>
//         /// Attempts to log the user in using the provided credentials.
//         /// </summary>
//         public async void TryLogin()
//         {
// #if UNITY_EDITOR
//             if (isLoginButtonInteractable)
//             {
//                 // Retrieve the username from the input if provided, otherwise use 'TEST' as default.
//                 string username = string.IsNullOrEmpty(usernameInput.text) ? "TEST" : usernameInput.text;
//
//                 // In editor mode, perform anonymous login using the provided or default username.
//                 bool signInSuccessful = await authenticationManager.SignInAnonymouslyAsync();
//                 if (signInSuccessful)
//                 {
//                     Debug.Log("Anonymous login successful in editor with username: " + username);
//                     GameManager.Instance.CurrentUser = username;  
//                     SceneManager.LoadScene(SceneNames.Start);
//                 }
//                 else
//                 {
//                     Debug.LogError("Failed anonymous login in editor with username: " + username);
//                     panelBlinkEffect.TriggerBlink(Color.red);
//                 }
//             }
// #else
//     // In build mode, check for username and determine login type based on password presence.
//     string username = usernameInput.text;
//     string password = passwordInput.text;
//
//     if (!string.IsNullOrEmpty(username))
//     {
//         if (string.IsNullOrEmpty(password))
//         {
//             // Perform anonymous login with just a username.
//             bool signInSuccessful = await authenticationManager.SignInAnonymouslyAsync();
//             if (signInSuccessful)
//             {
//                 Debug.Log("Anonymous login successful with username.");
//                 GameManager.Instance.CurrentUser = username;
//                 SceneManager.LoadScene(SceneNames.Start);
//             }
//             else
//             {
//                 Debug.LogError("Failed anonymous login with username.");
//                 panelBlinkEffect.TriggerBlink(Color.red);
//             }
//         }
//         else
//         {
//             // Regular login with username and password.
//             bool signInSuccessful = await authenticationManager.SignInWithUsernamePasswordAsync(username, password); // <-- Call the method here.
//             if (signInSuccessful)
//             {
//                 Debug.Log("Login successful: " + username);
//                 GameManager.Instance.CurrentUser = username;
//                 SceneManager.LoadScene(SceneNames.Start);
//             }
//             else
//             {
//                 Debug.LogError("Login failed for: " + username);
//                 panelBlinkEffect.TriggerBlink(Color.red);
//             }
//         }
//     }
// #endif
//         }
//
//         /// <summary>
//         /// Attempts to register a new user with the provided credentials.
//         /// </summary>
//         public async void TryRegister()
//         {
//             if (isRegisterButtonInteractable)
//             {
//                 string username = usernameInput.text.Trim(); 
//                 string password = passwordInput.text;
//
//                 if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
//                 {
//                     Debug.LogError("Username and password cannot be empty.");
//                     return;
//                 }
//
//                 Debug.Log("Trying User register: " + username);
//                 
//                 await authenticationManager.SignUpWithUsernamePasswordAsync(username, password);
//                 
//                 ClearInputFields();
//             }
//         }

        private void ClearInputFields()
        {
            usernameInput.text = "";
            passwordInput.text = "";
        }
    }
}