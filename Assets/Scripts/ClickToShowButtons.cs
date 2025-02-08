using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
        if (Input.GetMouseButtonDown(0)) 
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
            Debug.Log($"🔘 Button Clicked! Received Tag: {tag}, Object's Actual Tag: {lastClickedObject.tag}");
            ObjectActionHandler.Instance.PerformAction(lastClickedObject, tag);
            HideLastButtons();
        }
    }
}
