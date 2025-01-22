using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Towers : MonoBehaviour
{

    public bool onCooldown;
    public float _timeCooldown;
    
    public int _amountBullets;
    public float _timeBetweenBullets;
    public bool cooldownBetweenBullets;

    public GameObject _shootPoint;
    public GameObject _bulletPref;
    public float _bulletSpeed;

    private CircleCollider2D circleCollider;
    public float _rangeShoot;

    public float _spreadAngle;
    private Transform targetEnemy;

    public Towers(bool st_onCooldown, float st_timeCooldown, int st_amountBullets, float st_timeBetweenBullets, bool st_cooldownBetweenBullets, 
        GameObject st_shootPoint, GameObject st_bulletPref, float st_bulletSpeed, CircleCollider2D st_circleCollider, float st_rangeShoot, 
        float st_spreadAngle, Transform st_targetEnemy)
    {
        onCooldown = st_onCooldown;
        _timeCooldown = st_timeCooldown;
        _amountBullets = st_amountBullets;
        _timeBetweenBullets = st_timeBetweenBullets;
        cooldownBetweenBullets = st_cooldownBetweenBullets;
        _shootPoint = st_shootPoint;
        _bulletPref = st_bulletPref;
        _bulletSpeed =  st_bulletSpeed;
        circleCollider = st_circleCollider ;
        _rangeShoot = st_rangeShoot;
        _spreadAngle = st_spreadAngle;
        targetEnemy = st_targetEnemy;
    }
    private void Start()
    {

        circleCollider = GetComponent<CircleCollider2D>();
        cooldownBetweenBullets = false;
        _shootPoint = transform.GetChild(0).gameObject;

        RangeChange(_rangeShoot, circleCollider);
    }
    // Si hay un enemigo en el rango comienza accede a Shoot
    private void Update()
    {
        if (targetEnemy != null) 
        {
            Shoot();
        }
    }
    // Comprobamos si un enemigo entra dentro del rango de la torre
    // Si hay devuelve la posicion 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            targetEnemy = collision.transform;
        }
    }
    // Si no hay enemigos, devuelve un valor nulo
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            targetEnemy = null;
        }
    }
    public void Shoot()
    {
        if (onCooldown == false)
        {
            StartCoroutine(ShootBurst());

            onCooldown = true;
        }
    }

    //Crea una bala hacia la dirección donde se encuentra el enemigo
    public virtual void CreateBullet(Vector3 targetPosition)
    {
        GameObject bulletInstance = Instantiate(_bulletPref, _shootPoint.transform.position, Quaternion.identity);
        bulletInstance.transform.SetParent(this.transform);
        Vector3 direction = (targetPosition - _shootPoint.transform.position).normalized;

        Rigidbody2D bulletRb = bulletInstance.GetComponent<Rigidbody2D>();
        bulletRb.velocity = direction * _bulletSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bulletInstance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    //Permite y controla que las balas se disparen en rafaga cada X cantidad de segundos
    //En la misma rafaga, las balas estas levemente separadas para no generar un OVERLAP de las mismas
    public virtual IEnumerator ShootBurst()
    {
        for (int i = 0; i < _amountBullets; i++)
        {
            if (cooldownBetweenBullets == false)
            {
                cooldownBetweenBullets = true;
                Debug.Log("Disparando bala " + (i+1));
                // Disparamos una bala
                CreateBullet(targetEnemy.position);

                // Tiempo de espera entre cada bala de la rafaga
                yield return new WaitForSeconds(_timeBetweenBullets);      
                cooldownBetweenBullets = false;
            }
        }
        // Espera para disparar otra rafaga
        yield return new WaitForSeconds(_timeCooldown);
        onCooldown = false;
    }
    public void RangeChange(float range, CircleCollider2D collider)
    {
        circleCollider.radius = range;
    }
    //private void BulletDispersion()
    //{
    //    var qAngle = Quaternion.AngleAxis(-_amountBullets / 2.0 * _spreadAngle, transform.up) * transform.rotation;
    //    var qDelta = Quaternion.AngleAxis(_spreadAngle, transform.up);

    //    qAngle = qDelta * qAngle;
    //}
}
