using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private MazeCell currentCell;

	private MazeDirection currentDirection;

	private CharacterController _controller;


	// movement
	public float speed = 2.0F;
	private Vector3 moveDirection = Vector3.zero;

	// camera
    public float mouseSensitivity = 2.0f;
    float xAxisClamp = 1.0f;

	void Start()
	{
         _controller = GetComponent<CharacterController>();
	}

	void Update()
	{
		MovePlayer();

		RotateCamera();
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

        Vector3 targetRotCam = Camera.main.transform.rotation.eulerAngles;
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


        Camera.main.transform.rotation = Quaternion.Euler(targetRotCam);
        transform.rotation = Quaternion.Euler(targetRotBody);
    }

	public void SetLocation(MazeCell cell)
	{
		currentCell = cell;
		Vector3 pos = cell.transform.localPosition;
		transform.localPosition = pos;
	}

}