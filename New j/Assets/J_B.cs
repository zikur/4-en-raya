using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_B : MonoBehaviour
{

    private GameObject[,] grid;
    private int height = 10;
    private int width = 10;
    private bool primerturno;
    public Color jugador1;
    public Color jugador2;
    bool ganador;

	private int specialRoundCounter = 0;
	private readonly int specialRound = 4; //La propiedad readonly significa que esta variable no se puede modificar en el codigo.

    public Color colorfondo;

    void Start()
    {
        grid = new GameObject[width, height];                                               // posicion en altura y ancho
        for (int i = 0; i < width; i++)                                                     //poner esferas a lo ancho
        {

            for (int j = 0; j < height; j++)                                                //ponera esferas a lo alto
            {

                var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);                 
                go.transform.position = new Vector3(i, j, 0);                               //la esfera almacenada se ubica en una pocicion en el vector 3
                grid[i, j] = go;                                                            //coordenadas de los primitives
                go.GetComponent<Renderer>().material.color = colorfondo;                    //se crea un material de tipo color
                grid[i, j] = go;                                                            
            }
            
        }
    }

    void Update()
    {
        Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);           //posicion de la camara

        if (Input.GetKey(KeyCode.Mouse0) && ganador == false)
        {
            UpdatePickedPiece(mPosition);
        }
    }


    void UpdatePickedPiece(Vector3 position)
    {
        int i = (int)(position.x + 0.5f);                                                   //variable i se ubica en una pocicion x
        int j = (int)(position.y + 0.5f);                                                   //variable j se ubica en una pocicion y

		Debug.Log(i + ", " + j);

        if (Input.GetButtonDown("Fire1"))
        {
            if (i >= 0 && j >= 0 && i < width && j < height)                                //variable i y variable j se ubican en anchor y altura.
            {
                GameObject go = grid[i, j];                                                 //el objeto se pone en el espacio i y en el j
                if (go.GetComponent<Renderer>().material.color == colorfondo)               //color del fondo (color que se les da a las esferas)
                {
                    Color colorAusar = Color.clear;
                    if (primerturno)
                        colorAusar = jugador2;                                              //colores para definir jugadores

                    else
                        colorAusar = jugador1;

                    go.GetComponent<Renderer>().material.color = colorAusar;                //turnos de los jugadores
                    primerturno = !primerturno;
					CheckSpecialRound();
                    VerificadorverticalA(i, j, colorAusar);                                 // verificadores de puntaje
                    VerificadorverticalB(i, j, colorAusar);
                    DiagonalA(i, j, colorAusar);
                    DiagonalB(i, j, colorAusar);
                }
            }
        }
    }

	/// <summary>
	/// Agrega 1 al contador de rondas especiales y revisa si este es mayor o igual a la ronda especial
	/// </summary>
	private void CheckSpecialRound() {
		specialRoundCounter++;
		if (specialRoundCounter >= specialRound) 
        {
			RandomPieceChanger();
			specialRoundCounter = 0; 
		}
	}

	/// <summary>
	/// Elige una bola aleatoriamente y le asigna un color aleatorio. En caso de ser una bola que ya fue pintada, le cambia el color.
	/// </summary>
	private void RandomPieceChanger() {
		int rx = Random.Range(0, width); //Random.Range es una funcion de unity engine. Crea un numero aleatorio entre el menor (0) y el mayor (width), sin incluir el valor mayor (en el caso de que ambos sean integers)
		int ry = Random.Range(0, height);

		GameObject selectedGO = grid[rx, ry];
		Color colorToUse = Color.clear;
		Color materialColor = selectedGO.GetComponent<Renderer>().material.color; //Hago esto para evitar hacer getcomponent tantas veces seguidas ya que es una funcion relativamente pesada y puede afectar el rendimiento

		if (materialColor == colorfondo)      
		{
			int r = Random.Range(0, 2); //Elige entre 0 y 1, ya que el 2 es excluido

			if (r == 0) {
				colorToUse = jugador1;
			} else {
				colorToUse = jugador2;
			}
		} else if (materialColor == jugador1) {
			colorToUse = jugador2; //Si la bola aleatoriamente elegida ya fue coloreada, la coloreamos del color del otro jugador
		} else if (materialColor == jugador2) {
			colorToUse = jugador1;
		}

		selectedGO.GetComponent<Renderer>().material.color = colorToUse;

		//revisar si algún jugador ganó cuando sucede lo random
		VerificadorverticalA(rx, ry, colorToUse);
		VerificadorverticalB(rx, ry, colorToUse);
		DiagonalA(rx, ry, colorToUse);
		DiagonalB(rx, ry, colorToUse);
	}


    public void VerificadorverticalA(int x, int y, Color colorVerificar)
    {

        int contador = 0;

        for (int i = x - 3; i <= x + 3; i++)
        {
            if (i < 0 || i >= width)
                continue;

            GameObject go = grid[i, y];

            if (go.GetComponent<Renderer>().material.color == colorVerificar)
            {
                contador++;
                if (contador == 4 && colorVerificar == jugador1)
                {
                    Debug.Log("Eljugador 1 gana");
                    ganador = true;
                }

                else if (contador == 4 && colorVerificar == jugador2)
                {

                    Debug.Log("Eljugador 2 gana");
                    ganador = true;
                }
            }
            else
                contador = 0;
        }
    }

    public void VerificadorverticalB(int x, int y, Color colorVerificar)
    {
        int contador = 0;

        for (int j = y - 3; j <= y + 3; j++)
        {
            if (j < 0 || j >= height)
                continue;

            GameObject go = grid[x, j];

            if (go.GetComponent<Renderer>().material.color == colorVerificar)
            {
                contador++;
                if (contador == 4 && colorVerificar == jugador1)
                {
                    Debug.Log("Eljugador 1 gana");
                    ganador = true;
                }

                else if (contador == 4 && colorVerificar == jugador2)
                {

                    Debug.Log("Eljugador 2 gana");
                    ganador = true;
                }
            }
            else
                contador = 0;
        }
    }


    public void DiagonalA(int x, int y, Color colorVerificar)
    {

        int contador = 0;
        int j = y - 4;

        for (int i = x - 3; i <= x + 3; i++)
        {
            j++;
            if (j < 0 || j >= height || i < 0 || i >= width)
                continue;

            GameObject go = grid[i, j];

            // Debug.Log(go.GetComponent<Renderer>().material.color);
            // Debug.Log(colorAVerificando);
            if (go.GetComponent<Renderer>().material.color == colorVerificar)
            {
                contador++;

                if (contador == 4 && colorVerificar == jugador1)
                {
                    Debug.Log("Eljugador 1 gana");
                    ganador = true;
                }

                else if (contador == 4 && colorVerificar == jugador2)
                {

                    Debug.Log("Eljugador 2 gana");
                    ganador = true;
                }

            }
            else
                contador = 0;
        }
    }

    public void DiagonalB(int x, int y, Color colorVerificar)
    {

        int contador = 0;
        int j = y + 4;

        for (int i = x - 3; i <= x + 3; i++)
        {
            j--;

            if (j < 0 || j >= height || i < 0 || i >= width)
                continue;

            GameObject go = grid[i, j];

            if (go.GetComponent<Renderer>().material.color == colorVerificar)
            {
                contador++;

                if (contador == 4 && colorVerificar == jugador1)
                {
                    Debug.Log("Eljugador 1 gana");
                    ganador = true;
                }

                else if (contador == 4 && colorVerificar == jugador2)
                {

                    Debug.Log("Eljugador 2 gana");
                    ganador = true;
                }
            }
            else
                contador = 0;
        }
    }

}
