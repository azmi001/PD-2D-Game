using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVWriter : MonoBehaviour
{
    string filename = "";

    [System.Serializable]
    public class PlayerCharacterList
    {
        public Character[] character;
    }

    public PlayerCharacterList characterList = new PlayerCharacterList();
    // Start is called before the first frame update
    void Start()
    {
        filename = Application.dataPath + "/Database.csv";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            WriteCSV();
        }
    }

    public void WriteCSV()
    {
        if(characterList.character.Length > 0)
        {
            TextWriter tw = new StreamWriter(filename, false);
            tw.WriteLine("Unit Name, Unit Level, Unit Exp, Deffence, Demage, Max HP, Heal, unlock");
            tw.Close();

            tw = new StreamWriter(filename, true);

            for (int i = 0; i < characterList.character.Length; i++) 
            {
                tw.WriteLine(characterList.character[i].unitName + "," +
                            characterList.character[i].unitLevel + "," +
                            characterList.character[i].unitexp + "," +
                            characterList.character[i].deffense + "," +
                            characterList.character[i].damage + "," +
                            characterList.character[i].maxHP + "," +
                            characterList.character[i].Heal + "," +
                            characterList.character[i].Unlock);
            }
            tw.Close();
        }
    }
}
