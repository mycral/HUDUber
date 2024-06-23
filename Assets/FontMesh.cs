using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

/// <summary>
/// 使用Unity自带的font来显示文本
/// 这些文本模型写入到Mesh中，然后引用字体Material显示
/// </summary>
/// 
[ExecuteAlways]
public class FontMesh : MonoBehaviour
{
    public Font m_kFont;
    public string m_kStrText;
    private Mesh m_kMesh;
    public int m_iFontSize;
    public FontStyle m_eFontStyle;

    public List<Vector3> m_kVertexList;
    public List<int> m_kTriangleList;
    public List<Vector4> m_kUV1List;
    public List<Color> m_kVertexColorList;

    public int m_iRawVertexCount;

    public Vector2 m_kEffectDistance;
    public Color m_kEffectColor = Color.black;
    public Color m_kFontColor = Color.white;


    public Bounds m_kTextBounds;

    public Sprite m_kTestSprite;
    public Color m_kSpriteColor = Color.white;

    public Material m_UberMaterial;


    void RefreshText()
    {
        if(m_kFont == null)
        {
            return;
        }   
        if(m_kStrText == null)
        {
            return;
        }

        if(m_kMesh == null)
        {
            m_kMesh = new Mesh();
            m_kVertexList = new List<Vector3>();
            m_kTriangleList = new List<int>();
            m_kUV1List = new List<Vector4>();
            m_kVertexColorList = new List<Color>();
        }
        m_kMesh.Clear();
        m_kVertexList.Clear();
        m_kTriangleList.Clear();
        m_kUV1List.Clear();
        m_kVertexColorList.Clear();

        //build text vertexes into mesh
        List<Vector4> kTextRects = new List<Vector4>();
        
        m_kFont.RequestCharactersInTexture(m_kStrText, m_iFontSize, m_eFontStyle);

        RebuildMesh();

        GetComponent<MeshRenderer>().sharedMaterial = m_UberMaterial;
        GetComponent<MeshFilter>().sharedMesh = m_kMesh;
    }
    
    void RebuildMesh()
    {
        string str = m_kStrText;
        Font font = m_kFont;
        Mesh mesh = m_kMesh;
        Vector3 pos = Vector3.zero;

        for (int i=0; i<str.Length;i++)
        {
            // Get character rendering information from the font
            CharacterInfo ch;
            bool retx = font.GetCharacterInfo(str[i], out ch, m_iFontSize, m_eFontStyle);

            m_kVertexList.Add(pos + new Vector3 (ch.minX, ch.maxY, 0));
            m_kVertexList.Add(pos + new Vector3(ch.maxX, ch.maxY, 0));
            m_kVertexList.Add(pos + new Vector3(ch.maxX, ch.minY, 0));
            m_kVertexList.Add(pos + new Vector3(ch.minX, ch.minY, 0));

            m_kUV1List.Add(ch.uvTopLeft);
            m_kUV1List.Add(ch.uvTopRight);
            m_kUV1List.Add(ch.uvBottomRight);
            m_kUV1List.Add(ch.uvBottomLeft);

            m_kVertexColorList.Add(m_kFontColor);
            m_kVertexColorList.Add(m_kFontColor);
            m_kVertexColorList.Add(m_kFontColor);
            m_kVertexColorList.Add(m_kFontColor);

            // Advance character position
            pos += new Vector3(ch.advance, 0,0);
        }

        m_kTextBounds = new Bounds(m_kVertexList[0],Vector3.zero);

        for(int i = 0; i < m_kVertexList.Count; i++)
        {
            m_kTextBounds.Encapsulate(m_kVertexList[i]);
        }


        m_iRawVertexCount = m_kVertexColorList.Count;
        AddBackGround(new Rect(0, 0, 1, 1));
        ApplyShadow(m_kEffectColor, new Vector2(m_kEffectDistance.x, -m_kEffectDistance.y));
        ApplyShadow(m_kEffectColor, new Vector2(-m_kEffectDistance.x, -m_kEffectDistance.x));
        ApplyShadow(m_kEffectColor, new Vector2(m_kEffectDistance.x, m_kEffectDistance.x));
        ApplyShadow(m_kEffectColor, new Vector2(-m_kEffectDistance.x, m_kEffectDistance.x));
        TextCoreMoveToBack();
        GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_SpriteTex", m_kTestSprite.texture);
        GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MainTex", m_kFont.material.mainTexture);
        ApplyTriangles();
        mesh.SetVertices(m_kVertexList);
        mesh.SetUVs(0, m_kUV1List);
        mesh.SetColors(m_kVertexColorList);
        mesh.SetTriangles(m_kTriangleList,0);

        mesh.RecalculateBounds();
    }

    public void ApplyTriangles()
    {
        int charCount = m_kVertexList.Count / 4;

        for (int i = 0; i < charCount; i++)
        {
            m_kTriangleList.Add(4 * i + 0);
            m_kTriangleList.Add(4 * i + 1);
            m_kTriangleList.Add(4 * i + 2);

            m_kTriangleList.Add(4 * i + 0);
            m_kTriangleList.Add(4 * i + 2);
            m_kTriangleList.Add(4 * i + 3);
        }
    }

    public void TextCoreMoveToBack()
    {
        for (int i = 0; i < m_iRawVertexCount; i++)
        {
            m_kVertexList.Add(m_kVertexList[i]);
            m_kUV1List.Add(m_kUV1List[i]);
            m_kVertexColorList.Add(m_kVertexColorList[i]);
        }

        m_kVertexList.RemoveRange(0, m_iRawVertexCount);
        m_kUV1List.RemoveRange(0, m_iRawVertexCount);
        m_kVertexColorList.RemoveRange(0, m_iRawVertexCount);
    }
    public void ApplyShadow(Color color,Vector2 offset)
    {
        for(int i=0;i < m_iRawVertexCount; i++)
        {
            m_kVertexList.Add(m_kVertexList[i] + new Vector3(offset.x, offset.y, 0));
            m_kUV1List.Add(m_kUV1List[i]);
            m_kVertexColorList.Add(color);
        }
    }


    public void AddBackGround(Rect rect)
    {
        Vector3 max = m_kTextBounds.max + new Vector3(5, 5, 0);
        Vector3 min = m_kTextBounds.min - new Vector3(5,5,0);

        m_kVertexList.Add(new Vector3(min.x, max.y, 0));
        m_kVertexList.Add(new Vector3(max.x, max.y, 0));
        m_kVertexList.Add(new Vector3(max.x, min.y, 0));
        m_kVertexList.Add(new Vector3(min.x, min.y, 0));

        m_kUV1List.Add(new Vector4(0,0,rect.x,rect.y + rect.height));
        m_kUV1List.Add(new Vector4(0,0,rect.x + rect.width,rect.y + rect.height));
        m_kUV1List.Add(new Vector4(0,0,rect.x + rect.width,rect.y));
        m_kUV1List.Add(new Vector4(0,0,rect.x ,rect.y));

        m_kVertexColorList.Add(m_kSpriteColor);
        m_kVertexColorList.Add(m_kSpriteColor);
        m_kVertexColorList.Add(m_kSpriteColor);
        m_kVertexColorList.Add(m_kSpriteColor);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RefreshText();
    }
}
