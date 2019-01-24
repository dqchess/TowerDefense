using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDefinition : MonoBehaviour {

    public Transform[] Waypoints;

    public IEnumerator<Transform> GetPathEnumerator()
    {
        if (Waypoints == null || Waypoints.Length < 1)
            yield break;

        int index = 0;

        while (true)
        {
            yield return Waypoints[index];

            if (Waypoints.Length == 1)
                continue;

            if (index < (Waypoints.Length - 1))
                index++;
        }
    }

    public void OnDrawGizmos()
    {
        if (Waypoints == null || Waypoints.Length < 2)
            return;

        for (var i = 1; i < Waypoints.Length; i++)
        {
            Gizmos.DrawLine(Waypoints[i - 1].position, Waypoints[i].position);
        }
    }
}
