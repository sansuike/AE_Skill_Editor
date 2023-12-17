using System;
using UnityEngine;
using ARPG_AE_JOKER.SkillEditor;

public class TestPlayMode : MonoBehaviour
{
    [SerializeField] private Animation_Controller animation_Controller;
    [SerializeField] private Transform modelTransfrom;
    [SerializeField] private Skill_Player skill_Player;
    [SerializeField] private SkillConfig[] skillConfig;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Rigidbody body;
    [SerializeField] private AnimationClip idleClip;
    [SerializeField] private Transform[] weapons;
    [SerializeField] private SkillConfig[] katana_open;
    [SerializeField] private SkillConfig[] grateSword_open;
    [Range(0, 9.8f), SerializeField] private float grativ;
    [SerializeField, Range(1, -1)] private int useRigidBodyOrCharactorContrller;
    [SerializeField] private SkillEditorSceneCamera skillEditorSceneCamera;

    private void Awake()
    {
        skill_Player.Init(animation_Controller, modelTransfrom);
        animation_Controller.Init();
        body = GetComponentInParent<Rigidbody>();
        skillEditorSceneCamera = GameObject.Find("SkillEditorSceneCamera").GetComponent<SkillEditorSceneCamera>();
        skillEditorSceneCamera.focus = GameObject.Find("CameraPos").transform;
    }


    private void Start()
    {
        animation_Controller.PlaySingleAnimation(idleClip);


        foreach (var item in weapons)
        {
            item.gameObject.SetActive(false);
        }

        animation_Controller.AddAnimationEventListener("StartInput", () =>
        {
            Debug.Log("StartInput");
        });

        animation_Controller.AddAnimationEventListener("MixAnimation", () =>
        {
            Debug.Log("MixAnimation");
        });
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            for (int i = 0; i < 9; i++)
            {
                if (Input.GetKeyDown((KeyCode)256 + i))
                {
                    if (skillConfig.Length > i)
                        skill_Player.PlaySkill(skillConfig[i], SkillEnd, RootMotionEvent);
                }
            }
        }
    }

    private void SkillEnd()
    {
        print("End");
        animation_Controller.PlaySingleAnimation(idleClip);
        body.velocity = Vector3.zero;
    }

    public void RootMotionEvent(Vector3 pos, Quaternion rot, Vector3 velocity)
    {
        pos.y -= grativ * Time.deltaTime;
        if (useRigidBodyOrCharactorContrller < 0)
            body.velocity = velocity;
        else
            characterController.Move(pos);
        modelTransfrom.transform.rotation *= rot;
    }
}