using UnityEngine;

public class Billboard : MonoBehaviour
{

	Transform m_Camera;

	void Start()
	{
		// 获取场景里的main camera
		m_Camera = Camera.main.transform;
	}

	// 用LateUpdate, 在每一帧的最后调整Canvas朝向
	void LateUpdate()
	{
		if (m_Camera == null)
		{
			return;
		}
		transform.rotation = Quaternion.LookRotation(transform.position - m_Camera.position);
	}
}
