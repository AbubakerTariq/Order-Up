using UnityEngine;

public class BaseCounter : MonoBehaviour
{
    [Space] [Header("Component references")]
    [SerializeField] private MeshRenderer[] counterRenderers;

    [Space] [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;

    private void Start()
    {
        SetMaterial(defaultMaterial);
    }

    public void HighlightCounter()
    {
        SetMaterial(selectedMaterial);
    }

    public void UnHighlightCounter()
    {
        SetMaterial(defaultMaterial);
    }

    private void SetMaterial(Material material)
    {
        foreach (MeshRenderer renderer in counterRenderers)
        {
            renderer.material = material;
        }
    }

    public virtual void Interact(Player player)
    {
        Debug.Log(player + " interacted with: " + this);
    }

    public virtual void Operate(Player player)
    {
        Debug.Log(player + " performed operation on: " + this);
    }
}