using UnityEngine;
using Obi;
using System.Xml;

public class ParticleCollisionHandler : MonoBehaviour
{
    public ObiEmitter emitterL;
    public ObiEmitter emitterR;
    public ObiCollider obiCollider;
    public ObiCollider obiCollider1;
    void Start()
    {
        // 订阅碰撞事件
        //if (emitterL!=null)
        //{
        //    emitterL.solver.OnCollision += Solver_OnCollisionL;
        //}
        if (emitterR!=null)
        {
            //emitterL.solver.OnCollision += Solver_OnCollisionL;
            emitterR.solver.OnCollision += Solver_OnCollisionR;
        }

    }

    private void OnDestroy()
    {
        // 取消订阅碰撞事件'
        //if (emitterL != null)
        //{
        //    emitterL.solver.OnCollision -= Solver_OnCollisionL;
        //}
        if (emitterR != null)
        {
            emitterR.solver.OnCollision -= Solver_OnCollisionR;
        }
    }

    private void Solver_OnCollisionL(ObiSolver solver, ObiSolver.ObiCollisionEventArgs e)
    {
        HandleCollision(e, emitterL, "EmitterL");
    }

    private void Solver_OnCollisionR(ObiSolver solver, ObiSolver.ObiCollisionEventArgs e)
    {
        HandleCollision(e, emitterR, "EmitterR");
    }

    private void HandleCollision(ObiSolver.ObiCollisionEventArgs e, ObiEmitter emitter, string emitterName)
    {
        foreach (var contact in e.contacts)
        {
            // 获取粒子和碰撞器的索引
            int particleIndex = contact.bodyA;
            int colliderIndex = contact.bodyB;

            // 获取碰撞器
            var contactCollider = ObiColliderWorld.GetInstance().colliderHandles[colliderIndex].owner;

            if (contactCollider != null && contactCollider == obiCollider)
            {
                // 将粒子从活跃粒子集合中移除并回收
                emitter.life[particleIndex] = 0; // 设置粒子的生命周期为0，表示回收
            }
        }
    }

    //  TEST 




}
