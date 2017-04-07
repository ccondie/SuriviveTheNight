using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buckshot : Projectile {
    void Awake() {
        damage = 20f;
        maxSpeed = 7f;
    }
}
