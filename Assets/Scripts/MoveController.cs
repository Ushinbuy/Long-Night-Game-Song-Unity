using System;
using UnityEngine;

public abstract class MoveController : MonoBehaviour
{
    protected float movementUnit = 2.1f;
    protected int numPositions = 3;
    protected int currentLine;

    [SerializeField]protected float speedX = 1f;
    Vector3 yMovingVectorUnit;
    private Vector2 direction;
    private float nextPositionX;
    private float nextPositionY;
    protected float[] allPositions;
    protected MoveState yMoveState = MoveState.NONE;
    private MoveState xMoveState = MoveState.NONE;

    protected enum MoveState
    {
        NONE,
        MOVING,
        DONE
    }

    private void Start()
    {
        InitMoveController();
    }

    protected void InitMoveController()
    {
        direction = Vector2.zero;
        FindCurrentLine();
        allPositions = new float[numPositions];
        int zeroPositionOffset = - numPositions / 2; // calc offset of center postition
        for (int i = 0; i < numPositions; i++)
        {
            allPositions[i] = movementUnit * (float)zeroPositionOffset;
            zeroPositionOffset++;
        }
    }

    public void FindCurrentLine()
    {
        int devideResult = Convert.ToInt32(transform.position.x / movementUnit);
        currentLine = devideResult + (numPositions) /2;
    }

    protected bool SetLeftMove()
    {
        if (currentLine > 0)
        {
            currentLine--;
            direction = Vector2.left;
            xMoveState = MoveState.MOVING;
            nextPositionX = allPositions[currentLine];
            return true;
        }
        return false;
    }

    protected bool SetRightMove()
    {
        if (currentLine < numPositions - 1)
        {
            currentLine++;
            direction = Vector2.right;
            xMoveState = MoveState.MOVING;
            nextPositionX = allPositions[currentLine];
            return true;
        }
        return false;
    }

    protected void MoveToY(float yPosition, float ySpeed)
    {
        yMoveState = MoveState.MOVING;
        if(yPosition < transform.position.y)
        {
            ySpeed *= -1f;
        }
        nextPositionY = yPosition;
        yMovingVectorUnit = new Vector3(0, ySpeed, 0);
    }

    protected bool MoveToRoad(int inputPosition)
    {
        if((inputPosition > numPositions) ||
            (inputPosition < 0))
        {
            Debug.LogError("Road is out of limits");
            return false;
        }

        int diffPositions = inputPosition - currentLine;
        if (diffPositions == 0)
        {
            return true;
        }
        else if(diffPositions < 0)
        {
            for(int i = 0; i < (-diffPositions); i++)
            {
                SetLeftMove();
            }
        }
        else
        {
            for (int i = 0; i < diffPositions; i++)
            {
                SetRightMove();
            }
        }
        return true;
    }

    protected virtual void FixedUpdate()
    {
        if (xMoveState == MoveState.MOVING)
        {
            MovingX();
        }
        if (yMoveState == MoveState.MOVING)
        {
            MovingY();
        }
    }

    protected void MovingY()
    {
        transform.Translate(yMovingVectorUnit * Time.deltaTime);
        if (Mathf.Abs(transform.position.y - nextPositionY) < 0.1f)
        {
            yMoveState = MoveState.DONE;
        }
    }

    protected void MovingX()
    {
        float amountToMove = speedX * Time.deltaTime;
        transform.Translate(direction * amountToMove);
        if (Mathf.Abs(transform.position.x - nextPositionX) < 0.1f)
        {
            xMoveState = MoveState.DONE;
        }
    }
}