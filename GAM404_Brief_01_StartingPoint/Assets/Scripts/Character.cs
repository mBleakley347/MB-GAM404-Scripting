using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Functions to complete:
/// - Initial Stats
/// - Return Battle Points
/// - Deal Damage
/// </summary>
public class Character : MonoBehaviour
{
    public CharacterName charName; // This is a reference to an instance of the characters name script.

    [Range(0.0f, 1.0f)]
    public float mojoRemaining = 1; // This is the characters hp this is a float 0-100 but is normalised to 0.0 - 1.0;

    [Header("Stats")]
    /// <summary>
    /// Stats and descriptions
    /// Style: increases damage base
    /// luck: improves chances
    /// rhythm: decreases damage taken
    /// Strength: The Amount of power behind the moves. increases possible damage
    /// Dexterity: The Amount of grace behind the moves. increases chance to hit
    /// Stamina: The Ability to party on. Determines Health
    /// acuity: the amount of focus on the moves. increases the chances of dodge and Crit
    /// Dodge Chance: a combination of Dexterity and acuity. determines the ability to blow off the other teams moves
    /// Crit Chance: a combination of Stength and acuity. determines the ability to preform extradinary feats to deal more damage.
    /// </summary>
    public int availablePoints = 15; // The total amount of points we want to assign to our stats!
    public enum type
    {
        Attack,
        Defence,
        Luck
    }; // Specialisation what stat the character will focus
    public type characterFocus;

    public int level; // This is optional if you want to expand on this to level up your characters and stuff welcome to.
    public int currentXP; // This is optional as well if you want to expand on this brief to assign xp etc!
    public int style, luck, rhythm;// modifiers
    public int strength, dexterity, stamina, acuity;// base Stats
    public float dodgeChance, critChance;// Dirived Stats

    [Header("Related objects")]
    public DanceTeam myTeam; // This holds a reference to the characters current dance team instance they are assigned to.

    public bool isSelected; // This is used for determining if this character is selected in a battle.

    [SerializeField]
    protected TMPro.TextMeshPro nickText; // This is just a piece of text in Unity,  to display the characters name.
 
    public AnimationController animController; // A reference to the animationController, is used to switch dancing states.

    // This is called once, this then calls Initial Stats function
    void Awake()
    {
        InitialStats();
        animController = GetComponent<AnimationController>();
    }

    /// <summary>
    /// A function used to handle setting the intial stats of our game at the begining of the game.
    /// </summary>
    private void InitialStats()
    {
        Debug.LogWarning("InitialStats called, needs to distribute points into stats. This should be able to be ported from previous brief work");
        switch (UnityEngine.Random.Range(0,3)){
            case 0:
                characterFocus = type.Attack;
                break;
            case 1:
                characterFocus = type.Defence;
                break;
            case 2:
                characterFocus = type.Luck;
                break;
            default:
                Debug.Log("default");
                characterFocus = type.Attack;
                break;
        }
        Debug.Log(characterFocus);
        for (int i = 0; i < availablePoints; i++)
        {
            switch (UnityEngine.Random.Range(0, 10))
            {
                case 0:
                    style++;
                    break;
                case 1:
                    luck++;
                    break;
                case 2:
                    rhythm++;
                    break;
                case 3:
                    strength++;
                    break;
                case 4:
                    dexterity++;
                    break;
                case 5:
                    stamina++;
                    break;
                case 6:
                    acuity++;
                    break;
                default:
                    switch (characterFocus)
                    {
                        case type.Attack:
                            switch (UnityEngine.Random.Range(0, 3))
                            {
                                case 0:
                                    strength++;
                                    break;
                                case 1:
                                    dexterity++;
                                    break;
                                case 2:
                                    style++;
                                    break;
                            }
                            break;
                        case type.Defence:
                            switch (UnityEngine.Random.Range(0, 3))
                            {
                                case 0:
                                    stamina++;
                                    break;
                                case 1:
                                    rhythm++;
                                    break;
                                case 2:
                                    acuity++;
                                    break;
                            }
                            break;
                        case type.Luck:
                            switch (UnityEngine.Random.Range(0, 3))
                            {
                                case 0:
                                    luck++;
                                    break;
                                case 1:
                                    acuity++;
                                    break;
                                case 2:
                                    luck++;
                                    break;
                            }
                            break;
                    }
                    break;
            }            
        }
        dodgeChance = ((dexterity * acuity) + (luck));
        critChance = ((strength * acuity) + (luck));
        mojoRemaining = 10 + (stamina * 5);

        // We probably want to set out default level and some default random stats 
        // for our luck, style and rythmn.
    }

