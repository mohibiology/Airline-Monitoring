using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ClickToShowButtons : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;  

    [System.Serializable]
    public class ButtonMapping
    {
        public string tag;            // Object's tag (e.g., "NPC", "Chest", "Enemy")
        public List<Button> uiButtons; // List of UI buttons for that object
    }

    [SerializeField] private List<ButtonMapping> buttonMappings = new List<ButtonMapping>();

    private Dictionary<string, List<Button>> buttonDictionary = new Dictionary<string, List<Button>>();
    private List<Button> lastActiveButtons = new List<Button>(); // Stores last active buttons

    void Start()
    {
        // Convert List to Dictionary for fast lookups
        foreach (var mapping in buttonMappings)
        {
            if (!buttonDictionary.ContainsKey(mapping.tag))
            {
                buttonDictionary.Add(mapping.tag, mapping.uiButtons);
                foreach (var button in mapping.uiButtons)
                {
                    button.gameObject.SetActive(false); // Hide all buttons at start
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                string hitTag = hit.collider.tag;

                if (buttonDictionary.ContainsKey(hitTag)) // Check if buttons exist for this tag
                {
                    // Hide previous buttons
                    HideLastButtons();

                    // Show new buttons
                    foreach (var button in buttonDictionary[hitTag])
                    {
                        button.gameObject.SetActive(true);
                        lastActiveButtons.Add(button); // Store active buttons
                    }
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
    }
}
