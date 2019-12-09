using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectyleBehaviour : MonoBehaviour
{

    Vector3 _direction;
    [SerializeField]
    float speed;

    private void Update()
    {
        transform.Translate( _direction.normalized * speed * Time.deltaTime);
    }

    public void SetDestination(Vector3 direction)
    {
        _direction = direction;
        _direction.y += 1;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player")
        {

            other.GetComponent<PlayerController>().UpdateHp(-5);
        }
        Debug.Log("Vaia");
    }

}
