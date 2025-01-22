using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float _damage;
    public float _rangeDestroy;

    private float distance;
    public GameObject _shooter;

    private Towers Towers;

    private void Start()
    {
        // Establece al padre como Shooter
        _shooter = transform.parent.gameObject;
        // Busca el script Towers y devuelve el rango
        // Convierte al _range del padre en el _rangeDestroy +3 
        Towers = GetComponentInParent<Towers>();
        _rangeDestroy = Towers._rangeShoot + 3;
    }
    private void Update()
    {
        RangeToDestroyBullet();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            DestroyBullet();
        }
    }
    // Calcula la distancia de la bala del Shooter
    // Si la distancia es mayor al _rangeDestroy se destruye
    // Si la distancia es menor de 0, para la ejecución
    private void RangeToDestroyBullet()
    {        
        distance = Vector3.Distance(_shooter.transform.position, this.gameObject.transform.position);
        if (distance <= 0) { return; }
        if (distance > _rangeDestroy) 
        {
            DestroyBullet();
        }
    }
    // Destruye el objeto
    private void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
}
