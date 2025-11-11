using UnityEngine;
using UnityEditor;

public class MeshTools
{
    // MeshFilter 컴포넌트를 우클릭했을 때 메뉴에 "Bake Mesh Rotation" 항목을 추가합니다.
    [MenuItem("CONTEXT/MeshFilter/Bake Mesh Rotation")]
    public static void BakeMeshRotation(MenuCommand command)
    {
        MeshFilter meshFilter = (MeshFilter)command.context;
        Mesh sourceMesh = meshFilter.sharedMesh;

        if (sourceMesh == null)
        {
            Debug.LogError("MeshFilter에 메시가 없습니다.");
            return;
        }

        // --- 1. 새로운 메시 인스턴스 생성 ---
        // 원본 메시를 직접 수정하면 프로젝트의 모든 캡슐이 바뀌는 대참사가 일어나므로 복사본을 만듭니다.
        Mesh newMesh = new Mesh();
        newMesh.name = sourceMesh.name + "_Rotated";

        // --- 2. 원본 메시 데이터 복사 ---
        Vector3[] vertices = sourceMesh.vertices;
        int[] triangles = sourceMesh.triangles;
        Vector2[] uv = sourceMesh.uv;
        Vector3[] normals = sourceMesh.normals;
        Color[] colors = sourceMesh.colors;

        // --- 3. 정점(Vertex) 회전시키기 ---
        // 원하는 회전값을 설정합니다. 캡슐의 축에 따라 X, Y, Z를 조절하세요.
        Quaternion rotation = Quaternion.Euler(0, 90, 90);

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = rotation * vertices[i]; // 각 정점의 위치를 회전
            normals[i] = rotation * normals[i];   // 노멀(법선)도 같이 회전시켜야 조명이 깨지지 않습니다.
        }

        // --- 4. 복사본 메시에 수정된 데이터 적용 ---
        newMesh.vertices = vertices;
        newMesh.triangles = triangles;
        newMesh.uv = uv;
        newMesh.normals = normals;
        newMesh.colors = colors;

        newMesh.RecalculateBounds(); // 메시의 경계를 재계산

        // --- 5. 수정된 메시를 새 에셋 파일로 저장 ---
        string path = EditorUtility.SaveFilePanelInProject("Save Rotated Mesh", newMesh.name + ".asset", "asset", "메시를 저장할 경로를 선택하세요.");
        if (string.IsNullOrEmpty(path)) return; // 취소하면 중단

        AssetDatabase.CreateAsset(newMesh, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("회전된 메시가 여기에 저장되었습니다: " + path);

        // --- 6. 현재 오브젝트의 메시를 새로 생성된 메시로 교체 ---
        meshFilter.sharedMesh = newMesh;

        EditorUtility.SetDirty(meshFilter.gameObject); // 변경사항 저장
    }
}