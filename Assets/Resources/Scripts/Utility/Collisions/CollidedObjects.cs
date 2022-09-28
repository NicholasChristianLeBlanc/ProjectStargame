using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidedObjects : MonoBehaviour
{
    private List<GameObject> collidedObjects;
    private int collidedObjectsCount = 0;

    private void Start()
    {
        collidedObjects = new List<GameObject>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && (collision.transform.position - transform.position).normalized.y < 0)
        {
            collidedObjects.Add(collision.gameObject);
            collidedObjectsCount++;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            collidedObjects.Add(collision.gameObject);
            collidedObjectsCount++;
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            collidedObjects.Remove(collision.gameObject);
            collidedObjectsCount--;

            if (collidedObjectsCount < 0)
            {
                collidedObjectsCount = 0;
            }
        }
    }

    public List<GameObject> GetCollidedObjects()
    {
        if (collidedObjects.Count >= 1)
        {
            return collidedObjects;
        }
        else
        {
            return null;
        }
    }
}
