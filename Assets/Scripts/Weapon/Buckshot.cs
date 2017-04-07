using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buckshot : Projectile {
    void Awake() {
        damage = 20f; //5 shots to kill (one shell from up close)
        maxSpeed = 7f;
        startSpeed = maxSpeed;
        currentSpeed = startSpeed;
    }
}
