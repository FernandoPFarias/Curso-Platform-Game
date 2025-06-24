using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 1f;
    public List<PushableObject> pushables; // Arraste os objetos empurráveis aqui no Inspector
    private PushableObject grabbedObject;

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (grabbedObject == null)
            {
                Vector2 dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
                foreach (var pushable in pushables)
                {
                    if (pushable == null) continue;
                    Vector2 toObj = (Vector2)pushable.transform.position - (Vector2)transform.position;
                    // Checa se está na direção e dentro da distância
                    if (Vector2.Dot(dir, toObj.normalized) > 0.7f && toObj.magnitude <= interactDistance)
                    {
                        pushable.TryGrab(transform);
                        grabbedObject = pushable;
                        break;
                    }
                }
            }
            else
            {
                grabbedObject.Release();
                grabbedObject = null;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + dir * interactDistance);
    }
} 