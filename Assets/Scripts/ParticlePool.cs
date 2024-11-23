using System.Collections.Generic;
using UnityEngine;
using Obi;

public class ParticlePool : MonoBehaviour
{
    private Queue<int> particleQueue = new Queue<int>();

    public void Initialize(ObiSolver solver)
    {
        // ��ʼ������أ����������������������
        for (int i = 0; i < solver.positions.count; i++)
        {
            particleQueue.Enqueue(i);
        }
    }

    public int GetParticle()
    {
        if (particleQueue.Count > 0)
        {
            return particleQueue.Dequeue();
        }
        return -1; // ���û�п������ӷ���-1
    }

    public void ReturnParticle(int particleIndex)
    {
        particleQueue.Enqueue(particleIndex);
    }
}
