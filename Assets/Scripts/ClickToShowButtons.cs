using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ClickToShowButtons : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;

    [System.Serializable]
    public class ButtonMapping
    {
        public string tag;
        public List<Button> uiButtons;
    }

    [SerializeField] private List<ButtonMapping> buttonMappings = new List<ButtonMapping>();

    private Dictionary<string, List<Button>> buttonDictionary = new Dictionary<string, List<Button>>();
    private List<Button> lastActiveButtons = new List<Button>();
    private GameObject lastClickedObject = null; // Store last clicked object

    void Start()
    {
        foreach (var mapping in buttonMappings)
        {
            if (!buttonDictionary.ContainsKey(mapping.tag))
            {
                buttonDictionary.Add(mapping.tag, mapping.uiButtons);
            }

            foreach (var button in mapping.uiButtons)
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI()) 
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                string hitTag = hit.collider.tag;
                Debug.Log($"🖱 Clicked on: {hit.collider.gameObject.name}, Tag: {hitTag}");

                if (buttonDictionary.ContainsKey(hitTag))
                {
                    HideLastButtons();
                    lastClickedObject = hit.collider.gameObject;

                    Debug.Log($"📌 Stored lastClickedObject: {lastClickedObject.name}, Tag: {lastClickedObject.tag}");

                    // ✅ Store the correct tag at the moment of clicking
                    foreach (var button in buttonDictionary[hitTag])
                    {
                        button.gameObject.SetActive(true);
                        lastActiveButtons.Add(button);

                        // ✅ Remove previous listeners to avoid duplicate calls
                        button.onClick.RemoveAllListeners();
                        button.onClick.AddListener(() => OnButtonClick(lastClickedObject.tag));
                    }

                    MouseControl.canMoveCamera = false;
                }
                else
                {
                    HideLastButtons();
                }
            }
            else
            {
                HideLastButtons();
            }
        }
    }



    void HideLastButtons()
    {
        foreach (var button in lastActiveButtons)
        {
            button.gameObject.SetActive(false);
        }
        lastActiveButtons.Clear();

        MouseControl.canMoveCamera = true; // ✅ Re-enable camera movement when buttons are hidden
    }

    void OnButtonClick(string tag)
{
    if (lastClickedObject != null) 
    {
        // Find the list of buttons for the clicked tag
        if (buttonDictionary.ContainsKey(tag))
        {
            List<Button> buttons = buttonDictionary[tag];

            // Check if the clicked button is the first one in the list for this tag
            Button clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

            if (clickedButton != null && buttons[0] == clickedButton) // Check if it's the first button in the list
            {
                Debug.Log($"🔘 First button in the list clicked for tag: {tag}");

                // Perform a specific action for the first button, if needed
                if(lastClickedObject.tag == "BeforeOnTheLinePlane")
                {
                    ObjectActionHandler.Instance.PerformAction(lastClickedObject, tag, "Line Up");
                }   
                else
                {
                    ObjectActionHandler.Instance.PerformAction(lastClickedObject, tag);
                    Debug.Log($"🔘 First Button Clicked! Triggering Action for: {lastClickedObject.name}, Tag: {tag}");
                }
            }
            else
            {
                // Perform the regular action for other buttons
                ObjectActionHandler.Instance.PerformAction(lastClickedObject, tag);
                Debug.Log($"🔘 Button Clicked! Triggering Action for: {lastClickedObject.name}, Tag: {tag}");
            }
        }

        HideLastButtons();
    }
    else
    {
        Debug.LogError($"⚠️ Button Clicked, but no object is stored!");
    }
}

    private bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}
