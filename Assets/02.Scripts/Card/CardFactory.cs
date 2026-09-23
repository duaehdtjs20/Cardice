using UnityEngine;

public class CardFactory : MonoBehaviour
{
    [SerializeField] private CardDatabase _database;

    public Card SendRandomCard()
    {
        CardData randomData = _database.Cards[Random.Range(0, _database.Cards.Count)];

        return new Card(randomData);
    }
    public bool TrySendSelectCard(int id, out Card newCard)
    {
        foreach (var card in _database.Cards)
        {
            if (card.Id == id)
            {
                newCard = new Card(card);
                return true;
            }
        }
        newCard = null;
        return false;
    }
}
