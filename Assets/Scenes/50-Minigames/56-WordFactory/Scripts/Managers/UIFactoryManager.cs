using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.Managers
{
    /// <summary>
    /// Enum for representing the word check states.
    /// </summary>
    public enum WordCheckState
    {
        Correct,
        Repeated,
        Wrong
    }

    /// <summary>
    /// Manages UI changes for info board and lights during the Word Factory game.
    /// </summary>
    public class UIFactoryManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI correctWordText;
        [SerializeField] private TextMeshProUGUI wrongWordText; 
        [SerializeField] private Image infoBoardHolder;  
        [SerializeField] private Image lightHolder;     

        [SerializeField] private Sprite infoBoardBaseSprite;      
        [SerializeField] private Sprite infoBoardCorrectSprite;   
        [SerializeField] private Sprite infoBoardUsedSprite;      
        [SerializeField] private Sprite infoBoardWrongSprite;     

        [SerializeField] private Sprite lightBase;     
        [SerializeField] private Sprite lightCorrect1;   
        [SerializeField] private Sprite lightCorrect2;   
        [SerializeField] private Sprite lightRepeated1;  
        [SerializeField] private Sprite lightRepeated2;  
        [SerializeField] private Sprite lightWrong1;     
        [SerializeField] private Sprite lightWrong2;     

        private Coroutine lightCoroutine;              

        /// <summary>
        /// Updates the info board based on the given state and resets after a delay.
        /// </summary>
        /// <param name="state">The state to set on the info board (Correct, Repeated, Wrong).</param>
        public void UpdateInfoBoard(WordCheckState state)
        {
            switch (state)
            {
                case WordCheckState.Repeated:
                    infoBoardHolder.sprite = infoBoardUsedSprite;
                    break;
                case WordCheckState.Wrong:
                    infoBoardHolder.sprite = infoBoardWrongSprite;
                    break;
                case WordCheckState.Correct:
                    infoBoardHolder.sprite = infoBoardCorrectSprite;
                    break;
                default:
                    Debug.LogWarning("Invalid state passed to UpdateInfoBoard.");
                    return;
            }

            StartCoroutine(ResetInfoBoard(5f));
        }
        
        // Call this method whenever the count changes
        public void UpdateWordCounts(int correct, int total, int wrong)
        {
            correctWordText.text = $"Ord: {correct}/{total}";
            wrongWordText.text = $"Fejl: {wrong}/3";
        }

        /// <summary>
        /// Resets the info board image to its default state after a delay.
        /// </summary>
        private IEnumerator ResetInfoBoard(float delay)
        {
            yield return new WaitForSeconds(delay);
            infoBoardHolder.sprite = infoBoardBaseSprite; 
        }

        /// <summary>
        /// Triggers light blinking based on the provided state.
        /// </summary>
        /// <param name="state">The state of the light (Correct, Repeated, Wrong).</param>
        /// <param name="blinkCount">How many times the light should blink.</param>
        public void TriggerLightBlink(WordCheckState state, int blinkCount)
        {
            // Stop any ongoing light blink coroutine
            if (lightCoroutine != null)
            {
                StopCoroutine(lightCoroutine);
            }

            switch (state)
            {
                case WordCheckState.Wrong:
                    lightCoroutine = StartCoroutine(
                        BlinkLight(blinkCount,
                            lightWrong1,
                            lightWrong2));
                    break;
                case WordCheckState.Repeated:
                    lightCoroutine = StartCoroutine(
                        BlinkLight(blinkCount,
                            lightRepeated1,
                            lightRepeated2));
                    break;
                case WordCheckState.Correct:
                    lightCoroutine = StartCoroutine(
                        BlinkLight(blinkCount,
                            lightCorrect1,
                            lightCorrect2));
                    break;
                default:
                    Debug.LogWarning("Invalid state passed to TriggerLightBlink.");
                    break;
            }
        }

        /// <summary>
        /// Coroutine for blinking the light a certain number of times.
        /// </summary>
        private IEnumerator BlinkLight(int blinkCount, Sprite sprite1, Sprite sprite2)
        {
            for (int i = 0; i < blinkCount; i++)
            {
                lightHolder.sprite = sprite1;
                yield return new WaitForSeconds(0.5f);
                lightHolder.sprite = sprite2;
                yield return new WaitForSeconds(0.5f);
            }

            lightHolder.sprite = lightBase;  // Reset to default light
        }
    }
}