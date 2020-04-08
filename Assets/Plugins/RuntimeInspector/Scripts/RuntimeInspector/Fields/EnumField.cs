﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace
{
	public class EnumField : InspectorField
	{
#pragma warning disable 0649
		[SerializeField]
		private Image background;

		[SerializeField]
		private Image dropdownArrow;

		[SerializeField]
		private RectTransform templateTransform;

		[SerializeField]
		private Image templateBackground;

		[SerializeField]
		private Image templateCheckmark;

		[SerializeField]
		private Text templateText;

		[SerializeField]
		private Dropdown input;
#pragma warning restore 0649

		private static readonly Dictionary<Type, List<string>> enumNames = new Dictionary<Type, List<string>>();
		private static readonly Dictionary<Type, List<object>> enumValues = new Dictionary<Type, List<object>>();

		private List<string> currEnumNames;
		private List<object> currEnumValues;

		public override void Initialize()
		{
			base.Initialize();
			input.onValueChanged.AddListener( OnValueChanged );
		}

		public override bool SupportsType( Type type )
		{
#if UNITY_EDITOR || !NETFX_CORE
			return type.IsEnum;
#else
			return type.GetTypeInfo().IsEnum;
#endif
		}

		protected override void OnBound( MemberInfo variable )
		{
			base.OnBound( variable );

			if( !enumNames.TryGetValue( BoundVariableType, out currEnumNames ) || !enumValues.TryGetValue( BoundVariableType, out currEnumValues ) )
			{
				string[] names = Enum.GetNames( BoundVariableType );
				Array values = Enum.GetValues( BoundVariableType );

				currEnumNames = new List<string>( names.Length );
				currEnumValues = new List<object>( names.Length );

				for( int i = 0; i < names.Length; i++ )
				{
					currEnumNames.Add( names[i] );
					currEnumValues.Add( values.GetValue( i ) );
				}

				enumNames[BoundVariableType] = currEnumNames;
				enumValues[BoundVariableType] = currEnumValues;
			}

			input.ClearOptions();
			input.AddOptions( currEnumNames );
		}

		private void OnValueChanged( int input )
		{
			Value = currEnumValues[input];
			Inspector.RefreshDelayed();
		}

		protected override void OnSkinChanged()
		{
			base.OnSkinChanged();

			Vector2 templateSizeDelta = templateTransform.sizeDelta;
			templateSizeDelta.y = Skin.LineHeight;
			templateTransform.sizeDelta = templateSizeDelta;

			background.color = Skin.InputFieldNormalBackgroundColor;
			dropdownArrow.color = Skin.TextColor.Tint( 0.1f );

			input.captionText.SetSkinInputFieldText( Skin );
			templateText.SetSkinInputFieldText( Skin );

			templateBackground.color = Skin.InputFieldNormalBackgroundColor.Tint( 0.075f );
			templateCheckmark.color = Skin.ToggleCheckmarkColor;

			Vector2 rightSideAnchorMin = new Vector2( Skin.LabelWidthPercentage, 0f );
			variableNameMask.rectTransform.anchorMin = rightSideAnchorMin;
			( (RectTransform) input.transform ).anchorMin = rightSideAnchorMin;
		}

		public override void Refresh()
		{
			base.Refresh();

			int valueIndex = currEnumValues.IndexOf( Value );
			if( valueIndex != -1 )
				input.value = valueIndex;
		}
	}
}