using System.Collections;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private enum CameraState { Middle, Left, Right }
    private CameraState currentState = CameraState.Middle;
    private Quaternion targetRotation;
    public Transform originalPos;

    private const float CooldownDuration = 0.5f;
    private float cooldownTimer = 0f;
    private float rotationSpeed = 5f;

    private bool occuppied = false;

    void Start()
    {
        targetRotation = transform.rotation;
    }

    void Update()
    {
        HandleMouseHover();
        SmoothRotate();
    }

    private void HandleMouseHover()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (GetOccupied())
        {
            if (Input.mousePosition.y < Screen.height * 0.25f && cooldownTimer <= 0)
                ResetCam();
            return;
        }

        float mouseX = Input.mousePosition.x;
        float screenWidth = Screen.width;

        if(cooldownTimer <= 0)
        {
            if (currentState == CameraState.Middle)
            {
                if (mouseX < screenWidth * 0.25f)
                    RotateCamera(CameraState.Left, -90f);
                else if (mouseX > screenWidth * 0.75f)
                    RotateCamera(CameraState.Right, 90f);
            }
            else if (currentState == CameraState.Left && mouseX > screenWidth * 0.75f)
                RotateCamera(CameraState.Middle, 0f);
            else if (currentState == CameraState.Right && mouseX < screenWidth * 0.25f)
                RotateCamera(CameraState.Middle, 0f);
        }
    }

    private void RotateCamera(CameraState newState, float angle)
    {
        currentState = newState;
        targetRotation = Quaternion.Euler(0f, angle, 0f);
        cooldownTimer = CooldownDuration;
    }

    private void SmoothRotate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void MoveCam(Transform newPos)
    {
        SetOccupied(true);
        targetRotation = newPos.rotation;
        StartCoroutine(MoveToPosition(newPos.position));
        cooldownTimer = CooldownDuration * 2;
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;

        while(elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPosition, elapsedTime / duration);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }

    public void ResetCam()
    {
        SetOccupied(false);
        targetRotation = originalPos.rotation;
        StartCoroutine(MoveToPosition(originalPos.position));
        cooldownTimer = CooldownDuration * 2;
    }

    public bool GetOccupied()
    {
        return occuppied;
    }

    public void SetOccupied(bool value)
    {
        occuppied = value;
    }
}
