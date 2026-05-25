using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class logrosUIInicio : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void CargarLogros()
    {
        GameObject.FindGameObjectWithTag("dialogosUnicos").GetComponent<TextMeshProUGUI>().text = $"Dialogos unicos escuchados: {DatosDeGuardado.datosDeGuardado.dialogosTiendaEscuchados-6}/8";
        GameObject.FindGameObjectWithTag("dialogosUnicos").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Cuarto mas alto: {DatosDeGuardado.datosDeGuardado.cuartoMasAlto}";
        TextMeshProUGUI[] logros = transform.GetComponentsInChildren<TextMeshProUGUI>();
        for(int i = 0; i < logros.Length; i++)
        {
            if (DatosDeGuardado.datosDeGuardado.logros[i]) { logros[i].color = Color.green; }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
