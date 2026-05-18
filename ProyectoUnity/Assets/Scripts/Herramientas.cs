
using System.Collections.Generic;

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static Herramientas;


public class Herramientas : MonoBehaviour
{
    // Update is called once per frame
    /*  Sacamos hipotenusa
     *  
     *  Cateto adyacente con X, cateto opuesto con Y
     * 
     *  sen = cateto opuesto / hipotenusa
     *  cos = cateto adyacente / hipotenusa
     *  tan = seno / coseno
     *  
     *  Se saca la opuesta
     *  
     *  sen^-1
     *  cos^-1
     *  tan^-1
     *  
     */
    public class Habilidad
    {
        public float cooldownMax;
        public float cooldownActual;
        public float costoMana;

        public Habilidad(float cooldown, float costoMana)
        {
            this.cooldownMax = cooldown;
            this.cooldownActual = cooldown;
            this.costoMana = costoMana;
        }
    }

    public class Objeto
    {
        public string nombre;
        public int id;
        public int rareza;

        public float hpMax;

        public float mpMax;

        public float danoFisicoMax;

        public float danoMagicoMax;

        public float defensaFisicaMax;

        public float defensaMagicaMax;

        public float velocidadDeAtaqueMax;

        public float critico;

        public Objeto(
            string nombre,
            int id,
            int rareza,
            float hpMax,
            float mpMax,
            float danoFisicoMax,
            float danoMagicoMax,
            float defensaFisicaMax,
            float defensaMagicaMax,
            float velocidadDeAtaqueMax,
            float critico
        )
        {
            this.nombre = nombre;
            this.id = id;
            this.rareza = rareza;
            this.hpMax = hpMax;
            this.mpMax = mpMax;
            this.danoFisicoMax = danoFisicoMax;
            this.danoMagicoMax = danoMagicoMax;
            this.defensaFisicaMax = defensaFisicaMax;
            this.defensaMagicaMax = defensaMagicaMax;
            this.velocidadDeAtaqueMax = velocidadDeAtaqueMax;
            this.critico = critico;
        }
    }
    public class Stats
    {
        public GameObject padre;
        EfectosDeSonido audioJugador = GameObject.FindGameObjectWithTag("SonidosJugador").GetComponent<EfectosDeSonido>();

        public float hpMax = 100;
        public float hpActual = 100;

        public float mpMax = 9000;
        public float mpActual = 9000;

        public float danoFisicoMax = 7;
        public float danoFisicoActual = 7;

        public float danoMagicoMax = 5;
        public float danoMagicoActual = 5;

        public float defensaFisicaMax = 5;
        public float defensaFisicaActual = 5;

        public float defensaMagicaMax = 5;
        public float defensaMagicaActual = 5;

        public float velocidadDeAtaqueMax = 3f;
        public float velocidadDeAtaqueActual = 3F;

        public float critico = 5;
        public float IFrames = 0.2f;
        public bool Invencible = false;

