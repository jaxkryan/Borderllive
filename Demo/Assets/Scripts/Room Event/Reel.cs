using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Reel : MonoBehaviour
{
    public Sprite[] reelSymbols; // Array to hold different symbols
    public Image reelImage; // Image component of the reel
    public float spinDuration = 1.0f; // Duration of the spin

    public void Spin()
    {
        StartCoroutine(SpinReel());
    }

    private IEnumerator SpinReel()
    {
        float elapsedTime = 0f;
        while (elapsedTime < spinDuration)
        {
            // Randomly select a symbol during the spin
            int randomIndex = Random.Range(1, reelSymbols.Length);
            randomIndex = Random.Range(1, 100) > 60 ? 1 : randomIndex;
            randomIndex = Random.Range(1, 100) > 70 ? 2 : randomIndex;
            randomIndex = Random.Range(1, 100) > 80 ? 3 : randomIndex;
            randomIndex = Random.Range(1, 100) > 90 ? 0 : randomIndex;
            reelImage.sprite = reelSymbols[randomIndex];
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(0.05f); // Adjust the speed of the spinning
        }
    }
}
