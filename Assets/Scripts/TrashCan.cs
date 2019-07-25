﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrashCan : MonoBehaviour
{
    private LevelManager levelManager;
    private string garbageTag = "Garbage";

    public AudioClip correctAudio;

    public AudioClip wrongAudio;

    private AudioSource audioSource;

    private int type;

    public void SetType(int type)
    {
        this.type = type;
    }

    private void Start()
    {
        levelManager = LevelManager.instance;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(garbageTag))
        {
            bool isMatch = false;
            Garbage garbage = collision.gameObject.GetComponent<Garbage>();

            if (type == garbage.type)
            {
                isMatch = true;
                if (type == (int)GarbageData.GarbageType.Mysterious)
                {
                    levelManager.OnBuff(garbage);
                }
                else
                {
                    levelManager.RemoveGarbage(garbage.gameObject);
                }

                audioSource.PlayOneShot(correctAudio);
            }
            else
            {
                levelManager.AddNotes(garbage.garbageData); // 添加分类失败后的语句
                levelManager.GetComponent<LevelInit>().SubStar();
                levelManager.ThrowGarbage(garbage.gameObject);
                SpitGarbage(garbage);

                audioSource.PlayOneShot(wrongAudio);
            }
            levelManager.OnCollectingGarbage(isMatch);
        }
    }

    private void SpitGarbage(Garbage garbage)
    {
        garbage.SpitByTrashCan(transform);
    }

}
