﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Projectile {
    void Awake() {
        damage = 100f;
        maxSpeed = 3f;
    }
}
