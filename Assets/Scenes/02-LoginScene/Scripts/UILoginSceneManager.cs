using CORE;
using UI.Scripts;
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
        [SerializeField] private LoginManager loginManager;
        [SerializeField] private AuthenticationManager authenticationManager;
        [SerializeField] private UserRegistrationManager userRegistrationManager;
        [SerializeField] private GameObject loginScreen;
        [SerializeField] private Image panel;
        [SerializeField] private Image loginButton;
        [SerializeField] private Image registerButton;
        [SerializeField] private Toggle anonLoginToggle; 
        
        private bool isLoginButtonInteractable = false;
        private bool isRegisterButtonInteractable = false;
        
        // References to HoverEffectUI scripts attached to login button
        private HoverEffectUI loginButtonHoverEffect;
        private BlinkEffectUI panelBlinkEffect;

        /// <summary>
        /// This method initializes the login screen and sets up the initial state of the UI elements.
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
        /// This method attaches listeners to input fields to validate user input in real-time.
        /// </summary>
        private void Start()
        {
            // Enable or disable anonymous login based on editor mode.
#if UNITY_EDITOR
            anonLoginToggle.gameObject.SetActive(true);
            anonLoginToggle.onValueChanged.AddListener(OnAnonLoginToggleChanged);
#else
            anonLoginToggle.gameObject.SetActive(false);
#endif
            
            
            // Add listeners to the input fields to check for changes
            loginManager.UsernameInput.onValueChanged.AddListener(delegate { ValidateInput(); });
            loginManager.PasswordInput.onValueChanged.AddListener(delegate { ValidateInput(); });
            userRegistrationManager.UsernameInput.onValueChanged.AddListener(delegate { ValidateInput(); });
            userRegistrationManager.PasswordInput.onValueChanged.AddListener(delegate { ValidateInput(); });
        }

        /// <summary>
        /// Validates the input fields and updates the button interactivity accordingly.
        /// </summary>
        private void ValidateInput()
        {
            // Update the interactable state based on whether the fields are non-empty
            isLoginButtonInteractable = !string.IsNullOrEmpty(loginManager.UsernameInput.text) && !string.IsNullOrEmpty(loginManager.PasswordInput.text);
            isRegisterButtonInteractable = !string.IsNullOrEmpty(userRegistrationManager.UsernameInput.text) && !string.IsNullOrEmpty(userRegistrationManager.PasswordInput.text);

            // Update the visibility of the buttons
            ToggleButtonVisibility(loginButton, isLoginButtonInteractable);
            ToggleButtonVisibility(registerButton, isRegisterButtonInteractable);
        }
        
        /// <summary>
        /// This method is triggered when the anonymous login toggle is changed.
        /// </summary>
        /// <param name="isAnonLogin">True if anonymous login is enabled, false otherwise.</param>
        private void OnAnonLoginToggleChanged(bool isAnonLogin)
        {
            authenticationManager.SetUseAnonymousLogin(isAnonLogin); // Switch between anonymous and username/password login
        }
        
        /// <summary>
        /// Sets the visibility of a button and triggers hover effects if applicable.
        /// </summary>
        /// <param name="buttonImage">The button image to toggle.</param>
        /// <param name="isVisible">Whether the button should be visible.</param>
        private void ToggleButtonVisibility(Image buttonImage, bool isVisible)
        {
            buttonImage.enabled = isVisible; // Enable or disable the image component

            if (isVisible)
            {
                loginButtonHoverEffect.HoverEnter();
                loginButtonHoverEffect.HoverExit();
            }
        }
        
        /// <summary>
        /// Attempts to log the user in using the provided credentials.
        /// </summary>
        public void TryLogin()
        {
            if (isLoginButtonInteractable)
            {
                string username = loginManager.UsernameInput.text;
                string password = loginManager.PasswordInput.text;
                
#if UNITY_EDITOR
                // During development, allow either anon or username/password based on the toggle
                if (anonLoginToggle.isOn)
                {
                    authenticationManager.SignInAnonymouslyAsync(); // Anonymous login
                }
                else
                {
                    authenticationManager.SignInWithUsernamePasswordAsync(username, password); // Username/password login
                }
#else
                // In build, force username/password login
                authenticationManager.SignInWithUsernamePasswordAsync(username, password);
#endif

                if (loginManager.ValidateLogin(username, password))
                {
                    Debug.Log("Login successful: " + username);
                    SceneManager.LoadScene(SceneNames.Start);
                    GameManager.Instance.CurrentUser = username;
                }
                else
                {
                    panelBlinkEffect.TriggerBlink(Color.red);
                    Debug.LogError("Login failed for: " + username);
                }
            }
        }

        /// <summary>
        /// Attempts to register a new user with the provided credentials.
        /// </summary>
        public void TryRegister()
        {
            if (isRegisterButtonInteractable)
            {
                // remove eventual spaces before/after username
                string username = userRegistrationManager.UsernameInput.text.Trim(); 
                string password = userRegistrationManager.PasswordInput.text;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    Debug.LogError("Username and password cannot be empty.");
                    return;
                }
                
                Debug.Log("User register: " + username);
                userRegistrationManager.RegisterUser(username, password);
                
                userRegistrationManager.ClearInputFields();
            }
        }
    }
}
