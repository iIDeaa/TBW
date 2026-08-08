using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum  CoreType
{
    Weak, Strong
}
public class CoreController : MonoBehaviour
{
    [Header("Core Setting")]
    [SerializeField] private CoreType coreType = CoreType.Weak;

    [Header("Normal Minigame")]
    [SerializeField] private int hitsPerRound = 5;
    [SerializeField] private int totalRounds = 2;

    public CoreType Type => coreType;
    public int HitsPerRound => hitsPerRound;
    public int TotalRounds => totalRounds;

    public void StartCore(){
        Debug.Log(
            $"Start {coreType} Core: " + $"{hitsPerRound} hits x {totalRounds} rounds"
        );
    }
}
