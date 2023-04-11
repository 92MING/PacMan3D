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
    public SkillBase(CharacterBase character)
    {
        thisChar = character;
    }
    public readonly CharacterBase thisChar;
    public abstract SkillReleaseType releaseType { get; }
    public abstract int energyConsume { get; }

    //真正的use, 每个技能各自定义
    public abstract void use();
}

public abstract class Skill<ChildSkill> : SkillBase
    where ChildSkill : Skill<ChildSkill>
{
    public Skill(CharacterBase character) : base(character) { }

    public string nameKey => typeof(ChildSkill).Name; //skill name key. use this to get translated name
    public string name => SystemManager.TryGetTranslation(nameKey, out string _name) ? _name : nameKey; //skill name
    public string descriptionKey => nameKey + "_description"; //skill description key. use this to get translated description
    public string description => SystemManager.TryGetTranslation(descriptionKey, out string _description) ? _description : descriptionKey; //skill description
}

//瞬發型技能
public abstract class ImmediateSkill<ChildSkill>: Skill<ChildSkill>
    where ChildSkill: ImmediateSkill<ChildSkill>
{
    public ImmediateSkill(CharacterBase character) : base(character) { }
    public override SkillReleaseType releaseType => SkillReleaseType.Immediate;
}

//按Q+點鼠標型的技能
public abstract class PressAndClickSkill<ChildSkill> : Skill<ChildSkill>
    where ChildSkill: PressAndClickSkill<ChildSkill>
{
    public PressAndClickSkill(CharacterBase character) : base(character) { }
    public override SkillReleaseType releaseType => SkillReleaseType.PressAndClick;
    protected bool _prepared = false;
    public bool prepared => _prepared;

    //準備使用技能（按下Q）
    protected virtual void prepare()
    {
        if (prepared) return;
        _prepared = true;
        //其他行为透过重写这个方法实现
    }
    //取消使用技能
    protected virtual void cancelPrepare()
    {
        if (!prepared) return;
        _prepared = false;
        //其他行为透过重写这个方法实现
    }
}

//普通攻击类。普通攻击必為瞬發
public abstract class NormalAttackSkill<ChildSkill> : ImmediateSkill<ChildSkill>
    where ChildSkill: NormalAttackSkill<ChildSkill>
{
    public NormalAttackSkill(CharacterBase character) : base(character) { }
    public override int energyConsume => 0; //普通攻击不消耗能量
    public abstract float timeInterval { get; } // 释放时间间隔
}


