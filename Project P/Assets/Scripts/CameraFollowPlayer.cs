using UnityEngine;
using Cinemachine;

public class CameraFollowPlayer : MonoBehaviour {
    public Transform player; // 플레이어 트랜스폼
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineTransposer transposer;

    void Start() {
        // Virtual Camera에서 Transposer 컴포넌트를 가져옵니다.
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    void LateUpdate() {
        // 플레이어의 y축 회전을 고정
        Vector3 playerRotation = player.eulerAngles;
        playerRotation.y = 0;
        virtualCamera.transform.eulerAngles = playerRotation;

        // 플레이어의 위치를 따라가도록 설정
        transposer.m_FollowOffset = new Vector3(transposer.m_FollowOffset.x, transposer.m_FollowOffset.y, transposer.m_FollowOffset.z);
    }
}
