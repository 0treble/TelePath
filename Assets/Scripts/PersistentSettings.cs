using UnityEngine;

public class PersistentSettings : MonoBehaviour
{
    public static PersistentSettings Instance { get; private set; }

    private void Awake()
    {
        // Ensures that PersistentSettings object persists across scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
    }

    // Settings as public variables for easy access
    public float[] frequencies = { 8f, 12f, 15f, 10f }; // Default frequencies 8, 14.4, 18, 12
    public int[] labels = { 1, 3, 4, 2 }; // Default labels
    public float restDuration = 12f; // Default rest duration
    public float stimulusDuration = 12f; // Default stimulus duration
    public int numTrials = 1; // Default number of trials

    // UDP Settings
    public string udpAddress = "192.168.25.174"; // Default UDP address
    public int udpSendPort = 5005; // Default UDP send port
    public int udpReceivePort = 5006; // Default UDP receive port

    private void Start()
    {
        LoadSettings(); // Ensure settings are loaded on start
    }

    // Load settings from PlayerPrefs
    public void LoadSettings()
    {
        frequencies[0] = PlayerPrefs.GetFloat("Frequency1", 8f);
        frequencies[1] = PlayerPrefs.GetFloat("Frequency2", 12f);
        frequencies[2] = PlayerPrefs.GetFloat("Frequency3", 15f);
        frequencies[3] = PlayerPrefs.GetFloat("Frequency4", 10f);

        labels[0] = PlayerPrefs.GetInt("Label1", 1);
        labels[1] = PlayerPrefs.GetInt("Label2", 3);
        labels[2] = PlayerPrefs.GetInt("Label3", 4);
        labels[3] = PlayerPrefs.GetInt("Label4", 2);

        restDuration = PlayerPrefs.GetFloat("RestDuration", 12f);
        stimulusDuration = PlayerPrefs.GetFloat("StimulusDuration", 12f);
        numTrials = PlayerPrefs.GetInt("NumTrials", 3);

        udpAddress = PlayerPrefs.GetString("UdpAddress", "192.168.25.174");
        udpSendPort = PlayerPrefs.GetInt("UdpSendPort", 5005);
        udpReceivePort = PlayerPrefs.GetInt("UdpReceivePort", 5006);

        Debug.Log("Settings Loaded!");
    }

    // Setters for saving the values
    public void SetFrequencies(float[] frequencies)
    {
        PlayerPrefs.SetFloat("Frequency1", frequencies[0]);
        PlayerPrefs.SetFloat("Frequency2", frequencies[1]);
        PlayerPrefs.SetFloat("Frequency3", frequencies[2]);
        PlayerPrefs.SetFloat("Frequency4", frequencies[3]);
        SaveSettings(); // Save after setting values
    }

    public void SetLabels(int[] labels)
    {
        PlayerPrefs.SetInt("Label1", labels[0]);
        PlayerPrefs.SetInt("Label2", labels[1]);
        PlayerPrefs.SetInt("Label3", labels[2]);
        PlayerPrefs.SetInt("Label4", labels[3]);
        SaveSettings(); // Save after setting values
    }

    public void SetRestDuration(float restDuration)
    {
        PlayerPrefs.SetFloat("RestDuration", restDuration);
        SaveSettings(); // Save after setting values
    }

    public void SetStimulusDuration(float stimulusDuration)
    {
        PlayerPrefs.SetFloat("StimulusDuration", stimulusDuration);
        SaveSettings(); // Save after setting values
    }

    public void SetNumTrials(int numTrials)
    {
        PlayerPrefs.SetInt("NumTrials", numTrials);
        SaveSettings(); // Save after setting values
    }

    public void SetUdpAddress(string address)
    {
        PlayerPrefs.SetString("UdpAddress", address);
        SaveSettings(); // Save after setting values
    }

    public void SetUdpSendPort(int port)
    {
        PlayerPrefs.SetInt("UdpSendPort", port);
        SaveSettings(); // Save after setting values
    }

    public void SetUdpReceivePort(int port)
    {
        PlayerPrefs.SetInt("UdpReceivePort", port);
        SaveSettings(); // Save after setting values
    }

    // Save settings to PlayerPrefs
    public void SaveSettings()
    {
        PlayerPrefs.Save();
    }
}
