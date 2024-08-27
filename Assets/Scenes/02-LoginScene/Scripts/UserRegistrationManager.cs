using System;
using System.IO;
using TMPro;
using UnityEngine;

namespace Scenes._00_LoginScene.Scripts
{
    /// <summary>
    /// Manages user registration by saving usernames and securely hashed passwords to a file.
    /// </summary>
    public class UserRegistrationManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField usernameInput; 
        [SerializeField] private TMP_InputField passwordInput;
        
        public TMP_InputField UsernameInput => usernameInput;
        public TMP_InputField PasswordInput => passwordInput;

        /// <summary>
        /// Registers a new user by saving their username and hashed password.
        /// </summary>
        /// <param name="username">The username provided by the user.</param>
        /// <param name="password">The password provided by the user.</param>
        public void RegisterUser(string username, string password)
        {
            string userDataPath = Path.Combine(Application.dataPath, "CORE", "UserData");
            string path = Path.Combine(userDataPath, "users.txt");
            
            // Check if the plaintext username already exists in the data file.
            if (UserExists(username, path))
            {
                Debug.LogError("User already exists.");
                return;
            }

            // if user doesnt exist we can Create a salt and hash the username and password 
            string salt = GenerateHashManager.GenerateSalt(16); 
            string hashedUsername = GenerateHashManager.GenerateHash(username, salt);
            string hashedPassword = GenerateHashManager.GenerateHash(password, salt);
           
            // Save the user with the plaintext username and hashed password.
            SaveUserToTxtFile(username, hashedPassword, salt);
        }
        
        /// <summary>
        /// Clears the input fields for username and password.
        /// </summary>
        public void ClearInputFields()
        {
            usernameInput.text = "";
            passwordInput.text = "";
        }

        /// <summary>
        /// Saves the user data to a text file.
        /// </summary>
        /// <param name="username">The plaintext username.</param>
        /// <param name="hashedPassword">The hashed password.</param>
        /// <param name="salt">The salt used in hashing the password.</param>
        private void SaveUserToTxtFile(string username, string hashedPassword, string salt)
        {            
            string userDataPath = Path.Combine(Application.dataPath, "CORE", "UserData");
            string path = Path.Combine(userDataPath, "users.txt");

            try
            {
                string userData = $"{username};{hashedPassword};{salt}\n";
                File.AppendAllText(path, userData);
                Debug.Log("User saved successfully.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save user data: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Checks if a user with the given username already exists.
        /// </summary>
        /// <param name="username">The plaintext username.</param>
        /// <param name="path">The file path where user data is stored.</param>
        /// <returns>True if the user exists, false otherwise.</returns>
        private bool UserExists(string username, string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    string[] lines = File.ReadAllLines(path);
                    foreach (string line in lines)
                    {
                        string[] data = line.Split(';');
                        if (data.Length == 3 && data[0] == username)
                        {
                            // User exists
                            return true; 
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to check if user exists: {ex.Message}");
                }
            }
            
            // No user found with that name
            return false; 
        }
    }
}