using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.Minigames.MiniRacingGame.Scripts
{
    public class CameraController : MonoBehaviour
    {

        public GameObject car; //player component dragged on the script here in the editor
        private Vector3 offset;

        /// <summary>
        /// Set the camera offset.
        /// </summary>
        void Start()
        {
            offset = transform.position - car.transform.position; //Camera transform pos - player transform pos = offset
        }

        /// <summary>
        /// Keeps up the camera distance from player.
        /// </summary>
        // Update is called once per frame
        void LateUpdate() //late update used because we want the physics of the player to run first
        {
            transform.position = car.transform.position + offset;
        }
    }
}

