using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace countMastersTest
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform _characterPos;

        public void setActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public Vector3 getUserCharacterPosition()
        {
            return _characterPos.position;
        }
    }
}
