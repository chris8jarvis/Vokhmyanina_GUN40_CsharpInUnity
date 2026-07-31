using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interface
{
    public interface IDamageable
    {
        void TakeDamage(int amount);
        void Heal(int amount);
    }
}
