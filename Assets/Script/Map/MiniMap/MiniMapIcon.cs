using UnityEngine;

public class MiniMapIcon : MonoBehaviour
{
    [SerializeField] private Transform target; // Player
    [SerializeField] private Transform map;    // RawImage หรือ MiniMap Panel

    [Header("World Size")]
    [SerializeField] private Vector2 mapWorldSize = new Vector2(100,100);

    [Header("UI Size")]
    [SerializeField] private Vector2 mapUISize = new Vector2(800,800);


    private RectTransform rect;


    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }


    private void Update()
    {
        Vector3 playerPos = target.position;


        float x = 
            (playerPos.x / mapWorldSize.x)
            * mapUISize.x;


        float y = 
            (playerPos.y / mapWorldSize.y)
            * mapUISize.y;


        rect.localPosition = new Vector2(
            x,
            y
        );
    }
}