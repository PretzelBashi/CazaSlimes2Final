using UnityEngine;

public class AnimacionTemrinada : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Slime Slime;
    void Start()
    {
        Slime = transform.GetComponentInParent<Slime>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaltoTerminado()
    {
        Slime.SaltoTerminado();
    }
}
