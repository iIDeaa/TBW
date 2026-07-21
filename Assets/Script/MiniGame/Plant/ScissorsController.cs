using UnityEngine;

public class ScissorsController : BaseTool
{
    public Transform topBlade;
    public Transform bottomBlade;

    public float openAngle = 25f;
    public float closeAngle = 5f;
    public float rotateSpeed = 15f;

    protected override void Update()
    {
        base.Update();

        float target = Input.GetMouseButton(0)
            ? closeAngle
            : openAngle;

        topBlade.localRotation =
            Quaternion.Lerp(
                topBlade.localRotation,
                Quaternion.Euler(0,0,target),
                Time.deltaTime * rotateSpeed);

        bottomBlade.localRotation =
            Quaternion.Lerp(
                bottomBlade.localRotation,
                Quaternion.Euler(0,0,-target),
                Time.deltaTime * rotateSpeed);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!Input.GetMouseButton(0))
            return;

        Grass grass = other.GetComponent<Grass>();

        if(grass != null)
            grass.Cut();
    }
}