using UnityEngine;
using Obi;

public class ParticleManager : MonoBehaviour
{
    public ObiEmitter emitter;
    public float thresholdHeight = 0.0f; // 设置粒子高度阈值

    void Start()
    {
        if (emitter == null)
        {
            emitter = GetComponent<ObiEmitter>();
        }


        emitter.solver.OnCollision += Solver_OnCollision;
    }

    private void OnDestroy()
    {
        emitter.solver.OnCollision -= Solver_OnCollision;
    }

    private void Solver_OnCollision(ObiSolver solver, ObiSolver.ObiCollisionEventArgs e)
    {
        var positions = solver.positions;

        for (int i = 0; i < e.contacts.Count; ++i)
        {
            var contact = e.contacts[i];
            var particleIndex = contact.bodyA;

            if (positions[particleIndex].y < thresholdHeight )
            {
                emitter.DeactivateParticle(particleIndex);
                bool print= emitter.EmitParticle(0,0.02f);
                if (!print)
                {
                    Debug.LogError(emitter.activeParticleCount);
                }
            }
        }
    }

    //private bool IsCollidingWithSpecifiedObject(ObiSolver.ObiCollisionEventArgs.ObiContact contact)
    //{
    //    // 根据需要调整碰撞条件
    //    return true;
    //}

    //private void RecycleAndEmitParticle(int particleIndex)
    //{
    //    particlePool.ReturnParticle(particleIndex);

    //    int newParticleIndex = particlePool.GetParticle();
    //    if (newParticleIndex != -1)
    //    {
    //        EmitParticle(newParticleIndex, emitter.transform.position, Vector3.up * 2.0f);
    //    }
    //}

    //private void EmitParticle(int index, Vector3 position, Vector3 velocity)
    //{
    //    var solver = emitter.solver;

    //    solver.positions[index] = position;
    //    solver.velocities[index] = velocity;

    //    emitter.partic[emitter.numActiveParticles] = index;
    //    emitter.numActiveParticles++;
    //}
}
