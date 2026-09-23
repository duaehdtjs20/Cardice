using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private int _baseHp;
    [SerializeField] private int _baseAttack;
    [SerializeField] private int _baseDefense;
    [SerializeField] private int _level;

    public int BaseHp => _baseHp;
    public int BaseAttack => _baseAttack;
    public int BaseDefense => _baseDefense;
    public int Level => _level;
}
