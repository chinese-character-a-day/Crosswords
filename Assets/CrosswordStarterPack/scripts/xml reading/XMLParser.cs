﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Text;
using System.IO;

public class XMLParser : MonoBehaviour {

	public static XMLParser Instance;

	[Serializable]
	public class clue
	{
		public string type;
		public string number;
		public string primary;
		public string alternate;

		public void print() {
//			Debug.Log("clue: type: " + type);
//			Debug.Log("clue: number: " + number);
//			Debug.Log("clue: primary: " + primary);
//			Debug.Log("clue: alternate: " + alternate);

			string mesg = "{";
			mesg += "\"type\": \""+type+"\",";
			mesg += "\"number\": \""+number+"\",";
			mesg += "\"primary\": \""+primary+"\",";
			mesg += "\"alternate\": \""+alternate+"\"}";

			Debug.Log(mesg);

		}
	}

	[Serializable]
	public class aPuzzle
	{
		public string number;
		public string difficulty;
		public string tag;
		public string title;
		public string puzzleSize;
		public string puzzleCutout;
		public string puzzleSolid;
		public string puzzleData;

		public List<clue> acrossClues = new List<clue>();
		public List<clue> downClues = new List<clue>();

		public void print() {
			Debug.Log("aPuzzle: number: " + number);
			Debug.Log("aPuzzle: difficulty: " + difficulty);
			Debug.Log("aPuzzle: tag: " + tag);
			Debug.Log("aPuzzle: title: " + title);
			Debug.Log("aPuzzle: puzzleSize: " + puzzleSize);
			Debug.Log("aPuzzle: puzzleCutout: " + puzzleCutout);
			Debug.Log("aPuzzle: puzzleSolid: " + puzzleSolid);
			Debug.Log("aPuzzle: puzzleData: " + puzzleData);

			Debug.Log("aPuzzle: acrossClues:");
			foreach (clue puzzleClue in acrossClues) {
				puzzleClue.print();
			}

			Debug.Log("aPuzzle: downClues:");
			foreach (clue puzzleClue in downClues) {
				puzzleClue.print();
			}
		}
	}

	public List<aPuzzle> puzzles = new List<aPuzzle>();

	public TextAsset xmlFile;	

	public int puzzleToLoad;

	private static bool _created = false ;

	private int debugLevel = 1;

	void Awake ()
	{
		Instance = this;
		if (!_created){
			DontDestroyOnLoad (this.gameObject);
			_created = true ;
		}else{
			Destroy (this.gameObject);
		}
	}

	void Start()
	{
		//BreakdownXML();
	}
	
	public void BreakdownXML()
	{
		Debug.Log("XMLParser: BreakdownXML");

		//xmlFile = (TextAsset)Resources.Load("Sampler", typeof(TextAsset)); //testFile

		XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
		xmlDoc.LoadXml(xmlFile.text); // load the file.
		XmlNodeList puzzlesList = xmlDoc.GetElementsByTagName("crossword_puzzle"); // array of the level nodes.

		//Debug.Log (puzzlesList.Count); // the total amount of puzzles inside the file
		foreach (XmlNode puzzleInfo in puzzlesList)
		{
			aPuzzle _tempPuzzle = new aPuzzle();

			_tempPuzzle.number = puzzleInfo.Attributes["number"].Value;
			_tempPuzzle.difficulty = puzzleInfo.Attributes["difficulty"].Value;
			_tempPuzzle.tag = puzzleInfo.Attributes["tag"].Value;

			//Debug.Log (puzzleInfo.Attributes["number"].Value);
			//Debug.Log (puzzleInfo.Attributes["difficulty"].Value);
			//Debug.Log (puzzleInfo.Attributes["tag"].Value);

			XmlNodeList puzzlecontent = puzzleInfo.ChildNodes;

			foreach (XmlNode puzzleNode in puzzlecontent)
			{
				if(puzzleNode.Name == "title")
				{
					_tempPuzzle.title = puzzleNode.InnerText;
					Debug.Log("XMLParser: BreakdownXML: puzzleNode: " + puzzleNode.InnerText);
				}
				
				if(puzzleNode.Name == "grid")
				{
					_tempPuzzle.puzzleSize = puzzleNode.Attributes["size"].Value;
					_tempPuzzle.puzzleCutout = puzzleNode.Attributes["cutout"].Value;
					_tempPuzzle.puzzleSolid = puzzleNode.Attributes["solid"].Value;
					_tempPuzzle.puzzleData = puzzleNode.InnerText;

					//Debug.Log (puzzleNode.Attributes["size"].Value);
					//Debug.Log (puzzleNode.Attributes["cutout"].Value);
					//Debug.Log (puzzleNode.Attributes["solid"].Value);
					//Debug.Log (puzzleNode.InnerText);

					Debug.Log ("XMLParser: BreakdownXML: puzzleData: " + _tempPuzzle.puzzleData);
				}
				
				if(puzzleNode.Name == "across")
				{
					XmlNodeList clues = puzzleNode.ChildNodes;
					foreach (XmlNode clue in clues)
					{
						clue _tempClue = new clue();
						_tempClue.type = "across";
						_tempClue.number = clue.Attributes["number"].Value;

						if (debugLevel > 1) Debug.Log ("XMLParser: BreakdownXML: The across Clue number [" + clue.Attributes["number"].Value + "]");

						XmlNodeList clueTypes = clue.ChildNodes;
						foreach (XmlNode clueType in clueTypes)
						{
							if(clueType.Name == "primary")
							{
								_tempClue.primary = clueType.InnerText;
								//Debug.Log ("Primary Clue is " + clueType.InnerText);
							}

							if(clueType.Name == "alternate")
							{
								_tempClue.alternate = clueType.InnerText;
								//Debug.Log ("Alternate Clue is " + clueType.InnerText);
							}
						}
						_tempPuzzle.acrossClues.Add(_tempClue);
					}
				}

				if(puzzleNode.Name == "down")
				{
					XmlNodeList clues = puzzleNode.ChildNodes;
					foreach (XmlNode clue in clues)
					{
						clue _tempClue = new clue();
						_tempClue.type = "down";
						_tempClue.number = clue.Attributes["number"].Value;
						
						if (debugLevel > 1) Debug.Log ("XMLParser: BreakdownXML: The down Clue number [" + clue.Attributes["number"].Value + "]");
						
						XmlNodeList clueTypes = clue.ChildNodes;
						foreach (XmlNode clueType in clueTypes)
						{
							if(clueType.Name == "primary")
							{
								_tempClue.primary = clueType.InnerText;
								//Debug.Log ("Primary Clue is " + clueType.InnerText);
							}
							
							if(clueType.Name == "alternate")
							{
								_tempClue.alternate = clueType.InnerText;
								//Debug.Log ("Alternate Clue is " + clueType.InnerText);
							}
						}
						_tempPuzzle.downClues.Add(_tempClue);
					}
				}
			}
			puzzles.Add(_tempPuzzle);
		}
		Debug.Log("XMLParser: BreakdownXML: " + puzzles.Count);

		//just for testing
		//GridManager.Instance.currentPuzzle = puzzles[0];
		//GridManager.Instance.BuildTheGrid();
	}
}
