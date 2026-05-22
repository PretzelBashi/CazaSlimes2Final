using TMPro;
using UnityEngine;

public class numeroDamage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Vector3 posicion;
    
    float contador;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        posicion = transform.position;
        posicion.y += 1 * Time.deltaTime;
        contador += Time.deltaTime;
        if(contador > 2) { Destroy(this.transform.parent.gameObject); }
        this.transform.position = posicion;
        Vector3 direccion = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;

        direccion.y = 0.7f;

        transform.rotation = Quaternion.LookRotation(direccion);
    }

    public void CambiarNumero(float dano, int tipoDano)
    {
        switch (tipoDano)
        {
            case 0:
                this.transform.GetComponent<TextMeshProUGUI>().text = dano.ToString();
                this.transform.GetComponent<TextMeshProUGUI>().color = Color.red;
                break;
            case 1:
                this.transform.GetComponent<TextMeshProUGUI>().text = dano.ToString();
                this.transform.GetComponent<TextMeshProUGUI>().color = Color.blue;
                break;
            case 2:
                this.transform.GetComponent<TextMeshProUGUI>().text = dano.ToString();
                this.transform.GetComponent<TextMeshProUGUI>().color = Color.green;
                break;
        }

    }
}
