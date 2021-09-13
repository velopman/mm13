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
    private PlayerSaveDataController data;
    private PlayerAbility unlockedAbilities;


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
    }


    // Callback methods

    private void onLoad(PlayerSaveDataController.PlayerData data)
    {
        this.unlockedAbilities = data.unlockedAbilities;
    }
}
