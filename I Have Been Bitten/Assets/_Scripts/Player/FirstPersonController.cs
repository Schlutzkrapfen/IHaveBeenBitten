using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using UnityEngine.VFX;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif



namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class FirstPersonController : MonoBehaviour
	{
		[FormerlySerializedAs("MoveSpeed")]
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float moveSpeed = 4.0f;
		 [Tooltip("Sprint speed of the character in m/s")]
		public float sprintSpeed = 6.0f;
		 [Tooltip("Rotation speed of the character")]
		public float rotationSpeed = 1.0f;
	 [Tooltip("Acceleration and deceleration")]
		public float speedChangeRate = 10.0f;
		[Tooltip("The CrouchSpeed for the Player")]
		[SerializeField] private float crouchSpeed = 1.0f;

		[Header("Dash")]
		[Space(10)]
		[SerializeField] BoxCollider dashCollider;
		[Tooltip("how much the player dashes")]
                    		 [SerializeField]
                    		 private float dashSpeed = 8.0f;
          	[Tooltip("how much faster the Player Could Dash to the enemy")]
                              		 [SerializeField]
                              		 private float dashToEnemySpeedBonus = 2.0f;          
                 	 [Tooltip("how long the player dashes if it set lower than 0 it disables the dash")] 
                    		public float dashLenght = .5f;
			[Tooltip("For the animation when you dash to your target")]
     		[SerializeField]
     		private VisualEffect dashLines;
		[Tooltip("when the Dashes starts")]
		public float DashStart = 0.0f;
		 [Tooltip("how long it takes bevor the next dash is ready")]
		 [SerializeField]
		 private float dashReadyLength = 1.0f;
		 [Tooltip("How far away the player should be when dashing to an Enemy")]
        		 [SerializeField]
        		 private float dashOffset = 1.0f;	 
		         [Tooltip("How far the Player needs to be away before the Transformation takes affekt")]
				 [SerializeField]
				 private float byteDistance = .5f;
				 [SerializeField] private GameObject bloodExplosion;
		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float jumpHeight = 0.3f;
	 [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float gravity = -15.0f;

		[Tooltip("Collider for the dashdetection")]
		[SerializeField]
		private Trigger trigger;
	 
		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float jumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float fallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("How far the ground should be set true when you fly")]
		[SerializeField] private float range = 0.1f;
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool grounded = true;
	
		 [Tooltip("What layers the character uses as ground")]
		public LayerMask groundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject cinemachineCameraTarget;
		 [Tooltip("How far in degrees can you move the camera up")]
		public float topClamp = 89.0f;
		 [Tooltip("How far in degrees can you move the camera down")]
		public float bottomClamp = -89.0f;
			[Tooltip("How strong the Camera Shake when you Dash")]
        		public float shakeIntensityForDash = 100;
        		[Tooltip("How long the Camera Shake when you Dash")]
        		public float shakeDurationForDash = 1f;
			[Header("Sound")] 
			[SerializeField] private AudioSource biteEmtySound;
			[SerializeField] private AudioSource biteHitSound;
			 [SerializeField] private AudioSource dashSound;
			[SerializeField] private AudioSource footSteps;
		[Tooltip("how long the animation goes unitl its finished (for sound)")]
        		public float waitForAnimtionToBeRightTime = 1f;

		[Tooltip("what the minmal Pitch is that can randomly be generated")]
		[SerializeField]private float minPitch = 0.9f;
		[Tooltip("what the maximal Pitch is that can randomly be generated")]
		[SerializeField]private float maxPitch = 1.1f;

		// cinemachine
		private float cinemachineTargetPitch;
		private RaycastHit hit;
		private Transform rayStart;

		// player
		
		private float speed;
		private Vector3 crouchScaleVector3 = new Vector3(1f, 0.5f, 1f);
		private Vector3 normalCrouchScaleVector3 = new Vector3(1f, 1f, 1f);
		private float rotationVelocity;
		private float verticalVelocity;
		private float terminalVelocity = 53.0f;
		private Vector3 dashPosition;
		private bool dashStart;
		public bool dashReady = true;
		private bool soundAlreadyPlayed = false;
		private int humenLayer = 6;
		
		

		// timeout deltatime
		private float jumpTimeoutDelta;
		private float fallTimeoutDelta;
		private float dashTimeoutDelta;

	
#if ENABLE_INPUT_SYSTEM
		private PlayerInput playerInput;
		
#endif
		private bool isSomethingCollider;
		private new Animation animation;
		private CharacterController controller;
		private StarterAssetsInputs input;
		private GameObject mainCamera;
		private Interactor interactor;

		private const float threshold = 0.01f;

		private bool IsCurrentDeviceMouse
		{
			get
			{
				#if ENABLE_INPUT_SYSTEM
				return playerInput.currentControlScheme == "KeyboardMouse";
				#else
				return false;
				#endif
			}
		}

		private void Awake()
		{

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			// get a reference to our main camera
			if (mainCamera == null)
			{
				mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			rayStart = transform;
			dashLines.Stop();
			
			controller = GetComponent<CharacterController>();
			input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
			playerInput = GetComponent<PlayerInput>();
			animation = GetComponent<Animation>();
			interactor = GetComponent<Interactor>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

			// reset our timeouts on start
			jumpTimeoutDelta = jumpTimeout;
			fallTimeoutDelta = fallTimeout;
		}

		private void Update()
		{
			if (GameManager.Instance.Pause) return;
			JumpAndGravity();
			GroundedCheck();
			Crouch();
			Dash();
			Move();
		}

		private void LateUpdate()
		{
			if (dashReady)
			{
				CameraRotation();
			}
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			grounded = (Physics.Raycast(rayStart.position, rayStart.transform.up * -1, out hit, range, groundLayers));
		}

		private void CameraRotation()
		{
			// if there is an input
			if (input.look.sqrMagnitude >= threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				cinemachineTargetPitch += input.look.y * rotationSpeed * deltaTimeMultiplier;
				rotationVelocity = input.look.x * rotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

				// Update Cinemachine camera target pitch
				cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * rotationVelocity);
			}
		}

		private void Crouch()
		{
			if (input.crouch && transform.localScale ==normalCrouchScaleVector3)
			{
				transform.DOScale(crouchScaleVector3, 0.1f);

			}
			else if(transform.localScale == crouchScaleVector3 && !input.crouch)
			{
				transform.DOScale(normalCrouchScaleVector3, 0.2f);
			}
		}
		

		
		private void Move()
		{
			if (dashStart)
			{
				return;
			}
		
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed;
			if  (input.crouch)
			{
				targetSpeed =crouchSpeed;
			}
			else if(input.sprint)
			{
				targetSpeed = sprintSpeed;
			}
			else
			{
				targetSpeed = moveSpeed;
			}

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (input.move == Vector2.zero)
			{
				targetSpeed = 0.0f;
			}

			// a reference to the players current horizontal velocity
			var velocity = controller.velocity;
			float currentHorizontalSpeed = new Vector3(velocity.x, 0.0f, velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);

				// round speed to 3 decimal places
				speed = Mathf.Round(speed * 1000f) / 1000f;
			}
			else
			{
				speed = targetSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (input.move != Vector2.zero)
			{
				// move
				inputDirection = transform.right * input.move.x + transform.forward * input.move.y;
			}
			if (dashTimeoutDelta >= 0 )
			{
				dashTimeoutDelta -= Time.deltaTime;
				float time = dashTimeoutDelta / dashLenght;
				
				inputDirection = dashPosition;
			

		
				if (trigger.collisions.Count == 0 )
				{
					//interactor.Interacte();
					controller.Move(inputDirection * (dashSpeed * EaseOutQuint(time) * Time.deltaTime )+ new Vector3(0.0f,verticalVelocity,0.0f)*Time.deltaTime);
				}
				else
				{
					DashToEnemy(time);
				}
				return;
			}
			else
			{ 
				for (int i = 0; i < trigger.collisions.Count; i++)
				{
					trigger.collisions.Remove(trigger.collisions[i]);
				}
				for (int i = 0; i < trigger.Interactors.Count; i++)
				{
					trigger.Interactors.Remove(trigger.Interactors[i]);
				}
				dashCollider.enabled = false;

            }

			soundAlreadyPlayed = false;

			if (targetSpeed == 0)
			{
				animation.ChangeWalkAnimaiton(false);
			}
			else
			{
				animation.ChangeWalkAnimaiton(true);
			}
				
			dashLines.Stop();
			controller.Move(inputDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

			// move the player
		}
		void DashToEnemy(float time)
		{
			Interactable interactors = null;
			if (trigger.collisions.Count != 1)
			{
				int listPosition = CalculateNearerEnemy(trigger.collisions);

				dashPosition = trigger.collisions[listPosition].gameObject.transform.position;
				interactors = trigger.Interactors[listPosition];
			}
			else
			{
				dashPosition = trigger.collisions[0].gameObject.transform.position;
				interactors = trigger.Interactors[0];
			}

			dashPosition =	CalculateCollsioionPosition(this.transform.position, dashPosition);
			if (byteDistance > Vector3.Distance(transform.position, dashPosition))
			{  
			
				interactor.Interacte(interactors);
				Vector3 bloodParticelPositon = dashPosition;
				if (!soundAlreadyPlayed)
				{
						biteHitSound.pitch = Random.Range(minPitch, maxPitch);
						biteHitSound.Play();
						bloodParticelPositon.y = mainCamera.transform.position.y;
						Quaternion spawnRotation = Quaternion.identity;
						Instantiate(bloodExplosion, bloodParticelPositon,spawnRotation );
				}
			
				soundAlreadyPlayed = true;
			}
			controller.Move((dashPosition-transform.position ).normalized* (dashSpeed * dashToEnemySpeedBonus * EaseOutQuint(time) * Time.deltaTime));
		}
		/// <summary>
		/// Calaculates the exact position where the player needs to go to
		/// the Enemy that it's not too far or to near it. The distance can be Changed with the
		/// offset
		/// </summary>
		/// <param name="playPosition"></param>
		/// <param name="hitPosition"></param>
		/// <returns></returns>
		Vector3  CalculateCollsioionPosition(Vector3 playPosition, Vector3 hitPosition)
		{
			Vector3 answer = new Vector3();

        
			Vector2 playerPositonVector2 = new Vector2(playPosition.x, playPosition.z);
			Vector2 hitPositionVector2 = new Vector2(hitPosition.x, hitPosition.z);
			Vector2 answerVector2 = new Vector2();
			answerVector2 = (playerPositonVector2 - hitPositionVector2).normalized * dashOffset + hitPositionVector2;
			answer = new Vector3(answerVector2.x, playPosition.y, answerVector2.y);
			return answer;
		}

		 int CalculateNearerEnemy(List<Transform> pointsList)
		 {
			Vector3 nearespoint = pointsList[0].transform.position;
			Vector3 playerPositon = transform.position;
			float shortestDistance = Vector3.Distance(playerPositon, nearespoint);
			int listPositon = 0;
			for (int i = 0; i < pointsList.Count; i++)
			{
				float distance = Vector3.Distance(playerPositon, pointsList[i].transform.position);
				if (distance < shortestDistance)
				{
					shortestDistance = distance;
					listPositon = i;
				}
			}
			return listPositon;
		 }
		private float  EaseOutQuint(float time)
		{
			return 1 - Mathf.Pow(1 - time,5);
			
		}

		private void JumpAndGravity()
		{
			if (grounded)
			{
				// reset the fall timeout timer
				fallTimeoutDelta = fallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (verticalVelocity < 0.0f)
				{
					verticalVelocity = -2f;
				}

				// Jump
				if (input.jump && jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
				}

				// jump timeout
				if (jumpTimeoutDelta >= 0.0f)
				{
					jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				jumpTimeoutDelta = jumpTimeout;

				// fall timeout
				if (fallTimeoutDelta >= 0.0f)
				{
					fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (verticalVelocity < terminalVelocity)
			{
				verticalVelocity += gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}


		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			var position = transform.position;
			//Gizmos.DrawSphere(new Vector3(position.x, position.y - groundedOffset, position.z), groundedRadius);
		}	private void Dash()
         		{
         
         			if (input.eat && !dashStart && dashReady )
		            {
						dashCollider.enabled = true;
			            dashReady = false;
			            dashStart = true;
			            dashSound.pitch = Random.Range(minPitch, maxPitch);
			            dashSound.Play();
			            StartCoroutine(WaitforDashToStart(DashStart));
         				StartCoroutine(WaitForAnimationToBeRightTime(waitForAnimtionToBeRightTime));
			            StartCoroutine(WaitForNextDashToBeReady(dashReadyLength));
		            }

         		}
         
         		IEnumerator WaitForAnimationToBeRightTime(float waitTimes)
	            {
		            yield return new WaitForSeconds(waitTimes);
		            biteEmtySound.pitch = Random.Range(minPitch, maxPitch);
         			biteEmtySound.Play();

		            CameraShake.Instance.ShakeCamera(shakeIntensityForDash,shakeDurationForDash);
			        
         		}

	            IEnumerator WaitforTransition(float waitTime)
	            {
		            yield return new WaitForSeconds(waitTime);
		            
					animation.ChangeAttackAnimation(true);
	            }

	            IEnumerator WaitforDashToStart(float waitTimes)
	            {
		            yield return new WaitForSeconds(waitTimes);
		          
		            	
				if (animation.attackanimationIsplaying)
				{
					animation.ResetAttackAnimation();
					StartCoroutine(WaitforTransition(0.1f));
				}
				animation.ChangeAttackAnimation(true);

		            dashLines.Play();
			        dashStart = false;
         			dashTimeoutDelta = dashLenght;
			        dashPosition = mainCamera.transform.forward;
	            }

	            IEnumerator WaitForNextDashToBeReady(float waitTimes)
	            {
		            yield return new WaitForSeconds(waitTimes);
		            dashReady = true;	
		           
		            
		            Debug.Log(trigger.collisions.Count);
	            }

	            public void FootSteps()
	            {
		            if (grounded)
		            {
			            footSteps.pitch = Random.Range(minPitch, maxPitch);
			            footSteps.Play();

		            }
	            }
	          
	}
}
