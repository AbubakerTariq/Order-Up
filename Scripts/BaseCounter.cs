using UnityEngine;

public class BaseCounter : MonoBehaviour
{
    [Space]
    [SerializeField] private MeshRenderer[] counterRenderers;

    [Space] [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;

    [Space] [Header("SFX")]
    [SerializeField] protected AudioSource audioSource;

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

    public virtual void Interact(Player player) { }
    public virtual void OperateStart(Player player) { }
    public virtual void OperateEnd(Player player) { }
}