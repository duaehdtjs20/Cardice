using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "Database/CardDatabase")]
public class CardDatabase : ScriptableObject
{
    [SerializeField] private List<CardData> _cards;

    public IReadOnlyList<CardData> Cards => _cards;
}
