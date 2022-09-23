using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform hero;          // The player
    public Camera playerCamera;     // The camera
    public float cameraSpeed;       // The speed at which the camera moves
    public float floatBias;         /* Factor which determines how close the camera 
                                        floats to the player, vs the mouse.
                                        Use a value of zero to deactivate camera floating
                                    */

    void Update()
    {
        // Dont do anything if float is disabled
        if (floatBias <= 0) {
            return;
        }
        Vector2 destination = GetDestination();
        Vector2 goal = destination + (Vector2) hero.position;
        Vector2 pos = playerCamera.transform.position;
        Vector2 direction = (goal - pos);
        Vector2 movement = direction * cameraSpeed * Time.deltaTime;

        SetPosition(new Vector2(pos.x + movement.x, pos.y + movement.y));
    }

    // Set the camera position
    void SetPosition(Vector2 vec) {
        playerCamera.transform.position = new Vector3(vec.x, vec.y, playerCamera.transform.position.z);
    }

    // Get the position between the player and mouse that the camera will move towards
    Vector2 GetDestination()
    {
        Vector2 pos = hero.position;
        Vector2 mouse = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        return (mouse - pos) * floatBias;
    }
}
