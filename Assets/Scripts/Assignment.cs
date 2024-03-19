using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Text;




public class Assignment : MonoBehaviour, IManager
{
    [Serializable]
    public class GroupMember
    {
        public string name;
        public int birthYear;
        public string favColor;
        public string favMovie;
    }
    
    private string _state;
    public string State
    {
        get { return _state; }
        // Code gets run here 
        set { _state = value; }
        // Get/set is right in between privat and public. 
    }

    [Serializable]
    public class Group
    {
        public string name;
        public List<GroupMember> members;
    }

    private string _dataPath;
    private string _textFile;
    private string _streamingTextFile;

    private string _xmlGroups;
    private string _jsonGroups;

    public List<GroupMember> groupMembers = new List<GroupMember>
    {
        new GroupMember { name = "Jojo", birthYear = 2000, favColor = "Purple", favMovie = "The Tree of Life" },
        new GroupMember { name = "LeChock", birthYear = 1989, favColor = "Blue", favMovie = "Intersteallar" },
        new GroupMember { name = "Phil", birthYear = 1997, favColor = "Yellow", favMovie = "Big Hero 6" },
        new GroupMember { name = "Eri", birthYear = 2002, favColor = "Orange", favMovie = "Good Will Hunting"},
        new GroupMember { name = "Matti", birthYear = 2001, favColor ="Red", favMovie = "Another Round"},
        new GroupMember { name = "Nat", birthYear = 1992, favColor = "Green", favMovie = "Dune 2"}
    };


    void Awake()
    {
        _dataPath = Application.persistentDataPath + "/Group_Data/";
        Debug.Log(_dataPath);
        _xmlGroups = _dataPath + "Groups.xml";
        _jsonGroups = _dataPath + "Groups.json";
    }
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _state = "Data Manager initialized..";
        Debug.Log(_state);

        NewDirectory();
        WriteToXML(_xmlGroups);
        SerializeXML();
        DeserializeXML();
        // SerializeJSON();
        // DeserializeJSON();
        
    }
    
    public void NewDirectory()
    {
        if (Directory.Exists(_dataPath))
        {
            Debug.Log("Directory already exists...");
            return;
        }

        Directory.CreateDirectory(_dataPath);
        Debug.Log("New directory created!");
    }
    public void WriteToXML(string filename)
    {
        if (!File.Exists(filename))
        {
            FileStream xmlStream = File.Create(filename);
            XmlWriter xmlWriter = XmlWriter.Create(xmlStream);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("group-members");

            for (int i = 1; i < 5; i++)
            {
                xmlWriter.WriteElementString("member", "Member-" + i);
            }

            xmlWriter.WriteEndElement();
            xmlWriter.Close();
            xmlStream.Close();
        }
    }

    public void SerializeXML()
    {
        var xmlSerializer = new XmlSerializer(typeof(List<GroupMember>));

        using (FileStream stream = File.Create(_xmlGroups))
        {
            xmlSerializer.Serialize(stream, groupMembers);
        }
    }
    public void DeserializeXML()
    {
        if (File.Exists(_xmlGroups))
        {
            var xmlSerializer = new XmlSerializer(typeof(List<GroupMember>));

            using (FileStream stream = File.OpenRead(_xmlGroups))
            {
                var groupList = (List<GroupMember>)xmlSerializer.Deserialize(stream);

                foreach (var member in groupList)
                {
                    Debug.LogFormat("Member: {0} - Birth Year: {1} - Favourite Colour: {2} Favourite Movie: {3}", member.name, member.birthYear, member.favColor, member.favMovie);
                }
            }
            SerializeJSON(groupMembers);
        }
    }

    public void SerializeJSON(List<GroupMember> groupMembersData)
    {
        Group group = new Group();
        group.members = groupMembersData;
        group.name = "Super Group";

        Debug.Log("Group members count " + group.members.Count);
        string jsonString = JsonUtility.ToJson(group, true);

        using (StreamWriter stream = File.CreateText(_jsonGroups))
        {
            stream.WriteLine(jsonString);
        }
    }

     public void DeserializeJSON()
    {
        if(File.Exists(_jsonGroups))
        {
            using (StreamReader stream = new StreamReader(_jsonGroups))
            {
                var jsonString = stream.ReadToEnd();
                var groups = JsonUtility.FromJson<Group>(jsonString);

                foreach (var member in groups.members)
                {
                    Debug.LogFormat("Member: {0} - Birth Year: {1} - Favourite Colour: {2} Favourite Movie: {3}", member.name, member.birthYear, member.favColor, member.favMovie);
                }
            }
        }
        
    }
}
