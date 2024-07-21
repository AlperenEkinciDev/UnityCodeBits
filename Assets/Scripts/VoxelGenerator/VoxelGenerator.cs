using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoxelGenerator
{

    public static VoxelData GenerateTerrainVoxel(float[,,] noiseMap3D, float blockSize, float threshold, int chunkSize, int chunkNum)
    {

        int depth = noiseMap3D.GetLength(2);
        int height = noiseMap3D.GetLength(1);
        int width = noiseMap3D.GetLength(0);

        int totalChunkCountDepth = depth / chunkSize;
        int totalChunkCountWidth = width / chunkSize;

        int startOffsetDepth = (chunkNum / totalChunkCountWidth) * chunkSize;
        int startOffsetWidth = (chunkNum % totalChunkCountWidth) * chunkSize;

        int endPointtDepth = startOffsetDepth + chunkSize;
        int endPointWidth = startOffsetWidth + chunkSize;

        int vertexCount = 0;

        for (int z = startOffsetDepth; z < endPointtDepth; z++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = startOffsetWidth; x < endPointWidth; x++)
                {
                    //**********************************//
                    //Width
                    if (x > 0 && x < width - 1 && noiseMap3D[x, y, z] >= threshold)
                    {
                        if (noiseMap3D[x - 1, y, z] < threshold)
                        {
                            vertexCount += 4;
                        }
                        if (noiseMap3D[x + 1, y, z] < threshold)
                        {
                            vertexCount += 4;
                        }
                    }
                    else if (noiseMap3D[x, y, z] >= threshold)
                    {
                        if (x == 0)
                        {
                            vertexCount += 4;
                            if (noiseMap3D[x + 1, y, z] < threshold)
                            {
                                vertexCount += 4;
                            }
                        }
                        else if (x == width - 1)
                        {
                            vertexCount += 4;
                            if (noiseMap3D[x - 1, y, z] < threshold)
                            {
                                vertexCount += 4;
                            }
                        }
                    }
                    //Width//
                    //**********************************//
                    //Height//
                    if (y > 0 && y < height - 1 && noiseMap3D[x, y, z] >= threshold)
                    {
                        if (noiseMap3D[x, y - 1, z] < threshold)
                        {
                            vertexCount += 4;
                        }
                        if (noiseMap3D[x, y + 1, z] < threshold)
                        {
                            vertexCount += 4;
                        }
                    }
                    else if (noiseMap3D[x, y, z] >= threshold)
                    {
                        if (y == 0)
                        {
                            vertexCount += 4;
                            if (noiseMap3D[x, y + 1, z] < threshold)
                            {
                                vertexCount += 4;
                            }
                        }
                        else if (y == height - 1)
                        {
                            vertexCount += 4;
                            if (noiseMap3D[x, y - 1, z] < threshold)
                            {
                                vertexCount += 4;
                            }
                        }
                    }
                    //Height//
                    //**********************************//
                    //Depth//
                    if (z > 0 && z < depth - 1 && noiseMap3D[x, y, z] >= threshold)
                    {
                        if (noiseMap3D[x, y, z - 1] < threshold)
                        {
                            vertexCount += 4;
                        }
                        if (noiseMap3D[x, y, z + 1] < threshold)
                        {
                            vertexCount += 4;
                        }
                    }
                    else if (noiseMap3D[x, y, z] >= threshold)
                    {
                        if (z == 0)
                        {
                            vertexCount += 4;
                            if (noiseMap3D[x, y, z + 1] < threshold)
                            {
                                vertexCount += 4;
                            }
                        }
                        else if (z == depth - 1)
                        {
                            vertexCount += 4;
                            if (noiseMap3D[x, y, z - 1] < threshold)
                            {
                                vertexCount += 4;
                            }
                        }
                    }
                    //Depth//
                    //**********************************//
                }
            }
        }//Counts vertices (very inefficient)



        VoxelData voxelData = new VoxelData(vertexCount);
        for (int z = startOffsetDepth; z < endPointtDepth; z++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = startOffsetWidth; x < endPointWidth; x++)
                {
                    float widthStart = -(width - 1) / 2f;
                    float heightStart = -height;
                    float depthStart = -(depth - 1) / 2f;
                    Vector3 faceOffset = new Vector3(widthStart, heightStart, depthStart);


                    //**********************************//
                    //Width
                    if (x > 0 && x < width - 1 && noiseMap3D[x, y, z] >= threshold)
                    {
                        if (noiseMap3D[x - 1, y, z] < threshold)
                        {
                            voxelData.AddFace(x, y, z, -2, blockSize, faceOffset);
                        }
                        if (noiseMap3D[x + 1, y, z] < threshold)
                        {
                            voxelData.AddFace(x, y, z, 2, blockSize, faceOffset);
                        }
                    }
                    else if(noiseMap3D[x, y, z] >= threshold)
                    {
                        if (x == 0)
                        {
                            voxelData.AddFace(x, y, z, -2, blockSize, faceOffset);

                            if (noiseMap3D[x + 1, y, z] < threshold)
                            {
                                voxelData.AddFace(x, y, z, 2, blockSize, faceOffset);
                            }
                        }
                        else if(x == width -1)
                        {
                            voxelData.AddFace(x, y, z, 2, blockSize, faceOffset);

                            if (noiseMap3D[x - 1, y, z] < threshold)
                            {
                                voxelData.AddFace(x, y, z, -2, blockSize, faceOffset);
                            }
                        }
                    }
                    //Width//
                    //**********************************//
                    //Height//
                    if (y > 0 && y < height - 1 && noiseMap3D[x, y, z] >= threshold)
                    {
                        if (noiseMap3D[x, y - 1, z] < threshold)
                        {
                            voxelData.AddFace(x, y, z, -3, blockSize, faceOffset);
                        }
                        if (noiseMap3D[x, y + 1, z] < threshold)
                        {
                            voxelData.AddFace(x, y, z, 3, blockSize, faceOffset);
                        }
                    }
                    else if (noiseMap3D[x, y, z] >= threshold)
                    {
                        if (y == 0)
                        {
                            voxelData.AddFace(x, y, z, -3, blockSize, faceOffset);

                            if (noiseMap3D[x, y + 1, z] < threshold)
                            {
                                voxelData.AddFace(x, y, z, 3, blockSize, faceOffset);
                            }
                        }
                        else if (y == height - 1)
                        {
                            voxelData.AddFace(x, y, z, 3, blockSize, faceOffset);

                            if (noiseMap3D[x, y - 1, z] < threshold)
                            {
                                voxelData.AddFace(x, y, z, -3, blockSize, faceOffset);
                            }
                        }
                    }
                    //Height//
                    //**********************************//
                    //Depth//
                    if (z > 0 && z < depth - 1 && noiseMap3D[x, y, z] >= threshold)
                    {
                        if (noiseMap3D[x, y, z - 1] < threshold)
                        {
                            voxelData.AddFace(x, y, z, -1, blockSize, faceOffset);
                        }
                        if (noiseMap3D[x, y, z + 1] < threshold)
                        {
                            voxelData.AddFace(x, y, z, +1, blockSize, faceOffset);
                        }
                    }
                    else if (noiseMap3D[x, y, z] >= threshold)
                    {
                        if (z == 0)
                        {
                            voxelData.AddFace(x, y, z, -1, blockSize, faceOffset);

                            if (noiseMap3D[x, y, z + 1] < threshold)
                            {
                                voxelData.AddFace(x, y, z, +1, blockSize, faceOffset);
                            }
                        }
                        else if(z == depth - 1)
                        {
                            voxelData.AddFace(x, y, z, +1, blockSize, faceOffset);

                            if (noiseMap3D[x, y, z - 1] < threshold)
                            {
                                voxelData.AddFace(x, y, z, -1, blockSize, faceOffset);
                            }
                        }
                    }
                    //Depth//
                    //**********************************//
                }
            }
        }
        return voxelData;
    }

    public static float[,,] ManipulateVoxelAtPosition(RaycastHit hit, float[,,] noiseMap3D, float objectSize, int detailMultiplier, float deletionRadius, float value)
    {
        Vector3 position = hit.point - hit.normal * objectSize / 2f;

        int depth = noiseMap3D.GetLength(2);
        int height = noiseMap3D.GetLength(1);
        int width = noiseMap3D.GetLength(0);

        if (width * height * depth != 0)
        {
            float widthStart = -width / 2f;
            float heightStart = -height;
            float depthStart = -depth / 2f;
            float singleVoxelSize = objectSize / detailMultiplier;
            deletionRadius = deletionRadius * detailMultiplier;

            int hitCordX = (int)(position.x / singleVoxelSize - widthStart);
            int hitCordY = (int)(position.y / singleVoxelSize - heightStart);
            int hitCordZ = (int)(position.z / singleVoxelSize - depthStart);


            int startX = hitCordX - (int)(deletionRadius - 1);
            int startY = hitCordY - (int)(deletionRadius - 1);
            int startZ = hitCordZ - (int)(deletionRadius - 1);

            for (int z = 0; z < 2 * deletionRadius + 1; z++)
            {
                for (int y = 0; y < 2 * deletionRadius + 1; y++)
                {
                    for (int x = 0; x < 2 * deletionRadius + 1; x++)
                    {
                        if (startX + x > -1 && startX + x < width && startY + y > -1 && startY + y < height && startZ + z > -1 && startZ + z < depth) {
                            if (Mathf.Sqrt(Mathf.Pow((hitCordX - startX - x), 2) + Mathf.Pow((hitCordY - startY - y), 2) + Mathf.Pow((hitCordZ - startZ - z), 2)) < deletionRadius) {
                                noiseMap3D[startX + x, startY + y, startZ + z] = value;
                            }
                        }
                    }
                }
            }
        }

        return noiseMap3D;
    }
}

