using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboInput : MonoBehaviour
{

    private KeyCode[] Combo;

    [SerializeField] private float timelimit = 3f;
    [SerializeField] private CarbonCoreController activeCore;

    private int ComboIndex;
    private float LastCorrecttime;
    private bool isRunning;

    public void ComboStart(){
        CreatRandomIndex();

        ComboIndex = 0;
        LastCorrecttime = Time.time;
        isRunning = true;

        Debug.Log("Combo Start");
    }

    void Update()
    {
        if(!isRunning){
            return;
        }
        if(Time.time - LastCorrecttime > timelimit){
            return;
        }
        if(!Input.anyKeyDown){
            return;
        }
        if(Input.GetKeyDown(Combo[ComboIndex])){
            ComboIndex++;
            LastCorrecttime = Time.time;

            Debug.Log("Combo progress" + ComboIndex + "/" + Combo.Length);

            if(ComboIndex == Combo.Length){
                PerfectCombo();
            }
            else{
                ReCombo();
            }
        }
    }

    private void PerfectCombo(){
        isRunning = false;
        activeCore.PerfectBroken();
        Debug.Log("Perfect Combo");

    }

    private void ReCombo(){
        ComboIndex = 0;
        LastCorrecttime = Time.time;

        Debug.Log("Reset Combo");
    }

    private void CreatRandomIndex(){
        List<KeyCode> keys = new List<KeyCode>{KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D};

        for(int i = 0 ; i < keys.Count ; i++){
            int randomindex = Random.Range(i,keys.Count);

            KeyCode temp = keys[i];
            keys[i] = keys[randomindex];
            keys[randomindex] = temp;
        }
        Combo = new KeyCode[]{keys[0], keys[1], keys[2], keys[3], KeyCode.Space};
    }
    
}
