using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillReleaseType
{
    Immediate, //按下按键，即时释放
    PressAndClick, //按下按键，再等待玩家按鼠标左键
}

public abstract class SkillBase
{
    public CharacterBase thisChar;

    protected abstract void use();
}

public abstract class Skill<ChildSkill> : SkillBase
    where ChildSkill : Skill<ChildSkill>
{
    public string nameKey => typeof(ChildSkill).Name; //skill name key. use this to get translated name
    public string name => SystemManager.TryGetTranslation(nameKey, out string _name) ? _name : nameKey; //skill name
    public string descriptionKey => nameKey + "_description"; //skill description key. use this to get translated description
    public string description => SystemManager.TryGetTranslation(descriptionKey, out string _description) ? _description : descriptionKey; //skill description
    
    public abstract SkillReleaseType releaseType { get; }    
    public abstract int energyConsume { get; }

    
}

public abstract class NormalAttackSkill<ChildSkill> : Skill<NormalAttackSkill<ChildSkill>>
{
    public override SkillReleaseType releaseType => SkillReleaseType.Immediate;
    public override int energyConsume { get; }
}
