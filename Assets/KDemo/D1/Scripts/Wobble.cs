using UnityEngine;

public class Wobble : MonoBehaviour
{
    public float MaxWobble = 0.03f;
    public float WobbleSpeed = 5.0f;
    public float RecoveryRate = 1f;

    Renderer rend;
    Vector3 prevPos;
    Vector3 prevRot;
    float wobbleAmountToAddX;
    float wobbleAmountToAddZ;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        // decreases the wobble over time
        wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, Time.deltaTime * RecoveryRate);
        wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, Time.deltaTime * RecoveryRate);

        // make a sine wave of the decreasing wobble
        float wobbleAmountX = wobbleAmountToAddX * Mathf.Sin(WobbleSpeed * Time.time);
        float wobbleAmountZ = wobbleAmountToAddZ * Mathf.Sin(WobbleSpeed * Time.time);

        // send it to the shader
        rend.material.SetFloat("_WobbleX", wobbleAmountX);
        rend.material.SetFloat("_WobbleZ", wobbleAmountZ);

        // Move Speed
        Vector3 moveSpeed = (prevPos - transform.position) / Time.deltaTime;
        Vector3 rotationDelta = transform.rotation.eulerAngles - prevRot;

        // add clamped speed to wobble
        wobbleAmountToAddX += Mathf.Clamp((moveSpeed.x + (rotationDelta.z * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);
        wobbleAmountToAddZ += Mathf.Clamp((moveSpeed.z + (rotationDelta.x * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);

        // save the last position
        prevPos = transform.position;
        prevRot = transform.rotation.eulerAngles;
    }
}