using UnityEngine;

public class EfectosDeSonido : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource Musica;
    public AudioSource Negativos;
    public AudioSource Positivos;
    public AudioSource Habilidades;

    public AudioClip JugadorDano;
    public AudioClip Pelea;
    public AudioClip Disparo;
    public AudioClip DisparoPesado;
    public AudioClip RayoLaser;
    public AudioClip Curacion;
    public AudioClip DanoSlime;
    public AudioClip NivelTerminado;
    public AudioClip intermedioGenerico;
    public AudioClip Tienda;
    public AudioClip Critico;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void ReproducirNegativos(AudioClip sonido)
    {
        Negativos.PlayOneShot(sonido);
    }

    public void ReproducirPositivos(AudioClip sonido)
    {
        Negativos.PlayOneShot(sonido);
    }

    public void ReproducirMusica(AudioClip sonido)
    {
        Negativos.PlayOneShot(sonido);
    }
}
