using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vasik
{
    public class Debris : MonoBehaviour
    {
        private void Start()
        {
            Destroy(gameObject, 2f);
        }
    }

}
