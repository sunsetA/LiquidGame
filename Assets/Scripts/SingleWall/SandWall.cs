using Obi;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SandWall : MonoBehaviour
{
    public ObiCollider2D m_collider;
    public ObiEmitter emitter;

    public int Index = -1;


    public int colliderCount = 0;
    public int ColliderCount = 1000;



    void Start()
    {
        emitter.solver.OnCollision += Solver_OnCollision;

    }

    private void Solver_OnCollision(ObiSolver solver, ObiSolver.ObiCollisionEventArgs contacts)
    {
        if (!this.isActiveAndEnabled)
        {
            return;
        }
        foreach (var contact in contacts.contacts)
        {
            // ��ȡ���Ӻ���ײ��������
            int particleIndex = contact.bodyA;
            int colliderIndex = contact.bodyB;

            // ��ȡ��ײ��
            var contactCollider = ObiColliderWorld.GetInstance().colliderHandles[colliderIndex].owner;

            if (contactCollider != null && contactCollider == m_collider)
            {
                // �����Ӵӻ�Ծ���Ӽ������Ƴ�������
                emitter.life[particleIndex] = 0; // �������ӵ���������Ϊ0����ʾ����
                Debug.Log("���ճɹ�");
                colliderCount++;
                Vector3 localscale = transform.localScale;
                this.transform.localScale = new Vector3(localscale.x, Mathf.Lerp(0.02f, 0.1f, (float)colliderCount / ColliderCount), localscale.z);
                if (colliderCount> ColliderCount)
                {
                    m_collider.enabled = false;
                }
            }
        }
    }

    private void OnDestroy()
    {
        emitter.solver.OnCollision -= Solver_OnCollision;
    }
}
