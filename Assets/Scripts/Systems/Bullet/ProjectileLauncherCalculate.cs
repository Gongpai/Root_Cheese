using UnityEditor;
using UnityEngine;

namespace GDD
{
    public class ProjectileLauncherCalculate : MonoBehaviour
    {
        [SerializeField] private bool reset_time = true;
        [SerializeField] private GameObject target;
        private float _gravity = 9.8f;
        private float _distance = 0;
        private float _maxHeight = 20;
        private float _velocity = 0;
        private float _angle = 0;
        private float time = 0;
        private void OnDrawGizmos()
        {
            if (target != null)
            {
                InitialVelocity();
                LaunchAngle();
                
                Handles.color = Color.red;
                
                float vrot = Mathf.Rad2Deg * _angle;
                //Debug.Log("Angle : " + vrot);
                Quaternion rot = Quaternion.AngleAxis(vrot, Vector3.left);
                //Debug.Log("ROTTTT : " + rot);
                Handles.ArrowHandleCap(0, transform.position, rot, 0.5f, EventType.Repaint);

                Quaternion trot = Quaternion.LookRotation(target.transform.position, Vector3.up);
                Handles.color = Color.blue;
                Handles.ArrowHandleCap(0, transform.position, trot, 0.5f, EventType.Repaint);
                
                Quaternion tyrot = Quaternion.LookRotation(new Vector3(target.transform.position.x, 0, target.transform.position.z), Vector3.up);
                Handles.color = Color.yellow;
                Handles.ArrowHandleCap(0, transform.position, tyrot, 0.5f, EventType.Repaint);

                Quaternion rrot = tyrot * rot;
                Handles.color = Color.green;
                Handles.ArrowHandleCap(0, transform.position, rrot, 0.5f, EventType.Repaint);
                
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(transform.position, 0.05f);

                if (reset_time)
                    time = 1;
                
                if (time <= 0)
                {
                    GameObject Grenade = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    Grenade.name = "Grenade";
                    Grenade.GetComponent<Collider>().isTrigger = true;
                    Grenade.transform.localScale = Vector3.one * 0.25f;
                    Rigidbody rig = Grenade.AddComponent<Rigidbody>();
                    rig.mass = 0.25f;
                    rig.AddForce(GetDirection() * GetForce(), ForceMode.Impulse);
                    Debug.Log("Rigiboby Dir : " + GetDirection() + " Forece : " + GetForce());
                    
                    time = 1;

                    Destroy(Grenade, 5);
                }
                else
                {
                    time -= Time.deltaTime;
                }
            }
        }

        public Vector3 GetDirection()
        {
            float vrot = Mathf.Rad2Deg * _angle;
            Quaternion rot = Quaternion.AngleAxis(vrot, Vector3.left);
            Quaternion looktorot = Quaternion.LookRotation(new Vector3(target.transform.position.x, 0, target.transform.position.z), Vector3.up);

            Quaternion finalRot = looktorot * rot;
            return finalRot * Vector3.forward;
        }

        public float GetForce()
        {
            return _velocity * _angle;
        }
        
        private void InitialVelocity()
        {
            //Find Distance
            _distance = Vector3.Distance(transform.position, target.transform.position);
            _maxHeight = _distance;
            
            // Time Max Height
            float time_maxHeight = Mathf.Sqrt(2 * _maxHeight / _gravity);
            
            // Find Start Velocity X,Y
            float x_velocity = _distance / time_maxHeight;
            float y_velocity = _maxHeight / time_maxHeight - 0.5f * _gravity * time_maxHeight;
            
            // Find Start Velocity
            _velocity = Mathf.Sqrt(x_velocity * x_velocity + y_velocity * y_velocity);
        }

        private void LaunchAngle()
        {
            //Find Start Velocity Y
            float y_velocity = _maxHeight / Mathf.Sqrt(2 * _maxHeight / _gravity);
            
            //Find Launch Angle
            _angle = Mathf.Atan(y_velocity / _distance);
        }
    }
}