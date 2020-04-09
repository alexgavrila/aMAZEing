using UnityEngine;

public class Player : MonoBehaviour
{
	private MazeCell currentCell;

	private MazeDirection currentDirection;

	private CharacterController _controller;

	private PlayerRifle rifle;

	private Camera inGameCamera;

	public int PickedCoins { private set; get; } = 0;
	public int EnemiesKilled { private set; get; } = 0;

	// movement
	public float speed = 2.0F;
	private Vector3 moveDirection = Vector3.zero;

	// camera
    public float mouseSensitivity = 2.0f;
    float xAxisClamp = 1.0f;
    
    #region Constants
		public const string CoinName = "Coin";
    #endregion

	void Start()
	{
         _controller = GetComponent<CharacterController>();
         rifle = GetComponentInChildren<PlayerRifle>();

         inGameCamera = GetComponentInChildren<Camera>();
	}

	void Update()
	{
		// When the game is being paused, do not update the player
		if (GameMenuUI.IsGamePaused)
		{
			return;
		}

		MovePlayer();

		RotateCamera();

		if (Input.GetButton("Fire1"))
		{
			rifle.Shoot();
		}
	}

	void MovePlayer()
	{
		moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection *= speed;
		_controller.Move(moveDirection * Time.deltaTime);
	}

	void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotAmountX = mouseX * mouseSensitivity;
        float rotAmountY = mouseY * mouseSensitivity;

        xAxisClamp -= rotAmountY;

        Vector3 targetRotCam = inGameCamera.transform.rotation.eulerAngles;
        Vector3 targetRotBody = transform.rotation.eulerAngles;

        targetRotCam.x -= rotAmountY;
        targetRotCam.z = 0;
        targetRotBody.y += rotAmountX;

        if(xAxisClamp > 90)
        {
            xAxisClamp = 90;
            targetRotCam.x = 90;
        }
        else if(xAxisClamp < -90)
        {
            xAxisClamp = -90;
            targetRotCam.x = 270;
        }


        inGameCamera.transform.rotation = Quaternion.Euler(targetRotCam);
        transform.rotation = Quaternion.Euler(targetRotBody);
    }

	public void SetLocation(MazeCell cell)
	{
		currentCell = cell;
		Vector3 pos = cell.transform.localPosition;
		pos.y = transform.position.y;
		
		transform.localPosition = pos;
	}

	public void OnPickUp(PickUp pickUpObj)
	{
		if (pickUpObj.pickUpName == CoinName)
		{
			PickedCoins += 1;
		}
	}

	public void OnEnemyKilled()
	{
		EnemiesKilled++;
	}

	public void OnDeath()
	{
		print("Player died");
	}

	public MazeCell GetCellBelow()
	{
		var downDir = -transform.up;

		RaycastHit hit;
		if (Physics.Raycast(transform.position, downDir, out hit))
		{
			if (hit.collider.transform.parent == null)
			{
				return null;
			}

			// The collider is the quad object. Get its mazeCell parent
			GameObject mazeCell = hit.collider.transform.parent.gameObject;
			
			if (mazeCell.CompareTag("MazeCell"))
			{
				currentCell = mazeCell.GetComponent<MazeCell>();
				
				return currentCell;
			}
		}

		return null;
	}

}