using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public float effectLength;
    public int sFx;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlaySFX(sFx);
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, effectLength);
    }
}
