using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PoolObjectsDestroyer : MonoBehaviour
{
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void DestroyPoolObjectOnDelay(PoolObject _poolObject, float _delay)
    {
        StartCoroutine(DestroyObjectOnDelay(_poolObject, _delay));
    }

    private IEnumerator DestroyObjectOnDelay(PoolObject _poolObject, float _delay)
    {
        yield return new WaitForSeconds(_delay);

        if (_poolObject.IsActivated == false)
            Addressables.ReleaseInstance(_poolObject.gameObject);
    }
}