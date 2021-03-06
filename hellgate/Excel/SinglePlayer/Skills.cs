﻿using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Skills
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String skill;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 buffer;
        [ExcelOutput(IsBitmask = true)]
        public Bitmask01 bitmask1;
        [ExcelOutput(IsBitmask = true)]
        public Bitmask02 bitmask2;
        [ExcelOutput(IsBitmask = true)]
        public Bitmask03 bitmask3;
        [ExcelOutput(IsBitmask = true)]
        public Bitmask04 bitmask4;
        [ExcelOutput(IsBitmask = true)]
        public Bitmask05 bitmask5;
        [ExcelOutput(IsBitmask = true)]
        public Bitmask06 bitmask6;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 displayName;//stridx
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public String descriptionStringFunction;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public String effectStringFunction;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 192)]
        public String skillBonusFunction;//doesn't appear to be used
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 192)]
        public String accumulationStringFunction;//doesn't appear to be used
        public Int32 unknown101;
        public Int32 unknown102;
        public Int32 unknown103;
        public Int32 unknown104;
        public Int32 unknown105;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 descriptionString;//stridx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 effectString;//stridx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 skillBonusString;//stridx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 accumulationString;//stridx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 stringAfterRequiredWeapon;//stridx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skillsToAccumulate1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skillsToAccumulate2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skillsToAccumulate3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public String events;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String activator;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String largeIcon;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String smallIcon;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] unknown2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "FONTCOLORS")]
        public Int32 iconColor;
        [ExcelOutput(IsTableIndex = true, TableStringId = "FONTCOLORS")]
        public Int32 iconBackgroundColor;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLTABS")]
        public Int32 skillTab;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 124)]
        byte[] unknown3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLGROUPS")]
        public Int32 skillGroup1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLGROUPS")]
        public Int32 skillGroup2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLGROUPS")]
        public Int32 skillGroup3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLGROUPS")]
        public Int32 skillGroup4;
        public Int32 skillPageColumn;
        public Int32 skillPageRow;
        public Int32 level1;
        public Int32 level2;
        public Int32 level3;
        public Int32 level4;
        public Int32 level5;
        public Int32 level6;
        public Int32 level7;
        public Int32 level8;
        public Int32 level9;
        public Int32 level10;
        public Int32 level11;
        public Int32 level12;
        public Int32 level13;
        public Int32 level14;
        public Int32 level15;
        public Int32 maxLevel;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public String summonedAi;
        public Int32 unknown201;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 summonedInvLocation1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 summonedInvLocation2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 summonedInvLocation3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 summonedInvLocation4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 summonedInvLocation5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 summonedInvLocation6;
        public Int32 maxSummonedMinorPetClasses;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 uiThrobsOnState;//idx
        public float powerCost;
        public float powerCostPerLevel;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        byte[] unknown5;
        public Int32 priority;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 requiredStats1;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 requiredStats2;
        public Int32 requiredStatValuesA1;//these seem to be unused
        public Int32 requiredStatValuesA2;
        public Int32 requiredStatValuesA3;
        public Int32 requiredStatValuesA4;
        public Int32 requiredStatValuesA5;
        public Int32 requiredStatValuesA6;
        public Int32 requiredStatValuesA7;
        public Int32 requiredStatValuesA8;
        public Int32 requiredStatValuesA9;
        public Int32 requiredStatValuesA10;
        public Int32 requiredStatValuesA11;
        public Int32 requiredStatValuesA12;
        public Int32 requiredStatValuesA13;
        public Int32 requiredStatValuesA14;
        public Int32 requiredStatValuesA15;
        public Int32 requiredStatValuesB1;
        public Int32 requiredStatValuesB2;
        public Int32 requiredStatValuesB3;
        public Int32 requiredStatValuesB4;
        public Int32 requiredStatValuesB5;
        public Int32 requiredStatValuesB6;
        public Int32 requiredStatValuesB7;
        public Int32 requiredStatValuesB8;
        public Int32 requiredStatValuesB9;
        public Int32 requiredStatValuesB10;
        public Int32 requiredStatValuesB11;
        public Int32 requiredStatValuesB12;
        public Int32 requiredStatValuesB13;
        public Int32 requiredStatValuesB14;
        public Int32 requiredStatValuesB15;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 requiredSkills1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 requiredSkills2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 requiredSkills3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 requiredSkills4;
        public Int32 levelsOfRequiredSkills1;
        public Int32 levelsOfRequiredSkills2;
        public Int32 levelsOfRequiredSkills3;
        public Int32 levelsOfRequiredSkills4;
        [ExcelOutput(IsBool = true)]
        public Int32 bOnlyRequireOne;
        [ExcelOutput(IsBool = true)]
        public Int32 bUsesCraftingPoints;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 weaponLocation1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 weaponLocation2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 fallBackSkills1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 fallBackSkills2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 fallBackSkills3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 68)]
        byte[] unknown6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 targetUnittype;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 ignoreUnitType;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 ignoreTargetsWithState1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 ignoreTargetsWithState2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 selectTargetsWithState1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 selectTargetsWithState2;
        public Int32 unknown301;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODES")]
        public Int32 modeOverride;//idx
        //[ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 bonusSkills0;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 bonusSkills1;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 bonusSkills2;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 bonusSkills3;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 bonusSkills4;
        [ExcelOutput(IsScript = true)]
        public Int32 bonusSkillScript0;
        [ExcelOutput(IsScript = true)]
        public Int32 bonusSkillScript1;
        [ExcelOutput(IsScript = true)]
        public Int32 bonusSkillScript2;
        [ExcelOutput(IsScript = true)]
        public Int32 bonusSkillScript3;
        [ExcelOutput(IsScript = true)]
        public Int32 bonusSkillScript4;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar0;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar1;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar2;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar3;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar4;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar5;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar6;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar7;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar8;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar9;
        [ExcelOutput(IsScript = true)]
        public Int32 rangeMultScript;
        [ExcelOutput(IsScript = true)]
        public Int32 cost;
        [ExcelOutput(IsScript = true)]
        public Int32 coolDownPercentChange;
        [ExcelOutput(IsScript = true)]
        public Int32 statsTransferOnAttack;
        [ExcelOutput(IsScript = true)]
        public Int32 statsSkillEvent;
        [ExcelOutput(IsScript = true)]
        public Int32 statsSkillEventServer;
        [ExcelOutput(IsScript = true)]
        public Int32 statsSkillEventServerPostProcess;
        [ExcelOutput(IsScript = true)]
        public Int32 statsServerPostLaunch;
        [ExcelOutput(IsScript = true)]
        public Int32 statsServerPostLaunchPost;
        [ExcelOutput(IsScript = true)]
        public Int32 statsPostLaunch;
        [ExcelOutput(IsScript = true)]
        public Int32 statsOnStateSet;
        [ExcelOutput(IsScript = true)]
        public Int32 statsServerOnStateSet;
        [ExcelOutput(IsScript = true)]
        public Int32 statsServerOnStateSetPost;
        [ExcelOutput(IsScript = true)]
        public Int32 statsOnStateSetPost;
        [ExcelOutput(IsScript = true)]
        public Int32 statsOnPulseServerOnly;
        [ExcelOutput(IsScript = true)]
        public Int32 statsOnDeSelectServerOnly;
        [ExcelOutput(IsScript = true)]
        public Int32 statsOnPulse;
        [ExcelOutput(IsScript = true)]
        public Int32 statsOnLevelChange;
        [ExcelOutput(IsScript = true)]
        public Int32 statsOnSkillStart;
        [ExcelOutput(IsScript = true)]
        public Int32 statsScriptOnTarget;
        [ExcelOutput(IsScript = true)]
        public Int32 scriptFromScriptEvents;
        [ExcelOutput(IsScript = true)]
        public Int32 selectCost;
        [ExcelOutput(IsScript = true)]
        public Int32 startCondition;
        [ExcelOutput(IsScript = true)]
        public Int32 startConditionOntarget;
        [ExcelOutput(IsScript = true)]
        public Int32 stateDurationInTicks;
        [ExcelOutput(IsScript = true)]
        public Int32 stateDurationBonus;
        [ExcelOutput(IsScript = true)]
        public Int32 activatorCondition;
        [ExcelOutput(IsScript = true)]
        public Int32 activatorConditionOnTarget;
        [ExcelOutput(IsScript = true)]
        public Int32 eventChance;
        [ExcelOutput(IsScript = true)]
        public Int32 eventParam0;
        [ExcelOutput(IsScript = true)]
        public Int32 eventParam1;
        [ExcelOutput(IsScript = true)]
        public Int32 eventParam2;
        [ExcelOutput(IsScript = true)]
        public Int32 eventParam3;
        [ExcelOutput(IsScript = true)]
        public Int32 infoScript;
        [ExcelOutput(IsScript = true)]
        public Int32 stateRemovedServer;
        [ExcelOutput(IsScript = true)]
        public Int32 powerCostScript;
        [ExcelOutput(IsScript = true)]
        public Int32 coolDownSkillScript;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skillOnpulse;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 selectCheckStat;//idx
        public StartFunc startFunc;
        public Int32 targetFunc;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        byte[] unknown8;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 givesSkill;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 extraSkillToTurnOn;//idx
        public PlayerInputOverride playerInputOverride;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 requiresUnitType;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 requiresWeaponUnitType;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        byte[] unknown9;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 fuseMissilesOnStateClear;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 requiresState;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 prohibitingState0;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 prohibitingState1;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 prohibitingState2;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 prohibitingState3;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 stateOnSelect;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 clearStateOnSelect;//idx
        public Int32 holdTicks;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODES")]
        public Int32 holdWithMode;//idx
        public Int32 warmUpTicks;
        public Int32 testTicks;
        public Int32 coolDown;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLGROUPS")]
        public Int32 coolDownSkillGroup;//idx
        public Int32 coolDownForGroup;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 coolDownFinishedSound;//idx
        public Int32 coolDownMinPercent;
        public ActivatorKey activatorKey;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODES")]
        public Int32 activateMode;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 activateSkill;//idx
        public Int32 activatePriority;
        public float rangeMin;
        public float rangeMax;
        public float firingCone;
        public float rangeDesired;
        public float rangePercentPerLevel;
        public float weaponRangeMultiplier;
        public float impactForwardBias;
        public float modeSpeed;
        [ExcelOutput(IsTableIndex = true, TableStringId = "DAMAGETYPES")]
        public Int32 damageTypeOverride;//idx
        public float damageMultiplier;
        public Int32 maxExtraSpreadBullets;
        public Int32 spreadBulletMultiplier;
        public float reflectiveLifeTimeInSeconds;
        public float Param1;
        public float Param2;
        public Usage usage;
        public Family family;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNIT_EVENT_TYPES")]
        public Int32 unitEventTrigger0;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNIT_EVENT_TYPES")]
        public Int32 unitEventTrigger1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNIT_EVENT_TYPES")]
        public Int32 unitEventTrigger2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNIT_EVENT_TYPES")]
        public Int32 unitEventTrigger3;
        public Int32 unitEventTriggerChanceScript;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        byte[] unknown10;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 linkedLevelSkill0;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 linkedLevelSkill1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 linkedLevelSkill2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skillParent;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MISSILES")]
        public Int32 fieldMissile;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        byte[] unknown11;

        [FlagsAttribute]
        public enum Bitmask01 : uint
        {
            _undefined1 = (1 << 0),
            _undefined2 = (1 << 1),
            usesWeapon = (1 << 2),
            weaponIsRequired = (1 << 3),
            weaponTargeting = (1 << 4),
            usesWeaponSkill = (1 << 5),
            useWeaponCooldown = (1 << 6),
            cooldownUnitInsteadOfWeapon = (1 << 7),
            _undefined3 = (1 << 8),
            useWeaponIcon = (1 << 9),
            useAllWeapons = (1 << 10),
            combineWeaponDamage = (1 << 11),
            usesUnitFiringError = (1 << 12),
            displayFiringError = (1 << 13),
            _undefined4 = (1 << 14),
            checkLOS = (1 << 15),
            noLowAiming3rdPerson = (1 << 16),
            noHighAiming3rdPerson = (1 << 17),
            canTargetUnit = (1 << 18),
            findTargetUnit = (1 << 19),
            mustTargetUnit = (1 << 20),
            mustNotTargetUnit = (1 << 21),
            monsterMustTargetUnit = (1 << 22),
            cannotRetarget = (1 << 23),
            verifyTarget = (1 << 24),
            verifyTargetOnRequest = (1 << 25),
            targetsPosition = (1 << 26),
            keepTargetPositionOnRequest = (1 << 27),
            targetPosInStart = (1 << 28),
            targetDead = (1 << 29),
            dyingOnStart = (1 << 30),
            dyingAfterStart = ((uint)1 << 31)
        }

        [FlagsAttribute]
        public enum Bitmask02 : uint
        {
            targetSelectableDead = 1,
            targetFriend = 2,
            targetEnemy = 4,
            targetContainer = 8,
            targetDestructables = 16,
            targetOnlyDying = 32,
            dontTargetDestructables = 64,
            targetPets = 128,
            ignoreTeams = 256,
            allowUntargetables = 512,
            uiUsersTarget = 1024,
            dontFaceTarget = 2048,
            mustFaceForward = 4096,
            aimAtTarget = 8192,
            // = 16384,
            useMouseSkillsTargeting = 32768,
            isMelee = 65536,
            doMeleeItemEvents = 131072,
            canDelayMelee = 262144,
            deadCanDo = 524288,
            stopAll = 1048576,
            stopOnDead = 2097152,
            startOnSelect = 4194304,
            alwaysSelected = 8388608,
            // = 16777216,
            highlightWhenSelected = 33554432,
            repeatFire = 67108864,
            repeatAll = 134217728,
            hold = 268435456,
            loopMode = 536870912,
            stopHoldingSkills = 1073741824,
            holdOtherSkills = 2147483648
        }

        [FlagsAttribute]
        public enum Bitmask03 : uint
        {
            preventOtherSkills = 1,
            preventSkillsByPriority = 2,
            runToTarget = 4,
            skillIsOn = 8,
            onGroundOnly = 16,
            learnable = 32,
            hotkeyable = 64,
            canGoInMouseButton = 128,
            canGoInLeftMouseButton = 256,
            usesPower = 512,
            drainsPower = 1024,
            ajustPowerByLevel = 2048,
            powerCostBoundedByMaxPower = 4096,
            allowRequest = 8192,
            trackRequest = 16384,
            trackMetrics = 32768,
            saveMissiles = 65536,
            removeMissilesOnStop = 131072,
            stopOnCollied = 262144,
            noPlayerSkillInput = 524288,
            noPlayerMovementInput = 1048576,
            noIdleOnStop = 2097152,
            doNotClearRemoveOnMoveStates = 4194304,
            useRange = 8388608,
            displayRange = 16777216,
            getHitCanDo = 33554432,
            movingCantDo = 67108864,
            playerStopMoving = 134217728,
            constantCooldown = 268435456,
            ignoreCooldownOnStart = 536870912,
            playCooldownOnWeapon = 1073741824,
            useUnitsCooldown = 2147483648
        }

        [FlagsAttribute]
        public enum Bitmask04 : uint
        {
            displayCooldown = 1,
            checkRangeOnStart = 2,
            checkMeleeRangeOnStart = 4,
            dontUseWeaponRange = 8,
            canStartInTown = 16,
            cantStartInPvp = 32,
            canStartInRts = 64,
            alwaysTestCanStartSkill = 128,
            isAggressive = 256,
            angersTargetOnExecute = 512,
            isRanged = 1024,
            isSpell = 2048,
            serverOnly = 4096,
            clientOnly = 8192,
            checkInventorySpace = 16384,
            activatorWhileMoving = 32768,
            activatorIgnoreMoving = 65536,
            canNotDoInHellrift = 131072,
            uiIsRedOnFalback = 262144,
            impactUsesAimBone = 524288,
            decoyCannotUse = 1048576,
            // = 2097152,
            // = 4194304,
            doNotPreferForMouse = 8388608,
            useHolyAuraForRange = 16777216,
            // = 33554432,
            disallowSameSkill = 67108864,
            dontUseRangeForMeleeEvents = 134217728,
            forceSkillRangeForMeleeEvents = 268435456,
            disabled = 536870912,
            skillLevelFromStateTarget = 1073741824,
            usesItemRequirements = 2147483648
        }

        [FlagsAttribute]
        public enum Bitmask05 : uint
        {
            preventAnimationCutoff = 1,
            movesUnit = 2,
            selectsAMeleeSkill = 4,
            requiresSkillLevel = 8,
            disableClientsidePathing = 16,
            executeRequestedSkillOnMeleeAttack = 32,
            canBeExecutedFromMeleeAttack = 64,
            dontStopRequestAfterExecute = 128,
            lerpCameraWhileOn = 256,
            forceUseWeaponTargeting = 512,
            dontClearCooldownOnDeath = 1024,
            dontCooldownOnStart = 2048,
            powerOnEvent = 4096,
            // = 8192,
            defaultShiftSkillEnabled = 16384,
            shiftSkillAlwaysEnabled = 32768,
            canKillPetsForPowerCost = 65536,
            reducePowerMaxByPowerCost = 131072,
            skillFromUnitEventTriggerNeedsDam = 262144,
            aiIsBusyWhileOn = 524288,
            dontSelectOnPurchase = 1048576,
            // = 2097152,
            neverSetCooldown = 4194304,
            ignorePreventAllSkills = 8388608,
            mustStartInPortalSafeLevel = 16777216,
            // = 33554432,
            dontTargetPets = 67108864,
            requirePathToTarget = 134217728,
            fireToLocation = 268435456,
            testFiringConeOnStart = 536870912,
            ignoreChampions = 1073741824,
            faceTargetPosition = 2147483648
        }

        [FlagsAttribute]
        public enum Bitmask06 : uint
        {
            targetDeadAndAlive = 1,
            restartSkillOnUnitReactivate = 2,
            subscriptionRequiredToLearn = 4,
            subscriptionRequiredToUse = 8,
            forceUiToShowEffect = 16,
            dontIgnoreOwnedState = 32,
            ghostCanDo = 64,
            useDoneForMissilePosition = 128,
            transferDamageToPets = 256,
            dontStagger = 512,
            doesNotActivelyUseWeapon = 1024
        }

        public enum StartFunc
        {
            Neg = -1,
            Null = 0,
            Generic = 1,
            NoModes = 2,
            WarmUp = 3,
            NoModes2 = 4
        }
        public enum PlayerInputOverride
        {
            Null = -1,
            None = 0,
            Forward = 1,
            Back = 2,
            Left = 4,
            ForwardLeft = 5,
            BackLeft = 6,
            Right = 8,
            ForwardRight = 9,
            BackRight = 10
        }
        public enum ActivatorKey
        {
            Null = -1,
            Shift = 0,
            Potion = 1,
            Melee = 2
        }
        public enum Usage
        {
            Null = -1,
            Active = 0,
            Passive = 1,
            Toggle = 2
        }

        public enum Family
        {
            Null = -1,
            TurretGround = 0,
            TurretFlying = 1
        }

        public static class Mysh
        {
            public static readonly byte[] Data = new byte[]
            {
                0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x78, 0x78, 0x00, 0x00, 0x00, 0x39, 0x00, 0x00,
                0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x6D, 0x79, 0x73, 0x68, 0x03, 0x00, 0x00,
                0x00, 0x03, 0x00, 0x00, 0x00, 0x73, 0x65, 0x6C,
                0x00, 0x76, 0x02, 0x00, 0x39, 0x00, 0x00, 0x00,
                0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x6D, 0x79, 0x73, 0x68, 0x03, 0x00, 0x00, 0x00,
                0x07, 0x00, 0x00, 0x00, 0x64, 0x6D, 0x67, 0x74,
                0x79, 0x70, 0x65, 0xF6, 0x03, 0x4F, 0x8B, 0x39,
                0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x04,
                0x00, 0x00, 0x00 
            };
        }
    }
}
