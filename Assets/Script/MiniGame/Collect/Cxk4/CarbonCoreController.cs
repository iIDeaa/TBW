using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarbonCoreController : MonoBehaviour
{
    [SerializeField] private int perfectScore = 100;
    [SerializeField] private int normalScore = 25;


    public void PerfectBroken(){
        Debug.Log("Perfect : All is Broken");

        ClearAlltrash();
        ScoreManager.Instance.AddScore(perfectScore);

        Destroy(gameObject);
    }
    public void NormalBroken(){
        Debug.Log("Great : Some Trash is Broken");

        ClearSometrash();
        ScoreManager.Instance.AddScore(normalScore);

        Destroy(gameObject);
    }

    private void ClearAlltrash(){
        Trash[] allTrash = FindObjectsOfType<Trash>();

        foreach (Trash trash in allTrash)
        {
            Destroy(trash.gameObject);
        }
    }

    private void ClearSometrash(){
        Trash[] allTrash = FindObjectsOfType<Trash>();
        int amountToClear = allTrash.Length / 2;

        for(int i = 0 ; i < amountToClear ; i++){
            Destroy(allTrash[i].gameObject);
        }
    }

    


    // Update is called once per frame
    void Update()
    {
        
    }
}
