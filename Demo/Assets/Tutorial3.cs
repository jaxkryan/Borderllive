using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections;
using UnityEngine.UI;

public class Tutorial3 : MonoBehaviour
{
    private GameObject player;
    private BerserkGauge berserkGauge;

    public GameObject canvas;
    public Image berserkImage;
    public TMP_Text text1;
    public TMP_Text text2;
    public TMP_Text text3;

    public LocalizedString localizedText1;  // Localized text for 1st message
    public LocalizedString localizedText2;  // Localized text for 2nd message
    public LocalizedString localizedText3;  // Localized text for 3rd message

    private bool isTextSequenceRunning = false;

    void Start()
    {
        // Find the player and its BerserkGauge component
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            berserkGauge = player.GetComponentInChildren<BerserkGauge>();
        }

        if (text1 == null || text2 == null || text3 == null)
        {
            Debug.LogError("One or more TMP_Text references are not assigned!");
        }

        // Initially hide all texts
        text2.gameObject.SetActive(false);
        text3.gameObject.SetActive(false);

        // Load localized text values
        AssignLocalizedText();
    }

    void Update()
    {
        if (berserkGauge != null)
        {
            if (berserkGauge._isBerserkActive && !isTextSequenceRunning)
            {
                // Hide image and 1st text, and start the sequence for 2nd and 3rd text
                text1.gameObject.SetActive(false);
                berserkImage.gameObject.SetActive(false);

                // Start coroutine only once
                StartCoroutine(ShowTextSequence());
            }
            else if (!berserkGauge._isBerserkActive && isTextSequenceRunning)
            {
                // Stop coroutine if berserk becomes inactive and reset the canvas
                StopAllCoroutines();
                ResetCanvas();
            }
        }
    }

    private IEnumerator ShowTextSequence()
    {
        isTextSequenceRunning = true;

        // Show the 2nd text
        text2.gameObject.SetActive(true);

        // Wait for 6 seconds
        yield return new WaitForSeconds(6f);

        // Ensure text2 is hidden before showing text3
        text2.gameObject.SetActive(false);

        // Wait for one frame to ensure that text2 is fully hidden
        yield return null;

        // Show the 3rd text
        text3.gameObject.SetActive(true);

        // isTextSequenceRunning = false;
    }


    private void ResetCanvas()
    {
        // Reset the UI
        text1.gameObject.SetActive(true);
        text2.gameObject.SetActive(false);
        text3.gameObject.SetActive(false);

        isTextSequenceRunning = false;
    }

    private void AssignLocalizedText()
    {
        // Use the Localization system to assign the correct localized strings
        localizedText1.StringChanged += (string localizedValue) => text1.text = localizedValue;
        localizedText2.StringChanged += (string localizedValue) => text2.text = localizedValue;
        localizedText3.StringChanged += (string localizedValue) => text3.text = localizedValue;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canvas.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        canvas.SetActive(false);
    }
}
