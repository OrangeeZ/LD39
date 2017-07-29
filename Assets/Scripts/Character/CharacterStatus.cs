using System;
using System.Collections.Generic;
using UniRx;

[Serializable]
public class CharacterStatus {

	public IReactiveProperty<float> MaxHealth;
	public IReactiveProperty<float> MoveSpeed;

	public readonly CharacterStatusInfo Info;

	public readonly ModifierCalculator ModifierCalculator;

	private List<CharacterStatusEffectInfo> _statusEffects = new List<CharacterStatusEffectInfo>();

	public CharacterStatus( CharacterStatusInfo info ) {

		Info = info;

		MaxHealth = new ReactiveProperty<float>( Info.MaxHealth );
		MoveSpeed = new ReactiveProperty<float>( Info.MoveSpeed );

		ModifierCalculator = new ModifierCalculator();

		ModifierCalculator.Changed += OnModifiersChange;
	}

	public void AddEffect( CharacterStatusEffectInfo statusEffect ) {

		_statusEffects.Add( statusEffect );
	}

	public void RemoveEffect( CharacterStatusEffectInfo statusEffect ) {

		_statusEffects.Remove( statusEffect );
	}

	private void OnModifiersChange() {

		MoveSpeed.Value = ModifierCalculator.CalculateFinalValue( ModifierType.BaseMoveSpeed, Info.MoveSpeed );
		//MaxHealth = ModifierCalculator.CalculateFinalValue( Base )
	}

}