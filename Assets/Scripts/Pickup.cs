using UnityEngine;

public class Pickup : MonoBehaviour, ICollectible
{
    public bool hasBeenCollected = false;

    public int ScoreAmount;
    public virtual void Collect()
    {
        hasBeenCollected = true;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GameManager.instance.IncreaseScore(ScoreAmount);
            Destroy(gameObject);
        }
    }
}
