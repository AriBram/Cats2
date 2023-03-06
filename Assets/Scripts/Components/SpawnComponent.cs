using UnityEngine;

public class SpawnComponent : MonoBehaviour
{
   [SerializeField] private Transform _target;
   [SerializeField] private GameObject _prefab;

   [ContextMenu("Spawn")]
   public void Spawn()
   {
       var instantiate =  Instantiate(_prefab, _target.position, Quaternion.identity);
       instantiate.transform.localScale = _target.lossyScale;
   }

   [ContextMenu("SpawnWithoutLossyScale")]
   public void SpawnWithoutLossyScale()
   {
       var instantiate =  Instantiate(_prefab, _target.position, Quaternion.identity);
   }
}
