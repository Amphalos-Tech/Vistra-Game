using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxVisualizer : MonoBehaviour
{
    public float width;
    public float height;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(width, height));
    }
}
