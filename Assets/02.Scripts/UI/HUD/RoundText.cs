using TMPro;

using UnityEngine;

public class RoundText : MonoBehaviour
{
    [SerializeField] private GameStream _stream;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        _stream.OnChanged += Refresh;
    }
    private void OnDisable()
    {
        _stream.OnChanged -= Refresh;
    }

    private void Refresh()
    {
        _text.text = "Round : " + _stream.Round.ToString();
    }
}
