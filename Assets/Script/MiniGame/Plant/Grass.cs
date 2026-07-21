using UnityEngine;

public class Grass : MonoBehaviour
{
    bool isCut;
    // public ParticleSystem cutParticle;

    public void Cut()
{
    if(isCut)
        return;

    isCut = true;

    // Instantiate(
    //     cutParticle,
    //     transform.position,
    //     Quaternion.identity);

    PlantMinigameManager.Instance.GrassCut();

    Destroy(gameObject);
}
}