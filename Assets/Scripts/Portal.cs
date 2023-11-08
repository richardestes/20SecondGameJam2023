using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public SpriteRenderer PortalSprite;
    public Transform Destination;
    public SpriteRenderer DestinationSprite;
    public Color InactiveColor;

    private void Start()
    {
        InactiveColor = new Color(.5f, .5f, .5f, 1f);
        DestinationSprite.color = InactiveColor;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayer();
            DestinationSprite.color = Color.white;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PortalSprite.color = InactiveColor;
        }
    }

    void TeleportPlayer()
    {

        GameManager.instance.Player.transform.position = Destination.position;
    }
}