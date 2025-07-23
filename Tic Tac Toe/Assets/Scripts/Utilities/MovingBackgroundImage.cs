using UnityEngine;

public class MovingBackgroundImage : MonoBehaviour
{
    private Material currentMaterial;
    [SerializeField] private Vector2 offsetSpeed;
    private Vector2 currentOffset;

    private void Awake()
    {
        currentMaterial = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        currentOffset += offsetSpeed * Time.deltaTime;
        currentMaterial.SetTextureOffset("_BaseMap", currentOffset);
    }
}
