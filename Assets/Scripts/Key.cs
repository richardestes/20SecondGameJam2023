using UnityEngine;

public class Key: MonoBehaviour, ICollectible
{
    public bool hasBeenCollected = false;

    public virtual void Collect()
    {
        hasBeenCollected = true;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GameManager.instance.ChangeState(GameManager.GameState.NextLevel);
            Destroy(gameObject);
        }
    }
}
