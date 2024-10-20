using UnityEngine;
using UnityEngine.UI;

public class DynamicSpacing : MonoBehaviour
{
    public HorizontalLayoutGroup layoutGroup;  // Reference to the Horizontal Layout Group
    public RectTransform shopPanelParent;      // Parent object containing the ShopPanel items
    public float maxSpacing = 20f;             // Maximum spacing between ShopPanel items
    public float minSpacing = 5f;              // Minimum spacing between ShopPanel items
    public float panelWidth = 100f;            // Set your ShopPanel width here (if uniform)

    void Start()
    {
        AdjustSpacing();
    }

    void AdjustSpacing()
    {
        int itemCount = shopPanelParent.childCount; // Get the number of ShopPanels
        
        if (itemCount > 0)
        {
            // Get the width of the parent container (shopPanelParent)
            float parentWidth = shopPanelParent.rect.width;

            // Total width occupied by all items
            float totalItemWidth = itemCount * panelWidth;

            // Remaining space in the parent container after accounting for the items
            float remainingWidth = parentWidth - totalItemWidth;

            // Spacing between each item = remainingWidth divided by (itemCount - 1) for equal distribution
            float newSpacing = remainingWidth / (itemCount - 1);

            // Clamp the spacing between minSpacing and maxSpacing
            newSpacing = Mathf.Clamp(newSpacing, minSpacing, maxSpacing);

            // Apply the new spacing to the Horizontal Layout Group
            layoutGroup.spacing = newSpacing;

            Debug.Log("New Spacing: " + newSpacing);
        }
    }
}
