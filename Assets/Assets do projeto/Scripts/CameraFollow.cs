using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//esse script ja veio pronto com o projeto do tutorial, o cara pegou da net e adaptou, mas é bem facil de entender e da pra aproveitar
// em outros projetos, porem num projeto mais elaborado melhor usar cinemachine mesmo
public class CameraFollow : MonoBehaviour {

	public float xMargin = 0f; // Distance in the x axis the player can move before the camera follows.
	//public float yMargin = 1f; // Distance in the y axis the player can move before the camera follows. desativado neste projeto, camera nao vai subir
	public float xSmooth = 8f; // How smoothly the camera catches up with it's target movement in the x axis.
	public float ySmooth = 8f; // How smoothly the camera catches up with it's target movement in the y axis.
	public Vector2 maxXAndY; // The maximum x and y coordinates the camera can have.
	public Vector2 minXAndY; // The minimum x and y coordinates the camera can have.

	private Transform m_Player; // Reference to the player's transform.


	private void Awake()
	{
		// Setting up the reference.
		m_Player = GameObject.FindGameObjectWithTag("Player").transform;
	}


	private bool CheckXMargin()
	{
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return (transform.position.x - m_Player.position.x) < xMargin;
        //operador estranho, explicação: o valor em parenteses retorna positivo ( i.e.: 1) pq a camera vai estar a frente do personagem
        //se retornar negativo ( i.e.: -1) então o player passou da camera, sendo negativo e menor que o xmargin, ja que este é positivo
        //ai vai retornar true =)

    }


    //private bool CheckYMargin()
    //{
    // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
    //return Mathf.Abs(transform.position.y - m_Player.position.y) > yMargin;
    //}
    //tambem desativado, sem necessidade nesse projeto, a camera não sobe, nem quando pula


    private void Update()
	{
		TrackPlayer();
	}


	private void TrackPlayer()
	{
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = transform.position.x;
		float targetY = transform.position.y;

		// If the player has moved beyond the x margin...
		if (CheckXMargin())
		{
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			targetX = Mathf.Lerp(transform.position.x, m_Player.position.x, xSmooth * Time.deltaTime); // a lá, deltatime no lerp, vai vendo kkk
		}

		// If the player has moved beyond the y margin...
		//if (CheckYMargin())
		//{
			// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
			//targetY = Mathf.Lerp(transform.position.y, m_Player.position.y, ySmooth * Time.deltaTime);
		//}

		// The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
		targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
		//targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(targetX, transform.position.y, transform.position.z); //y vai usar o valor do transform, ja que nao vai ser alterado
	}
}

