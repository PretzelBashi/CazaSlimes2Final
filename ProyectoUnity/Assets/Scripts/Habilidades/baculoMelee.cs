using System.Threading.Tasks;
using UnityEngine;
using static Herramientas;

public class baculoMelee : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float duracion;
    float duracionDes;
    Renderer[] rendersHijos;
    MagoStats jugadorStats;
    void Start()
    {
        duracion = 0;
        rendersHijos = transform.GetComponentsInChildren<Renderer>();
        jugadorStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Jugador>().jugadorStats;
        duracionDes = 0;
    }

    // Update is called once per frame
    void Update()
    {
        duracion += Time.deltaTime;
        if (duracion > 0.5)
        {
            duracionDes += Time.deltaTime;
            if (duracionDes > 1) { Destroy(gameObject); }
            foreach (Renderer r in rendersHijos)
            {

                if(r.transform.name != "Trail")
                {
                    foreach (Material mat in r.materials)
                    {
                        Color emission = mat.GetColor("_EmissionColor");
                        emission = Color.Lerp(emission, Color.black, Time.deltaTime * 7f);
                        mat.SetColor("_EmissionColor", emission);

                        Color color = mat.color;
                        if (color.a > 0)
                        {
                            color.a -= 1f * Time.deltaTime;

                            mat.color = color;
                        }
                        else
                        {
                            color.a = 0;
                        }
                    }
                }

            }
        }
        transform.position += transform.forward * 7 * Time.deltaTime;
    }

    async private void OnTriggerEnter(Collider collision)
    {
        float danoFisico = jugadorStats.danoFisicoActual;
        float danoMagico = (jugadorStats.danoMagicoActual + 1) / 3;

        if (collision.gameObject.tag == "Slime")
        {

            if (danoFisico > 0)
            {
                
                collision.transform.GetComponent<Slime>().slimeStats.ActualizarHP(danoFisico, 0, collision.transform.GetComponent<Slime>().prefabNumeroDano);
            }
            await Task.Delay(Mathf.FloorToInt(collision.transform.GetComponent<Slime>().slimeStats.IFrames * 1000));
            if (danoMagico > 0)
            {
                collision.transform.GetComponent<Slime>().slimeStats.ActualizarHP(danoMagico, 1, collision.transform.GetComponent<Slime>().prefabNumeroDano);
            }

        }
    }
}
