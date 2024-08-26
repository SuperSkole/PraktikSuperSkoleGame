using System;
using System.IO;
using CORE;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes._00_LoginScene.Scripts
{
    /// <summary>
    /// Manages the login process, including validating user credentials against stored data.
    /// </summary>
    public class LoginManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField usernameInput; 
        [SerializeField] private TMP_InputField passwordInput; 
    
        public TMP_InputField UsernameInput => usernameInput;
        public TMP_InputField PasswordInput => passwordInput;

        /// <summary>
        /// Validates the login credentials by comparing the provided username and password against the stored data.
        /// </summary>
        /// <param name="username">The username provided by the user.</param>
        /// <param name="inputPassword">The password provided by the user.</param>
        /// <returns>Returns true if the login is successful, false otherwise.</returns>
        public bool ValidateLogin(string username, string inputPassword)
        {
            try
            {
                // Define the path to the file containing user data.
                string userDataPath = Path.Combine(Application.dataPath, "CORE", "UserData");
                string path = Path.Combine(userDataPath, "users.txt");
                
                if (File.Exists(path))
                {
                    // Read all lines in the file.
                    string[] lines = File.ReadAllLines(path);
                    foreach (string line in lines)
                    {
                        // Split each line by ';'.
                        string[] data = line.Split(';');
                        
                        // Ensure the line has exactly 3 elements (username, hash, salt).
                        if (data.Length == 3)
                        {
                            string storedUsername = data[0];
                            string storedHash = data[1];
                            string storedSalt = data[2];

                            // Validate the username and password.
                            if (username == storedUsername)
                            {
                                // Compare the hash of the input password with the stored hash.
                                return GenerateHashManager.GenerateHash(inputPassword, storedSalt) == storedHash;
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError("User data file not found.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"An error occurred while validating login: {ex.Message}");
            }

            return false;
        }
    }
}
