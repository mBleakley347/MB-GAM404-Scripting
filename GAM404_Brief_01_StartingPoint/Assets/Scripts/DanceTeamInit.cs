using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Functions to complete:
/// - Init Teams
/// </summary>
public class DanceTeamInit : MonoBehaviour
{
    public DanceTeam teamA, teamB; // A reference to our teamA and teamB DanceTeam instances.

    public GameObject dancerPrefab; // This is the dancer that gets spawned in for each team.
    public int dancersPerSide = 5; // This is the number of dancers for each team, if you want more, you need to modify this in the inspector.
    public CharacterNameGenerator nameGenerator; // This is a reference to our CharacterNameGenerator instance.
    private CharacterName[] teamACharacterNames; // An array to hold all our character names of TeamA.
    private CharacterName[] teamBCharacterNames; // An array to hold all the character names of TeamB. // Addition orginal did not conform to naming convention lowercase c for Character
    private CharacterName[] characterPool;

    /// <summary>
    /// Called to iniatlise the dance teams with some dancers :D
    /// </summary>
    public void InitTeams()
    {
        //Debug.LogWarning("InitTeams called, needs to generate names for the teams and set them with teamA.SetTroupeName and teamA.InitialiseTeamFromNames");
        // We need to set out team names using teamA.SetTroupeName.
        teamA.SetTroupeName("Team1");
        teamB.SetTroupeName("Team2");
        // We need to generate some character names for our teams to use from our CharacterNameGenerator.
        characterPool = nameGenerator.GenerateNames(15);
        teamACharacterNames = new CharacterName[dancersPerSide];
        teamBCharacterNames = new CharacterName[dancersPerSide];
        for (int i = 0; i < (dancersPerSide * 2); i++)
        {
            CharacterName nextTeamDancer = new CharacterName();
            while(nextTeamDancer.firstName == null)
            {
                int temp = Random.Range(0, 15);
                nextTeamDancer = characterPool[temp];
                characterPool[temp] = new CharacterName();
                Debug.Log(nextTeamDancer.firstName + " " + i);
            } 
            
            if(i >= dancersPerSide)
            {
                Debug.Log(nextTeamDancer.firstName + " To Team A");
                teamACharacterNames[i - dancersPerSide] = nextTeamDancer;
            }
            else
            {
                Debug.Log(nextTeamDancer.firstName + " To Team B");
                teamBCharacterNames[i] = nextTeamDancer;
            }
        }
        // We need to spawn in some dancers using teamA.InitialiseTeamFromNames.
        teamA.InitaliseTeamFromNames(dancerPrefab, DanceTeam.Direction.Left, teamACharacterNames);
        teamB.InitaliseTeamFromNames(dancerPrefab, DanceTeam.Direction.Right, teamBCharacterNames);

        //Debug.LogWarning("InitTeams called, needs to create character names via CharacterNameGenerator and get them into the team.InitaliseTeamFromNames");
    }
}
