using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Noah Logan

public interface IPlayer
{
    void Move();

    void PickupItem(string itemName);

    void UseItem();
}
