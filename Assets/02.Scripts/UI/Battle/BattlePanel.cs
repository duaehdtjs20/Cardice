using TMPro;

using UnityEngine;

public class BattlePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _attackerHpText;
    [SerializeField] private TMP_Text _attackerMaxDamageText;
    [SerializeField] private TMP_Text _attackerMinDamageText;

    [SerializeField] private TMP_Text _defenderHpText;
    [SerializeField] private TMP_Text _defenderMaxDefenseText;
    [SerializeField] private TMP_Text _defenderMinDefenseText;

    [SerializeField] private TMP_Text _attackerDiceValue;
    [SerializeField] private TMP_Text _defenderDiceValue;

    [SerializeField] private GameObject _battleDiceButton;

    private void OnEnable()
    {
        _battleDiceButton.SetActive(true);
    }

    public void DrawInfo(IDamageable attacker,  IDamageable defender)
    {
        _attackerHpText.text = "Hp : " + attacker.Hp.ToString();
        _attackerMaxDamageText.text = attacker.AttackDamage.ToString();
        _attackerMinDamageText.text = attacker.AttackDamage.ToString();

        _defenderHpText.text = "Hp : " + defender.Hp.ToString();
        _defenderMaxDefenseText.text = defender.Defense.ToString();
        _defenderMinDefenseText.text = defender.Defense.ToString();
    }
    public void DrawDiceValue(int attackerValue, int defenderValue)
    {
        _attackerDiceValue.text = attackerValue.ToString();
        _defenderDiceValue.text = defenderValue.ToString();
    }
}
