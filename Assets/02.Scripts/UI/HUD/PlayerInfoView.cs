using TMPro;

using UnityEngine;

public class PlayerInfoView : MonoBehaviour
{
    [SerializeField] private GameStream _stream;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private TMP_Text _atkText;
    [SerializeField] private TMP_Text _defText;
    [SerializeField] private TMP_Text _goldText;

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
        _hpText.text = "HP : " + _stream.Player.Hp.ToString();
        _atkText.text = "ATK : " + _stream.Player.AttackDamage.ToString();
        _defText.text = "DEF : " + _stream.Player.Defense.ToString();
        _goldText.text = "GOLD : " + _stream.Player.Gold.ToString();
    }
}
