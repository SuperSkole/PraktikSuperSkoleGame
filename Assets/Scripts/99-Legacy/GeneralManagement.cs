using System.Collections;
using System.Collections.Generic;
using _99_Legacy.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _99_Legacy
{
    public class GeneralManagement : MonoBehaviour
    {
        #region attributes
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private TextMeshProUGUI expText;
        [SerializeField] private TextMeshProUGUI lvlText;
        [SerializeField] private GameObject lvlHolder;//Level UP Text GO
        [SerializeField] private GameObject expSlider;

        public Dictionary<int, int> levelsDic = new Dictionary<int, int>();

        public int Level = 1;
        int numberOfLevels = 50; // Example: Create 50 levels
        // private int[] xpNeed = { 10, 12, 14, 17, 20 };//an increase of 20%

        public int goldAmount;
        public int expAmount;
        private int tmpXPHolder;
        private int tmpGoldHolder;
        #endregion


        /// <summary>
        /// when we get loading game up and runing change this so it takes in the loaded value, except if its a new game
        /// </summary>
        // private void Start()
        // {
        //     lvlHolder.SetActive(false);
        //     if (this.gameObject.GetComponent<SaveGameToJson>().IsThereSaveFile() == false)
        //     {
        //         UpdateValues();
        //     }
        //     if (levelsDic.Count <= 0)
        //     {
        //         PopulateXPRequirements(numberOfLevels);
        //     }
        // }
        /// <summary>
        /// Fills the Dictionary levelsDic with levels with a certain % 
        /// </summary>
        /// <param name="numberOfLevels"></param>
        private void PopulateXPRequirements(int numberOfLevels)
        {
            int baseXP = 10; // Starting XP requirement
            float incrementFactor = 1.20f; // 20% increment

            for (int i = 1; i <= numberOfLevels; i++)
            {
                levelsDic.Add(i, Mathf.RoundToInt(baseXP));
                baseXP = Mathf.RoundToInt(baseXP * incrementFactor);
            }
        }

        /// <summary>
        /// Adds an amount of XP to the player
        /// </summary>
        /// <param name="amount"></param>
        public void AddEXP(int amount)
        {
            tmpXPHolder = (expAmount + amount);

            Debug.Log($"GernalManagement/AddEXP/ {amount} of XP has been added");
        }
        public void UseXP()
        {
            StartCoroutine(IncrementXP(tmpXPHolder));
        }
        public void UseGold()
        {
            StartCoroutine(IncrementGold(tmpGoldHolder));
        }
        /// <summary>
        /// Adds gold that is displayed
        /// </summary>
        /// <param name="amount"></param>
        public void AddGold(int amount)
        {
            tmpGoldHolder = (goldAmount + amount);

            Debug.Log($"GernalManagement/AddGold/ {amount} of Gold has been added");
        }
        /// <summary>
        /// Removes gold and returns either "true" or "false" deppending on if the player has enough gold
        /// </summary>
        /// <param name="amount"></param>
        public bool RemoveGold(int amount)
        {
            if (CanAfford(amount))
            {
                goldAmount -= amount;
                goldText.text = goldAmount.ToString();
                this.gameObject.GetComponent<OldGameManager>().player.CurrentGoldAmount = goldAmount;
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// call this to check if the player has the money.
        /// </summary>
        /// <param name="amount"></param>
        public bool CanAfford(int amount)
        {
            //If there isnt enough gold
            if (amount > goldAmount)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Increment the XP bar, so it slowly fills up
        /// </summary>
        /// <param name="endAmount">At what point should it stop incrementing</param>
        /// <returns></returns>
        private IEnumerator IncrementXP(int endAmount)
        {
            //endAmount++;
            while (expAmount < endAmount)
            {
                yield return new WaitForSeconds(0.1f);
                expAmount++;
                this.gameObject.GetComponent<OldGameManager>().player.CurrentXPAmount = expAmount;
                int requiredXP;
                if (levelsDic.TryGetValue(Level, out requiredXP) && expAmount >= requiredXP)
                {
                    // Calculate remaining XP after leveling up
                    expAmount -= requiredXP;
                    endAmount -= requiredXP;
                    Level++;

                    // Update player information
                    var playertmp = GameObject.FindGameObjectWithTag("Player");
                    playertmp.GetComponent<PlayerWorldMovement>().PlayLevelUpEffect();
                    this.gameObject.GetComponent<OldGameManager>().player.CurrentLevel = Level;
                    UpdateValues();
                }
                // Ensure the UI is updated with the current XP amount
                expText.text = expAmount.ToString() + "/" + requiredXP.ToString();
                expSlider.GetComponent<Slider>().value = expAmount;
            }
        }
        /// <summary>
        /// Increment the Gold, so it slowly fills up
        /// </summary>
        /// <param name="endAmount">>At what point should it stop incrementing</param>
        /// <returns></returns>
        private IEnumerator IncrementGold(int endAmount)
        {
            //endAmount++;
            while (goldAmount < endAmount)
            {
                yield return new WaitForSeconds(0.1f);
                goldAmount++;
                goldText.text = goldAmount.ToString();
                this.gameObject.GetComponent<OldGameManager>().player.CurrentGoldAmount = goldAmount;

            }
        }

        /// <summary>
        /// Updates all the displayed variables 
        /// </summary>
        public void UpdateValues()
        {
            //print(Level);
            if (levelsDic.Count == 0)
            {
                PopulateXPRequirements(numberOfLevels);
            }
            int requiredXP;
            levelsDic.TryGetValue(Level, out requiredXP);
            expText.text = expAmount.ToString() + "/" + requiredXP.ToString();
            goldText.text = goldAmount.ToString();
            lvlText.text = Level.ToString();
            expSlider.GetComponent<Slider>().maxValue = requiredXP;
            expSlider.GetComponent<Slider>().value = expAmount;


        }
        public void EnableLvlTxt(bool value)
        {
            lvlHolder.SetActive(value);
        }
    }
}
