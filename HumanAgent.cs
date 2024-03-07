using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using static UnityEngine.GraphicsBuffer;
using System.Collections.Generic;

public class HumanAgent : Agent
{
    public TextAsset dataText;

    public Animator targetHuman_lmaSource;

    public Animator targetHuman_aniSource;
    public Animator botHuman;
    public GameObject target;

    private string[] animations =
    {
        // bow
        "bow_active", // 0
        "bow_angry", // 1
        "bow_childish", // 2
        "bow_chimpira", // 3
        "bow_feminine", // 4
        "bow_giant", // 5
        "bow_happy", // 6
        "bow_masculinity", // 7
        "bow_musical", // 8
        "bow_normal", // 9
        "bow_not_confident", // 10
        "bow_old", // 11
        "bow_proud", // 12
        "bow_sad", // 13
        "bow_tired", // 14
        // bye
        "bye_active", // 0
        "bye_angry", // 1
        "bye_childish", // 2
        "bye_chimpira", // 3
        "bye_feminine", // 4
        "bye_giant", // 5
        "bye_happy", // 6
        "bye_masculinity", // 7
        "bye_musical", // 8
        "bye_normal", // 9
        "bye_not_confident", // 10
        "bye_old", // 11
        "bye_proud", // 12
        "bye_sad", // 13
        "bye_tired", // 14
        // byebye
        "byebye_active", // 0
        "byebye_angry", // 1
        "byebye_childish", // 2
        "byebye_chimpira", // 3
        "byebye_feminine", // 4
        "byebye_giant", // 5
        "byebye_happy", // 6
        "byebye_masculinity", // 7
        "byebye_musical", // 8
        "byebye_normal", // 9
        "byebye_not_confident", // 10
        "byebye_old", // 11
        "byebye_proud", // 12
        "byebye_sad", // 13
        "byebye_tired", // 14
        // dash
        "dash_active", // 0
        "dash_angry", // 1
        "dash_childish", // 2
        "dash_chimpira", // 3
        "dash_feminine", // 4
        "dash_giant", // 5
        "dash_happy", // 6
        "dash_masculinity", // 7
        "dash_musical", // 8
        "dash_normal", // 9
        "dash_not_confident", // 10
        "dash_old", // 11
        "dash_proud", // 12
        "dash_sad", // 13
        "dash_tired", // 14
        // guide
        "guide_active", // 0
        "guide_angry", // 1
        "guide_childish", // 2
        "guide_chimpira", // 3
        "guide_feminine", // 4
        "guide_giant", // 5
        "guide_happy", // 6
        "guide_masculinity", // 7
        "guide_musical", // 8
        "guide_normal", // 9
        "guide_not_confident", // 10
        "guide_old", // 11
        "guide_proud", // 12
        "guide_sad", // 13
        "guide_tired", // 14
        // run
        "run_active", // 0
        "run_angry", // 1
        "run_childish", // 2
        "run_chimpira", // 3
        "run_feminine", // 4
        "run_giant", // 5
        "run_happy", // 6
        "run_masculinity", // 7
        "run_musical", // 8
        "run_normal", // 9
        "run_not_confident", // 10
        "run_old", // 11
        "run_proud", // 12
        "run_sad", // 13
        "run_tired", // 14
         // walk
        "walk_active_1", // 0
        "walk_angry_1", // 1
        "walk_childish_1", // 2
        "walk_chimpira_1", // 3
        "walk_feminine_1", // 4
        "walk_giant_1", // 5
        "walk_happy_1", // 6
        "walk_masculinity_1", // 7
        "walk_musical_1", // 8
        "walk_normal_1", // 9
        "walk_not_confident_1", // 10
        "walk_old_1", // 11
        "walk_proud_1", // 12
        "walk_sad_1", // 13
        "walk_tired_1", // 14
        "walk_active_2", // 0
        "walk_angry_2", // 1
        "walk_childish_2", // 2
        "walk_chimpira_2", // 3
        "walk_feminine_2", // 4
        "walk_giant_2", // 5
        "walk_happy_2", // 6
        "walk_masculinity_2", // 7
        "walk_musical_2", // 8
        "walk_normal_2", // 9
        "walk_not_confident_2", // 10
        "walk_old_2", // 11
        "walk_proud_2", // 12
        "walk_sad_2", // 13
        "walk_tired_2", // 14
        // other
        "call_normal_1",
        "call_normal_2",
        "kick_normal",
        "punch_normal_1",
        "punch_normal_2",
        "respond_normal",
        "slash_normal_1",
        "slash_normal_2"
    };

