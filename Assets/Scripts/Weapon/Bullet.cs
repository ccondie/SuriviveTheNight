using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile {
    void Awake() {
        damage      = 34f; //3 shots to kill
        maxSpeed    = 7f;
        startSpeed = maxSpeed;
        currentSpeed = startSpeed;
    }
}
