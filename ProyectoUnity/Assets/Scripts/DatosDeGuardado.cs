using UnityEngine;
using static Herramientas;
public class DatosDeGuardado : MonoBehaviour
{
    public static DatosGuardadosLocal datosDeGuardado;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        datosDeGuardado = DatosGuardadosLocal.Cargar();
    }
}
