using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

 
// Different types of movements for the platform
public enum MovementType {
    line,
    circular,
    zigzag
};

// Movement orientation for line movement type
public enum LineMovementOrientation {
    horizontal,
    vertical
}

// Movement orientation for circular movement type
public enum CircularMovementOrientation {
    clockwise,
    counterclockwise
}

public class MovingObstacle : MonoBehaviour
{

    // General vars (affect all types of movemens)

    [Header("Delay Time")]
    public bool IsDelayed;
    public bool IsDelayedRandomized;
    public float DelayTime;
    
    
    [SerializeField]
    MovementType movementType;
    public float speed = 2f;
    Vector3 startPosition;
    public Color gizmoColor = Color.yellow;

    // Line movement vars
    public LineMovementOrientation lineMovementOrientation; 
    public float lineDistance = 5f; 

    // Circle movement vars
    public CircularMovementOrientation circularMovementOrientation;
    public float circleRadius = 5f;

    // Zigzag movement vars
    public int zigzagLines = 4;
    public float zigzagLineDistance = 2;
    float zigzagStep;
    bool zigzagMovingPositive = true;
    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        zigzagStep = 0f;
        if (IsDelayedRandomized)
        {
            RandomizeDelay();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDelayed)
        {
            StartCoroutine(MovementDelay());
            return;
        }
        
        // Manage how the platform have to move according to movementType var
        switch (movementType) {
            case MovementType.line:
                moveInAStraightLine();
            break;
            case MovementType.circular:
                MoveInCircles();
            break;
            case MovementType.zigzag:
                MoveInZigzag();
            break;
        }
    }

    IEnumerator MovementDelay()
    {
        yield return new WaitForSeconds(DelayTime);
        IsDelayed = false;
    }

    void RandomizeDelay()
    {
        DelayTime = UnityEngine.Random.Range(1f, 4f);
    }
    
    // Move the platform in a straight line in movementOrientation
    public void moveInAStraightLine() 
    {
        // Get current coordenates 
        float x = startPosition.x;
        float y = startPosition.y;
        float z = startPosition.z;

        // Calculating next position according to the orientation selected
        switch (lineMovementOrientation) {
            case LineMovementOrientation.horizontal:
                x = startPosition.x + Mathf.Sin(Time.time * speed) * lineDistance;
            break;
            case LineMovementOrientation.vertical:
                y = startPosition.y + Mathf.Sin(Time.time * speed) * lineDistance;
            break;
        }

        // Moving platform
        transform.position = new Vector3(x,y,z);
    }

    public void MoveInCircles() 
    {
        // Calculating direction (CW or CCW)
        int direction = (circularMovementOrientation == CircularMovementOrientation.counterclockwise)?1:-1;

        // Calculating coordinates 
        float x = startPosition.x + Mathf.Cos(Time.time * speed * direction) * circleRadius;
        x -= circleRadius;
        float y = startPosition.y + Mathf.Sin(Time.time * speed * direction) * circleRadius;
        float z = transform.position.z;

        // Moving Platform
        transform.position = new Vector3(x,y,z);

    }


    public void MoveInZigzag() 
    {   
        // Changing direction when the platform reach the limits 
        if (transform.position.x >= startPosition.x + zigzagLineDistance * zigzagLines) {
            zigzagMovingPositive = false;
        } else if (transform.position.x <= startPosition.x) {
            zigzagMovingPositive = true;
        }

        // Calculating coordenates
        float factor = (Mathf.Acos(Mathf.Cos(zigzagStep * (float)Math.PI)) / (float)Math.PI);
        float x = startPosition.x + zigzagStep * zigzagLineDistance;
        float y = startPosition.y + factor * zigzagLineDistance;
        
        if (zigzagMovingPositive) {
            zigzagStep += speed/50;
        } else {
            zigzagStep -= speed/50;
        }

        // keep platform within the limits
        zigzagStep = Mathf.Clamp(zigzagStep, 0, zigzagLines);


        // Moving platform 
        this.transform.position = new Vector3(x,y);
    }

    // Funtion to see the platform path (only for debugging)
    private void OnDrawGizmosSelected() {
        Gizmos.color = gizmoColor;
        Vector3 src;
        Vector3 dest;

        switch (movementType) {
            case MovementType.line:
                src = new Vector3 (startPosition.x - lineDistance, startPosition.y);
                dest = new Vector3 (startPosition.x + lineDistance, startPosition.y); 
                Gizmos.DrawLine(src, dest);
                src = new Vector3 (startPosition.x, startPosition.y - lineDistance);
                dest = new Vector3 (startPosition.x, startPosition.y + lineDistance); 
                Gizmos.DrawLine(src, dest);
            break;
            case MovementType.circular:
                // Circular movement 
                src = new Vector3(transform.position.x - circleRadius, transform.position.y);
                Gizmos.DrawWireSphere(src, circleRadius);
            break;
            case MovementType.zigzag:
                float x = startPosition.x;
                float y = startPosition.y;

                for(int i = 0; i < zigzagLines; i++){
                    
                    // Current position
                    src = new Vector3(x, y);
                    
                    // Calculating next position
                    x += zigzagLineDistance;

                    // If "i" is even draw line going up, else, draw line going down
                    // the zigzag movement always start going up
                    y = (i%2 == 0)?startPosition.y + zigzagLineDistance:startPosition.y;

                    dest = new Vector3(x,y);

                    // Drawing line
                    Gizmos.DrawLine(src, dest);

                }
                break;
        }
    }
    
}