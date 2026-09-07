using UnityEngine;
namespace LaboratorioMovimiento
{
    public sealed class CollectibleCoin : MonoBehaviour
    {
        private bool collected;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (collected || !other.GetComponent<PlayerController>()) return;
            collected = true;
            if (LabGameManager.Instance) LabGameManager.Instance.CollectCoin();
            Destroy(gameObject);
        }
    }
}
