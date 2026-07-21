using UnityEngine;

public class BaseTool : MonoBehaviour
{
    protected Camera cam;

    protected virtual void Start()
    {
        cam = Camera.main;
    }

    protected virtual void Update()
    {
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        transform.position = pos;
    }
}