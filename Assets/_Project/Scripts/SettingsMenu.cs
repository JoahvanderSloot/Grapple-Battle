using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public playerSettings m_PlayerSettings;

    [Header("Sensitivity")]
    [SerializeField] Slider m_sensSlider;
    [SerializeField] TextMeshProUGUI m_sensText;
    [SerializeField] TMP_InputField m_sensInputField;

    private void Start()
    {
        m_sensSlider.value = m_PlayerSettings.sens;
        m_sensInputField.text = m_PlayerSettings.sens.ToString();

        m_sensSlider.onValueChanged.AddListener(OnSliderValueChanged);
        m_sensInputField.onEndEdit.AddListener(OnInputFieldValueChanged);
    }

    private void OnSliderValueChanged(float _value)
    {
        int _intValue = Mathf.Clamp(Mathf.RoundToInt(_value), 10, 1000);
        m_PlayerSettings.sens = _intValue;
        m_sensInputField.text = _intValue.ToString();
        m_sensText.text = _intValue.ToString();
    }

    private void OnInputFieldValueChanged(string value_)
    {
        AudioManager.m_Instance.Play("Click");
        if (int.TryParse(value_, out int _inputValue))
        {
            _inputValue = Mathf.Clamp(_inputValue, 10, 1000);

            m_PlayerSettings.sens = _inputValue;
            m_sensSlider.value = _inputValue;

            m_sensInputField.text = _inputValue.ToString();
            m_sensText.text = _inputValue.ToString();
        }
        else
        {
            m_sensInputField.text = m_PlayerSettings.sens.ToString();
        }
    }

}
