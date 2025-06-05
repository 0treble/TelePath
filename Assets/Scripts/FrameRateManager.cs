using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    private static FrameRateManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
            Application.targetFrameRate = 72; // Set frame rate to 72 FPS
            QualitySettings.vSyncCount = 0; // Disable V-Sync
            Time.fixedDeltaTime = 1f / 72;
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate instances
        }
    }
}
