using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RayCastController : MonoBehaviour
{
    //layer of ground and plateforms
    public LayerMask collisionMask;

    [HideInInspector] public const float skinWidth = .015f;
    const float dstBetweenRays = .25f;

    //number points to look for collisions
    [HideInInspector] public int horizontalRayCount;
    [HideInInspector] public int verticalRayCount;

    // space between each point
    [HideInInspector] public float horizontalRaySpacing;
    [HideInInspector] public float verticalRaySpacing;

    //**Spencer Change *** Changed variable name because gameobjects have a variable named this allready and it was conflicting
    [HideInInspector] public Collider2D boxCollider;
    [HideInInspector] public RaycastOrigins raycastOrigins;

    //set orgins of collision test points at corners of hit box
    public void UpdateRaycastOrigins()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    //clculates distance between collision test points
    public void CalculateRaySpacing()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    //values of corners of hitbox
    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}