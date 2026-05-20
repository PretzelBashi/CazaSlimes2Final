using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

//public class CreadorDeEscenarios : MonoBehaviour


//Falta una forma de cargar y descargar cuartos
public class EscenariosDisponibles
{
    public List<Escenario> escenariosDisponibles;
    public EscenariosDisponibles(List<GameObject> escenarios)
    {
        escenariosDisponibles = new List<Escenario>();
        for (int i = 0; i < escenarios.Count; i++)
        {
            escenariosDisponibles.Add(new Escenario(escenarios[i], 0, i));
            //0 Cueva inicial 41x41
        }
    }
}



public class IntermediosDisponibles
{
    public List<Escenario> intermediosDisponibles;
    public IntermediosDisponibles(List<GameObject> intermedios)
    {
        intermediosDisponibles = new List<Escenario>();
        for (int i = 0; i < intermedios.Count; i++)
        {
            intermediosDisponibles.Add(new Escenario(intermedios[i], 0, i));
            //0 Puente placeHolder 41x41
        }
    }
}



public class Escenario
{
    public GameObject prefabEscenario;
    public int id; //id -1 es un puente
    public int tipoDeCuarto;
    public int[] colisiones = new int[4]; //0 vacio, 1 pared, 2 intermedio, 3 cueva, 4 leido
    public bool renderizado = false;


    public Escenario(GameObject prefab, int tipoDeCuarto, int id)
    {
        prefabEscenario = prefab;
        this.tipoDeCuarto = tipoDeCuarto;
        this.id = id;
    }
}



public class CreadorDeEscenarios : MonoBehaviour
{
    public List<GameObject> escenariosACargar;
    public List<GameObject> intermediosACargar;

    //Falta generacion interna procedural de la cueva
    public List<GameObject> elementosSuelo;
    public List<GameObject> elementosEstalactitas;
    public List<GameObject> elementosPlataformas;
    public List<GameObject> elementosPuertasBloqueadas;
    public List<GameObject> elementosPuertasAccesibles;

    EscenariosDisponibles escenariosCargados;
    IntermediosDisponibles intermediosCargados;


    public List<Escenario> mapa;
    List<Vector3> coordenadasPre;
    List<Vector3> coordenadas;
    Vector3 coordenadasBuffer;
    Escenario cuartoBuffer;
    Escenario cuartoParaAsignar;

