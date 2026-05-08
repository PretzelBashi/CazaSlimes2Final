using UnityEngine;

public class cuevaInfo : MonoBehaviour
{
    Vector3 coordenadas;

    public void actualizarCoordenadas(Vector3 coordenadas)
    {
        this.coordenadas = coordenadas;
    }

    public Vector3 leerCoordenadas()
    {
        return coordenadas;
    }
}
