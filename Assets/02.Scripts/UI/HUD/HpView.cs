using TMPro;

using UnityEngine;

public class HpView : MonoBehaviour
{
    [SerializeField] private Enemy _target;
    [SerializeField] private TMP_Text _hpText;

    private void OnEnable()
    {
        _target.OnChanged += Refresh;
    }
    private void OnDisable()
    {
        _target.OnChanged -= Refresh;
    }

    private void Refresh()
    {
        _hpText.text = _target.Hp.ToString();
    }
}