    Vector3 posicionBuffer;
    int direccionBuffer;
    int bufferDireccion;
    int cuartoActual;
    int coordenadaActual;
    int bufferDireccionAnterior;
    infoPartidaActual infoPartidaActual;
    void Start()
    {
        escenariosCargados = new EscenariosDisponibles(escenariosACargar);
        intermediosCargados = new IntermediosDisponibles(intermediosACargar);

        mapa = new List<Escenario>();

        coordenadas = new List<Vector3>();
        coordenadasPre = new List<Vector3>();
        posicionBuffer = new Vector3();

        coordenadasBuffer = new Vector2(0, 0);
        bufferDireccion = Random.Range(0, 4);

        direccionBuffer = 0;
        cuartoActual = 0;
        coordenadaActual = 0;

        infoPartidaActual = GameObject.FindGameObjectWithTag("infoPartidaActual").GetComponent<infoPartidaActual>();
        infoPartidaActual.PartidaNormal();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public List<Vector3> ConseguirCoordenadas()
    {
        return coordenadas;
    }
    public void EjecutarCuarto(int cuarto)
    {

    }
    //
    async public Task ReemplazarCoordenadas()
    {
        for (;coordenadaActual < coordenadasPre.Count;coordenadaActual++)
        {
            Vector3 altoAncho = new Vector3();

            switch (mapa[coordenadaActual].tipoDeCuarto)
            {
                case 2:
                    switch (mapa[coordenadaActual].id)
                    {
                        case 0: altoAncho = new Vector3(39, 40, 39); break; //CuevaDefault
                    } break;
                case 3:
                    switch (mapa[coordenadaActual].id)
                    {
                        case 0: altoAncho = new Vector3(39, 40, 39); break; //PuenteDefault
                    } break;
            }
            coordenadas[coordenadaActual] = new Vector3(coordenadas[coordenadaActual].x * altoAncho.x, coordenadas[coordenadaActual].y * altoAncho.y, coordenadas[coordenadaActual].z * altoAncho.z);
        }
        return;
    }
    async public Task CargarMapa(int cantidadDeCuartos)
    {


        if(cuartoActual == 0)
        {
            cuartoParaAsignar = escenariosCargados.escenariosDisponibles[Random.Range(0, escenariosCargados.escenariosDisponibles.Count)]; 
            cuartoBuffer = new Escenario(cuartoParaAsignar.prefabEscenario, cuartoParaAsignar.tipoDeCuarto, cuartoParaAsignar.id);
            //Continuar la simplificacion de esta funcion. Despues hacer la generacion de los elementos de la cueva

            cuartoBuffer.tipoDeCuarto = 3;
            cuartoBuffer.colisiones[bufferDireccion] = 2;

            coordenadasPre.Add(new Vector2(0, 0));
            coordenadas.Add(new Vector2(0, 0));
            for (int i = 0; i < 4; i++) { if (i != bufferDireccion) { cuartoBuffer.colisiones[i] = 1; }  }
            for (int i = 0; i < 4; i++) {  }
            

            mapa.Add(cuartoBuffer);

            for (int i = 1; i < cantidadDeCuartos; i++) //Creacion del mapa entre intermedios y cuevas (probablemente)
            {
                CargarCuarto(mapa.Count, mapa[mapa.Count -1].tipoDeCuarto); //Arreglar eso, problema de indices y el primer ciclo
                
            }
        } else
        {

            cantidadDeCuartos++;
            for (int i = 1; i < cantidadDeCuartos; i++) //Creacion del mapa entre intermedios y cuevas (probablemente)
            {
                CargarCuarto(mapa.Count, mapa[mapa.Count -1].tipoDeCuarto);

                //Debug.Log(bufferDireccion);
                
                //Debug.Log($"{cuartoBuffer.colisiones[0]}, {cuartoBuffer.colisiones[1]}, {cuartoBuffer.colisiones[2]}, {cuartoBuffer.colisiones[3]}");
            }
        }


        //Debug.Log(bufferDireccion);


        //---------- :3 
        //Cuevas de 2.5 x 2.5
        //puente 2 x 2
        return;
    }
    public void DesrrenderizarMapa(int posis)
    {

    }
    async public Task<Vector3[]> RenderizarCuartos(int cantidadDeCuartosACargar, int posicionReferencia)
    {

        //for (int i = 0; i < mapa.Count; i++) { Debug.Log($"{mapa[i].colisiones[0]}, {mapa[i].colisiones[1]}, {mapa[i].colisiones[2]}, {mapa[i].colisiones[3]}"); }
        //Debug.Log(cuartoActual);
        //Debug.Log(cantidadDeCuartosACargar);
        int cuartoActualBuffer = cuartoActual;
        List<Vector3> coordenadasCargadas = new List<Vector3>();

        for (; cuartoActual < (cuartoActualBuffer + cantidadDeCuartosACargar); cuartoActual++)
        {
            
            
            //Debug.Log(direccionBuffer);
            posicionBuffer = new Vector3(coordenadas[cuartoActual].x, posicionBuffer.y, coordenadas[cuartoActual].y); // Agregar que la posicion aumente dependiendo en la direccion en la que estamos creando esto
            mapa[cuartoActual].renderizado = true;
            GameObject objetoN = Instantiate(mapa[cuartoActual].prefabEscenario, coordenadas[cuartoActual], Quaternion.identity);
            Vector3 posicionCuarto = objetoN.transform.position;
            if ((cuartoActual == infoPartidaActual.partida.cuartosPorProfundidad * infoPartidaActual.partida.profundidad))
            {
                for (int i = 0; i < 4; i++)
                {

                    switch (i)
                    {
                        case 0:
                            if (mapa[cuartoActual].colisiones[i] != 2 && mapa[cuartoActual].colisiones[i] != 3) { Instantiate(elementosPuertasBloqueadas[0], posicionCuarto + new Vector3(-18.5f, objetoN.transform.position.y, 0), Quaternion.Euler(0, 90, 0)).transform.SetParent(objetoN.transform); }
                            else { Instantiate(elementosPuertasAccesibles[0], posicionCuarto + new Vector3(-18.5f, objetoN.transform.position.y, 0), Quaternion.Euler(0, 90, 0)).transform.SetParent(objetoN.transform); bufferDireccionAnterior = i; }
                            break;
                        case 1:
                            if (mapa[cuartoActual].colisiones[i] != 2 && mapa[cuartoActual].colisiones[i] != 3) { Instantiate(elementosPuertasBloqueadas[0], posicionCuarto + new Vector3(0, objetoN.transform.position.y, 18.5f), Quaternion.Euler(0, 180, 0)).transform.SetParent(objetoN.transform); }
                            else { Instantiate(elementosPuertasAccesibles[0], posicionCuarto + new Vector3(0, objetoN.transform.position.y, 18.5f), Quaternion.Euler(0, 180, 0)).transform.SetParent(objetoN.transform); bufferDireccionAnterior = i; }
                            break;
                        case 2:
                            if (mapa[cuartoActual].colisiones[i] != 2 && mapa[cuartoActual].colisiones[i] != 3) { Instantiate(elementosPuertasBloqueadas[0], posicionCuarto + new Vector3(18.5f, objetoN.transform.position.y, 0), Quaternion.Euler(0, 270, 0)).transform.SetParent(objetoN.transform); }
                            else { Instantiate(elementosPuertasAccesibles[0], posicionCuarto + new Vector3(18.5f, objetoN.transform.position.y, 0), Quaternion.Euler(0, 270, 0)).transform.SetParent(objetoN.transform); bufferDireccionAnterior = i; }
                            break;
                        case 3:
                            if (mapa[cuartoActual].colisiones[i] != 2 && mapa[cuartoActual].colisiones[i] != 3) { Instantiate(elementosPuertasBloqueadas[0], posicionCuarto + new Vector3(0, objetoN.transform.position.y, -18.5f), Quaternion.Euler(0, 360, 0)).transform.SetParent(objetoN.transform); }
                            else { Instantiate(elementosPuertasAccesibles[0], posicionCuarto + new Vector3(0, objetoN.transform.position.y, -18.5f), Quaternion.Euler(0, 360, 0)).transform.SetParent(objetoN.transform); bufferDireccionAnterior = i; }
                            break;
                    }
                }
            } else
            {
                int puertaAnterior = 0;
                switch (bufferDireccionAnterior) { case 0: puertaAnterior = 2; break; case 1: puertaAnterior = 3; break; case 2: puertaAnterior = 0; break; case 3: puertaAnterior = 1; break; }

                for (int i = 0; i < 4; i++)
                {
                    if(i != puertaAnterior)
                    {
                        switch (i)
                        {
                            case 0:
                                if (mapa[cuartoActual].colisiones[i] != 2 && mapa[cuartoActual].colisiones[i] != 3) { Instantiate(elementosPuertasBloqueadas[0], posicionCuarto + new Vector3(-18.5f, objetoN.transform.position.y, 0), Quaternion.Euler(0, 90, 0)).transform.SetParent(objetoN.transform); }
                                else { Instantiate(elementosPuertasAccesibles[0], posicionCuarto + new Vector3(-18.5f, objetoN.transform.position.y, 0), Quaternion.Euler(0, 90, 0)).transform.SetParent(objetoN.transform); bufferDireccionAnterior = i; }
                                break;
                            case 1:
                                if (mapa[cuartoActual].colisiones[i] != 2 && mapa[cuartoActual].colisiones[i] != 3) { Instantiate(elementosPuertasBloqueadas[0], posicionCuarto + new Vector3(0, objetoN.transform.position.y, 18.5f), Quaternion.Euler(0, 180, 0)).transform.SetParent(objetoN.transform); }
                                else { Instantiate(elementosPuertasAccesibles[0], posicionCuarto + new Vector3(0, objetoN.transform.position.y, 18.5f), Quaternion.Euler(0, 180, 0)).transform.SetParent(objetoN.transform); bufferDireccionAnterior = i; }
                                break;
                            case 2:
                                if (mapa[cuartoActual].colisiones[i] != 2 && mapa[cuartoActual].colisiones[i] != 3) { Instantiate(elementosPuertasBloqueadas[0], posicionCuarto + new Vector3(18.5f, objetoN.transform.position.y, 0), Quaternion.Euler(0, 270, 0)).transform.SetParent(objetoN.transform); }
                                else { Instantiate(elementosPuertasAccesibles[0], posicionCuarto + new Vector3(18.5f, objetoN.transform.position.y, 0), Quaternion.Euler(0, 270, 0)).transform.SetParent(objetoN.transform); bufferDireccionAnterior = i; }
                                break;
                            case 3:
                                if (mapa[cuartoActual].colisiones[i] != 2 && mapa[cuartoActual].colisiones[i] != 3) { Instantiate(elementosPuertasBloqueadas[0], posicionCuarto + new Vector3(0, objetoN.transform.position.y, -18.5f), Quaternion.Euler(0, 360, 0)).transform.SetParent(objetoN.transform); }
                                else { Instantiate(elementosPuertasAccesibles[0], posicionCuarto + new Vector3(0, objetoN.transform.position.y, -18.5f), Quaternion.Euler(0, 360, 0)).transform.SetParent(objetoN.transform); bufferDireccionAnterior = i; }
                                break;
                        }
                    }
                }
            }
            
 

            objetoN.GetComponent<cuevaInfo>().actualizarCoordenadas(new Vector3(coordenadas[cuartoActual].x, coordenadas[cuartoActual].y, coordenadas[cuartoActual].z));
            //Debug.Log($"{coordenadas[cuartoActual].x},{coordenadas[cuartoActual].y},{coordenadas[cuartoActual].z}");
            coordenadasCargadas.Add(new Vector3(coordenadas[cuartoActual].x, coordenadas[cuartoActual].y, coordenadas[cuartoActual].z));
            //Debug.Log(new Vector3(coordenadasPre[cuartoActual].x, coordenadasPre[cuartoActual].y, coordenadasPre[cuartoActual].z));
        }
        return  coordenadasCargadas.ToArray();

    }
    public void CargarCuarto(int i, int cuartoActual)
    {
        int direccionTemp2 = 0;
        bool coordinadasValidar2 = true;
        float altura = 0;
        switch (bufferDireccion) { case 0: direccionTemp2 = 2; break; case 1: direccionTemp2 = 3; break; case 2: direccionTemp2 = 0; break; case 3: direccionTemp2 = 1; break; }

        switch (bufferDireccion)
        {
            case 0:
                coordenadasBuffer = new Vector3(coordenadasPre[i - 1].x - 1, altura, coordenadasPre[i - 1].z);
                break;
            case 1:
                coordenadasBuffer = new Vector3(coordenadasPre[i - 1].x, altura, coordenadasPre[i - 1].z + 1);
                break;
            case 2:
                coordenadasBuffer = new Vector3(coordenadasPre[i - 1].x + 1, altura, coordenadasPre[i - 1].z);
                break;
            case 3:
                coordenadasBuffer = new Vector3(coordenadasPre[i - 1].x, altura, coordenadasPre[i - 1].z - 1);
                break;
        }

        coordenadas.Add(new Vector3(coordenadasBuffer.x, coordenadasBuffer.y, coordenadasBuffer.z));
        coordenadasPre.Add(new Vector3(coordenadasBuffer.x, coordenadasBuffer.y, coordenadasBuffer.z));
        //checar bufferDireccion y direccionTemp2

        do
        {
            coordinadasValidar2 = true;
            //Corregir coordenadasBuffer para que sea Vector2
            bufferDireccion = Random.Range(0, 4);

            switch (bufferDireccion)
            {
                case 0:
                    coordenadasBuffer = new Vector3(coordenadasPre[i].x - 1, altura, coordenadasPre[i ].z);
                    break;
                case 1:
                    coordenadasBuffer = new Vector3(coordenadasPre[i ].x, altura, coordenadasPre[i ].z + 1);
                    break;
                case 2:
                    coordenadasBuffer = new Vector3(coordenadasPre[i ].x + 1, altura, coordenadasPre[i ].z);
                    break;
                case 3:
                    coordenadasBuffer = new Vector3(coordenadasPre[i ].x, altura, coordenadasPre[i ].z - 1);
                    break;
            }

            for (int j = 0; j < coordenadasPre.Count; j++)
            {
                if (coordenadasPre[j].x == coordenadasBuffer.x &&
                  coordenadasPre[j].y == coordenadasBuffer.y &&
                  coordenadasPre[j].z == coordenadasBuffer.z)
                {
                    coordinadasValidar2 = false;
                    break;
                }
            }

        } while (bufferDireccion == direccionTemp2 || !coordinadasValidar2);



        switch (cuartoActual)
        {
            case 2:
                cuartoParaAsignar = escenariosCargados.escenariosDisponibles[Random.Range(0, escenariosCargados.escenariosDisponibles.Count)];
                cuartoBuffer = new Escenario(cuartoParaAsignar.prefabEscenario, cuartoParaAsignar.tipoDeCuarto, cuartoParaAsignar.id);

                cuartoBuffer.tipoDeCuarto = 3;
                cuartoBuffer.colisiones[bufferDireccion] = 2;
                break;
            case 3:
                cuartoParaAsignar = intermediosCargados.intermediosDisponibles[Random.Range(0, intermediosCargados.intermediosDisponibles.Count)];
                cuartoBuffer = new Escenario(cuartoParaAsignar.prefabEscenario, cuartoParaAsignar.tipoDeCuarto, cuartoParaAsignar.id);

                cuartoBuffer.tipoDeCuarto = 2;
                cuartoBuffer.colisiones[bufferDireccion] = 3;
                break;
        }
        cuartoBuffer.colisiones[direccionTemp2] = mapa[i - 1].tipoDeCuarto;
        //Debug.Log(cuartoBuffer.tipoDeCuarto);
        for (int j = 0; j < 4; j++) { if ((j != bufferDireccion) && (j != direccionTemp2)) { cuartoBuffer.colisiones[j] = 1; } }
        mapa.Add(cuartoBuffer);


    }
}
