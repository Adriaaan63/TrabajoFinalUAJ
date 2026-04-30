/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.UltimateCharacterController.Editor.Inspectors.Character.Abilities
{
	using Opsive.Shared.Editor.UIElements.Controls;
	using Opsive.UltimateCharacterController.Editor.Controls.Types.AbilityDrawers;
	using Opsive.UltimateCharacterController.Editor.Utility;
	using UnityEditor;
	using UnityEditor.Animations;
	using UnityEngine;

	/// <summary>
	/// Draws a custom inspector for the Cover Ability.
	/// </summary>
	[ControlType(typeof(Opsive.DeathmatchAIKit.Character.Abilities.Cover))]
	public class CoverDrawer : AbilityDrawer
	{
		// ------------------------------------------- Start Generated Code -------------------------------------------
		// ------- Do NOT make any changes below. Changes will be removed when the animator is generated again. -------
		// ------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// Returns true if the ability can build to the animator.
		/// </summary>
		public override bool CanBuildAnimator { get { return true; } }

		/// <summary>
		/// An editor only method which can add the abilities states/transitions to the animator.
		/// </summary>
		/// <param name="animatorControllers">The Animator Controllers to add the states to.</param>
		/// <param name="firstPersonAnimatorControllers">The first person Animator Controllers to add the states to.</param>
		public override void BuildAnimator(AnimatorController[] animatorControllers, AnimatorController[] firstPersonAnimatorControllers)
		{
			for (int i = 0; i < animatorControllers.Length; ++i) {
				if (animatorControllers[i].layers.Length <= 0) {
					Debug.LogWarning("Warning: The animator controller does not contain the same number of layers as the demo animator. All of the animations cannot be added.");
					return;
				}

				var baseStateMachine1483310764 = animatorControllers[i].layers[0].stateMachine;

				// The state machine should start fresh.
				for (int j = 0; j < animatorControllers[i].layers.Length; ++j) {
					for (int k = 0; k < baseStateMachine1483310764.stateMachines.Length; ++k) {
						if (baseStateMachine1483310764.stateMachines[k].stateMachine.name == "Cover") {
							baseStateMachine1483310764.RemoveStateMachine(baseStateMachine1483310764.stateMachines[k].stateMachine);
							break;
						}
					}
				}

				// AnimationClip references.
				var crouchingCoverCenterLeftAimAnimationClip46448Path = AssetDatabase.GUIDToAssetPath("791c6a81c01a0b045b6e2dcc6e0e9ff4"); 
				var crouchingCoverCenterLeftAimAnimationClip46448 = AnimatorBuilder.GetAnimationClip(crouchingCoverCenterLeftAimAnimationClip46448Path, "CrouchingCoverCenterLeftAim");
				var crouchingCoverCenterLeftAimReturnAnimationClip46456Path = AssetDatabase.GUIDToAssetPath("791c6a81c01a0b045b6e2dcc6e0e9ff4"); 
				var crouchingCoverCenterLeftAimReturnAnimationClip46456 = AnimatorBuilder.GetAnimationClip(crouchingCoverCenterLeftAimReturnAnimationClip46456Path, "CrouchingCoverCenterLeftAimReturn");
				var crouchingCoverAimLeftHoldAnimationClip46462Path = AssetDatabase.GUIDToAssetPath("cf8a597d4a1446f43b772efd9cbcdaaa"); 
				var crouchingCoverAimLeftHoldAnimationClip46462 = AnimatorBuilder.GetAnimationClip(crouchingCoverAimLeftHoldAnimationClip46462Path, "CrouchingCoverAimLeftHold");
				var crouchingCoverAimRightAnimationClip46470Path = AssetDatabase.GUIDToAssetPath("cf8a597d4a1446f43b772efd9cbcdaaa"); 
				var crouchingCoverAimRightAnimationClip46470 = AnimatorBuilder.GetAnimationClip(crouchingCoverAimRightAnimationClip46470Path, "CrouchingCoverAimRight");
				var crouchingCoverAimRightHoldAnimationClip46476Path = AssetDatabase.GUIDToAssetPath("cf8a597d4a1446f43b772efd9cbcdaaa"); 
				var crouchingCoverAimRightHoldAnimationClip46476 = AnimatorBuilder.GetAnimationClip(crouchingCoverAimRightHoldAnimationClip46476Path, "CrouchingCoverAimRightHold");
				var crouchingCoverAimRightReturnAnimationClip46486Path = AssetDatabase.GUIDToAssetPath("cf8a597d4a1446f43b772efd9cbcdaaa"); 
				var crouchingCoverAimRightReturnAnimationClip46486 = AnimatorBuilder.GetAnimationClip(crouchingCoverAimRightReturnAnimationClip46486Path, "CrouchingCoverAimRightReturn");
				var crouchingCoverIdleLeftAnimationClip46500Path = AssetDatabase.GUIDToAssetPath("e9ee80e39ac40244b9ace63bd92ee356"); 
				var crouchingCoverIdleLeftAnimationClip46500 = AnimatorBuilder.GetAnimationClip(crouchingCoverIdleLeftAnimationClip46500Path, "CrouchingCoverIdleLeft");
				var crouchingCoverCenterRightAimHoldAnimationClip46506Path = AssetDatabase.GUIDToAssetPath("791c6a81c01a0b045b6e2dcc6e0e9ff4"); 
				var crouchingCoverCenterRightAimHoldAnimationClip46506 = AnimatorBuilder.GetAnimationClip(crouchingCoverCenterRightAimHoldAnimationClip46506Path, "CrouchingCoverCenterRightAimHold");
				var crouchingCoverCenterRightAimReturnAnimationClip46514Path = AssetDatabase.GUIDToAssetPath("791c6a81c01a0b045b6e2dcc6e0e9ff4"); 
				var crouchingCoverCenterRightAimReturnAnimationClip46514 = AnimatorBuilder.GetAnimationClip(crouchingCoverCenterRightAimReturnAnimationClip46514Path, "CrouchingCoverCenterRightAimReturn");
				var crouchingCoverCenterRightAimAnimationClip46520Path = AssetDatabase.GUIDToAssetPath("791c6a81c01a0b045b6e2dcc6e0e9ff4"); 
				var crouchingCoverCenterRightAimAnimationClip46520 = AnimatorBuilder.GetAnimationClip(crouchingCoverCenterRightAimAnimationClip46520Path, "CrouchingCoverCenterRightAim");
				var crouchingCoverIdleRightAnimationClip46534Path = AssetDatabase.GUIDToAssetPath("e9ee80e39ac40244b9ace63bd92ee356"); 
				var crouchingCoverIdleRightAnimationClip46534 = AnimatorBuilder.GetAnimationClip(crouchingCoverIdleRightAnimationClip46534Path, "CrouchingCoverIdleRight");
				var crouchingCoverStartAnimationClip46546Path = AssetDatabase.GUIDToAssetPath("5f2facb2ef9e50c429ae4a517a926abb"); 
				var crouchingCoverStartAnimationClip46546 = AnimatorBuilder.GetAnimationClip(crouchingCoverStartAnimationClip46546Path, "CrouchingCoverStart");
				var crouchingCoverStrafeLeftAnimationClip46566Path = AssetDatabase.GUIDToAssetPath("856ef158bb445f74b8997062574876dd"); 
				var crouchingCoverStrafeLeftAnimationClip46566 = AnimatorBuilder.GetAnimationClip(crouchingCoverStrafeLeftAnimationClip46566Path, "CrouchingCoverStrafeLeft");
				var crouchingCoverStrafeRightAnimationClip46568Path = AssetDatabase.GUIDToAssetPath("856ef158bb445f74b8997062574876dd"); 
				var crouchingCoverStrafeRightAnimationClip46568 = AnimatorBuilder.GetAnimationClip(crouchingCoverStrafeRightAnimationClip46568Path, "CrouchingCoverStrafeRight");
				var crouchingCoverAimLeftAnimationClip46574Path = AssetDatabase.GUIDToAssetPath("cf8a597d4a1446f43b772efd9cbcdaaa"); 
				var crouchingCoverAimLeftAnimationClip46574 = AnimatorBuilder.GetAnimationClip(crouchingCoverAimLeftAnimationClip46574Path, "CrouchingCoverAimLeft");
				var crouchingCoverCenterLeftAimHoldAnimationClip46578Path = AssetDatabase.GUIDToAssetPath("791c6a81c01a0b045b6e2dcc6e0e9ff4"); 
				var crouchingCoverCenterLeftAimHoldAnimationClip46578 = AnimatorBuilder.GetAnimationClip(crouchingCoverCenterLeftAimHoldAnimationClip46578Path, "CrouchingCoverCenterLeftAimHold");
				var crouchingCoverAimLeftReturnAnimationClip46586Path = AssetDatabase.GUIDToAssetPath("cf8a597d4a1446f43b772efd9cbcdaaa"); 
				var crouchingCoverAimLeftReturnAnimationClip46586 = AnimatorBuilder.GetAnimationClip(crouchingCoverAimLeftReturnAnimationClip46586Path, "CrouchingCoverAimLeftReturn");
				var standingCoverAimRightReturnAnimationClip46602Path = AssetDatabase.GUIDToAssetPath("ffbc1b8223c72b0408df1fd3d16e87fd"); 
				var standingCoverAimRightReturnAnimationClip46602 = AnimatorBuilder.GetAnimationClip(standingCoverAimRightReturnAnimationClip46602Path, "StandingCoverAimRightReturn");
				var standingCoverAimRightHoldAnimationClip46608Path = AssetDatabase.GUIDToAssetPath("ffbc1b8223c72b0408df1fd3d16e87fd"); 
				var standingCoverAimRightHoldAnimationClip46608 = AnimatorBuilder.GetAnimationClip(standingCoverAimRightHoldAnimationClip46608Path, "StandingCoverAimRightHold");
				var standingCoverAimRightAnimationClip46614Path = AssetDatabase.GUIDToAssetPath("ffbc1b8223c72b0408df1fd3d16e87fd"); 
				var standingCoverAimRightAnimationClip46614 = AnimatorBuilder.GetAnimationClip(standingCoverAimRightAnimationClip46614Path, "StandingCoverAimRight");
				var standingCoverAimLeftHoldAnimationClip46620Path = AssetDatabase.GUIDToAssetPath("ffbc1b8223c72b0408df1fd3d16e87fd"); 
				var standingCoverAimLeftHoldAnimationClip46620 = AnimatorBuilder.GetAnimationClip(standingCoverAimLeftHoldAnimationClip46620Path, "StandingCoverAimLeftHold");
				var standingCoverAimLeftReturnAnimationClip46628Path = AssetDatabase.GUIDToAssetPath("ffbc1b8223c72b0408df1fd3d16e87fd"); 
				var standingCoverAimLeftReturnAnimationClip46628 = AnimatorBuilder.GetAnimationClip(standingCoverAimLeftReturnAnimationClip46628Path, "StandingCoverAimLeftReturn");
				var standingCoverAimLeftAnimationClip46634Path = AssetDatabase.GUIDToAssetPath("ffbc1b8223c72b0408df1fd3d16e87fd"); 
				var standingCoverAimLeftAnimationClip46634 = AnimatorBuilder.GetAnimationClip(standingCoverAimLeftAnimationClip46634Path, "StandingCoverAimLeft");
				var standingCoverStrafeLeftAnimationClip46648Path = AssetDatabase.GUIDToAssetPath("5a16b70d64433284abc4081f9536e72b"); 
				var standingCoverStrafeLeftAnimationClip46648 = AnimatorBuilder.GetAnimationClip(standingCoverStrafeLeftAnimationClip46648Path, "StandingCoverStrafeLeft");
				var standingCoverIdleLeftAnimationClip46650Path = AssetDatabase.GUIDToAssetPath("242453afc20bc2b4cb84137b8059d653"); 
				var standingCoverIdleLeftAnimationClip46650 = AnimatorBuilder.GetAnimationClip(standingCoverIdleLeftAnimationClip46650Path, "StandingCoverIdleLeft");
				var standingCoverIdleRightAnimationClip46652Path = AssetDatabase.GUIDToAssetPath("242453afc20bc2b4cb84137b8059d653"); 
				var standingCoverIdleRightAnimationClip46652 = AnimatorBuilder.GetAnimationClip(standingCoverIdleRightAnimationClip46652Path, "StandingCoverIdleRight");
				var standingCoverStrafeRightAnimationClip46654Path = AssetDatabase.GUIDToAssetPath("5a16b70d64433284abc4081f9536e72b"); 
				var standingCoverStrafeRightAnimationClip46654 = AnimatorBuilder.GetAnimationClip(standingCoverStrafeRightAnimationClip46654Path, "StandingCoverStrafeRight");
				var standingCoverStartAnimationClip46664Path = AssetDatabase.GUIDToAssetPath("c5894cc6de53e9a4c89e74f1f19864b3"); 
				var standingCoverStartAnimationClip46664 = AnimatorBuilder.GetAnimationClip(standingCoverStartAnimationClip46664Path, "StandingCoverStart");

				// State Machine.
				var coverAnimatorStateMachine43420 = baseStateMachine1483310764.AddStateMachine("Cover", new Vector3(624f, 60f, 0f));

				// State Machine.
				var crouchingCoverAnimatorStateMachine46410 = coverAnimatorStateMachine43420.AddStateMachine("Crouching Cover", new Vector3(24f, -204f, 0f));

				// States.
				var coverCenterAimLeftAimAnimatorState46414 = crouchingCoverAnimatorStateMachine46410.AddState("Cover Center Aim Left Aim", new Vector3(-96f, 288f, 0f));
				coverCenterAimLeftAimAnimatorState46414.motion = crouchingCoverCenterLeftAimAnimationClip46448;
				coverCenterAimLeftAimAnimatorState46414.cycleOffset = 0f;
				coverCenterAimLeftAimAnimatorState46414.cycleOffsetParameterActive = false;
				coverCenterAimLeftAimAnimatorState46414.iKOnFeet = true;
				coverCenterAimLeftAimAnimatorState46414.mirror = false;
				coverCenterAimLeftAimAnimatorState46414.mirrorParameterActive = false;
				coverCenterAimLeftAimAnimatorState46414.speed = 1.5f;
				coverCenterAimLeftAimAnimatorState46414.speedParameterActive = false;
				coverCenterAimLeftAimAnimatorState46414.writeDefaultValues = true;

				var coverCenterAimLeftReturnAnimatorState46416 = crouchingCoverAnimatorStateMachine46410.AddState("Cover Center Aim Left Return", new Vector3(-336f, 288f, 0f));
				coverCenterAimLeftReturnAnimatorState46416.motion = crouchingCoverCenterLeftAimReturnAnimationClip46456;
				coverCenterAimLeftReturnAnimatorState46416.cycleOffset = 0f;
				coverCenterAimLeftReturnAnimatorState46416.cycleOffsetParameterActive = false;
				coverCenterAimLeftReturnAnimatorState46416.iKOnFeet = true;
				coverCenterAimLeftReturnAnimatorState46416.mirror = false;
				coverCenterAimLeftReturnAnimatorState46416.mirrorParameterActive = false;
				coverCenterAimLeftReturnAnimatorState46416.speed = 1.8f;
				coverCenterAimLeftReturnAnimatorState46416.speedParameterActive = false;
				coverCenterAimLeftReturnAnimatorState46416.writeDefaultValues = true;

				var crouchingCoverAimLeftHoldAnimatorState46418 = crouchingCoverAnimatorStateMachine46410.AddState("Crouching Cover Aim Left Hold", new Vector3(-576f, 96f, 0f));
				crouchingCoverAimLeftHoldAnimatorState46418.motion = crouchingCoverAimLeftHoldAnimationClip46462;
				crouchingCoverAimLeftHoldAnimatorState46418.cycleOffset = 0f;
				crouchingCoverAimLeftHoldAnimatorState46418.cycleOffsetParameterActive = false;
				crouchingCoverAimLeftHoldAnimatorState46418.iKOnFeet = true;
				crouchingCoverAimLeftHoldAnimatorState46418.mirror = false;
				crouchingCoverAimLeftHoldAnimatorState46418.mirrorParameterActive = false;
				crouchingCoverAimLeftHoldAnimatorState46418.speed = 1f;
				crouchingCoverAimLeftHoldAnimatorState46418.speedParameterActive = false;
				crouchingCoverAimLeftHoldAnimatorState46418.writeDefaultValues = true;

				var crouchingCoverAimRightAnimatorState46420 = crouchingCoverAnimatorStateMachine46410.AddState("Crouching Cover Aim Right", new Vector3(384f, 156f, 0f));
				crouchingCoverAimRightAnimatorState46420.motion = crouchingCoverAimRightAnimationClip46470;
				crouchingCoverAimRightAnimatorState46420.cycleOffset = 0f;
				crouchingCoverAimRightAnimatorState46420.cycleOffsetParameterActive = false;
				crouchingCoverAimRightAnimatorState46420.iKOnFeet = true;
				crouchingCoverAimRightAnimatorState46420.mirror = false;
				crouchingCoverAimRightAnimatorState46420.mirrorParameterActive = false;
				crouchingCoverAimRightAnimatorState46420.speed = 1.5f;
				crouchingCoverAimRightAnimatorState46420.speedParameterActive = false;
				crouchingCoverAimRightAnimatorState46420.writeDefaultValues = true;

				var crouchingCoverAimRightHoldAnimatorState46422 = crouchingCoverAnimatorStateMachine46410.AddState("Crouching Cover Aim Right Hold", new Vector3(636f, 96f, 0f));
				crouchingCoverAimRightHoldAnimatorState46422.motion = crouchingCoverAimRightHoldAnimationClip46476;
				crouchingCoverAimRightHoldAnimatorState46422.cycleOffset = 0f;
				crouchingCoverAimRightHoldAnimatorState46422.cycleOffsetParameterActive = false;
				crouchingCoverAimRightHoldAnimatorState46422.iKOnFeet = true;
				crouchingCoverAimRightHoldAnimatorState46422.mirror = false;
				crouchingCoverAimRightHoldAnimatorState46422.mirrorParameterActive = false;
				crouchingCoverAimRightHoldAnimatorState46422.speed = 1f;
				crouchingCoverAimRightHoldAnimatorState46422.speedParameterActive = false;
				crouchingCoverAimRightHoldAnimatorState46422.writeDefaultValues = true;

				var crouchingCoverAimRightReturnAnimatorState46424 = crouchingCoverAnimatorStateMachine46410.AddState("Crouching Cover Aim Right Return", new Vector3(384f, 36f, 0f));
				crouchingCoverAimRightReturnAnimatorState46424.motion = crouchingCoverAimRightReturnAnimationClip46486;
				crouchingCoverAimRightReturnAnimatorState46424.cycleOffset = 0f;
				crouchingCoverAimRightReturnAnimatorState46424.cycleOffsetParameterActive = false;
				crouchingCoverAimRightReturnAnimatorState46424.iKOnFeet = true;
				crouchingCoverAimRightReturnAnimatorState46424.mirror = false;
				crouchingCoverAimRightReturnAnimatorState46424.mirrorParameterActive = false;
				crouchingCoverAimRightReturnAnimatorState46424.speed = 1.8f;
				crouchingCoverAimRightReturnAnimatorState46424.speedParameterActive = false;
				crouchingCoverAimRightReturnAnimatorState46424.writeDefaultValues = true;

				var crouchingCoverIdleLeftAnimatorState46426 = crouchingCoverAnimatorStateMachine46410.AddState("Crouching Cover Idle Left", new Vector3(-156f, -84f, 0f));
				crouchingCoverIdleLeftAnimatorState46426.motion = crouchingCoverIdleLeftAnimationClip46500;
				crouchingCoverIdleLeftAnimatorState46426.cycleOffset = 0f;
				crouchingCoverIdleLeftAnimatorState46426.cycleOffsetParameterActive = false;
				crouchingCoverIdleLeftAnimatorState46426.iKOnFeet = false;
				crouchingCoverIdleLeftAnimatorState46426.mirror = false;
				crouchingCoverIdleLeftAnimatorState46426.mirrorParameterActive = false;
				crouchingCoverIdleLeftAnimatorState46426.speed = 1f;
				crouchingCoverIdleLeftAnimatorState46426.speedParameterActive = false;
				crouchingCoverIdleLeftAnimatorState46426.writeDefaultValues = true;

				var coverCenterAimRightHoldAnimatorState46428 = crouchingCoverAnimatorStateMachine46410.AddState("Cover Center Aim Right Hold", new Vector3(264f, 396f, 0f));
				coverCenterAimRightHoldAnimatorState46428.motion = crouchingCoverCenterRightAimHoldAnimationClip46506;
				coverCenterAimRightHoldAnimatorState46428.cycleOffset = 0f;
				coverCenterAimRightHoldAnimatorState46428.cycleOffsetParameterActive = false;
				coverCenterAimRightHoldAnimatorState46428.iKOnFeet = true;
				coverCenterAimRightHoldAnimatorState46428.mirror = false;
				coverCenterAimRightHoldAnimatorState46428.mirrorParameterActive = false;
				coverCenterAimRightHoldAnimatorState46428.speed = 1f;
				coverCenterAimRightHoldAnimatorState46428.speedParameterActive = false;
				coverCenterAimRightHoldAnimatorState46428.writeDefaultValues = true;

				var coverCenterAimRightReturnAnimatorState46430 = crouchingCoverAnimatorStateMachine46410.AddState("Cover Center Aim Right Return", new Vector3(384f, 288f, 0f));
				coverCenterAimRightReturnAnimatorState46430.motion = crouchingCoverCenterRightAimReturnAnimationClip46514;
				coverCenterAimRightReturnAnimatorState46430.cycleOffset = 0f;
				coverCenterAimRightReturnAnimatorState46430.cycleOffsetParameterActive = false;
				coverCenterAimRightReturnAnimatorState46430.iKOnFeet = true;
				coverCenterAimRightReturnAnimatorState46430.mirror = false;
				coverCenterAimRightReturnAnimatorState46430.mirrorParameterActive = false;
				coverCenterAimRightReturnAnimatorState46430.speed = 1.8f;
				coverCenterAimRightReturnAnimatorState46430.speedParameterActive = false;
				coverCenterAimRightReturnAnimatorState46430.writeDefaultValues = true;

				var coverCenterAimRightAimAnimatorState46432 = crouchingCoverAnimatorStateMachine46410.AddState("Cover Center Aim Right Aim", new Vector3(144f, 288f, 0f));
				coverCenterAimRightAimAnimatorState46432.motion = crouchingCoverCenterRightAimAnimationClip46520;
				coverCenterAimRightAimAnimatorState46432.cycleOffset = 0f;
				coverCenterAimRightAimAnimatorState46432.cycleOffsetParameterActive = false;
				coverCenterAimRightAimAnimatorState46432.iKOnFeet = true;
				coverCenterAimRightAimAnimatorState46432.mirror = false;
				coverCenterAimRightAimAnimatorState46432.mirrorParameterActive = false;
				coverCenterAimRightAimAnimatorState46432.speed = 1.5f;
				coverCenterAimRightAimAnimatorState46432.speedParameterActive = false;
				coverCenterAimRightAimAnimatorState46432.writeDefaultValues = true;

				var crouchingCoverIdleRightAnimatorState46434 = crouchingCoverAnimatorStateMachine46410.AddState("Crouching Cover Idle Right", new Vector3(204f, -84f, 0f));
				crouchingCoverIdleRightAnimatorState46434.motion = crouchingCoverIdleRightAnimationClip46534;
				crouchingCoverIdleRightAnimatorState46434.cycleOffset = 0f;
				crouchingCoverIdleRightAnimatorState46434.cycleOffsetParameterActive = false;
				crouchingCoverIdleRightAnimatorState46434.iKOnFeet = false;
				crouchingCoverIdleRightAnimatorState46434.mirror = false;
				crouchingCoverIdleRightAnimatorState46434.mirrorParameterActive = false;
				crouchingCoverIdleRightAnimatorState46434.speed = 1f;
				crouchingCoverIdleRightAnimatorState46434.speedParameterActive = false;
				crouchingCoverIdleRightAnimatorState46434.writeDefaultValues = true;

				var takeCrouchingCoverAnimatorState44144 = crouchingCoverAnimatorStateMachine46410.AddState("Take Crouching Cover", new Vector3(24f, -264f, 0f));
				takeCrouchingCoverAnimatorState44144.motion = crouchingCoverStartAnimationClip46546;
				takeCrouchingCoverAnimatorState44144.cycleOffset = 0f;
				takeCrouchingCoverAnimatorState44144.cycleOffsetParameterActive = false;
				takeCrouchingCoverAnimatorState44144.iKOnFeet = true;
				takeCrouchingCoverAnimatorState44144.mirror = false;
				takeCrouchingCoverAnimatorState44144.mirrorParameterActive = false;
				takeCrouchingCoverAnimatorState44144.speed = 2f;
				takeCrouchingCoverAnimatorState44144.speedParameterActive = false;
				takeCrouchingCoverAnimatorState44144.writeDefaultValues = true;

				var crouchingCoverStrafeAnimatorState46436 = crouchingCoverAnimatorStateMachine46410.AddState("Crouching Cover Strafe", new Vector3(24f, 96f, 0f));
				var crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562 = new BlendTree();
				AssetDatabase.AddObjectToAsset(crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562, animatorControllers[i]);
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562.hideFlags = HideFlags.HideInHierarchy;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562.blendParameter = "HorizontalMovement";
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562.blendParameterY = "AbilityFloatData";
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562.blendType = BlendTreeType.FreeformCartesian2D;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562.maxThreshold = 3f;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562.minThreshold = 0f;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562.name = "Blend Tree";
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562.useAutomaticThresholds = false;
				var crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child0 =  new ChildMotion();
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child0.motion = crouchingCoverStrafeLeftAnimationClip46566;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child0.cycleOffset = 0f;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child0.directBlendParameter = "HorizontalMovement";
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child0.mirror = false;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child0.position = new Vector2(-1f, 0f);
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child0.threshold = 0f;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child0.timeScale = 1f;
				var crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child1 =  new ChildMotion();
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child1.motion = crouchingCoverIdleLeftAnimationClip46500;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child1.cycleOffset = 0f;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child1.directBlendParameter = "HorizontalMovement";
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child1.mirror = false;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child1.position = new Vector2(0f, 0f);
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child1.threshold = 1f;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child1.timeScale = 1f;
				var crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child2 =  new ChildMotion();
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child2.motion = crouchingCoverIdleRightAnimationClip46534;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child2.cycleOffset = 0f;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child2.directBlendParameter = "HorizontalMovement";
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child2.mirror = false;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child2.position = new Vector2(0f, 1f);
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child2.threshold = 2f;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child2.timeScale = 1f;
				var crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child3 =  new ChildMotion();
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child3.motion = crouchingCoverStrafeRightAnimationClip46568;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child3.cycleOffset = 0f;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child3.directBlendParameter = "HorizontalMovement";
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child3.mirror = false;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child3.position = new Vector2(1f, 1f);
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child3.threshold = 3f;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child3.timeScale = 1f;
				crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562.children = new ChildMotion[] {
					crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child0,
					crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child1,
					crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child2,
					crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562Child3
				};
				crouchingCoverStrafeAnimatorState46436.motion = crouchingCoverStrafeAnimatorState46436blendTreeBlendTree46562;
				crouchingCoverStrafeAnimatorState46436.cycleOffset = 0f;
				crouchingCoverStrafeAnimatorState46436.cycleOffsetParameterActive = false;
				crouchingCoverStrafeAnimatorState46436.iKOnFeet = true;
				crouchingCoverStrafeAnimatorState46436.mirror = false;
				crouchingCoverStrafeAnimatorState46436.mirrorParameterActive = false;
				crouchingCoverStrafeAnimatorState46436.speed = 1f;
				crouchingCoverStrafeAnimatorState46436.speedParameterActive = false;
				crouchingCoverStrafeAnimatorState46436.writeDefaultValues = true;

				var crouchingCoverAimLeftAnimatorState46438 = crouchingCoverAnimatorStateMachine46410.AddState("Crouching Cover Aim Left", new Vector3(-336f, 156f, 0f));
				crouchingCoverAimLeftAnimatorState46438.motion = crouchingCoverAimLeftAnimationClip46574;
				crouchingCoverAimLeftAnimatorState46438.cycleOffset = 0f;
				crouchingCoverAimLeftAnimatorState46438.cycleOffsetParameterActive = false;
				crouchingCoverAimLeftAnimatorState46438.iKOnFeet = true;
				crouchingCoverAimLeftAnimatorState46438.mirror = false;
				crouchingCoverAimLeftAnimatorState46438.mirrorParameterActive = false;
				crouchingCoverAimLeftAnimatorState46438.speed = 1.5f;
				crouchingCoverAimLeftAnimatorState46438.speedParameterActive = false;
				crouchingCoverAimLeftAnimatorState46438.writeDefaultValues = true;

				var coverCenterAimLeftHoldAnimatorState46440 = crouchingCoverAnimatorStateMachine46410.AddState("Cover Center Aim Left Hold", new Vector3(-216f, 396f, 0f));
				coverCenterAimLeftHoldAnimatorState46440.motion = crouchingCoverCenterLeftAimHoldAnimationClip46578;
				coverCenterAimLeftHoldAnimatorState46440.cycleOffset = 0f;
				coverCenterAimLeftHoldAnimatorState46440.cycleOffsetParameterActive = false;
				coverCenterAimLeftHoldAnimatorState46440.iKOnFeet = true;
				coverCenterAimLeftHoldAnimatorState46440.mirror = false;
				coverCenterAimLeftHoldAnimatorState46440.mirrorParameterActive = false;
				coverCenterAimLeftHoldAnimatorState46440.speed = 1f;
				coverCenterAimLeftHoldAnimatorState46440.speedParameterActive = false;
				coverCenterAimLeftHoldAnimatorState46440.writeDefaultValues = true;

				var crouchingCoverAimLeftReturnAnimatorState46442 = crouchingCoverAnimatorStateMachine46410.AddState("Crouching Cover Aim Left Return", new Vector3(-336f, 36f, 0f));
				crouchingCoverAimLeftReturnAnimatorState46442.motion = crouchingCoverAimLeftReturnAnimationClip46586;
				crouchingCoverAimLeftReturnAnimatorState46442.cycleOffset = 0f;
				crouchingCoverAimLeftReturnAnimatorState46442.cycleOffsetParameterActive = false;
				crouchingCoverAimLeftReturnAnimatorState46442.iKOnFeet = true;
				crouchingCoverAimLeftReturnAnimatorState46442.mirror = false;
				crouchingCoverAimLeftReturnAnimatorState46442.mirrorParameterActive = false;
				crouchingCoverAimLeftReturnAnimatorState46442.speed = 1.8f;
				crouchingCoverAimLeftReturnAnimatorState46442.speedParameterActive = false;
				crouchingCoverAimLeftReturnAnimatorState46442.writeDefaultValues = true;

				// State Machine Defaults.
				crouchingCoverAnimatorStateMachine46410.anyStatePosition = new Vector3(-564f, -252f, 0f);
				crouchingCoverAnimatorStateMachine46410.defaultState = takeCrouchingCoverAnimatorState44144;
				crouchingCoverAnimatorStateMachine46410.entryPosition = new Vector3(-564f, -300f, 0f);
				crouchingCoverAnimatorStateMachine46410.exitPosition = new Vector3(768f, -72f, 0f);
				crouchingCoverAnimatorStateMachine46410.parentStateMachinePosition = new Vector3(384f, -264f, 0f);

				// State Machine.
				var standingCoverAnimatorStateMachine46412 = coverAnimatorStateMachine43420.AddStateMachine("Standing Cover", new Vector3(24f, -312f, 0f));

				// States.
				var standingAimRightReturnAnimatorState46588 = standingCoverAnimatorStateMachine46412.AddState("Standing Aim Right Return", new Vector3(624f, 96f, 0f));
				standingAimRightReturnAnimatorState46588.motion = standingCoverAimRightReturnAnimationClip46602;
				standingAimRightReturnAnimatorState46588.cycleOffset = 0f;
				standingAimRightReturnAnimatorState46588.cycleOffsetParameterActive = false;
				standingAimRightReturnAnimatorState46588.iKOnFeet = true;
				standingAimRightReturnAnimatorState46588.mirror = false;
				standingAimRightReturnAnimatorState46588.mirrorParameterActive = false;
				standingAimRightReturnAnimatorState46588.speed = 1.8f;
				standingAimRightReturnAnimatorState46588.speedParameterActive = false;
				standingAimRightReturnAnimatorState46588.writeDefaultValues = true;

				var standingCoverAimRightHoldAnimatorState46478 = standingCoverAnimatorStateMachine46412.AddState("Standing Cover Aim Right Hold", new Vector3(864f, 36f, 0f));
				standingCoverAimRightHoldAnimatorState46478.motion = standingCoverAimRightHoldAnimationClip46608;
				standingCoverAimRightHoldAnimatorState46478.cycleOffset = 0f;
				standingCoverAimRightHoldAnimatorState46478.cycleOffsetParameterActive = false;
				standingCoverAimRightHoldAnimatorState46478.iKOnFeet = true;
				standingCoverAimRightHoldAnimatorState46478.mirror = false;
				standingCoverAimRightHoldAnimatorState46478.mirrorParameterActive = false;
				standingCoverAimRightHoldAnimatorState46478.speed = 1f;
				standingCoverAimRightHoldAnimatorState46478.speedParameterActive = false;
				standingCoverAimRightHoldAnimatorState46478.writeDefaultValues = true;

				var standingCoverAimRightAnimatorState46590 = standingCoverAnimatorStateMachine46412.AddState("Standing Cover Aim Right", new Vector3(624f, -24f, 0f));
				standingCoverAimRightAnimatorState46590.motion = standingCoverAimRightAnimationClip46614;
				standingCoverAimRightAnimatorState46590.cycleOffset = 0f;
				standingCoverAimRightAnimatorState46590.cycleOffsetParameterActive = false;
				standingCoverAimRightAnimatorState46590.iKOnFeet = true;
				standingCoverAimRightAnimatorState46590.mirror = false;
				standingCoverAimRightAnimatorState46590.mirrorParameterActive = false;
				standingCoverAimRightAnimatorState46590.speed = 1.5f;
				standingCoverAimRightAnimatorState46590.speedParameterActive = false;
				standingCoverAimRightAnimatorState46590.writeDefaultValues = true;

				var standingCoverAimLeftHoldAnimatorState46464 = standingCoverAnimatorStateMachine46412.AddState("Standing Cover Aim Left Hold", new Vector3(-348f, 36f, 0f));
				standingCoverAimLeftHoldAnimatorState46464.motion = standingCoverAimLeftHoldAnimationClip46620;
				standingCoverAimLeftHoldAnimatorState46464.cycleOffset = 0f;
				standingCoverAimLeftHoldAnimatorState46464.cycleOffsetParameterActive = false;
				standingCoverAimLeftHoldAnimatorState46464.iKOnFeet = true;
				standingCoverAimLeftHoldAnimatorState46464.mirror = false;
				standingCoverAimLeftHoldAnimatorState46464.mirrorParameterActive = false;
				standingCoverAimLeftHoldAnimatorState46464.speed = 1f;
				standingCoverAimLeftHoldAnimatorState46464.speedParameterActive = false;
				standingCoverAimLeftHoldAnimatorState46464.writeDefaultValues = true;

				var standingCoverAimLeftReturnAnimatorState46592 = standingCoverAnimatorStateMachine46412.AddState("Standing Cover Aim Left Return", new Vector3(-96f, 96f, 0f));
				standingCoverAimLeftReturnAnimatorState46592.motion = standingCoverAimLeftReturnAnimationClip46628;
				standingCoverAimLeftReturnAnimatorState46592.cycleOffset = 0f;
				standingCoverAimLeftReturnAnimatorState46592.cycleOffsetParameterActive = false;
				standingCoverAimLeftReturnAnimatorState46592.iKOnFeet = true;
				standingCoverAimLeftReturnAnimatorState46592.mirror = false;
				standingCoverAimLeftReturnAnimatorState46592.mirrorParameterActive = false;
				standingCoverAimLeftReturnAnimatorState46592.speed = 1.8f;
				standingCoverAimLeftReturnAnimatorState46592.speedParameterActive = false;
				standingCoverAimLeftReturnAnimatorState46592.writeDefaultValues = true;

				var standingCoverAimLeftAnimatorState46594 = standingCoverAnimatorStateMachine46412.AddState("Standing Cover Aim Left", new Vector3(-96f, -24f, 0f));
				standingCoverAimLeftAnimatorState46594.motion = standingCoverAimLeftAnimationClip46634;
				standingCoverAimLeftAnimatorState46594.cycleOffset = 0f;
				standingCoverAimLeftAnimatorState46594.cycleOffsetParameterActive = false;
				standingCoverAimLeftAnimatorState46594.iKOnFeet = true;
				standingCoverAimLeftAnimatorState46594.mirror = false;
				standingCoverAimLeftAnimatorState46594.mirrorParameterActive = false;
				standingCoverAimLeftAnimatorState46594.speed = 1.5f;
				standingCoverAimLeftAnimatorState46594.speedParameterActive = false;
				standingCoverAimLeftAnimatorState46594.writeDefaultValues = true;

				var standingCoverStrafeAnimatorState46564 = standingCoverAnimatorStateMachine46412.AddState("Standing Cover Strafe", new Vector3(264f, 36f, 0f));
				var standingCoverStrafeAnimatorState46564blendTreeBlendTree46646 = new BlendTree();
				AssetDatabase.AddObjectToAsset(standingCoverStrafeAnimatorState46564blendTreeBlendTree46646, animatorControllers[i]);
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646.hideFlags = HideFlags.HideInHierarchy;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646.blendParameter = "HorizontalMovement";
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646.blendParameterY = "AbilityFloatData";
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646.blendType = BlendTreeType.FreeformCartesian2D;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646.maxThreshold = 3f;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646.minThreshold = 0f;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646.name = "Blend Tree";
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646.useAutomaticThresholds = false;
				var standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child0 =  new ChildMotion();
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child0.motion = standingCoverStrafeLeftAnimationClip46648;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child0.cycleOffset = 0f;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child0.directBlendParameter = "HorizontalMovement";
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child0.mirror = false;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child0.position = new Vector2(-1f, 0f);
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child0.threshold = 0f;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child0.timeScale = 1f;
				var standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child1 =  new ChildMotion();
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child1.motion = standingCoverIdleLeftAnimationClip46650;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child1.cycleOffset = 0f;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child1.directBlendParameter = "HorizontalMovement";
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child1.mirror = false;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child1.position = new Vector2(0f, 0f);
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child1.threshold = 1f;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child1.timeScale = 1f;
				var standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child2 =  new ChildMotion();
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child2.motion = standingCoverIdleRightAnimationClip46652;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child2.cycleOffset = 0f;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child2.directBlendParameter = "HorizontalMovement";
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child2.mirror = false;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child2.position = new Vector2(0f, 1f);
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child2.threshold = 2f;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child2.timeScale = 1f;
				var standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child3 =  new ChildMotion();
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child3.motion = standingCoverStrafeRightAnimationClip46654;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child3.cycleOffset = 0f;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child3.directBlendParameter = "HorizontalMovement";
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child3.mirror = false;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child3.position = new Vector2(1f, 1f);
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child3.threshold = 3f;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child3.timeScale = 1f;
				standingCoverStrafeAnimatorState46564blendTreeBlendTree46646.children = new ChildMotion[] {
					standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child0,
					standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child1,
					standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child2,
					standingCoverStrafeAnimatorState46564blendTreeBlendTree46646Child3
				};
				standingCoverStrafeAnimatorState46564.motion = standingCoverStrafeAnimatorState46564blendTreeBlendTree46646;
				standingCoverStrafeAnimatorState46564.cycleOffset = 0f;
				standingCoverStrafeAnimatorState46564.cycleOffsetParameterActive = false;
				standingCoverStrafeAnimatorState46564.iKOnFeet = true;
				standingCoverStrafeAnimatorState46564.mirror = false;
				standingCoverStrafeAnimatorState46564.mirrorParameterActive = false;
				standingCoverStrafeAnimatorState46564.speed = 1f;
				standingCoverStrafeAnimatorState46564.speedParameterActive = false;
				standingCoverStrafeAnimatorState46564.writeDefaultValues = true;

				var takeStandingCoverAnimatorState44146 = standingCoverAnimatorStateMachine46412.AddState("Take Standing Cover", new Vector3(264f, -324f, 0f));
				takeStandingCoverAnimatorState44146.motion = standingCoverStartAnimationClip46664;
				takeStandingCoverAnimatorState44146.cycleOffset = 0f;
				takeStandingCoverAnimatorState44146.cycleOffsetParameterActive = false;
				takeStandingCoverAnimatorState44146.iKOnFeet = true;
				takeStandingCoverAnimatorState44146.mirror = false;
				takeStandingCoverAnimatorState44146.mirrorParameterActive = false;
				takeStandingCoverAnimatorState44146.speed = 2f;
				takeStandingCoverAnimatorState44146.speedParameterActive = false;
				takeStandingCoverAnimatorState44146.writeDefaultValues = true;

				var standingCoverIdleRightAnimatorState46536 = standingCoverAnimatorStateMachine46412.AddState("Standing Cover Idle Right", new Vector3(444f, -204f, 0f));
				standingCoverIdleRightAnimatorState46536.motion = standingCoverIdleRightAnimationClip46652;
				standingCoverIdleRightAnimatorState46536.cycleOffset = 0f;
				standingCoverIdleRightAnimatorState46536.cycleOffsetParameterActive = false;
				standingCoverIdleRightAnimatorState46536.iKOnFeet = false;
				standingCoverIdleRightAnimatorState46536.mirror = false;
				standingCoverIdleRightAnimatorState46536.mirrorParameterActive = false;
				standingCoverIdleRightAnimatorState46536.speed = 1f;
				standingCoverIdleRightAnimatorState46536.speedParameterActive = false;
				standingCoverIdleRightAnimatorState46536.writeDefaultValues = true;

				var standingCoverIdleLeftAnimatorState46502 = standingCoverAnimatorStateMachine46412.AddState("Standing Cover Idle Left", new Vector3(96f, -204f, 0f));
				standingCoverIdleLeftAnimatorState46502.motion = standingCoverIdleLeftAnimationClip46650;
				standingCoverIdleLeftAnimatorState46502.cycleOffset = 0f;
				standingCoverIdleLeftAnimatorState46502.cycleOffsetParameterActive = false;
				standingCoverIdleLeftAnimatorState46502.iKOnFeet = false;
				standingCoverIdleLeftAnimatorState46502.mirror = false;
				standingCoverIdleLeftAnimatorState46502.mirrorParameterActive = false;
				standingCoverIdleLeftAnimatorState46502.speed = 1f;
				standingCoverIdleLeftAnimatorState46502.speedParameterActive = false;
				standingCoverIdleLeftAnimatorState46502.writeDefaultValues = true;

				// State Machine Defaults.
				standingCoverAnimatorStateMachine46412.anyStatePosition = new Vector3(-444f, -324f, 0f);
				standingCoverAnimatorStateMachine46412.defaultState = takeStandingCoverAnimatorState44146;
				standingCoverAnimatorStateMachine46412.entryPosition = new Vector3(-444f, -372f, 0f);
				standingCoverAnimatorStateMachine46412.exitPosition = new Vector3(1008f, -84f, 0f);
				standingCoverAnimatorStateMachine46412.parentStateMachinePosition = new Vector3(744f, -324f, 0f);

				// State Machine Defaults.
				coverAnimatorStateMachine43420.anyStatePosition = new Vector3(-312f, -288f, 0f);
				coverAnimatorStateMachine43420.defaultState = takeStandingCoverAnimatorState44146;
				coverAnimatorStateMachine43420.entryPosition = new Vector3(-312f, -336f, 0f);
				coverAnimatorStateMachine43420.exitPosition = new Vector3(408f, -192f, 0f);
				coverAnimatorStateMachine43420.parentStateMachinePosition = new Vector3(396f, -288f, 0f);

				// State Transitions.
				var animatorStateTransition46444 = coverCenterAimLeftAimAnimatorState46414.AddTransition(coverCenterAimLeftHoldAnimatorState46440);
				animatorStateTransition46444.canTransitionToSelf = true;
				animatorStateTransition46444.duration = 0.01f;
				animatorStateTransition46444.exitTime = 1f;
				animatorStateTransition46444.hasExitTime = true;
				animatorStateTransition46444.hasFixedDuration = false;
				animatorStateTransition46444.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46444.offset = 0f;
				animatorStateTransition46444.orderedInterruption = true;
				animatorStateTransition46444.isExit = false;
				animatorStateTransition46444.mute = false;
				animatorStateTransition46444.solo = false;
				animatorStateTransition46444.AddCondition(AnimatorConditionMode.Equals, 5f, "AbilityIntData");

				var animatorStateTransition46446 = coverCenterAimLeftAimAnimatorState46414.AddTransition(crouchingCoverIdleLeftAnimatorState46426);
				animatorStateTransition46446.canTransitionToSelf = true;
				animatorStateTransition46446.duration = 0.01f;
				animatorStateTransition46446.exitTime = 0.625f;
				animatorStateTransition46446.hasExitTime = false;
				animatorStateTransition46446.hasFixedDuration = true;
				animatorStateTransition46446.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46446.offset = 0f;
				animatorStateTransition46446.orderedInterruption = true;
				animatorStateTransition46446.isExit = false;
				animatorStateTransition46446.mute = false;
				animatorStateTransition46446.solo = false;
				animatorStateTransition46446.AddCondition(AnimatorConditionMode.NotEqual, 5f, "AbilityIntData");

				var animatorStateTransition46450 = coverCenterAimLeftReturnAnimatorState46416.AddTransition(crouchingCoverStrafeAnimatorState46436);
				animatorStateTransition46450.canTransitionToSelf = true;
				animatorStateTransition46450.duration = 0.01f;
				animatorStateTransition46450.exitTime = 0.92f;
				animatorStateTransition46450.hasExitTime = true;
				animatorStateTransition46450.hasFixedDuration = false;
				animatorStateTransition46450.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46450.offset = 0f;
				animatorStateTransition46450.orderedInterruption = true;
				animatorStateTransition46450.isExit = false;
				animatorStateTransition46450.mute = false;
				animatorStateTransition46450.solo = false;
				animatorStateTransition46450.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

				var animatorStateTransition46452 = coverCenterAimLeftReturnAnimatorState46416.AddTransition(crouchingCoverIdleLeftAnimatorState46426);
				animatorStateTransition46452.canTransitionToSelf = true;
				animatorStateTransition46452.duration = 0.01f;
				animatorStateTransition46452.exitTime = 0.92f;
				animatorStateTransition46452.hasExitTime = true;
				animatorStateTransition46452.hasFixedDuration = false;
				animatorStateTransition46452.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46452.offset = 0f;
				animatorStateTransition46452.orderedInterruption = true;
				animatorStateTransition46452.isExit = false;
				animatorStateTransition46452.mute = false;
				animatorStateTransition46452.solo = false;
				animatorStateTransition46452.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
				animatorStateTransition46452.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

				var animatorStateTransition46454 = coverCenterAimLeftReturnAnimatorState46416.AddTransition(crouchingCoverStrafeAnimatorState46436);
				animatorStateTransition46454.canTransitionToSelf = true;
				animatorStateTransition46454.duration = 0.01f;
				animatorStateTransition46454.exitTime = 0.92f;
				animatorStateTransition46454.hasExitTime = true;
				animatorStateTransition46454.hasFixedDuration = false;
				animatorStateTransition46454.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46454.offset = 0f;
				animatorStateTransition46454.orderedInterruption = true;
				animatorStateTransition46454.isExit = false;
				animatorStateTransition46454.mute = false;
				animatorStateTransition46454.solo = false;
				animatorStateTransition46454.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

				var animatorStateTransition46458 = crouchingCoverAimLeftHoldAnimatorState46418.AddTransition(crouchingCoverAimLeftReturnAnimatorState46442);
				animatorStateTransition46458.canTransitionToSelf = true;
				animatorStateTransition46458.duration = 0.01f;
				animatorStateTransition46458.exitTime = 0.9f;
				animatorStateTransition46458.hasExitTime = false;
				animatorStateTransition46458.hasFixedDuration = false;
				animatorStateTransition46458.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46458.offset = 0f;
				animatorStateTransition46458.orderedInterruption = true;
				animatorStateTransition46458.isExit = false;
				animatorStateTransition46458.mute = false;
				animatorStateTransition46458.solo = false;
				animatorStateTransition46458.AddCondition(AnimatorConditionMode.NotEqual, 3f, "AbilityIntData");

				var animatorStateTransition46460 = crouchingCoverAimLeftHoldAnimatorState46418.AddTransition(standingCoverAimLeftHoldAnimatorState46464);
				animatorStateTransition46460.canTransitionToSelf = true;
				animatorStateTransition46460.duration = 0.1f;
				animatorStateTransition46460.exitTime = 0f;
				animatorStateTransition46460.hasExitTime = false;
				animatorStateTransition46460.hasFixedDuration = true;
				animatorStateTransition46460.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46460.offset = 0f;
				animatorStateTransition46460.orderedInterruption = true;
				animatorStateTransition46460.isExit = false;
				animatorStateTransition46460.mute = false;
				animatorStateTransition46460.solo = false;
				animatorStateTransition46460.AddCondition(AnimatorConditionMode.Less, 0.5f, "Height");

				var animatorStateTransition46466 = crouchingCoverAimRightAnimatorState46420.AddTransition(crouchingCoverAimRightHoldAnimatorState46422);
				animatorStateTransition46466.canTransitionToSelf = true;
				animatorStateTransition46466.duration = 0.01f;
				animatorStateTransition46466.exitTime = 1f;
				animatorStateTransition46466.hasExitTime = true;
				animatorStateTransition46466.hasFixedDuration = false;
				animatorStateTransition46466.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46466.offset = 0f;
				animatorStateTransition46466.orderedInterruption = true;
				animatorStateTransition46466.isExit = false;
				animatorStateTransition46466.mute = false;
				animatorStateTransition46466.solo = false;
				animatorStateTransition46466.AddCondition(AnimatorConditionMode.Equals, 4f, "AbilityIntData");

				var animatorStateTransition46468 = crouchingCoverAimRightAnimatorState46420.AddTransition(crouchingCoverIdleRightAnimatorState46434);
				animatorStateTransition46468.canTransitionToSelf = true;
				animatorStateTransition46468.duration = 0.01f;
				animatorStateTransition46468.exitTime = 0.625f;
				animatorStateTransition46468.hasExitTime = false;
				animatorStateTransition46468.hasFixedDuration = true;
				animatorStateTransition46468.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46468.offset = 0f;
				animatorStateTransition46468.orderedInterruption = true;
				animatorStateTransition46468.isExit = false;
				animatorStateTransition46468.mute = false;
				animatorStateTransition46468.solo = false;
				animatorStateTransition46468.AddCondition(AnimatorConditionMode.NotEqual, 4f, "AbilityIntData");

				var animatorStateTransition46472 = crouchingCoverAimRightHoldAnimatorState46422.AddTransition(crouchingCoverAimRightReturnAnimatorState46424);
				animatorStateTransition46472.canTransitionToSelf = true;
				animatorStateTransition46472.duration = 0.01f;
				animatorStateTransition46472.exitTime = 0.9f;
				animatorStateTransition46472.hasExitTime = false;
				animatorStateTransition46472.hasFixedDuration = false;
				animatorStateTransition46472.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46472.offset = 0f;
				animatorStateTransition46472.orderedInterruption = true;
				animatorStateTransition46472.isExit = false;
				animatorStateTransition46472.mute = false;
				animatorStateTransition46472.solo = false;
				animatorStateTransition46472.AddCondition(AnimatorConditionMode.NotEqual, 4f, "AbilityIntData");

				var animatorStateTransition46474 = crouchingCoverAimRightHoldAnimatorState46422.AddTransition(standingCoverAimRightHoldAnimatorState46478);
				animatorStateTransition46474.canTransitionToSelf = true;
				animatorStateTransition46474.duration = 0.1f;
				animatorStateTransition46474.exitTime = 0f;
				animatorStateTransition46474.hasExitTime = false;
				animatorStateTransition46474.hasFixedDuration = true;
				animatorStateTransition46474.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46474.offset = 0f;
				animatorStateTransition46474.orderedInterruption = true;
				animatorStateTransition46474.isExit = false;
				animatorStateTransition46474.mute = false;
				animatorStateTransition46474.solo = false;
				animatorStateTransition46474.AddCondition(AnimatorConditionMode.Less, 0.5f, "Height");

				var animatorStateTransition46480 = crouchingCoverAimRightReturnAnimatorState46424.AddTransition(crouchingCoverStrafeAnimatorState46436);
				animatorStateTransition46480.canTransitionToSelf = true;
				animatorStateTransition46480.duration = 0.01f;
				animatorStateTransition46480.exitTime = 0.92f;
				animatorStateTransition46480.hasExitTime = true;
				animatorStateTransition46480.hasFixedDuration = false;
				animatorStateTransition46480.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46480.offset = 0f;
				animatorStateTransition46480.orderedInterruption = true;
				animatorStateTransition46480.isExit = false;
				animatorStateTransition46480.mute = false;
				animatorStateTransition46480.solo = false;
				animatorStateTransition46480.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

				var animatorStateTransition46482 = crouchingCoverAimRightReturnAnimatorState46424.AddTransition(crouchingCoverIdleRightAnimatorState46434);
				animatorStateTransition46482.canTransitionToSelf = true;
				animatorStateTransition46482.duration = 0.01f;
				animatorStateTransition46482.exitTime = 0.92f;
				animatorStateTransition46482.hasExitTime = true;
				animatorStateTransition46482.hasFixedDuration = false;
				animatorStateTransition46482.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46482.offset = 0f;
				animatorStateTransition46482.orderedInterruption = true;
				animatorStateTransition46482.isExit = false;
				animatorStateTransition46482.mute = false;
				animatorStateTransition46482.solo = false;
				animatorStateTransition46482.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
				animatorStateTransition46482.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

				var animatorStateTransition46484 = crouchingCoverAimRightReturnAnimatorState46424.AddTransition(crouchingCoverStrafeAnimatorState46436);
				animatorStateTransition46484.canTransitionToSelf = true;
				animatorStateTransition46484.duration = 0.01f;
				animatorStateTransition46484.exitTime = 0.92f;
				animatorStateTransition46484.hasExitTime = true;
				animatorStateTransition46484.hasFixedDuration = false;
				animatorStateTransition46484.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46484.offset = 0f;
				animatorStateTransition46484.orderedInterruption = true;
				animatorStateTransition46484.isExit = false;
				animatorStateTransition46484.mute = false;
				animatorStateTransition46484.solo = false;
				animatorStateTransition46484.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

				var animatorStateTransition46488 = crouchingCoverIdleLeftAnimatorState46426.AddTransition(coverCenterAimLeftAimAnimatorState46414);
				animatorStateTransition46488.canTransitionToSelf = true;
				animatorStateTransition46488.duration = 0.01f;
				animatorStateTransition46488.exitTime = 0.95f;
				animatorStateTransition46488.hasExitTime = false;
				animatorStateTransition46488.hasFixedDuration = false;
				animatorStateTransition46488.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46488.offset = 0f;
				animatorStateTransition46488.orderedInterruption = true;
				animatorStateTransition46488.isExit = false;
				animatorStateTransition46488.mute = false;
				animatorStateTransition46488.solo = false;
				animatorStateTransition46488.AddCondition(AnimatorConditionMode.Equals, 5f, "AbilityIntData");

				var animatorStateTransition46490 = crouchingCoverIdleLeftAnimatorState46426.AddTransition(crouchingCoverAimLeftAnimatorState46438);
				animatorStateTransition46490.canTransitionToSelf = true;
				animatorStateTransition46490.duration = 0.01f;
				animatorStateTransition46490.exitTime = 0.95f;
				animatorStateTransition46490.hasExitTime = false;
				animatorStateTransition46490.hasFixedDuration = false;
				animatorStateTransition46490.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46490.offset = 0f;
				animatorStateTransition46490.orderedInterruption = true;
				animatorStateTransition46490.isExit = false;
				animatorStateTransition46490.mute = false;
				animatorStateTransition46490.solo = false;
				animatorStateTransition46490.AddCondition(AnimatorConditionMode.Equals, 3f, "AbilityIntData");

				var animatorStateTransition46492 = crouchingCoverIdleLeftAnimatorState46426.AddTransition(crouchingCoverIdleRightAnimatorState46434);
				animatorStateTransition46492.canTransitionToSelf = true;
				animatorStateTransition46492.duration = 0.1f;
				animatorStateTransition46492.exitTime = 0.95f;
				animatorStateTransition46492.hasExitTime = false;
				animatorStateTransition46492.hasFixedDuration = true;
				animatorStateTransition46492.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46492.offset = 0f;
				animatorStateTransition46492.orderedInterruption = true;
				animatorStateTransition46492.isExit = false;
				animatorStateTransition46492.mute = false;
				animatorStateTransition46492.solo = false;
				animatorStateTransition46492.AddCondition(AnimatorConditionMode.Greater, 0.5f, "AbilityFloatData");
				animatorStateTransition46492.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
				animatorStateTransition46492.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

				var animatorStateTransition46494 = crouchingCoverIdleLeftAnimatorState46426.AddTransition(crouchingCoverStrafeAnimatorState46436);
				animatorStateTransition46494.canTransitionToSelf = true;
				animatorStateTransition46494.duration = 0.15f;
				animatorStateTransition46494.exitTime = 0.95f;
				animatorStateTransition46494.hasExitTime = false;
				animatorStateTransition46494.hasFixedDuration = true;
				animatorStateTransition46494.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46494.offset = 0f;
				animatorStateTransition46494.orderedInterruption = true;
				animatorStateTransition46494.isExit = false;
				animatorStateTransition46494.mute = false;
				animatorStateTransition46494.solo = false;
				animatorStateTransition46494.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

				var animatorStateTransition46496 = crouchingCoverIdleLeftAnimatorState46426.AddTransition(crouchingCoverStrafeAnimatorState46436);
				animatorStateTransition46496.canTransitionToSelf = true;
				animatorStateTransition46496.duration = 0.15f;
				animatorStateTransition46496.exitTime = 0.95f;
				animatorStateTransition46496.hasExitTime = false;
				animatorStateTransition46496.hasFixedDuration = true;
				animatorStateTransition46496.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46496.offset = 0f;
				animatorStateTransition46496.orderedInterruption = true;
				animatorStateTransition46496.isExit = false;
				animatorStateTransition46496.mute = false;
				animatorStateTransition46496.solo = false;
				animatorStateTransition46496.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

				var animatorStateTransition46498 = crouchingCoverIdleLeftAnimatorState46426.AddTransition(standingCoverIdleLeftAnimatorState46502);
				animatorStateTransition46498.canTransitionToSelf = true;
				animatorStateTransition46498.duration = 0.1f;
				animatorStateTransition46498.exitTime = 0.95f;
				animatorStateTransition46498.hasExitTime = false;
				animatorStateTransition46498.hasFixedDuration = true;
				animatorStateTransition46498.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46498.offset = 0f;
				animatorStateTransition46498.orderedInterruption = true;
				animatorStateTransition46498.isExit = false;
				animatorStateTransition46498.mute = false;
				animatorStateTransition46498.solo = false;
				animatorStateTransition46498.AddCondition(AnimatorConditionMode.Less, 0.5f, "Height");

				var animatorStateTransition46504 = coverCenterAimRightHoldAnimatorState46428.AddTransition(coverCenterAimRightReturnAnimatorState46430);
				animatorStateTransition46504.canTransitionToSelf = true;
				animatorStateTransition46504.duration = 0.01f;
				animatorStateTransition46504.exitTime = 0.9f;
				animatorStateTransition46504.hasExitTime = false;
				animatorStateTransition46504.hasFixedDuration = false;
				animatorStateTransition46504.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46504.offset = 0f;
				animatorStateTransition46504.orderedInterruption = true;
				animatorStateTransition46504.isExit = false;
				animatorStateTransition46504.mute = false;
				animatorStateTransition46504.solo = false;
				animatorStateTransition46504.AddCondition(AnimatorConditionMode.NotEqual, 6f, "AbilityIntData");

				var animatorStateTransition46508 = coverCenterAimRightReturnAnimatorState46430.AddTransition(crouchingCoverIdleRightAnimatorState46434);
				animatorStateTransition46508.canTransitionToSelf = true;
				animatorStateTransition46508.duration = 0.01f;
				animatorStateTransition46508.exitTime = 0.92f;
				animatorStateTransition46508.hasExitTime = true;
				animatorStateTransition46508.hasFixedDuration = false;
				animatorStateTransition46508.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46508.offset = 0f;
				animatorStateTransition46508.orderedInterruption = true;
				animatorStateTransition46508.isExit = false;
				animatorStateTransition46508.mute = false;
				animatorStateTransition46508.solo = false;
				animatorStateTransition46508.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
				animatorStateTransition46508.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

				var animatorStateTransition46510 = coverCenterAimRightReturnAnimatorState46430.AddTransition(crouchingCoverStrafeAnimatorState46436);
				animatorStateTransition46510.canTransitionToSelf = true;
				animatorStateTransition46510.duration = 0.01f;
				animatorStateTransition46510.exitTime = 0.92f;
				animatorStateTransition46510.hasExitTime = true;
				animatorStateTransition46510.hasFixedDuration = false;
				animatorStateTransition46510.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46510.offset = 0f;
				animatorStateTransition46510.orderedInterruption = true;
				animatorStateTransition46510.isExit = false;
				animatorStateTransition46510.mute = false;
				animatorStateTransition46510.solo = false;
				animatorStateTransition46510.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

				var animatorStateTransition46512 = coverCenterAimRightReturnAnimatorState46430.AddTransition(crouchingCoverStrafeAnimatorState46436);
				animatorStateTransition46512.canTransitionToSelf = true;
				animatorStateTransition46512.duration = 0.01f;
				animatorStateTransition46512.exitTime = 0.92f;
				animatorStateTransition46512.hasExitTime = true;
				animatorStateTransition46512.hasFixedDuration = false;
				animatorStateTransition46512.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46512.offset = 0f;
				animatorStateTransition46512.orderedInterruption = true;
				animatorStateTransition46512.isExit = false;
				animatorStateTransition46512.mute = false;
				animatorStateTransition46512.solo = false;
				animatorStateTransition46512.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

				var animatorStateTransition46516 = coverCenterAimRightAimAnimatorState46432.AddTransition(coverCenterAimRightHoldAnimatorState46428);
				animatorStateTransition46516.canTransitionToSelf = true;
				animatorStateTransition46516.duration = 0.01f;
				animatorStateTransition46516.exitTime = 1f;
				animatorStateTransition46516.hasExitTime = true;
				animatorStateTransition46516.hasFixedDuration = false;
				animatorStateTransition46516.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46516.offset = 0f;
				animatorStateTransition46516.orderedInterruption = true;
				animatorStateTransition46516.isExit = false;
				animatorStateTransition46516.mute = false;
				animatorStateTransition46516.solo = false;
				animatorStateTransition46516.AddCondition(AnimatorConditionMode.Equals, 6f, "AbilityIntData");

				var animatorStateTransition46518 = coverCenterAimRightAimAnimatorState46432.AddTransition(crouchingCoverIdleRightAnimatorState46434);
				animatorStateTransition46518.canTransitionToSelf = true;
				animatorStateTransition46518.duration = 0.01f;
				animatorStateTransition46518.exitTime = 0.625f;
				animatorStateTransition46518.hasExitTime = false;
				animatorStateTransition46518.hasFixedDuration = true;
				animatorStateTransition46518.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46518.offset = 0f;
				animatorStateTransition46518.orderedInterruption = true;
				animatorStateTransition46518.isExit = false;
				animatorStateTransition46518.mute = false;
				animatorStateTransition46518.solo = false;
				animatorStateTransition46518.AddCondition(AnimatorConditionMode.NotEqual, 6f, "AbilityIntData");

				var animatorStateTransition46522 = crouchingCoverIdleRightAnimatorState46434.AddTransition(coverCenterAimRightAimAnimatorState46432);
				animatorStateTransition46522.canTransitionToSelf = true;
				animatorStateTransition46522.duration = 0.01f;
				animatorStateTransition46522.exitTime = 0.95f;
				animatorStateTransition46522.hasExitTime = false;
				animatorStateTransition46522.hasFixedDuration = false;
				animatorStateTransition46522.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46522.offset = 0f;
				animatorStateTransition46522.orderedInterruption = true;
				animatorStateTransition46522.isExit = false;
				animatorStateTransition46522.mute = false;
				animatorStateTransition46522.solo = false;
				animatorStateTransition46522.AddCondition(AnimatorConditionMode.Equals, 6f, "AbilityIntData");

				var animatorStateTransition46524 = crouchingCoverIdleRightAnimatorState46434.AddTransition(crouchingCoverAimRightAnimatorState46420);
				animatorStateTransition46524.canTransitionToSelf = true;
				animatorStateTransition46524.duration = 0.01f;
				animatorStateTransition46524.exitTime = 0.95f;
				animatorStateTransition46524.hasExitTime = false;
				animatorStateTransition46524.hasFixedDuration = false;
				animatorStateTransition46524.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46524.offset = 0f;
				animatorStateTransition46524.orderedInterruption = true;
				animatorStateTransition46524.isExit = false;
				animatorStateTransition46524.mute = false;
				animatorStateTransition46524.solo = false;
				animatorStateTransition46524.AddCondition(AnimatorConditionMode.Equals, 4f, "AbilityIntData");

				var animatorStateTransition46526 = crouchingCoverIdleRightAnimatorState46434.AddTransition(crouchingCoverIdleLeftAnimatorState46426);
				animatorStateTransition46526.canTransitionToSelf = true;
				animatorStateTransition46526.duration = 0.1f;
				animatorStateTransition46526.exitTime = 0.95f;
				animatorStateTransition46526.hasExitTime = false;
				animatorStateTransition46526.hasFixedDuration = true;
				animatorStateTransition46526.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46526.offset = 0f;
				animatorStateTransition46526.orderedInterruption = true;
				animatorStateTransition46526.isExit = false;
				animatorStateTransition46526.mute = false;
				animatorStateTransition46526.solo = false;
				animatorStateTransition46526.AddCondition(AnimatorConditionMode.Less, 0.5f, "AbilityFloatData");
				animatorStateTransition46526.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
				animatorStateTransition46526.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

				var animatorStateTransition46528 = crouchingCoverIdleRightAnimatorState46434.AddTransition(crouchingCoverStrafeAnimatorState46436);
				animatorStateTransition46528.canTransitionToSelf = true;
				animatorStateTransition46528.duration = 0.15f;
				animatorStateTransition46528.exitTime = 0.95f;
				animatorStateTransition46528.hasExitTime = false;
				animatorStateTransition46528.hasFixedDuration = true;
				animatorStateTransition46528.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46528.offset = 0f;
				animatorStateTransition46528.orderedInterruption = true;
				animatorStateTransition46528.isExit = false;
				animatorStateTransition46528.mute = false;
				animatorStateTransition46528.solo = false;
				animatorStateTransition46528.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

				var animatorStateTransition46530 = crouchingCoverIdleRightAnimatorState46434.AddTransition(crouchingCoverStrafeAnimatorState46436);
				animatorStateTransition46530.canTransitionToSelf = true;
				animatorStateTransition46530.duration = 0.15f;
				animatorStateTransition46530.exitTime = 0.95f;
				animatorStateTransition46530.hasExitTime = false;
				animatorStateTransition46530.hasFixedDuration = true;
				animatorStateTransition46530.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46530.offset = 0f;
				animatorStateTransition46530.orderedInterruption = true;
				animatorStateTransition46530.isExit = false;
				animatorStateTransition46530.mute = false;
				animatorStateTransition46530.solo = false;
				animatorStateTransition46530.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

				var animatorStateTransition46532 = crouchingCoverIdleRightAnimatorState46434.AddTransition(standingCoverIdleRightAnimatorState46536);
				animatorStateTransition46532.canTransitionToSelf = true;
				animatorStateTransition46532.duration = 0.1f;
				animatorStateTransition46532.exitTime = 0.95f;
				animatorStateTransition46532.hasExitTime = false;
				animatorStateTransition46532.hasFixedDuration = true;
				animatorStateTransition46532.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46532.offset = 0f;
				animatorStateTransition46532.orderedInterruption = true;
				animatorStateTransition46532.isExit = false;
				animatorStateTransition46532.mute = false;
				animatorStateTransition46532.solo = false;
				animatorStateTransition46532.AddCondition(AnimatorConditionMode.Less, 0.5f, "Height");

				var animatorStateTransition46538 = takeCrouchingCoverAnimatorState44144.AddTransition(crouchingCoverStrafeAnimatorState46436);
				animatorStateTransition46538.canTransitionToSelf = true;
				animatorStateTransition46538.duration = 1f;
				animatorStateTransition46538.exitTime = 1f;
				animatorStateTransition46538.hasExitTime = true;
				animatorStateTransition46538.hasFixedDuration = true;
				animatorStateTransition46538.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46538.offset = 0f;
				animatorStateTransition46538.orderedInterruption = true;
				animatorStateTransition46538.isExit = false;
				animatorStateTransition46538.mute = false;
				animatorStateTransition46538.solo = false;
				animatorStateTransition46538.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

				var animatorStateTransition46540 = takeCrouchingCoverAnimatorState44144.AddTransition(crouchingCoverIdleRightAnimatorState46434);
				animatorStateTransition46540.canTransitionToSelf = true;
				animatorStateTransition46540.duration = 0.3f;
				animatorStateTransition46540.exitTime = 1f;
				animatorStateTransition46540.hasExitTime = true;
				animatorStateTransition46540.hasFixedDuration = true;
				animatorStateTransition46540.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46540.offset = 0f;
				animatorStateTransition46540.orderedInterruption = true;
				animatorStateTransition46540.isExit = false;
				animatorStateTransition46540.mute = false;
				animatorStateTransition46540.solo = false;
				animatorStateTransition46540.AddCondition(AnimatorConditionMode.Greater, 0f, "AbilityIntData");
				animatorStateTransition46540.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
				animatorStateTransition46540.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

				var animatorStateTransition46542 = takeCrouchingCoverAnimatorState44144.AddTransition(crouchingCoverIdleLeftAnimatorState46426);
				animatorStateTransition46542.canTransitionToSelf = true;
				animatorStateTransition46542.duration = 0.3f;
				animatorStateTransition46542.exitTime = 1f;
				animatorStateTransition46542.hasExitTime = true;
				animatorStateTransition46542.hasFixedDuration = true;
				animatorStateTransition46542.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46542.offset = 0f;
				animatorStateTransition46542.orderedInterruption = true;
				animatorStateTransition46542.isExit = false;
				animatorStateTransition46542.mute = false;
				animatorStateTransition46542.solo = false;
				animatorStateTransition46542.AddCondition(AnimatorConditionMode.Less, 0f, "AbilityIntData");
				animatorStateTransition46542.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
				animatorStateTransition46542.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

				var animatorStateTransition46544 = takeCrouchingCoverAnimatorState44144.AddTransition(crouchingCoverStrafeAnimatorState46436);
				animatorStateTransition46544.canTransitionToSelf = true;
				animatorStateTransition46544.duration = 1f;
				animatorStateTransition46544.exitTime = 1f;
				animatorStateTransition46544.hasExitTime = true;
				animatorStateTransition46544.hasFixedDuration = false;
				animatorStateTransition46544.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46544.offset = 0f;
				animatorStateTransition46544.orderedInterruption = true;
				animatorStateTransition46544.isExit = false;
				animatorStateTransition46544.mute = false;
				animatorStateTransition46544.solo = false;
				animatorStateTransition46544.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

				var animatorStateTransition46548 = crouchingCoverStrafeAnimatorState46436.AddTransition(coverCenterAimLeftAimAnimatorState46414);
				animatorStateTransition46548.canTransitionToSelf = true;
				animatorStateTransition46548.duration = 0.01f;
				animatorStateTransition46548.exitTime = 0.9210526f;
				animatorStateTransition46548.hasExitTime = false;
				animatorStateTransition46548.hasFixedDuration = false;
				animatorStateTransition46548.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46548.offset = 0f;
				animatorStateTransition46548.orderedInterruption = true;
				animatorStateTransition46548.isExit = false;
				animatorStateTransition46548.mute = false;
				animatorStateTransition46548.solo = false;
				animatorStateTransition46548.AddCondition(AnimatorConditionMode.Equals, 5f, "AbilityIntData");

				var animatorStateTransition46550 = crouchingCoverStrafeAnimatorState46436.AddTransition(crouchingCoverIdleLeftAnimatorState46426);
				animatorStateTransition46550.canTransitionToSelf = true;
				animatorStateTransition46550.duration = 0.15f;
				animatorStateTransition46550.exitTime = 0.9210526f;
				animatorStateTransition46550.hasExitTime = false;
				animatorStateTransition46550.hasFixedDuration = true;
				animatorStateTransition46550.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46550.offset = 0f;
				animatorStateTransition46550.orderedInterruption = true;
				animatorStateTransition46550.isExit = false;
				animatorStateTransition46550.mute = false;
				animatorStateTransition46550.solo = false;
				animatorStateTransition46550.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");
				animatorStateTransition46550.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
				animatorStateTransition46550.AddCondition(AnimatorConditionMode.Less, 0.5f, "AbilityFloatData");

				var animatorStateTransition46552 = crouchingCoverStrafeAnimatorState46436.AddTransition(crouchingCoverIdleRightAnimatorState46434);
				animatorStateTransition46552.canTransitionToSelf = true;
				animatorStateTransition46552.duration = 0.15f;
				animatorStateTransition46552.exitTime = 0.9210526f;
				animatorStateTransition46552.hasExitTime = false;
				animatorStateTransition46552.hasFixedDuration = true;
				animatorStateTransition46552.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46552.offset = 0f;
				animatorStateTransition46552.orderedInterruption = true;
				animatorStateTransition46552.isExit = false;
				animatorStateTransition46552.mute = false;
				animatorStateTransition46552.solo = false;
				animatorStateTransition46552.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");
				animatorStateTransition46552.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
				animatorStateTransition46552.AddCondition(AnimatorConditionMode.Greater, 0.5f, "AbilityFloatData");

				var animatorStateTransition46554 = crouchingCoverStrafeAnimatorState46436.AddTransition(coverCenterAimRightAimAnimatorState46432);
				animatorStateTransition46554.canTransitionToSelf = true;
				animatorStateTransition46554.duration = 0.01f;
				animatorStateTransition46554.exitTime = 0.9210526f;
				animatorStateTransition46554.hasExitTime = false;
				animatorStateTransition46554.hasFixedDuration = false;
				animatorStateTransition46554.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46554.offset = 0f;
				animatorStateTransition46554.orderedInterruption = true;
				animatorStateTransition46554.isExit = false;
				animatorStateTransition46554.mute = false;
				animatorStateTransition46554.solo = false;
				animatorStateTransition46554.AddCondition(AnimatorConditionMode.Equals, 6f, "AbilityIntData");

				var animatorStateTransition46556 = crouchingCoverStrafeAnimatorState46436.AddTransition(crouchingCoverAimLeftAnimatorState46438);
				animatorStateTransition46556.canTransitionToSelf = true;
				animatorStateTransition46556.duration = 0.01f;
				animatorStateTransition46556.exitTime = 0.9f;
				animatorStateTransition46556.hasExitTime = false;
				animatorStateTransition46556.hasFixedDuration = false;
				animatorStateTransition46556.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46556.offset = 0f;
				animatorStateTransition46556.orderedInterruption = true;
				animatorStateTransition46556.isExit = false;
				animatorStateTransition46556.mute = false;
				animatorStateTransition46556.solo = false;
				animatorStateTransition46556.AddCondition(AnimatorConditionMode.Equals, 3f, "AbilityIntData");

				var animatorStateTransition46558 = crouchingCoverStrafeAnimatorState46436.AddTransition(crouchingCoverAimRightAnimatorState46420);
				animatorStateTransition46558.canTransitionToSelf = true;
				animatorStateTransition46558.duration = 0.01f;
				animatorStateTransition46558.exitTime = 0.9f;
				animatorStateTransition46558.hasExitTime = false;
				animatorStateTransition46558.hasFixedDuration = false;
				animatorStateTransition46558.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46558.offset = 0f;
				animatorStateTransition46558.orderedInterruption = true;
				animatorStateTransition46558.isExit = false;
				animatorStateTransition46558.mute = false;
				animatorStateTransition46558.solo = false;
				animatorStateTransition46558.AddCondition(AnimatorConditionMode.Equals, 4f, "AbilityIntData");

				var animatorStateTransition46560 = crouchingCoverStrafeAnimatorState46436.AddTransition(standingCoverStrafeAnimatorState46564);
				animatorStateTransition46560.canTransitionToSelf = true;
				animatorStateTransition46560.duration = 0.1f;
				animatorStateTransition46560.exitTime = 0.9210526f;
				animatorStateTransition46560.hasExitTime = false;
				animatorStateTransition46560.hasFixedDuration = true;
				animatorStateTransition46560.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46560.offset = 0f;
				animatorStateTransition46560.orderedInterruption = true;
				animatorStateTransition46560.isExit = false;
				animatorStateTransition46560.mute = false;
				animatorStateTransition46560.solo = false;
				animatorStateTransition46560.AddCondition(AnimatorConditionMode.Less, 0.5f, "Height");

				var animatorStateTransition46570 = crouchingCoverAimLeftAnimatorState46438.AddTransition(crouchingCoverAimLeftHoldAnimatorState46418);
				animatorStateTransition46570.canTransitionToSelf = true;
				animatorStateTransition46570.duration = 0.01f;
				animatorStateTransition46570.exitTime = 1f;
				animatorStateTransition46570.hasExitTime = true;
				animatorStateTransition46570.hasFixedDuration = false;
				animatorStateTransition46570.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46570.offset = 0f;
				animatorStateTransition46570.orderedInterruption = true;
				animatorStateTransition46570.isExit = false;
				animatorStateTransition46570.mute = false;
				animatorStateTransition46570.solo = false;
				animatorStateTransition46570.AddCondition(AnimatorConditionMode.Equals, 3f, "AbilityIntData");

				var animatorStateTransition46572 = crouchingCoverAimLeftAnimatorState46438.AddTransition(crouchingCoverIdleLeftAnimatorState46426);
				animatorStateTransition46572.canTransitionToSelf = true;
				animatorStateTransition46572.duration = 0.01f;
				animatorStateTransition46572.exitTime = 0.625f;
				animatorStateTransition46572.hasExitTime = false;
				animatorStateTransition46572.hasFixedDuration = false;
				animatorStateTransition46572.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46572.offset = 0f;
				animatorStateTransition46572.orderedInterruption = true;
				animatorStateTransition46572.isExit = false;
				animatorStateTransition46572.mute = false;
				animatorStateTransition46572.solo = false;
				animatorStateTransition46572.AddCondition(AnimatorConditionMode.NotEqual, 3f, "AbilityIntData");

				var animatorStateTransition46576 = coverCenterAimLeftHoldAnimatorState46440.AddTransition(coverCenterAimLeftReturnAnimatorState46416);
				animatorStateTransition46576.canTransitionToSelf = true;
				animatorStateTransition46576.duration = 0.01f;
				animatorStateTransition46576.exitTime = 0.9f;
				animatorStateTransition46576.hasExitTime = false;
				animatorStateTransition46576.hasFixedDuration = false;
				animatorStateTransition46576.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46576.offset = 0f;
				animatorStateTransition46576.orderedInterruption = true;
				animatorStateTransition46576.isExit = false;
				animatorStateTransition46576.mute = false;
				animatorStateTransition46576.solo = false;
				animatorStateTransition46576.AddCondition(AnimatorConditionMode.NotEqual, 5f, "AbilityIntData");

				var animatorStateTransition46580 = crouchingCoverAimLeftReturnAnimatorState46442.AddTransition(crouchingCoverStrafeAnimatorState46436);
				animatorStateTransition46580.canTransitionToSelf = true;
				animatorStateTransition46580.duration = 0.01f;
				animatorStateTransition46580.exitTime = 0.92f;
				animatorStateTransition46580.hasExitTime = true;
				animatorStateTransition46580.hasFixedDuration = false;
				animatorStateTransition46580.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46580.offset = 0f;
				animatorStateTransition46580.orderedInterruption = true;
				animatorStateTransition46580.isExit = false;
				animatorStateTransition46580.mute = false;
				animatorStateTransition46580.solo = false;
				animatorStateTransition46580.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

				var animatorStateTransition46582 = crouchingCoverAimLeftReturnAnimatorState46442.AddTransition(crouchingCoverIdleLeftAnimatorState46426);
				animatorStateTransition46582.canTransitionToSelf = true;
				animatorStateTransition46582.duration = 0.01f;
				animatorStateTransition46582.exitTime = 0.92f;
				animatorStateTransition46582.hasExitTime = true;
				animatorStateTransition46582.hasFixedDuration = false;
				animatorStateTransition46582.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46582.offset = 0f;
				animatorStateTransition46582.orderedInterruption = true;
				animatorStateTransition46582.isExit = false;
				animatorStateTransition46582.mute = false;
				animatorStateTransition46582.solo = false;
				animatorStateTransition46582.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
				animatorStateTransition46582.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

				var animatorStateTransition46584 = crouchingCoverAimLeftReturnAnimatorState46442.AddTransition(crouchingCoverStrafeAnimatorState46436);
				animatorStateTransition46584.canTransitionToSelf = true;
				animatorStateTransition46584.duration = 0.01f;
				animatorStateTransition46584.exitTime = 0.92f;
				animatorStateTransition46584.hasExitTime = true;
				animatorStateTransition46584.hasFixedDuration = false;
				animatorStateTransition46584.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46584.offset = 0f;
				animatorStateTransition46584.orderedInterruption = true;
				animatorStateTransition46584.isExit = false;
				animatorStateTransition46584.mute = false;
				animatorStateTransition46584.solo = false;
				animatorStateTransition46584.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

				// State Transitions.
				var animatorStateTransition46596 = standingAimRightReturnAnimatorState46588.AddTransition(standingCoverStrafeAnimatorState46564);
				animatorStateTransition46596.canTransitionToSelf = true;
				animatorStateTransition46596.duration = 0.01f;
				animatorStateTransition46596.exitTime = 0.92f;
				animatorStateTransition46596.hasExitTime = true;
				animatorStateTransition46596.hasFixedDuration = false;
				animatorStateTransition46596.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46596.offset = 0f;
				animatorStateTransition46596.orderedInterruption = true;
				animatorStateTransition46596.isExit = false;
				animatorStateTransition46596.mute = false;
				animatorStateTransition46596.solo = false;
				animatorStateTransition46596.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

				var animatorStateTransition46598 = standingAimRightReturnAnimatorState46588.AddTransition(standingCoverIdleRightAnimatorState46536);
				animatorStateTransition46598.canTransitionToSelf = true;
				animatorStateTransition46598.duration = 0.01f;
				animatorStateTransition46598.exitTime = 0.92f;
				animatorStateTransition46598.hasExitTime = true;
				animatorStateTransition46598.hasFixedDuration = false;
				animatorStateTransition46598.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46598.offset = 0f;
				animatorStateTransition46598.orderedInterruption = true;
				animatorStateTransition46598.isExit = false;
				animatorStateTransition46598.mute = false;
				animatorStateTransition46598.solo = false;
				animatorStateTransition46598.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
				animatorStateTransition46598.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

				var animatorStateTransition46600 = standingAimRightReturnAnimatorState46588.AddTransition(standingCoverStrafeAnimatorState46564);
				animatorStateTransition46600.canTransitionToSelf = true;
				animatorStateTransition46600.duration = 0.01f;
				animatorStateTransition46600.exitTime = 0.92f;
				animatorStateTransition46600.hasExitTime = true;
				animatorStateTransition46600.hasFixedDuration = false;
				animatorStateTransition46600.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46600.offset = 0f;
				animatorStateTransition46600.orderedInterruption = true;
				animatorStateTransition46600.isExit = false;
				animatorStateTransition46600.mute = false;
				animatorStateTransition46600.solo = false;
				animatorStateTransition46600.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

				var animatorStateTransition46604 = standingCoverAimRightHoldAnimatorState46478.AddTransition(standingAimRightReturnAnimatorState46588);
				animatorStateTransition46604.canTransitionToSelf = true;
				animatorStateTransition46604.duration = 0.01f;
				animatorStateTransition46604.exitTime = 0.9f;
				animatorStateTransition46604.hasExitTime = false;
				animatorStateTransition46604.hasFixedDuration = false;
				animatorStateTransition46604.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46604.offset = 0f;
				animatorStateTransition46604.orderedInterruption = true;
				animatorStateTransition46604.isExit = false;
				animatorStateTransition46604.mute = false;
				animatorStateTransition46604.solo = false;
				animatorStateTransition46604.AddCondition(AnimatorConditionMode.NotEqual, 4f, "AbilityIntData");

				var animatorStateTransition46606 = standingCoverAimRightHoldAnimatorState46478.AddTransition(crouchingCoverAimRightHoldAnimatorState46422);
				animatorStateTransition46606.canTransitionToSelf = true;
				animatorStateTransition46606.duration = 0.1f;
				animatorStateTransition46606.exitTime = 0f;
				animatorStateTransition46606.hasExitTime = false;
				animatorStateTransition46606.hasFixedDuration = true;
				animatorStateTransition46606.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46606.offset = 0f;
				animatorStateTransition46606.orderedInterruption = true;
				animatorStateTransition46606.isExit = false;
				animatorStateTransition46606.mute = false;
				animatorStateTransition46606.solo = false;
				animatorStateTransition46606.AddCondition(AnimatorConditionMode.Greater, 0.5f, "Height");

				var animatorStateTransition46610 = standingCoverAimRightAnimatorState46590.AddTransition(standingCoverAimRightHoldAnimatorState46478);
				animatorStateTransition46610.canTransitionToSelf = true;
				animatorStateTransition46610.duration = 0.01f;
				animatorStateTransition46610.exitTime = 1f;
				animatorStateTransition46610.hasExitTime = true;
				animatorStateTransition46610.hasFixedDuration = false;
				animatorStateTransition46610.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46610.offset = 0f;
				animatorStateTransition46610.orderedInterruption = true;
				animatorStateTransition46610.isExit = false;
				animatorStateTransition46610.mute = false;
				animatorStateTransition46610.solo = false;
				animatorStateTransition46610.AddCondition(AnimatorConditionMode.Equals, 4f, "AbilityIntData");

				var animatorStateTransition46612 = standingCoverAimRightAnimatorState46590.AddTransition(standingCoverIdleRightAnimatorState46536);
				animatorStateTransition46612.canTransitionToSelf = true;
				animatorStateTransition46612.duration = 0.01f;
				animatorStateTransition46612.exitTime = 0.625f;
				animatorStateTransition46612.hasExitTime = false;
				animatorStateTransition46612.hasFixedDuration = true;
				animatorStateTransition46612.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46612.offset = 0f;
				animatorStateTransition46612.orderedInterruption = true;
				animatorStateTransition46612.isExit = false;
				animatorStateTransition46612.mute = false;
				animatorStateTransition46612.solo = false;
				animatorStateTransition46612.AddCondition(AnimatorConditionMode.NotEqual, 4f, "AbilityIntData");

				var animatorStateTransition46616 = standingCoverAimLeftHoldAnimatorState46464.AddTransition(standingCoverAimLeftReturnAnimatorState46592);
				animatorStateTransition46616.canTransitionToSelf = true;
				animatorStateTransition46616.duration = 0.01f;
				animatorStateTransition46616.exitTime = 0.9f;
				animatorStateTransition46616.hasExitTime = false;
				animatorStateTransition46616.hasFixedDuration = false;
				animatorStateTransition46616.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46616.offset = 0f;
				animatorStateTransition46616.orderedInterruption = true;
				animatorStateTransition46616.isExit = false;
				animatorStateTransition46616.mute = false;
				animatorStateTransition46616.solo = false;
				animatorStateTransition46616.AddCondition(AnimatorConditionMode.NotEqual, 3f, "AbilityIntData");

				var animatorStateTransition46618 = standingCoverAimLeftHoldAnimatorState46464.AddTransition(crouchingCoverAimLeftHoldAnimatorState46418);
				animatorStateTransition46618.canTransitionToSelf = true;
				animatorStateTransition46618.duration = 0.1f;
				animatorStateTransition46618.exitTime = 0f;
				animatorStateTransition46618.hasExitTime = false;
				animatorStateTransition46618.hasFixedDuration = true;
				animatorStateTransition46618.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46618.offset = 0f;
				animatorStateTransition46618.orderedInterruption = true;
				animatorStateTransition46618.isExit = false;
				animatorStateTransition46618.mute = false;
				animatorStateTransition46618.solo = false;
				animatorStateTransition46618.AddCondition(AnimatorConditionMode.Greater, 0.5f, "Height");

				var animatorStateTransition46622 = standingCoverAimLeftReturnAnimatorState46592.AddTransition(standingCoverStrafeAnimatorState46564);
				animatorStateTransition46622.canTransitionToSelf = true;
				animatorStateTransition46622.duration = 0.01f;
				animatorStateTransition46622.exitTime = 0.92f;
				animatorStateTransition46622.hasExitTime = true;
				animatorStateTransition46622.hasFixedDuration = false;
				animatorStateTransition46622.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46622.offset = 0f;
				animatorStateTransition46622.orderedInterruption = true;
				animatorStateTransition46622.isExit = false;
				animatorStateTransition46622.mute = false;
				animatorStateTransition46622.solo = false;
				animatorStateTransition46622.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

				var animatorStateTransition46624 = standingCoverAimLeftReturnAnimatorState46592.AddTransition(standingCoverIdleLeftAnimatorState46502);
				animatorStateTransition46624.canTransitionToSelf = true;
				animatorStateTransition46624.duration = 0.01f;
				animatorStateTransition46624.exitTime = 0.92f;
				animatorStateTransition46624.hasExitTime = true;
				animatorStateTransition46624.hasFixedDuration = false;
				animatorStateTransition46624.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46624.offset = 0f;
				animatorStateTransition46624.orderedInterruption = true;
				animatorStateTransition46624.isExit = false;
				animatorStateTransition46624.mute = false;
				animatorStateTransition46624.solo = false;
				animatorStateTransition46624.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
				animatorStateTransition46624.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

				var animatorStateTransition46626 = standingCoverAimLeftReturnAnimatorState46592.AddTransition(standingCoverStrafeAnimatorState46564);
				animatorStateTransition46626.canTransitionToSelf = true;
				animatorStateTransition46626.duration = 0.01f;
				animatorStateTransition46626.exitTime = 0.92f;
				animatorStateTransition46626.hasExitTime = true;
				animatorStateTransition46626.hasFixedDuration = false;
				animatorStateTransition46626.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46626.offset = 0f;
				animatorStateTransition46626.orderedInterruption = true;
				animatorStateTransition46626.isExit = false;
				animatorStateTransition46626.mute = false;
				animatorStateTransition46626.solo = false;
				animatorStateTransition46626.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

				var animatorStateTransition46630 = standingCoverAimLeftAnimatorState46594.AddTransition(standingCoverAimLeftHoldAnimatorState46464);
				animatorStateTransition46630.canTransitionToSelf = true;
				animatorStateTransition46630.duration = 0.01f;
				animatorStateTransition46630.exitTime = 1f;
				animatorStateTransition46630.hasExitTime = true;
				animatorStateTransition46630.hasFixedDuration = false;
				animatorStateTransition46630.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46630.offset = 0f;
				animatorStateTransition46630.orderedInterruption = true;
				animatorStateTransition46630.isExit = false;
				animatorStateTransition46630.mute = false;
				animatorStateTransition46630.solo = false;
				animatorStateTransition46630.AddCondition(AnimatorConditionMode.Equals, 3f, "AbilityIntData");

				var animatorStateTransition46632 = standingCoverAimLeftAnimatorState46594.AddTransition(standingCoverIdleLeftAnimatorState46502);
				animatorStateTransition46632.canTransitionToSelf = true;
				animatorStateTransition46632.duration = 0.01f;
				animatorStateTransition46632.exitTime = 0.625f;
				animatorStateTransition46632.hasExitTime = false;
				animatorStateTransition46632.hasFixedDuration = true;
				animatorStateTransition46632.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46632.offset = 0f;
				animatorStateTransition46632.orderedInterruption = true;
				animatorStateTransition46632.isExit = false;
				animatorStateTransition46632.mute = false;
				animatorStateTransition46632.solo = false;
				animatorStateTransition46632.AddCondition(AnimatorConditionMode.NotEqual, 3f, "AbilityIntData");

				var animatorStateTransition46636 = standingCoverStrafeAnimatorState46564.AddTransition(standingCoverAimRightAnimatorState46590);
				animatorStateTransition46636.canTransitionToSelf = true;
				animatorStateTransition46636.duration = 0.01f;
				animatorStateTransition46636.exitTime = 0.9f;
				animatorStateTransition46636.hasExitTime = false;
				animatorStateTransition46636.hasFixedDuration = false;
				animatorStateTransition46636.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46636.offset = 0f;
				animatorStateTransition46636.orderedInterruption = true;
				animatorStateTransition46636.isExit = false;
				animatorStateTransition46636.mute = false;
				animatorStateTransition46636.solo = false;
				animatorStateTransition46636.AddCondition(AnimatorConditionMode.Equals, 4f, "AbilityIntData");

				var animatorStateTransition46638 = standingCoverStrafeAnimatorState46564.AddTransition(standingCoverAimLeftAnimatorState46594);
				animatorStateTransition46638.canTransitionToSelf = true;
				animatorStateTransition46638.duration = 0.01f;
				animatorStateTransition46638.exitTime = 0.9f;
				animatorStateTransition46638.hasExitTime = false;
				animatorStateTransition46638.hasFixedDuration = false;
				animatorStateTransition46638.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46638.offset = 0f;
				animatorStateTransition46638.orderedInterruption = true;
				animatorStateTransition46638.isExit = false;
				animatorStateTransition46638.mute = false;
				animatorStateTransition46638.solo = false;
				animatorStateTransition46638.AddCondition(AnimatorConditionMode.Equals, 3f, "AbilityIntData");

				var animatorStateTransition46640 = standingCoverStrafeAnimatorState46564.AddTransition(standingCoverIdleRightAnimatorState46536);
				animatorStateTransition46640.canTransitionToSelf = true;
				animatorStateTransition46640.duration = 0.15f;
				animatorStateTransition46640.exitTime = 0.9210526f;
				animatorStateTransition46640.hasExitTime = false;
				animatorStateTransition46640.hasFixedDuration = true;
				animatorStateTransition46640.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46640.offset = 0f;
				animatorStateTransition46640.orderedInterruption = true;
				animatorStateTransition46640.isExit = false;
				animatorStateTransition46640.mute = false;
				animatorStateTransition46640.solo = false;
				animatorStateTransition46640.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");
				animatorStateTransition46640.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
				animatorStateTransition46640.AddCondition(AnimatorConditionMode.Greater, 0.5f, "AbilityFloatData");

				var animatorStateTransition46642 = standingCoverStrafeAnimatorState46564.AddTransition(standingCoverIdleLeftAnimatorState46502);
				animatorStateTransition46642.canTransitionToSelf = true;
				animatorStateTransition46642.duration = 0.15f;
				animatorStateTransition46642.exitTime = 0.9210526f;
				animatorStateTransition46642.hasExitTime = false;
				animatorStateTransition46642.hasFixedDuration = true;
				animatorStateTransition46642.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46642.offset = 0f;
				animatorStateTransition46642.orderedInterruption = true;
				animatorStateTransition46642.isExit = false;
				animatorStateTransition46642.mute = false;
				animatorStateTransition46642.solo = false;
				animatorStateTransition46642.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");
				animatorStateTransition46642.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
				animatorStateTransition46642.AddCondition(AnimatorConditionMode.Less, 0.5f, "AbilityFloatData");

				var animatorStateTransition46644 = standingCoverStrafeAnimatorState46564.AddTransition(crouchingCoverStrafeAnimatorState46436);
				animatorStateTransition46644.canTransitionToSelf = true;
				animatorStateTransition46644.duration = 0.1f;
				animatorStateTransition46644.exitTime = 0.9210526f;
				animatorStateTransition46644.hasExitTime = false;
				animatorStateTransition46644.hasFixedDuration = true;
				animatorStateTransition46644.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46644.offset = 0f;
				animatorStateTransition46644.orderedInterruption = true;
				animatorStateTransition46644.isExit = false;
				animatorStateTransition46644.mute = false;
				animatorStateTransition46644.solo = false;
				animatorStateTransition46644.AddCondition(AnimatorConditionMode.Greater, 0.5f, "Height");

				var animatorStateTransition46656 = takeStandingCoverAnimatorState44146.AddTransition(standingCoverStrafeAnimatorState46564);
				animatorStateTransition46656.canTransitionToSelf = true;
				animatorStateTransition46656.duration = 1f;
				animatorStateTransition46656.exitTime = 1f;
				animatorStateTransition46656.hasExitTime = true;
				animatorStateTransition46656.hasFixedDuration = false;
				animatorStateTransition46656.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46656.offset = 0f;
				animatorStateTransition46656.orderedInterruption = true;
				animatorStateTransition46656.isExit = false;
				animatorStateTransition46656.mute = false;
				animatorStateTransition46656.solo = false;
				animatorStateTransition46656.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

				var animatorStateTransition46658 = takeStandingCoverAnimatorState44146.AddTransition(standingCoverIdleLeftAnimatorState46502);
				animatorStateTransition46658.canTransitionToSelf = true;
				animatorStateTransition46658.duration = 0.3f;
				animatorStateTransition46658.exitTime = 1f;
				animatorStateTransition46658.hasExitTime = true;
				animatorStateTransition46658.hasFixedDuration = true;
				animatorStateTransition46658.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46658.offset = 0f;
				animatorStateTransition46658.orderedInterruption = true;
				animatorStateTransition46658.isExit = false;
				animatorStateTransition46658.mute = false;
				animatorStateTransition46658.solo = false;
				animatorStateTransition46658.AddCondition(AnimatorConditionMode.Less, 0.5f, "AbilityFloatData");
				animatorStateTransition46658.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
				animatorStateTransition46658.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

				var animatorStateTransition46660 = takeStandingCoverAnimatorState44146.AddTransition(standingCoverIdleRightAnimatorState46536);
				animatorStateTransition46660.canTransitionToSelf = true;
				animatorStateTransition46660.duration = 0.3f;
				animatorStateTransition46660.exitTime = 1f;
				animatorStateTransition46660.hasExitTime = true;
				animatorStateTransition46660.hasFixedDuration = true;
				animatorStateTransition46660.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46660.offset = 0f;
				animatorStateTransition46660.orderedInterruption = true;
				animatorStateTransition46660.isExit = false;
				animatorStateTransition46660.mute = false;
				animatorStateTransition46660.solo = false;
				animatorStateTransition46660.AddCondition(AnimatorConditionMode.Greater, 0.5f, "AbilityFloatData");
				animatorStateTransition46660.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
				animatorStateTransition46660.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

				var animatorStateTransition46662 = takeStandingCoverAnimatorState44146.AddTransition(standingCoverStrafeAnimatorState46564);
				animatorStateTransition46662.canTransitionToSelf = true;
				animatorStateTransition46662.duration = 1f;
				animatorStateTransition46662.exitTime = 1f;
				animatorStateTransition46662.hasExitTime = true;
				animatorStateTransition46662.hasFixedDuration = false;
				animatorStateTransition46662.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46662.offset = 0f;
				animatorStateTransition46662.orderedInterruption = true;
				animatorStateTransition46662.isExit = false;
				animatorStateTransition46662.mute = false;
				animatorStateTransition46662.solo = false;
				animatorStateTransition46662.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

				var animatorStateTransition46666 = standingCoverIdleRightAnimatorState46536.AddTransition(standingCoverIdleLeftAnimatorState46502);
				animatorStateTransition46666.canTransitionToSelf = true;
				animatorStateTransition46666.duration = 0.1f;
				animatorStateTransition46666.exitTime = 0.95f;
				animatorStateTransition46666.hasExitTime = false;
				animatorStateTransition46666.hasFixedDuration = true;
				animatorStateTransition46666.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46666.offset = 0f;
				animatorStateTransition46666.orderedInterruption = true;
				animatorStateTransition46666.isExit = false;
				animatorStateTransition46666.mute = false;
				animatorStateTransition46666.solo = false;
				animatorStateTransition46666.AddCondition(AnimatorConditionMode.Less, 0.5f, "AbilityFloatData");
				animatorStateTransition46666.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
				animatorStateTransition46666.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

				var animatorStateTransition46668 = standingCoverIdleRightAnimatorState46536.AddTransition(standingCoverAimRightAnimatorState46590);
				animatorStateTransition46668.canTransitionToSelf = true;
				animatorStateTransition46668.duration = 0.01f;
				animatorStateTransition46668.exitTime = 0.95f;
				animatorStateTransition46668.hasExitTime = false;
				animatorStateTransition46668.hasFixedDuration = false;
				animatorStateTransition46668.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46668.offset = 0f;
				animatorStateTransition46668.orderedInterruption = true;
				animatorStateTransition46668.isExit = false;
				animatorStateTransition46668.mute = false;
				animatorStateTransition46668.solo = false;
				animatorStateTransition46668.AddCondition(AnimatorConditionMode.Equals, 4f, "AbilityIntData");

				var animatorStateTransition46670 = standingCoverIdleRightAnimatorState46536.AddTransition(standingCoverStrafeAnimatorState46564);
				animatorStateTransition46670.canTransitionToSelf = true;
				animatorStateTransition46670.duration = 0.15f;
				animatorStateTransition46670.exitTime = 0.95f;
				animatorStateTransition46670.hasExitTime = false;
				animatorStateTransition46670.hasFixedDuration = true;
				animatorStateTransition46670.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46670.offset = 0f;
				animatorStateTransition46670.orderedInterruption = true;
				animatorStateTransition46670.isExit = false;
				animatorStateTransition46670.mute = false;
				animatorStateTransition46670.solo = false;
				animatorStateTransition46670.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

				var animatorStateTransition46672 = standingCoverIdleRightAnimatorState46536.AddTransition(standingCoverStrafeAnimatorState46564);
				animatorStateTransition46672.canTransitionToSelf = true;
				animatorStateTransition46672.duration = 0.15f;
				animatorStateTransition46672.exitTime = 0.95f;
				animatorStateTransition46672.hasExitTime = false;
				animatorStateTransition46672.hasFixedDuration = true;
				animatorStateTransition46672.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46672.offset = 0f;
				animatorStateTransition46672.orderedInterruption = true;
				animatorStateTransition46672.isExit = false;
				animatorStateTransition46672.mute = false;
				animatorStateTransition46672.solo = false;
				animatorStateTransition46672.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

				var animatorStateTransition46674 = standingCoverIdleRightAnimatorState46536.AddTransition(crouchingCoverIdleRightAnimatorState46434);
				animatorStateTransition46674.canTransitionToSelf = true;
				animatorStateTransition46674.duration = 0.1f;
				animatorStateTransition46674.exitTime = 0.95f;
				animatorStateTransition46674.hasExitTime = false;
				animatorStateTransition46674.hasFixedDuration = true;
				animatorStateTransition46674.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46674.offset = 0f;
				animatorStateTransition46674.orderedInterruption = true;
				animatorStateTransition46674.isExit = false;
				animatorStateTransition46674.mute = false;
				animatorStateTransition46674.solo = false;
				animatorStateTransition46674.AddCondition(AnimatorConditionMode.Greater, 0.5f, "Height");

				var animatorStateTransition46676 = standingCoverIdleLeftAnimatorState46502.AddTransition(standingCoverIdleRightAnimatorState46536);
				animatorStateTransition46676.canTransitionToSelf = true;
				animatorStateTransition46676.duration = 0.1f;
				animatorStateTransition46676.exitTime = 0.95f;
				animatorStateTransition46676.hasExitTime = false;
				animatorStateTransition46676.hasFixedDuration = true;
				animatorStateTransition46676.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46676.offset = 0f;
				animatorStateTransition46676.orderedInterruption = true;
				animatorStateTransition46676.isExit = false;
				animatorStateTransition46676.mute = false;
				animatorStateTransition46676.solo = false;
				animatorStateTransition46676.AddCondition(AnimatorConditionMode.Greater, 0.5f, "AbilityFloatData");
				animatorStateTransition46676.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
				animatorStateTransition46676.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

				var animatorStateTransition46678 = standingCoverIdleLeftAnimatorState46502.AddTransition(standingCoverAimLeftAnimatorState46594);
				animatorStateTransition46678.canTransitionToSelf = true;
				animatorStateTransition46678.duration = 0.01f;
				animatorStateTransition46678.exitTime = 0.95f;
				animatorStateTransition46678.hasExitTime = false;
				animatorStateTransition46678.hasFixedDuration = false;
				animatorStateTransition46678.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46678.offset = 0f;
				animatorStateTransition46678.orderedInterruption = true;
				animatorStateTransition46678.isExit = false;
				animatorStateTransition46678.mute = false;
				animatorStateTransition46678.solo = false;
				animatorStateTransition46678.AddCondition(AnimatorConditionMode.Equals, 3f, "AbilityIntData");

				var animatorStateTransition46680 = standingCoverIdleLeftAnimatorState46502.AddTransition(standingCoverStrafeAnimatorState46564);
				animatorStateTransition46680.canTransitionToSelf = true;
				animatorStateTransition46680.duration = 0.15f;
				animatorStateTransition46680.exitTime = 0.95f;
				animatorStateTransition46680.hasExitTime = false;
				animatorStateTransition46680.hasFixedDuration = true;
				animatorStateTransition46680.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46680.offset = 0f;
				animatorStateTransition46680.orderedInterruption = true;
				animatorStateTransition46680.isExit = false;
				animatorStateTransition46680.mute = false;
				animatorStateTransition46680.solo = false;
				animatorStateTransition46680.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

				var animatorStateTransition46682 = standingCoverIdleLeftAnimatorState46502.AddTransition(standingCoverStrafeAnimatorState46564);
				animatorStateTransition46682.canTransitionToSelf = true;
				animatorStateTransition46682.duration = 0.15f;
				animatorStateTransition46682.exitTime = 0.95f;
				animatorStateTransition46682.hasExitTime = false;
				animatorStateTransition46682.hasFixedDuration = true;
				animatorStateTransition46682.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46682.offset = 0f;
				animatorStateTransition46682.orderedInterruption = true;
				animatorStateTransition46682.isExit = false;
				animatorStateTransition46682.mute = false;
				animatorStateTransition46682.solo = false;
				animatorStateTransition46682.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

				var animatorStateTransition46684 = standingCoverIdleLeftAnimatorState46502.AddTransition(crouchingCoverIdleLeftAnimatorState46426);
				animatorStateTransition46684.canTransitionToSelf = true;
				animatorStateTransition46684.duration = 0.1f;
				animatorStateTransition46684.exitTime = 0.95f;
				animatorStateTransition46684.hasExitTime = false;
				animatorStateTransition46684.hasFixedDuration = true;
				animatorStateTransition46684.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition46684.offset = 0f;
				animatorStateTransition46684.orderedInterruption = true;
				animatorStateTransition46684.isExit = false;
				animatorStateTransition46684.mute = false;
				animatorStateTransition46684.solo = false;
				animatorStateTransition46684.AddCondition(AnimatorConditionMode.Greater, 0.5f, "Height");

				// State Machine Transitions.
				var animatorStateTransition43886 = baseStateMachine1483310764.AddAnyStateTransition(takeCrouchingCoverAnimatorState44144);
				animatorStateTransition43886.canTransitionToSelf = true;
				animatorStateTransition43886.duration = 0.25f;
				animatorStateTransition43886.exitTime = 0.75f;
				animatorStateTransition43886.hasExitTime = false;
				animatorStateTransition43886.hasFixedDuration = true;
				animatorStateTransition43886.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition43886.offset = 0f;
				animatorStateTransition43886.orderedInterruption = true;
				animatorStateTransition43886.isExit = false;
				animatorStateTransition43886.mute = false;
				animatorStateTransition43886.solo = false;
				animatorStateTransition43886.AddCondition(AnimatorConditionMode.If, 0f, "AbilityChange");
				animatorStateTransition43886.AddCondition(AnimatorConditionMode.Equals, 401f, "AbilityIndex");
				animatorStateTransition43886.AddCondition(AnimatorConditionMode.Greater, 0.5f, "Height");

				var animatorStateTransition43888 = baseStateMachine1483310764.AddAnyStateTransition(takeStandingCoverAnimatorState44146);
				animatorStateTransition43888.canTransitionToSelf = true;
				animatorStateTransition43888.duration = 0.25f;
				animatorStateTransition43888.exitTime = 0.75f;
				animatorStateTransition43888.hasExitTime = false;
				animatorStateTransition43888.hasFixedDuration = true;
				animatorStateTransition43888.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition43888.offset = 0f;
				animatorStateTransition43888.orderedInterruption = true;
				animatorStateTransition43888.isExit = false;
				animatorStateTransition43888.mute = false;
				animatorStateTransition43888.solo = false;
				animatorStateTransition43888.AddCondition(AnimatorConditionMode.If, 0f, "AbilityChange");
				animatorStateTransition43888.AddCondition(AnimatorConditionMode.Equals, 401f, "AbilityIndex");
				animatorStateTransition43888.AddCondition(AnimatorConditionMode.Less, 0.5f, "Height");
			}
		}
	}
}
