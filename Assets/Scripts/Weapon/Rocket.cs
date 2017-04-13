using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rocket : Projectile {
    void Awake() {
        damage = 100f; //1 shot kill
        maxSpeed = 7f;
        startSpeed = 1;
        currentSpeed = startSpeed;
        impactSound = Resources.Load("impact-rocket") as AudioClip;
    }

}
