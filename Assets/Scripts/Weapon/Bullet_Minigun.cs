using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Minigun : Projectile {
    void Awake() {
        damage = 17f;
        maxSpeed = 7f;
    }
}
