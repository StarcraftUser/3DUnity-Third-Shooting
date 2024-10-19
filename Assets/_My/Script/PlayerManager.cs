using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem.XR;

public class PlayerManager : MonoBehaviour
{
	private StarterAssetsInputs input;
	private ThirdPersonController controller;
	private Animator anim;
	[Header("Aim")]
	[SerializeField]
	private CinemachineVirtualCamera aimCam;

	[SerializeField]
	private GameObject aimImage;

	[SerializeField]
	private GameObject aimObj;

	[SerializeField]
	private LayerMask targetLayer;

	[SerializeField]
	private float aimObjDis = 10f;


	[Header("IK")]
	[SerializeField]
	private Rig handRig;
	[SerializeField]
	private Rig aimRig;
	// Start is called before the first frame update
	void Start()
	{
		input = GetComponent<StarterAssetsInputs>();
		controller = GetComponent<ThirdPersonController>();
		anim = GetComponent<Animator>();

	}

	// Update is called once per frame
	void Update()
	{
		AimCheck();
	}

	private void AimCheck()
	{
		if (input.reload)
		{
			input.reload = false;

			if (controller.isReload)
			{
				return;
			}

			aimCam.gameObject.SetActive(false);
			aimImage.SetActive(false);
			controller.isAimMove = false;

            SetRigWeight(0f);

            anim.SetLayerWeight(1, 1f);
			anim.SetTrigger("Reload");
			controller.isReload = true;

		}

		if (controller.isReload)
		{
			return;
		}

		if (input.aim)
		{
			AimControll(true);

			Vector3 targetPosition = Vector3.zero;
			Transform camTransform = Camera.main.transform;
			RaycastHit hit;

			if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity, targetLayer))
			{
				//Debug.Log("Name : " + hit.transform.gameObject.name);

				targetPosition = hit.point;
				aimObj.transform.position = hit.point;
			}
			else
			{
				targetPosition = camTransform.position + camTransform.forward * aimObjDis;
				aimObj.transform.position = camTransform.position + camTransform.forward * aimObjDis;
			}

			Vector3 targetAim = targetPosition;
			targetAim.y = transform.position.y;
			Vector3 aimDir = (targetAim - transform.position).normalized;

			transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * 50f);

			SetRigWeight(1f);

			if (input.shoot)
			{
				anim.SetBool("Shoot", true);
			}
			else
			{
				anim.SetBool("Shoot", false);
			}
		}
		else
		{
			AimControll(false);
			SetRigWeight(0f);
			anim.SetBool("Shoot", false);
		}
	}

	private void AimControll(bool isCheck)
	{
		aimCam.gameObject.SetActive(isCheck);
		aimImage.SetActive(isCheck);
		controller.isAimMove = isCheck;
		anim.SetLayerWeight(1, isCheck ? 1f : 0f);

	}

	public void Reload()
	{
		//Debug.Log("Reload");
		controller.isReload = false;
		SetRigWeight(1f);
		anim.SetLayerWeight(1, 0f);
	}

	private void SetRigWeight(float weight)
	{
		aimRig.weight = weight;
		handRig.weight = weight;

	}
}