    HumanBodyBones[] bones = { 
        HumanBodyBones.RightLowerArm,
        HumanBodyBones.RightUpperArm,
        HumanBodyBones.LeftUpperArm,
        HumanBodyBones.LeftLowerArm,
        HumanBodyBones.Hips,
        HumanBodyBones.LeftShoulder,
        HumanBodyBones.RightShoulder,
        HumanBodyBones.Spine,
        HumanBodyBones.Chest,
        HumanBodyBones.Neck,
        HumanBodyBones.Head,
        HumanBodyBones.RightHand,
        HumanBodyBones.LeftHand};

    private float positionNormalization = 20f;
    private float rotationNormalization = 360f;

    Dictionary<string, UserData> dic;
    List<string> keys = new List<string>();

    void Start()
    {
        dic = new Dictionary<string,UserData>();

        string[] lines = dataText.text.Split("\r\n");
        for(int i = 1; i < lines.Length; i++)
        {
            string[] elements = lines[i].Split(',');
            if(elements.Length > 1) { 
                UserData tmp1 = null;
                dic.TryGetValue(elements[11], out tmp1);
                if(tmp1 != null)
                {
                    tmp1.AddVals(int.Parse(elements[4]), int.Parse(elements[5]), int.Parse(elements[6]), int.Parse(elements[7]), int.Parse(elements[8]));
                }
                else
                {
                    dic.Add(elements[11], new UserData(elements[11], int.Parse(elements[4]), int.Parse(elements[5]), int.Parse(elements[6]), int.Parse(elements[7]), int.Parse(elements[8])));
                }
            }
        }

        foreach (var key in dic.Keys)
        {
            keys.Add(key);
        }

    }

    class UserData
    {
        string aniName;
        List<int> O = new List<int>();
        List<int> C = new List<int>();
        List<int> E = new List<int>();
        List<int> A = new List<int>();
        List<int> N = new List<int>();

        public UserData(string aniName, int v0, int v1, int v2, int v3, int v4)
        {
            this.aniName = aniName;
            O.Add(v0);
            C.Add(v1);
            E.Add(v2);
            A.Add(v3);
            N.Add(v4);
        }

        public void AddVals(int v0, int v1, int v2, int v3, int v4)
        {
            O.Add(v0);
            C.Add(v1);
            E.Add(v2);
            A.Add(v3);
            N.Add(v4);
        }

        public int GetOneE()
        {
            return E[Random.Range(0, E.Count)];
        }

        public float GetAVGE()
        {
            float val = 0;
            for (int i = 0; i < E.Count; i++)
            {
                val += E[i];
            }
            val = val / ((float)E.Count);

            return val;
        }
    }

    bool sameCategoryAnims = false;

    public void SetRandomAnimation()
    {
        int index_ani = Random.Range(15, 15 + 14 * 2); // keys.Count);
        string key_ani = animations[index_ani];

        int index_lma = Random.Range(15, 15 + 14 * 2); // keys.Count);
        string key_lma = animations[index_lma];

        sameCategoryAnims = ((index_ani / 14) == (index_lma / 14)) && ((index_ani / 14) < 8);

        targetHuman_aniSource.Play(key_ani, 0, 0);
        targetHuman_lmaSource.Play(key_lma, 0, 0);
        
        UserData tmp = null;
        dic.TryGetValue(key_ani, out tmp);
        E_ani = tmp.GetAVGE();

        dic.TryGetValue(key_lma, out tmp);
        E_lma = tmp.GetAVGE();
    }

    float E_ani;
    float E_lma;

