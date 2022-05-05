using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectablePart : MonoBehaviour
{
    [SerializeField] int minX;
    [SerializeField] int maxX;
    [SerializeField] int minY;
    [SerializeField] int maxY;

    [SerializeField] bool respawnOnStart;

    [SerializeField] UnityEvent OnRespawnEvent;
    [SerializeField] bool onlyCallEventOnce;
    bool eventCalled;

    private void Start()
    {
        if (respawnOnStart)
            Respawn();
    }

    public void Respawn()
    {
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX + 1), Random.Range(minY, maxY + 1));
        transform.position = randomPosition;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (var hitCollider in hitColliders)
        {
            Respawn();
            return;
        }
        if (!eventCalled)
        {
            OnRespawnEvent?.Invoke();
            if (onlyCallEventOnce)
            {
                eventCalled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bomb"))
        {
            Respawn();
        }
    }
}
