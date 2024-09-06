using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : Weapon
{
    public int poolSize = 10;

    [SerializeField] GameObject projetile;

    public int shootingFrame = 0;

    List<GameObject> instances = new List<GameObject>();

    public override void Start()
    {
        base.Start();

        FillObjectPool();
    }

    public override void Attack()
    {
        StartCoroutine(UseRangedWeapon());

        base.Attack();
    }

    IEnumerator UseRangedWeapon()
    {
        yield return new WaitForSeconds(keyframeDuration * shootingFrame / AnimationSpeed());

        Shoot();
    }

    void FillObjectPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateInstance();
        }
    }

    GameObject InstatiateObject(Vector2 pos)
    {
        foreach (GameObject instance in instances)
        {
            if (!instance.activeInHierarchy)
            {                
                return ActivateInstance(instance, pos);
            }
        }

        GameObject newInstance = CreateInstance();

        return ActivateInstance(newInstance, pos);
    }

    GameObject CreateInstance()
    {
        GameObject instance = Instantiate(projetile, transform);
        instance.SetActive(false);

        instances.Add(instance);

        return instance;
    }

    GameObject ActivateInstance(GameObject instance, Vector2 pos)
    {
        instance.transform.position = pos;
        instance.transform.rotation = transform.rotation;
        instance.transform.SetParent(null);
        instance.SetActive(true);

        return instance;
    }


    public void Shoot()
    {
        Audio.instance.PlayOneShot(Audio.Sound.attack, .20f);

        InstatiateObject(new Vector2(transform.position.x + UserInput.instance.attackDirInput.x, transform.position.y + UserInput.instance.attackDirInput.y));
    }

    private void OnDisable()
    {
        foreach (GameObject instance in instances)
        {
            Destroy(instance);
        }
    }
}
