using System.Collections.Generic;
using UnityEngine;
using Obi;

public class ParticlePool : MonoBehaviour
{
    private Queue<int> particleQueue = new Queue<int>();

    public void Initialize(ObiSolver solver)
    {
        // 初始化对象池，将所有粒子索引加入队列
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
        return -1; // 如果没有可用粒子返回-1
    }

    public void ReturnParticle(int particleIndex)
    {
        particleQueue.Enqueue(particleIndex);
    }
}
