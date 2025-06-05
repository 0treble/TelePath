using TMPro;
using UnityEngine;

public class CalibrationSettingsMenu : MonoBehaviour
{
    // UI input fields
    public TMP_InputField restDurationInput;
    public TMP_InputField stimulusDurationInput;
    public TMP_InputField numTrialsInput;
    public TMP_InputField udpAddressInput;
    public TMP_InputField udpSendPortInput;
    public TMP_InputField udpReceivePortInput;

    // Frequency Input Fields
    public TMP_InputField frequency1Input;
    public TMP_InputField frequency2Input;
    public TMP_InputField frequency3Input;
    public TMP_InputField frequency4Input;

    public TextMeshProUGUI statusText;

    void Start()
    {
        // Load the persistent settings into the UI inputs
        var settings = PersistentSettings.Instance;

        restDurationInput.text = settings.restDuration.ToString();
        stimulusDurationInput.text = settings.stimulusDuration.ToString();
        numTrialsInput.text = settings.numTrials.ToString();

        udpAddressInput.text = settings.udpAddress;
        udpSendPortInput.text = settings.udpSendPort.ToString();
        udpReceivePortInput.text = settings.udpReceivePort.ToString();

        frequency1Input.text = settings.frequencies[0].ToString();
        frequency2Input.text = settings.frequencies[1].ToString();
        frequency3Input.text = settings.frequencies[2].ToString();
        frequency4Input.text = settings.frequencies[3].ToString();

        // Add listeners to update values when user edits
        restDurationInput.onEndEdit.AddListener(UpdateRestDuration);
        stimulusDurationInput.onEndEdit.AddListener(UpdateStimulusDuration);
        numTrialsInput.onEndEdit.AddListener(UpdateNumTrials);
        udpAddressInput.onEndEdit.AddListener(UpdateUDPAddress);
        udpSendPortInput.onEndEdit.AddListener(UpdateUDPSendPort);
        udpReceivePortInput.onEndEdit.AddListener(UpdateUDPReceivePort);
        frequency1Input.onEndEdit.AddListener(UpdateFrequency1);
        frequency2Input.onEndEdit.AddListener(UpdateFrequency2);
        frequency3Input.onEndEdit.AddListener(UpdateFrequency3);
        frequency4Input.onEndEdit.AddListener(UpdateFrequency4);
    }

    // Update methods
    void UpdateRestDuration(string newValue) => TryParseFloat(newValue, ref PersistentSettings.Instance.restDuration, "Rest Duration");
    void UpdateStimulusDuration(string newValue) => TryParseFloat(newValue, ref PersistentSettings.Instance.stimulusDuration, "Stimulus Duration");
    void UpdateNumTrials(string newValue) => TryParseInt(newValue, ref PersistentSettings.Instance.numTrials, "Number of Trials");
    void UpdateUDPAddress(string newValue)
    {
        statusText.text = $"Updating UDP Address to: {newValue}";
        PersistentSettings.Instance.udpAddress = newValue;
        PersistentSettings.Instance.SaveSettings(); // Save after updating the value
        statusText.text = $"UDP Address after update: {PersistentSettings.Instance.udpAddress}";
    }
    void UpdateUDPSendPort(string newValue) => TryParseInt(newValue, ref PersistentSettings.Instance.udpSendPort, "UDP Send Port");
    void UpdateUDPReceivePort(string newValue) => TryParseInt(newValue, ref PersistentSettings.Instance.udpReceivePort, "UDP Receive Port");
    void UpdateFrequency1(string newValue) => TryParseFloat(newValue, ref PersistentSettings.Instance.frequencies[0], "Frequency 1");
    void UpdateFrequency2(string newValue) => TryParseFloat(newValue, ref PersistentSettings.Instance.frequencies[1], "Frequency 2");
    void UpdateFrequency3(string newValue) => TryParseFloat(newValue, ref PersistentSettings.Instance.frequencies[2], "Frequency 3");
    void UpdateFrequency4(string newValue) => TryParseFloat(newValue, ref PersistentSettings.Instance.frequencies[3], "Frequency 4");

    // Helper methods for parsing
    void TryParseFloat(string input, ref float variable, string label)
    {
        if (float.TryParse(input, out float newValue))
        {
            variable = newValue;
            statusText.text = $"Updated {label}: {newValue}";
            PersistentSettings.Instance.SaveSettings(); // Save after updating the value
        }
        else
        {
            statusText.text = $"Invalid input for {label}";
        }
    }

    void TryParseInt(string input, ref int variable, string label)
    {
        if (int.TryParse(input, out int newValue))
        {
            variable = newValue;
            statusText.text = $"Updated {label}: {newValue}";
            PersistentSettings.Instance.SaveSettings(); // Save after updating the value
        }
        else
        {
            statusText.text = $"Invalid input for {label}";
        }
    }

    // Call SaveSettings when you leave the options menu
    void OnApplicationQuit()
    {
        PersistentSettings.Instance.SaveSettings();
    }
}
