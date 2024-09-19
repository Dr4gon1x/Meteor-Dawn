using UnityEngine;
using UnityEngine.UIElements;

public class LightMovement : MonoBehaviour
{

    // -----------------------------------------------------------------------------------------------------------------

    public float angleTurnSharpness = 45f; // The amount of degrees the angle can increase/decrease within a second 
    public float startSpeed = 15f;

    float rotationX;
    float rotationZ;

    float angle;
    float angleTarget;
    int angleIncreaseMultiplier;

    float speed;
    float time = 0f;
    float cooldown;

    // -----------------------------------------------------------------------------------------------------------------

    void Start()
    {
        speed = startSpeed;

        // Gives current angle a random start value between 0 and 360, and sets target angle equeal to current angle, as well as random cooldown
        angle = UnityEngine.Random.Range(0, 360);
        angleTarget = angle;
        cooldown = UnityEngine.Random.Range(0, 5);
    }

    // -----------------------------------------------------------------------------------------------------------------

    void Update()
    {

        if (angle == angleTarget) // If the angle is equal to the target angle, the script will wait for the cooldown and then give a new target angle
        {
            time += Time.deltaTime;

            if (time >= cooldown)
            {
                angleTarget = UnityEngine.Random.Range(0, 360); // Gives target angle a random value between 0 and 360

                ShortestAnglePath(angle, angleTarget);

                time = 0f;
                cooldown = UnityEngine.Random.Range(0, 5);
            }
        }
        else // If the current angle is not equal to target angle, we will increase/degrease the current angle 
        {

            angle += angleTurnSharpness * Time.deltaTime * angleIncreaseMultiplier;

            if (angleIncreaseMultiplier == 1 & angleTarget < angle)

            {
                angle = angleTarget;
            }

            else if (angleIncreaseMultiplier == -1 & angleTarget > angle)

            {
                angle = angleTarget;
            }

        }

        positionUpdate();

    }

    // -----------------------------------------------------------------------------------------------------------------

    void ShortestAnglePath(float currentAngle, float targetAngle)
    {
        /* Finds the shortest path from the current angle to the target angle. So for example, if the target angle is 350° and 
         * the current angle is 20°, then we want to degrease the current angle and therefore angleIncreaseMultiplier would be -1. */

        if (Mathf.Abs(targetAngle - currentAngle) > 180)
        {
            if (targetAngle > currentAngle)
            {
                angle += 360;
                angleIncreaseMultiplier = -1;
            }
            else
            {
                angle -= 360;
                angleIncreaseMultiplier = 1;
            }
        }
        else
        {
            if (targetAngle > currentAngle)
            {
                angleIncreaseMultiplier = 1;
            }
            else
            {
                angleIncreaseMultiplier = -1;
            }
        }
    }

    // -----------------------------------------------------------------------------------------------------------------

    void positionUpdate()
    {
        // Takes angle and speed and updates the light postion / rotation

        rotationX = Mathf.Cos(angle * Mathf.Deg2Rad) * speed;
        rotationZ = Mathf.Sin(angle * Mathf.Deg2Rad) * speed;

        this.transform.Rotate(rotationX * Time.deltaTime, 0, rotationZ * Time.deltaTime);
    }

    // -----------------------------------------------------------------------------------------------------------------

}