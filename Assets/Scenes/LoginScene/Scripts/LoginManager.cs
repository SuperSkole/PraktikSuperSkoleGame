using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes.LoginScene.Scripts
{
    public class LoginManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField usernameInput; 
        [SerializeField] private TMP_InputField passwordInput; 
        
    
        public TMP_InputField UsernameInput => usernameInput;
        public TMP_InputField PasswordInput => passwordInput;

        
    
        public void OnLoginButtonClicked()
        {
            string username = usernameInput.text; 
            string password = passwordInput.text; 
        
            // Early out if empty
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Debug.LogError("Username and password cannot be empty.");
                return;
            }

    
            Debug.Log("Login attempt: " + username);
            if (ValidateLogin(username, password))
            {
                Debug.Log("Login successful: " + username);
                // TODO: Send username to game 
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                Debug.LogError("Login failed: " + username);
            }
        
            usernameInput.text = "";
            passwordInput.text = "";
        }
    
        public bool ValidateLogin(string username, string inputPassword)
        {
            // Define path to file
            string userDataPath = Path.Combine(Application.dataPath, "CORE", "UserData");
            string path = Path.Combine(userDataPath, "users.txt");
            if (File.Exists(path))
            {
                // Read all lines in file
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    // split each line by ;
                    string[] data = line.Split(';');
                    // check split gives us 3 datatypes
                    if (data.Length == 3)
                    {
                        // assign each split to separate data
                        string storedUsername = data[0];
                        string storedHash = data[1];
                        string storedSalt = data[2];

                        // Checks if the hashed version of the input username, with the stored salt, matches the stored hashed username
                        if (GenerateHashManager.GenerateHash(username, storedSalt) == storedUsername)
                        {
                            // If the username is correct, it then checks if the hashed version of the input password, with the same stored salt,
                            // matches the stored hashed password, and return true/false
                            return GenerateHashManager.GenerateHash(inputPassword, storedSalt) == storedHash;
                        }

                    }
                }
            }
            return false;
        }
    }
}