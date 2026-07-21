using UnityEngine;

public class Tree : MonoBehaviour
{
    public float swaySpeed = 1f;
    public float swayAmount = 5f;

    void Update()
    {
        float angle = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}