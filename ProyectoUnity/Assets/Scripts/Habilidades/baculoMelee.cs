using UnityEngine;

public class baculoMelee : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float duracion;
    float duracionDes;
    Renderer[] rendersHijos;
    void Start()
    {
        duracion = 0;
        rendersHijos = transform.GetComponentsInChildren<Renderer>();
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
                Debug.Log("Desvaneciendo");
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
}
