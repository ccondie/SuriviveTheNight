using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_SMG : Projectile {
    void Awake() {
        damage = 25f; //4 shots to kill
        maxSpeed = 7f;
        startSpeed = maxSpeed;
        currentSpeed = startSpeed;
    }
}
