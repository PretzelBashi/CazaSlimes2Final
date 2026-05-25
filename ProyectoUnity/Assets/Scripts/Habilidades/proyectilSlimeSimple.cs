using UnityEngine;
using static Herramientas;
public class proyectilSlimeSimple : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject prefabExplosion;
    public float danoMagico;
    float crit;


    public void CambiarDano(float dano, float crit)
    {
        danoMagico = dano;
        this.crit = crit;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * 13 * Time.deltaTime;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(prefabExplosion, transform.position, Quaternion.identity).GetComponent<explosionSlimeGenerica>().Explotar(10, 0.2f, 0, danoMagico, crit);
        
        Destroy(gameObject);
    }
}
