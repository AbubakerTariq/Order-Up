using UnityEngine;

public class Counter : MonoBehaviour
{
    [Space] [Header("Component references")]
    [SerializeField] private MeshRenderer counterRenderer;

    [Space] [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;

    private void Start()
    {
        counterRenderer.material = defaultMaterial;
    }

    public void HighlightCounter()
    {
        counterRenderer.material = selectedMaterial;
    }

    public void UnHighlightCounter()
    {
        counterRenderer.material = defaultMaterial;
    }

    public void Interact()
    {
        Debug.Log("Interacted with: " + this);
    }
}