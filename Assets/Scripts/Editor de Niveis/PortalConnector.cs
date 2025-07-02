using UnityEngine;

public class PortalConnector : MonoBehaviour
{
    public string portalId;
    public string targetSceneId;
    public string targetPortalId;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        // Visualização básica do portal
    }
}
