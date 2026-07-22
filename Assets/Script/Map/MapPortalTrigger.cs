using UnityEngine;

public class MapPortalTrigger : MonoBehaviour
{
    public string playerTag = "Player";
    public string mapSceneName = "MapSelect";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        FadeManager.Instance.FadeToScene(mapSceneName); 
    }
}