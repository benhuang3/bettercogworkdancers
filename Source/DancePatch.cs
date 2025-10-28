using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BepInEx.Logging;

using UnityEngine.SceneManagement;
using GlobalEnums;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityObject = UnityEngine.Object;
using HazardType = GlobalEnums.HazardType;




namespace BetterCogworkDancers;

/// <summary>
/// Modifies the behavior of the First Sinner boss.
/// </summary>
[RequireComponent(typeof(tk2dSpriteAnimator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayMakerFSM))]
internal class DancePatch : MonoBehaviour
{
    private PlayMakerFSM _control = null!;
    private PlayMakerFSM _parentControl = null!;  
    private int Phase = 1;
    public static ManualLogSource Log;



    private void Start()
    {
        StartCoroutine(SetupBoss());
    }

    private IEnumerator SetupBoss()
    {
        GetComponents();
        ModifyParentFsm();  
        yield return null;
    }

    private void GetComponents()
    {
        _control = FSMUtility.LocateMyFSM(base.gameObject, "Control");

        var initState = _control.FsmStates.FirstOrDefault(state => state.Name == "Init");
        if (initState != null)
        {
            var getParentAction = initState.Actions
                .OfType<GetParent>()
                .FirstOrDefault();

            if (getParentAction != null)
            {
                var parentGO = getParentAction.storeResult.Value;
                if (parentGO != null)
                {
                    _parentControl = parentGO.GetComponent<PlayMakerFSM>();
                    Log.LogInfo("Parent Control FSM found.");
                }
            }
            else
            {
                Log.LogInfo("GetParent action not found in Init state.");
            }
        }
        else
        {
            Log.LogInfo("Init state not found in Control FSM.");
        }
    }

    /// <summary>
    /// Modify parent FSM behaviors.
    /// </summary>
    private void ModifyParentFsm()
    {
        if (_parentControl == null) return;

        ModifyPhase2();
        ModifyPhase3();
        ModifyFinalPhase();
    }


    private void ModifyPhase2()
    {
        var p2StartState = _parentControl.FsmStates.FirstOrDefault(state => state.Name == "Set Phase 2");
        if (p2StartState != null)
        {
            foreach (var action in p2StartState.Actions)
            {
                if (action is SetFsmFloat setFsmFloat)
                {
                    setFsmFloat.setValue = 0.43f;
                }
            }
        }
    }

    private void ModifyPhase3()
    {
        var p3StartState = _parentControl.FsmStates.FirstOrDefault(state => state.Name == "Set Phase 3");
        if (p3StartState != null)
        {
            foreach (var action in p3StartState.Actions)
            {
                if (action is SetFsmFloat setFsmFloat)
                {
                    setFsmFloat.setValue = 0.43f;
                }
            }
        }
    }

    private void ModifyFinalPhase()
    {
        var p2StartState = _parentControl.FsmStates.FirstOrDefault(state => state.Name == "Set Final Phase");
        if (p2StartState != null)
        {
            foreach (var action in p2StartState.Actions)
            {
                if (action is SetFsmFloat setFsmFloat)
                {
                    setFsmFloat.setValue = 0.65f;
                }
            }
        }
    }
    
}