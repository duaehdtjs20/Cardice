using UnityEngine;

public class Card
{
    private int _id;
    private string _name;
    private ECardType _cardType;
    private int _cost;
    private string _description;

    public Card(CardData data)
    {
        _id = data.Id;
        _name = data.Name;
        _cardType = data.CardType;
        _cost = data.Cost;
        _description = data.Description;
    }
}
