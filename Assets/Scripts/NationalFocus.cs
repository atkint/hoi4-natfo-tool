using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NationalFocus : MonoBehaviour
{
    public NFContainer nationalNFContainer;
    public FocusParser parser;
    public GameObject focusButtonPrefab;
    public RectTransform scrollContent;
    public Transform focusTransform;
    public UIFocusEditor editorPanel;

    public Dictionary<string, FocusButton> focusButtons = new Dictionary<string, FocusButton>();

    float xOffset = 96f;
    float yOffset = 96f;

    private void Start()
    {
        editorPanel = FindObjectOfType<UIFocusEditor>();

        nationalNFContainer = parser.ReadFile(@"C:\Users\Tim\Desktop\hoi4-natfo-tool\Assets\Example\france.txt");
        GameObjectify(nationalNFContainer);
        RefreshFocusButtons();
    }

    public GameObject GameObjectify(NFContainer container)
    {
        var containerName = container.ID == "" ? container.Name : container.Name + " (" + container.ID + ")";
        GameObject containerGO = new GameObject(containerName);

        foreach (NFElement e in container.Elements)
        {
            GameObject elementGO;

            if (e.GetType() == typeof(NFVariable))
            {
                elementGO = new GameObject(e.Name+" = "+((NFVariable)e).Value);
            }
            else if (e.GetType() == typeof(NFContainer))
            {
                NFContainer c = (NFContainer)e;
                elementGO = GameObjectify(c);
                if (e.Name == "focus")
                {
                    //Debug.Log(c.ID);
                    GameObject newFocus = Instantiate(focusButtonPrefab, focusTransform);
                    FocusButton fb = newFocus.GetComponent<FocusButton>();
                    fb.container = c;

                    focusButtons.Add(c.ID, fb);
                    fb.GetComponent<Button>().onClick.AddListener(() => FocusSelected(fb));
                }
            }
            else
            {
                elementGO = new GameObject();
            }

            elementGO.transform.SetParent(containerGO.transform);
        }

        
        return containerGO;
    }

    // Focuses have a parent from which their position is offset
    public void RefreshFocusButtons()
    {
        // Diagnostic variables, might be useful if large trees go below 0 x
        float lowestX = 0;
        float highestX = 0;
        float lowestY = 0;
        float highestY = 0;

        foreach (FocusButton fb in focusButtons.Values)
        {
            fb.name = fb.container.Name;
            if (!string.IsNullOrEmpty(fb.container.RelativePositionID))
            {
                fb.transform.SetParent(focusButtons[fb.container.RelativePositionID].transform,true);
            }
            
            fb.transform.localPosition = new Vector3(fb.container.X * xOffset, fb.container.Y * -yOffset);
            
            // X and Y can be negative so we need to offset the focuses to be on the screen
            lowestX = fb.transform.position.x < lowestX ? fb.transform.position.x : lowestX;
            highestX = fb.transform.position.x > highestX ? fb.transform.position.x : highestX;
            lowestY = fb.transform.position.y < lowestY ? fb.transform.position.y : lowestY;
            highestY = fb.transform.position.y > highestY ? fb.transform.position.y : highestY;
        }
        scrollContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, highestX+xOffset);
        scrollContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (lowestY - yOffset)* -1);

        
        Debug.Log(string.Format("Lowest\nX: {0} - {1} Y: {2} - {3}", lowestX, highestX, lowestY, highestY));
    }

    void FocusSelected(FocusButton fb)
    {
        editorPanel.gameObject.SetActive(true);
        editorPanel.SetFocus(fb);
    }
}
