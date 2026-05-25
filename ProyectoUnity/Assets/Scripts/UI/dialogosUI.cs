using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Herramientas;

public class dialogosUI : MonoBehaviour
{
    public Sprite[] spritesPersonajes;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    TextMeshProUGUI textoDialogo;
    TextMeshProUGUI nombrePersonaje;
    TodosLosDialogos dialogos;
    Conversacion conversacionCargada;
    Image retratoPersonaje;
    int id;
    bool skip;
    bool conversacionIniciada;
    bool siguienteDialogo;
    int velocidadDeDialogo;
    float contadorDialogo;
    void Awake()
    {
        id = 0;
        velocidadDeDialogo = 100;
        dialogos = new TodosLosDialogos();
        contadorDialogo = 0;

        skip = false;
        textoDialogo = transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        nombrePersonaje = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        retratoPersonaje = transform.GetChild(2).GetComponent<Image>();

    }

    async public void IniciarDialogosTutorial()
    {
        id = 0;
        conversacionCargada = dialogos.todosLosDialogos[id];
        transform.GetComponent<CanvasGroup>().alpha = 1;
        skip = false;
        conversacionIniciada = true;
        await ReproducirDialogo();
        OcultarDialogo();
        await Task.Delay(2000);
        GameObject.FindGameObjectWithTag("infoPartidaActual").GetComponent<infoPartidaActual>().dialogoTutoTerminado = true;
        IniciarDialogo(1);
    }

    async public Task IniciarDialogo(int id)
    {
        this.id = id;
        conversacionCargada = dialogos.todosLosDialogos[id];
        transform.GetComponent<CanvasGroup>().alpha = 1;
        skip = false;
        conversacionIniciada = true;
        await ReproducirDialogo();
        OcultarDialogo();
        return;
    }
    public void OcultarDialogo()
    {
        transform.GetComponent<CanvasGroup>().alpha = 0;
    }

    async public Task ReproducirDialogo()
    {
        for (int i = 0; i < conversacionCargada.conversacion.Count; i++)
        {
            Debug.Log(conversacionCargada.conversacion.Count);
            nombrePersonaje.text = conversacionCargada.conversacion[i].personaje;
            int spriteActual = 0;
            switch (conversacionCargada.conversacion[i].personaje)
            {
                case "Vendedora": retratoPersonaje.sprite = spritesPersonajes[0]; spriteActual = 0; break;
                case "Tu (Saki)": retratoPersonaje.sprite = spritesPersonajes[2]; spriteActual = 2; break;
            }

            textoDialogo.text = conversacionCargada.conversacion[i].mensaje;
            textoDialogo.maxVisibleCharacters = 0;

            int textoContador = 0;
            skip = false;

            do
            {
                textoContador++;
                textoDialogo.maxVisibleCharacters = textoContador;

                if (textoContador % 2 == 0) {
                    retratoPersonaje.sprite = spritesPersonajes[spriteActual];
                }
                else {
                    retratoPersonaje.sprite = spritesPersonajes[spriteActual+1];
                }
                
                await Task.Delay(velocidadDeDialogo);
                Debug.Log(skip);
            } while (textoContador < textoDialogo.text.Length && !skip);
            Debug.Log("Salida" +skip);
            retratoPersonaje.sprite = spritesPersonajes[spriteActual];

            if(i == 19 && id == 0)
            {
                GameObject.FindGameObjectWithTag("infoPartidaActual").GetComponent<infoPartidaActual>().dialogoSlime = true;
            }

            textoDialogo.maxVisibleCharacters = textoDialogo.text.Length;
            skip = false;
            do { await Task.Yield(); } while (!skip);
            await Task.Yield();
            skip = false;
        }

        return;
    }

    // Update is called once per frame
    public void Skip(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            skip = true;
        }
    }
}
