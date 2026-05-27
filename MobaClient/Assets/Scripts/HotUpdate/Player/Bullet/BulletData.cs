using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Normal,
    Explosive,
    Piercing,
    Penetrating,
    Homing,
    Tracking,
}

public class BulletData
{
    public float speed;
    public float damage;
    public float lifeTime;
    public BulletType type;
}

public class BulletHitContext
{
    public Vector3 hitPosition;
}
