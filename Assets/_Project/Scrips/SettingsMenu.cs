using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public PlayerSettings playerSettings;

    [Header("Sensitivity")]
    [SerializeField] private Slider sensSlider;
    [SerializeField] private TextMeshProUGUI sensText;
    [SerializeField] private TMP_InputField sensInputField;

    private void Start()
    {
        sensSlider.value = playerSettings.sens;
        sensInputField.text = playerSettings.sens.ToString();

        sensSlider.onValueChanged.AddListener(OnSliderValueChanged);
        sensInputField.onEndEdit.AddListener(OnInputFieldValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        int intValue = Mathf.Clamp(Mathf.RoundToInt(value), 10, 1000);
        playerSettings.sens = intValue;
        sensInputField.text = intValue.ToString();
        sensText.text = intValue.ToString();
    }

    private void OnInputFieldValueChanged(string value)
    {
        if (int.TryParse(value, out int inputValue))
        {
            inputValue = Mathf.Clamp(inputValue, 10, 1000);

            playerSettings.sens = inputValue;
            sensSlider.value = inputValue;

            sensInputField.text = inputValue.ToString();
            sensText.text = inputValue.ToString();
        }
        else
        {
            sensInputField.text = playerSettings.sens.ToString();
        }
    }

}
