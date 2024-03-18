using System;
using System.Collections.Generic;
using GDD.Util;
using UnityEngine;

namespace GDD
{
    public class ProjectileReflectionLines : MonoBehaviour
    {
        [SerializeField] private Mesh m_lineMesh;
        [SerializeField] private Material[] m_lineMats;
        [SerializeField] private Mesh m_arrowMesh;
        [SerializeField] private Material[] m_arrowMats;
        private List<GameObject> _lines = new List<GameObject>();
        private List<GameObject> _arrows = new List<GameObject>();
        private GameObject _spawnLine;
        private GameObject _spawnArrow;
        private Transform _line_parent;
        private Vector3 default_scale_arrow = new Vector3(0.75f, 0.2f, 1);
        private Vector3 default_scale_line = new Vector3(0.1f, 0.05f, 0.25f);
        
        public bool is_null
        {
            get
            {
                return _lines.Count <= 0;
            }
        }
        
        private void Start()
        {
            CreateParent();
            
            //arrow
            if (m_arrowMesh == null)
                _spawnArrow = Resources.Load<GameObject>("PRBullet/PR_Arrow");
            else
                _spawnArrow = CreateArrowObject(m_arrowMesh, m_arrowMats);
            
            //line
            _spawnLine = GameObject.CreatePrimitive(PrimitiveType.Cube);
            
            //Mat
            Material linemat = Resources.Load<Material>("Materials/LineMat");
            m_lineMats =  new Material[]{linemat};
            m_arrowMats = new Material[]{linemat};
            
            _spawnLine.transform.parent = transform;
            _spawnLine.name = "PRLine_Object";
            _spawnLine.layer = LayerMask.NameToLayer("Bullet");
            
            _spawnLine.transform.localScale = default_scale_line;
            _spawnLine.SetActive(false);
        }

        private GameObject CreateArrowObject(Mesh mesh, Material[] materials)
        {
            GameObject arrow = new GameObject("Arrow Object");
            arrow.AddComponent<MeshFilter>().sharedMesh = mesh;
            arrow.AddComponent<MeshRenderer>().sharedMaterials = materials;
            arrow.transform.position = Vector3.zero * -1000;

            return arrow;
        }

        private void CreateLine(Vector3 start, Vector3 end)
        {
            GameObject line = Instantiate(_spawnLine);
            Destroy(line.GetComponent<Collider>());
            line.transform.parent = _line_parent;
            line.transform.localPosition = Vector3.zero;
            line.GetComponent<MeshRenderer>().sharedMaterials = m_lineMats;
            line.SetActive(true);
            
            //Line------------------------------------------------
            //Scale
            float dis = Vector3.Distance(start, end);
            line.transform.localScale = new Vector3(line.transform.localScale.x * 0.5f, line.transform.localScale.y, dis - 0.5f);
            
            //Position
            Vector3 pos = VectorUtil.GetVectorDistance(start, end, (dis / 2) - (0.25f / 2));
            /*
            Vector3 pos = start - end;
            pos = Vector3.Normalize(pos);
            pos *= dis / (2 + 0.25f);
            pos += end;
            */
            line.transform.position = pos;
            
            //Rotation
            Quaternion rot_line = Quaternion.LookRotation(end - start, Vector3.up);
            line.transform.rotation = rot_line;
            
            //Arrow------------------------------------------------
            _arrows.Add(CreateArrow(VectorUtil.GetVectorDistance(start, end, (0.25f / 2)), rot_line));
            
            _lines.Add(line);
        }

        private GameObject CreateArrow(Vector3 pos, Quaternion rot)
        {
            GameObject arrow = Instantiate(_spawnArrow);
            arrow.transform.parent = _line_parent;
            arrow.transform.localScale = default_scale_arrow;
            arrow.transform.localPosition = Vector3.zero;
            arrow.name = "Arrow";
            arrow.layer = LayerMask.NameToLayer("Bullet");
            Destroy(_spawnLine.GetComponent<Collider>());
            arrow.GetComponent<MeshRenderer>().sharedMaterials = m_arrowMats;
            arrow.transform.position = pos;
            arrow.transform.rotation = rot;

            return arrow;
        }
        
        public void AddLine(Vector3 start, Vector3 end)
        {
            CreateLine(start, end);
        }

        public void UpdateLinePosition(Vector3 start, Vector3 end, int index)
        {
            //Scale
            float dis = Vector3.Distance(start, end);
            _lines[index].transform.localScale = new Vector3(_lines[index].transform.localScale.x, _lines[index].transform.localScale.y, dis - 0.5f);
            
            //Position
            Vector3 pos = VectorUtil.GetVectorDistance(start, end, (dis / 2) - (0.25f / 2));
            _lines[index].transform.position = pos;
            
            //Rotation
            Quaternion rot_line = Quaternion.LookRotation(end - start, Vector3.up);
            _lines[index].transform.rotation = rot_line;
            
            //Arrow
            _arrows[index].transform.position = VectorUtil.GetVectorDistance(start, end, (0.25f / 2));
            _arrows[index].transform.rotation = rot_line;
        }

        public void ClearLine()
        {
            Destroy(_line_parent.gameObject);
            _lines = new List<GameObject>();
            _arrows = new List<GameObject>();
            CreateParent();
        }

        private void CreateParent()
        {
            GameObject spawn_parant = new GameObject("Spawn Line");
            spawn_parant.transform.parent = transform;
            spawn_parant.transform.localPosition = Vector3.zero;
            _line_parent = spawn_parant.transform;
        }
    }
}