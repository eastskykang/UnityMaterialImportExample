/*
 * MIT License
 * 
 * Copyright (c) 2019 Dongho Kang
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.IO;
using UnityEngine;

namespace UnityMeshImportExample.MaterialImporter
{
    public class MaterialImporter
    {
        public static Material Load(string albedoPath="", string normalPath="", string roughnessPath="", string metallicPath="", string heightPath="", string occlusionPath="")
        {
            var mat = new Material(Shader.Find("Standard"));

            Texture2D albedo = null;
            if (!string.IsNullOrEmpty(albedoPath))
            {
                albedo = new Texture2D(2,2);
                byte[] byteArray = File.ReadAllBytes(albedoPath);
                bool isLoaded = albedo.LoadImage(byteArray);
                if (!isLoaded)
                {
                    throw new Exception("Cannot find texture file: " + albedoPath);
                }
            }

            Texture2D normal = null;
            if (!string.IsNullOrEmpty(normalPath))
            {
                normal = new Texture2D(2,2);
                byte[] byteArray = File.ReadAllBytes(normalPath);
                bool isLoaded = normal.LoadImage(byteArray);
                if (!isLoaded)
                {
                    throw new Exception("Cannot find texture file: " + normalPath);
                }
            }
            
            Texture2D height = null;
            if (!string.IsNullOrEmpty(heightPath))
            {
                height = new Texture2D(2,2);
                byte[] byteArray = File.ReadAllBytes(heightPath);
                bool isLoaded = height.LoadImage(byteArray);
                if (!isLoaded)
                {
                    throw new Exception("Cannot find texture file: " + heightPath);
                }
            }

            Texture2D occlusion = null;
            if (!string.IsNullOrEmpty(occlusionPath))
            {
                occlusion = new Texture2D(2,2);
                byte[] byteArray = File.ReadAllBytes(occlusionPath);
                bool isLoaded = occlusion.LoadImage(byteArray);
                if (!isLoaded)
                {
                    throw new Exception("Cannot find texture file: " + heightPath);
                }
            }

            // Roughness and Metallic should be treated together
            Texture2D metAndRgh = null;
            if (!string.IsNullOrEmpty(metallicPath) && !string.IsNullOrEmpty(roughnessPath))
            {
                // Both are specified
                metAndRgh = new Texture2D(2, 2);
                byte[] metByteArray = File.ReadAllBytes(metallicPath);
                byte[] rghByteArray = File.ReadAllBytes(roughnessPath);
                bool isLoaded = metAndRgh.LoadImage(metByteArray);
                if (!isLoaded)
                {
                    throw new Exception("Cannot find texture file: " + metallicPath);
                }
            }
            else if (!string.IsNullOrEmpty(metallicPath))
            {
                // Only Metallic is specified
                metAndRgh = new Texture2D(2, 2);
                byte[] byteArray = File.ReadAllBytes(metallicPath);
                bool isLoaded = metAndRgh.LoadImage(byteArray);
                if (!isLoaded)
                {
                    throw new Exception("Cannot find texture file: " + metallicPath);
                }
            }
            else if (!string.IsNullOrEmpty(roughnessPath))
            {
                // Only Roughness is specified
            }

            // Albedo
            if (albedo != null)
            {
                mat.SetTexture("_MainTex", albedo);
            }

            // Normal
            if (normal != null)
            {
                mat.EnableKeyword("_NORMALMAP");
                mat.SetTexture("_BumpMap", normal);
            }

            // Height
            if (height != null)
            {
                mat.EnableKeyword("_PARALLAXMAP");
                mat.SetTexture("_ParallaxMap", height);
            }
            
            // Occlusion 
            if (occlusion != null)
            {
                mat.EnableKeyword("_OCCLUSIONMAP");
                mat.SetTexture("_OcclusionMap", occlusion);
            }
            
            // Metallic and Roughness
            if (metAndRgh != null)
            {
                mat.EnableKeyword("_METALLICGLOSSMAP");
                mat.SetTexture("_MetallicGlossMap", metAndRgh);
            }

            return mat;
        }
    }
}