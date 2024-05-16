using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    Rigidbody _rigidbody;
    Flock flock;
    Vector3 _direction;
    List<Mover> _neighbours = new List<Mover>();

    public Gun gun;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (flock == null) _rigidbody.AddRelativeForce(Vector3.forward * 20);
        else
        {
            _rigidbody.AddRelativeForce(Vector3.forward * flock.m_speed);

            GetNeighbours();

            _direction = Vector3.zero;
            _direction += SteerSeparation() * flock.m_separationWeight;
            _direction += Alignment() * flock.m_alignmentWeight;
            _direction += Cohesion() * flock.m_cohesionWeight;
            if (flock.target != null) _direction += TargetSteering(flock.target.position) * flock.m_targetWeight;
            ChangeDirection(_direction);

            // if (InVisionCone(flock.target.position, flock.m_shootVisionAngle))
            // {
            //     gun.Fire();
            // }
        }



    }
    private void GetNeighbours()
    {
        _neighbours = flock.birds.FindAll(n => n != this &&
                                            InVisionCone(n.transform.position, flock.m_visionConeAngle)
                                            && InRadius(n.transform.position, flock.m_sightRadius));
    }

    private Vector3 SteerSeparation()
    {
        Vector3 direction = Vector3.zero;
        if (_neighbours.Count == 0) return direction;

        foreach (var neighbour in _neighbours)
        {
            float ratio = Mathf.Clamp01((neighbour.transform.position - transform.position).magnitude / flock.m_separationRadius);
            direction -= ratio * (neighbour.transform.position - transform.position);
        }
        Debug.Log(_direction.normalized);
        return direction.normalized;
    }
    private Vector3 Alignment()
    {
        Vector3 direction = Vector3.zero;

        if (flock.birds.Count == 0) return direction;
        foreach (var neighbour in flock.birds)
        {
            float ratio = Mathf.Clamp01((neighbour.transform.position - transform.position).magnitude / flock.m_alignmentRadius);
            direction += ratio * (neighbour.transform.forward - transform.forward);

        }
        return direction.normalized;
    }
    private Vector3 Cohesion()
    {
        Vector3 direction = Vector3.zero;
        if (flock.birds.Count == 0) return direction;
        Vector3 middle = Vector3.zero;
        foreach (var neighbour in flock.birds)
        {
            middle += neighbour.transform.position;
        }
        middle /= flock.birds.Count;
        direction += (middle - transform.position);
        return direction.normalized;
    }
    private Vector3 TargetSteering(Vector3 targetPos)
    {
        Vector3 direction = Vector3.zero;

        direction += (targetPos - transform.position);
        return direction.normalized;
    }


    public void ChangeDirection(Vector3 dir)
    {
        //rotate towards dir over time
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(dir), Time.deltaTime);
    }
    public void SetFLock(Flock flock)
    {
        this.flock = flock;
    }
    bool InVisionCone(Vector3 target, float angle = 0)
    {
        //create vision cone and then check if target is in the cone
        if (Vector3.Angle(transform.forward, target - transform.position) < angle)
        {
            return true;
        }
        return false;

    }
    bool InRadius(Vector3 target, float radius = 0)
    {
        if ((target - transform.position).magnitude < radius)
        {
            return true;
        }
        return false;
    }
}
