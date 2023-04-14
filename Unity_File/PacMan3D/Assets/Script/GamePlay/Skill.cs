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
    public SkillBase() { }
    public SkillBase(CharacterBase character)
    {
        thisChar = character;
    }
    public readonly CharacterBase thisChar;
    public abstract SkillReleaseType releaseType { get; }
    public abstract int energyConsume { get; }
    public abstract bool prepared { get; } //技能是否已经准备好，只有PressAndClick类技能需要

    //准备使用技能，只有PressAndClick类技能需要
    public abstract void prepare();
    //真正的准备技能部分，重写这部分
    public virtual void _prepare() { }
    //使用技能, 每个技能各自定义
    public abstract void use();
    //真正的使用技能部分，重写这部分
    public virtual void _use() { }
    //取消使用技能，只有PressAndClick类技能需要
    public abstract void cancel();
    //真正的取消技能部分，重写这部分
    public virtual void _cancel() { }

    //检查与扣减能量
    protected bool checkEnergyEnough()
    {
        if (thisChar is null) return false;
        return thisChar.energy >= energyConsume;
    }
    protected void consumeEnergy()
    {
        thisChar.energy -= energyConsume;
    }
}

public abstract class Skill<ChildSkill> : SkillBase
    where ChildSkill : Skill<ChildSkill>
{
    public Skill() { }
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
    public override bool prepared => true; //瞬发型技能无需准备

    public ImmediateSkill() { }
    public ImmediateSkill(CharacterBase character) : base(character) { }
    public override SkillReleaseType releaseType => SkillReleaseType.Immediate;

    public override void prepare() { Debug.LogWarning("Immediate skill do not need prepare!"); }
    public override void _prepare() { Debug.LogWarning("Immediate skill do not need prepare!"); }
    public override void cancel() { Debug.LogWarning("Immediate skill do not need cancel!"); }
    public override void _cancel() { Debug.LogWarning("Immediate skill do not need cancel!"); }
    public override void use()
    {
        if (thisChar is null) return;
        if (!checkEnergyEnough()) return;
        consumeEnergy();
        _use(); //其他行为透过重写这个方法实现
        thisChar.switchAnimation(CharacterAnimation.SKILL);
    }
    
}

//按Q+點鼠標型的技能
public abstract class PressAndClickSkill<ChildSkill> : Skill<ChildSkill>
    where ChildSkill: PressAndClickSkill<ChildSkill>
{
    public PressAndClickSkill() { }
    public PressAndClickSkill(CharacterBase character) : base(character) { }
    public override SkillReleaseType releaseType => SkillReleaseType.PressAndClick;
    protected bool _prepared = false;
    public override bool prepared => _prepared;

    //準備使用技能（按下Q）
    public override void prepare()
    {
        if (prepared) return;
        if (!checkEnergyEnough()) return;
        
        consumeEnergy(); //准备时就已经消耗能量
        _prepared = true;
        _prepare(); //其他行为透过重写这个方法实现
    }
    //取消使用技能
    public override void cancel()
    {
        if (!prepared) return;
        _prepared = false;
        _cancel(); //其他行为透过重写这个方法实现
    }
    public override void use()
    {
        if (thisChar is null) return;
        if (!prepared) return;
        _prepared = false;
        _use(); //其他行为透过重写这个方法实现
        thisChar.switchAnimation(CharacterAnimation.SKILL);
    }
    
}

//普通攻击类。普通攻击必為瞬發
public abstract class NormalAttackSkill<ChildSkill> : ImmediateSkill<ChildSkill>
    where ChildSkill: NormalAttackSkill<ChildSkill>
{
    public NormalAttackSkill() { }
    public NormalAttackSkill(CharacterBase character) : base(character) { }
    public override int energyConsume => 0; //普通攻击不消耗能量
    public abstract float timeInterval { get; } // 释放时间间隔
    protected float _nextUse = 0f;

    public override void use()
    {
        if (thisChar is null) return;
        if (Time.time < _nextUse) return; //普通攻击有间隔
        _nextUse = Time.time + timeInterval;
        _use(); //其他行为透过重写这个方法实现
        thisChar.switchAnimation(CharacterAnimation.ATTACK);
    }
}


