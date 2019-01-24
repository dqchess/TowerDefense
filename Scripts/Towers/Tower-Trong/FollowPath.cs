using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

    //public GameObject _Enemy;
    public GameObject _Path;
    PathDefinition path;
    public float Speed = 1;
    public float MaxDistanceToGoal = .1f;

    private IEnumerator<Transform> _currentPoint;

    public void Start()
    {
        _Path = GameObject.Find("EnemyPath");
        path = _Path.GetComponent<PathDefinition>();

        _currentPoint = path.GetPathEnumerator();
        _currentPoint.MoveNext();

        if (_currentPoint.Current == null)
            return;

        transform.position = _currentPoint.Current.position;
    }

    public void Update()
    {
        if (_currentPoint == null || _currentPoint.Current == null)
            return;

        // If this object has reached the end of the path

        transform.position = Vector2.MoveTowards(transform.position, _currentPoint.Current.position, Time.deltaTime * Speed);


        var distanceSquared = (transform.position - _currentPoint.Current.position).sqrMagnitude;
        if (distanceSquared < MaxDistanceToGoal * MaxDistanceToGoal)
        {
            _currentPoint.MoveNext();

            if (_currentPoint.Current.position == path.Waypoints[path.Waypoints.Length - 1].position && transform.position == _currentPoint.Current.position)
            {
                // Damage player and destroy this enemy
                this.gameObject.GetComponent<Enemy>().DamagePlayerHealth();
            }
        }
    }
}
