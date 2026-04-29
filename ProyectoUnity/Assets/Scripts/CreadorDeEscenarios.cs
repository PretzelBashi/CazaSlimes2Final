using System.Collections.Generic;
using UnityEngine;

//public class CreadorDeEscenarios : MonoBehaviour
public class EscenariosDisponibles
{
    public List<Escenario> escenariosDisponibles;
    public EscenariosDisponibles(List<GameObject> escenarios)
    {
        escenariosDisponibles = new List<Escenario>();
        for (int i = 0; i < escenarios.Count; i++)
        {
            Debug.Log(escenarios[0]);
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

    EscenariosDisponibles escenariosCargados;
    IntermediosDisponibles intermediosCargados;

    List<Escenario> mapa;
    List<Vector2> coordenadas;

    void Start()
    {
        escenariosCargados = new EscenariosDisponibles(escenariosACargar);
        intermediosCargados = new IntermediosDisponibles(intermediosACargar);

        mapa = new List<Escenario>();
        CargarMapa(6);
    }



    // Update is called once per frame
    void Update()
    {

    }



    public void CargarMapa(int cantidadDeCuartos)
    {
        Escenario cuartoParaAsignar1 = escenariosCargados.escenariosDisponibles[Random.Range(0, escenariosCargados.escenariosDisponibles.Count)];
        Escenario cuartoBuffer = new Escenario(cuartoParaAsignar1.prefabEscenario, cuartoParaAsignar1.tipoDeCuarto, cuartoParaAsignar1.id);
        coordenadas = new List<Vector2>();
        int bufferDireccion = Random.Range(0, 4);
        Vector2 coordenadasBuffer = new Vector2(0,0);


        cuartoBuffer.tipoDeCuarto = 3;
        cuartoBuffer.colisiones[bufferDireccion] = 2;
        coordenadas.Add(new Vector2(0,0));
        for (int i = 0; i < 4; i++) { if (i != bufferDireccion) { cuartoBuffer.colisiones[i] = 1; } }



        mapa.Add(cuartoBuffer);

        //Debug.Log(bufferDireccion);

        for (int i = 0; i < cantidadDeCuartos+1; i++) //Creacion del mapa entre intermedios y cuevas (probablemente)
        {
            if (i != 0)
            {
                switch (mapa[i - 1].tipoDeCuarto)
                {
                    case 0: break;
                    case 1: break;
                    case 2:
                        int direccionTemp2 = 0;
                        bool coordinadasValidar2 = true;
                        switch (bufferDireccion) { case 0: direccionTemp2 = 2; break; case 1: direccionTemp2 = 3; break; case 2: direccionTemp2 = 0; break; case 3: direccionTemp2 = 1; break; }


                        do {
                            //Corregir coordenadasBuffer para que sea Vector2
                            bufferDireccion = Random.Range(0, 4);
                            switch (bufferDireccion)
                            {
                                case 0:
                                    coordenadasBuffer = new int[] { coordenadas[i - 1][0] - 1, coordenadas[i - 1][1] };
                                    break;
                                case 1:
                                    coordenadasBuffer = new int[] { coordenadas[i - 1][0], coordenadas[i - 1][1] + 1 };
                                    break;
                                case 2:
                                    coordenadasBuffer = new int[] { coordenadas[i - 1][0] + 1, coordenadas[i - 1][1] };
                                    break;
                                case 3:
                                    coordenadasBuffer = new int[] { coordenadas[i - 1][0], coordenadas[i - 1][1] - 1 };
                                    break;
                            }
                            for (int j = 0; j < coordenadas.Count; j++)
                            {
                                if (coordenadas[j][0] == coordenadasBuffer[0] &&
                                    coordenadas[j][1] == coordenadasBuffer[1])
                                {
                                    coordinadasValidar2 = false;
                                    break;
                                }
                            }

                        } while (bufferDireccion == direccionTemp2 && coordinadasValidar2);

                        coordenadas.Add(new int[]{coordenadasBuffer[0], coordenadasBuffer[1]});

                        cuartoParaAsignar1 = escenariosCargados.escenariosDisponibles[Random.Range(0, escenariosCargados.escenariosDisponibles.Count)];
                        cuartoBuffer = new Escenario(cuartoParaAsignar1.prefabEscenario, cuartoParaAsignar1.tipoDeCuarto, cuartoParaAsignar1.id);


                        //checar bufferDireccion y direccionTemp2
                        cuartoBuffer.tipoDeCuarto = 3;
                        cuartoBuffer.colisiones[direccionTemp2] = mapa[i - 1].tipoDeCuarto;
                        cuartoBuffer.colisiones[bufferDireccion] = 2;
                        for (int j = 0; j < 4; j++) { if ((j != bufferDireccion) && (j != direccionTemp2)) { cuartoBuffer.colisiones[j] = 1; } }
                        mapa.Add(cuartoBuffer);
                        break;
                    case 3:
                        int direccionTemp3 = 0;
                        bool coordinadasValidar3 = true;
                        switch (bufferDireccion) { case 0: direccionTemp3 = 2; break; case 1: direccionTemp3 = 3; break; case 2: direccionTemp3 = 0; break; case 3: direccionTemp3 = 1; break; }


                        do
                        {

                            bufferDireccion = Random.Range(0, 4);
                            switch (bufferDireccion)
                            {
                                case 0:
                                    coordenadasBuffer = new int[] { coordenadas[i - 1][0] - 1, coordenadas[i - 1][1] };
                                    break;
                                case 1:
                                    coordenadasBuffer = new int[] { coordenadas[i - 1][0], coordenadas[i - 1][1] + 1 };
                                    break;
                                case 2:
                                    coordenadasBuffer = new int[] { coordenadas[i - 1][0] + 1, coordenadas[i - 1][1] };
                                    break;
                                case 3:
                                    coordenadasBuffer = new int[] { coordenadas[i - 1][0], coordenadas[i - 1][1] - 1 };
                                    break;
                            }
                            for (int j = 0; j < coordenadas.Count; j++)
                            {
                                if (coordenadas[j][0] == coordenadasBuffer[0] &&
                                    coordenadas[j][1] == coordenadasBuffer[1])
                                {
                                    coordinadasValidar3 = false;
                                    break;
                                }
                            }

                        } while (bufferDireccion == direccionTemp3 && coordinadasValidar3);

                        coordenadas.Add(new int[] { coordenadasBuffer[0], coordenadasBuffer[1] });


                        cuartoParaAsignar1 = intermediosCargados.intermediosDisponibles[Random.Range(0, intermediosCargados.intermediosDisponibles.Count)];
                        cuartoBuffer = new Escenario(cuartoParaAsignar1.prefabEscenario, cuartoParaAsignar1.tipoDeCuarto, cuartoParaAsignar1.id);


                        cuartoBuffer.tipoDeCuarto = 2;
                        cuartoBuffer.colisiones[direccionTemp3] = mapa[i - 1].tipoDeCuarto;
                        cuartoBuffer.colisiones[bufferDireccion] = 3;
                        for (int j = 0; j < 4; j++) { if ((j != bufferDireccion) && (j != direccionTemp3)) { cuartoBuffer.colisiones[j] = 1; } }
                        mapa.Add(cuartoBuffer);
                        break;
                }


                

            }
            Debug.Log(bufferDireccion);
            Debug.Log("Coordenadas" + coordenadasBuffer[0] + " " + coordenadasBuffer[1]);
            //Debug.Log($"{cuartoBuffer.colisiones[0]}, {cuartoBuffer.colisiones[1]}, {cuartoBuffer.colisiones[2]}, {cuartoBuffer.colisiones[3]}");
        }
        //---------- :3 
        //Cuevas de 2.5 x 2.5
        //puente 2 x 2
        Vector3 posicionBuffer = new Vector3();
        int direccionBuffer = 0;


        Debug.Log("colisionesPOSTPROCESADO");
        //for (int i = 0; i < mapa.Count; i++) { Debug.Log($"{mapa[i].colisiones[0]}, {mapa[i].colisiones[1]}, {mapa[i].colisiones[2]}, {mapa[i].colisiones[3]}"); }

        for (int i = 0; i < mapa.Count; i++)
        {
            
            if (i == 0)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (mapa[i].colisiones[j] == 3 || mapa[i].colisiones[j] == 2)
                    {
                        direccionBuffer = j;
                        break;
                    }
                }
            } else
            {
                switch (direccionBuffer)
                {
                    case 0: posicionBuffer.x -= 41; direccionBuffer = 2; break;
                    case 1: posicionBuffer.z += 41; direccionBuffer = 3; break;
                    case 2: posicionBuffer.x += 41; direccionBuffer = 0; break;
                    case 3: posicionBuffer.z -= 41; direccionBuffer = 1; break;
                }
                mapa[i].colisiones[direccionBuffer] = 4;

                for (int j = 0; j < 4; j++)
                {
                    if (mapa[i].colisiones[j] == 3 || mapa[i].colisiones[j] == 2)
                    {
                        direccionBuffer = j;
                        break;
                    }
                }
            }
            Debug.Log(direccionBuffer);
            posicionBuffer = new Vector3(posicionBuffer.x, posicionBuffer.y, posicionBuffer.z); // Agregar que la posicion aumente dependiendo en la direccion en la que estamos creando esto
            Instantiate(mapa[i].prefabEscenario, posicionBuffer, Quaternion.identity);
        }
    }
}
