using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsMover : MonoBehaviour
{
    private Vector3 _direction;
    private float _speed;
    private float _lifeTime;
    private float _timer;

    public bool IsActive { get; private set; }

    public void Init(Vector3 startPos, Vector3 direction, float speed, float lifeTime)
    {
        transform.position = startPos;
        _direction = direction.normalized;
        _speed = speed;
        _lifeTime = lifeTime;
        _timer = 0f;
        IsActive = true;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!IsActive) return;

        transform.position += _direction * _speed * Time.deltaTime;

        _timer += Time.deltaTime;
        if (_timer >= _lifeTime)
        {
            Deactivate();
        }
    }

    public void Deactivate()
    {
        IsActive = false;
        gameObject.SetActive(false);
    }
}
