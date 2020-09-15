using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float xMoveThreshold;
    [SerializeField]
    private float yMoveThreshold;
    [SerializeField]
	private float maxDistanceFromPlayer;
    [SerializeField]
    private float baseCameraSpeed;
    [SerializeField]
	private float backgroundSpeedDivisor;
    [SerializeField]
    private GameObject parallax;
    private float cameraSpeed;
	private float xDiff;
    private float yDiff;
	private GameObject player;
	private Vector3 targetPos;

	// Use this for initialization
	void Start () {
		
	}

	void FixedUpdate () {
		if (player == null) {
			return;
		}

		float backgroundSpeed = (cameraSpeed * Time.deltaTime) / backgroundSpeedDivisor;

		if (player.transform.position.x > gameObject.transform.position.x) {
			xDiff = player.transform.position.x - gameObject.transform.position.x;
			backgroundSpeed *= -1;
		} else {
			xDiff = gameObject.transform.position.x - player.transform.position.x;
		}

        if (player.transform.position.y > gameObject.transform.position.y) {
			yDiff = player.transform.position.y - gameObject.transform.position.y;
			backgroundSpeed *= -1;
		} else {
			yDiff = gameObject.transform.position.y - player.transform.position.y;
		}

		if (xDiff >= xMoveThreshold || yDiff >= yMoveThreshold) {
            //Determine traversal speed of the camera. Move faster the further away the player is.
            float playerSpeed = player.GetComponent<SurvivorController> ().getVelocity().magnitude;
            float speedMultiplier = Vector2.Distance(transform.position, player.transform.position) / maxDistanceFromPlayer;
            cameraSpeed = baseCameraSpeed * speedMultiplier;
            if (playerSpeed > cameraSpeed) {
                cameraSpeed = playerSpeed * speedMultiplier;
            }

            //Move towards the player
			targetPos = player.transform.position;
			targetPos.z = gameObject.transform.position.z;
			gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position, targetPos, cameraSpeed * Time.deltaTime);

			//Move parallax background
            if (parallax != null) {
                SpriteRenderer backgroundSprite = parallax.GetComponent<SpriteRenderer> ();
			    backgroundSprite.transform.localPosition = new Vector3 (backgroundSprite.transform.localPosition.x + backgroundSpeed, backgroundSprite.transform.localPosition.y, backgroundSprite.transform.localPosition.z);
            }
		}
	}

	public void startFollowing(GameObject targetPlayer) {
		player = targetPlayer;
		transform.position = new Vector3 (player.transform.position.x, transform.position.y, transform.position.z);
	}
}
