using System.IO;
using Scenes.LoginScene.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInput; 
    [SerializeField] private TMP_InputField passwordInput; 
    [SerializeField] private Button loginButton;
    
    void Start()
    {
        // Initially disable the login button
        loginButton.interactable = false;

        // Add listeners to the input fields to check for changes
        usernameInput.onValueChanged.AddListener(delegate { ValidateInput(); });
        passwordInput.onValueChanged.AddListener(delegate { ValidateInput(); });
    }

    void ValidateInput()
    {
        // Check if both fields are non-empty
        loginButton.interactable = !string.IsNullOrEmpty(usernameInput.text) && !string.IsNullOrEmpty(passwordInput.text);
    }
    
    public void OnLoginButtonClicked()
    {
        string username = usernameInput.text; 
        string password = passwordInput.text; 
        
        // Earlu out if empty
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Username and password cannot be empty.");
            return;
        }

    
        Debug.Log("Login attempt: " + username);
        if (ValidateLogin(username, password))
        {
            Debug.Log("Login successful: " + username);
            SceneManager.LoadScene("MainMenu"); 
        }
        else
        {
            Debug.LogError("Login failed: " + username);
        }
        
        usernameInput.text = "";
        passwordInput.text = "";
    }
    
    private bool ValidateLogin(string username, string inputPassword)
    {
        // Define path to file
        string path = Application.persistentDataPath + "/users.txt";
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
                    // assign each split to seperate data
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