using UnityEngine;

public class AnimationTermiandaJugador : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Jugador jugador;
    void Start()
    {
        jugador = transform.GetComponentInParent<Jugador>();
    }

    // Update is called once per frame
    public void AnimacionDisparoTerminada()
    {
        jugador.AnimacionDisparoTerminada();
    }
}
