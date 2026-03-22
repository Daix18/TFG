using System.Collections;
using UnityEngine;

public class JumpScare : MonoBehaviour
{
    public GameObject _zombie;

    Transform _playerCamera;

    float _distanceToPlayer = 1.0f;

    float heightOffset = -2.5f; 

    private void Start()
    {
        _playerCamera = Camera.main.transform;
    }

    public void TriggerJumpScare()
    {
        StartCoroutine(ZombieJumpScare());
    }

    IEnumerator ZombieJumpScare()
    {
        Vector3 spawnPosition = _playerCamera.position + _playerCamera.forward * _distanceToPlayer + Vector3.up;
        spawnPosition.y += heightOffset;
        GameObject instance = Instantiate(_zombie,spawnPosition, Quaternion.identity);
        Vector3 target = _playerCamera.position;
        target.y = instance.transform.position.y;
        AudioManager.THIS.PlaySound("JUMPSCARE");
        instance.transform.LookAt(target);
        yield return new WaitForSeconds(1f);
        Destroy(instance);
    }
}
