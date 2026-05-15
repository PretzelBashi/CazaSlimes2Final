using UnityEngine;

public class explosionGenerica : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    bool explotar;
    bool desvanecer;
    float rango;
    float tiempoEscalando;
    float tiempoDesvaneciendo;
    float escalaPorSegundo;
    float rapidezEscalado;
    Light luz;
    Renderer[] rendersHijos;
    void Start()
    {
        tiempoDesvaneciendo = 0;
        tiempoEscalando = 0;
        rendersHijos = transform.GetComponentsInChildren<Renderer>();
        luz = transform.GetChild(0).GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if(explotar && tiempoEscalando <= rapidezEscalado)
        {
            tiempoEscalando += Time.deltaTime;
            transform.localScale += Vector3.one * escalaPorSegundo * Time.deltaTime;

            if(tiempoEscalando >= rapidezEscalado)
            {
                desvanecer = true;
                explotar = false;
            }
        } else if (desvanecer)
        {
            
            tiempoDesvaneciendo += Time.deltaTime;

            if(tiempoDesvaneciendo >= 1f) { desvanecer = false; Destroy(gameObject); Debug.Log("Destruir"); }
            Debug.Log("tiempoDesvaneciendo");
            foreach (Renderer r in rendersHijos)
            {
                Debug.Log("Desvaneciendo");
                foreach (Material mat in r.materials)
                {
                    Color emission = mat.GetColor("_EmissionColor");
                    emission = Color.Lerp(emission, Color.black, Time.deltaTime * 7f);
                    luz.intensity -= 50 * Time.deltaTime;
                    mat.SetColor("_EmissionColor", emission);

                    Color color = mat.color;
                    if(color.a > 0)
                    {
                        color.a -= 1 * Time.deltaTime;
                        
                        mat.color = color;
                    } else
                    {
                        color.a = 0;
                    }
                }
            }
        }
    }

    public void Explotar(float rango, float rapidezEscalado)
    {
        this.rango = rango;
        this.rapidezEscalado = rapidezEscalado;
        explotar = true;
        escalaPorSegundo = (rango - transform.localScale.x)/rapidezEscalado;
    }
}
