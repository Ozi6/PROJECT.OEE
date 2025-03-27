using UnityEngine;

public class Clickable : MonoBehaviour
{
    public CameraControl mainCamControl;

    private Renderer paneRenderer;
    private Material originalMaterial;

    public Material outlineMaterial;
    public Transform goPos;

    private bool isHovered = false;

    void Start()
    {
        paneRenderer = GetComponent<Renderer>();
        originalMaterial = paneRenderer.material;
    }

    void Update()
    {
        DetectHover();
        DetectClick();
    }

    private void DetectHover()
    {
        if(mainCamControl.GetOccupied())
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (!isHovered)
                {
                    isHovered = true;
                    paneRenderer.material = outlineMaterial;
                }
            }
            else
                ResetOutline();
        }
        else
            ResetOutline();
    }

    private void DetectClick()
    {
        if (isHovered && Input.GetMouseButtonDown(0))
        {
            mainCamControl.MoveCam(goPos);
            ResetOutline();
        }
    }

    private void ResetOutline()
    {
        if (isHovered)
        {
            isHovered = false;
            paneRenderer.material = originalMaterial;
        }
    }
}
