using UnityEngine;
using EzySlice;

public class AutoStoneModelCutter : MonoBehaviour
{
    [Header("Configuration")]
    public Material stoneInsideMaterial;
    public float cuttingForce = 150f;
    public bool cutOnCollision = true; // Auto-cut when cutting model collides with stone
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!cutOnCollision) return;
        
        // Check if the colliding object is a stone
        if (collision.gameObject.CompareTag("Stone"))
        {
            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 cutNormal = transform.up; // Use this object's up direction as cut plane
            
            Debug.Log($"Collision detected with {collision.gameObject.name}, performing cut");
            CutStone(collision.gameObject, contactPoint, cutNormal);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!cutOnCollision) return;
        
        // Check if the trigger object is a stone
        if (other.gameObject.CompareTag("Stone"))
        {
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            Vector3 cutNormal = transform.up;
            
            Debug.Log($"Trigger detected with {other.gameObject.name}, performing cut");
            CutStone(other.gameObject, contactPoint, cutNormal);
        }
    }

    public void CutStone(GameObject stone, Vector3 cutPosition, Vector3 cutNormal)
    {
        SlicedHull hull = stone.Slice(cutPosition, cutNormal);

        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(stone, stoneInsideMaterial);
            GameObject lowerHull = hull.CreateLowerHull(stone, stoneInsideMaterial);

            if (upperHull != null && lowerHull != null)
            {
                SetupPiece(upperHull, cutPosition);
                SetupPiece(lowerHull, cutPosition);
                
                Destroy(stone);
                Debug.Log($"Stone cut successfully: {stone.name}");
            }
        }
        else
        {
            Debug.LogWarning($"Failed to slice {stone.name}");
        }
    }

    void SetupPiece(GameObject piece, Vector3 explosionCenter)
    {
        MeshCollider col = piece.AddComponent<MeshCollider>();
        col.convex = true;
        
        Rigidbody rb = piece.AddComponent<Rigidbody>();
        rb.AddExplosionForce(cuttingForce, explosionCenter, 2f);
        
        piece.layer = gameObject.layer;
    }
}
