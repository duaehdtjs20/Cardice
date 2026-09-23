using System.Collections.Generic;

using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private List<Tile> _tiles;

    public IReadOnlyList<Tile> Tiles => _tiles;

    public void DrawTiles()
    {
        foreach (var tile in _tiles)
        {
            if (tile.TryGetComponent(out SpriteRenderer render))
            {
                render.color = SelectColor(tile.Type);
            }
        }
    }

    private Color SelectColor(ETileType type)
    {
        switch (type)
        {
            case ETileType.LevelUp:
                return Color.cadetBlue;
            case ETileType.CardShop:
                return Color.aquamarine;
            case ETileType.ChipShop:
                return Color.bisque;
            case ETileType.Card:
                return Color.coral;
            case ETileType.Coin:
                return Color.chocolate;
            default:
                return Color.white;
        }
    }
}
