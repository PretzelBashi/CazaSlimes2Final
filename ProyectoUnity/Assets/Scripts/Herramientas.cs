
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static Herramientas;

[System.Serializable]
public class DatosGuardadosLocal
{
    public int cuartoMasAlto;
    public int slimesAsesinados;
    public int slimeRecolectado;
    public int cuartosRecorridos;
    public int dialogosTiendaEscuchados = 6;

    public bool[] logros = new bool[15];
    public List<Objeto> objetosDesbloqueados = new List<Objeto>();


    /* * (Set miku y teto) * 
     * 0- Asesina 100 slimes * 
     * 1- Asesina 500 slimes * 
     * 2- Asesina 700 slimes * 
     * 3- Asesina 1000 slimes *
     * 4- Asesina 2000 slimes * * (collares chistosos) * 
     * 
     * 5-Llega al cuarto 25 en esquizo (casco de calavera de venado o algo asi) * 
     * 6-Llega al cuarto 50 en intermedio o mas* 
     * 7-Llega al cuarto 100 en facil o mas * 
     * 8-Llega al cuarto 200 en facil o mas* * (Objetos unicos) * 
     * 
     * 9-Alcanza 1000 de dano magico / Baculo entropico * 
     * 10-Alcanza 1000 de dano fisico / Excalibur * 
     * 11-Alcanza 500 de defensa / Pechera esmaltada de mitrilo * 
     * 12-Alcanza 50 de velocidad de ataque / Matakrakens * * (Set adachi) * 
     * 13-Llega al cuarto 10 sin recibir dano * 
     * 14-Llega al cuarto 20 sin recibir dano */

    public void Guardar()
    {
        File.WriteAllText(
            Application.persistentDataPath + "/saveData.json",
            JsonUtility.ToJson(this, true)
        );
    }

