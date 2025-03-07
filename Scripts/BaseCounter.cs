using UnityEngine;

public class BaseCounter : MonoBehaviour
{
    public virtual void Interact(Player player)
    {
        Debug.Log(player + " interacted with: " + this);
    }

    public virtual void Operate(Player player)
    {
        Debug.Log(player + " performed operation on: " + this);
    }

    public virtual void HighlightCounter()
    {
        Debug.Log("Highlighted: " + this);
    }

    public virtual void UnHighlightCounter()
    {
        Debug.Log("Unhighlighted: " + this);
    }
}