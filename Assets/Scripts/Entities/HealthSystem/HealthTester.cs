using Player;
using UnityEngine;

public class HealthTester : MonoBehaviour
{
    [Header("Configuración de prueba")]
    [SerializeField, Tooltip("Cuánto daño aplicar al presionar la tecla.")]
    private float damageAmount = 10f;

    [SerializeField, Tooltip("Tecla para dañar al jugador.")]
    private KeyCode damageKey = KeyCode.Space;

    private IHealth _health;

    private void Awake()
    {
        // Obtén el PlayerManager (o donde guardes tu HealthSystem)
        var playerManager = ServiceLocator.Instance.GetService<PlayerManager>();
        if (playerManager == null)
        {
            Debug.LogError("[HealthTester] No se encontró PlayerManager en el ServiceLocator.");
            enabled = false;
            return;
        }

        _health = playerManager.HealthComponent;
        if (_health == null)
        {
            Debug.LogError("[HealthTester] HealthComponent es NULL en PlayerManager.");
            enabled = false;
        }
    }

    private void Update()
    {
        if (!enabled) return;

        if (Input.GetKeyDown(damageKey))
        {
            Debug.Log($"[HealthTester] Aplicando {damageAmount} puntos de daño.");
            _health.TakeDamage(damageAmount);
        }
    }
}