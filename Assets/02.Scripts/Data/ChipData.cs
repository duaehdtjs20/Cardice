using UnityEngine;

[CreateAssetMenu(fileName = "ChipData", menuName = "Scriptable Objects/ChipData")]
public class ChipData : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private EGrade _grade;
    [SerializeField] private EChipType _chipType;
    [SerializeField] private string _description;

    public int Id => _id;
    public EGrade Grade => _grade;
    public EChipType ChipType => _chipType;
    public string Description => _description;
}