        //Falta aplicar daño critico y todo eso
        public void ActualizarUIs()
        {
            if (padre.tag == "Slime")
            {
                padre.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = (hpActual+0.1f)/hpMax;
                if(hpActual <= 0)
                {
                    padre.GetComponent<Slime>().DestruirSlime();
                }
            }
            else { 
                GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().ActualizarStats();
                audioJugador.ReproducirNegativos(audioJugador.JugadorDano);
            }
        }
        async public void ActualizarHP(float danoARecibir, int tipoDeDano, GameObject prefabNumero)
        {
            if (Invencible) return;
            Invencible = true;
            float vidaFinal = 0;
            float danoRecibidoTotal = danoARecibir;
            
            switch (tipoDeDano) //0 es fisico, 1 es magico y 2 es curacion
            {
                case 0: if (danoARecibir < 0) { danoARecibir = 0; } vidaFinal = hpActual - (danoARecibir - defensaFisicaActual); danoRecibidoTotal = danoARecibir - defensaFisicaActual;  break;
                case 1: if (danoARecibir < 0) { danoARecibir = 0; } vidaFinal = hpActual - (danoARecibir - defensaMagicaActual); danoRecibidoTotal = danoARecibir - defensaMagicaActual; break;
                case 2: if (danoARecibir < 0) { danoARecibir = 0; } vidaFinal = hpActual + danoARecibir; break;
            }

            if (vidaFinal > hpMax)
            {
                hpActual = hpMax;
            }
            else if (vidaFinal > 0)
            {
                hpActual = vidaFinal;
            }
            else
            {
                hpActual = 0;
            }
            ActualizarUIs();
            GameObject numero = Instantiate(prefabNumero, padre.transform.position, Quaternion.identity);
            numero.transform.SetParent(padre.transform);
            numero.GetComponentInChildren<numeroDamage>().CambiarNumero(danoRecibidoTotal,tipoDeDano);
            await Task.Delay(Mathf.FloorToInt(IFrames * 1000));
            Invencible = false;
        }


        public void ActualizarMP(float mpRecibido) //Pueden ser negativos
        {
            float mpFinal = 0;
            mpFinal = mpActual + mpRecibido;

            if (mpFinal > mpMax)
            {
                mpActual = mpActual;
            }
            else if (mpFinal > 0)
            {
                mpActual = mpFinal;
            }
            else
            {
                mpActual = 0;
            }

            GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().ActualizarStats();
        }

        public void ActualizarStatsMaximas(float hpMax, float mpMax, float danoFisicoMax, float danoMagicoMax, float defensaFisicaMax, float defensaMagicaMax, float velocidadDeAtaqueMax, float critico, bool SumarOAbsoluto)
        {
            if (SumarOAbsoluto) //Sumas
            {
                if (this.hpMax == hpActual) { hpActual += hpMax; }
                this.hpMax += hpMax;

                if (this.mpMax == mpActual) { mpActual += mpMax; }
                this.mpMax += mpMax;

                if (this.danoFisicoMax == danoFisicoActual) { danoFisicoActual += danoFisicoMax; }
                this.danoFisicoMax += danoFisicoMax;

                if (this.danoMagicoMax == danoMagicoActual) { danoMagicoActual += danoMagicoMax; }
                this.danoMagicoMax += danoMagicoMax;

                if (this.defensaFisicaMax == defensaFisicaActual) { defensaFisicaActual += defensaFisicaMax; }
                this.defensaFisicaMax += defensaFisicaMax;

                if (this.defensaMagicaMax == defensaMagicaActual) { defensaMagicaActual += defensaMagicaMax; }
                this.defensaMagicaMax += defensaMagicaMax;

                if (this.velocidadDeAtaqueMax == velocidadDeAtaqueActual) { velocidadDeAtaqueActual += velocidadDeAtaqueMax; }
                this.velocidadDeAtaqueMax += velocidadDeAtaqueMax;

                this.critico += critico;
            }

            else //Valores absolutos (Por si acaso)
            {
                if (hpMax > 0) { this.hpMax = hpMax; }
                if (mpMax > 0) { this.mpMax = mpMax; }
                if (danoFisicoMax > 0) { this.danoFisicoMax = danoFisicoMax; }
                if (danoMagicoMax > 0) { this.danoMagicoMax = danoMagicoMax; }
                if (defensaFisicaMax > 0) { this.defensaFisicaMax = defensaFisicaMax; }
                if (defensaMagicaMax > 0) { this.defensaMagicaMax = defensaMagicaMax; }
            }

            GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().ActualizarStats();
        }

        public void ActualizarStatsActuales(string statACambiar, float nuevoValor)
        {
            switch (statACambiar)
            {
                case "danoFisico": danoFisicoActual = nuevoValor; break;
                case "danoMagico": danoMagicoActual = nuevoValor; break;
                case "defensaFisica": defensaFisicaActual = nuevoValor; break;
                case "defensaMagica": defensaMagicaActual = nuevoValor; break;
            }

            GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().ActualizarStats();
        }

