using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Window : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _message;
    [SerializeField] private Button _closeButton;

    void Awake()
    {
        _closeButton.onClick.AddListener(Hide);
    }

    public void Show(string message)
    {
        gameObject.SetActive(true);
        _message.text = message;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}