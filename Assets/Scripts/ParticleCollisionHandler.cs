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
        // ������ײ�¼�
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
        // ȡ��������ײ�¼�'
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
            // ��ȡ���Ӻ���ײ��������
            int particleIndex = contact.bodyA;
            int colliderIndex = contact.bodyB;

            // ��ȡ��ײ��
            var contactCollider = ObiColliderWorld.GetInstance().colliderHandles[colliderIndex].owner;

            if (contactCollider != null && contactCollider == obiCollider)
            {
                // �����Ӵӻ�Ծ���Ӽ������Ƴ�������
                emitter.life[particleIndex] = 0; // �������ӵ���������Ϊ0����ʾ����
            }
        }
    }

    //  TEST 




}
