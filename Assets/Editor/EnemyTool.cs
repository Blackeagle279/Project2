using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Sprites;

public class EnemyTool : EditorWindow {

	public List<Enemies> enemyList = new List<Enemies>();

	string nameString;

	bool nameFlag;

	string[] enemyNames;

	int currentChoice = 0;
	int lastChoice = 0;
	int health;
    int attack;
    int defence;
    int agility;
    int mana;

    Sprite mySprite;

    bool myToggle;

    [MenuItem("Custom Tools/Enemy Tool %g")]

	private static void Editortool()
	{
		
		EditorWindow.GetWindow<EnemyTool> ();
	
	}

	void Awake()
	{
		getEnemies();
	}

	void OnGUI()
	{

		currentChoice = EditorGUILayout.Popup (currentChoice, enemyNames);

		foreach (Enemies e in enemyList) 
		{

			EditorGUILayout.LabelField (e.emname);

		}

		if (GUILayout.Button ("Button")) 
		{
			getEnemies ();
		}

        mySprite = EditorGUILayout.ObjectField(mySprite, typeof(Sprite), true) as Sprite;
        if (mySprite != null)
        {
            Texture2D atext = SpriteUtility.GetSpriteTexture(mySprite, false);
            GUILayout.Label(atext);
        }
        nameString = EditorGUILayout.TextField ("Name: ", nameString);
        EditorGUILayout.LabelField("Health");
        health = EditorGUILayout.IntSlider (health, 1, 300);
        EditorGUILayout.LabelField("Attack");
        attack = EditorGUILayout.IntSlider(attack, 1, 100);
        EditorGUILayout.LabelField("Defence");
        defence = EditorGUILayout.IntSlider(defence, 1, 100);
        EditorGUILayout.LabelField("Agility");
        agility = EditorGUILayout.IntSlider(agility, 1, 100);
        myToggle = EditorGUILayout.BeginToggleGroup("Magic User", myToggle);
        EditorGUILayout.LabelField("Mana");
        mana = EditorGUILayout.IntSlider(mana, 1, 100);
        EditorGUILayout.EndToggleGroup();
        if (nameFlag) 
		{
		
			EditorGUILayout.HelpBox ("Name can not be blank", MessageType.Error);

		}
		if (currentChoice == 0) {
			if (GUILayout.Button ("Create")) 
			{
				createEnemy ();
			}
		} 
		else 
		{
			if (GUILayout.Button ("Save")) 
			{
				alterEnemy ();
			}
		}
		if (currentChoice != lastChoice) {
			if (currentChoice == 0) 
			{
				//blank out fields for new enemy
				nameString = "";
			}
			else 
			{
				//Load fields with enemy data
				nameString = enemyList[currentChoice - 1].emname;
				health = enemyList [currentChoice - 1].health;
			}
			lastChoice = currentChoice;

		} 

		
	}

	private void getEnemies()
	{
		enemyList.Clear ();
		string[] guids = AssetDatabase.FindAssets ("t:Enemies", null);
		foreach (string guid in guids) 
		{

			string myString = AssetDatabase.GUIDToAssetPath (guid);

			Enemies enemyInst = AssetDatabase.LoadAssetAtPath (myString, typeof(Enemies)) as Enemies;

			enemyList.Add (enemyInst);

		}
		List<string> enemyNameList = new List<string>();
		foreach (Enemies e in enemyList) 
		{

			enemyNameList.Add (e.emname);

		}

		enemyNameList.Insert (0, "New");

		enemyNames = enemyNameList.ToArray ();
	}

	private void createEnemy()
	{


		if(nameString == "")
		{

			nameFlag = true;

		}
		else
		{
			
			Enemies myEnemy = ScriptableObject.CreateInstance<Enemies> ();
			myEnemy.emname = nameString;
			AssetDatabase.CreateAsset(myEnemy, "Assets/Resources/Data/EnemyData/"+myEnemy.emname.Replace(" ", "_")+".asset");
			nameFlag = false;
			getEnemies ();
		}



	}

	private void alterEnemy()
	{

		enemyList [0].health = health;

	}

}
