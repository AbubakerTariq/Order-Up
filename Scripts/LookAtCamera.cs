using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool inverted = true; 
    private void Update()
    {
        if (inverted)
        {
            transform.forward = -Camera.main.transform.forward;
        }
        else
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}