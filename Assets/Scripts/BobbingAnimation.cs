using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    Vector3 initialPosition;
    public float Frequency;  // Speed of movement
    public float Magnitude; // Range of movement
    public Vector3 Direction; // Direction of movement
    private Pickup pickup;
    
    void Start()
    {
        pickup = GetComponent<Pickup>();
        // Save the starting position of the game object
        initialPosition = transform.position;
    }

    
    void Update()
    {
        if (pickup && !pickup.hasBeenCollected)
        {
            transform.position = initialPosition + Direction * (Mathf.Sin(Time.time * Frequency) * Magnitude); // Sine function for smooth bobbing effect
        }
        /* BREAKDOWN ON HOW THIS SHIT WORKS
        Mathf.Sin(Time.time * frequency): This part generates a wave-like value that oscillates between -1 and 1 over time. The Mathf.Sin function creates this wave,
        and multiplying it by Time.time * frequency adjusts the speed of the wave based on the frequency value.

        direction * Mathf.Sin(Time.time * frequency): This part applies the wave value to the specified direction. It takes the direction vector (e.g., up, down, left, right, or any combination)
        and moves it back and forth based on the wave value. This creates the movement in the desired direction.

        direction * Mathf.Sin(Time.time * frequency) * magnitude: This part adjusts the magnitude or range of the movement.
        By multiplying the direction with the wave value and then by the magnitude value, we control how far the object moves in each direction. Increasing the magnitude value amplifies the movement,
        while decreasing it reduces the range.

        initialPosition + direction * Mathf.Sin(Time.time * frequency) * magnitude: Lastly, this part combines the original position of the game object (initialPosition) with the calculated movement.
        Adding this movement to the initial position ensures that the game object moves up and down (or left and right, etc.) from its original position, rather than from the world origin.
        
        TL;DR: This line of code creates a bobbing effect by repeatedly moving the game object in a specific direction from its original position,
        with the movement changing in a wave-like pattern over time.
         */
    }
}