public class VoxelData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int verticesCounter = 0;
    int trianglesCounter = 0;
    bool useFlatShading;

    public VoxelData(int vertexCount)
    {
        vertices = new Vector3[vertexCount];
        uvs = new Vector2[vertexCount];
        triangles = new int[vertexCount * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[trianglesCounter] = a;
        triangles[trianglesCounter + 1] = b;
        triangles[trianglesCounter + 2] = c;

        trianglesCounter += 3;
    }

    public void AddFace(int x, int y, int z, int facing, float blockSize, Vector3 faceOffset) //1:z, -1:-z, 2:x, -2:-x, 3:y, -3:-y
    {
        float planeDistanceMultiplier = 1f;

        if (Mathf.Abs(facing) == 1)
        {
            if (Mathf.Sign(facing) == -1) {
                AddTriangle(verticesCounter, verticesCounter + 1, verticesCounter + 2);
                AddTriangle(verticesCounter + 1, verticesCounter, verticesCounter + 3);
            }
            else
            {
                AddTriangle(verticesCounter + 1, verticesCounter, verticesCounter + 2);
                AddTriangle(verticesCounter, verticesCounter + 1, verticesCounter + 3);
            }

            vertices[verticesCounter] = (faceOffset + new Vector3(x, y, z) + Mathf.Sign(facing) / 2 * new Vector3(-planeDistanceMultiplier, planeDistanceMultiplier, 1)) * blockSize;
            verticesCounter++;
            vertices[verticesCounter] = (faceOffset + new Vector3(x, y, z) + Mathf.Sign(facing) / 2 * new Vector3(planeDistanceMultiplier, -planeDistanceMultiplier, 1)) * blockSize;
            verticesCounter++;
            vertices[verticesCounter] = (faceOffset + new Vector3(x, y, z) + Mathf.Sign(facing) / 2 * new Vector3(-planeDistanceMultiplier, -planeDistanceMultiplier, 1)) * blockSize;
            verticesCounter++;
            vertices[verticesCounter] = (faceOffset + new Vector3(x, y, z) + Mathf.Sign(facing) / 2 * new Vector3(planeDistanceMultiplier, planeDistanceMultiplier, 1)) * blockSize;
            verticesCounter++;
        }
        else if (Mathf.Abs(facing) == 2)
        {
            if (Mathf.Sign(facing) == 1)
            {
                AddTriangle(verticesCounter, verticesCounter + 1, verticesCounter + 2);
                AddTriangle(verticesCounter + 1, verticesCounter, verticesCounter + 3);
            }
            else
            {
                AddTriangle(verticesCounter + 1, verticesCounter, verticesCounter + 2);
                AddTriangle(verticesCounter, verticesCounter + 1, verticesCounter + 3);
            }

            vertices[verticesCounter] = (faceOffset + new Vector3(x, y, z) + Mathf.Sign(facing) / 2 * new Vector3(1, planeDistanceMultiplier, -planeDistanceMultiplier)) * blockSize;
            verticesCounter++;
            vertices[verticesCounter] = (faceOffset + new Vector3(x, y, z) + Mathf.Sign(facing) / 2 * new Vector3(1, -planeDistanceMultiplier, planeDistanceMultiplier)) * blockSize;
            verticesCounter++;
            vertices[verticesCounter] = (faceOffset + new Vector3(x, y, z) + Mathf.Sign(facing) / 2 * new Vector3(1, -planeDistanceMultiplier, -planeDistanceMultiplier)) * blockSize;
            verticesCounter++;
            vertices[verticesCounter] = (faceOffset + new Vector3(x, y, z) + Mathf.Sign(facing) / 2 * new Vector3(1, planeDistanceMultiplier, planeDistanceMultiplier)) * blockSize;
            verticesCounter++;
        }
        else if (Mathf.Abs(facing) == 3)
        {
            if (Mathf.Sign(facing) == 1)
            {
                AddTriangle(verticesCounter, verticesCounter + 1, verticesCounter + 2);
                AddTriangle(verticesCounter + 1, verticesCounter, verticesCounter + 3);
            }
            else
            {
                AddTriangle(verticesCounter + 1, verticesCounter, verticesCounter + 2);
                AddTriangle(verticesCounter, verticesCounter + 1, verticesCounter + 3);
            }

            vertices[verticesCounter] = (faceOffset + new Vector3(x, y, z) + Mathf.Sign(facing)/ 2 * new Vector3(-planeDistanceMultiplier, 1, planeDistanceMultiplier)) * blockSize;
            verticesCounter++;
            vertices[verticesCounter] = (faceOffset + new Vector3(x, y, z) + Mathf.Sign(facing) / 2 * new Vector3(planeDistanceMultiplier, 1, -planeDistanceMultiplier)) *blockSize;
            verticesCounter++;
            vertices[verticesCounter] = (faceOffset + new Vector3(x, y, z) + Mathf.Sign(facing) / 2 * new Vector3(-planeDistanceMultiplier, 1, -planeDistanceMultiplier)) *blockSize;
            verticesCounter++;
            vertices[verticesCounter] = (faceOffset + new Vector3(x, y, z) + Mathf.Sign(facing) / 2 * new Vector3(planeDistanceMultiplier, 1, planeDistanceMultiplier)) *blockSize;
            verticesCounter++;
        }
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        //mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.name = "GeneratedVoxel - " + mesh.vertexCount.ToString();
        return mesh;
    }
}