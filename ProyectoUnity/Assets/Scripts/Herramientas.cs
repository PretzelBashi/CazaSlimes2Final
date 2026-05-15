using System.Runtime.InteropServices;
using UnityEngine;


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
    public class Stats
    {
        public float hpMax = 100;
        public float hpActual = 100;

        public float mpMax = 100;
        public float mpActual = 100;

        public float danoFisicoMax = 5;
        public float danoFisicoActual = 5;

        public float danoMagicoMax = 5;
        public float danoMagicoActual = 5;

        public float defensaFisicaMax = 5;
        public float defensaFisicaActual = 5;

        public float defensaMagicaMax = 5;
        public float defensaMagicaActual = 5;

        public float velocidadDeAtaqueMax = 1f;
        public float velocidadDeAtaqueActual = 1F;

        public float critico = 5;

        //Falta aplicar daño critico y todo eso

        public void ActualizarHP(float danoARecibir, int tipoDeDano)
        {
            float vidaFinal = 0;

            switch (tipoDeDano) //0 es fisico, 1 es magico y 2 es curacion
            {
                case 0: vidaFinal = hpActual - (danoARecibir - defensaFisicaActual); break;
                case 1: vidaFinal = hpActual - (danoARecibir - defensaFisicaActual); break;
                case 2: vidaFinal = hpActual + danoARecibir; break;
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
                Debug.Log("Muerte");
            }

            GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().ActualizarStats();
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

        public void ActualizarStatsMaximas(float hpMax, float mpMax, float danoFisicoMax, float danoMagicoMax, float defensaFisicaMax, float defensaMagicaMax, bool SumarOAbsoluto)
        {
            if (SumarOAbsoluto) //Sumas
            {
                this.hpMax += hpMax;
                this.mpMax += mpMax;
                this.danoFisicoMax += danoFisicoMax;
                this.danoMagicoMax += danoMagicoMax;
                this.defensaFisicaMax += defensaFisicaMax;
                this.defensaMagicaMax += defensaMagicaMax;
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

    }

    public class MagoStats : Stats
    {
        public Habilidad[] habilidades = new Habilidad[4];
        public MagoStats()
        {
            float cd1;
            if ((8 - this.velocidadDeAtaqueActual) < 0) { cd1 = 0.1f; } else { cd1 = 8 - this.velocidadDeAtaqueActual; }

            habilidades = new Habilidad[]{
                new Habilidad(cd1, 20),
                new Habilidad(8, 70),
                new Habilidad(10, 50),
                new Habilidad(120, 250)
            };
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
