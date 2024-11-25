using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	// Start is called before the first frame update

	public static GameManager Instance;

	[Header("Bullet")]
	[SerializeField]
	private Transform bulletPoint;
	[SerializeField]
	private GameObject bulletObj;
	[SerializeField]
	private float maxShootDelay = 0.2f;
	[SerializeField]
	private Text bulletText;
	private float currentShootDelay = 0.2f;
	private int maxBullet = 30;
	private int currentBullet = 0;

	[Header("Weapon FX")]
	[SerializeField]
	private GameObject weaponFlashFX;
	[SerializeField]
	private Transform bulletCasePoint;
	[SerializeField]
	private GameObject bulletCaseFX;
	[SerializeField]
	private Transform weaponClipPoint;
	[SerializeField]
	private GameObject WeaponClipFX;


	void Start()
	{
		Instance = this;
		currentShootDelay = 0f;
		InitBullet();

	}

	// Update is called once per frame
	void Update()
	{
		bulletText.text = currentBullet + " / " + maxBullet;
	}

	public void Shooting(Vector3 targetPosition)
	{
		currentShootDelay += Time.deltaTime;

		if (currentShootDelay < maxShootDelay || currentBullet <= 0) return;

		currentShootDelay = 0f;

		currentBullet--;

		Instantiate(weaponFlashFX, bulletPoint);
		Instantiate(bulletCaseFX, bulletCasePoint);
		Vector3 aim = (targetPosition - bulletPoint.position).normalized;
		Instantiate(bulletObj, bulletPoint.position, Quaternion.LookRotation(aim, Vector3.up));


	}

	public void ReroadClip()
	{
		Instantiate(WeaponClipFX, weaponClipPoint);
	}

	public void fillMaxAmmo()
	{
		InitBullet();
	}

	private void InitBullet()
	{
		currentBullet = maxBullet;
	}
}