    /// <summary>
    /// We probably want to use this to remove some hp (mojo) from our character.....
    /// Then we probably want to check to see if they are dead or not from that damage and return true or false.
    /// </summary>
    public void DealDamage(float amount)
    {
        amount -= rhythm;
        if (UnityEngine.Random.Range(0f, 100f) > dodgeChance)
        {
            mojoRemaining -= amount;
            if(mojoRemaining <= 0)
            {
                myTeam.RemoveFromActive(this);
            }
        }
        // we probably want to do a check in here to see if the character is dead or not...
        // if they are we probably want to remove them from their team's active dancer list...sure wish there was a function in their dance team  script we could use for this.
    }

    /// <summary>
    /// Used to generate a number of battle points that is used in combat.
    /// </summary>
    /// <returns></returns>
    public int ReturnBattlePoints()
    {
        int battlePoints = 5;
        battlePoints += UnityEngine.Random.Range(0, 10 + dexterity + (luck / 2));
        
        return battlePoints;
        // We want to design some algorithm that will generate a number of points based off of our luck,style and rythm, we probably want to add some randomness in our calculation too
        // to ensure that there is not always a draw, by default it just returns 0. 
        // If you right click this function and find all references you can see where it is called.
        //Debug.LogWarning("ReturnBattlePoints has been called we probably want to create some battle points based on our stats");
        return 0;
    }

    /// <summary>
    /// A function called when the battle is completed and some xp is to be awarded.
    /// Takes in and store in BattleOutcome from the BattleHandler script which is how much the player has won by.
    /// By Default it is set to 100% victory.
    /// </summary>
    public void CalculateXP(float BattleOutcome)
    {
        //Debug.LogWarning("This character needs some xp to be given, the outcome of the fight was: " + BattleOutcome);
        currentXP += (int)BattleOutcome;
        if (currentXP >= (level * 10))
        {
            LevelUp();
        }

        // The result of the battle is coming in which is stored in BattleOutcome .... we probably want to do something with it to calculate XP.

        // We probably also want to check to see if the player can level up and if so do something....
    }

    /// <summary>
    /// A function used to handle actions associated with levelling up.
    /// </summary>
    private void LevelUp()
    {
        level++;
        AssignSkillPointsOnLevelUp(3);
        if (currentXP >= (level * 10))
        {
            LevelUp();
        }
        //We probably want to increase the player level, the xp threshold and increase our current skill points based on our level.
        //Debug.LogWarning("Level up has been called");
    }

    /// <summary>
    /// A function used to assign a random amount of points ot each of our skills.
    /// </summary>
    public void AssignSkillPointsOnLevelUp(int PointsToAssign)
    {
        for (int i = 0; i < PointsToAssign; i++)
        {
            switch (UnityEngine.Random.Range(0, 10))
            {
                case 0:
                    style++;
                    break;
                case 1:
                    luck++;
                    break;
                case 2:
                    rhythm++;
                    break;
                case 3:
                    strength++;
                    break;
                case 4:
                    dexterity++;
                    break;
                case 5:
                    mojoRemaining += 5;
                    stamina++;
                    break;
                case 6:
                    acuity++;
                    break;
                default:
                    switch (characterFocus)
                    {
                        case type.Attack:
                            switch (UnityEngine.Random.Range(0, 3))
                            {
                                case 0:
                                    strength++;
                                    break;
                                case 1:
                                    dexterity++;
                                    break;
                                case 2:
                                    style++;
                                    break;
                            }
                            break;
                        case type.Defence:
                            switch (UnityEngine.Random.Range(0, 3))
                            {
                                case 0:
                                    mojoRemaining += 5;
                                    stamina++;
                                    break;
                                case 1:
                                    rhythm++;
                                    break;
                                case 2:
                                    acuity++;
                                    break;
                            }
                            break;
                        case type.Luck:
                            switch (UnityEngine.Random.Range(0, 3))
                            {
                                case 0:
                                    luck++;
                                    break;
                                case 1:
                                    acuity++;
                                    break;
                                case 2:
                                    luck++;
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }
        dodgeChance = ((dexterity * acuity) + (luck));
        critChance = ((strength * acuity) + (luck));

        //Debug.LogWarning("AssignSkillPointsOnLevelUp has been called " + PointsToAssign);

        // We are taking an amount of points to assign in, and we want to assign it to our luck, style and rhythm, we 
        // want some random amount of points added to our current values.
    }

    /// <summary>
    /// Is called inside of our DanceTeam.cs is used to set the characters name!
    /// </summary>
    /// <param name="characterName"></param>
    public void AssignName(CharacterName characterName)
    {
        charName = characterName;
        if(nickText != null)
        {
            nickText.text = charName.nickName;
            nickText.transform.LookAt(Camera.main.transform.position);
            //text faces the wrong way so
            nickText.transform.Rotate(0, 180, 0);
        }
    }
}
