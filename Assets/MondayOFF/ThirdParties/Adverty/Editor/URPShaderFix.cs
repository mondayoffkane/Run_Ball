/*
Is is necessary?
*/

// using UnityEditor;
// using UnityEditor.PackageManager;

// namespace MondayOFF
// {
//     public class URPShaderFix : AssetPostprocessor
//     {
//         private static UnityEditor.PackageManager.Requests.ListRequest packageListRequest;
//         private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAs
// #if UNITY_2021_2_OR_NEWER
//             , bool didDomainReload
// #endif
//             )
//         {
//             // Get URP version
//             packageListRequest = Client.List();
//             EditorApplication.update -= WaitForPackageList;
//             EditorApplication.update += WaitForPackageList;
//         }

//         private static void WaitForPackageList()
//         {
//             if (packageListRequest.IsCompleted)
//             {
//                 EditorApplication.update -= WaitForPackageList;
//                 FixShader();
//             }
//         }

//         private static void FixShader()
//         {
//             var packageList = packageListRequest.Result;
//             var isURP = false;

//             foreach (var pkg in packageList)
//             {
//                 if (pkg.name == "com.unity.render-pipelines.universal")
//                 {
//                     isURP = true;
//                     break;
//                 }
//             }

//             // target file is AdSpriteLitShader.shader in Resources folder
//             var targetGUIDs = AssetDatabase.FindAssets("AdSpriteLitShader");
//             if (targetGUIDs.Length == 0)
//             {
//                 return;
//             }
//             foreach (var targetGUID in targetGUIDs)
//             {
//                 var targetPath = AssetDatabase.GUIDToAssetPath(targetGUID);
//                 // read file
//                 var shaderLines = System.IO.File.ReadAllLines(targetPath);
//                 for (var i = 0; i < shaderLines.Length; i++)
//                 {
//                     if (shaderLines[i].Contains("//Uncomment to use with URP 12.1.7 (Unity 2021.3.0+)"))
//                     {
//                         if (isURP)
//                         {
//                             // uncomment the line below
//                             shaderLines[i + 1] = shaderLines[i + 1].Replace("//", "");
//                             // and comment the line below that
//                             if (!shaderLines[i + 2].Contains("//"))
//                             {
//                                 shaderLines[i + 2] = "//" + shaderLines[i + 2];
//                             }
//                         }
//                         else
//                         {

//                             // uncomment the line below
//                             shaderLines[i + 2] = shaderLines[i + 1].Replace("//", "");
//                             // and comment the line below that
//                             if (!shaderLines[i + 1].Contains("//"))
//                             {
//                                 shaderLines[i + 1] = "//" + shaderLines[i + 1];
//                             }
//                         }
//                     }
//                 }
//                 // write file
//                 System.IO.File.WriteAllLines(targetPath, shaderLines);
//             }

//             packageListRequest = null;
//         }
//     }
// }

