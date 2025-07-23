using UnityEngine;
using UnityEngine.UI;
public class ImageParallax : MonoBehaviour
{
    private Material currentMaterial;
    [SerializeField] private Vector2 offsetSpeed;
    private Vector2 currentOffset;

    [SerializeField] private bool isUIImage;

    private void Awake()
    {
        if (isUIImage)
        {
            currentMaterial = GetComponent<Image>().material;
        }
        else
        {
            currentMaterial = GetComponent<SpriteRenderer>().material;
        }
    }

    private void Update()
    {
        currentOffset += offsetSpeed * Time.deltaTime;
        currentMaterial.SetTextureOffset("_BaseMap", currentOffset);
    }
}
