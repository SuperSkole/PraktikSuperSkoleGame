using CORE;
using UI.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes.LoginScene.Scripts
{
    public class UILoginSceneManager : MonoBehaviour
    {
        [SerializeField] private LoginManager loginManager;
        [SerializeField] private UserRegistrationManager userRegistrationManager;
        
        [SerializeField] private GameObject loginScreen;
        
        [SerializeField] private Image panel;
        [SerializeField] private Image loginButton;
        [SerializeField] private Image registerButton;
        
        private bool isLoginButtonInteractable = false;
        private bool isRegisterButtonInteractable = false;
        
        // References to HoverEffectUI scripts attached to login button
        private HoverEffectUI loginButtonHoverEffect;
        private BlinkEffectUI panelBlinkEffect;

        //starter ud med login aktiveret f�rst
        private void Awake ()
        {
            loginScreen.SetActive(true);
            
            panelBlinkEffect = panel.GetComponent<BlinkEffectUI>();
            loginButtonHoverEffect = loginButton.GetComponent<HoverEffectUI>();
            
            ToggleButtonVisibility(loginButton, false);
            ToggleButtonVisibility(registerButton, false);
        }

        private void Start()
        {
            // Add listeners to the input fields to check for changes
            loginManager.UsernameInput.onValueChanged.AddListener(delegate { ValidateInput(); });
            loginManager.PasswordInput.onValueChanged.AddListener(delegate { ValidateInput(); });
            userRegistrationManager.UsernameInput.onValueChanged.AddListener(delegate { ValidateInput(); });
            userRegistrationManager.PasswordInput.onValueChanged.AddListener(delegate { ValidateInput(); });

        }

        private void ValidateInput()
        {
            // Update the interactable state based on whether the fields are non-empty
            isLoginButtonInteractable = !string.IsNullOrEmpty(loginManager.UsernameInput.text) && !string.IsNullOrEmpty(loginManager.PasswordInput.text);
            isRegisterButtonInteractable = !string.IsNullOrEmpty(userRegistrationManager.UsernameInput.text) && !string.IsNullOrEmpty(userRegistrationManager.PasswordInput.text);

            // Update the visibility of the buttons
            ToggleButtonVisibility(loginButton, isLoginButtonInteractable);
            ToggleButtonVisibility(registerButton, isRegisterButtonInteractable);
        }
        
        private void ToggleButtonVisibility(Image buttonImage, bool isVisible)
        {
            buttonImage.enabled = isVisible; // Enable or disable the image component

            if (isVisible)
            {
                loginButtonHoverEffect.HoverEnter();
                loginButtonHoverEffect.HoverExit();
            }
        }
        
        public void TryLogin()
        {
            if (isLoginButtonInteractable)
            {
                string username = loginManager.UsernameInput.text;
                string password = loginManager.PasswordInput.text;

                if (loginManager.ValidateLogin(username, password))
                {
                    Debug.Log("Login successful: " + username);
                    SceneManager.LoadScene("01-StartScene");
                    GameManager.Instance.CurrentUser = username;
                }
                else
                {
                    panelBlinkEffect.TriggerBlink(Color.red);
                    Debug.LogError("Login failed for: " + username);
                }
            }
        }

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
