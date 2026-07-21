using UnityEngine;

public class Rock : MonoBehaviour
{
    public bool isHolding;

    public void PickUp(Transform hand)
    {
        isHolding = true;

        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
    }

    public void Drop()
    {
        isHolding = false;

        transform.SetParent(null);
    }
}