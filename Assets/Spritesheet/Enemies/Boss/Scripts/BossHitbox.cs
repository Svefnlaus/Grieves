using UnityEngine;

public class BossHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        other.TryGetComponent<Hpmanager>(out Hpmanager player);
        player.TakeDamage(BossBehavior.bossNormalAttack);
    }
}
