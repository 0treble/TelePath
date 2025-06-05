using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CalibrationControlButton : MonoBehaviour
{
    public CalibrationObject calibrationObject; // Reference to the CalibrationObject script
    public TextMeshProUGUI buttonText; // Reference to the TextMeshPro button text

    private bool isCalibrating = false; // Track calibration state

    private void Start()
    {
        buttonText.text = "START"; // Ensure initial state is "START"
    }

    public void OnButtonClick()
    {
        if (isCalibrating)
        {
            calibrationObject.StopCalibration();
            buttonText.text = "START"; // Update button text
        }
        else
        {
            calibrationObject.StartCalibration();
            buttonText.text = "STOP"; // Update button text
        }

        isCalibrating = !isCalibrating;
    }

    public void OnCalibrationComplete()
    {
        buttonText.text = "START"; // Reset button text when calibration is done
        isCalibrating = false; // Ensure state is reset
    }

    private void CheckButtons()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnButtonClick();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    private void Update()
    {
        CheckButtons();
    }
}
