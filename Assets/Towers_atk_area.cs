using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Towers_atk_area : Towers
{
    private Vector3 directions;
    public float _spreadOnX;
    public float _spreadOnY;

    public Towers_atk_area(bool st_onCooldown, float st_timeCooldown, int st_amountBullets, float st_timeBetweenBullets, bool st_cooldownBetweenBullets,
    GameObject st_shootPoint, GameObject st_bulletPref, float st_bulletSpeed, CircleCollider2D st_circleCollider, float st_rangeShoot,
    float st_spreadAngle, Transform st_targetEnemy)
        :base(st_onCooldown,st_timeCooldown, st_amountBullets,st_timeBetweenBullets,st_cooldownBetweenBullets,st_shootPoint,st_bulletPref,
            st_bulletSpeed,st_circleCollider,st_rangeShoot,st_spreadAngle,st_targetEnemy
    ){}
    public override IEnumerator ShootBurst()
    {        
        float angleStep = 360f / _amountBullets;
        float angle = 0f;

        for (int i = 0; i < _amountBullets; i++)
        {
            Debug.Log("Disparando bala " + (i + 1));

            float bulletDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletDirY = Mathf.Sin(angle * Mathf.Deg2Rad);

            directions = new Vector3(bulletDirX, bulletDirY, 0);

            angle += angleStep;
            // Disparamos LAS balas
            CreateBullet(directions);
        }
        // Espera para disparar otra rafaga
        yield return new WaitForSeconds(_timeCooldown);
        onCooldown = false;
        //return base.ShootBurst();
    }

}
