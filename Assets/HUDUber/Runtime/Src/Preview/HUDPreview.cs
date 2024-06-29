using HUDUber.Prefab;
using UnityEditor;
using UnityEngine;


namespace HUDUber
{
    [ExecuteAlways]
    public class HUDPreview : MonoBehaviour
    {
        //遍历 HUDPrefabScriptPanel 极其子节点，生成基于Panel为根节点的HUD预览
        [ContextMenu("PreviewHUD")]
        public void PreviewHUD()
        {
            Check();

            HUDPrefabScriptPanel panelPrefab = GetComponent<HUDPrefabScriptPanel>();
            m_kPanel = panelPrefab.Build() as Panel;
        }

        public void RefreshMesh()
        {
            if (m_kPanel == null)
                return;

            using var autoRelMB = VertexHelper.GetAutoReleaseMeshBuffer();
            var mb = autoRelMB.target;
            m_kPanel.RebuildSize();
            m_kPanel.RebuildLayout();
            m_kMesh.Clear();
            m_kPanel.BuildMesh(mb);
            mb.FillMesh(m_kMesh);
        }

        private Panel m_kPanel;
        private MeshBuffer m_kMeshBuffer;
        private Mesh m_kMesh;
        private MeshRenderer m_kMeshRenderer;
        private MeshFilter m_kMeshFilter;

        private void Check()
        {
            m_kMeshFilter = GetComponent<MeshFilter>();
            if (m_kMeshFilter == null)
            {
                m_kMeshFilter = gameObject.AddComponent<MeshFilter>();
            }
            m_kMeshRenderer = GetComponent<MeshRenderer>();
            if (m_kMeshRenderer == null)
            {
                m_kMeshRenderer = gameObject.AddComponent<MeshRenderer>();
            }
            if(m_kMeshRenderer.sharedMaterial == null)
            {
                m_kMeshRenderer.sharedMaterial = LoadAssetByGUID<Material>("423010bd702263b45b66b6b3fa01bacd");
            }
            m_kMesh = m_kMeshFilter.sharedMesh;
            if (m_kMesh == null)
            {
                m_kMesh = new Mesh();
                m_kMeshFilter.sharedMesh = m_kMesh;
            }
        }

        private void Update()
        {
            if(m_kPanel == null)
            {
                PreviewHUD();
            }

            RefreshMesh();
        }

        private void OnDisable()
        {
            VertexHelper.ReleaseStaticNativeDatas();
        }

        public static T LoadAssetByGUID<T>(string guid) where T: UnityEngine.Object
        {
           string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path == null)
                return null;
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }
}