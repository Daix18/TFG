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
        GameObject instance = Instantiate(_zombie);
        AudioManager.THIS.PlaySound("JUMPSCARE");
        float duration = 1f;
        float timer = 0f;
        while (timer < duration)
        {
            Vector3 spawnPosition = _playerCamera.position + _playerCamera.forward * _distanceToPlayer + Vector3.up;
            spawnPosition.y += heightOffset;

            instance.transform.position = spawnPosition;

            Vector3 target = _playerCamera.position;
            target.y = instance.transform.position.y;

            instance.transform.LookAt(target);
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(instance);
    }
}
