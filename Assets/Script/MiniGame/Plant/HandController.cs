using UnityEngine;

public class HandController : BaseTool
{
    Rock currentRock;

    void OnTriggerStay2D(Collider2D other)
    {
        if (!Input.GetMouseButton(0))
            return;

        if (currentRock != null)
            return;

        Rock rock = other.GetComponent<Rock>();

        if (rock != null)
        {
            currentRock = rock;
            currentRock.PickUp(transform);
            Debug.Log("Picked");
        }
    }

    protected override void Update()
    {
        base.Update();

        if (currentRock != null && Input.GetMouseButtonUp(0))
        {
            currentRock.Drop();
            currentRock = null;
        }
    }
}