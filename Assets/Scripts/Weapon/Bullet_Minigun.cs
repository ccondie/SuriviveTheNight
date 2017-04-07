using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Minigun : Projectile {
    void Awake() {
        damage = 17f; //6 shots to kill
        maxSpeed = 7f;
        startSpeed = maxSpeed;
        currentSpeed = startSpeed;
    }
}
