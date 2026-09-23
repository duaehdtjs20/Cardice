using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private ECardType _cardType;
    [SerializeField] private int _cost;
    [SerializeField] private string _dscription;

    public int Id => _id;
    public string Name => _name;
    public ECardType CardType => _cardType;
    public int Cost => _cost;
    public string Description => _dscription;
}
