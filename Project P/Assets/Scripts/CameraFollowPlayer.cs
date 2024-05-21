using UnityEngine;
using Cinemachine;

public class CameraFollowPlayer : MonoBehaviour {
    public Transform player; // �÷��̾� Ʈ������
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineTransposer transposer;

    void Start() {
        // Virtual Camera���� Transposer ������Ʈ�� �����ɴϴ�.
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    void LateUpdate() {
        // �÷��̾��� y�� ȸ���� ����
        Vector3 playerRotation = player.eulerAngles;
        playerRotation.y = 0;
        virtualCamera.transform.eulerAngles = playerRotation;

        // �÷��̾��� ��ġ�� ���󰡵��� ����
        transposer.m_FollowOffset = new Vector3(transposer.m_FollowOffset.x, transposer.m_FollowOffset.y, transposer.m_FollowOffset.z);
    }
}