    public static DatosGuardadosLocal Cargar()
    {
        string ruta = Application.persistentDataPath + "/saveData.json";

        if (!File.Exists(ruta)) return new DatosGuardadosLocal();

        return JsonUtility.FromJson<DatosGuardadosLocal>(File.ReadAllText(ruta));
    }
}
public class UltimasStats
{
    public static int cantidadObjetos;
    public static int cantidadSlimes;
    public static int cantidadCuevas;
    public static int cantidadSlimeRecolectado;
    public static MagoStats jugador;
    public static bool flawless;
}
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
    public class TodosLosDialogos
    {
        public List<Conversacion> todosLosDialogos = new List<Conversacion>();

        public TodosLosDialogos()
        {
            Conversacion tutorial = new Conversacion();

            tutorial.conversacion.Add(new Dialogo("Desconocida", "Que haces aqui?"));
            tutorial.conversacion.Add(new Dialogo("Tu (Saki)", "Eh? Quien eres? Como me estas hablando?"));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "Maldicion... esperaba alguien mas fuerte"));
            tutorial.conversacion.Add(new Dialogo("Tu (Saki)", "Que? Para que?..."));
            tutorial.conversacion.Add(new Dialogo("Tu (Saki)", "Me llamaste debil?"));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "Puedo ver que no sabes pelear"));
            tutorial.conversacion.Add(new Dialogo("Tu (Saki)", "Soy la maga mas fuerte de Asteria!"));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "Si... lo que digas"));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "Mi supervivencia depende de la tuya..."));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "Te mostrare lo basico"));
            tutorial.conversacion.Add(new Dialogo("Tu (Saki)", "..."));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "Usa W A S D o la palanca izquierda para moverte"));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "Presiona la palanca izquierda para dashear"));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "Usa el raton o la palanca derecha para ver a tu alrededor"));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "Con el click izquierdo o gatillo derecho usas tu ataque basico"));

            tutorial.conversacion.Add(new Dialogo("Desconocida", "Abajo puedes ver tus habilidades, el numero en azul es el mana que necesitas para usarlas"));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "La letra en la esquina inferior es la tecla a la que estan asignadas"));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "Si estas en control, es bumber izquierdo, boton derecho (o B), bumber derecho y gatillo izquierdo respectivamente"));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "Para avanzar la siguiente zona debes debilitar los cristales que tapan la entrada matando a todos los slimes"));

            tutorial.conversacion.Add(new Dialogo("Desconocida", "Parece que te encontraron, detecto un slime cerca"));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "Ves ese cubo blanco que tiro el slime? tocalo"));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "Estos son objetos tirados por los slimes, mientras mas alta su rareza, mas poderosos"));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "Presiona TAB o Select para abrir tu inventario"));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "Ahi encontraras tus stats. Consigue objetos y te haras mas fuerte"));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "Pon tu cursor sobre un objeto para ver sus stats"));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "Presiona TAB o Select para cerrar el inventario"));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "Eso es todo"));
            tutorial.conversacion.Add(new Dialogo("Desconocida", "Ven a verme en la siguiente zona"));

            todosLosDialogos.Add(tutorial);

            Conversacion tutorial2 = new Conversacion();

            tutorial2.conversacion.Add(new Dialogo("Tu (Saki)", "... que hace una chiquilla en este lugar?"));
            tutorial2.conversacion.Add(new Dialogo("Desconocida", "..."));
            tutorial2.conversacion.Add(new Dialogo("Desconocida", "No es relevante ahora."));
            tutorial2.conversacion.Add(new Dialogo("Tu (Saki)", "..."));
            tutorial2.conversacion.Add(new Dialogo("Tu (Saki)", "Y como salimos?"));
            tutorial2.conversacion.Add(new Dialogo("Desconocida", "No se, si lo supiera no estaria aqui"));
            tutorial2.conversacion.Add(new Dialogo("Tu (Saki)", "... gracias por nada entonces"));
            tutorial2.conversacion.Add(new Dialogo("Desconocida", "Espera!"));
            tutorial2.conversacion.Add(new Dialogo("Desconocida", "Puedo ayudarte. A cambio de slimes de dare objetos mas fuertes"));
            tutorial2.conversacion.Add(new Dialogo("Desconocida", "Acercate a mi y presiona E"));
            tutorial2.conversacion.Add(new Dialogo("Desconocida", "Haz click en el objeto que quieras y lo tranportare hacia aqui"));
            tutorial2.conversacion.Add(new Dialogo("Tu (Saki)", "Como haces eso?"));
            tutorial2.conversacion.Add(new Dialogo("Desconocida", "Esta piedra consume slime y lo convierte en mana, con el puedo transportar objetos de la mazmorra hacia ti"));
            tutorial2.conversacion.Add(new Dialogo("Tu (Saki)", "... ya veo"));
            tutorial2.conversacion.Add(new Dialogo("Desconocida", "Mientras mas avances, los slimes se haran mas fuertes, pero podre ofrecerte mejores objetos"));
            tutorial2.conversacion.Add(new Dialogo("Desconocida", "Buena suerte"));
            tutorial2.conversacion.Add(new Dialogo("Tu (Saki)", "Esta bien... supongo"));

            todosLosDialogos.Add(tutorial2);
            //2
            Conversacion muerteFacil = new Conversacion();

            muerteFacil.conversacion.Add(new Dialogo("Desconocida", "... perdon padre, parece que tuvimos mala suerte"));
            muerteFacil.conversacion.Add(new Dialogo("Tu (Saki)", "Q- Que? Desconocida? Estas aqui?"));
            muerteFacil.conversacion.Add(new Dialogo("Desconocida", "No te preocupes, tu camino no ha terminado"));
            muerteFacil.conversacion.Add(new Dialogo("Desconocida", "Aun te necesito"));

            todosLosDialogos.Add(muerteFacil);

            Conversacion muerteIntermedio = new Conversacion();

            muerteIntermedio.conversacion.Add(new Dialogo("Desconocida", "Nada mal, aprendes rapido"));
            muerteIntermedio.conversacion.Add(new Dialogo("Tu (Saki)", "AYUDAME! CURAME O ALGO!!!"));
            muerteIntermedio.conversacion.Add(new Dialogo("Desconocida", "No te preocupes, no camino no ha terminado"));
            muerteIntermedio.conversacion.Add(new Dialogo("Desconocida", "Aun te necesito"));

            todosLosDialogos.Add(muerteIntermedio);

            Conversacion muerteEsquizo = new Conversacion();

            muerteEsquizo.conversacion.Add(new Dialogo("Desconocida", "Perdon... pero es algo que tengo que hacer... "));
            muerteEsquizo.conversacion.Add(new Dialogo("Tu (Saki)", "Esta... no es la primera vez... verdad?"));
            muerteEsquizo.conversacion.Add(new Dialogo("Desconocida", "Ni sera la ultima"));
            muerteEsquizo.conversacion.Add(new Dialogo("Tu (Saki)", "... Ha... solo espero que mi tortura valga la pena para ti"));
            muerteEsquizo.conversacion.Add(new Dialogo("Desconocida", "... todo pronto acabara"));

            todosLosDialogos.Add(muerteEsquizo);

            Conversacion muerteEntropia = new Conversacion();

            muerteEntropia.conversacion.Add(new Dialogo("Desconocida", "Eres muy resistente... pero no lo suficiente"));
            muerteEntropia.conversacion.Add(new Dialogo("Tu (Saki)", "Por que... me haces esto?"));
            muerteEntropia.conversacion.Add(new Dialogo("Desconocida", "... perdon"));
            muerteEntropia.conversacion.Add(new Dialogo("Desconocida", "Lo hago por mi padre, no puedo rendirme ahora"));
            muerteEntropia.conversacion.Add(new Dialogo("Tu (Saki)", "... cada vez recuerdo mas tus palabras, desconocida..."));
            muerteEntropia.conversacion.Add(new Dialogo("Tu (Saki)", "Podrias... decirme tu nombre?"));
            muerteEntropia.conversacion.Add(new Dialogo("Desconocida", "Lo olvidaras... como mi padre"));

            todosLosDialogos.Add(muerteEntropia);

            Conversacion muerteFinal = new Conversacion();

            muerteFinal.conversacion.Add(new Dialogo("Desconocida", "Saki... "));
            muerteFinal.conversacion.Add(new Dialogo("Tu (Saki)", "Kjjj..."));
            muerteFinal.conversacion.Add(new Dialogo("Tu (Saki)", "Ojala no solo me hablaras cuando estoy apunto de mori"));
            muerteFinal.conversacion.Add(new Dialogo("Desconocida", "... si te dijera que podemos detener esto juntos..."));
            muerteFinal.conversacion.Add(new Dialogo("Desconocida", "Me ayudarias?"));
            muerteFinal.conversacion.Add(new Dialogo("Tu (Saki)", "Ha!..."));
            muerteFinal.conversacion.Add(new Dialogo("Tu (Saki)", "Tu sabes que no tendria otra opcion..."));
            muerteFinal.conversacion.Add(new Dialogo("Tu (Saki)", "Ademas... empiezas a caerme bien"));
            muerteFinal.conversacion.Add(new Dialogo("Desconocida", "Incluso aunque supieras que todo es mi culpa?"));
            muerteFinal.conversacion.Add(new Dialogo("Tu (Saki)", "Estoy segura de que... si fuera decision tuya... ya me hubieses liberado"));
            muerteFinal.conversacion.Add(new Dialogo("Desconocida", "..."));
            muerteFinal.conversacion.Add(new Dialogo("Desconocida", "Talvez... en otro mundo..."));
            muerteFinal.conversacion.Add(new Dialogo("Desconocida", "..."));
            muerteFinal.conversacion.Add(new Dialogo("Tu (Saki)", "... ugh... ha... solo... has lo que tengas que hacer, hermanita"));
            muerteFinal.conversacion.Add(new Dialogo("Tu (Saki)", "Confio en ti"));
            muerteFinal.conversacion.Add(new Dialogo("Desconocida", "..."));
            muerteFinal.conversacion.Add(new Dialogo("Desconocida", "Me llamo Entropy..."));
            muerteFinal.conversacion.Add(new Dialogo("Tu (Saki)", "Je... no se porque no me sorprende"));
            muerteFinal.conversacion.Add(new Dialogo("Desconocida", "-- GRACIAS POR PROBAR LA BETA!!!! NO SE QUIEN SEAS PERO TE AMO!!! :D (excepto " +
                "si eres Chris) TALVEZ EN UN FUTURO AGREGE EL JEFE FINAL!!!!! ADIOOOOOS!!!! -Pretzel"));

            todosLosDialogos.Add(muerteFinal); //6

            //7
            Conversacion tiendaDialogo1 = new Conversacion();

            tiendaDialogo1.conversacion.Add(new Dialogo("Desconocida", "Recuerda que en modo intermedio para abajo, los objetos unicos solo" +
                " aparecen hasta pasando la ronda 14"));
            tiendaDialogo1.conversacion.Add(new Dialogo("Tu (Saki)", "Y si mejor me vendes uno?"));
            tiendaDialogo1.conversacion.Add(new Dialogo("Desconocida", "Mientras mas avances, mas y mejores objetos te puedo ofrecer"));
            tiendaDialogo1.conversacion.Add(new Dialogo("Tu (Saki)", "... me das flojera"));
            tiendaDialogo1.conversacion.Add(new Dialogo("Desconocida", "Perdon?"));
            tiendaDialogo1.conversacion.Add(new Dialogo("Tu (Saki)", "Nada"));

            todosLosDialogos.Add(tiendaDialogo1);

            Conversacion tiendaDialogo2 = new Conversacion();

            tiendaDialogo2.conversacion.Add(new Dialogo("Desconocida", "Hola"));
            tiendaDialogo2.conversacion.Add(new Dialogo("Tu (Saki)", "... como llegaste aqui?"));
            tiendaDialogo2.conversacion.Add(new Dialogo("Desconocida", "Teletransportacion"));
            tiendaDialogo2.conversacion.Add(new Dialogo("Tu (Saki)", "Y por que no nos sacas con tu magia?"));
            tiendaDialogo2.conversacion.Add(new Dialogo("Desconocida", "Umm.."));
            tiendaDialogo2.conversacion.Add(new Dialogo("Desconocida", "Hay un campo magico que me reprime a solo lugares dentro de la cueva"));
            tiendaDialogo2.conversacion.Add(new Dialogo("Tu (Saki)", "Claro..."));

            todosLosDialogos.Add(tiendaDialogo2);

            Conversacion tiendaDialogo3 = new Conversacion();

            tiendaDialogo3.conversacion.Add(new Dialogo("Desconocida", "Sigue asi"));
            tiendaDialogo3.conversacion.Add(new Dialogo("Tu (Saki)", "Por que no me ayudas?"));
            tiendaDialogo3.conversacion.Add(new Dialogo("Desconocida", "Estoy encadenada a esta piedra"));
            tiendaDialogo3.conversacion.Add(new Dialogo("Tu (Saki)", "Oh... podria intentar romperla"));
            tiendaDialogo3.conversacion.Add(new Dialogo("Desconocida", "Es una cadena de mitrilo, tu magia es inutil contra ella"));
            tiendaDialogo3.conversacion.Add(new Dialogo("Tu (Saki)", "Mmm..."));
            tiendaDialogo3.conversacion.Add(new Dialogo("Tu (Saki)", "Entonces la piedra decide a donde vas?"));
            tiendaDialogo3.conversacion.Add(new Dialogo("Desconocida", "Algo asi"));
            tiendaDialogo3.conversacion.Add(new Dialogo("Tu (Saki)", "Y quien controla a la piedra?"));
            tiendaDialogo3.conversacion.Add(new Dialogo("Desconocida", "No se... ... te lo dire otro dia, talvez cuando pruebes tu fuerza contra " +
                "la entropia."));
            tiendaDialogo3.conversacion.Add(new Dialogo("Tu (Saki)", "Que es eso?"));
            tiendaDialogo3.conversacion.Add(new Dialogo("Desconocida", "Otro dia te lo dire"));

            todosLosDialogos.Add(tiendaDialogo3);

            Conversacion tiendaDialogo4 = new Conversacion();

            tiendaDialogo4.conversacion.Add(new Dialogo("Tu (Saki)", "Entonces que es la entropia?"));
            tiendaDialogo4.conversacion.Add(new Dialogo("Desconocida", "El caos"));
            tiendaDialogo4.conversacion.Add(new Dialogo("Tu (Saki)", "... Aja?"));
            tiendaDialogo4.conversacion.Add(new Dialogo("Desconocida", "Algo inconsistente, incalculable, impredecible"));
            tiendaDialogo4.conversacion.Add(new Dialogo("Tu (Saki)", "Y eso que tiene que ver con nosotros?"));
            tiendaDialogo4.conversacion.Add(new Dialogo("Desconocida", "Hay una... entidad, por asi decirlo, dentro de esta cueva"));
            tiendaDialogo4.conversacion.Add(new Dialogo("Desconocida", "Hace que esta cueva mute y evolucione con el tiempo"));
            tiendaDialogo4.conversacion.Add(new Dialogo("Tu (Saki)", "Entonces si la derrotamos podremos salir?"));
            tiendaDialogo4.conversacion.Add(new Dialogo("Desconocida", "..."));
            tiendaDialogo4.conversacion.Add(new Dialogo("Desconocida", "Podria decirse que si"));
            tiendaDialogo4.conversacion.Add(new Dialogo("Tu (Saki)", "Pues encontremosla de una vez!"));
            tiendaDialogo4.conversacion.Add(new Dialogo("Desconocida", "..."));
            tiendaDialogo4.conversacion.Add(new Dialogo("Desconocida", "Adelante"));

            todosLosDialogos.Add(tiendaDialogo4);

            Conversacion tiendaDialogo5 = new Conversacion();

            tiendaDialogo5.conversacion.Add(new Dialogo("Desconocida", "Una pregunta..."));
            tiendaDialogo5.conversacion.Add(new Dialogo("Tu (Saki)", "Dime"));
            tiendaDialogo5.conversacion.Add(new Dialogo("Desconocida", "Por que entraste a esta cueva?"));
            tiendaDialogo5.conversacion.Add(new Dialogo("Tu (Saki)", "Fama, claro"));
            tiendaDialogo5.conversacion.Add(new Dialogo("Desconocida", "..."));
            tiendaDialogo5.conversacion.Add(new Dialogo("Tu (Saki)", "Esta cueva tiene la leyenda de que nadie quien entra sale, ya se han perdido " +
                "cientos de aventureros aqui adentro"));
            tiendaDialogo5.conversacion.Add(new Dialogo("Desconocida", "Y aun asi decidiste entrar?"));
            tiendaDialogo5.conversacion.Add(new Dialogo("Tu (Saki)", "Le prometi a mi padre que lo haria sentir orgulloso"));
            tiendaDialogo5.conversacion.Add(new Dialogo("Tu (Saki)", "Aunque no lo creas, soy la mas debil de mi familia..."));
            tiendaDialogo5.conversacion.Add(new Dialogo("Desconocida", "..."));
            tiendaDialogo5.conversacion.Add(new Dialogo("Tu (Saki)", "No quiero decepcionar al viejo, no le queda mucho tiempo sabes?"));
            tiendaDialogo5.conversacion.Add(new Dialogo("Desconocida", "Pues... has sobrevivido mas que todos los demas..."));
            tiendaDialogo5.conversacion.Add(new Dialogo("Desconocida", "Eres fuerte, Saki"));
            tiendaDialogo5.conversacion.Add(new Dialogo("Tu (Saki)", "Has visto a mas gente entrar?!"));
            tiendaDialogo5.conversacion.Add(new Dialogo("Desconocida", "... te lo cuento despues."));

            todosLosDialogos.Add(tiendaDialogo5);

            Conversacion tiendaDialogo6 = new Conversacion();

            tiendaDialogo6.conversacion.Add(new Dialogo("Tu (Saki)", "Asi que... no soy la primera?"));
            tiendaDialogo6.conversacion.Add(new Dialogo("Desconocida", "..."));
            tiendaDialogo6.conversacion.Add(new Dialogo("Desconocida", "Antes de ti han venido varios, decenas"));
            tiendaDialogo6.conversacion.Add(new Dialogo("Desconocida", "Arrogantes, descuidados... con voluntades debiles"));
            tiendaDialogo6.conversacion.Add(new Dialogo("Desconocida", "Tu eres diferente Saki, eres la primera que resiste tanto tiempo"));
            tiendaDialogo6.conversacion.Add(new Dialogo("Tu (Saki)", "Bueno, apenas llevo aqui que, unas 2 o 3 horas?"));
            tiendaDialogo6.conversacion.Add(new Dialogo("Tu (Saki)", "Aunque..."));
            tiendaDialogo6.conversacion.Add(new Dialogo("Tu (Saki)", "Se que apenas nos conocimos hoy..."));
            tiendaDialogo6.conversacion.Add(new Dialogo("Tu (Saki)", "Pero por alguna razon siento que nos conocemos de hace mucho tiempo sabes?"));
            tiendaDialogo6.conversacion.Add(new Dialogo("Desconocida", "!..."));
            tiendaDialogo6.conversacion.Add(new Dialogo("Tu (Saki)", "Pasa algo?"));
            tiendaDialogo6.conversacion.Add(new Dialogo("Desconocida", "Nada... solo me parece raro lo que dices"));
            tiendaDialogo6.conversacion.Add(new Dialogo("Tu (Saki)", "Ja!, no te preocupes, ya estoy delirando, estos slimes me estan volviendo loca"));
            tiendaDialogo6.conversacion.Add(new Dialogo("Desconocida", "Ya veo..."));

            todosLosDialogos.Add(tiendaDialogo6);

            Conversacion tiendaDialogo7 = new Conversacion();

            tiendaDialogo7.conversacion.Add(new Dialogo("Desconocida", "Que haras cuando salgas?"));
            tiendaDialogo7.conversacion.Add(new Dialogo("Tu (Saki)", "Probablemente vender las toneladas de slime que he recolectado"));
            tiendaDialogo7.conversacion.Add(new Dialogo("Tu (Saki)", "Les contaremos a todos nuestra legendaria aventura y seremos las magas " +
                "mas poderosas de Asteria!"));
            tiendaDialogo7.conversacion.Add(new Dialogo("Desconocida", "..."));
            tiendaDialogo7.conversacion.Add(new Dialogo("Tu (Saki)", "Que? Creiste que te abandonaria? Me has ayudado mucho para dejarte aqui en " +
                "esta horrible cueva"));
            tiendaDialogo7.conversacion.Add(new Dialogo("Desconocida", "... Aunque pudiera separarme de esta piedra... no tengo a donde ir"));
            tiendaDialogo7.conversacion.Add(new Dialogo("Tu (Saki)", "Ya veremos que hacer hermanita"));
            tiendaDialogo7.conversacion.Add(new Dialogo("Desconocida", "Que?"));
            tiendaDialogo7.conversacion.Add(new Dialogo("Tu (Saki)", "Vamos, no puedes negar que yo soy la hermana mayor aqui"));
            tiendaDialogo7.conversacion.Add(new Dialogo("Desconocida", "Um... como digas...?"));

            todosLosDialogos.Add(tiendaDialogo7);

            Conversacion tiendaDialogo8 = new Conversacion(); //14

            tiendaDialogo8.conversacion.Add(new Dialogo("Desconocida", "Oye..."));
            tiendaDialogo8.conversacion.Add(new Dialogo("Tu (Saki)", "Que paso hermanita?"));
            tiendaDialogo8.conversacion.Add(new Dialogo("Desconocida", "... no me digas asi"));
            tiendaDialogo8.conversacion.Add(new Dialogo("Tu (Saki)", "Claro... hermanita"));
            tiendaDialogo8.conversacion.Add(new Dialogo("Desconocida", "... algo se acerca"));
            tiendaDialogo8.conversacion.Add(new Dialogo("Tu (Saki)", "Mas slimes?"));
            tiendaDialogo8.conversacion.Add(new Dialogo("Desconocida", "No... me refiero a... algo grande"));
            tiendaDialogo8.conversacion.Add(new Dialogo("Tu (Saki)", "No importa que sea, estoy segura que lo derrotaremos, hermanita"));
            tiendaDialogo8.conversacion.Add(new Dialogo("Desconocida", "... no quisieras ser mi hermana"));
            tiendaDialogo8.conversacion.Add(new Dialogo("Tu (Saki)", "Muy tarde"));
            tiendaDialogo8.conversacion.Add(new Dialogo("Desconocida", "... la entropia cada vez es mas fuerta, pronto llegara..."));
            tiendaDialogo8.conversacion.Add(new Dialogo("Tu (Saki)", "Quien?"));
            tiendaDialogo8.conversacion.Add(new Dialogo("Desconocida", "Cuando estes lista... te lo dire... hermana"));
            tiendaDialogo8.conversacion.Add(new Dialogo("Tu (Saki)", "Que mas quieres que haga para que me digas la verdad?"));
            tiendaDialogo8.conversacion.Add(new Dialogo("Desconocida", "... si te digo, mi padre..."));
            tiendaDialogo8.conversacion.Add(new Dialogo("Tu (Saki)", "Presentamelo, tengo experiencia tratando con padres ausentes"));
            tiendaDialogo8.conversacion.Add(new Dialogo("Desconocida", "... demuestrame que eres lo suficientemente fuerte... " +
                "llega a la cueva 25 en entropia y te lo contare"));
            tiendaDialogo8.conversacion.Add(new Dialogo("Tu (Saki)", "Je, no me subestimes"));

            todosLosDialogos.Add(tiendaDialogo8);
        }
    }
    public class Conversacion
    {
        public List<Dialogo> conversacion = new List<Dialogo>();
    }
    public class Dialogo
    {
        public string personaje;
        public string mensaje;
        public Dialogo(string personaje, string mensaje)
        {
            this.personaje = personaje;
            this.mensaje = mensaje;
        }
    }
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


        public void ActualizarUIs(bool curacion, bool critico)
        {
            if (padre.tag == "Slime")
            {
                padre.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = (hpActual+0.1f)/hpMax;

                if (critico){padre.GetComponent<AudioSource>().PlayOneShot(audioJugador.Critico);} 
                else {padre.GetComponent<AudioSource>().PlayOneShot(audioJugador.DanoSlime);}
                    
                if (hpActual <= 0)
                {
                    padre.GetComponent<Slime>().DestruirSlime();
                }
            }
            else { 
                GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().ActualizarStats();
                if (curacion)
                {
                    audioJugador.Positivos.PlayOneShot(audioJugador.Curacion);
                } else
                {
                    if (critico) {audioJugador.ReproducirNegativos(audioJugador.Critico);}
                    else { audioJugador.ReproducirNegativos(audioJugador.JugadorDano); }
                        
                }
                if(padre.GetComponent<Jugador>().jugadorStats.hpActual <= 0)
                {
                    GameObject.FindGameObjectWithTag("infoPartidaActual").GetComponent<infoPartidaActual>().TerminarPartida();
                }
            }

        }
        async public void ActualizarHP(float danoARecibir, int tipoDeDano, float crit, GameObject prefabNumero)
        {
            bool curacion = false;
            bool critico = false;
            if (Invencible ) return;

            if ((Random.Range(0, 101)) <= crit)
            {
            
                if (crit > 100)
                {
                    danoARecibir = (danoARecibir * 2) + (crit-100 + 1) * 2;
                }
                else
                {
                    danoARecibir *= 2;
                    critico = true;
                }

            }

            Invencible = true;
            float vidaFinal = 0;
            float danoRecibidoTotal = danoARecibir;

            switch (tipoDeDano) //0 es fisico, 1 es magico y 2 es curacion
            {
                case 0:
                    if (danoARecibir - defensaFisicaActual < 0) {danoARecibir = 0; vidaFinal = hpActual; }
                    else { vidaFinal = hpActual - (danoARecibir - defensaFisicaActual); danoRecibidoTotal = danoARecibir - defensaFisicaActual; }
                        
                    break;
                case 1:
                    if (danoARecibir - defensaMagicaActual < 0) { danoARecibir = 0; vidaFinal = hpActual; }
                    else { vidaFinal = hpActual - (danoARecibir - defensaMagicaActual); danoRecibidoTotal = danoARecibir - defensaMagicaActual; }

                    break;
                case 2: vidaFinal = hpActual + danoARecibir; danoRecibidoTotal = danoARecibir; curacion = true; break;
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
            ActualizarUIs(curacion, critico);
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
    public static float dificultad = 1;
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
