using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LaboratorioMovimiento
{
    public sealed class LabGameManager : MonoBehaviour
    {
        public static LabGameManager Instance { get; private set; }
        [SerializeField] private Text coinText;
        [SerializeField] private Text statusText;
        [SerializeField] private int totalCoins = 3;
        private int collectedCoins;
        public int CollectedCoins => collectedCoins;

        private void Awake() { Instance = this; UpdateUi(); }
        private void OnDestroy() { if (Instance == this) Instance = null; }
        public void CollectCoin() { collectedCoins++; UpdateUi(); }
        private void UpdateUi()
        {
            if (coinText != null) coinText.text = $"MONEDAS  {collectedCoins}/{totalCoins}";
            if (statusText != null) statusText.text = collectedCoins >= totalCoins ? "LABORATORIO COMPLETADO" : "Explora, salta y mueve la caja";
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
