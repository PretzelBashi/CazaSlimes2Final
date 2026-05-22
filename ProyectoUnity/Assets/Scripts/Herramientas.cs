
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

        public int saltosMax;
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
            float critico,
            int saltos
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
            this.saltosMax = saltos;
        }
    }
    public class Stats
    {
        public GameObject padre;
        EfectosDeSonido audioJugador = GameObject.FindGameObjectWithTag("SonidosJugador").GetComponent<EfectosDeSonido>();

        public float hpMax = 100;
        public float hpActual = 100;

        public float mpMax = 100;
        public float mpActual = 100;

        public float danoFisicoMax = 7;
        public float danoFisicoActual = 7;

        public float danoMagicoMax = 5;
        public float danoMagicoActual = 5;

        public float defensaFisicaMax = 5;
        public float defensaFisicaActual = 5;

        public float defensaMagicaMax = 5;
        public float defensaMagicaActual = 5;

        public float velocidadDeAtaqueMax = 0.7f;
        public float velocidadDeAtaqueActual = 0.7F;

        public float criticoActual = 2;
        public float criticoMax = 2;

        public float IFrames = 0.2f;
        public bool Invencible = false;

        public int saltosMax = 2;
        public int saltosActual = 2;

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
        async public void ActualizarHP(float danoARecibir, int tipoDeDano, float crit, GameObject prefabNumero)
        {
            if (Invencible ) return;

            if ((Random.Range(0,101)) <= crit)
            {
                Debug.Log("Critico");
                if (crit > 100)
                {
                    danoARecibir = (danoARecibir * 2) + (crit-100 + 1) * 2;
                }
                else
                {
                    danoARecibir *= 2;
                }

            }

            Invencible = true;
            float vidaFinal = 0;
            float danoRecibidoTotal = danoARecibir;
            
            switch (tipoDeDano) //0 es fisico, 1 es magico y 2 es curacion
            {
                case 0: if (danoARecibir < 0) { danoARecibir = 0; } vidaFinal = hpActual - (danoARecibir - defensaFisicaActual); danoRecibidoTotal = danoARecibir - defensaFisicaActual;  break;
                case 1: if (danoARecibir < 0) { danoARecibir = 0; } vidaFinal = hpActual - (danoARecibir - defensaMagicaActual); danoRecibidoTotal = danoARecibir - defensaMagicaActual; break;
                case 2: if (danoARecibir < 0) { danoARecibir = 0; } vidaFinal = hpActual + danoARecibir; danoRecibidoTotal = hpActual + danoRecibidoTotal; break;
            }
            if(tipoDeDano == 2)
            {
                Debug.Log(vidaFinal);
                Debug.Log(vidaFinal > 0);
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
                mpActual = mpMax;
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

        public void ActualizarStatsMaximas(float hpMax, float mpMax, float danoFisicoMax, float danoMagicoMax, float defensaFisicaMax, float defensaMagicaMax, float velocidadDeAtaqueMax, float criticoMax, int saltosMax, bool SumarOAbsoluto)
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

                if (this.criticoMax == criticoActual) { criticoActual += criticoMax; }
                this.criticoMax += criticoMax;

                if (this.saltosMax == saltosActual) { saltosActual += saltosMax; }
                this.saltosMax += saltosMax;
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
                case "criticoActual": criticoActual = nuevoValor; break;
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
                case "criticoActual": criticoActual = criticoMax; break;
            }

            GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().ActualizarStats();
        }

        public void ImprimirStats()
        {
            /*Debug.Log(
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

                "\ncritico: " + criticoMax

            );*/
        }
    }

    public class MagoStats : Stats
    {
        public int slimeRecolectado =0;

        public Habilidad[] habilidades = new Habilidad[4];
        public List<Objeto> objetos = new List<Objeto>();
        public MagoStats()
        {
            float cd1;
            if ((8 - this.velocidadDeAtaqueActual) < 0) { cd1 = 0.1f; } else { cd1 = 8 - this.velocidadDeAtaqueActual; }

            habilidades = new Habilidad[]{
                new Habilidad(cd1, 50),
                new Habilidad(8, 100),
                new Habilidad(30, 120),
                new Habilidad(120, 250)
            };
        }
        public void AgregarObjeto(Objeto objeto)
        {
            ActualizarStatsMaximas(objeto.hpMax,objeto.mpMax,objeto.danoFisicoMax,objeto.danoMagicoMax,objeto.defensaFisicaMax,objeto.defensaMagicaMax, objeto.velocidadDeAtaqueMax,objeto.critico,objeto.saltosMax, true);
            objetos.Add(objeto);
        }

        public void QuitarObjeto(int indice)
        {
            Objeto objeto = objetos[indice];
            ActualizarStatsMaximas(-objeto.hpMax, -objeto.mpMax, -objeto.danoFisicoMax, -objeto.danoMagicoMax, -objeto.defensaFisicaMax, -objeto.defensaMagicaMax, -objeto.velocidadDeAtaqueMax, -objeto.critico, -objeto.saltosMax, true);
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
