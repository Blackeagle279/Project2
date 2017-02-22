using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Sprites;

public class EnemyTool : EditorWindow {

	public List<Enemies> enemyList = new List<Enemies>();

	string nameString;

	bool nameFlag;
    bool spriteFlag;

	string[] enemyNames;

	int currentChoice = 0;
	int lastChoice = 0;
	int health;
    int attack;
    int defence;
    int agility;
    int mana;
    float attSpeed = 1;

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
        EditorGUILayout.LabelField("Attack Speed");
        attSpeed = EditorGUILayout. FloatField(attSpeed);
        myToggle = EditorGUILayout.BeginToggleGroup("Magic User", myToggle);
        EditorGUILayout.LabelField("Mana");
        mana = EditorGUILayout.IntSlider(mana, 1, 100);
        EditorGUILayout.EndToggleGroup();
        if (nameFlag) 
		{
		
			EditorGUILayout.HelpBox ("Name can not be blank", MessageType.Error);
        }
        if(spriteFlag)
        {
            EditorGUILayout.HelpBox("Sprite can not be blank", MessageType.Error);
        }
        if(attSpeed < 1 || attSpeed > 20)
        {
            EditorGUILayout.HelpBox("Attack speed needs to be within 1-20", MessageType.Error);
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
                health = 0;
                attack = 0;
                defence = 0;
                agility = 0;
                attSpeed = 1;
                mana = 0;
                myToggle = false;
                mySprite = null;
			}
			else 
			{
				//Load fields with enemy data
				nameString = enemyList[currentChoice - 1].emname;
				health = enemyList [currentChoice - 1].health;
                attack = enemyList[currentChoice - 1].atk;
                defence = enemyList[currentChoice - 1].def;
                agility = enemyList[currentChoice - 1].agi ;
                attSpeed = enemyList[currentChoice - 1].atkTime;
                myToggle = enemyList[currentChoice - 1].isMagic;
                mana = enemyList[currentChoice - 1].manaPool;
                mySprite = enemyList[currentChoice - 1].mySprite;
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
            return;

		}
		else
		{
			
			Enemies myEnemy = ScriptableObject.CreateInstance<Enemies> ();
			myEnemy.emname = nameString;
            myEnemy.atk = attack;
            myEnemy.def = defence;
            myEnemy.agi = agility;
            myEnemy.atkTime = attSpeed;
            myEnemy.manaPool = mana;
            myEnemy.isMagic = myToggle;
            myEnemy.mySprite = mySprite;
            AssetDatabase.CreateAsset(myEnemy, "Assets/Resources/Data/EnemyData/"+myEnemy.emname.Replace(" ", "_")+".asset");
			nameFlag = false;
			getEnemies ();
		}

        if (mySprite == null)
        {

            spriteFlag = true;
            return;

        }
        else
        {

            Enemies myEnemy = ScriptableObject.CreateInstance<Enemies>();
            myEnemy.emname = nameString;
            myEnemy.atk = attack;
            myEnemy.def = defence;
            myEnemy.agi = agility;
            myEnemy.atkTime = attSpeed;
            myEnemy.manaPool = mana;
            myEnemy.isMagic = myToggle;
            myEnemy.mySprite = mySprite;
            AssetDatabase.CreateAsset(myEnemy, "Assets/Resources/Data/EnemyData/" + myEnemy.emname.Replace(" ", "_") + ".asset");
            spriteFlag = false;
            getEnemies();
        }



    }

	private void alterEnemy()
	{
        enemyList[currentChoice -1].emname = nameString;
        enemyList[currentChoice -1].mySprite = mySprite;
		enemyList[currentChoice - 1].health = health;
        enemyList[currentChoice - 1].atk = attack;
        enemyList[currentChoice - 1].def = defence;
        enemyList[currentChoice - 1].agi = agility;
        enemyList[currentChoice - 1].atkTime = attSpeed;
        enemyList[currentChoice - 1].manaPool = mana;

        AssetDatabase.SaveAssets();

    }

}
