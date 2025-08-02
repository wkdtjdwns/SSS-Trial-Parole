using System.Collections.Generic;
using UnityEngine;

public class GhostReplayer2D : MonoBehaviour
{
    private List<Vector3> positions;
    private int index;

    public void Setup(List<Vector3> posList)
    {
        positions = posList;
        index = 0;
    }

    void Update()
    {
        if (positions == null || positions.Count == 0) return;

        if (index < positions.Count)
        {
            transform.position = positions[index];
            index++;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
