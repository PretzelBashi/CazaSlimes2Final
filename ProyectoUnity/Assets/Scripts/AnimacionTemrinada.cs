using UnityEngine;

public class AnimacionTemrinada : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Slime Slime;
    void Start()
    {
        Slime = transform.GetComponentInParent<Slime>();
    }
    public void SaltoTerminado()
    {
        Slime.SaltoTerminado();
    }
}
