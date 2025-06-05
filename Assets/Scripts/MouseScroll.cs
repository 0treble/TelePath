using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseScroll : MonoBehaviour
{
    public Selectable[] buttons;  // Assign your buttons in the Inspector (Optional)
    public Button specialButton; // Assign if needed (Optional)

    private int currentIndex = 0;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip selectSound;
    public AudioClip clickSound;
    public AudioClip pickupSound;

    void Start()
    {
        if (buttons != null && buttons.Length > 0)
        {
            EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);
        }
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (buttons != null && buttons.Length > 0)
        {
            if (scroll > 0f) // Scroll Up
            {
                ChangeSelection(-1);
            }
            else if (scroll < 0f) // Scroll Down
            {
                ChangeSelection(1);
            }

            // Left-click activates the selected button
            if (Input.GetMouseButtonDown(0))
            {
                ClickSelectedButton();
            }
        }

        // Check if both left and right mouse buttons are pressed
        if (specialButton != null && Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            ClickSpecialButton();
        }
    }

    void ChangeSelection(int direction)
    {
        if (buttons == null || buttons.Length == 0) return;

        currentIndex = (currentIndex + direction + buttons.Length) % buttons.Length;
        EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);

        // Play select sound
        PlaySound(selectSound);
    }

    void ClickSelectedButton()
    {
        if (buttons == null || buttons.Length == 0) return;

        Button button = buttons[currentIndex].GetComponent<Button>();
        if (button != null)
        {
            button.onClick.Invoke();  // Simulate a button press
            PlaySound(clickSound);    // Play click sound
        }
    }

    void ClickSpecialButton()
    {
        specialButton?.onClick.Invoke(); // If specialButton is assigned, invoke it
        PlaySound(clickSound); // Play click sound for the special button
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