    public override void OnEpisodeBegin()
    {
        SetRandomAnimation();
        // targetHuman.Play("Idle", 0, Random.value);
        /*
        target.transform.localPosition = new Vector3(
            Random.value * 1.5f - 0.7f,
            Random.value * 1.5f - 0.7f,
            Random.value * 0.4f
        );*/
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(E_ani); // 1
        sensor.AddObservation(E_lma); // 1
        sensor.AddObservation(E_lma - E_ani); // 1

        float time_now = targetHuman_aniSource.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
        sensor.AddObservation(time_now); // 1

        for (int i = 0; i < bones.Length; i++) // 13 bone * 7 = 91
        {
            sensor.AddObservation(targetHuman_aniSource.GetBoneTransform(bones[i]).position / positionNormalization); // * 3
            sensor.AddObservation(targetHuman_aniSource.GetBoneTransform(bones[i]).localRotation); // * 4
        }

        // sensor.AddObservation(target.transform.localPosition / positionNormalization);
        
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        int index = 0;
        
        for (int i = 0; i < bones.Length; i++)
        {
            botHuman.GetBoneTransform(bones[i]).localRotation =
            new Quaternion(
                actionBuffers.ContinuousActions[index],
                actionBuffers.ContinuousActions[index + 1],
                actionBuffers.ContinuousActions[index + 2],
                actionBuffers.ContinuousActions[index + 3]);

            index+= 4;
        }

        //float totalDistance = 0;
        float totalRotation = 0;

        for (int i = 0; i < bones.Length; i++)
        {
           /* totalDistance += Vector3.Distance(
                 botHuman.GetBoneTransform(bones[i]).position,
                 targetHuman_aniSource.GetBoneTransform(bones[i]).position) / positionNormalization;*/

            totalRotation += Quaternion.Angle(
                botHuman.GetBoneTransform(bones[i]).localRotation,
                targetHuman_aniSource.GetBoneTransform(bones[i]).localRotation) / rotationNormalization;
        }

        float lma_difference;

        float target_hand_distance = Vector3.Distance(targetHuman_lmaSource.GetBoneTransform(HumanBodyBones.LeftHand).position, targetHuman_lmaSource.GetBoneTransform(HumanBodyBones.RightHand).position);
        float bot_hand_distance = Vector3.Distance(botHuman.GetBoneTransform(HumanBodyBones.LeftHand).position, botHuman.GetBoneTransform(HumanBodyBones.RightHand).position);

        float target_leg_distance = Vector3.Distance(targetHuman_lmaSource.GetBoneTransform(HumanBodyBones.LeftFoot).position, targetHuman_lmaSource.GetBoneTransform(HumanBodyBones.RightFoot).position);
        float bot_leg_distance = Vector3.Distance(botHuman.GetBoneTransform(HumanBodyBones.LeftFoot).position, botHuman.GetBoneTransform(HumanBodyBones.RightFoot).position);

        float target_lhand_head_distance = Vector3.Distance(targetHuman_lmaSource.GetBoneTransform(HumanBodyBones.LeftHand).position, targetHuman_lmaSource.GetBoneTransform(HumanBodyBones.Head).position);
        float bot_lhand_head_distancee = Vector3.Distance(botHuman.GetBoneTransform(HumanBodyBones.LeftHand).position, botHuman.GetBoneTransform(HumanBodyBones.Head).position);

        float target_rhand_head_distance = Vector3.Distance(targetHuman_lmaSource.GetBoneTransform(HumanBodyBones.RightHand).position, targetHuman_lmaSource.GetBoneTransform(HumanBodyBones.Head).position);
        float bot_rhand_head_distancee = Vector3.Distance(botHuman.GetBoneTransform(HumanBodyBones.RightHand).position, botHuman.GetBoneTransform(HumanBodyBones.Head).position);

        if (sameCategoryAnims)
        {
            lma_difference = 0;
        }
        else
        {
            lma_difference = Mathf.Abs(target_hand_distance - bot_hand_distance)
                + Mathf.Abs(target_leg_distance - bot_leg_distance)
                + Mathf.Abs(target_lhand_head_distance - bot_lhand_head_distancee)
                + Mathf.Abs(target_rhand_head_distance - bot_rhand_head_distancee);
        }
        

        /*
        float distToSphere = Vector3.Distance(
                 botHuman.GetBoneTransform(HumanBodyBones.RightHand).position,
                 target.transform.position) / positionNormalization;
        */

        SetReward(1 - (lma_difference + totalRotation)); // + distToSphere * 10f); + totalDistance

        // Reached target
        /*if (totalDistance < 5f)
        {
            SetReward(1 - (totalDistance/ 5f));

            /*if((totalDistance * totalRotation) == 0)
            {
                EndEpisode();
            }
        }*/
    }

}
