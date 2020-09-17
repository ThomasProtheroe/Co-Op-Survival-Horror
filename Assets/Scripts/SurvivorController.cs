using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class SurvivorController : MonoBehaviourPunCallbacks, IPunObservable
{

    #region Private Serializable Fields

    [SerializeField]
    private int maxHp;
    [SerializeField]
    private float maxStamina;
    [SerializeField]
    private float staminaRecoveryDelay;
    [SerializeField]
    private float staminaRecoveryRate;
    [SerializeField]
    private float jumpStaminaUsage;
    [SerializeField]
    private float sprintStaminaUsage;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float sprintMultiplier;
    [SerializeField]
    private float aimSpeedMultiplier;
    [SerializeField]
    private float fallMultiplier;
    [SerializeField]
    private float fallControlSpeed;
    [SerializeField]
    private float fallDamageThreshold;
    [SerializeField]
    private float baseFallDamage;
    [SerializeField]
    private float verticalJumpStrength;
    [SerializeField]
    private float horizontalJumpStrength;
    [SerializeField]
    private float landingStaggerTime;

    #endregion


    #region Private Serializable References

    [SerializeField]
    private GameObject healthBar;
    [SerializeField]
    private GameObject staminaBar;

    #endregion


    #region Private References

    private Rigidbody2D rigidBody;
    private CameraController cameraController;

    #endregion



    #region Private Fields

    //Player attributes
    private Vector2 velocity;
    private int currentHp;
    private float currentStamina;
    private float currentMoveSpeed;

    //Condition flags
    private bool canMove = true; //TODO - Create a PlayerStatus class to track status conditions such as stun, slowdown etc.
    private bool canAct = true;
    private bool grounded = true;
    private bool sprinting = false;
    private bool recoveringStamina;
    private bool facingRight = true;

    //Inputs
    private float inputMoveX;
    private float inputMoveY;
    private bool inputJump;
    private bool inputSprint;
    private bool inputDown;
    private bool inputAim;

    //Timers
    private float landingStaggerTimer;
    private float staminaRecoveryTimer;

    #endregion


    #region Gear



    #endregion


    #region Public Fields

    public static GameObject localPlayerInstance;

    #endregion


    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        //Set references
        rigidBody = GetComponent<Rigidbody2D> ();
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController> ();

        //Set starting stats
        currentHp = maxHp;
        currentStamina = maxStamina;
        currentMoveSpeed = moveSpeed;

        if (photonView.IsMine == true) {
            //Follow with camera
            cameraController.startFollowing(gameObject);

            //Link to health/stamina bars
            healthBar = GameObject.FindGameObjectWithTag("HealthBar");
            staminaBar = GameObject.FindGameObjectWithTag("StaminaBar");
        }
    }

    void Awake() {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine) {
            SurvivorController.localPlayerInstance = this.gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update() {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
            return;
        }

        //Player input and movement
        getPlayerInput();
        applyPlayerMovement();

        //Change the players velocity based on input
        if (canMove) {
            rigidBody.velocity = new Vector2 (velocity.x, rigidBody.velocity.y);    
        }
    }

    void FixedUpdate() {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
            return;
        }

        //Handle all timers
        if (landingStaggerTimer > 0f) {
            landingStaggerTimer -= Time.deltaTime;
            if (landingStaggerTimer <= 0f) {
                landingStaggerTimer = 0f;
                endLandingStagger();
            } 
        }

        if (staminaRecoveryTimer > 0f) {
            staminaRecoveryTimer -= Time.deltaTime;
            if (staminaRecoveryTimer <= 0f) {
                staminaRecoveryTimer = 0f;
                startStaminaRecovery();
            } 
        }
        
        //More responsive jumps
        if (rigidBody.velocity.y < 0) {
            rigidBody.gravityScale = fallMultiplier;
        } else {
            rigidBody.gravityScale = 1.4f;
        }

        //Stamina recovery
        if (recoveringStamina) {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            updateStaminaBar();
            if (currentStamina >= maxStamina) {
                currentStamina = maxStamina;
                recoveringStamina = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
            return;
        }

        //Detect when the player lands on a solid surface
        if (!grounded) {
            if ((other.gameObject.tag == "Terrain") || (other.gameObject.tag == "Platform" && rigidBody.velocity.y < 0.0f)) {
                grounded = true;
                onLanding();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
            return;
        }
        //In future we may need to check that no other terrain colliders are inside the ground check box before setting grounded=false
        //Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

        if (grounded) {
            if (other.gameObject.tag == "Terrain" || other.gameObject.tag == "Platform") {
                grounded = false;
            }
        }
    }

    #endregion


    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            // We own this player: send the others our data
            stream.SendNext(currentHp);
        } else {
            // Network player, receive data
            this.currentHp = (int)stream.ReceiveNext();
        }
    }

    #endregion


    private void getPlayerInput() {
        inputMoveX = Input.GetAxis("Horizontal");
        inputMoveY = Input.GetAxis("Vertical");
        inputJump = Input.GetButtonDown("Jump");
        inputDown = Input.GetButtonDown("Down");
        inputSprint = Input.GetButton("Sprint");
    }

    private void applyPlayerMovement() {
        sprinting = false;
        if (canMove) {
            //Horizontal movement
            if (grounded) {
                if (inputMoveX > 0.0f) {
                    velocity.x = currentMoveSpeed;
                } else if (inputMoveX < 0.0f) {
                    velocity.x = -1.0f * currentMoveSpeed;
                } else {
                    velocity.x = 0.0f;
                }
                //Sprinting
                if (velocity.x != 0.0f && inputSprint) {
                    if (currentStamina > 0.0f) {
                        sprinting = true;
                        velocity.x = velocity.x * sprintMultiplier;
                    }
                    reduceStamina(sprintStaminaUsage * Time.deltaTime);
                }
            } else {
                //Limit player control while airborn
                if (inputMoveX > 0.0f) {
                    velocity.x = rigidBody.velocity.x + fallControlSpeed * Time.deltaTime;
                } else if (inputMoveX < 0.0f) {
                    velocity.x = rigidBody.velocity.x - fallControlSpeed * Time.deltaTime;
                }
            }

            //Jumping
            if (grounded && inputJump && (currentStamina > 0.0f)) {
                grounded = false;
                //Upward jump velocity
                rigidBody.velocity = new Vector2 (rigidBody.velocity.x, verticalJumpStrength);

                //Horizontal jump velocity
                if (rigidBody.velocity.x != 0f) {
                    velocity.x = rigidBody.velocity.x * horizontalJumpStrength;
                }
                
                reduceStamina(jumpStaminaUsage);
            }

            //Drop through platforms
            if (inputMoveY < 0) {
                gameObject.layer = 9;
            } else {
                gameObject.layer = 8;
            }

            //Facing
            if (velocity.x > 0 && !facingRight) {
                Flip();
            } else if (velocity.x < 0 && facingRight) {
                Flip();
            }
        }
    }


    private void stopMovement() {
        rigidBody.velocity = new Vector2 (0.0f, 0.0f);
    }

    private void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    private void endLandingStagger() {
        canMove = true;
    }

    private void reduceStamina(float amount) {
        recoveringStamina = false;
        
        currentStamina -= amount;
        if (currentStamina < 0.0f) {
            currentStamina = 0.0f;
        }

        updateStaminaBar();
        startStaminaRecoveryTimer();
    }

    private void startStaminaRecovery() {
        recoveringStamina = true;
    }

    private void takeDamage(int damage) {
        currentHp -= damage;

        updateHealthBar();

        if (currentHp <= 0) {
            killSurvivor();
        }
    }

    private void killSurvivor() {
        Debug.Log("You're Dead.");
    }


    #region Timer Helpers

    private void startStaminaRecoveryTimer() {
        staminaRecoveryTimer = staminaRecoveryDelay;
    }

    #endregion


    #region GUI Update Helpers

    private void updateHealthBar() {
        healthBar.GetComponent<Image> ().fillAmount = (float)currentHp / (float)maxHp;
    }

    private void updateStaminaBar() {
        staminaBar.GetComponent<Image> ().fillAmount = currentStamina / maxStamina;
    }

    #endregion


    #region custom event handling

    private void onLanding() {
        if (System.Math.Abs(rigidBody.velocity.y) > fallDamageThreshold) {
            takeDamage((int)(baseFallDamage * (System.Math.Abs(rigidBody.velocity.y) - fallDamageThreshold)));
        }

        stopMovement();
        landingStaggerTimer = landingStaggerTime;
        canMove = false;
    }

    #endregion


    #region getters/setters

    //Getters/Setters
    public Vector2 getVelocity() {
        return rigidBody.velocity;
    }

    #endregion
}
