using UnityEngine;
using Obi;

public class CollisionWallController : MonoBehaviour
{
    private ObiCollider obiCollider;
    public float collisionThreshold = 10000;
    public float collisionCount = 0;

    public float percent = 0;
    public ObiSolver obiSolver;
    void Start()
    {
        if (obiCollider == null)
        {
            obiCollider = GetComponent<ObiCollider>();
        }
    }

    private void OnEnable()
    {
        obiSolver.OnCollision += Solver_OnCollision;
    }

    private void OnDisable()
    {
        obiSolver.OnCollision -= Solver_OnCollision;
    }

    private void Solver_OnCollision(ObiSolver solver, Obi.ObiSolver.ObiCollisionEventArgs e)
    {
        foreach (var contact in e.contacts)
        {
            var contactCollider = ObiColliderWorld.GetInstance().colliderHandles[contact.bodyB].owner;

            if (contactCollider != null && contactCollider == obiCollider)
            {
                collisionCount++;
                percent = (collisionCount*1.0f) % collisionThreshold;
                if (collisionCount >= collisionThreshold)
                {
                    // ������ֵ������Ϊ��͸
                    //obiCollider.gameObject.layer = LayerMask.NameToLayer("IgnoreFluidCollision");
                    obiCollider.gameObject.GetComponent<Collider>().enabled = false;
                }
                else
                {
                    // δ������ֵʱ����Ϊ����͸
                    //obiCollider.gameObject.layer = LayerMask.NameToLayer("Default");
                }
            }
        }
    }
}
