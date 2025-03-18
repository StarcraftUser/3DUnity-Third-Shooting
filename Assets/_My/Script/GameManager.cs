using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
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

	[Header("Enemy")]
	[SerializeField]
	private GameObject[] spawnPoint;

	[Header("BGM")]
	[SerializeField]
	private AudioClip bgmSound;
	private AudioSource BGM;

	private PlayableDirector cut;
	public bool isReady = true;

	void Start()
	{
		Instance = this;
		currentShootDelay = 0f;

		cut = GetComponent<PlayableDirector>();
		cut.Play();

		InitBullet();


	}

	// Update is called once per frame
	void Update()
	{
		bulletText.text = currentBullet + " / " + maxBullet;
	}

	public void Shooting(Vector3 targetPosition, Enemy enemy, AudioSource weaponSound, AudioClip shootingSound)
	{
		currentShootDelay += Time.deltaTime;

		if (currentShootDelay < maxShootDelay || currentBullet <= 0) return;

		currentShootDelay = 0f;

		currentBullet--;

		weaponSound.clip = shootingSound;
		weaponSound.Play();

		Vector3 aim = (targetPosition - bulletPoint.position).normalized;

		//Instantiate(weaponFlashFX, bulletPoint);
		GameObject flashFX = PoolManager.instance.ActivateObj(1);
		SetObjPosition(flashFX, bulletPoint);
		flashFX.transform.rotation = Quaternion.LookRotation(aim, Vector3.up);

		//Instantiate(bulletCaseFX, bulletCasePoint);
		GameObject caseFX = PoolManager.instance.ActivateObj(2);
		SetObjPosition(caseFX, bulletCasePoint);

		//Instantiate(bulletObj, bulletPoint.position, Quaternion.LookRotation(aim, Vector3.up));
		GameObject prefabToSpawn = PoolManager.instance.ActivateObj(0);
		SetObjPosition(prefabToSpawn, bulletPoint);
		prefabToSpawn.transform.rotation = Quaternion.LookRotation(aim, Vector3.up);

		/*
		//Raycast
		if (enemy != null && enemy.enemyCurrentHP > 0)
		{
			enemy.enemyCurrentHP -= 1;
			Debug.Log("enemy HP : " + enemy.enemyCurrentHP);
		}
		*/

	}

	public void ReroadClip()
	{
		//Instantiate(WeaponClipFX, weaponClipPoint);

		GameObject clipFX = PoolManager.instance.ActivateObj(3);
		SetObjPosition(clipFX, weaponClipPoint);

		InitBullet();
	}

	public void fillMaxAmmo()
	{
		InitBullet();
	}

	private void InitBullet()
	{
		currentBullet = maxBullet;
	}

	private void SetObjPosition(GameObject obj, Transform targetTransform)
	{
		obj.transform.position = targetTransform.position;
	}

	IEnumerator EnemySpawn()
	{
		//Instantiate(enemy, spawnPoint[Random.Range(0, spawnPoint.Length)].transform.position, Quaternion.identity);
		GameObject enemy = PoolManager.instance.ActivateObj(4);
		SetObjPosition(enemy, spawnPoint[Random.Range(0, spawnPoint.Length)].transform);

		yield return new WaitForSeconds(3f);

		StartCoroutine(EnemySpawn());
	}

	private void PlayBGMSound()
	{
		BGM = GetComponent<AudioSource>();
		BGM.clip = bgmSound;
		BGM.loop = true;
		BGM.Play();
	}

	public void StartGame()
	{
		isReady = false;
		PlayBGMSound();
		StartCoroutine(EnemySpawn());
	}
}
