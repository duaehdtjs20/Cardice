using TMPro;

using UnityEngine;

public class DiceText : MonoBehaviour
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
        _text.text = "Dice : " + _stream.DiceValue.ToString();
    }
}
