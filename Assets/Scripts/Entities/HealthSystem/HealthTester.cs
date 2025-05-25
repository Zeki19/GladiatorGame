using UnityEngine;

public class HealthTester : MonoBehaviour
{
    [Header("Configuraci�n de prueba")]
    [SerializeField, Tooltip("Cu�nto da�o aplicar al presionar la tecla.")]
    private float damageAmount = 10f;

    [SerializeField, Tooltip("Tecla para da�ar al jugador.")]
    private KeyCode damageKey = KeyCode.Space;

    private IHealth _health;

    private void Awake()
    {
        // Obt�n el PlayerManager (o donde guardes tu HealthSystem)
        var playerManager = ServiceLocator.Instance.GetService<PlayerManager>();
        if (playerManager == null)
        {
            Debug.LogError("[HealthTester] No se encontr� PlayerManager en el ServiceLocator.");
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
            Debug.Log($"[HealthTester] Aplicando {damageAmount} puntos de da�o.");
            _health.TakeDamage(damageAmount);
        }
    }
}