        public void ReiniciarStatActual(string statAReiniciar)
        {
            switch (statAReiniciar)
            {
                case "hp": hpActual = hpMax; break;
                case "mp": mpActual = mpMax; break;
                case "danoFisico": danoFisicoActual = danoFisicoMax; break;
                case "danoMagico": danoMagicoActual = danoMagicoMax; break;
                case "defensaFisica": defensaFisicaActual = defensaFisicaMax; break;
                case "defensaMagica": defensaMagicaActual = defensaMagicaMax; break;
            }

            GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().ActualizarStats();
        }

        public void ImprimirStats()
        {
            Debug.Log(
                "hpMax: " + hpMax +
                "\nhpActual: " + hpActual +

                "\nmpMax: " + mpMax +
                "\nmpActual: " + mpActual +

                "\ndanoFisicoMax: " + danoFisicoMax +
                "\ndanoFisicoActual: " + danoFisicoActual +

                "\ndanoMagicoMax: " + danoMagicoMax +
                "\ndanoMagicoActual: " + danoMagicoActual +

                "\ndefensaFisicaMax: " + defensaFisicaMax +
                "\ndefensaFisicaActual: " + defensaFisicaActual +

                "\ndefensaMagicaMax: " + defensaMagicaMax +
                "\ndefensaMagicaActual: " + defensaMagicaActual +

                "\nvelocidadDeAtaqueMax: " + velocidadDeAtaqueMax +
                "\nvelocidadDeAtaqueActual: " + velocidadDeAtaqueActual +

                "\ncritico: " + critico

            );
        }
    }

    public class MagoStats : Stats
    {
        public Habilidad[] habilidades = new Habilidad[4];
        public List<Objeto> objetos = new List<Objeto>();
        public MagoStats()
        {
            float cd1;
            if ((8 - this.velocidadDeAtaqueActual) < 0) { cd1 = 0.1f; } else { cd1 = 8 - this.velocidadDeAtaqueActual; }

            habilidades = new Habilidad[]{
                new Habilidad(cd1, 100),
                new Habilidad(8, 70),
                new Habilidad(10, 50),
                new Habilidad(120, 250)
            };
        }
        public void AgregarObjeto(Objeto objeto)
        {
            ActualizarStatsMaximas(objeto.hpMax,objeto.mpMax,objeto.danoFisicoMax,objeto.danoMagicoMax,objeto.defensaFisicaMax,objeto.defensaMagicaMax, objeto.velocidadDeAtaqueMax,objeto.critico,true);
            objetos.Add(objeto);
        }

        public void QuitarObjeto(int indice)
        {
            Objeto objeto = objetos[indice];
            ActualizarStatsMaximas(-objeto.hpMax, -objeto.mpMax, -objeto.danoFisicoMax, -objeto.danoMagicoMax, -objeto.defensaFisicaMax, -objeto.defensaMagicaMax, -objeto.velocidadDeAtaqueMax, -objeto.critico, true);
            objetos.RemoveAt(indice);
        }
    }

    public static bool perspectiva;
    static public float ObtenerAngulo2D(Vector2 punto1, Vector2 punto2)
    {
        float hipotenusa = Mathf.Sqrt(Mathf.Pow(punto2.x - punto1.x, 2) + Mathf.Pow(punto2.y - punto1.y, 2));
        float catetoAdyacente = punto2.x - punto1.x;
        float cos = catetoAdyacente / hipotenusa;
        float arcocoseno = Mathf.Acos(cos) * Mathf.Rad2Deg;



        if (punto2.y < punto1.y)
        {
            arcocoseno = -arcocoseno;
        }

        return -arcocoseno;
    }
}
