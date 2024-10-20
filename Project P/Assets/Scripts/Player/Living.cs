using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Living : MonoBehaviour
{
    public float startingHealth = 100f; // 시작 체력
    public float health { get; protected set; } // 현재 체력
    public bool dead { get; protected set; } // 사망 상태
    public event Action onDeath; // 사망시 발동할 이벤트

    
    protected virtual void OnEnable() {
        // 사망하지 않은 상태로 시작
        dead = false;
        // 체력을 시작 체력으로 초기화
        health = startingHealth;
    }

    public virtual void OnDamage(float damage) {
        // 데미지만큼 체력 감소
        health -= damage;

        // 체력이 0 이하 && 아직 죽지 않았다면 사망 처리 실행
        if (health <= 0 && !dead) {
            Die();
        }
    }
    public virtual void Die() {
        // onDeath 이벤트에 등록된 메서드가 있다면 실행
        if (onDeath != null) {
            onDeath();
        }

        // 사망 상태를 참으로 변경
        dead = true;
    }
    
}
