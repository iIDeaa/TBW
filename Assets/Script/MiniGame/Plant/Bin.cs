using UnityEngine;

public class Bin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Rock rock = other.GetComponent<Rock>();

        if (rock == null)
            return;

        PlantMinigameManager.Instance.RockRemoved();

        Destroy(rock.gameObject);
    }
}