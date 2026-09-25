using System;
using System.Collections.Generic;
using TinhKiemAuto;
using TinhKiemAuto.Models;

// Token: 0x02000006 RID: 6
public class FindPath
{
	// Token: 0x0600000F RID: 15 RVA: 0x00002128 File Offset: 0x00000328
	private static Scene GetSceneById(int sceneId)
	{
		foreach (Scene scene in FindPath.scenes)
		{
			if (scene.sceneId == sceneId)
			{
				return scene;
			}
		}
		return null;
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002188 File Offset: 0x00000388
	private static SceneInfo FindSceneInsideList(Scene scene)
	{
		foreach (SceneInfo sceneInfo in FindPath.scenesList)
		{
			if (sceneInfo.scene.sceneId == scene.sceneId)
			{
				return sceneInfo;
			}
		}
		return null;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x000021F0 File Offset: 0x000003F0
	private static void ReadData()
	{
		string[] array = LoadFile.LoadFileWithDecrypt(Global.DataPath + "\\16.dat").Split(new char[]
		{
			'\n'
		});
		for (int i = 1; i < array.Length; i++)
		{
			string[] array2 = array[i].Trim().Split(new char[]
			{
				'\t'
			});
			int sceneId = int.Parse(array2[0]);
			string sceneName = array2[1];
			Scene item = new Scene(sceneId, sceneName);
			FindPath.scenes.Add(item);
		}
		string[] array3 = LoadFile.LoadFileWithDecrypt(Global.DataPath + "\\17.dat").Split(new char[]
		{
			'\n'
		});
		for (int j = 1; j < array3.Length; j++)
		{
			string[] array4 = array3[j].Trim().Split(new char[]
			{
				'\t'
			});
			int sceneId2 = int.Parse(array4[0]);
			int sceneId3 = int.Parse(array4[2]);
			string npcName = array4[4];
			int teleportationCost = int.Parse(array4[5]);
			int num = int.Parse(array4[6]);
			int num2 = int.Parse(array4[7]);
			Scene sceneById = FindPath.GetSceneById(sceneId2);
			Scene sceneById2 = FindPath.GetSceneById(sceneId3);
			SceneInfo sceneInfo = FindPath.FindSceneInsideList(sceneById);
			if (sceneInfo == null)
			{
				sceneInfo = new SceneInfo(sceneById);
				FindPath.scenesList.Add(sceneInfo);
			}
			sceneInfo.AddNearScene(sceneById2, num, num2);
			sceneInfo.AddTeleportNPC(new TeleportNPC(npcName, teleportationCost, num, num2));
		}
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002350 File Offset: 0x00000550
	public static List<SceneInfo> GetPathsList(int fromSceneId, int toSceneId)
	{
		if (FindPath.scenesList.Count <= 0)
		{
			FindPath.ReadData();
		}
		Queue<SceneInfo> queue = new Queue<SceneInfo>();
		SceneInfo[] array = new SceneInfo[1000];
		bool[] array2 = new bool[1000];
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = true;
			array[i] = null;
		}
		SceneInfo sceneInfo = FindPath.FindSceneInsideList(FindPath.GetSceneById(fromSceneId));
		SceneInfo sceneInfo2 = FindPath.FindSceneInsideList(FindPath.GetSceneById(toSceneId));
		queue.Enqueue(sceneInfo);
		array[fromSceneId] = null;
		array2[sceneInfo.scene.sceneId] = false;
		while (queue.Count > 0)
		{
			SceneInfo sceneInfo3 = queue.Dequeue();
			foreach (NearScene nearScene in sceneInfo3.nearScenes)
			{
				if (array2[nearScene.scene.sceneId])
				{
					array2[nearScene.scene.sceneId] = false;
					SceneInfo sceneInfo4 = FindPath.FindSceneInsideList(nearScene.scene);
					if (sceneInfo4 != null)
					{
						queue.Enqueue(sceneInfo4);
						array[sceneInfo4.scene.sceneId] = sceneInfo3;
						if (sceneInfo4 == sceneInfo2)
						{
							List<SceneInfo> list = new List<SceneInfo>();
							SceneInfo sceneInfo5 = sceneInfo4;
							list.Add(sceneInfo5);
							while (array[sceneInfo5.scene.sceneId] != sceneInfo)
							{
								sceneInfo5 = array[sceneInfo5.scene.sceneId];
								list.Add(sceneInfo5);
							}
							list.Add(sceneInfo);
							return list;
						}
					}
				}
			}
		}
		return null;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000024E8 File Offset: 0x000006E8
	public static int GetPortalX(SceneInfo fromScene, SceneInfo toScene)
	{
		foreach (NearScene nearScene in fromScene.nearScenes)
		{
			if (nearScene.scene == toScene.scene)
			{
				return nearScene.nearScenePortalX;
			}
		}
		return -1;
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002550 File Offset: 0x00000750
	public static int GetPortalY(SceneInfo fromScene, SceneInfo toScene)
	{
		foreach (NearScene nearScene in fromScene.nearScenes)
		{
			if (nearScene.scene == toScene.scene)
			{
				return nearScene.nearScenePortalY;
			}
		}
		return -1;
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000025B8 File Offset: 0x000007B8
	public static TeleportNPC GetTeleportNPC(SceneInfo fromScene, SceneInfo toScene)
	{
		foreach (NearScene nearScene in fromScene.nearScenes)
		{
			if (nearScene.scene == toScene.scene)
			{
				foreach (TeleportNPC teleportNPC in fromScene.teleportNPCs)
				{
					if (nearScene.nearScenePortalX == teleportNPC.posX && nearScene.nearScenePortalY == teleportNPC.posY)
					{
						return teleportNPC;
					}
				}
			}
		}
		return null;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x0000267C File Offset: 0x0000087C
	public static PathInfo GetNextPath(int fromSceneId, int toSceneId)
	{
		List<SceneInfo> pathsList = FindPath.GetPathsList(fromSceneId, toSceneId);
		pathsList.Reverse();
		TeleportNPC teleportNPC = FindPath.GetTeleportNPC(pathsList[0], pathsList[1]);
		PathInfo result;
		if (teleportNPC == null)
		{
			result = new PathInfo(false, FindPath.GetPortalX(pathsList[0], pathsList[1]), FindPath.GetPortalY(pathsList[0], pathsList[1]), pathsList[1].scene.sceneId, "NONE");
		}
		else
		{
			result = new PathInfo(true, FindPath.GetPortalX(pathsList[0], pathsList[1]), FindPath.GetPortalY(pathsList[0], pathsList[1]), pathsList[1].scene.sceneId, teleportNPC.npcName);
		}
		return result;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x0000273C File Offset: 0x0000093C
	public static string GetScreenName(int screenID)
	{
		string result = "";
		foreach (Scene scene in FindPath.scenes)
		{
			if (scene.sceneId == screenID)
			{
				result = scene.sceneName;
				break;
			}
		}
		return result;
	}

	// Token: 0x04000003 RID: 3
	private static List<SceneInfo> scenesList = new List<SceneInfo>();

	// Token: 0x04000004 RID: 4
	private static List<Scene> scenes = new List<Scene>();
}
