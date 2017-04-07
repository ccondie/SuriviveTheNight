using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile {
    void Awake() {
        damage      = 34f;
        maxSpeed    = 7f;
    }
}
