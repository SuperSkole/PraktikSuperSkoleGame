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
            // if we are in editor we can use toggle
            // TODO we might want to let cheat activate toogle in build
#if UNITY_EDITOR
            anonLoginToggle.gameObject.SetActive(true);
            anonLoginToggle.onValueChanged.AddListener(OnAnonLoginToggleChanged);
#else
            anonLoginToggle.gameObject.SetActive(false);
#endif

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
            isLoginButtonInteractable = !string.IsNullOrEmpty(loginManager.UsernameInput.text) && !string.IsNullOrEmpty(loginManager.PasswordInput.text);
            isRegisterButtonInteractable = !string.IsNullOrEmpty(userRegistrationManager.UsernameInput.text) && !string.IsNullOrEmpty(userRegistrationManager.PasswordInput.text);

#if UNITY_EDITOR
            if (anonLoginToggle.isOn)
            {
                ToggleButtonVisibility(loginButton, isLoginButtonInteractable = true);
            }
            else
#endif
            {
                ToggleButtonVisibility(loginButton, isLoginButtonInteractable);
            }
            
            ToggleButtonVisibility(registerButton, isRegisterButtonInteractable);
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
            if (isLoginButtonInteractable)
            {
                string username = loginManager.UsernameInput.text;
                string password = loginManager.PasswordInput.text;

#if UNITY_EDITOR
                if (anonLoginToggle.isOn)
                {
                    // Anonymous login in editor mode
                    authenticationManager.SignInAnonymouslyAsync();
                    GameManager.Instance.CurrentUser = "TEST";
                    SceneManager.LoadScene(SceneNames.Start);
                }
                else
#endif
                {
                    // Username/password login
                    bool signInSuccessful = await authenticationManager.SignInWithUsernamePasswordAsync(username, password);

                    if (signInSuccessful) 
                    {
                        Debug.Log("Login successful: " + username);
                        GameManager.Instance.CurrentUser = username;
                        SceneManager.LoadScene(SceneNames.Start);
                    }
                    else
                    {
                        panelBlinkEffect.TriggerBlink(Color.red);
                        Debug.LogError("Login failed for: " + username);
                    }
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
                string username = userRegistrationManager.UsernameInput.text.Trim(); 
                string password = userRegistrationManager.PasswordInput.text;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    Debug.LogError("Username and password cannot be empty.");
                    return;
                }

                Debug.Log("Trying User register: " + username);
                
                authenticationManager.SignUpWithUsernamePasswordAsync(username, password);
                
                userRegistrationManager.ClearInputFields();
            }
        }
    }
}
