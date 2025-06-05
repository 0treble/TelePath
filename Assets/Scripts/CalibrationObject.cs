using UnityEngine;
using System.Collections;
using TMPro;

public class CalibrationObject : MonoBehaviour
{
    public Material material1; // First flicker material
    public Material material2; // Second flicker material
    public Material restMaterial; // Material for rest period
    public CalibrationControlButton controlButton; // Reference to the control button script

    private Renderer objRenderer;
    private bool isMaterial1 = true;
    private int currentTrial = 0;
    private UDPHandler udpHandler;
    private bool isCalibrating = false; // Flag to track calibration state

    private float[] frequencies; // Frequencies for flickering
    private int[] labels; // UDP labels for each frequency
    private float restDuration; // Duration of rest period
    private float stimulusDuration; // Duration of stimulus period
    private int numTrials; // Number of times to repeat the full calibration cycle

    public TextMeshProUGUI debugText; // UI Text to display received messages (optional)

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        udpHandler = FindFirstObjectByType<UDPHandler>();

        if (udpHandler == null)
        {
            debugText.text = $"UDPHandler not found in the scene!";
        }

        // Load settings from PersistentSettings
        LoadSettings();
    }

    // Call this to load the persistent settings
    private void LoadSettings()
    {
        PersistentSettings settings = PersistentSettings.Instance;

        // Assign values to class-level variables
        frequencies = settings.frequencies;
        labels = settings.labels;
        restDuration = settings.restDuration;
        stimulusDuration = settings.stimulusDuration;
        numTrials = settings.numTrials;

        // Update debugText with settings information
        debugText.text = $"Settings loaded: ";
        debugText.text += $"Frequencies: {string.Join("Hz, ", frequencies)} Hz; ";
        debugText.text += $"Labels: {string.Join(", ", labels)}; ";
        debugText.text += $"Rest Duration: {restDuration}s; ";
        debugText.text += $"Stimulus Duration: {stimulusDuration}s; ";
        debugText.text += $"Number of Trials: {numTrials}";
    }

    public void StartCalibration()
    {
        if (!isCalibrating)
        {
            // Reload settings every time calibration starts
            LoadSettings();

            isCalibrating = true;
            currentTrial = 0; // Reset trial counter
            StartCoroutine(RunCalibration());

            debugText.text = "Calibration started.";
        }
    }

    public void StopCalibration()
    {
        if (isCalibrating)
        {
            isCalibrating = false;
            StopAllCoroutines();
            objRenderer.material = restMaterial;
            udpHandler.SendUDPCommand(0); // Send stop signal

            debugText.text = "Calibration stopped.";

            if (controlButton != null)
            {
                controlButton.OnCalibrationComplete();
            }
        }
    }

    private IEnumerator RunCalibration()
    {
        debugText.text = "Starting calibration...";
        udpHandler.SendUDPCommand(8); // Start calibration trigger

        for (currentTrial = 0; currentTrial < numTrials; currentTrial++)
        {
            if (!isCalibrating) yield break;  // Stop if calibration was interrupted

            debugText.text = $"Trial {currentTrial + 1}/{numTrials} started.";

            for (int i = 0; i < frequencies.Length; i++)
            {
                if (!isCalibrating) yield break; // Early exit if calibration stopped

                // Rest period
                debugText.text = "Rest period started.";
                objRenderer.material = restMaterial;
                udpHandler.SendUDPCommand(0);
                yield return new WaitForSeconds(restDuration);

                // Stimulus period
                debugText.text = $"Stimulus {i + 1} (Frequency: {frequencies[i]} Hz, Label: {labels[i]}).";
                udpHandler.SendUDPCommand(labels[i]);
                yield return StartCoroutine(FlickerAtFrequency(frequencies[i]));
            }
        }

        if (isCalibrating)
        {
            debugText.text = "Final rest period started.";
            objRenderer.material = restMaterial;
            udpHandler.SendUDPCommand(0);
            yield return new WaitForSeconds(restDuration);

            debugText.text = "Calibration complete!";
            udpHandler.SendUDPCommand(9); // Stop calibration trigger
            udpHandler.SendUDPCommand(0);
        }

        isCalibrating = false;

        if (controlButton != null)
        {
            controlButton.OnCalibrationComplete();
        }

        PersistentSettings.Instance.SaveSettings();
    }

    private IEnumerator FlickerAtFrequency(float frequency)
    {
        float flickerTimer = 0f;
        float flickerInterval = 1f / frequency;
        float endTime = Time.time + stimulusDuration;

        while (Time.time < endTime && isCalibrating)
        {
            flickerTimer += Time.deltaTime;
            if (flickerTimer >= flickerInterval)
            {
                flickerTimer = 0f;
                isMaterial1 = !isMaterial1;
                objRenderer.material = isMaterial1 ? material1 : material2;
            }
            yield return null;
        }

        objRenderer.material = restMaterial; // Ensure it ends in rest material
    }
}
