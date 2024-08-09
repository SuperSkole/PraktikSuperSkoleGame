using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.Serialization;
using TMPro;

namespace Scenes.LoginScene.Scripts
{
    public class UserRegistrationManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField usernameInput; 
        [SerializeField] private TMP_InputField passwordInput; 
        [SerializeField] private Button registerButton;
    
        void Start()
        {
            // Initially disable the button
            registerButton.interactable = false;

            // Add listeners to the input fields to check for changes
            usernameInput.onValueChanged.AddListener(delegate { ValidateInput(); });
            passwordInput.onValueChanged.AddListener(delegate { ValidateInput(); });
        }

        void ValidateInput()
        {
            // Check if both fields are non-empty
            registerButton.interactable = !string.IsNullOrEmpty(usernameInput.text) && !string.IsNullOrEmpty(passwordInput.text);
        }
        
        
        
        public void OnRegisterButtonClicked()
        {
            // remove evtual spaces before/after username
            string username = usernameInput.text.Trim(); 
            string password = passwordInput.text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Debug.LogError("Username and password cannot be empty.");
                return;
            }
            
            Debug.Log("User register: " + username);
            RegisterUser(username, password);
            
            usernameInput.text = "";
            passwordInput.text = "";
        }
        
        

        private void RegisterUser(string username, string password)
        {
            // Create a 16 bytes salt
            string salt = GenerateHashManager.GenerateSalt(16); 
            // Hash password with ´salt
            string hashedPassword = GenerateHashManager.GenerateHash(password, salt);
            // Hash username with ´salt
            string hashedUsername = GenerateHashManager.GenerateHash(username, salt);
            
            SaveUserToTxtFile(hashedUsername, hashedPassword, salt);
        }

        private void SaveUserToTxtFile(string hashedUsername, string hashedPassword, string salt)
        {
            string path = Application.dataPath + "/users.txt";
            //string path = Application.persistentDataPath + "/users.txt";
            string userData = hashedUsername + ";" + hashedPassword + ";" + salt + "\n";
            File.AppendAllText(path, userData);
            Debug.Log("User saved successfully.");
        }
    }
}