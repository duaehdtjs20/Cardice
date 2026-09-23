using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Data/StageData")]
public class StageData : ScriptableObject
{
    [SerializeField] private int _maxRound;
    [SerializeField] private int[] _waveRounds;

    public int MaxRound => _maxRound;
    public IReadOnlyList<int> WaveRounds => _waveRounds;
}
