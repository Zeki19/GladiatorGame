using Enemies.Gaius;
using Enemies.Valeria;
using UnityEngine;
using UnityEngine.UI;

public class KillEnemyButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        if (_button != null)
        {
            _button.onClick.AddListener(KillCurrentEnemy);
        }
    }

    public void KillCurrentEnemy()
    {
        GaiusController gaius = ServiceLocator.Instance.GetService<GaiusController>();
        if (gaius != null)
        {
            gaius.KillEnemy();
            Debug.Log("Gaius eliminado");
            return;
        }

        ValeriaController valeria = ServiceLocator.Instance.GetService<ValeriaController>();
        if (valeria != null)
        {
            valeria.KillEnemy();
            Debug.Log("Valeria eliminada");
            return;
        }

        Debug.LogWarning("No hay ningún enemigo registrado en el ServiceLocator");
    }

    private void OnDestroy()
    {
        if (_button != null)
        {
            _button.onClick.RemoveListener(KillCurrentEnemy);
        }
    }
}