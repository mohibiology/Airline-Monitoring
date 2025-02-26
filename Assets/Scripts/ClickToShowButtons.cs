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
        AssignButtonListeners();
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

                    List<Button> buttons = buttonDictionary[hitTag];

                    for (int i = 0; i < buttons.Count; i++)
                    {
                        Button button = buttons[i];
                        button.gameObject.SetActive(true);
                        lastActiveButtons.Add(button);

                        button.onClick.RemoveAllListeners(); // Remove previous listeners

                        int index = i; // Capture index for closure
                        button.onClick.AddListener(() => OnButtonClick(hitTag, index)); // ✅ Pass index & tag
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
    void AssignButtonListeners()
    {
        if (lastClickedObject == null) return;

        string tag = lastClickedObject.tag;
        if (!buttonDictionary.ContainsKey(tag)) return;

        List<Button> buttons = buttonDictionary[tag];

        for (int i = 0; i < buttons.Count; i++)
        {
            Button button = buttons[i];
            button.onClick.RemoveAllListeners(); // Clear old listeners to prevent duplicates

            // Capture index to determine which button was clicked
            int index = i;
            button.onClick.AddListener(() => OnButtonClick(tag,index));
        }
    }
    void OnButtonClick(string tag, int index)
    {
        if (lastClickedObject != null) 
        {
            // Ensure the tag exists in the dictionary
            if (buttonDictionary.ContainsKey(tag))
            {
                List<Button> buttons = buttonDictionary[tag];

                Debug.Log($"🔘 Button Index {index} clicked for tag: {tag}");

                // ✅ Ensure index is within valid range
                if (index >= buttons.Count)
                {
                    Debug.LogError($"⚠️ Invalid index {index} for tag {tag} (Only {buttons.Count} buttons available)");
                    return;
                }

                // ✅ If the clicked object is "BeforeOnTheLinePlane", handle both buttons
                if (tag == "BeforeOnTheLinePlane")
                {
                    if (index == 0)
                    {
                        ObjectActionHandler.Instance.PerformAction(lastClickedObject, tag, "Line Up");
                    }
                    else if (index == 1)
                    {
                        ObjectActionHandler.Instance.PerformAction(lastClickedObject, tag, "IMD Take Off");
                    }
                }
                else
                {
                    // ✅ For other tags, always trigger the first button's action
                    ObjectActionHandler.Instance.PerformAction(lastClickedObject, tag);
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
