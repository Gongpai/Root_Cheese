using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GDD
{
    public class ProjectileLauncherCalculate : MonoBehaviour
    {
        [SerializeField] private bool reset_time = true;
        [SerializeField] private GameObject plance;
        [SerializeField] private Transform target;
        [Range(1.0f, 25.0f)] public float TargetRadius;
        [Range(20.0f, 75.0f)] public float LaunchAngle;
        [Range(0.0f, 10.0f)] public float TargetHeightOffsetFromGround;

        private Vector3 random_point;
        private GameObject spawnObject;
        public bool RandomizeHeightOffset;
        private float time = 0;

        void Start()
        {
            spawnObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spawnObject.name = "Bullets";
            spawnObject.AddComponent<Rigidbody>();
            spawnObject.transform.parent = transform;
            spawnObject.transform.localPosition = Vector3.zero;
            spawnObject.SetActive(false);

            random_point = Random.insideUnitSphere * 10;
        }

        //For Debug Only
        void Update()
        {
            if (time <= 0)
            {
                time = 10;
                SetNewTarget();
                Launch();
                transform.position = random_point;
                plance.transform.position = random_point;
                random_point = Random.insideUnitSphere * 10;
            }
            else
            {
                time -= Time.deltaTime;
            }
        }

        
        private void OnDrawGizmos()
        {
            Handles.color = Color.yellow;
            Quaternion rot = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
            Handles.ArrowHandleCap(0, transform.position, rot, 1f, EventType.Repaint);
            
            Handles.color = Color.red;
            Handles.ArrowHandleCap(0, target.position, Quaternion.Euler(Vector3.left * 90), 1f, EventType.Repaint);
        }
        float GetPlatformOffset()
        {
            float platformOffset = 0.0f;

            foreach (Transform childTransform in target.GetComponentsInChildren<Transform>())
            {
                if (childTransform.name == "Mark")
                {
                    platformOffset = childTransform.localPosition.y;
                    break;
                }
            }

            return platformOffset;
        }

        void Launch()
        {
            GameObject bullet = Instantiate(spawnObject);
            bullet.SetActive(true);
            bullet.transform.position = transform.position;
            Rigidbody rig = bullet.GetComponent<Rigidbody>();
            rig.velocity = Vector3.zero;

            Vector3 projectileXZPos = new Vector3(bullet.transform.position.x, bullet.transform.position.y, bullet.transform.position.z);
            Vector3 targetXZPos = new Vector3(target.position.x, bullet.transform.position.y, target.position.z);
            
            // rotate the object to face the target
            transform.LookAt(targetXZPos);
            bullet.transform.rotation = transform.rotation;

            float R = Vector3.Distance(projectileXZPos, targetXZPos);
            float G = Physics.gravity.y;
            float tanAlpha = Mathf.Tan(LaunchAngle * Mathf.Deg2Rad);
            float H = (target.position.y + GetPlatformOffset()) - bullet.transform.position.y;

            float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
            float Vy = tanAlpha * Vz;

            Vector3 localVelocity = new Vector3(0f, Vy, Vz);
            Vector3 globalVelocity = bullet.transform.TransformDirection(localVelocity);

            rig.velocity = globalVelocity;
        }

        void SetNewTarget()
        {
            Transform targetTF = target.GetComponent<Transform>();

            Vector3 rotationAxis = Vector3.up;
            float randomAngle = Random.Range(0.0f, 360.0f);
            Vector3 randomVectorOnGroundPlane = Quaternion.AngleAxis(randomAngle, rotationAxis) * Vector3.right;

            float heightOffset = (RandomizeHeightOffset ? Random.Range(0.2f, 1.0f) : 1.0f) * TargetHeightOffsetFromGround;
            float aboveOrBelowGround = (Random.Range(0.0f, 1.0f) > 0.5f ? 1.0f : -1.0f);
            Vector3 heightOffsetVector = new Vector3(0, heightOffset, 0) * aboveOrBelowGround;
            Vector3 randomPoint = randomVectorOnGroundPlane * TargetRadius + heightOffsetVector;

            target.SetPositionAndRotation(randomPoint, targetTF.rotation);
        }
    }
}