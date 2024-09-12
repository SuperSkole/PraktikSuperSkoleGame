using System.Collections.Generic;
using System.Linq;
using Scenes._10_PlayerScene.Scripts;
using UnityEngine;

namespace Scenes._20_MainWorld.Scripts.Car
{
    public class IsCarFlipped : MonoBehaviour
    {
        CarEvents carEvents;
        CarEventsManager carMa;
        bool eventSet;
        float timer;
        [SerializeField] private CarSaveTPPoint carSaveTPPoint;

        private void Awake()
        {
            carEvents = gameObject.GetComponent<CarEvents>();
            carMa = GetComponent<CarEventsManager>();  
        }
        // Update is called once per frame
        void FixedUpdate()
        {
            timer += Time.deltaTime;
            
            if (timer > 2f && !eventSet && carSaveTPPoint.RayPointDic.All(b => b.Value.isWheelTouching == false))
            {
                if (carMa.CarInteraction != null)
                {
                    carMa.CarInteraction.RemoveAllListeners();
                }

                carMa.CarInteraction.AddListener(FlipCarEvent);
                carMa.interactionIcon.SetActive(true);

                eventSet = true;

            }
            if (timer > 2f && eventSet && carSaveTPPoint.RayPointDic.All(b => b.Value.isWheelTouching == true))
            {
                if (carMa.CarInteraction != null)
                {
                    carMa.CarInteraction.RemoveAllListeners();
                }
                carMa.interactionIcon.SetActive(false);
                carMa.CarInteraction.AddListener(carEvents.TurnOffCar);
            }
        }
        public void FlipCarEvent()
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z);
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            if (carMa.CarInteraction != null)
            {
                carMa.CarInteraction.RemoveAllListeners();
            }
            carMa.CarInteraction.AddListener(carEvents.TurnOffCar);
            eventSet = false;
            timer = 0;
        }
    }
}
