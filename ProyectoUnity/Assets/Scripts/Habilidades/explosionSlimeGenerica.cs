using System.Threading.Tasks;
using UnityEngine;

public class explosionSlimeGenerica : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    bool explotar;
    bool desvanecer;
    float rango;
    float tiempoEscalando;
    float tiempoDesvaneciendo;
    float escalaPorSegundo;
    float rapidezEscalado;
    float danoFisico;
    float danoMagico;
    float crit;
    Light luz;
    Renderer[] rendersHijos;
    void Start()
    {
        tiempoDesvaneciendo = 0;
        tiempoEscalando = 0;
        rendersHijos = transform.GetComponentsInChildren<Renderer>();
        luz = transform.GetChild(0).GetComponent<Light>();
        crit = 0;
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

            if(tiempoDesvaneciendo >= 0.6f) { desvanecer = false; Destroy(gameObject);}
            foreach (Renderer r in rendersHijos)
            {
                foreach (Material mat in r.materials)
                {
                    Color emission = mat.GetColor("_EmissionColor");
                    emission = Color.Lerp(emission, Color.black, Time.deltaTime * 9f);
                    luz.intensity -= 50 * Time.deltaTime;
                    mat.SetColor("_EmissionColor", emission);

                    Color color = mat.color;
                    if(color.a > 0)
                    {
                        color.a -= 1.5f * Time.deltaTime;
                        
                        mat.color = color;
                    } else
                    {
                        color.a = 0;
                    }
                }
            }
        }
    }

    public void Explotar(float rango, float rapidezEscalado, float danoFisico, float danoMagico, float crit)
    {
        this.danoFisico = danoFisico;
        this.danoMagico = danoMagico;
        this.crit = crit;
        this.rango = rango;
        this.rapidezEscalado = rapidezEscalado;
        explotar = true;
        escalaPorSegundo = (rango - transform.localScale.x)/rapidezEscalado;
    }
    async private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (danoFisico > 0)
            {
                collision.transform.GetComponent<Jugador>().jugadorStats.ActualizarHP(danoFisico, 0,crit, collision.transform.GetComponent<Jugador>().prefabNumeroDano);
            }
            await Task.Delay(Mathf.FloorToInt(collision.transform.GetComponent<Jugador>().jugadorStats.IFrames * 1000));
            if (danoMagico > 0)
            {
                collision.transform.GetComponent<Jugador>().jugadorStats.ActualizarHP(danoMagico, 1,crit, collision.transform.GetComponent<Jugador>().prefabNumeroDano);
            }

        }
    }
}


