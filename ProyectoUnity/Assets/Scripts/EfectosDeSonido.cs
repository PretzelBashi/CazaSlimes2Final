using UnityEngine;

public class EfectosDeSonido : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource Musica;
    public AudioSource Negativos;
    public AudioSource Positivos;

    public AudioClip JugadorDano;
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
