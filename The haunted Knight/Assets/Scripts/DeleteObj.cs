using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObj : MonoBehaviour
{
    [SerializeField] private float _sec;
    private void Start()
    {
        StartCoroutine(Delete());
    }

    // Update is called once per frame
    private IEnumerator Delete()
    {
        yield return new WaitForSeconds(_sec);
        Destroy(gameObject);
    }
}
