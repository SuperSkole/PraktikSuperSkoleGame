using System.Text.RegularExpressions;
using CORE;
using TMPro;
using UI.Scripts;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes._02_LoginScene.Scripts
{
    /// <summary>
    /// Manages the UI interactions for the login and registration screens.
    /// </summary>
    public class UILoginSceneManager : MonoBehaviour
    {
        [SerializeField] private AuthenticationManager authenticationManager;
        [SerializeField] private GameObject loginScreen;
        [SerializeField] private Image panel;
        [SerializeField] private Image loginButton;
        [SerializeField] private Image registerButton;
        [SerializeField] private Toggle anonLoginToggle;
        
        [SerializeField] private TMP_InputField usernameInput;
        [SerializeField] private TMP_InputField passwordInput;

        private bool isLoginButtonInteractable = false;
        private bool isRegisterButtonInteractable = false;

        // Regex for validating password complexity
        // (min 8 chars, at least one uppercase, one lowercase, one number, and one special character)
        private const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

        private HoverEffectUI loginButtonHoverEffect;
        private BlinkEffectUI panelBlinkEffect;

        /// <summary>
        /// Initializes the login screen and sets up the initial state of the UI elements.
        /// </summary>
        private void Awake()
        {
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
#if UNITY_EDITOR
            // In the Unity editor, the login button is interactable if the toggle is on.
            isLoginButtonInteractable = anonLoginToggle.isOn;
#else
            // In build mode, the login button is interactable when there is a username input.
            isLoginButtonInteractable = !string.IsNullOrEmpty(usernameInput.text);
#endif
            // Validate password complexity for registration.
            isRegisterButtonInteractable = IsPasswordValid(passwordInput.text);

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

        /// <summary>
        /// Adjusts login settings when the anonymous login toggle is changed.
        /// </summary>
        /// <param name="isAnonLogin">Indicates if anonymous login is enabled.</param>
        private void OnAnonLoginToggleChanged(bool isAnonLogin)
        {
            authenticationManager.SetUseAnonymousLogin(isAnonLogin);
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

        /// <summary>
        /// Attempts to log the user in using the provided credentials.
        /// </summary>
        public async void TryLogin()
        {
#if UNITY_EDITOR
            if (isLoginButtonInteractable)
            {
                // Retrieve the username from the input if provided, otherwise use 'TEST' as default.
                string username = string.IsNullOrEmpty(usernameInput.text) ? "TEST" : usernameInput.text;
        
                // In editor mode, perform anonymous login using the provided or default username.
                bool signInSuccessful = await authenticationManager.SignInAnonymouslyAsync();
                if (signInSuccessful)
                {
                    Debug.Log("Anonymous login successful in editor with username: " + username);
                    GameManager.Instance.CurrentUser = username;  
                    SceneManager.LoadScene(SceneNames.Start);
                }
                else
                {
                    Debug.LogError("Failed anonymous login in editor with username: " + username);
                    panelBlinkEffect.TriggerBlink(Color.red);
                }
            }
#else
            // In build mode, check for username and determine login type based on password presence.
            string username = usernameInput.text;
            string password = passwordInput.text;

            if (!string.IsNullOrEmpty(username))
            {
                if (string.IsNullOrEmpty(password))
                {
                    // Perform anonymous login with just a username.
                    bool signInSuccessful = await authenticationManager.SignInAnonymouslyAsync();
                    if (signInSuccessful)
                    {
                        Debug.Log("Anonymous login successful with username.");
                        GameManager.Instance.CurrentUser = username;
                        SceneManager.LoadScene(SceneNames.Start);
                    }
                    else
                    {
                        Debug.LogError("Failed anonymous login with username.");
                        panelBlinkEffect.TriggerBlink(Color.red);
                    }
                }
                else
                {
                    // Regular login with username and password.
                    bool signInSuccessful = await authenticationManager.SignInWithUsernamePasswordAsync(username, password);
                    if (signInSuccessful)
                    {
                        Debug.Log("Login successful: " + username);
                        GameManager.Instance.CurrentUser = username;
                        SceneManager.LoadScene(SceneNames.Start);
                    }
                    else
                    {
                        Debug.LogError("Login failed for: " + username);
                        panelBlinkEffect.TriggerBlink(Color.red);
                    }
                }
            }
#endif
        }


        /// <summary>
        /// Attempts to register a new user with the provided credentials.
        /// </summary>
        public async void TryRegister()
        {
            if (isRegisterButtonInteractable)
            {
                string username = usernameInput.text.Trim(); 
                string password = passwordInput.text;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    Debug.LogError("Username and password cannot be empty.");
                    return;
                }

                Debug.Log("Trying User register: " + username);
                
                await authenticationManager.SignUpWithUsernamePasswordAsync(username, password);
                
                ClearInputFields();
            }
        }

        private void ClearInputFields()
        {
            usernameInput.text = "";
            passwordInput.text = "";
        }
    }
}
