using System;
using UnityEngine;

[Flags]
public enum PlayerAbility
{
    NONE        = 0,
    DASH        = 1,
    WALL_JUMP   = 2,
    HANG        = 4,
}

public class PlayerAbilityController : MonoBehaviour
{
    public delegate void OnAbilityUnlocked(PlayerAbility ability);
    public OnAbilityUnlocked onAbilityUnlocked;

    private PlayerSaveDataController data;
    public PlayerAbility unlockedAbilities;


    // Lifecycle methods

    void Start()
    {
        this.data = this.GetComponent<PlayerSaveDataController>();
        this.data.onLoad += this.onLoad;
    }


    // Public methods

    public bool Has(PlayerAbility ability)
    {
        return this.unlockedAbilities.HasFlag(ability);
    }

    public void UnlockAbility(PlayerAbility ability)
    {
        this.unlockedAbilities |= ability;
        this.data.SetUnlockedAbilities(this.unlockedAbilities);

        if (this.onAbilityUnlocked != null)
        {
            this.onAbilityUnlocked(ability);
        }
    }


    // Callback methods

    private void onLoad(PlayerSaveDataController.PlayerData data)
    {
        this.unlockedAbilities = data.unlockedAbilities;
    }
}
