using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnableObject
{
    void OnSpawn (string _poolTag);
    void OnPickUp ();
}
