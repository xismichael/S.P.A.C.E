using UnityEngine;

public class SpaceZoom : MonoBehaviour
{
    public GameObject ZoomBackground;
    public GameObject Space;
    private bool isZoomedIn = false;

    private RectTransform spaceRect;

    void Start()
    {
        // Cache RectTransform for performance
        spaceRect = Space.GetComponent<RectTransform>();
    }

    public void Initialized()
    {
        Debug.Log("Space Zoom Initialized");

        ZoomBackground.SetActive(false);
        spaceRect.anchoredPosition = new Vector2(386.7f, 162.8289f);
        spaceRect.localScale = new Vector3(1f, 1f, 1f);
        isZoomedIn = false;
    }

    public void zoomIn()
    {
        if (isZoomedIn) return;
        isZoomedIn = true;

        ZoomBackground.SetActive(true);
        spaceRect.anchoredPosition = Vector2.zero;
        spaceRect.localScale = new Vector3(1.5f, 1.5f, 5f);

        Debug.Log("Zoomed in on space");
    }

    public void zoomOut()
    {
        if (!isZoomedIn) return;
        isZoomedIn = false;

        ZoomBackground.SetActive(false);
        spaceRect.anchoredPosition = new Vector2(386.7f, 162.8289f);
        spaceRect.localScale = new Vector3(1f, 1f, 1f);

        Debug.Log("Zoomed out on space");
    }
}
