using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object / AudioClipListAsset", fileName = "New AudioClipListAsset")]
public class AudioClipListAsset : ScriptableObject
{
    [Header("Pickups and misc")]
    public AudioClip pickupItem;
    public AudioClip pickupWeapon;
    public AudioClip explosion;
    public AudioClip teleport;
    public AudioClip doorOpen; // Might not be needed?
    public AudioClip gameOver; // When we die

    [Header("Weapons")]
    public AudioClip weaponShoot; // Default
    public AudioClip weaponFlamethrowerShoot;
    public AudioClip weaponPulserifleShoot;
    public AudioClip weaponPulserifleBounce;
    public AudioClip weaponRocketShoot;
    public AudioClip weaponShotgunShoot;

    [Header("UI")]
    public AudioClip uiUpgrademenuOpen; // When upgrade menu opens
    public AudioClip uiUpgradeChosen; // When we choose upgrade
    public AudioClip uiButtonPress;

    [Header("Enemies")]
    public AudioClip enemyHurt;
    public AudioClip enemyDie;
    public AudioClip enemyShoot;
    public AudioClip enemyBossDie;

    [Header("Player")]
    public AudioClip playerHurt;
    public AudioClip playerDash;
    //public AudioClip playerHeal;

    [Header("Music")]
    public AudioClip musicTitle; // maybe?
    public AudioClip music1; 
    public AudioClip music2; 
}
