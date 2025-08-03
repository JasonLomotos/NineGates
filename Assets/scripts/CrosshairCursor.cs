using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CrosshairCursor : MonoBehaviour
{
    [Tooltip("Crosshair sprite to use. Leave empty for a default cross.")]
    public Sprite crosshairSprite;

    [Tooltip("Crosshair size in pixels.")]
    public Vector2 crosshairSize = new Vector2(32, 32);

    private Image crosshairImage;

    void Awake()
    {
        // Hide the system cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Create crosshair UI if not set up
        crosshairImage = GetComponent<Image>();
        if (crosshairImage == null)
        {
            crosshairImage = gameObject.AddComponent<Image>();
        }

        if (crosshairSprite != null)
        {
            crosshairImage.sprite = crosshairSprite;
            crosshairImage.color = Color.white;
        }
        else
        {
            // Draw a simple cross if no sprite is assigned
            crosshairImage.sprite = GenerateDefaultCrossSprite();
            crosshairImage.color = Color.white;
        }

        crosshairImage.rectTransform.sizeDelta = crosshairSize;
        crosshairImage.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        crosshairImage.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        crosshairImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        crosshairImage.rectTransform.anchoredPosition = Vector2.zero;
    }

    void Update()
    {
        // Example: Raycast from the center of the screen for interaction
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            // You can interact with hit.collider.gameObject here
            // Example: Debug.Log("Looking at " + hit.collider.name);
        }
    }

    // Generates a simple cross sprite if none is provided
    private Sprite GenerateDefaultCrossSprite()
    {
        int size = 32;
        Texture2D tex = new Texture2D(size, size);
        Color32 clear = new Color32(0, 0, 0, 0);
        Color32 white = new Color32(255, 255, 255, 255);

        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
            tex.SetPixel(x, y, clear);

        for (int i = 0; i < size; i++)
        {
            tex.SetPixel(size / 2, i, white);
            tex.SetPixel(i, size / 2, white);
        }

        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
    }
}