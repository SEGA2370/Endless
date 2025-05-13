using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EndlessLevelHandler : MonoBehaviour
{
    [SerializeField] GameObject[] sectionPrefabs;

    GameObject[] sectionsPool;

    GameObject[] sections = new GameObject[10];

    Transform playerCarTransform;

    WaitForSeconds waitFor100ms = new WaitForSeconds(0.1f);

    const float sectionLength = 26;

    void Awake()
    {
        sectionsPool = new GameObject[sectionPrefabs.Length * 2]; // Only 2 copies per type, not 20
    }

    IEnumerator Start()
    {
        // Wait until the Player is active
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player") != null);
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;

        int prefabIndex = 0;

        for (int i = 0; i < sectionsPool.Length; i++)
        {
            sectionsPool[i] = Instantiate(sectionPrefabs[prefabIndex]);
            sectionsPool[i].SetActive(false);

            prefabIndex++;
            if (prefabIndex > sectionPrefabs.Length - 1)
                prefabIndex = 0;
        }

        for (int i = 0; i < sections.Length; i++)
        {
            GameObject randomSection = GetRandomSectionFromPool();
            randomSection.transform.position = new Vector3(sectionsPool[i].transform.position.x, -10, i * sectionLength);
            randomSection.SetActive(true);
            sections[i] = randomSection;
        }

        StartCoroutine(UpdateLessOftenCO());
    }

    IEnumerator UpdateLessOftenCO()
    {
        while (true)
        {
            UpdateSectionPositions();
            yield return waitFor100ms;
        }
    }

    void UpdateSectionPositions()
    {
        for (int i = 0; i < sections.Length; i++)
        {
            //Check if section is too far behind.
            if (sections[i].transform.position.z - playerCarTransform.position.z < -sectionLength)
            {
                //Store the position of the section and disable it
                Vector3 lastSectionPosition = sections[i].transform.position;
                sections[i].SetActive(false);

                //Get new section & enable it   & move it forward
                sections[i] = GetRandomSectionFromPool();

                //mOVE THE NEW SECTION INTO PLACE AND ACTIVE IT
                sections[i].transform.position = new Vector3(lastSectionPosition.x, -10, lastSectionPosition.z + sectionLength * sections.Length);
                sections[i].SetActive(true);
            }
        }
    }

    GameObject GetRandomSectionFromPool()
    {
        int tries = sectionsPool.Length;
        int startIndex = Random.Range(0, sectionsPool.Length);

        for (int i = 0; i < tries; i++)
        {
            int index = (startIndex + i) % sectionsPool.Length;
            if (sectionsPool[index] != null && !sectionsPool[index].activeInHierarchy)
                return sectionsPool[index];
        }

        // If no available section, instantiate a new one
        int prefabIndex = Random.Range(0, sectionPrefabs.Length);
        GameObject newSection = Instantiate(sectionPrefabs[prefabIndex]);
        newSection.SetActive(false);

        // Optionally expand the pool
        Array.Resize(ref sectionsPool, sectionsPool.Length + 1);
        sectionsPool[sectionsPool.Length - 1] = newSection;

        return newSection;
    }
}
