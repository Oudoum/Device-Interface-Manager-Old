//------------------------------------------------------------------------------
//
//  PMDG 747QOTSII external connection SDK
//  Copyright (c) 2017 Precision Manuals Development Group
// 
//  Converted from unmanged to managed language type by Oudoum
// 
//------------------------------------------------------------------------------
using System.Runtime.InteropServices;

namespace Device_Interface_Manager.SimConnectProfiles.PMDG;

public class PMDG_747QOTSII_SDK
{
    public const string DATA_NAME = "PMDG_747QOTSII_Data";
    public const string CONTROL_NAME = "PMDG_747QOTSII_Control";
    public const string CDU_0_NAME = "PMDG_747QOTSII_CDU_0";
    public const string CDU_1_NAME = "PMDG_747QOTSII_CDU_1";
    public const string CDU_2_NAME = "PMDG_747QOTSII_CDU_2";


    public enum PMDG_747QOTSII
    {
        DATA_ID = 0x504D444B,
        DATA_DEFINITION = 0x504D444C,
        CONTROL_ID = 0x504D444D,
        CONTROL_DEFINITION = 0x504D444E,
        CDU_0_ID = 0x4E477835,
        CDU_1_ID = 0x4E477836,
        CDU_2_ID = 0x4E477837,
        CDU_0_DEFINITION = 0x4E477838,
        CDU_1_DEFINITION = 0x4E477839,
        CDU_2_DEFINITION = 0x4E47783A
    };


    // NOTE - add these lines to the 747QOTSII_Options.ini file: 
    //
    //[SDK]
    //EnableDataBroadcast=1
    //
    // to enable the aircraft data sending from the 747QOTSII.
    //
    //
    // Add any of these lines to the [SDK] section of the 747QOTSII_Options.ini file: 
    //
    //EnableCDUBroadcast.0=1 
    //EnableCDUBroadcast.1=1 
    //EnableCDUBroadcast.2=1 
    //
    // to enable the contents of the corresponding CDU screen to be sent to external programs.


    // 747QOTSII data structure
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct PMDG_747QOTSII_Data
    {
        ////////////////////////////////////////////
        // Controls and indicators
        ////////////////////////////////////////////


        // Overhead Maintenance Panel
        //------------------------------------------

        // Electrical
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] ELEC_GenFieldReset;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] ELEC_APUFieldReset;
        [MarshalAs(UnmanagedType.I1)] public bool ELEC_SplitSystemBreaker;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] ELEC_annunGen_FIELD_OFF;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] ELEC_annunAPU_FIELD_OFF;
        [MarshalAs(UnmanagedType.I1)] public bool ELEC_annunSplitSystemBreaker_OPEN;

        // Fuel
        [MarshalAs(UnmanagedType.I1)] public bool FUEL_CWTScavengePump_Sw_ON;
        [MarshalAs(UnmanagedType.I1)] public bool FUEL_Reserve23Xfer_Sw_OPEN;

        // EEC MAINT
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] ENG_EECPower_Sw_TEST;

        // Flight Controls Hydraulic Valve Power			
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] FCTL_WingHydValve_Sw_SHUT_OFF;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] FCTL_TailHydValve_Sw_SHUT_OFF;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] FCTL_annunTailHydVALVE_CLOSED;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] FCTL_annunWingHydVALVE_CLOSED;

        // Air Conditioning
        public byte AIR_LowerLobeFlowRate_Selector;   // 744 Freighter only, 0: BOTH, 1: AFT LOW ... 6: AFT HIGH
        public byte AIR_LowerLobeAftCargoHt_Selector; // 748 only, 0: OFF, 1: LOW  2: HIGH


        // Overhead Panel
        //------------------------------------------

        // IRS
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)] public byte[] IRS_Selector;                  // left/center/right   0: OFF  1: ALIGN  2: NAV  3: ATT
        [MarshalAs(UnmanagedType.I1)] public bool IRS_annunON_BAT;

        // Electrical
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] ELEC_annunUtilOFF;
        [MarshalAs(UnmanagedType.I1)] public bool ELEC_Battery_Sw_ON;
        public byte ELEC_APU_Selector;                    // 0: OFF  1: ON  2: START
        public byte ELEC_StandbyPowerSw;              // 0: OFF  1: AUTO  2: BAT
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] ELEC_APUGen_Sw_ON;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] ELEC_UtilSw;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] ELEC_BusTie_Sw_AUTO;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] ELEC_annunBusTieISLN;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] ELEC_Gen_Sw_ON;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] ELEC_IDGDiscSw;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] ELEC_ExtPwrSw;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] ELEC_annunExtPowr_ON;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] ELEC_annunExtPowr_AVAIL;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] ELEC_annunAPUGen_ON;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] ELEC_annunAPUGen_AVAIL;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] ELEC_annunGenOFF;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] ELEC_annunIDGDiscDRIVE;

        // Hydraulics
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] HYD_EnginePump_Sw_ON;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public byte[] HYD_DemandPump_Selector;           // 0: OFF  1: AUTO  2: ON  3: AUX (for selector 4 only)
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] HYD_annunSYS_FAULT;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] HYD_annunEnginePumpPRESS;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] HYD_annunDemandPumpPRESS;
        [MarshalAs(UnmanagedType.I1)] public bool HYD_RamAirTurbineSw;               // Applies to 747-8 variants
        [MarshalAs(UnmanagedType.I1)] public bool HYD_annunRamAirTurbinePRESS;       // Applies to 747-8 variants
        [MarshalAs(UnmanagedType.I1)] public bool HYD_annunRamAirTurbineUNLKD;       // Applies to 747-8 variants

        // Fire Protection
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public byte[] FIRE_EngineHandle;             // ENG 1-4   0: IN (NORMAL)  1: PULLED OUT  2: TURNED LEFT  3: TURNED RIGHT  (2 & 3 are momenentary positions)
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] FIRE_EngineHandleUnlock_Sw;     // ENG 1-4   MOMENTARY SWITCH resets when handle pulled
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] FIRE_annunENG_BTL_DISCH;            // BTL A Left, BTL B Left, BTL A Right, BTL B Right
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] FIRE_CargoFire_Sw_Arm;          // FWD/AFT
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] FIRE_annunCargoFire;                // FWD/AFT
        [MarshalAs(UnmanagedType.I1)] public bool FIRE_MainDeckFire_Sw_Arm;          // Freighter only
        [MarshalAs(UnmanagedType.I1)] public bool FIRE_annunMainDeckFire;                // Freighter only
        [MarshalAs(UnmanagedType.I1)] public bool FIRE_CargoFireDisch_Sw;                // MOMENTARY SWITCH
        [MarshalAs(UnmanagedType.I1)] public bool FIRE_annunCargoDISCH;
        [MarshalAs(UnmanagedType.I1)] public bool FIRE_FireOvhtTest_Sw;              // MOMENTARY SWITCH
        public byte FIRE_APUHandle;                       // 0: IN (NORMAL)  1: PULLED OUT  2: TURNED LEFT  3: TURNED RIGHT  (2 & 3 are momentary positions)
        [MarshalAs(UnmanagedType.I1)] public bool FIRE_APUHandleUnlock_Sw;           // MOMENTARY SWITCH resets when handle pulled
        [MarshalAs(UnmanagedType.I1)] public bool FIRE_annunAPU_BTL_DISCH;

        // Engine Control
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] ENG_EECMode_Sw_NORM;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] ENG_Start_Sw_Pulled;
        [MarshalAs(UnmanagedType.I1)] public bool ENG_ConIginition_Sw_ON;
        public byte ENG_StbyIginition_Selector;           // 0: 1, 1: NORM, 2: 2
        public int ENG_AutoIginition_Selector;         // GE engines: SINGLE/BOTH, PW engines: 1/BOTH/2, RR engines:1/NORM/2/BOTH 
        [MarshalAs(UnmanagedType.I1)] public bool ENG_Autostart_Sw_ON;               // Freighter Only
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] ENG_Start_Light;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] ENG_annunALTN;

        // Fuel
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] FUEL_CrossFeed_Sw;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] FUEL_MainPumpFwd_Sw;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] FUEL_MainPumpAft_Sw;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] FUEL_OvrdPumpFwd_Sw;                // OVRD 2  / OVRD 3
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] FUEL_OvrdPumpAft_Sw;                // OVRD 2  / OVRD 3
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] FUEL_PumpStab_Sw;               // left / right, passenger only
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] FUEL_PumpCtr_Sw;                    // left / right
        [MarshalAs(UnmanagedType.I1)] public bool FUEL_XferMain14_Sw;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] FUEL_JettisonNozle_Sw;          // left / right
        public byte FUEL_JettisonArm_Selector;            // Without MLW option: 0/1 NOT USED 2=OFF 3=A 4=B, With MLW option: 0=SEL_A 1=MLW_A 2=OFF 3=MLW_B 4=SEL_B
        public byte FUEL_FuelToRemain_Selector;           // 0: DECR  1: Neutral  2: INCR

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] FUEL_annunXFEED_VALVE;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] FUEL_annunPRESS_MainFwd;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] FUEL_annunPRESS_MainAft;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] FUEL_annunPRESS_OvrdFwd;            // OVRD 2  / OVRD 3	
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] FUEL_annunPRESS_OvrdAft;            // OVRD 2  / OVRD 3
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] FUEL_annunPRESS_Stab;           // left / right, passenger only
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] FUEL_annunPRESS_Ctr;                // left / right
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] FUEL_annunJettisonNozleVALVE;   // left / right

        // Anti-Ice, Window Heat
        public byte ICE_WingAntiIceSw;                    // 0: OFF  1: AUTO  2: ON  (AUTO applies only to 747-8)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public byte[] ICE_EngAntiIceSw;              // 0: OFF  1: AUTO  2: ON  (AUTO applies only to 747-8)
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] ICE_annunEngAntiIceVALVE;
        [MarshalAs(UnmanagedType.I1)] public bool ICE_annunWingAntiIceVALVE;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] ICE_WindowHeat_Sw_ON;           // Left/Right
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] ICE_annunWindowHeatINOP;            // Left/Right

        // Rain Protection
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)] public byte[] WIPERS_Selector;                   // left/right   0: OFF  1: INT  2: LOW  3:HIGH; INT applies only to 747-8
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] WASHER_Sw;                      // left/right MOMENTARY action
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] RAIN_REP_Sw;                        // left/right MOMENTARY action, passenger only

        // Flight Deck Lights
        public byte LTS_DomeLightKnob;                    // Position 0...150
        public byte LTS_CktBkrOverheadKnob;               // Position 0...150
        public byte LTS_GlareshieldPNLlKnob;          // Position 0...150
        public byte LTS_GlareshieldFLOODKnob;         // Position 0...150
        public byte LTS_AisleStandPNLKnob;                // Position 0...150
        public byte LTS_AisleStandFLOODKnob;          // Position 0...150
        [MarshalAs(UnmanagedType.I1)] public bool LTS_Storm_Sw_ON;
        public byte LTS_IndLightsTestSw;              // 0: TEST  1: BRT  2: DIM

        // Exterior Lights
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] LTS_LandingLights_Sw_ON;            // OUTB L, OUTB R, INBD L, INBD R
        public byte LTS_Beacon_Sw;                        // 0: LWR  1: OFF  2: BOTH
        [MarshalAs(UnmanagedType.I1)] public bool LTS_NAV_Sw_ON;
        [MarshalAs(UnmanagedType.I1)] public bool LTS_Logo_Sw_ON;
        [MarshalAs(UnmanagedType.I1)] public bool LTS_Wing_Sw_ON;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] LTS_RunwayTurnoff_Sw_ON;
        [MarshalAs(UnmanagedType.I1)] public bool LTS_Taxi_Sw_ON;
        [MarshalAs(UnmanagedType.I1)] public bool LTS_Strobe_Sw_ON;

        // Air Systems - Pressurization
        [MarshalAs(UnmanagedType.I1)] public bool AIR_LdgAlt_PushOn_Sw;              // MOMENTARY action switch 
        public byte AIR_LdgAlt_Selector;              // 0: DECR  1: Neutral  2: INCR
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] AIR_OutflowValveMan_Sw;         // left / right	
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)] public int[] AIR_OutflowValveNeedle;          // left/right Value 0...100 (CL...OPEN)
        public byte AIR_OutflowValves_Selector;           // 0: OPEN  1: Neutral  2: CLOSE
        public byte AIR_CabinAltAuto_Selector;            // 0: NORM  1: A  2: B
        [MarshalAs(UnmanagedType.I1)] public bool AIR_SmokeEvacHandle_Pulled;            // located on overhead CB panel 

        // Air Systems - Air Conditioning
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)] public byte[] AIR_Pack_Selector;             // values 0: OFF  1: NORM  2: A  3: B (0/1 only for 748)
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] AIR_TrimAir_Sw_On;              // 744: only the first used, 748: both used (left/right)
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] AIR_RecircFan_Sw_On;                // upper / lower	
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)] public byte[] AIR_TempSelector;              // array  0=FLT DECK, 1=MAIN DECK FWD, 2=MAIN DECK AFT, 
                                                                                                          //        3=LOWER LOBE FWD, 4=LOWER LOBE AFT, 5=PASS TEMP
                                                                                                          // 0: C ... 60: W ... 70: MAN or ALTN (pass)
        [MarshalAs(UnmanagedType.I1)] public bool AIR_PackReset_Sw_Pushed;           // MOMENTARY action		
        public byte AIR_EquipCooling_Selector;            // 0: STBY  1: NORM  2: OVRD
        [MarshalAs(UnmanagedType.I1)] public bool AIR_HighFlow_Sw_On;
        [MarshalAs(UnmanagedType.I1)] public bool AIR_Gasper_Sw_On;                  // Passenger 744 only
        [MarshalAs(UnmanagedType.I1)] public bool AIR_FltDeckFan_Sw_On;              // Freighter only
        [MarshalAs(UnmanagedType.I1)] public bool AIR_AftCargoHeat_Sw_On;                // 744 only
        [MarshalAs(UnmanagedType.I1)] public bool AIR_ZoneReset_Sw_Pushed;           // 744 only, MOMENTARY action	
        [MarshalAs(UnmanagedType.I1)] public bool AIR_AltnVent_Sw_On;                    // 748 only
        public byte AIR_AltnVent_Selector;                // 748 only, 0:CLOSE  1: NEUTRAL  2: OPEN
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 3)] public bool[] AIR_annunPackOFF;               // 748 only		
        [MarshalAs(UnmanagedType.I1)] public bool AIR_annunPack_SYS_FAIL;
        [MarshalAs(UnmanagedType.I1)] public bool AIR_annunZone_SYS_FAIL;                // 744 only
        [MarshalAs(UnmanagedType.I1)] public bool AIR_annunAftCragoHeat_TEMP;            // 744 only

        // Air Systems - Bleed Air
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] AIR_EngBleedAir_Sw_ON;
        [MarshalAs(UnmanagedType.I1)] public bool AIR_APUBleedAir_Sw_ON;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] AIR_IsolationValve_Sw;          // left / right 
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] AIR_annunEngBleedAirOFF;
        [MarshalAs(UnmanagedType.I1)] public bool AIR_annunAPUBleedAirVALVE;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] AIR_annunIsolationVALVE;            // left / right 
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] AIR_annun_SYS_FAULT;                // 744 only 

        // Miscellaneous, top of overhead
        public byte LTS_EmerLightsSelector;               // 0: OFF  1: ARMED  2: ON
        public byte COMM_CAPTAudio_Selector;          // 0: NORM  1: VHF-L DIRECT (freighter only)
        public byte COMM_OBSAudio_Selector;               // 0: CAPT  1: NORM  2: F/O
        [MarshalAs(UnmanagedType.I1)] public bool COMM_ServiceInterphoneSw;
        [MarshalAs(UnmanagedType.I1)] public bool COMM_CargoCabinInterphoneSw;       // Freighter only
        [MarshalAs(UnmanagedType.I1)] public bool OXY_Oxygen_Sw_On;                  // PASS OXY or SUPRNMRY OXY (freighter)
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] FCTL_YawDamper_Sw;              // Upper / Lower
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] FCTL_annunYawDamperINOP;            // Upper / Lower



        // Glareshield
        //------------------------------------------

        // Master Warning/Caution
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] WARN_Reset_Sw_Pushed;           // MOMENTARY action
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] WARN_annunMASTER_WARNING;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] WARN_annunMASTER_CAUTION;

        // EFIS switches (left / right)
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] EFIS_MinsSelBARO;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] EFIS_BaroSelHPA;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)] public byte[] EFIS_VORADFSel1;                   // 0: VOR  1: OFF  2: ADF
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)] public byte[] EFIS_VORADFSel2;                   // 0: VOR  1: OFF  2: ADF
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)] public byte[] EFIS_ModeSel;                  // 0: APP  1: VOR  2: MAP  3: PLAN
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)] public byte[] EFIS_RangeSel;                 // 0: 10 ... 6: 640
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)] public byte[] EFIS_MinsKnob;                 // 0..99
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)] public byte[] EFIS_BaroKnob;                 // 0..99

        // EFIS MOMENTARY action (left / right)
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] EFIS_MinsRST_Sw_Pushed;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] EFIS_BaroSTD_Sw_Pushed;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] EFIS_ModeCTR_Sw_Pushed;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] EFIS_RangeTFC_Sw_Pushed;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] EFIS_WXR_Sw_Pushed;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] EFIS_STA_Sw_Pushed;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] EFIS_WPT_Sw_Pushed;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] EFIS_ARPT_Sw_Pushed;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] EFIS_DATA_Sw_Pushed;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] EFIS_POS_Sw_Pushed;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] EFIS_TERR_Sw_Pushed;

        // Display Select Panel	- These are all MOMENTARY SWITCHES
        [MarshalAs(UnmanagedType.I1)] public bool DSP_L_INBD_Sw;     // 748 only						
        [MarshalAs(UnmanagedType.I1)] public bool DSP_R_INBD_Sw;     // 748 only
        [MarshalAs(UnmanagedType.I1)] public bool DSP_LWR_CTR_Sw;        // 748 only	
        [MarshalAs(UnmanagedType.I1)] public bool DSP_ENG_Sw;
        [MarshalAs(UnmanagedType.I1)] public bool DSP_STAT_Sw;
        [MarshalAs(UnmanagedType.I1)] public bool DSP_ELEC_Sw;
        [MarshalAs(UnmanagedType.I1)] public bool DSP_HYD_Sw;
        [MarshalAs(UnmanagedType.I1)] public bool DSP_FUEL_Sw;
        [MarshalAs(UnmanagedType.I1)] public bool DSP_ECS_Sw;
        [MarshalAs(UnmanagedType.I1)] public bool DSP_DRS_Sw;
        [MarshalAs(UnmanagedType.I1)] public bool DSP_GEAR_Sw;
        [MarshalAs(UnmanagedType.I1)] public bool DSP_FCTL_Sw;       // 748 only
        [MarshalAs(UnmanagedType.I1)] public bool DSP_INFO_Sw;       // 748 only
        [MarshalAs(UnmanagedType.I1)] public bool DSP_CHKL_Sw;       // 748 only	
        [MarshalAs(UnmanagedType.I1)] public bool DSP_NAV_Sw;            // 748 only
        [MarshalAs(UnmanagedType.I1)] public bool DSP_CANC_Sw;       // CANC for 744, CANC/RCL for 748
        [MarshalAs(UnmanagedType.I1)] public bool DSP_RCL_Sw;            // 744 only
        [MarshalAs(UnmanagedType.I1)] public bool DSP_annunL_INBD;   // 748 only
        [MarshalAs(UnmanagedType.I1)] public bool DSP_annunR_INBD;   // 748 only
        [MarshalAs(UnmanagedType.I1)] public bool DSP_annunLWR_CTR;  // 748 only

        // MCP - Variables
        public float MCP_IASMach;                      // Mach if < 10.0
        [MarshalAs(UnmanagedType.I1)] public bool MCP_IASBlank;
        public ushort MCP_Heading;
        public ushort MCP_Altitude;
        public short MCP_VertSpeed;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_VertSpeedBlank;

        // MCP - Switches
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] MCP_FD_Sw_On;                   // left / right
        [MarshalAs(UnmanagedType.I1)] public bool MCP_ATArm_Sw_On;
        public byte MCP_BankLimitSel;                 // 0: AUTO  1: 5  2: 10 ... 5: 25
        [MarshalAs(UnmanagedType.I1)] public bool MCP_DisengageBar;
        public byte MCP_Speed_Dial;                       // 0 ... 99
        public byte MCP_Heading_Dial;                 // 0 ... 99
        public byte MCP_Altitude_Dial;                    // 0 ... 99
        public byte MCP_VS_Wheel;                     // 0 ... 99

        // MCP - MOMENTARY action switches
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 3)] public bool[] MCP_AP_Sw_Pushed;               // left / center / right
        [MarshalAs(UnmanagedType.I1)] public bool MCP_THR_Sw_Pushed;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_SPD_Sw_Pushed;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_LNAV_Sw_Pushed;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_VNAV_Sw_Pushed;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_FLCH_Sw_Pushed;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_HDG_HOLD_Sw_Pushed;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_VS_Sw_Pushed;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_ALT_HOLD_Sw_Pushed;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_LOC_Sw_Pushed;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_APP_Sw_Pushed;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_Speeed_Sw_Pushed;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_Heading_Sw_Pushed;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_Altitude_Sw_Pushed;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_IAS_MACH_Toggle_Sw_Pushed;

        // MCP - Annunciator lights
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 3)] public bool[] MCP_annunAP;                        // left / center / right
        [MarshalAs(UnmanagedType.I1)] public bool MCP_annunTHR;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_annunSPD;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_annunLNAV;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_annunVNAV;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_annunFLCH;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_annunHDG_HOLD;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_annunVS;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_annunALT_HOLD;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_annunLOC;
        [MarshalAs(UnmanagedType.I1)] public bool MCP_annunAPP;

        // Left Glareshield
        public byte DSP_InbdDspl_L_Selector;          // 744: 0=EICAS, 1=NORM,2=PFD  748: 0=ND,1=NAV,2=MFD,3=EICAS
        public byte DSP_LwrDspl_L_Selector;               // 744 only 0=EICAS PRI, 1=NORM, 2=ND

        // Right Glareshield 
        public byte DSP_InbdDspl_R_Selector;          // 744: 0=PFD, 1=NORM,2=EICAS  748: 0=EICAS,1=MFD,2=NAV,3=PFD
        public byte DSP_LwrDspl_R_Selector;               // 744 only 0=ND PRI, 1=NORM, 2=EICAS


        // Forward panel
        //------------------------------------------

        // Forward Center 
        public byte ISP_FMC_Selector;                 // 0: LEFT   1: AUTO  2: RIGHT
        public byte ISP_EIU_C_Selector;                   // 0: L  1: AUTO  1: C  2: R
        public byte LTS_UpperDsplBRIGHTNESSKnob;      // Position 0...150
        public byte LTS_LowerDsplBRIGHTNESSKnob;      // Position 0...150
        [MarshalAs(UnmanagedType.I1)] public bool EICAS_EventRcd_Sw_Pushed;          // MOMENTARY action	
        [MarshalAs(UnmanagedType.I1)] public bool EFIS_HdgRef_Sw_Norm;
        [MarshalAs(UnmanagedType.I1)] public bool FCTL_AltnFlaps_Sw_ARM;
        public byte FCTL_AltnFlaps_Control_Sw;            // 0: RET  1: OFF  2: EXT
        public byte GEAR_Lever;                           // 0: UP  1: OFF  2: DOWN
        [MarshalAs(UnmanagedType.I1)] public bool GEAR_LockOvrd_Sw;                  // MOMENTARY SWITCH (resets when gear lever moved)
        [MarshalAs(UnmanagedType.I1)] public bool GEAR_AltnGearNoseBody_Sw_DPushed;
        [MarshalAs(UnmanagedType.I1)] public bool GEAR_AltnGearWing_Sw_DPushed;
        [MarshalAs(UnmanagedType.I1)] public bool GPWS_GSInhibit_Sw;
        [MarshalAs(UnmanagedType.I1)] public bool GPWS_annunGND_PROX_top;
        [MarshalAs(UnmanagedType.I1)] public bool GPWS_annunGND_PROX_bottom;
        [MarshalAs(UnmanagedType.I1)] public bool GPWS_FlapInhibitSw_OVRD;
        [MarshalAs(UnmanagedType.I1)] public bool GPWS_GearInhibitSw_OVRD;
        [MarshalAs(UnmanagedType.I1)] public bool GPWS_TerrInhibitSw_OVRD;

        // Standby - ISFD  (all are MOMENTARY action switches)
        [MarshalAs(UnmanagedType.I1)] public bool ISFD_Baro_Sw_Pushed;
        [MarshalAs(UnmanagedType.I1)] public bool ISFD_RST_Sw_Pushed;
        [MarshalAs(UnmanagedType.I1)] public bool ISFD_Minus_Sw_Pushed;
        [MarshalAs(UnmanagedType.I1)] public bool ISFD_Plus_Sw_Pushed;
        [MarshalAs(UnmanagedType.I1)] public bool ISFD_APP_Sw_Pushed;
        [MarshalAs(UnmanagedType.I1)] public bool ISFD_HP_IN_Sw_Pushed;

        // Forward Left 
        public byte ISP_FltDir_L_Selector;                // 0: L  1: C  2: R
        public byte ISP_Nav_L_Selector;                   // 0: FMC L  1: FMC R  2: CDU L  3: CDU C
        public byte ISP_EIU_L_Selector;                   // 0: L  1: AUTO  1: C  2: R
        public byte ISP_IRS_L_Selector;                   // 0: L  1: C  2: R
        public byte ISP_AirData_L_Selector;               // 0: L  1: C  2: R
        public int BRAKES_BrakePressNeedle;            // Value 0...100 (corresponds to 0...4000 PSI)
        [MarshalAs(UnmanagedType.I1)] public bool BRAKES_annunBRAKE_SOURCE;

        // Forward Right 
        public byte ISP_FltDir_R_Selector;                // 0: R  1: C  2: L
        public byte ISP_Nav_R_Selector;                   // 0: FMC R  1: FMC L  2: CDU R  3: CDU C
        public byte ISP_EIU_R_Selector;                   // 0: R  1: AUTO  1: C  2: L
        public byte ISP_IRS_R_Selector;                   // 0: R  1: C  2: L
        public byte ISP_AirData_R_Selector;               // 0: R  1: C  2: L

        // Left Sidewall
        public byte LTS_LeftFwdPanelPNLKnob;          // Position 0...150
        public byte LTS_LeftFwdPanelFLOODKnob;            // Position 0...150
        public byte LTS_LeftOutbdDsplBRIGHTNESSKnob;  // Position 0...150
        public byte LTS_LeftInbdDsplBRIGHTNESSKnob;       // Position 0...150

        // Right Sidewall
        public byte LTS_RightFwdPanelPNLKnob;         // Position 0...150
        public byte LTS_RightFwdPanelFLOODKnob;           // Position 0...150	
        public byte LTS_RightInbdDsplBRIGHTNESSKnob;  // Position 0...150
        public byte LTS_RightOutbdDsplBRIGHTNESSKnob; // Position 0...150

        // Left & Right Sidewalls
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)] public byte[] AIR_ShoulderHeaterKnob;            // 0:HI  1:LO  2:OFF
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)] public byte[] AIR_FootHeaterSelector;            // 0:HI  1:LO  2:OFF
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)] public byte[] AIR_WShldAirSelector;          // 0:ON  1:OFF

        // Chronometers (0:Left / 1:Right)
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] CHR_Chr_Sw_Pushed;              // MOMENTARY SWITCH
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)] public bool[] CHR_Date_Sw_Pushed;             // MOMENTARY SWITCH
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)] public byte[] CHR_Set_Selector;              // 0: RUN  1: HLDY  2: MM  3: HD
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)] public byte[] CHR_ET_Selector;                   // 0: RESET (MOMENTARY spring-loaded to HLD)  1: HLD  2: RUN


        // Control Stand
        //------------------------------------------

        [MarshalAs(UnmanagedType.I1)] public bool FCTL_StabCutOutSw_2_NORMAL;
        [MarshalAs(UnmanagedType.I1)] public bool FCTL_StabCutOutSw_3_NORMAL;
        public byte FCTL_AltnPitch_Switches;              // 0: NOSE DOWN  1: NEUTRAL  2: NOSE UP
        public byte FCTL_Speedbrake_Lever;                // Position 0...100  0: DOWN,  25: ARMED, 26...100: DEPLOYED 
        public byte FCTL_Flaps_Lever;                 // 0: UP  1: 1  2: 5  3: 10  4: 20  5: 25  6: 30 	
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] ENG_FuelControl_Sw_RUN;
        [MarshalAs(UnmanagedType.I1)] public bool BRAKES_ParkingBrakeLeverOn;


        // Forward Aisle Stand Panel
        //------------------------------------------

        // CDU (Left/Right/Center)
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 3)] public bool[] CDU_annunEXEC;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 3)] public bool[] CDU_annunDSPY;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 3)] public bool[] CDU_annunFAIL;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 3)] public bool[] CDU_annunMSG;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 3)] public bool[] CDU_annunOFST;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)] public byte[] CDU_BrtKnob;                       // 0: DecreasePosition 1: Neutral  2: Increase



        // Aft Aisle Stand Panel
        //------------------------------------------

        // Audio Control Panels								// Comm Systems: 0=VHFL 1=VHFC 2=VHFR 3=FLT 4=CAB 5=PA 6=HFL 7=HFR 8=SAT1 9=SAT2 10=SPKR 11=VOR/ADF 12=APP
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)] public byte[] COMM_SelectedMic;              // array: 0=capt, 1=F/O, 2=observer  values: 0..9 (VHF..SAT2) as listed above; -1 if no MIC is selected
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)] public byte[] COMM_ReceiverSwitches;         // array: 0=capt, 1=F/O, 2=observer  values: Bit flags for selector receivers 0...12 (VHFL..APP) as listed above

        // Radio Control Panels								// arrays: 0=capt, 1=F/O, 2=observer
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)] public byte[] COMM_SelectedRadio;                // 0: VHFL  1: VHFC  2: VHFL  3: HFL  5: HFR (4 not used)
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 3)] public bool[] COMM_RadioTransfer_Sw_Pushed;   // MOMENTARY action
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 3)] public bool[] COMM_RadioPanelOff;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 3)] public bool[] COMM_annunAM;

        // TCAS Panel
        [MarshalAs(UnmanagedType.I1)] public bool XPDR_XpndrSelector_R;              // true: R     false: L
        public byte XPDR_ModeSel;                     // 0: STBY  1: ALT RPTG OFF  2: XPNDR  3: TA ONLY  4: TA/RA
        [MarshalAs(UnmanagedType.I1)] public bool XPDR_Ident_Sw_Pushed;              // MOMENTARY action
        public byte BRAKES_AutobrakeSelector;         // 0: RTO  1: OFF  2: DISARM   3: "1" ... 5: MAX AUTO

        // Aileron & Rudder Trim
        public byte FCTL_AileronTrim_Switches;            // 0: LEFT WING DOWN  1: NEUTRAL  2: RIGHT WING DOWN (both switches move together)
        public byte FCTL_RudderTrim_Knob;             // 0: NOSE LEFT  1: NEUTRAL  2: NOSE RIGHT

        // Cabin Signs
        public byte SIGNS_NoSmokingSelector;          // 0: OFF  1: AUTO   2: ON
        public byte SIGNS_SeatBeltsSelector;          // 0: OFF  1: AUTO   2: ON

        // Evacuation Panel
        [MarshalAs(UnmanagedType.I1)] public bool EVAC_Command_Sw_ON;
        [MarshalAs(UnmanagedType.I1)] public bool EVAC_PressToTest_Sw_Pressed;
        [MarshalAs(UnmanagedType.I1)] public bool EVAC_HornSutOff_Sw_Pulled;
        [MarshalAs(UnmanagedType.I1)] public bool EVAC_LightIlluminated;

        // Additional variables
        //------------------------------------------

        // Door state
        // Possible values are, 0: open, 1: closed, 2: closed and armed, 3: closing, 4: opening.
        // The array contains these doors:
        //  0: Entry 1L,
        //  1: Entry 1R,
        //  2: Entry 2L,
        //  3: Entry 2R,
        //  4: Entry 3L,				
        //  5: Entry 3R,		
        //  6: Entry 4L,				
        //  7: Entry 4R,
        //  8: Entry 5L,
        //  9: Entry 5R,
        // 10: Upper Deck L
        // 11: Upper Deck R
        // 12: Cargo Fwd,
        // 13: Cargo Aft,
        // 14: Cargo Main,				
        // 15: Cargo Side,
        // 16: Cargo Nose,
        // 17: Main Elec,
        // 18: Ctr Elec
        // 19: F/D Overhead
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)] public byte[] DOOR_state;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 4)] public bool[] ENG_StartValve;                 // true: valve open
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)] public float[] AIR_DuctPress;                 // PSI Left, Right, Center (Center applies only to 747-8)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)] public float[] FUEL_TankQty;                  // LBS, 0: MAIN1  1: MAIN2  2: MAIN3  3: MAIN4  4: L WING  5: R WING  6: CWT  7: STAB  8: CTR RES
        [MarshalAs(UnmanagedType.I1)] public bool IRS_aligned;                       // at least one IRU is aligned
        public byte AircraftModel;                        // 1: -400  2: -400BCF  3: -400M  4: -400D  5: -400ER  6: -400F  7: -400ERF  8: -8I  9: -8F
        [MarshalAs(UnmanagedType.I1)] public bool WeightInKg;                            // false: LBS  true: KG
        [MarshalAs(UnmanagedType.I1)] public bool GPWS_V1CallEnabled;                    // GPWS V1 call-out option enabled
        [MarshalAs(UnmanagedType.I1)] public bool GroundConnAvailable;               // can connect/disconnect ground air/electrics

        public byte FMC_TakeoffFlaps;                 // degrees, 0 if not set
        public byte FMC_V1;                               // knots, 0 if not set
        public byte FMC_VR;                               // knots, 0 if not set
        public byte FMC_V2;                               // knots, 0 if not set
        public byte FMC_LandingFlaps;                 // degrees, 0 if not set
        public byte FMC_LandingVREF;                  // knots, 0 if not set
        public ushort FMC_CruiseAlt;                       // ft, 0 if not set
        public short FMC_LandingAltitude;              // ft; -32767 if not available
        public ushort FMC_TransitionAlt;                   // ft
        public ushort FMC_TransitionLevel;             // ft
        [MarshalAs(UnmanagedType.I1)] public bool FMC_PerfInputComplete;
        public float FMC_DistanceToTOD;                    // nm; 0.0 if passed, negative if n/a
        public float FMC_DistanceToDest;                   // nm; negative if n/a
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)] public string FMC_flightNumber;

        // More additional variables
        //------------------------------------------
        [MarshalAs(UnmanagedType.I1)] public bool ELEC_annunBatteryOFF;              // OFF light in the elec battery switch
        [MarshalAs(UnmanagedType.I1)] public bool FIRE_annunCargoDEPRESS;                // Freighter only
        [MarshalAs(UnmanagedType.I1)] public bool MCP_panelPowered;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 3)] public bool[] COMM_RadioPanelPowered;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 3)] public bool[] COMM_AudioControlPanelPowered;
        [MarshalAs(UnmanagedType.I1)] public bool TCAS_ATC_panelPowered;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 5)] public bool[] FIRE_HandleIllumination;            // [0..3]: Engines 1-4, [4]: APU
        [MarshalAs(UnmanagedType.I1)] public bool WheelChocksSet;


        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 160)] public byte[] reserved;
    };


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
    public struct PMDG_747QOTSII_CDU_Screen : PMDG_CDU_SDK.ICDU_Screen
    {
        public PMDG_CDU_SDK.CDU_Screen CDU_Screen { get; }

        [MarshalAs(UnmanagedType.I1)] public bool MenuVirtualPower;
        [MarshalAs(UnmanagedType.I1)] public bool Disabled;
        [MarshalAs(UnmanagedType.I1)] public bool isMonochrome;                     // true for the 747-400 with monochrome CDU
    }


    // 747QOTSII CDU Screen Cell Colors
    public const int CDU_COLOR_TEXT = 0;			// Normal text color: green on monochrome CDU, white on LCD CDU
    public const int CDU_COLOR_CYAN = 1;
    public const int CDU_COLOR_GREEN = 2;
    public const int CDU_COLOR_MAGENTA = 3;
    public const int CDU_COLOR_AMBER = 4;
    public const int CDU_COLOR_RED = 5;

    // 747QOTSII CDU Screen Cell flags
    public const int CDU_FLAG_SMALL_FONT = 0x01;        // small font, including that used for line headers 
    public const int CDU_FLAG_REVERSE = 0x02;      // character background is highlighted in reverse video
    public const int CDU_FLAG_UNUSED = 0x04;		// dimmed character color indicating inop/unused entries

    // 747QOTSII EFB Screen Dimensions
    public const int EFB_SCREEN_WIDTH = 512;
    public const int EFB_SCREEN_HEIGHT = 645;
    public const int EFB_SCREEN_BUFF_SIZE = EFB_SCREEN_WIDTH * EFB_SCREEN_HEIGHT * 2;

    // 747QOTSII EFB Screen Mapping 
    public const string EFB_L_SCREEN_CONTENTS = "Global\\PMDG_EFB_L_CONTENTS";
    public const string EFB_R_SCREEN_CONTENTS = "Global\\PMDG_EFB_R_CONTENTS";
    public const string EFB_L_OUTPUTCHANGED_EVENT = "Global\\PMDG_EFB_L_OUTPUTCHANGED_EVENT";
    public const string EFB_R_OUTPUTCHANGED_EVENT = "Global\\PMDG_EFB_R_OUTPUTCHANGED_EVENT";

    // 747QOTSII Control Structure
    public struct PMDG_747QOTSII_Control
    {
        public uint Event;
        public uint Parameter;
    };

    // Mouse flags for mouse click simulation

    public const uint MOUSE_FLAG_RIGHTSINGLE = 0x80000000u;
    public const uint MOUSE_FLAG_MIDDLESINGLE = 0x40000000u;
    public const uint MOUSE_FLAG_LEFTSINGLE = 0x20000000u;
    public const uint MOUSE_FLAG_RIGHTDOUBLE = 0x10000000u;
    public const uint MOUSE_FLAG_MIDDLEDOUBLE = 0x08000000u;
    public const uint MOUSE_FLAG_LEFTDOUBLE = 0x04000000u;
    public const uint MOUSE_FLAG_RIGHTDRAG = 0x02000000u;
    public const uint MOUSE_FLAG_MIDDLEDRAG = 0x01000000u;
    public const uint MOUSE_FLAG_LEFTDRAG = 0x00800000u;
    public const uint MOUSE_FLAG_MOVE = 0x00400000u;
    public const uint MOUSE_FLAG_DOWN_REPEAT = 0x00200000u;
    public const uint MOUSE_FLAG_RIGHTRELEASE = 0x00080000u;
    public const uint MOUSE_FLAG_MIDDLERELEASE = 0x00040000u;
    public const uint MOUSE_FLAG_LEFTRELEASE = 0x00020000u;
    public const uint MOUSE_FLAG_WHEEL_FLIP = 0x00010000u; // invert direction of mouse wheel
    public const uint MOUSE_FLAG_WHEEL_SKIP = 0x00008000u; // look at next 2 rect for mouse wheel commands
    public const uint MOUSE_FLAG_WHEEL_UP = 0x00004000u;
    public const uint MOUSE_FLAG_WHEEL_DOWN = 0x00002000u;


    // Audio control panel selected receiver flags.
    // The COMM_ReceiverSwitches[3] variables may contain any combination of these flags.
    public const int ACP_SEL_RECV_VHFL = 0x0001;
    public const int ACP_SEL_RECV_VHFC = 0x0002;
    public const int ACP_SEL_RECV_VHFR = 0x0004;
    public const int ACP_SEL_RECV_FLT = 0x0008;
    public const int ACP_SEL_RECV_CAB = 0x0010;
    public const int ACP_SEL_RECV_PA = 0x0020;
    public const int ACP_SEL_RECV_HFL = 0x0040;
    public const int ACP_SEL_RECV_HFR = 0x0080;
    public const int ACP_SEL_RECV_SAT1 = 0x0100;
    public const int ACP_SEL_RECV_SAT2 = 0x0200;
    public const int ACP_SEL_RECV_SPKR = 0x0400;
    public const int ACP_SEL_RECV_VOR_ADF = 0x0800;
    public const int ACP_SEL_RECV_APP = 0x1000;



    //////////////////////////////////////////////////////////////////
    //
    //  747QOTSII EVENTS 
    //
    //////////////////////////////////////////////////////////////////

    public const int THIRD_PARTY_EVENT_ID_MIN = 0x00011000;  // equals to 69632
    public const int EVT_CDU_L_START = THIRD_PARTY_EVENT_ID_MIN + 810;
    public const int EVT_CDU_R_START = THIRD_PARTY_EVENT_ID_MIN + 880;
    public const int EVT_CDU_C_START = THIRD_PARTY_EVENT_ID_MIN + 1140;
    public const int EVT_ACP_CAPT_FIRST = THIRD_PARTY_EVENT_ID_MIN + 1020;
    public const int EVT_ACP_FO_FIRST = THIRD_PARTY_EVENT_ID_MIN + 1260;
    public const int EVT_ACP_OBS_FIRST = THIRD_PARTY_EVENT_ID_MIN + 1070;
    public const int EVT_COM1_START_RANGE = THIRD_PARTY_EVENT_ID_MIN + 1000;
    public const int EVT_COM2_START_RANGE = THIRD_PARTY_EVENT_ID_MIN + 1240;
    public const int EVT_COM3_START_RANGE = THIRD_PARTY_EVENT_ID_MIN + 1050;
    public const int EVT_COM1_KEY_PAD_START_RANGE = THIRD_PARTY_EVENT_ID_MIN + 1426;
    public const int EVT_COM2_KEY_PAD_START_RANGE = THIRD_PARTY_EVENT_ID_MIN + 1466;
    public const int EVT_COM3_KEY_PAD_START_RANGE = THIRD_PARTY_EVENT_ID_MIN + 1446;
    public const int EVT_PED_CALL_PANEL_FIRST_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 1220;
    public const int EVT_PED_CALL_PANEL_FIRST_KEY = THIRD_PARTY_EVENT_ID_MIN + 1227;
    public const int EVT_EFB_L_START = THIRD_PARTY_EVENT_ID_MIN + 1700;
    public const int EVT_EFB_L_KEY_START = EVT_EFB_L_START + 30;
    public const int EVT_EFB_R_START = EVT_EFB_L_KEY_START + 55;
    public const int EVT_EFB_R_KEY_START = EVT_EFB_R_START + 30;
    public const int EVT_SC_SENSORS_START = THIRD_PARTY_EVENT_ID_MIN + 14650;
    public const int EVT_SC_AXIS_START = THIRD_PARTY_EVENT_ID_MIN + 14680;

    public enum PMDGEvents
    {
        // Overhead IRS
        EVT_OH_IRU_SELECTOR_L = THIRD_PARTY_EVENT_ID_MIN + 5,
        EVT_OH_IRU_SELECTOR_C = THIRD_PARTY_EVENT_ID_MIN + 6,
        EVT_OH_IRU_SELECTOR_R = THIRD_PARTY_EVENT_ID_MIN + 7,

        // Overhead - Electric  
        EVT_OH_ELEC_STBY_PWR_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 10,
        EVT_OH_ELEC_L_UTIL = THIRD_PARTY_EVENT_ID_MIN + 11,
        EVT_OH_ELEC_R_UTIL = THIRD_PARTY_EVENT_ID_MIN + 12,
        EVT_OH_ELEC_APU_SEL_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 13,
        EVT_OH_ELEC_EXT_PWR1_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 14,
        EVT_OH_ELEC_EXT_PWR2_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 15,
        EVT_OH_ELEC_APU_GEN1_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 16,
        EVT_OH_ELEC_APU_GEN2_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 17,
        EVT_OH_ELEC_BATTERY_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 18,
        EVT_OH_ELEC_BATTERY_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10018,
        EVT_OH_ELEC_BUS_TIE1_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 19,
        EVT_OH_ELEC_BUS_TIE2_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 20,
        EVT_OH_ELEC_BUS_TIE3_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 21,
        EVT_OH_ELEC_BUS_TIE4_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 22,
        EVT_OH_ELEC_GEN1_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 23,
        EVT_OH_ELEC_GEN2_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 24,
        EVT_OH_ELEC_GEN3_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 25,
        EVT_OH_ELEC_GEN4_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 26,
        EVT_OH_ELEC_DISCONNECT1_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 27,
        EVT_OH_ELEC_DISCONNECT1_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10027,
        EVT_OH_ELEC_DISCONNECT2_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 28,
        EVT_OH_ELEC_DISCONNECT2_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10028,
        EVT_OH_ELEC_DISCONNECT3_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 29,
        EVT_OH_ELEC_DISCONNECT3_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10029,
        EVT_OH_ELEC_DISCONNECT4_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 30,
        EVT_OH_ELEC_DISCONNECT4_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10030,

        // Overhead - Hydraulic
        EVT_OH_HYD_ENG1 = THIRD_PARTY_EVENT_ID_MIN + 52,
        EVT_OH_HYD_ENG2 = THIRD_PARTY_EVENT_ID_MIN + 53,
        EVT_OH_HYD_ENG3 = THIRD_PARTY_EVENT_ID_MIN + 54,
        EVT_OH_HYD_ENG4 = THIRD_PARTY_EVENT_ID_MIN + 55,
        EVT_OH_HYD_DEMAND1 = THIRD_PARTY_EVENT_ID_MIN + 48,
        EVT_OH_HYD_DEMAND2 = THIRD_PARTY_EVENT_ID_MIN + 49,
        EVT_OH_HYD_DEMAND3 = THIRD_PARTY_EVENT_ID_MIN + 50,
        EVT_OH_HYD_DEMAND4 = THIRD_PARTY_EVENT_ID_MIN + 51,
        EVT_OH_HYD_RAM_AIR = THIRD_PARTY_EVENT_ID_MIN + 153,  // Applies to 747-8 
        EVT_OH_HYD_RAM_AIR_COVER = THIRD_PARTY_EVENT_ID_MIN + 10153,  // Applies to 747-8

        // Overhead - Fire Protection 
        EVT_OH_FIRE_HANDLE_ENGINE_1_BOTTOM = THIRD_PARTY_EVENT_ID_MIN + 71,
        EVT_OH_FIRE_HANDLE_ENGINE_2_BOTTOM = THIRD_PARTY_EVENT_ID_MIN + 72,
        EVT_OH_FIRE_HANDLE_ENGINE_3_BOTTOM = THIRD_PARTY_EVENT_ID_MIN + 73,
        EVT_OH_FIRE_HANDLE_ENGINE_4_BOTTOM = THIRD_PARTY_EVENT_ID_MIN + 74,
        EVT_OH_FIRE_HANDLE_APU_BOTTOM = THIRD_PARTY_EVENT_ID_MIN + 75,

        EVT_OH_FIRE_HANDLE_ENGINE_1_TOP = THIRD_PARTY_EVENT_ID_MIN + 85,
        EVT_OH_FIRE_HANDLE_ENGINE_2_TOP = THIRD_PARTY_EVENT_ID_MIN + 86,
        EVT_OH_FIRE_HANDLE_ENGINE_3_TOP = THIRD_PARTY_EVENT_ID_MIN + 87,
        EVT_OH_FIRE_HANDLE_ENGINE_4_TOP = THIRD_PARTY_EVENT_ID_MIN + 88,
        EVT_OH_FIRE_HANDLE_APU_TOP = THIRD_PARTY_EVENT_ID_MIN + 89,

        EVT_OH_FIRE_UNLOCK_SWITCH_ENGINE_1 = THIRD_PARTY_EVENT_ID_MIN + 10071,
        EVT_OH_FIRE_UNLOCK_SWITCH_ENGINE_2 = THIRD_PARTY_EVENT_ID_MIN + 10072,
        EVT_OH_FIRE_UNLOCK_SWITCH_ENGINE_3 = THIRD_PARTY_EVENT_ID_MIN + 10073,
        EVT_OH_FIRE_UNLOCK_SWITCH_ENGINE_4 = THIRD_PARTY_EVENT_ID_MIN + 10074,
        EVT_OH_FIRE_UNLOCK_SWITCH_APU = THIRD_PARTY_EVENT_ID_MIN + 10075,

        EVT_OH_FIRE_OVHT_TEST = THIRD_PARTY_EVENT_ID_MIN + 60,
        EVT_OH_FIRE_OVHT_TEST_ENG = THIRD_PARTY_EVENT_ID_MIN + 67,
        EVT_OH_FIRE_CARGO_ARM_MAIN_DECK = THIRD_PARTY_EVENT_ID_MIN + 81,  // Freighter only
        EVT_OH_FIRE_CARGO_ARM_FWD = THIRD_PARTY_EVENT_ID_MIN + 82,
        EVT_OH_FIRE_CARGO_ARM_AFT = THIRD_PARTY_EVENT_ID_MIN + 83,
        EVT_OH_FIRE_CARGO_DISCH = THIRD_PARTY_EVENT_ID_MIN + 84,
        EVT_OH_FIRE_CARGO_DISCH_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10084,

        // Overhead - Engine control
        EVT_OH_EEC_1_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 1,
        EVT_OH_EEC_1_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10001,
        EVT_OH_EEC_2_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 2,
        EVT_OH_EEC_2_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10002,
        EVT_OH_EEC_3_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 3,
        EVT_OH_EEC_3_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10003,
        EVT_OH_EEC_4_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 4,
        EVT_OH_EEC_4_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10004,
        EVT_OH_ENGINE_1_START = THIRD_PARTY_EVENT_ID_MIN + 91,
        EVT_OH_ENGINE_2_START = THIRD_PARTY_EVENT_ID_MIN + 92,
        EVT_OH_ENGINE_3_START = THIRD_PARTY_EVENT_ID_MIN + 93,
        EVT_OH_ENGINE_4_START = THIRD_PARTY_EVENT_ID_MIN + 94,
        EVT_OH_IGNITION_STBY = THIRD_PARTY_EVENT_ID_MIN + 95,
        EVT_OH_IGNITION_CON = THIRD_PARTY_EVENT_ID_MIN + 96,
        EVT_OH_IGNITION_AUTO = THIRD_PARTY_EVENT_ID_MIN + 97,
        EVT_OH_ENGINE_AUTOSTART = THIRD_PARTY_EVENT_ID_MIN + 98,      // Single autostart switch (GE/RR)
        EVT_OH_ENGINE_AUTOSTART_1 = THIRD_PARTY_EVENT_ID_MIN + 350,   // Individual autostart switches (PW)
        EVT_OH_ENGINE_AUTOSTART_1_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10350,
        EVT_OH_ENGINE_AUTOSTART_2 = THIRD_PARTY_EVENT_ID_MIN + 351,
        EVT_OH_ENGINE_AUTOSTART_2_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10351,
        EVT_OH_ENGINE_AUTOSTART_3 = THIRD_PARTY_EVENT_ID_MIN + 352,
        EVT_OH_ENGINE_AUTOSTART_3_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10352,
        EVT_OH_ENGINE_AUTOSTART_4 = THIRD_PARTY_EVENT_ID_MIN + 353,
        EVT_OH_ENGINE_AUTOSTART_4_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10353,


        // Overhead - FUEL Panel
        EVT_OH_FUEL_XFER_MAIN_1_4 = THIRD_PARTY_EVENT_ID_MIN + 66,
        EVT_OH_FUEL_XFER_MAIN_1_4_COVER = THIRD_PARTY_EVENT_ID_MIN + 10066,

        EVT_OH_FUEL_TO_REMAIN_KNOB = THIRD_PARTY_EVENT_ID_MIN + 101,
        EVT_OH_FUEL_JETTISON_SELECTOR = THIRD_PARTY_EVENT_ID_MIN + 102,
        EVT_OH_FUEL_JETTISON_NOZZLE_L = THIRD_PARTY_EVENT_ID_MIN + 103,
        EVT_OH_FUEL_JETTISON_NOZZLE_L_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10103,
        EVT_OH_FUEL_JETTISON_NOZZLE_R = THIRD_PARTY_EVENT_ID_MIN + 104,
        EVT_OH_FUEL_JETTISON_NOZZLE_R_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10104,

        EVT_OH_FUEL_XFEED_1 = THIRD_PARTY_EVENT_ID_MIN + 110,
        EVT_OH_FUEL_XFEED_2 = THIRD_PARTY_EVENT_ID_MIN + 111,
        EVT_OH_FUEL_XFEED_2_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10111,
        EVT_OH_FUEL_XFEED_3 = THIRD_PARTY_EVENT_ID_MIN + 112,
        EVT_OH_FUEL_XFEED_3_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10112,
        EVT_OH_FUEL_XFEED_4 = THIRD_PARTY_EVENT_ID_MIN + 113,

        EVT_OH_FUEL_PUMP_AUX_L = THIRD_PARTY_EVENT_ID_MIN + 108,
        EVT_OH_FUEL_PUMP_AUX_R = THIRD_PARTY_EVENT_ID_MIN + 109,

        EVT_OH_FUEL_PUMP_L_CENTER = THIRD_PARTY_EVENT_ID_MIN + 114,
        EVT_OH_FUEL_PUMP_R_CENTER = THIRD_PARTY_EVENT_ID_MIN + 115,

        EVT_OH_FUEL_PUMP_MAIN_FWD_1 = THIRD_PARTY_EVENT_ID_MIN + 116,
        EVT_OH_FUEL_PUMP_MAIN_FWD_2 = THIRD_PARTY_EVENT_ID_MIN + 117,
        EVT_OH_FUEL_PUMP_MAIN_FWD_3 = THIRD_PARTY_EVENT_ID_MIN + 118,
        EVT_OH_FUEL_PUMP_MAIN_FWD_4 = THIRD_PARTY_EVENT_ID_MIN + 119,
        EVT_OH_FUEL_PUMP_MAIN_AFT_1 = THIRD_PARTY_EVENT_ID_MIN + 122,
        EVT_OH_FUEL_PUMP_MAIN_AFT_2 = THIRD_PARTY_EVENT_ID_MIN + 123,
        EVT_OH_FUEL_PUMP_MAIN_AFT_3 = THIRD_PARTY_EVENT_ID_MIN + 124,
        EVT_OH_FUEL_PUMP_MAIN_AFT_4 = THIRD_PARTY_EVENT_ID_MIN + 125,

        EVT_OH_FUEL_PUMP_OVRD_FWD_2 = THIRD_PARTY_EVENT_ID_MIN + 120,
        EVT_OH_FUEL_PUMP_OVRD_FWD_3 = THIRD_PARTY_EVENT_ID_MIN + 121,
        EVT_OH_FUEL_PUMP_OVRD_AFT_2 = THIRD_PARTY_EVENT_ID_MIN + 126,
        EVT_OH_FUEL_PUMP_OVRD_AFT_3 = THIRD_PARTY_EVENT_ID_MIN + 127,

        EVT_OH_FUEL_PUMP_L_STAB = THIRD_PARTY_EVENT_ID_MIN + 128,
        EVT_OH_FUEL_PUMP_R_STAB = THIRD_PARTY_EVENT_ID_MIN + 129,

        EVT_OH_FUEL_RSV_1_4_XFR = THIRD_PARTY_EVENT_ID_MIN + 154,// 747-8 only
        EVT_OH_FUEL_RSV_1_4_XFR_COVER = THIRD_PARTY_EVENT_ID_MIN + 10154,// 747-8 only

        // Overhead - ANTI-ICE
        EVT_OH_ICE_ENGINE_ANTIICE_1 = THIRD_PARTY_EVENT_ID_MIN + 131,
        EVT_OH_ICE_ENGINE_ANTIICE_2 = THIRD_PARTY_EVENT_ID_MIN + 132,
        EVT_OH_ICE_ENGINE_ANTIICE_3 = THIRD_PARTY_EVENT_ID_MIN + 133,
        EVT_OH_ICE_ENGINE_ANTIICE_4 = THIRD_PARTY_EVENT_ID_MIN + 134,
        EVT_OH_ICE_WING_ANTIICE = THIRD_PARTY_EVENT_ID_MIN + 135,
        EVT_OH_ICE_WINDOW_HEAT_L = THIRD_PARTY_EVENT_ID_MIN + 141,
        EVT_OH_ICE_WINDOW_HEAT_R = THIRD_PARTY_EVENT_ID_MIN + 142,

        // Overhead - Rain Protection
        EVT_OH_WIPER_SWITCH_L = THIRD_PARTY_EVENT_ID_MIN + 136,
        EVT_OH_WIPER_SWITCH_R = THIRD_PARTY_EVENT_ID_MIN + 139,
        EVT_OH_WASHER_SWITCH_L = THIRD_PARTY_EVENT_ID_MIN + 137,
        EVT_OH_WASHER_SWITCH_R = THIRD_PARTY_EVENT_ID_MIN + 138,
        EVT_OH_RAIN_REP_SWITCH_L = THIRD_PARTY_EVENT_ID_MIN + 140,
        EVT_OH_RAIN_REP_SWITCH_R = THIRD_PARTY_EVENT_ID_MIN + 143,

        // Overhead - Miscellaneous
        EVT_OH_EMER_EXIT_LIGHT_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 61,
        EVT_OH_EMER_EXIT_LIGHT_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10061,
        EVT_CAPT_AUDIO_SELECTOR = THIRD_PARTY_EVENT_ID_MIN + 62,  // Freighter only
        EVT_OBS_AUDIO_SELECTOR = THIRD_PARTY_EVENT_ID_MIN + 63,
        EVT_OH_SERVICE_INTERPHONE_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 64,
        EVT_OH_CABIN_CARGO_INTERPHONE_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 65,
        EVT_OH_OXYGEN_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 150,
        EVT_OH_OXYGEN_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10150,
        EVT_OH_YAW_DAMPER_UPR = THIRD_PARTY_EVENT_ID_MIN + 151,
        EVT_OH_YAW_DAMPER_LWR = THIRD_PARTY_EVENT_ID_MIN + 152,
        EVT_OH_CENTER_AIR_DATA_SELECTOR = THIRD_PARTY_EVENT_ID_MIN + 155,// 748 only
        EVT_OH_SMOKE_EVAC_HANDLE = THIRD_PARTY_EVENT_ID_MIN + 321,// On CB panel


        // Overhead - Cabin Press
        EVT_OH_PRESS_LAND_ALT_KNOB_ROTATE = THIRD_PARTY_EVENT_ID_MIN + 160,
        EVT_OH_PRESS_LAND_ALT_PUSH_ON = THIRD_PARTY_EVENT_ID_MIN + 161,
        EVT_OH_PRESS_VALVE_MAN_L = THIRD_PARTY_EVENT_ID_MIN + 162,
        EVT_OH_PRESS_VALVE_MAN_R = THIRD_PARTY_EVENT_ID_MIN + 163,
        EVT_OH_PRESS_VALVE_CTRL = THIRD_PARTY_EVENT_ID_MIN + 164,
        EVT_OH_PRESS_AUTO_SELECT = THIRD_PARTY_EVENT_ID_MIN + 165,

        // Overhead Air Conditioning
        EVT_OH_AIRCOND_FLT_DECK_FAN_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 170,  // Freighter only	
        EVT_OH_AIRCOND_TEMP_SELECTOR_FLT_DECK = THIRD_PARTY_EVENT_ID_MIN + 171,
        EVT_OH_AIRCOND_TEMP_SELECTOR_MAIN_DECK_FWD = THIRD_PARTY_EVENT_ID_MIN + 172,  // Freighter only
        EVT_OH_AIRCOND_TEMP_SELECTOR_MAIN_DECK_AFT = THIRD_PARTY_EVENT_ID_MIN + 173,  // Freighter only
        EVT_OH_AIRCOND_ZONE_RST_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 174,  // 744 only
        EVT_OH_AIRCOND_TRIM_AIR_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 175,
        EVT_OH_AIRCOND_TRIM_AIR_SWITCH_R = THIRD_PARTY_EVENT_ID_MIN + 188,    // 748 only
        EVT_OH_AIRCOND_TEMP_SELECTOR_LWR_LOBE_FWD = THIRD_PARTY_EVENT_ID_MIN + 176,   // Freighter only
        EVT_OH_AIRCOND_TEMP_SELECTOR_LWR_LOBE_AFT = THIRD_PARTY_EVENT_ID_MIN + 177,   // Freighter only
        EVT_OH_AIRCOND_EQUIP_COOLING_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 178,
        EVT_OH_AIRCOND_HI_FLOW_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 179,
        EVT_OH_AIRCOND_PACK_RST_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 180,
        EVT_OH_AIRCOND_AFT_CARGO_HT_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 181,  // 744 only; push-button on main OVHD panel
        EVT_OH_AIRCOND_GASPER_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 182,    // Passenger 744 only
        EVT_OH_AIRCOND_RECIRC_FAN_UPP_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 183,    // Passenger only	
        EVT_OH_AIRCOND_RECIRC_FAN_LWR_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 184,    // Passenger only
        EVT_OH_AIRCOND_TEMP_SELECTOR_PASS = THIRD_PARTY_EVENT_ID_MIN + 185,   // Passenger only
        EVT_OH_AIRCOND_ALT_VENT_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 186,  // 747-8 only
        EVT_OH_AIRCOND_ALT_VENT_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10186, // 747-8 only
        EVT_OH_AIRCOND_ALT_VENT_SELECTOR = THIRD_PARTY_EVENT_ID_MIN + 187,    // 747-8 only
        EVT_OH_AIRCOND_AFT_CARGO_TEMP = THIRD_PARTY_EVENT_ID_MIN + 189,   // Passenger 747-8 only
        EVT_OH_AIRCOND_PACK_SWITCH_L = THIRD_PARTY_EVENT_ID_MIN + 190,
        EVT_OH_AIRCOND_PACK_SWITCH_R = THIRD_PARTY_EVENT_ID_MIN + 191,
        EVT_OH_AIRCOND_PACK_SWITCH_C = THIRD_PARTY_EVENT_ID_MIN + 192,

        // Overhead Bleed Air
        EVT_OH_BLEED_ISOLATION_VALVE_SWITCH_L = THIRD_PARTY_EVENT_ID_MIN + 193,
        EVT_OH_BLEED_ISOLATION_VALVE_SWITCH_R = THIRD_PARTY_EVENT_ID_MIN + 194,
        EVT_OH_BLEED_APU_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 199,
        EVT_OH_BLEED_ENG_1_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 201,
        EVT_OH_BLEED_ENG_2_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 202,
        EVT_OH_BLEED_ENG_3_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 203,
        EVT_OH_BLEED_ENG_4_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 204,

        // Overhead Lights Panel
        EVT_OH_LIGHTS_STORM = THIRD_PARTY_EVENT_ID_MIN + 210,
        EVT_OH_PANEL_LIGHT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 211,
        EVT_OH_GS_PANEL_LIGHT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 212,
        EVT_OH_GS_FLOOD_LIGHT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 213,
        EVT_OH_DOME_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 214,
        EVT_OH_AISLE_STAND_FLOOD_LIGHT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 220,
        EVT_OH_AISLE_STAND_PANEL_LIGHT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 221,
        EVT_OH_LIGHTS_LANDING_OUTBD_L = THIRD_PARTY_EVENT_ID_MIN + 222,
        EVT_OH_LIGHTS_LANDING_OUTBD_R = THIRD_PARTY_EVENT_ID_MIN + 223,
        EVT_OH_LIGHTS_LANDING_INBD_L = THIRD_PARTY_EVENT_ID_MIN + 224,
        EVT_OH_LIGHTS_LANDING_INBD_R = THIRD_PARTY_EVENT_ID_MIN + 225,
        EVT_OH_LIGHTS_L_TURNOFF = THIRD_PARTY_EVENT_ID_MIN + 226,
        EVT_OH_LIGHTS_R_TURNOFF = THIRD_PARTY_EVENT_ID_MIN + 227,
        EVT_OH_LIGHTS_TAXI = THIRD_PARTY_EVENT_ID_MIN + 228,
        EVT_OH_LIGHTS_BEACON = THIRD_PARTY_EVENT_ID_MIN + 230,
        EVT_OH_LIGHTS_NAV = THIRD_PARTY_EVENT_ID_MIN + 231,
        EVT_OH_LIGHTS_STROBE = THIRD_PARTY_EVENT_ID_MIN + 232,
        EVT_OH_LIGHTS_WING = THIRD_PARTY_EVENT_ID_MIN + 233,
        EVT_OH_LIGHTS_LOGO = THIRD_PARTY_EVENT_ID_MIN + 234,
        EVT_OH_LIGHTS_IND_LTS_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 235,

        // Overhead Maintenance - FLT Control HYD power
        EVT_OH_HYD_VLV_PWR_TAIL_1 = THIRD_PARTY_EVENT_ID_MIN + 250,
        EVT_OH_HYD_VLV_PWR_TAIL_1_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10250,
        EVT_OH_HYD_VLV_PWR_TAIL_2 = THIRD_PARTY_EVENT_ID_MIN + 251,
        EVT_OH_HYD_VLV_PWR_TAIL_2_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10251,
        EVT_OH_HYD_VLV_PWR_TAIL_3 = THIRD_PARTY_EVENT_ID_MIN + 252,
        EVT_OH_HYD_VLV_PWR_TAIL_3_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10252,
        EVT_OH_HYD_VLV_PWR_TAIL_4 = THIRD_PARTY_EVENT_ID_MIN + 253,
        EVT_OH_HYD_VLV_PWR_TAIL_4_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10253,
        EVT_OH_CVR_TEST = THIRD_PARTY_EVENT_ID_MIN + 291,
        EVT_OH_CVR_ERASE = THIRD_PARTY_EVENT_ID_MIN + 292,

        // Overhead Maintenance - FLT Control HYD Power
        EVT_OH_HYD_VLV_PWR_WING_1 = THIRD_PARTY_EVENT_ID_MIN + 258,
        EVT_OH_HYD_VLV_PWR_WING_1_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10258,
        EVT_OH_HYD_VLV_PWR_WING_2 = THIRD_PARTY_EVENT_ID_MIN + 259,
        EVT_OH_HYD_VLV_PWR_WING_2_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10259,
        EVT_OH_HYD_VLV_PWR_WING_3 = THIRD_PARTY_EVENT_ID_MIN + 260,
        EVT_OH_HYD_VLV_PWR_WING_3_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10260,
        EVT_OH_HYD_VLV_PWR_WING_4 = THIRD_PARTY_EVENT_ID_MIN + 261,
        EVT_OH_HYD_VLV_PWR_WING_4_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10261,

        // Overhead Maintenance - APU Start Source (744F & 747-8)
        EVT_OH_APU_START_SOURCE = THIRD_PARTY_EVENT_ID_MIN + 266,

        // Window heat & anti-fog power (747-8)
        EVT_OH_WINDHT_ANTIFOG_PWR_SIDE_L = THIRD_PARTY_EVENT_ID_MIN + 242,
        EVT_OH_WINDHT_ANTIFOG_PWR_SIDE_L_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10242,
        EVT_OH_WINDHT_ANTIFOG_PWR_FWD_L = THIRD_PARTY_EVENT_ID_MIN + 243,
        EVT_OH_WINDHT_ANTIFOG_PWR_FWD_L_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10243,
        EVT_OH_WINDHT_ANTIFOG_PWR_SIDE_R = THIRD_PARTY_EVENT_ID_MIN + 244,
        EVT_OH_WINDHT_ANTIFOG_PWR_SIDE_R_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10244,
        EVT_OH_WINDHT_ANTIFOG_PWR_FWD_R = THIRD_PARTY_EVENT_ID_MIN + 245,
        EVT_OH_WINDHT_ANTIFOG_PWR_FWD_R_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10245,

        // Overhead Maintenance - Electric
        EVT_OH_ELEC_GND_TESTS_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 241,
        EVT_OH_ELEC_GND_TESTS_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10241,
        EVT_OH_ELEC_GEN_FIELD1_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 270,
        EVT_OH_ELEC_GEN_FIELD1_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10270,
        EVT_OH_ELEC_GEN_FIELD2_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 271,
        EVT_OH_ELEC_GEN_FIELD2_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10271,
        EVT_OH_ELEC_GEN_FIELD3_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 272,
        EVT_OH_ELEC_GEN_FIELD3_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10272,
        EVT_OH_ELEC_GEN_FIELD4_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 273,
        EVT_OH_ELEC_GEN_FIELD4_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10273,
        EVT_OH_ELEC_APU_FIELD1_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 278,
        EVT_OH_ELEC_APU_FIELD1_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10278,
        EVT_OH_ELEC_APU_FIELD2_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 279,
        EVT_OH_ELEC_APU_FIELD2_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10279,
        EVT_OH_ELEC_SPLIT_BREAKER_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 280,
        EVT_OH_ELEC_SPLIT_BREAKER_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10280,
        EVT_OH_ELEC_TOWING_PWR = THIRD_PARTY_EVENT_ID_MIN + 287, // 747-8

        // Overhead Maintenance - EMU MAINT (747-8)
        EVT_OH_EMU_MAINT_PWR_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 288,
        EVT_OH_EMU_MAINT_PWR_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10288,

        // Overhead Maintenance - Squib test
        EVT_OH_FIRE_SQUIB_TEST1 = THIRD_PARTY_EVENT_ID_MIN + 298,
        EVT_OH_FIRE_SQUIB_TEST2 = THIRD_PARTY_EVENT_ID_MIN + 299,
        EVT_OH_FIRE_SQUIB_TEST_FWD = THIRD_PARTY_EVENT_ID_MIN + 306,  // Freighter only
        EVT_OH_FIRE_SQUIB_TEST_AFT = THIRD_PARTY_EVENT_ID_MIN + 307,  // Freighter only

        // Overhead Maintenance - EEC
        EVT_OH_EEC_TEST_1_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 309,
        EVT_OH_EEC_TEST_1_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10309,
        EVT_OH_EEC_TEST_2_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 310,
        EVT_OH_EEC_TEST_2_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10310,
        EVT_OH_EEC_TEST_3_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 311,
        EVT_OH_EEC_TEST_3_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10311,
        EVT_OH_EEC_TEST_4_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 312,
        EVT_OH_EEC_TEST_4_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10312,
        EVT_OH_EEC_CH_SEL_1_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 315,
        EVT_OH_EEC_CH_SEL_2_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 316,
        EVT_OH_EEC_CH_SEL_3_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 317,
        EVT_OH_EEC_CH_SEL_4_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 318,

        // Overhead Maintenance - Fuel
        EVT_OH_FUEL_CWT_SCAVENGE_PUMP = THIRD_PARTY_EVENT_ID_MIN + 313,
        EVT_OH_FUEL_CWT_SCAVENGE_PUMP_COVER = THIRD_PARTY_EVENT_ID_MIN + 10313,
        EVT_OH_FUEL_RSV_2_3_XFR = THIRD_PARTY_EVENT_ID_MIN + 314,
        EVT_OH_FUEL_RSV_2_3_XFR_COVER = THIRD_PARTY_EVENT_ID_MIN + 10314,

        // Overhead Maintenance - Miscellaneous
        EVT_OH_FLIGHT_DECK_ACCES_LIGHTS = THIRD_PARTY_EVENT_ID_MIN + 240,
        EVT_OH_LOWER_LOBE_FLOW_RATE = THIRD_PARTY_EVENT_ID_MIN + 284, // 744 Freighter only
        EVT_OH_AIRCOND_AFT_CARGO_HT_SELECTOR = THIRD_PARTY_EVENT_ID_MIN + 285,    // 748 only

        // Glareshield - MCP
        EVT_MCP_FD_SWITCH_L = THIRD_PARTY_EVENT_ID_MIN + 550,
        EVT_MCP_AT_ARM_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 551,
        EVT_MCP_THR_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 552,
        EVT_MCP_SPD_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 553,
        EVT_MCP_SPEED_SELECTOR = THIRD_PARTY_EVENT_ID_MIN + 554,
        EVT_MCP_SPEED_PUSH_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 10554,
        EVT_MCP_IAS_MACH_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 555,
        EVT_MCP_LNAV_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 559,
        EVT_MCP_VNAV_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 560,
        EVT_MCP_LVL_CHG_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 561,
        EVT_MCP_BANK_ANGLE_SELECTOR = THIRD_PARTY_EVENT_ID_MIN + 565,
        EVT_MCP_HEADING_PUSH_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 10566,
        EVT_MCP_HEADING_SELECTOR = THIRD_PARTY_EVENT_ID_MIN + 566,
        EVT_MCP_HDG_HOLD_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 568,
        EVT_MCP_VS_SELECTOR = THIRD_PARTY_EVENT_ID_MIN + 574,
        EVT_MCP_VS_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 575,
        EVT_MCP_ALTITUDE_SELECTOR = THIRD_PARTY_EVENT_ID_MIN + 581,
        EVT_MCP_ALTITUDE_PUSH_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 10581,
        EVT_MCP_ALT_HOLD_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 582,
        EVT_MCP_APP_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 583,
        EVT_MCP_LOC_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 584,
        EVT_MCP_AP_L_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 585,
        EVT_MCP_AP_C_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 586,
        EVT_MCP_AP_R_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 587,
        EVT_MCP_DISENGAGE_BAR = THIRD_PARTY_EVENT_ID_MIN + 588,
        EVT_MCP_FD_SWITCH_R = THIRD_PARTY_EVENT_ID_MIN + 589,
        EVT_MCP_TOGA_SCREW_L = THIRD_PARTY_EVENT_ID_MIN + 590,
        EVT_MCP_TOGA_SCREW_R = THIRD_PARTY_EVENT_ID_MIN + 591,

        // Glareshield - EFIS Captain control panel
        EVT_EFIS_CPT_MINIMUMS_RST = THIRD_PARTY_EVENT_ID_MIN + 520,
        //EVT_EFIS_CPT_FIRST = EVT_EFIS_CPT_MINIMUMS_RST,
        EVT_EFIS_CPT_MINIMUMS = THIRD_PARTY_EVENT_ID_MIN + 521,
        EVT_EFIS_CPT_MINIMUMS_RADIO_BARO = THIRD_PARTY_EVENT_ID_MIN + 522,
        EVT_EFIS_CPT_VOR_ADF_SELECTOR_L = THIRD_PARTY_EVENT_ID_MIN + 523,
        EVT_EFIS_CPT_MODE = THIRD_PARTY_EVENT_ID_MIN + 524,
        EVT_EFIS_CPT_MODE_CTR = THIRD_PARTY_EVENT_ID_MIN + 525,
        EVT_EFIS_CPT_RANGE = THIRD_PARTY_EVENT_ID_MIN + 526,
        EVT_EFIS_CPT_RANGE_TFC = THIRD_PARTY_EVENT_ID_MIN + 527,
        EVT_EFIS_CPT_VOR_ADF_SELECTOR_R = THIRD_PARTY_EVENT_ID_MIN + 528,
        EVT_EFIS_CPT_BARO_IN_HPA = THIRD_PARTY_EVENT_ID_MIN + 529,
        EVT_EFIS_CPT_BARO = THIRD_PARTY_EVENT_ID_MIN + 530,
        EVT_EFIS_CPT_BARO_STD = THIRD_PARTY_EVENT_ID_MIN + 531,
        EVT_EFIS_CPT_MTRS = THIRD_PARTY_EVENT_ID_MIN + 532,
        EVT_EFIS_CPT_FPV = THIRD_PARTY_EVENT_ID_MIN + 533,
        EVT_EFIS_CPT_WXR = THIRD_PARTY_EVENT_ID_MIN + 534,
        EVT_EFIS_CPT_STA = THIRD_PARTY_EVENT_ID_MIN + 535,
        EVT_EFIS_CPT_WPT = THIRD_PARTY_EVENT_ID_MIN + 536,
        EVT_EFIS_CPT_ARPT = THIRD_PARTY_EVENT_ID_MIN + 537,
        EVT_EFIS_CPT_DATA = THIRD_PARTY_EVENT_ID_MIN + 538,
        EVT_EFIS_CPT_POS = THIRD_PARTY_EVENT_ID_MIN + 539,
        EVT_EFIS_CPT_TERR = THIRD_PARTY_EVENT_ID_MIN + 540,
        //EVT_EFIS_CPT_LAST = EVT_EFIS_CPT_TERR,

        // Glareshield - EFIS F/O control panels
        EVT_EFIS_FO_MINIMUMS_RST = THIRD_PARTY_EVENT_ID_MIN + 620,
        //EVT_EFIS_FO_FIRST = EVT_EFIS_FO_MINIMUMS_RST,
        EVT_EFIS_FO_MINIMUMS = THIRD_PARTY_EVENT_ID_MIN + 621,
        EVT_EFIS_FO_MINIMUMS_RADIO_BARO = THIRD_PARTY_EVENT_ID_MIN + 622,
        EVT_EFIS_FO_VOR_ADF_SELECTOR_L = THIRD_PARTY_EVENT_ID_MIN + 623,
        EVT_EFIS_FO_MODE = THIRD_PARTY_EVENT_ID_MIN + 624,
        EVT_EFIS_FO_MODE_CTR = THIRD_PARTY_EVENT_ID_MIN + 625,
        EVT_EFIS_FO_RANGE = THIRD_PARTY_EVENT_ID_MIN + 626,
        EVT_EFIS_FO_RANGE_TFC = THIRD_PARTY_EVENT_ID_MIN + 627,
        EVT_EFIS_FO_VOR_ADF_SELECTOR_R = THIRD_PARTY_EVENT_ID_MIN + 628,
        EVT_EFIS_FO_BARO_IN_HPA = THIRD_PARTY_EVENT_ID_MIN + 629,
        EVT_EFIS_FO_BARO = THIRD_PARTY_EVENT_ID_MIN + 630,
        EVT_EFIS_FO_BARO_STD = THIRD_PARTY_EVENT_ID_MIN + 631,
        EVT_EFIS_FO_MTRS = THIRD_PARTY_EVENT_ID_MIN + 632,
        EVT_EFIS_FO_FPV = THIRD_PARTY_EVENT_ID_MIN + 633,
        EVT_EFIS_FO_WXR = THIRD_PARTY_EVENT_ID_MIN + 634,
        EVT_EFIS_FO_STA = THIRD_PARTY_EVENT_ID_MIN + 635,
        EVT_EFIS_FO_WPT = THIRD_PARTY_EVENT_ID_MIN + 636,
        EVT_EFIS_FO_ARPT = THIRD_PARTY_EVENT_ID_MIN + 637,
        EVT_EFIS_FO_DATA = THIRD_PARTY_EVENT_ID_MIN + 638,
        EVT_EFIS_FO_POS = THIRD_PARTY_EVENT_ID_MIN + 639,
        EVT_EFIS_FO_TERR = THIRD_PARTY_EVENT_ID_MIN + 640,
        //EVT_EFIS_FO_LAST = EVT_EFIS_FO_TERR,

        // Glareshield - Display Select Panel
        EVT_DSP_ENG_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 650,
        EVT_DSP_STAT_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 651,
        EVT_DSP_ELEC_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 652,
        EVT_DSP_FUEL_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 653,
        EVT_DSP_AIR_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 654,
        EVT_DSP_HYD_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 655,
        EVT_DSP_DOOR_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 656,
        EVT_DSP_GEAR_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 657,
        EVT_DSP_CANC_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 658, // CANC for 744, CANC/RCL for 748
        EVT_DSP_RCL_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 659,  // 744 only
        EVT_DSP_L_INBD_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 660,   // 748 only
        EVT_DSP_R_INBD_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 661,   // 748 only
        EVT_DSP_LWR_CTR_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 662,  // 748 only
        EVT_DSP_FCTL_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 663, // 748 only
        EVT_DSP_INFO_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 664, // 748 only
        EVT_DSP_CHKL_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 665, // 748 only
        EVT_DSP_NAV_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 667,  // 748 only

        // Glareshield Left 
        EVT_LEFT_OUTBD_BRIGHT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 500,
        EVT_LEFT_INBD_BRIGHT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 501,
        EVT_LEFT_INBD_TERR_BRIGHT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 502,
        EVT_GLARESHIELD_PTT_L = THIRD_PARTY_EVENT_ID_MIN + 503,
        EVT_MAP_LIGHT_L_ROTATE = THIRD_PARTY_EVENT_ID_MIN + 504,
        EVT_MAP_LIGHT_L_PULL_PUSH = THIRD_PARTY_EVENT_ID_MIN + 10504,
        EVT_LEFT_PANEL_LIGHT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 505,
        EVT_LEFT_FLOOD_LIGHT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 506,
        EVT_CLOCK_L = THIRD_PARTY_EVENT_ID_MIN + 507,
        EVT_MASTER_WARNING_RESET_L = THIRD_PARTY_EVENT_ID_MIN + 509,
        EVT_DATA_LINK_ACPT_L = THIRD_PARTY_EVENT_ID_MIN + 511,    // 748 only
        EVT_DATA_LINK_CANC_L = THIRD_PARTY_EVENT_ID_MIN + 512,    // 748 only
        EVT_DATA_LINK_RJCT_L = THIRD_PARTY_EVENT_ID_MIN + 513,    // 748 only
        EVT_PVD_DIMMER_L = THIRD_PARTY_EVENT_ID_MIN + 514,
        EVT_PVD_ON_OFF_L = THIRD_PARTY_EVENT_ID_MIN + 10514,

        // Glareshield Right 
        EVT_RIGHT_OUTBD_BRIGHT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 600,
        EVT_RIGHT_INBD_BRIGHT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 601,
        EVT_RIGHT_INBD_TERR_BRIGHT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 602,
        EVT_GLARESHIELD_PTT_R = THIRD_PARTY_EVENT_ID_MIN + 603,
        EVT_MAP_LIGHT_R_ROTATE = THIRD_PARTY_EVENT_ID_MIN + 604,
        EVT_MAP_LIGHT_R_PULL_PUSH = THIRD_PARTY_EVENT_ID_MIN + 10604,
        EVT_RIGHT_PANEL_LIGHT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 605,
        EVT_RIGHT_FLOOD_LIGHT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 606,
        EVT_CLOCK_R = THIRD_PARTY_EVENT_ID_MIN + 607,
        EVT_MASTER_WARNING_RESET_R = THIRD_PARTY_EVENT_ID_MIN + 609,
        EVT_DATA_LINK_ACPT_R = THIRD_PARTY_EVENT_ID_MIN + 611,    // 748 only
        EVT_DATA_LINK_CANC_R = THIRD_PARTY_EVENT_ID_MIN + 612,    // 748 only
        EVT_DATA_LINK_RJCT_R = THIRD_PARTY_EVENT_ID_MIN + 613,    // 748 only
        EVT_PVD_DIMMER_R = THIRD_PARTY_EVENT_ID_MIN + 614,
        EVT_PVD_ON_OFF_R = THIRD_PARTY_EVENT_ID_MIN + 10614,


        // Left Sidewall
        EVT_CHART_LIGHT_L = THIRD_PARTY_EVENT_ID_MIN + 508,

        // Right Sidewall
        EVT_CHART_LIGHT_R = THIRD_PARTY_EVENT_ID_MIN + 608,

        // Left Forward Panel 
        EVT_FWD_FD_SOURCE_L = THIRD_PARTY_EVENT_ID_MIN + 700,
        EVT_FWD_NAV_SOURCE_L = THIRD_PARTY_EVENT_ID_MIN + 701,
        EVT_FWD_EIU_SOURCE_L = THIRD_PARTY_EVENT_ID_MIN + 702,
        EVT_FWD_IRS_SOURCE_L = THIRD_PARTY_EVENT_ID_MIN + 703,
        EVT_FWD_AIR_DATA_SOURCE_L = THIRD_PARTY_EVENT_ID_MIN + 704,
        EVT_CHRONO_L_CHR = THIRD_PARTY_EVENT_ID_MIN + 705,    // 744 only
        EVT_CHRONO_L_DATE = THIRD_PARTY_EVENT_ID_MIN + 706,   // 744 only
        EVT_CHRONO_L_ET = THIRD_PARTY_EVENT_ID_MIN + 707, // 744 only
        EVT_CHRONO_L_SET = THIRD_PARTY_EVENT_ID_MIN + 708,    // 744 only
        EVT_DSP_INB_DSPL_L = THIRD_PARTY_EVENT_ID_MIN + 710,
        EVT_DSP_LWR_DSPL_L = THIRD_PARTY_EVENT_ID_MIN + 711,  // 744 only
        EVT_ALTN_EFIS_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 714,    // 744 only

        // Right Forward Panel 
        EVT_FWD_FD_SOURCE_R = THIRD_PARTY_EVENT_ID_MIN + 780,
        EVT_FWD_NAV_SOURCE_R = THIRD_PARTY_EVENT_ID_MIN + 781,
        EVT_FWD_EIU_SOURCE_R = THIRD_PARTY_EVENT_ID_MIN + 782,
        EVT_FWD_IRS_SOURCE_R = THIRD_PARTY_EVENT_ID_MIN + 783,
        EVT_FWD_AIR_DATA_SOURCE_R = THIRD_PARTY_EVENT_ID_MIN + 784,
        EVT_CHRONO_R_CHR = THIRD_PARTY_EVENT_ID_MIN + 785,    // 744 only	
        EVT_CHRONO_R_DATE = THIRD_PARTY_EVENT_ID_MIN + 786,   // 744 only 
        EVT_CHRONO_R_ET = THIRD_PARTY_EVENT_ID_MIN + 787, // 744 only	
        EVT_CHRONO_R_SET = THIRD_PARTY_EVENT_ID_MIN + 788,    // 744 only
        EVT_DSP_INB_DSPL_R = THIRD_PARTY_EVENT_ID_MIN + 790,
        EVT_DSP_LWR_DSPL_R = THIRD_PARTY_EVENT_ID_MIN + 791,  // 744 only
        EVT_GPWS_GS_INHIBIT_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 794,
        EVT_GPWS_FLAP_OVRD_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 795,
        EVT_GPWS_FLAP_OVRD_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10795,
        EVT_GPWS_GEAR_OVRD_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 796,
        EVT_GPWS_GEAR_OVRD_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10796,
        EVT_GPWS_TERR_OVRD_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 797,
        EVT_GPWS_TERR_OVRD_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10797,

        // Center Forward Panel
        EVT_FWD_UPPER_BRIGHT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 730,
        EVT_FWD_LOWER_BRIGHT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 731,
        EVT_FWD_LOWER_TERR_BRIGHT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 732,
        EVT_FWD_EICAS_EVENT_RCD = THIRD_PARTY_EVENT_ID_MIN + 733,
        EVT_EFIS_HDG_REF_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 734,
        EVT_FWD_FMC_SELECTOR = THIRD_PARTY_EVENT_ID_MIN + 735,
        EVT_FWD_EIU_SOURCE_C = THIRD_PARTY_EVENT_ID_MIN + 736,
        EVT_ALTN_FLAPS_POS = THIRD_PARTY_EVENT_ID_MIN + 763,
        EVT_ALTN_FLAPS_ARM = THIRD_PARTY_EVENT_ID_MIN + 764,
        EVT_GEAR_ALTN_GEAR_NOSE_BODY = THIRD_PARTY_EVENT_ID_MIN + 765,
        EVT_GEAR_ALTN_GEAR_NOSE_BODY_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10765,
        EVT_GEAR_ALTN_GEAR_WING = THIRD_PARTY_EVENT_ID_MIN + 766,
        EVT_GEAR_ALTN_GEAR_WING_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10766,
        EVT_GEAR_LEVER_UNLOCK = THIRD_PARTY_EVENT_ID_MIN + 767,
        EVT_GEAR_LEVER = THIRD_PARTY_EVENT_ID_MIN + 768,
        EVT_GEAR_LEVER_OFF = THIRD_PARTY_EVENT_ID_MIN + 769,

        // Center Forward Panel - ISFD
        EVT_ISFD_APP = THIRD_PARTY_EVENT_ID_MIN + 770,
        EVT_ISFD_HP_IN = THIRD_PARTY_EVENT_ID_MIN + 771,
        EVT_ISFD_PLUS = THIRD_PARTY_EVENT_ID_MIN + 772,
        EVT_ISFD_MINUS = THIRD_PARTY_EVENT_ID_MIN + 773,
        EVT_ISFD_ATT_RST = THIRD_PARTY_EVENT_ID_MIN + 774,
        EVT_ISFD_BARO = THIRD_PARTY_EVENT_ID_MIN + 775,
        EVT_ISFD_BARO_PUSH = THIRD_PARTY_EVENT_ID_MIN + 776,
        EVT_CLICKSPOT_ISFD = THIRD_PARTY_EVENT_ID_MIN + 777,

        // Center Forward Panel analog standby instruments
        EVT_STANDBY_ADI_CAGE_KNOB = THIRD_PARTY_EVENT_ID_MIN + 745,
        EVT_STANDBY_ALT_BARO_KNOB = THIRD_PARTY_EVENT_ID_MIN + 753,
        EVT_STANDBY_ADI_APPR_MODE = THIRD_PARTY_EVENT_ID_MIN + 744,
        EVT_RMI_LEFT_SELECTOR = THIRD_PARTY_EVENT_ID_MIN + 722,
        EVT_RMI_RIGHT_SELECTOR = THIRD_PARTY_EVENT_ID_MIN + 723,

        // Pedestal - Control Stand
        //
        EVT_CONTROL_STAND_PARK_BRAKE_LEVER = THIRD_PARTY_EVENT_ID_MIN + 953,

        EVT_CONTROL_STAND_SPEED_BRAKE_LEVER = THIRD_PARTY_EVENT_ID_MIN + 952,
        EVT_CONTROL_STAND_SPEED_BRAKE_LEVER_DOWN = THIRD_PARTY_EVENT_ID_MIN + 990,
        EVT_CONTROL_STAND_SPEED_BRAKE_LEVER_ARM = THIRD_PARTY_EVENT_ID_MIN + 991,
        EVT_CONTROL_STAND_SPEED_BRAKE_LEVER_FLT_DET = THIRD_PARTY_EVENT_ID_MIN + 992,
        EVT_CONTROL_STAND_SPEED_BRAKE_LEVER_UP = THIRD_PARTY_EVENT_ID_MIN + 993,
        EVT_CONTROL_STAND_REV_THRUST1_LEVER = THIRD_PARTY_EVENT_ID_MIN + 954,
        EVT_CONTROL_STAND_REV_THRUST2_LEVER = THIRD_PARTY_EVENT_ID_MIN + 955,
        EVT_CONTROL_STAND_REV_THRUST3_LEVER = THIRD_PARTY_EVENT_ID_MIN + 956,
        EVT_CONTROL_STAND_REV_THRUST4_LEVER = THIRD_PARTY_EVENT_ID_MIN + 957,

        EVT_CONTROL_STAND_FWD_THRUST1_LEVER = THIRD_PARTY_EVENT_ID_MIN + 958,
        EVT_CONTROL_STAND_FWD_THRUST2_LEVER = THIRD_PARTY_EVENT_ID_MIN + 959,
        EVT_CONTROL_STAND_FWD_THRUST3_LEVER = THIRD_PARTY_EVENT_ID_MIN + 960,
        EVT_CONTROL_STAND_FWD_THRUST4_LEVER = THIRD_PARTY_EVENT_ID_MIN + 961,

        EVT_CONTROL_STAND_TOGA1_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 962,
        EVT_CONTROL_STAND_TOGA2_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 963,
        EVT_CONTROL_STAND_AT1_DISENGAGE_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 964,
        EVT_CONTROL_STAND_AT2_DISENGAGE_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 965,

        EVT_CONTROL_STAND_FLAPS_LEVER = THIRD_PARTY_EVENT_ID_MIN + 966,
        EVT_CONTROL_STAND_FLAPS_LEVER_0 = THIRD_PARTY_EVENT_ID_MIN + 980,
        EVT_CONTROL_STAND_FLAPS_LEVER_1 = THIRD_PARTY_EVENT_ID_MIN + 981,
        EVT_CONTROL_STAND_FLAPS_LEVER_5 = THIRD_PARTY_EVENT_ID_MIN + 982,
        EVT_CONTROL_STAND_FLAPS_LEVER_10 = THIRD_PARTY_EVENT_ID_MIN + 983,
        EVT_CONTROL_STAND_FLAPS_LEVER_20 = THIRD_PARTY_EVENT_ID_MIN + 984,
        EVT_CONTROL_STAND_FLAPS_LEVER_25 = THIRD_PARTY_EVENT_ID_MIN + 985,
        EVT_CONTROL_STAND_FLAPS_LEVER_30 = THIRD_PARTY_EVENT_ID_MIN + 986,

        EVT_CONTROL_STAND_ENG1_FUEL_CTRL_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 968,
        EVT_CONTROL_STAND_ENG2_FUEL_CTRL_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 969,
        EVT_CONTROL_STAND_ENG3_FUEL_CTRL_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 970,
        EVT_CONTROL_STAND_ENG4_FUEL_CTRL_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 971,
        EVT_CONTROL_STAND_STABCUTOUT_2_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 972,
        EVT_CONTROL_STAND_STABCUTOUT_2_SWITCH_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10972,
        EVT_CONTROL_STAND_STABCUTOUT_3_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 973,
        EVT_CONTROL_STAND_STABCUTOUT_3_SWITCH_GUARD = THIRD_PARTY_EVENT_ID_MIN + 10973,
        EVT_CONTROL_STAND_ALT_PITCH_TRIM_SWITCHES = THIRD_PARTY_EVENT_ID_MIN + 974,

        EVT_CONTROL_STAND_CCD_SEL_L = THIRD_PARTY_EVENT_ID_MIN + 995, // 747-8 only
        EVT_CONTROL_STAND_CCD_TURN_L = THIRD_PARTY_EVENT_ID_MIN + 996,    // 747-8 only
        EVT_CONTROL_STAND_CCD_SEL_R = THIRD_PARTY_EVENT_ID_MIN + 997, // 747-8 only
        EVT_CONTROL_STAND_CCD_TURN_R = THIRD_PARTY_EVENT_ID_MIN + 998,    // 747-8 only

        // Pedestal - Fwd Aisle Stand - Left CDU
        //EVT_CDU_L_START = THIRD_PARTY_EVENT_ID_MIN + 810,
        EVT_CDU_L_L1 = EVT_CDU_L_START + 0,
        EVT_CDU_L_L2 = EVT_CDU_L_START + 1,
        EVT_CDU_L_L3 = EVT_CDU_L_START + 2,
        EVT_CDU_L_L4 = EVT_CDU_L_START + 3,
        EVT_CDU_L_L5 = EVT_CDU_L_START + 4,
        EVT_CDU_L_L6 = EVT_CDU_L_START + 5,
        EVT_CDU_L_R1 = EVT_CDU_L_START + 6,
        EVT_CDU_L_R2 = EVT_CDU_L_START + 7,
        EVT_CDU_L_R3 = EVT_CDU_L_START + 8,
        EVT_CDU_L_R4 = EVT_CDU_L_START + 9,
        EVT_CDU_L_R5 = EVT_CDU_L_START + 10,
        EVT_CDU_L_R6 = EVT_CDU_L_START + 11,
        EVT_CDU_L_INIT_REF = EVT_CDU_L_START + 12,
        EVT_CDU_L_RTE = EVT_CDU_L_START + 13,
        EVT_CDU_L_DEP_ARR = EVT_CDU_L_START + 14,
        EVT_CDU_L_ATC = EVT_CDU_L_START + 15,
        EVT_CDU_L_VNAV = EVT_CDU_L_START + 16,
        EVT_CDU_L_FIX = EVT_CDU_L_START + 17,
        EVT_CDU_L_LEGS = EVT_CDU_L_START + 18,
        EVT_CDU_L_HOLD = EVT_CDU_L_START + 19,
        EVT_CDU_L_FMCCOMM = EVT_CDU_L_START + 20,
        EVT_CDU_L_PROG = EVT_CDU_L_START + 21,
        EVT_CDU_L_EXEC = EVT_CDU_L_START + 22,
        EVT_CDU_L_MENU = EVT_CDU_L_START + 23,
        EVT_CDU_L_NAV_RAD = EVT_CDU_L_START + 24,
        EVT_CDU_L_PREV_PAGE = EVT_CDU_L_START + 25,
        EVT_CDU_L_NEXT_PAGE = EVT_CDU_L_START + 26,
        EVT_CDU_L_1 = EVT_CDU_L_START + 27,
        EVT_CDU_L_2 = EVT_CDU_L_START + 28,
        EVT_CDU_L_3 = EVT_CDU_L_START + 29,
        EVT_CDU_L_4 = EVT_CDU_L_START + 30,
        EVT_CDU_L_5 = EVT_CDU_L_START + 31,
        EVT_CDU_L_6 = EVT_CDU_L_START + 32,
        EVT_CDU_L_7 = EVT_CDU_L_START + 33,
        EVT_CDU_L_8 = EVT_CDU_L_START + 34,
        EVT_CDU_L_9 = EVT_CDU_L_START + 35,
        EVT_CDU_L_DOT = EVT_CDU_L_START + 36,
        EVT_CDU_L_0 = EVT_CDU_L_START + 37,
        EVT_CDU_L_PLUS_MINUS = EVT_CDU_L_START + 38,
        EVT_CDU_L_A = EVT_CDU_L_START + 39,
        EVT_CDU_L_B = EVT_CDU_L_START + 40,
        EVT_CDU_L_C = EVT_CDU_L_START + 41,
        EVT_CDU_L_D = EVT_CDU_L_START + 42,
        EVT_CDU_L_E = EVT_CDU_L_START + 43,
        EVT_CDU_L_F = EVT_CDU_L_START + 44,
        EVT_CDU_L_G = EVT_CDU_L_START + 45,
        EVT_CDU_L_H = EVT_CDU_L_START + 46,
        EVT_CDU_L_I = EVT_CDU_L_START + 47,
        EVT_CDU_L_J = EVT_CDU_L_START + 48,
        EVT_CDU_L_K = EVT_CDU_L_START + 49,
        EVT_CDU_L_L = EVT_CDU_L_START + 50,
        EVT_CDU_L_M = EVT_CDU_L_START + 51,
        EVT_CDU_L_N = EVT_CDU_L_START + 52,
        EVT_CDU_L_O = EVT_CDU_L_START + 53,
        EVT_CDU_L_P = EVT_CDU_L_START + 54,
        EVT_CDU_L_Q = EVT_CDU_L_START + 55,
        EVT_CDU_L_R = EVT_CDU_L_START + 56,
        EVT_CDU_L_S = EVT_CDU_L_START + 57,
        EVT_CDU_L_T = EVT_CDU_L_START + 58,
        EVT_CDU_L_U = EVT_CDU_L_START + 59,
        EVT_CDU_L_V = EVT_CDU_L_START + 60,
        EVT_CDU_L_W = EVT_CDU_L_START + 61,
        EVT_CDU_L_X = EVT_CDU_L_START + 62,
        EVT_CDU_L_Y = EVT_CDU_L_START + 63,
        EVT_CDU_L_Z = EVT_CDU_L_START + 64,
        EVT_CDU_L_SPACE = EVT_CDU_L_START + 65,
        EVT_CDU_L_DEL = EVT_CDU_L_START + 66,
        EVT_CDU_L_SLASH = EVT_CDU_L_START + 67,
        EVT_CDU_L_CLR = EVT_CDU_L_START + 68,
        EVT_CDU_L_BRITENESS = EVT_CDU_L_START + 69,
        //EVT_CDU_L_END = EVT_CDU_L_BRITENESS,

        // Pedestal - Fwd Aisle Stand - Right CDU
        //EVT_CDU_R_START = THIRD_PARTY_EVENT_ID_MIN + 880,
        EVT_CDU_R_L1 = EVT_CDU_R_START + 0,
        CDU_EVT_OFFSET_R = EVT_CDU_R_START - EVT_CDU_L_START,
        EVT_CDU_R_L2 = EVT_CDU_R_START + 1,
        EVT_CDU_R_L3 = EVT_CDU_R_START + 2,
        EVT_CDU_R_L4 = EVT_CDU_R_START + 3,
        EVT_CDU_R_L5 = EVT_CDU_R_START + 4,
        EVT_CDU_R_L6 = EVT_CDU_R_START + 5,
        EVT_CDU_R_R1 = EVT_CDU_R_START + 6,
        EVT_CDU_R_R2 = EVT_CDU_R_START + 7,
        EVT_CDU_R_R3 = EVT_CDU_R_START + 8,
        EVT_CDU_R_R4 = EVT_CDU_R_START + 9,
        EVT_CDU_R_R5 = EVT_CDU_R_START + 10,
        EVT_CDU_R_R6 = EVT_CDU_R_START + 11,
        EVT_CDU_R_INIT_REF = EVT_CDU_R_START + 12,
        EVT_CDU_R_RTE = EVT_CDU_R_START + 13,
        EVT_CDU_R_DEP_ARR = EVT_CDU_R_START + 14,
        EVT_CDU_R_ATC = EVT_CDU_R_START + 15,
        EVT_CDU_R_VNAV = EVT_CDU_R_START + 16,
        EVT_CDU_R_FIX = EVT_CDU_R_START + 17,
        EVT_CDU_R_LEGS = EVT_CDU_R_START + 18,
        EVT_CDU_R_HOLD = EVT_CDU_R_START + 19,
        EVT_CDU_R_FMCCOMM = EVT_CDU_R_START + 20,
        EVT_CDU_R_PROG = EVT_CDU_R_START + 21,
        EVT_CDU_R_EXEC = EVT_CDU_R_START + 22,
        EVT_CDU_R_MENU = EVT_CDU_R_START + 23,
        EVT_CDU_R_NAV_RAD = EVT_CDU_R_START + 24,
        EVT_CDU_R_PREV_PAGE = EVT_CDU_R_START + 25,
        EVT_CDU_R_NEXT_PAGE = EVT_CDU_R_START + 26,
        EVT_CDU_R_1 = EVT_CDU_R_START + 27,
        EVT_CDU_R_2 = EVT_CDU_R_START + 28,
        EVT_CDU_R_3 = EVT_CDU_R_START + 29,
        EVT_CDU_R_4 = EVT_CDU_R_START + 30,
        EVT_CDU_R_5 = EVT_CDU_R_START + 31,
        EVT_CDU_R_6 = EVT_CDU_R_START + 32,
        EVT_CDU_R_7 = EVT_CDU_R_START + 33,
        EVT_CDU_R_8 = EVT_CDU_R_START + 34,
        EVT_CDU_R_9 = EVT_CDU_R_START + 35,
        EVT_CDU_R_DOT = EVT_CDU_R_START + 36,
        EVT_CDU_R_0 = EVT_CDU_R_START + 37,
        EVT_CDU_R_PLUS_MINUS = EVT_CDU_R_START + 38,
        EVT_CDU_R_A = EVT_CDU_R_START + 39,
        EVT_CDU_R_B = EVT_CDU_R_START + 40,
        EVT_CDU_R_C = EVT_CDU_R_START + 41,
        EVT_CDU_R_D = EVT_CDU_R_START + 42,
        EVT_CDU_R_E = EVT_CDU_R_START + 43,
        EVT_CDU_R_F = EVT_CDU_R_START + 44,
        EVT_CDU_R_G = EVT_CDU_R_START + 45,
        EVT_CDU_R_H = EVT_CDU_R_START + 46,
        EVT_CDU_R_I = EVT_CDU_R_START + 47,
        EVT_CDU_R_J = EVT_CDU_R_START + 48,
        EVT_CDU_R_K = EVT_CDU_R_START + 49,
        EVT_CDU_R_L = EVT_CDU_R_START + 50,
        EVT_CDU_R_M = EVT_CDU_R_START + 51,
        EVT_CDU_R_N = EVT_CDU_R_START + 52,
        EVT_CDU_R_O = EVT_CDU_R_START + 53,
        EVT_CDU_R_P = EVT_CDU_R_START + 54,
        EVT_CDU_R_Q = EVT_CDU_R_START + 55,
        EVT_CDU_R_R = EVT_CDU_R_START + 56,
        EVT_CDU_R_S = EVT_CDU_R_START + 57,
        EVT_CDU_R_T = EVT_CDU_R_START + 58,
        EVT_CDU_R_U = EVT_CDU_R_START + 59,
        EVT_CDU_R_V = EVT_CDU_R_START + 60,
        EVT_CDU_R_W = EVT_CDU_R_START + 61,
        EVT_CDU_R_X = EVT_CDU_R_START + 62,
        EVT_CDU_R_Y = EVT_CDU_R_START + 63,
        EVT_CDU_R_Z = EVT_CDU_R_START + 64,
        EVT_CDU_R_SPACE = EVT_CDU_R_START + 65,
        EVT_CDU_R_DEL = EVT_CDU_R_START + 66,
        EVT_CDU_R_SLASH = EVT_CDU_R_START + 67,
        EVT_CDU_R_CLR = EVT_CDU_R_START + 68,
        EVT_CDU_R_BRITENESS = EVT_CDU_R_START + 69,
        //EVT_CDU_R_END = EVT_CDU_R_BRITENESS,

        // Pedestal - Aft Aisle Stand - Center CDU
        //EVT_CDU_C_START = THIRD_PARTY_EVENT_ID_MIN + 1140,
        EVT_CDU_C_L1 = EVT_CDU_C_START + 0,
        CDU_EVT_OFFSET_C = EVT_CDU_C_START - EVT_CDU_L_START,
        EVT_CDU_C_L2 = EVT_CDU_C_START + 1,
        EVT_CDU_C_L3 = EVT_CDU_C_START + 2,
        EVT_CDU_C_L4 = EVT_CDU_C_START + 3,
        EVT_CDU_C_L5 = EVT_CDU_C_START + 4,
        EVT_CDU_C_L6 = EVT_CDU_C_START + 5,
        EVT_CDU_C_R1 = EVT_CDU_C_START + 6,
        EVT_CDU_C_R2 = EVT_CDU_C_START + 7,
        EVT_CDU_C_R3 = EVT_CDU_C_START + 8,
        EVT_CDU_C_R4 = EVT_CDU_C_START + 9,
        EVT_CDU_C_R5 = EVT_CDU_C_START + 10,
        EVT_CDU_C_R6 = EVT_CDU_C_START + 11,
        EVT_CDU_C_INIT_REF = EVT_CDU_C_START + 12,
        EVT_CDU_C_RTE = EVT_CDU_C_START + 13,
        EVT_CDU_C_DEP_ARR = EVT_CDU_C_START + 14,
        EVT_CDU_C_ATC = EVT_CDU_C_START + 15,
        EVT_CDU_C_VNAV = EVT_CDU_C_START + 16,
        EVT_CDU_C_FIX = EVT_CDU_C_START + 17,
        EVT_CDU_C_LEGS = EVT_CDU_C_START + 18,
        EVT_CDU_C_HOLD = EVT_CDU_C_START + 19,
        EVT_CDU_C_FMCCOMM = EVT_CDU_C_START + 20,
        EVT_CDU_C_PROG = EVT_CDU_C_START + 21,
        EVT_CDU_C_EXEC = EVT_CDU_C_START + 22,
        EVT_CDU_C_MENU = EVT_CDU_C_START + 23,
        EVT_CDU_C_NAV_RAD = EVT_CDU_C_START + 24,
        EVT_CDU_C_PREV_PAGE = EVT_CDU_C_START + 25,
        EVT_CDU_C_NEXT_PAGE = EVT_CDU_C_START + 26,
        EVT_CDU_C_1 = EVT_CDU_C_START + 27,
        EVT_CDU_C_2 = EVT_CDU_C_START + 28,
        EVT_CDU_C_3 = EVT_CDU_C_START + 29,
        EVT_CDU_C_4 = EVT_CDU_C_START + 30,
        EVT_CDU_C_5 = EVT_CDU_C_START + 31,
        EVT_CDU_C_6 = EVT_CDU_C_START + 32,
        EVT_CDU_C_7 = EVT_CDU_C_START + 33,
        EVT_CDU_C_8 = EVT_CDU_C_START + 34,
        EVT_CDU_C_9 = EVT_CDU_C_START + 35,
        EVT_CDU_C_DOT = EVT_CDU_C_START + 36,
        EVT_CDU_C_0 = EVT_CDU_C_START + 37,
        EVT_CDU_C_PLUS_MINUS = EVT_CDU_C_START + 38,
        EVT_CDU_C_A = EVT_CDU_C_START + 39,
        EVT_CDU_C_B = EVT_CDU_C_START + 40,
        EVT_CDU_C_C = EVT_CDU_C_START + 41,
        EVT_CDU_C_D = EVT_CDU_C_START + 42,
        EVT_CDU_C_E = EVT_CDU_C_START + 43,
        EVT_CDU_C_F = EVT_CDU_C_START + 44,
        EVT_CDU_C_G = EVT_CDU_C_START + 45,
        EVT_CDU_C_H = EVT_CDU_C_START + 46,
        EVT_CDU_C_I = EVT_CDU_C_START + 47,
        EVT_CDU_C_J = EVT_CDU_C_START + 48,
        EVT_CDU_C_K = EVT_CDU_C_START + 49,
        EVT_CDU_C_L = EVT_CDU_C_START + 50,
        EVT_CDU_C_M = EVT_CDU_C_START + 51,
        EVT_CDU_C_N = EVT_CDU_C_START + 52,
        EVT_CDU_C_O = EVT_CDU_C_START + 53,
        EVT_CDU_C_P = EVT_CDU_C_START + 54,
        EVT_CDU_C_Q = EVT_CDU_C_START + 55,
        EVT_CDU_C_R = EVT_CDU_C_START + 56,
        EVT_CDU_C_S = EVT_CDU_C_START + 57,
        EVT_CDU_C_T = EVT_CDU_C_START + 58,
        EVT_CDU_C_U = EVT_CDU_C_START + 59,
        EVT_CDU_C_V = EVT_CDU_C_START + 60,
        EVT_CDU_C_W = EVT_CDU_C_START + 61,
        EVT_CDU_C_X = EVT_CDU_C_START + 62,
        EVT_CDU_C_Y = EVT_CDU_C_START + 63,
        EVT_CDU_C_Z = EVT_CDU_C_START + 64,
        EVT_CDU_C_SPACE = EVT_CDU_C_START + 65,
        EVT_CDU_C_DEL = EVT_CDU_C_START + 66,
        EVT_CDU_C_SLASH = EVT_CDU_C_START + 67,
        EVT_CDU_C_CLR = EVT_CDU_C_START + 68,
        EVT_CDU_C_BRITENESS = EVT_CDU_C_START + 69,
        //EVT_CDU_C_END = EVT_CDU_C_BRITENESS,

        // Pedestal - Aft Aisle Stand - ACP Captain
        //EVT_ACP_CAPT_FIRST = THIRD_PARTY_EVENT_ID_MIN + 1020,
        EVT_ACP_CAPT_MIC_VHFL = EVT_ACP_CAPT_FIRST + 0,
        EVT_ACP_CAPT_MIC_VHFC = EVT_ACP_CAPT_FIRST + 1,
        EVT_ACP_CAPT_MIC_VHFR = EVT_ACP_CAPT_FIRST + 2,
        EVT_ACP_CAPT_MIC_FLT = EVT_ACP_CAPT_FIRST + 3,
        EVT_ACP_CAPT_MIC_CAB = EVT_ACP_CAPT_FIRST + 4,
        EVT_ACP_CAPT_MIC_PA = EVT_ACP_CAPT_FIRST + 5,
        EVT_ACP_CAPT_MIC_HFL = EVT_ACP_CAPT_FIRST + 6,
        EVT_ACP_CAPT_MIC_HFR = EVT_ACP_CAPT_FIRST + 7,
        EVT_ACP_CAPT_MIC_SAT1 = EVT_ACP_CAPT_FIRST + 8,
        EVT_ACP_CAPT_MIC_SAT2 = EVT_ACP_CAPT_FIRST + 9,
        EVT_ACP_CAPT_REC_VHFL = EVT_ACP_CAPT_FIRST + 10,
        EVT_ACP_CAPT_REC_VHFC = EVT_ACP_CAPT_FIRST + 11,
        EVT_ACP_CAPT_REC_VHFR = EVT_ACP_CAPT_FIRST + 12,
        EVT_ACP_CAPT_REC_FLT = EVT_ACP_CAPT_FIRST + 13,
        EVT_ACP_CAPT_REC_CAB = EVT_ACP_CAPT_FIRST + 14,
        EVT_ACP_CAPT_REC_PA = EVT_ACP_CAPT_FIRST + 15,
        EVT_ACP_CAPT_REC_HFL = EVT_ACP_CAPT_FIRST + 16,
        EVT_ACP_CAPT_REC_HFR = EVT_ACP_CAPT_FIRST + 17,
        EVT_ACP_CAPT_REC_SAT1 = EVT_ACP_CAPT_FIRST + 18,
        EVT_ACP_CAPT_REC_SAT2 = EVT_ACP_CAPT_FIRST + 19,
        EVT_ACP_CAPT_REC_SPKR = EVT_ACP_CAPT_FIRST + 20,
        EVT_ACP_CAPT_REC_VORADF = EVT_ACP_CAPT_FIRST + 21,
        EVT_ACP_CAPT_REC_APP = EVT_ACP_CAPT_FIRST + 22,
        EVT_ACP_CAPT_MIC_INT_SWITCH = EVT_ACP_CAPT_FIRST + 23,
        EVT_ACP_CAPT_FILTER_SELECTOR = EVT_ACP_CAPT_FIRST + 24,
        EVT_ACP_CAPT_VOR_ADF_SELECTOR = EVT_ACP_CAPT_FIRST + 25,
        EVT_ACP_CAPT_APP_SELECTOR = EVT_ACP_CAPT_FIRST + 26,
        //EVT_ACP_CAPT_LAST = EVT_ACP_CAPT_APP_SELECTOR,

        // Pedestal - Aft Aisle Stand - ACP F/O
        //EVT_ACP_FO_FIRST = THIRD_PARTY_EVENT_ID_MIN + 1260,
        EVT_ACP_FO_MIC_VHFL = EVT_ACP_FO_FIRST + 0,
        EVT_ACP_FO_MIC_VHFC = EVT_ACP_FO_FIRST + 1,
        EVT_ACP_FO_MIC_VHFR = EVT_ACP_FO_FIRST + 2,
        EVT_ACP_FO_MIC_FLT = EVT_ACP_FO_FIRST + 3,
        EVT_ACP_FO_MIC_CAB = EVT_ACP_FO_FIRST + 4,
        EVT_ACP_FO_MIC_PA = EVT_ACP_FO_FIRST + 5,
        EVT_ACP_FO_MIC_HFL = EVT_ACP_FO_FIRST + 6,
        EVT_ACP_FO_MIC_HFR = EVT_ACP_FO_FIRST + 7,
        EVT_ACP_FO_MIC_SAT1 = EVT_ACP_FO_FIRST + 8,
        EVT_ACP_FO_MIC_SAT2 = EVT_ACP_FO_FIRST + 9,
        EVT_ACP_FO_REC_VHFL = EVT_ACP_FO_FIRST + 10,
        EVT_ACP_FO_REC_VHFC = EVT_ACP_FO_FIRST + 11,
        EVT_ACP_FO_REC_VHFR = EVT_ACP_FO_FIRST + 12,
        EVT_ACP_FO_REC_FLT = EVT_ACP_FO_FIRST + 13,
        EVT_ACP_FO_REC_CAB = EVT_ACP_FO_FIRST + 14,
        EVT_ACP_FO_REC_PA = EVT_ACP_FO_FIRST + 15,
        EVT_ACP_FO_REC_HFL = EVT_ACP_FO_FIRST + 16,
        EVT_ACP_FO_REC_HFR = EVT_ACP_FO_FIRST + 17,
        EVT_ACP_FO_REC_SAT1 = EVT_ACP_FO_FIRST + 18,
        EVT_ACP_FO_REC_SAT2 = EVT_ACP_FO_FIRST + 19,
        EVT_ACP_FO_REC_SPKR = EVT_ACP_FO_FIRST + 20,
        EVT_ACP_FO_REC_VORADF = EVT_ACP_FO_FIRST + 21,
        EVT_ACP_FO_REC_APP = EVT_ACP_FO_FIRST + 22,
        EVT_ACP_FO_MIC_INT_SWITCH = EVT_ACP_FO_FIRST + 23,
        EVT_ACP_FO_FILTER_SELECTOR = EVT_ACP_FO_FIRST + 24,
        EVT_ACP_FO_VOR_ADF_SELECTOR = EVT_ACP_FO_FIRST + 25,
        EVT_ACP_FO_APP_SELECTOR = EVT_ACP_FO_FIRST + 26,
        //EVT_ACP_FO_LAST = EVT_ACP_FO_APP_SELECTOR,

        // Pedestal - Aft Aisle Stand - ACP Observer
        //EVT_ACP_OBS_FIRST = THIRD_PARTY_EVENT_ID_MIN + 1070,
        EVT_ACP_OBS_MIC_VHFL = EVT_ACP_OBS_FIRST + 0,
        EVT_ACP_OBS_MIC_VHFC = EVT_ACP_OBS_FIRST + 1,
        EVT_ACP_OBS_MIC_VHFR = EVT_ACP_OBS_FIRST + 2,
        EVT_ACP_OBS_MIC_FLT = EVT_ACP_OBS_FIRST + 3,
        EVT_ACP_OBS_MIC_CAB = EVT_ACP_OBS_FIRST + 4,
        EVT_ACP_OBS_MIC_PA = EVT_ACP_OBS_FIRST + 5,
        EVT_ACP_OBS_MIC_HFL = EVT_ACP_OBS_FIRST + 6,
        EVT_ACP_OBS_MIC_HFR = EVT_ACP_OBS_FIRST + 7,
        EVT_ACP_OBS_MIC_SAT1 = EVT_ACP_OBS_FIRST + 8,
        EVT_ACP_OBS_MIC_SAT2 = EVT_ACP_OBS_FIRST + 9,
        EVT_ACP_OBS_REC_VHFL = EVT_ACP_OBS_FIRST + 10,
        EVT_ACP_OBS_REC_VHFC = EVT_ACP_OBS_FIRST + 11,
        EVT_ACP_OBS_REC_VHFR = EVT_ACP_OBS_FIRST + 12,
        EVT_ACP_OBS_REC_FLT = EVT_ACP_OBS_FIRST + 13,
        EVT_ACP_OBS_REC_CAB = EVT_ACP_OBS_FIRST + 14,
        EVT_ACP_OBS_REC_PA = EVT_ACP_OBS_FIRST + 15,
        EVT_ACP_OBS_REC_HFL = EVT_ACP_OBS_FIRST + 16,
        EVT_ACP_OBS_REC_HFR = EVT_ACP_OBS_FIRST + 17,
        EVT_ACP_OBS_REC_SAT1 = EVT_ACP_OBS_FIRST + 18,
        EVT_ACP_OBS_REC_SAT2 = EVT_ACP_OBS_FIRST + 19,
        EVT_ACP_OBS_REC_SPKR = EVT_ACP_OBS_FIRST + 20,
        EVT_ACP_OBS_REC_VORADF = EVT_ACP_OBS_FIRST + 21,
        EVT_ACP_OBS_REC_APP = EVT_ACP_OBS_FIRST + 22,
        EVT_ACP_OBS_MIC_INT_SWITCH = EVT_ACP_OBS_FIRST + 23,
        EVT_ACP_OBS_FILTER_SELECTOR = EVT_ACP_OBS_FIRST + 24,
        EVT_ACP_OBS_VOR_ADF_SELECTOR = EVT_ACP_OBS_FIRST + 25,
        EVT_ACP_OBS_APP_SELECTOR = EVT_ACP_OBS_FIRST + 26,
        //EVT_ACP_OBS_LAST = EVT_ACP_OBS_APP_SELECTOR,

        // Pedestal - Aft Aisle Stand - COMM Panels
        //EVT_COM1_START_RANGE = THIRD_PARTY_EVENT_ID_MIN + 1000,
        EVT_COM1_HF_SENSOR_KNOB = EVT_COM1_START_RANGE + 0,   // 747-400 & 747-8 
        EVT_COM1_TRANSFER_SWITCH = EVT_COM1_START_RANGE + 1,  // 747-400 & 747-8 
        EVT_COM1_OUTER_SELECTOR = EVT_COM1_START_RANGE + 2,   // 747-400 only
        EVT_COM1_INNER_SELECTOR = EVT_COM1_START_RANGE + 3,   // 747-400 only
        EVT_COM1_VHFL_SWITCH = EVT_COM1_START_RANGE + 4,  // 747-400 & 747-8 (labeled VHF L)
        EVT_COM1_VHFC_SWITCH = EVT_COM1_START_RANGE + 5,  // 747-400 only
        EVT_COM1_VHFR_SWITCH = EVT_COM1_START_RANGE + 6,  // 747-400 only
        EVT_COM1_PNL_OFF_SWITCH = EVT_COM1_START_RANGE + 7,   // 747-400 only
        EVT_COM1_HFL_SWITCH = EVT_COM1_START_RANGE + 8,  // 747-400 & 747-8 (labeled HF L)
        EVT_COM1_AM_SWITCH = EVT_COM1_START_RANGE + 9,    // 747-400 & 747-8 
        EVT_COM1_HFR_SWITCH = EVT_COM1_START_RANGE + 10,  // 747-400 only
        EVT_COM1_TEST_SWITCH = EVT_COM1_START_RANGE + 11, // 747-400 only
        //EVT_COM1_END_RANGE = EVT_COM1_TEST_SWITCH,

        //EVT_COM2_START_RANGE = THIRD_PARTY_EVENT_ID_MIN + 1240,
        EVT_COM2_HF_SENSOR_KNOB = EVT_COM2_START_RANGE + 0,
        EVT_COM2_TRANSFER_SWITCH = EVT_COM2_START_RANGE + 1,
        EVT_COM2_OUTER_SELECTOR = EVT_COM2_START_RANGE + 2,
        EVT_COM2_INNER_SELECTOR = EVT_COM2_START_RANGE + 3,
        EVT_COM2_VHFL_SWITCH = EVT_COM2_START_RANGE + 4,
        EVT_COM2_VHFC_SWITCH = EVT_COM2_START_RANGE + 5,
        EVT_COM2_VHFR_SWITCH = EVT_COM2_START_RANGE + 6,
        EVT_COM2_PNL_OFF_SWITCH = EVT_COM2_START_RANGE + 7,
        EVT_COM2_HFL_SWITCH = EVT_COM2_START_RANGE + 8,
        EVT_COM2_AM_SWITCH = EVT_COM2_START_RANGE + 9,
        EVT_COM2_HFR_SWITCH = EVT_COM2_START_RANGE + 10,
        EVT_COM2_TEST_SWITCH = EVT_COM2_START_RANGE + 11,
        //EVT_COM2_END_RANGE = EVT_COM2_TEST_SWITCH,

        //EVT_COM3_START_RANGE = THIRD_PARTY_EVENT_ID_MIN + 1050,
        EVT_COM3_HF_SENSOR_KNOB = EVT_COM3_START_RANGE + 0,
        EVT_COM3_TRANSFER_SWITCH = EVT_COM3_START_RANGE + 1,
        EVT_COM3_OUTER_SELECTOR = EVT_COM3_START_RANGE + 2,
        EVT_COM3_INNER_SELECTOR = EVT_COM3_START_RANGE + 3,
        EVT_COM3_VHFL_SWITCH = EVT_COM3_START_RANGE + 4,
        EVT_COM3_VHFC_SWITCH = EVT_COM3_START_RANGE + 5,
        EVT_COM3_VHFR_SWITCH = EVT_COM3_START_RANGE + 6,
        EVT_COM3_PNL_OFF_SWITCH = EVT_COM3_START_RANGE + 7,
        EVT_COM3_HFL_SWITCH = EVT_COM3_START_RANGE + 8,
        EVT_COM3_AM_SWITCH = EVT_COM3_START_RANGE + 9,
        EVT_COM3_HFR_SWITCH = EVT_COM3_START_RANGE + 10,
        EVT_COM3_TEST_SWITCH = EVT_COM3_START_RANGE + 11,
        //EVT_COM3_END_RANGE = EVT_COM3_TEST_SWITCH,

        // Pedestal - Aft Aisle Stand - 747-8 COMM Panels additional switches
        EVT_COM1_748_START = THIRD_PARTY_EVENT_ID_MIN + 1425,
        //EVT_COM1_DATA_SWITCH = EVT_COM1_748_START,
        //EVT_COM1_KEY_PAD_START_RANGE = THIRD_PARTY_EVENT_ID_MIN + 1426,
        EVT_COM1_KEY_PAD_1 = EVT_COM1_KEY_PAD_START_RANGE + 0,
        EVT_COM1_KEY_PAD_2 = EVT_COM1_KEY_PAD_START_RANGE + 1,
        EVT_COM1_KEY_PAD_3 = EVT_COM1_KEY_PAD_START_RANGE + 2,
        EVT_COM1_KEY_PAD_4 = EVT_COM1_KEY_PAD_START_RANGE + 3,
        EVT_COM1_KEY_PAD_5 = EVT_COM1_KEY_PAD_START_RANGE + 4,
        EVT_COM1_KEY_PAD_6 = EVT_COM1_KEY_PAD_START_RANGE + 5,
        EVT_COM1_KEY_PAD_7 = EVT_COM1_KEY_PAD_START_RANGE + 6,
        EVT_COM1_KEY_PAD_8 = EVT_COM1_KEY_PAD_START_RANGE + 7,
        EVT_COM1_KEY_PAD_9 = EVT_COM1_KEY_PAD_START_RANGE + 8,
        EVT_COM1_KEY_PAD_RCL = EVT_COM1_KEY_PAD_START_RANGE + 9,
        EVT_COM1_KEY_PAD_0 = EVT_COM1_KEY_PAD_START_RANGE + 10,
        EVT_COM1_KEY_PAD_CLR = EVT_COM1_KEY_PAD_START_RANGE + 11,
        //EVT_COM1_KEY_PAD_END_RANGE = EVT_COM1_KEY_PAD_CLR,
        //EVT_COM1_748_END = EVT_COM1_KEY_PAD_END_RANGE,

        EVT_COM2_748_START = THIRD_PARTY_EVENT_ID_MIN + 1465,
        //EVT_COM2_DATA_SWITCH = EVT_COM2_748_START,
        //EVT_COM2_KEY_PAD_START_RANGE = THIRD_PARTY_EVENT_ID_MIN + 1466,
        EVT_COM2_KEY_PAD_1 = EVT_COM2_KEY_PAD_START_RANGE + 0,
        EVT_COM2_KEY_PAD_2 = EVT_COM2_KEY_PAD_START_RANGE + 1,
        EVT_COM2_KEY_PAD_3 = EVT_COM2_KEY_PAD_START_RANGE + 2,
        EVT_COM2_KEY_PAD_4 = EVT_COM2_KEY_PAD_START_RANGE + 3,
        EVT_COM2_KEY_PAD_5 = EVT_COM2_KEY_PAD_START_RANGE + 4,
        EVT_COM2_KEY_PAD_6 = EVT_COM2_KEY_PAD_START_RANGE + 5,
        EVT_COM2_KEY_PAD_7 = EVT_COM2_KEY_PAD_START_RANGE + 6,
        EVT_COM2_KEY_PAD_8 = EVT_COM2_KEY_PAD_START_RANGE + 7,
        EVT_COM2_KEY_PAD_9 = EVT_COM2_KEY_PAD_START_RANGE + 8,
        EVT_COM2_KEY_PAD_RCL = EVT_COM2_KEY_PAD_START_RANGE + 9,
        EVT_COM2_KEY_PAD_0 = EVT_COM2_KEY_PAD_START_RANGE + 10,
        EVT_COM2_KEY_PAD_CLR = EVT_COM2_KEY_PAD_START_RANGE + 11,
        //EVT_COM2_KEY_PAD_END_RANGE = EVT_COM2_KEY_PAD_CLR,
        //EVT_COM2_748_END = EVT_COM2_KEY_PAD_END_RANGE,

        EVT_COM3_748_START = THIRD_PARTY_EVENT_ID_MIN + 1445,
        //EVT_COM3_DATA_SWITCH = EVT_COM3_748_START,
        //EVT_COM3_KEY_PAD_START_RANGE = THIRD_PARTY_EVENT_ID_MIN + 1446,
        EVT_COM3_KEY_PAD_1 = EVT_COM3_KEY_PAD_START_RANGE + 0,
        EVT_COM3_KEY_PAD_2 = EVT_COM3_KEY_PAD_START_RANGE + 1,
        EVT_COM3_KEY_PAD_3 = EVT_COM3_KEY_PAD_START_RANGE + 2,
        EVT_COM3_KEY_PAD_4 = EVT_COM3_KEY_PAD_START_RANGE + 3,
        EVT_COM3_KEY_PAD_5 = EVT_COM3_KEY_PAD_START_RANGE + 4,
        EVT_COM3_KEY_PAD_6 = EVT_COM3_KEY_PAD_START_RANGE + 5,
        EVT_COM3_KEY_PAD_7 = EVT_COM3_KEY_PAD_START_RANGE + 6,
        EVT_COM3_KEY_PAD_8 = EVT_COM3_KEY_PAD_START_RANGE + 7,
        EVT_COM3_KEY_PAD_9 = EVT_COM3_KEY_PAD_START_RANGE + 8,
        EVT_COM3_KEY_PAD_RCL = EVT_COM3_KEY_PAD_START_RANGE + 9,
        EVT_COM3_KEY_PAD_0 = EVT_COM3_KEY_PAD_START_RANGE + 10,
        EVT_COM3_KEY_PAD_CLR = EVT_COM3_KEY_PAD_START_RANGE + 11,
        //EVT_COM3_KEY_PAD_END_RANGE = EVT_COM3_KEY_PAD_CLR,
        //EVT_COM3_748_END = EVT_COM3_KEY_PAD_END_RANGE,

        // Pedestal - Aft Aisle Stand - TCAS panel
        EVT_TCAS_XPNDR = THIRD_PARTY_EVENT_ID_MIN + 1290, // 747-400 & 747-8
        EVT_TCAS_KNOB_L_OUTER = THIRD_PARTY_EVENT_ID_MIN + 1291,  // 747-400 only
        EVT_TCAS_KNOB_L_INNER = THIRD_PARTY_EVENT_ID_MIN + 1292,  // 747-400 only
        EVT_TCAS_KNOB_R_OUTER = THIRD_PARTY_EVENT_ID_MIN + 1293,  // 747-400 only
        EVT_TCAS_KNOB_R_INNER = THIRD_PARTY_EVENT_ID_MIN + 1294,  // 747-400 only
        EVT_TCAS_IDENT = THIRD_PARTY_EVENT_ID_MIN + 1295, // 747-400 & 747-8
        EVT_TCAS_MODE = THIRD_PARTY_EVENT_ID_MIN + 1296,  // 747-400 & 747-8
        EVT_TCAS_TEST = THIRD_PARTY_EVENT_ID_MIN + 1297,  // 747-400 only

        // Pedestal - Aft Aisle Stand - 747-8 TCAS panel additional switches
        EVT_TCAS_AIRSPACE_SELECTOR = THIRD_PARTY_EVENT_ID_MIN + 1298,
        EVT_TCAS_KEYPAD_1 = THIRD_PARTY_EVENT_ID_MIN + 1301,
        EVT_TCAS_KEYPAD_2 = THIRD_PARTY_EVENT_ID_MIN + 1302,
        EVT_TCAS_KEYPAD_3 = THIRD_PARTY_EVENT_ID_MIN + 1303,
        EVT_TCAS_KEYPAD_4 = THIRD_PARTY_EVENT_ID_MIN + 1304,
        EVT_TCAS_KEYPAD_5 = THIRD_PARTY_EVENT_ID_MIN + 1305,
        EVT_TCAS_KEYPAD_6 = THIRD_PARTY_EVENT_ID_MIN + 1306,
        EVT_TCAS_KEYPAD_7 = THIRD_PARTY_EVENT_ID_MIN + 1307,
        EVT_TCAS_KEYPAD_CLR = THIRD_PARTY_EVENT_ID_MIN + 1308,
        EVT_TCAS_KEYPAD_0 = THIRD_PARTY_EVENT_ID_MIN + 1309,

        // Pedestal - Aft Aisle Stand - CALL Panel 	(747-400 Freighter)
        //EVT_PED_CALL_PANEL_FIRST_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 1220,
        EVT_PED_CALL_UD = EVT_PED_CALL_PANEL_FIRST_SWITCH + 0,
        EVT_PED_CALL_CREW_REST_LEFT = EVT_PED_CALL_PANEL_FIRST_SWITCH + 1,
        EVT_PED_CALL_CREW_REST_RIGHT = EVT_PED_CALL_PANEL_FIRST_SWITCH + 2,
        EVT_PED_CALL_CARGO = EVT_PED_CALL_PANEL_FIRST_SWITCH + 3,
        EVT_PED_CALL_GND = EVT_PED_CALL_PANEL_FIRST_SWITCH + 4,
        //EVT_PED_CALL_PANEL_LAST_SWITCH = EVT_PED_CALL_GND,

        // Pedestal - Aft Aisle Stand - CALL Panel 	(747-400 Passenger)
        EVT_PED_CALL_KEY_NXT = THIRD_PARTY_EVENT_ID_MIN + 1225,
        EVT_PED_CALL_KEY_RST = THIRD_PARTY_EVENT_ID_MIN + 1226,
        //EVT_PED_CALL_PANEL_FIRST_KEY = THIRD_PARTY_EVENT_ID_MIN + 1227,
        EVT_PED_CALL_KEY_1 = EVT_PED_CALL_PANEL_FIRST_KEY + 0,
        EVT_PED_CALL_KEY_2 = EVT_PED_CALL_PANEL_FIRST_KEY + 1,
        EVT_PED_CALL_KEY_3 = EVT_PED_CALL_PANEL_FIRST_KEY + 2,
        EVT_PED_CALL_KEY_4 = EVT_PED_CALL_PANEL_FIRST_KEY + 3,
        EVT_PED_CALL_KEY_5 = EVT_PED_CALL_PANEL_FIRST_KEY + 4,
        EVT_PED_CALL_KEY_6 = EVT_PED_CALL_PANEL_FIRST_KEY + 5,
        EVT_PED_CALL_KEY_P = EVT_PED_CALL_PANEL_FIRST_KEY + 6,
        //EVT_PED_CALL_PANEL_LAST_KEY = EVT_PED_CALL_KEY_P,

        // Pedestal - Aft Aisle Stand 
        EVT_PED_NO_SMOKING_LIGHT_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 1100,    // Passenger only - CABIN CHIME in 747-8
        EVT_PED_FASTEN_BELTS_LIGHT_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 1101,  // Passenger only
        EVT_ABS_AUTOBRAKE_SELECTOR = THIRD_PARTY_EVENT_ID_MIN + 1102,
        EVT_FLT_DK_DOOR_SELECTOR = THIRD_PARTY_EVENT_ID_MIN + 1103,   // Freighter only
        EVT_MAIN_DECK_ALERT_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 1106, // 747-8 Freighter only
        EVT_MAIN_DECK_ALERT_COVER = THIRD_PARTY_EVENT_ID_MIN + 11106,// 747-8 Freighter only
        EVT_FCTL_AILERON_TRIM = THIRD_PARTY_EVENT_ID_MIN + 1211,
        EVT_FCTL_RUDDER_TRIM = THIRD_PARTY_EVENT_ID_MIN + 1212,
        EVT_FCTL_RUDDER_CTR = THIRD_PARTY_EVENT_ID_MIN + 1215,    // 747-8 only
        EVT_ATTENDANT_ADVISORY = THIRD_PARTY_EVENT_ID_MIN + 1234,

        // Pedestal - Aft Aisle Stand - Standard 744 WX RADAR panel 
        EVT_WXR_RANGE_SELECTOR = THIRD_PARTY_EVENT_ID_MIN + 1120,
        EVT_WXR_SYSTEM_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 1121,
        EVT_WXR_GAIN_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 1122,
        EVT_WXR_MODE_SELECTOR = THIRD_PARTY_EVENT_ID_MIN + 1123,
        EVT_WXR_TILT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 1124,

        // Pedestal - Aft Aisle Stand - WX RADAR panel Collins 2100 (Freighter and 747-8)
        EVT_WXR_C2100_L_GAIN_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 1401,
        EVT_WXR_C2100_L_TILT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 1402,
        EVT_WXR_C2100_R_GAIN_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 1403,
        EVT_WXR_C2100_R_TILT_CONTROL = THIRD_PARTY_EVENT_ID_MIN + 1404,
        EVT_WXR_C2100_AUTO = THIRD_PARTY_EVENT_ID_MIN + 1405,
        EVT_WXR_C2100_L_R = THIRD_PARTY_EVENT_ID_MIN + 1406,
        EVT_WXR_C2100_TEST = THIRD_PARTY_EVENT_ID_MIN + 1407,
        EVT_WXR_C2100_L_TFR = THIRD_PARTY_EVENT_ID_MIN + 1410,
        EVT_WXR_C2100_L_WX = THIRD_PARTY_EVENT_ID_MIN + 1411,
        EVT_WXR_C2100_L_WX_T = THIRD_PARTY_EVENT_ID_MIN + 1412,
        EVT_WXR_C2100_L_MAP = THIRD_PARTY_EVENT_ID_MIN + 1413,
        EVT_WXR_C2100_L_GC = THIRD_PARTY_EVENT_ID_MIN + 1414,
        EVT_WXR_C2100_R_TFR = THIRD_PARTY_EVENT_ID_MIN + 1415,
        EVT_WXR_C2100_R_WX = THIRD_PARTY_EVENT_ID_MIN + 1416,
        EVT_WXR_C2100_R_WX_T = THIRD_PARTY_EVENT_ID_MIN + 1417,
        EVT_WXR_C2100_R_MAP = THIRD_PARTY_EVENT_ID_MIN + 1418,
        EVT_WXR_C2100_R_GC = THIRD_PARTY_EVENT_ID_MIN + 1419,
        EVT_WXR_C2100_OFFSET_R = EVT_WXR_C2100_R_TFR - EVT_WXR_C2100_L_TFR,

        // Pedestal - Aft Aisle Stand - EVAC Panel (option for 747-400 paax) 
        EVT_PED_EVAC_TEST_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 1421,
        EVT_PED_EVAC_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 1422,
        EVT_PED_EVAC_SWITCH_GUARD = THIRD_PARTY_EVENT_ID_MIN + 11422,
        EVT_PED_EVAC_HORN_SHUTOFF = THIRD_PARTY_EVENT_ID_MIN + 1423,


        //
        // Miscellaneous
        //

        // CDU zoom & scratchpad clickspots
        EVT_CDU_L_ZOOM = THIRD_PARTY_EVENT_ID_MIN + 1501,
        EVT_CDU_L_SCRATCHPAD = THIRD_PARTY_EVENT_ID_MIN + 1502,
        EVT_CDU_R_ZOOM = THIRD_PARTY_EVENT_ID_MIN + 1503,
        EVT_CDU_R_SCRATCHPAD = THIRD_PARTY_EVENT_ID_MIN + 1504,
        EVT_CDU_C_ZOOM = THIRD_PARTY_EVENT_ID_MIN + 1505,
        EVT_CDU_C_SCRATCHPAD = THIRD_PARTY_EVENT_ID_MIN + 1506,

        // Yoke switches, digits wheels & checklist sliders
        //
        EVT_YOKE_L_AP_DISC_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 1540,
        EVT_YOKE_R_AP_DISC_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 1541,
        EVT_YOKE_L_DIGIT_WHEEL_1 = THIRD_PARTY_EVENT_ID_MIN + 1550,   // Option for 744 only
        EVT_YOKE_L_DIGIT_WHEEL_2 = THIRD_PARTY_EVENT_ID_MIN + 1551,   // Option for 744 only
        EVT_YOKE_L_DIGIT_WHEEL_3 = THIRD_PARTY_EVENT_ID_MIN + 1552,   // Option for 744 only
        EVT_YOKE_R_DIGIT_WHEEL_1 = THIRD_PARTY_EVENT_ID_MIN + 1553,   // Option for 744 only
        EVT_YOKE_R_DIGIT_WHEEL_2 = THIRD_PARTY_EVENT_ID_MIN + 1554, // Option for 744 only
        EVT_YOKE_R_DIGIT_WHEEL_3 = THIRD_PARTY_EVENT_ID_MIN + 1555,   // Option for 744 only
        EVT_YOKE_L_SLIDER = THIRD_PARTY_EVENT_ID_MIN + 1560,
        EVT_YOKE_R_SLIDER = THIRD_PARTY_EVENT_ID_MIN + 1561,

        // Oxygen Panels
        //
        EVT_OXY_TEST_RESET_SWITCH_L = THIRD_PARTY_EVENT_ID_MIN + 1601,
        EVT_OXY_TEST_RESET_SWITCH_R = THIRD_PARTY_EVENT_ID_MIN + 1602,
        EVT_OXY_EMER_TEST_L = THIRD_PARTY_EVENT_ID_MIN + 1605,
        EVT_OXY_EMER_TEST_R = THIRD_PARTY_EVENT_ID_MIN + 1606,

        // Heaters 
        EVT_FWD_LEFT_FOOT_HEATER = THIRD_PARTY_EVENT_ID_MIN + 1615,
        EVT_FWD_RIGHT_FOOT_HEATER = THIRD_PARTY_EVENT_ID_MIN + 1616,
        EVT_FWD_LEFT_SHOULDER_HEATER = THIRD_PARTY_EVENT_ID_MIN + 1617,
        EVT_FWD_RIGHT_SHOULDER_HEATER = THIRD_PARTY_EVENT_ID_MIN + 1618,
        EVT_FWD_LEFT_WSHLD_AIR = THIRD_PARTY_EVENT_ID_MIN + 1619,
        EVT_FWD_RIGHT_WSHLD_AIR = THIRD_PARTY_EVENT_ID_MIN + 1620,

        // Observer Panel Lights
        EVT_MAP_LIGHT_OBS_ROTATE = THIRD_PARTY_EVENT_ID_MIN + 1625,
        EVT_MAP_LIGHT_OBS_PULL_PUSH = THIRD_PARTY_EVENT_ID_MIN + 11625,
        EVT_OBSERVER_PANEL_LIGHT = THIRD_PARTY_EVENT_ID_MIN + 1626,
        EVT_SPOT_LIGHT_1 = THIRD_PARTY_EVENT_ID_MIN + 1627,
        EVT_SPOT_LIGHT_2 = THIRD_PARTY_EVENT_ID_MIN + 1628,
        EVT_SPOT_LIGHT_3 = THIRD_PARTY_EVENT_ID_MIN + 1629,

        // Overhead - Miscellaneous
        EVT_GPWS_RWY_OVRD_SWITCH = THIRD_PARTY_EVENT_ID_MIN + 1640,
        EVT_GPWS_RWY_OVRD_GUARD = THIRD_PARTY_EVENT_ID_MIN + 1641,

        // EFB
        //EVT_EFB_L_START = THIRD_PARTY_EVENT_ID_MIN + 1700,
        EVT_EFB_L_MENU = EVT_EFB_L_START + 0,
        EVT_EFB_L_BACK = EVT_EFB_L_START + 1,
        EVT_EFB_L_PAGE_UP = EVT_EFB_L_START + 2,
        EVT_EFB_L_PAGE_DOWN = EVT_EFB_L_START + 3,
        EVT_EFB_L_XFR = EVT_EFB_L_START + 4,
        EVT_EFB_L_ENTER = EVT_EFB_L_START + 5,
        EVT_EFB_L_ZOOM_IN = EVT_EFB_L_START + 6,
        EVT_EFB_L_ZOOM_OUT = EVT_EFB_L_START + 7,
        EVT_EFB_L_ARROW_UP = EVT_EFB_L_START + 8,
        EVT_EFB_L_ARROW_DOWN = EVT_EFB_L_START + 9,
        EVT_EFB_L_ARROW_LEFT = EVT_EFB_L_START + 10,
        EVT_EFB_L_ARROW_RIGHT = EVT_EFB_L_START + 11,
        EVT_EFB_L_LSK_1L = EVT_EFB_L_START + 12,
        EVT_EFB_L_LSK_2L = EVT_EFB_L_START + 13,
        EVT_EFB_L_LSK_3L = EVT_EFB_L_START + 14,
        EVT_EFB_L_LSK_4L = EVT_EFB_L_START + 15,
        EVT_EFB_L_LSK_5L = EVT_EFB_L_START + 16,
        EVT_EFB_L_LSK_6L = EVT_EFB_L_START + 17,
        EVT_EFB_L_LSK_7L = EVT_EFB_L_START + 18,
        EVT_EFB_L_LSK_8L = EVT_EFB_L_START + 19,
        EVT_EFB_L_LSK_1R = EVT_EFB_L_START + 20,
        EVT_EFB_L_LSK_2R = EVT_EFB_L_START + 21,
        EVT_EFB_L_LSK_3R = EVT_EFB_L_START + 22,
        EVT_EFB_L_LSK_4R = EVT_EFB_L_START + 23,
        EVT_EFB_L_LSK_5R = EVT_EFB_L_START + 24,
        EVT_EFB_L_LSK_6R = EVT_EFB_L_START + 25,
        EVT_EFB_L_LSK_7R = EVT_EFB_L_START + 26,
        EVT_EFB_L_LSK_8R = EVT_EFB_L_START + 27,
        EVT_EFB_L_BRIGHTNESS = EVT_EFB_L_START + 28,
        EVT_EFB_L_POWER = EVT_EFB_L_START + 29,

        //EVT_EFB_L_KEY_START = EVT_EFB_L_START + 30,
        EVT_EFB_L_KEY_A = EVT_EFB_L_KEY_START + 0,
        EVT_EFB_L_KEY_B = EVT_EFB_L_KEY_START + 1,
        EVT_EFB_L_KEY_C = EVT_EFB_L_KEY_START + 2,
        EVT_EFB_L_KEY_D = EVT_EFB_L_KEY_START + 3,
        EVT_EFB_L_KEY_E = EVT_EFB_L_KEY_START + 4,
        EVT_EFB_L_KEY_F = EVT_EFB_L_KEY_START + 5,
        EVT_EFB_L_KEY_G = EVT_EFB_L_KEY_START + 6,
        EVT_EFB_L_KEY_H = EVT_EFB_L_KEY_START + 7,
        EVT_EFB_L_KEY_I = EVT_EFB_L_KEY_START + 8,
        EVT_EFB_L_KEY_J = EVT_EFB_L_KEY_START + 9,
        EVT_EFB_L_KEY_K = EVT_EFB_L_KEY_START + 10,
        EVT_EFB_L_KEY_L = EVT_EFB_L_KEY_START + 11,
        EVT_EFB_L_KEY_M = EVT_EFB_L_KEY_START + 12,
        EVT_EFB_L_KEY_N = EVT_EFB_L_KEY_START + 13,
        EVT_EFB_L_KEY_O = EVT_EFB_L_KEY_START + 14,
        EVT_EFB_L_KEY_P = EVT_EFB_L_KEY_START + 15,
        EVT_EFB_L_KEY_Q = EVT_EFB_L_KEY_START + 16,
        EVT_EFB_L_KEY_R = EVT_EFB_L_KEY_START + 17,
        EVT_EFB_L_KEY_S = EVT_EFB_L_KEY_START + 18,
        EVT_EFB_L_KEY_T = EVT_EFB_L_KEY_START + 19,
        EVT_EFB_L_KEY_U = EVT_EFB_L_KEY_START + 20,
        EVT_EFB_L_KEY_V = EVT_EFB_L_KEY_START + 21,
        EVT_EFB_L_KEY_W = EVT_EFB_L_KEY_START + 22,
        EVT_EFB_L_KEY_X = EVT_EFB_L_KEY_START + 23,
        EVT_EFB_L_KEY_Y = EVT_EFB_L_KEY_START + 24,
        EVT_EFB_L_KEY_Z = EVT_EFB_L_KEY_START + 25,
        EVT_EFB_L_KEY_0 = EVT_EFB_L_KEY_START + 26,
        EVT_EFB_L_KEY_1 = EVT_EFB_L_KEY_START + 27,
        EVT_EFB_L_KEY_2 = EVT_EFB_L_KEY_START + 28,
        EVT_EFB_L_KEY_3 = EVT_EFB_L_KEY_START + 29,
        EVT_EFB_L_KEY_4 = EVT_EFB_L_KEY_START + 30,
        EVT_EFB_L_KEY_5 = EVT_EFB_L_KEY_START + 31,
        EVT_EFB_L_KEY_6 = EVT_EFB_L_KEY_START + 32,
        EVT_EFB_L_KEY_7 = EVT_EFB_L_KEY_START + 33,
        EVT_EFB_L_KEY_8 = EVT_EFB_L_KEY_START + 34,
        EVT_EFB_L_KEY_9 = EVT_EFB_L_KEY_START + 35,
        EVT_EFB_L_KEY_SPACE = EVT_EFB_L_KEY_START + 36,
        EVT_EFB_L_KEY_PLUS = EVT_EFB_L_KEY_START + 37,
        EVT_EFB_L_KEY_MINUS = EVT_EFB_L_KEY_START + 38,
        EVT_EFB_L_KEY_DOT = EVT_EFB_L_KEY_START + 39,
        EVT_EFB_L_KEY_SLASH = EVT_EFB_L_KEY_START + 40,
        EVT_EFB_L_KEY_BACKSPACE = EVT_EFB_L_KEY_START + 41,
        EVT_EFB_L_KEY_DEL = EVT_EFB_L_KEY_START + 42,
        EVT_EFB_L_KEY_EQUAL = EVT_EFB_L_KEY_START + 43,
        EVT_EFB_L_KEY_MULTIPLY = EVT_EFB_L_KEY_START + 44,
        EVT_EFB_L_KEY_LEFT_PAR = EVT_EFB_L_KEY_START + 45,
        EVT_EFB_L_KEY_RIGHT_PAR = EVT_EFB_L_KEY_START + 46,
        EVT_EFB_L_KEY_QUEST = EVT_EFB_L_KEY_START + 47,
        EVT_EFB_L_KEY_QUOTE = EVT_EFB_L_KEY_START + 48,
        EVT_EFB_L_KEY_COMMA = EVT_EFB_L_KEY_START + 49,
        EVT_EFB_L_KEY_PAGE_UP = EVT_EFB_L_KEY_START + 50,
        EVT_EFB_L_KEY_PAGE_DOWN = EVT_EFB_L_KEY_START + 51,
        EVT_EFB_L_KEY_ENTER = EVT_EFB_L_KEY_START + 52,
        EVT_EFB_L_KEY_ARROW_UP = EVT_EFB_L_KEY_START + 53,
        EVT_EFB_L_KEY_ARROW_DOWN = EVT_EFB_L_KEY_START + 54,
        //EVT_EFB_L_KEY_END = EVT_EFB_L_KEY_START + 54,
        //EVT_EFB_L_END = EVT_EFB_L_KEY_START + 54,

        //EVT_EFB_R_START = EVT_EFB_L_END + 1,
        EVT_EFB_R_MENU = EVT_EFB_R_START + 0,
        EVT_EFB_R_BACK = EVT_EFB_R_START + 1,
        EVT_EFB_R_PAGE_UP = EVT_EFB_R_START + 2,
        EVT_EFB_R_PAGE_DOWN = EVT_EFB_R_START + 3,
        EVT_EFB_R_XFR = EVT_EFB_R_START + 4,
        EVT_EFB_R_ENTER = EVT_EFB_R_START + 5,
        EVT_EFB_R_ZOOM_IN = EVT_EFB_R_START + 6,
        EVT_EFB_R_ZOOM_OUT = EVT_EFB_R_START + 7,
        EVT_EFB_R_ARROW_UP = EVT_EFB_R_START + 8,
        EVT_EFB_R_ARROW_DOWN = EVT_EFB_R_START + 9,
        EVT_EFB_R_ARROW_LEFT = EVT_EFB_R_START + 10,
        EVT_EFB_R_ARROW_RIGHT = EVT_EFB_R_START + 11,
        EVT_EFB_R_LSK_1L = EVT_EFB_R_START + 12,
        EVT_EFB_R_LSK_2L = EVT_EFB_R_START + 13,
        EVT_EFB_R_LSK_3L = EVT_EFB_R_START + 14,
        EVT_EFB_R_LSK_4L = EVT_EFB_R_START + 15,
        EVT_EFB_R_LSK_5L = EVT_EFB_R_START + 16,
        EVT_EFB_R_LSK_6L = EVT_EFB_R_START + 17,
        EVT_EFB_R_LSK_7L = EVT_EFB_R_START + 18,
        EVT_EFB_R_LSK_8L = EVT_EFB_R_START + 19,
        EVT_EFB_R_LSK_1R = EVT_EFB_R_START + 20,
        EVT_EFB_R_LSK_2R = EVT_EFB_R_START + 21,
        EVT_EFB_R_LSK_3R = EVT_EFB_R_START + 22,
        EVT_EFB_R_LSK_4R = EVT_EFB_R_START + 23,
        EVT_EFB_R_LSK_5R = EVT_EFB_R_START + 24,
        EVT_EFB_R_LSK_6R = EVT_EFB_R_START + 25,
        EVT_EFB_R_LSK_7R = EVT_EFB_R_START + 26,
        EVT_EFB_R_LSK_8R = EVT_EFB_R_START + 27,
        EVT_EFB_R_BRIGHTNESS = EVT_EFB_R_START + 28,
        EVT_EFB_R_POWER = EVT_EFB_R_START + 29,

        //EVT_EFB_R_KEY_START = EVT_EFB_R_START + 30,
        EVT_EFB_R_KEY_A = EVT_EFB_R_KEY_START + 0,
        EVT_EFB_R_KEY_B = EVT_EFB_R_KEY_START + 1,
        EVT_EFB_R_KEY_C = EVT_EFB_R_KEY_START + 2,
        EVT_EFB_R_KEY_D = EVT_EFB_R_KEY_START + 3,
        EVT_EFB_R_KEY_E = EVT_EFB_R_KEY_START + 4,
        EVT_EFB_R_KEY_F = EVT_EFB_R_KEY_START + 5,
        EVT_EFB_R_KEY_G = EVT_EFB_R_KEY_START + 6,
        EVT_EFB_R_KEY_H = EVT_EFB_R_KEY_START + 7,
        EVT_EFB_R_KEY_I = EVT_EFB_R_KEY_START + 8,
        EVT_EFB_R_KEY_J = EVT_EFB_R_KEY_START + 9,
        EVT_EFB_R_KEY_K = EVT_EFB_R_KEY_START + 10,
        EVT_EFB_R_KEY_L = EVT_EFB_R_KEY_START + 11,
        EVT_EFB_R_KEY_M = EVT_EFB_R_KEY_START + 12,
        EVT_EFB_R_KEY_N = EVT_EFB_R_KEY_START + 13,
        EVT_EFB_R_KEY_O = EVT_EFB_R_KEY_START + 14,
        EVT_EFB_R_KEY_P = EVT_EFB_R_KEY_START + 15,
        EVT_EFB_R_KEY_Q = EVT_EFB_R_KEY_START + 16,
        EVT_EFB_R_KEY_R = EVT_EFB_R_KEY_START + 17,
        EVT_EFB_R_KEY_S = EVT_EFB_R_KEY_START + 18,
        EVT_EFB_R_KEY_T = EVT_EFB_R_KEY_START + 19,
        EVT_EFB_R_KEY_U = EVT_EFB_R_KEY_START + 20,
        EVT_EFB_R_KEY_V = EVT_EFB_R_KEY_START + 21,
        EVT_EFB_R_KEY_W = EVT_EFB_R_KEY_START + 22,
        EVT_EFB_R_KEY_X = EVT_EFB_R_KEY_START + 23,
        EVT_EFB_R_KEY_Y = EVT_EFB_R_KEY_START + 24,
        EVT_EFB_R_KEY_Z = EVT_EFB_R_KEY_START + 25,
        EVT_EFB_R_KEY_0 = EVT_EFB_R_KEY_START + 26,
        EVT_EFB_R_KEY_1 = EVT_EFB_R_KEY_START + 27,
        EVT_EFB_R_KEY_2 = EVT_EFB_R_KEY_START + 28,
        EVT_EFB_R_KEY_3 = EVT_EFB_R_KEY_START + 29,
        EVT_EFB_R_KEY_4 = EVT_EFB_R_KEY_START + 30,
        EVT_EFB_R_KEY_5 = EVT_EFB_R_KEY_START + 31,
        EVT_EFB_R_KEY_6 = EVT_EFB_R_KEY_START + 32,
        EVT_EFB_R_KEY_7 = EVT_EFB_R_KEY_START + 33,
        EVT_EFB_R_KEY_8 = EVT_EFB_R_KEY_START + 34,
        EVT_EFB_R_KEY_9 = EVT_EFB_R_KEY_START + 35,
        EVT_EFB_R_KEY_SPACE = EVT_EFB_R_KEY_START + 36,
        EVT_EFB_R_KEY_PLUS = EVT_EFB_R_KEY_START + 37,
        EVT_EFB_R_KEY_MINUS = EVT_EFB_R_KEY_START + 38,
        EVT_EFB_R_KEY_DOT = EVT_EFB_R_KEY_START + 39,
        EVT_EFB_R_KEY_SLASH = EVT_EFB_R_KEY_START + 40,
        EVT_EFB_R_KEY_BACKSPACE = EVT_EFB_R_KEY_START + 41,
        EVT_EFB_R_KEY_DEL = EVT_EFB_R_KEY_START + 42,
        EVT_EFB_R_KEY_EQUAL = EVT_EFB_R_KEY_START + 43,
        EVT_EFB_R_KEY_MULTIPLY = EVT_EFB_R_KEY_START + 44,
        EVT_EFB_R_KEY_LEFT_PAR = EVT_EFB_R_KEY_START + 45,
        EVT_EFB_R_KEY_RIGHT_PAR = EVT_EFB_R_KEY_START + 46,
        EVT_EFB_R_KEY_QUEST = EVT_EFB_R_KEY_START + 47,
        EVT_EFB_R_KEY_QUOTE = EVT_EFB_R_KEY_START + 48,
        EVT_EFB_R_KEY_COMMA = EVT_EFB_R_KEY_START + 49,
        EVT_EFB_R_KEY_PAGE_UP = EVT_EFB_R_KEY_START + 50,
        EVT_EFB_R_KEY_PAGE_DOWN = EVT_EFB_R_KEY_START + 51,
        EVT_EFB_R_KEY_ENTER = EVT_EFB_R_KEY_START + 52,
        EVT_EFB_R_KEY_ARROW_UP = EVT_EFB_R_KEY_START + 53,
        EVT_EFB_R_KEY_ARROW_DOWN = EVT_EFB_R_KEY_START + 54,
        //EVT_EFB_R_KEY_END = EVT_EFB_R_KEY_START + 54,
        //EVT_EFB_R_END = EVT_EFB_R_KEY_START + 54,

        // Parameter: 1000000 x (action code) + 1000 x (X Coordinate) + (Y Coordinate)
        // action codes: 0 = mouse move, 1 = mouse click,  2= mouse release, 3 = mouse wheel up, 4 = mouse wheel down
        // X / Y Coordinates: 0..1000 of EFB_SCREEN_WIDTH / EFB_SCREEN_HEIGHT (not required for action codes 3 & 4)
        EVT_EFB_L_SCREEN_ACTION = THIRD_PARTY_EVENT_ID_MIN + 1900,
        EVT_EFB_R_SCREEN_ACTION = THIRD_PARTY_EVENT_ID_MIN + 1901,


        //
        // Custom shortcut special events
        //
        EVT_LDG_LIGHTS_TOGGLE = THIRD_PARTY_EVENT_ID_MIN + 14000,
        EVT_TURNOFF_LIGHTS_TOGGLE = THIRD_PARTY_EVENT_ID_MIN + 14001,
        EVT_COCKPIT_LIGHTS_TOGGLE = THIRD_PARTY_EVENT_ID_MIN + 14002,
        EVT_PANEL_LIGHTS_TOGGLE = THIRD_PARTY_EVENT_ID_MIN + 14003,
        EVT_FLOOD_LIGHTS_TOGGLE = THIRD_PARTY_EVENT_ID_MIN + 14004,
        EVT_FUEL_CTRL_SWITCHES_ALL_CUTOFF = THIRD_PARTY_EVENT_ID_MIN + 14005,

        EVT_DOOR_1L = THIRD_PARTY_EVENT_ID_MIN + 14011,
        EVT_DOOR_1R = THIRD_PARTY_EVENT_ID_MIN + 14012,
        EVT_DOOR_2L = THIRD_PARTY_EVENT_ID_MIN + 14013,
        EVT_DOOR_2R = THIRD_PARTY_EVENT_ID_MIN + 14014,
        EVT_DOOR_3L = THIRD_PARTY_EVENT_ID_MIN + 14015,
        EVT_DOOR_3R = THIRD_PARTY_EVENT_ID_MIN + 14016,
        EVT_DOOR_4L = THIRD_PARTY_EVENT_ID_MIN + 14017,
        EVT_DOOR_4R = THIRD_PARTY_EVENT_ID_MIN + 14018,
        EVT_DOOR_5L = THIRD_PARTY_EVENT_ID_MIN + 14019,
        EVT_DOOR_5R = THIRD_PARTY_EVENT_ID_MIN + 14020,
        EVT_DOOR_UPPER_DECK_L = THIRD_PARTY_EVENT_ID_MIN + 14021,
        EVT_DOOR_UPPER_DECK_R = THIRD_PARTY_EVENT_ID_MIN + 14022,
        EVT_DOOR_CARGO_FWD = THIRD_PARTY_EVENT_ID_MIN + 14023,
        EVT_DOOR_CARGO_AFT = THIRD_PARTY_EVENT_ID_MIN + 14024,
        EVT_DOOR_CARGO_BULK = THIRD_PARTY_EVENT_ID_MIN + 14025,
        EVT_DOOR_CARGO_SIDE = THIRD_PARTY_EVENT_ID_MIN + 14026,
        EVT_DOOR_CARGO_NOSE = THIRD_PARTY_EVENT_ID_MIN + 14027,
        EVT_DOOR_MAIN_ELEC = THIRD_PARTY_EVENT_ID_MIN + 14028,
        EVT_DOOR_CTR_ELEC = THIRD_PARTY_EVENT_ID_MIN + 14029,
        EVT_DOOR_FD_OVERHEAD = THIRD_PARTY_EVENT_ID_MIN + 14030,

        // MCP Direct Control 
        EVT_MCP_IAS_SET = THIRD_PARTY_EVENT_ID_MIN + 14502,   // Sets MCP IAS, if IAS mode is active
        EVT_MCP_MACH_SET = THIRD_PARTY_EVENT_ID_MIN + 14503,  // Sets MCP MACH (if active) to parameter*0.01 (e.g. send 78 to set M0.78)
        EVT_MCP_HDG_SET = THIRD_PARTY_EVENT_ID_MIN + 14504,   // Sets new heading or track, commands the shortest turn
        EVT_MCP_ALT_SET = THIRD_PARTY_EVENT_ID_MIN + 14505,
        EVT_MCP_VS_SET = THIRD_PARTY_EVENT_ID_MIN + 14506,    // Sets MCP VS (if VS window open) to parameter-10000 (e.g. send 8200 for -1800fpm)

        // Panel system events
        //EVT_CTRL_ACCELERATION_DISABLE = THIRD_PARTY_EVENT_ID_MIN + 14600,
        //EVT_CTRL_ACCELERATION_ENABLE = THIRD_PARTY_EVENT_ID_MIN + 14600,
        //EVT_2D_PANEL_OFFSET = 20000,  // added to events triggered by 2D panel pop-up windows

        // Internal use
        //EVT_SC_SENSORS_START = THIRD_PARTY_EVENT_ID_MIN + 14650, // = 84282
        EVT_SC_SENSOR_THROTTLE_1 = EVT_SC_SENSORS_START + 0,
        EVT_SC_SENSOR_THROTTLE_2 = EVT_SC_SENSORS_START + 1,
        EVT_SC_SENSOR_THROTTLE_3 = EVT_SC_SENSORS_START + 2,
        EVT_SC_SENSOR_THROTTLE_4 = EVT_SC_SENSORS_START + 3,
        EVT_SC_SENSOR_REVERSER_1 = EVT_SC_SENSORS_START + 4,
        EVT_SC_SENSOR_REVERSER_2 = EVT_SC_SENSORS_START + 5,
        EVT_SC_SENSOR_REVERSER_3 = EVT_SC_SENSORS_START + 6,
        EVT_SC_SENSOR_REVERSER_4 = EVT_SC_SENSORS_START + 7,
        EVT_SC_SENSOR_SPEEDBRAKE = EVT_SC_SENSORS_START + 8,
        EVT_SC_SENSOR_FLAPS = EVT_SC_SENSORS_START + 9,
        //EVT_SC_SENSORS_LAST = EVT_SC_SENSOR_FLAPS,

        //EVT_SC_AXIS_START = THIRD_PARTY_EVENT_ID_MIN + 14680, // = 84312
        EVT_SC_AXIS_OUTBD_DISPL_CAPT = EVT_SC_AXIS_START + 0,
        EVT_SC_AXIS_INBD_DISPL_CAPT = EVT_SC_AXIS_START + 1,
        EVT_SC_AXIS_EICAS_UPR = EVT_SC_AXIS_START + 2,
        EVT_SC_AXIS_EICAS_LWR = EVT_SC_AXIS_START + 3,
        EVT_SC_AXIS_INBD_DISPL_FO = EVT_SC_AXIS_START + 4,
        EVT_SC_AXIS_OUTBD_DISPL_FO = EVT_SC_AXIS_START + 5,
        EVT_SC_AXIS_INBD_TERR_CAPT = EVT_SC_AXIS_START + 6,
        EVT_SC_AXIS_INBD_TERR_FO = EVT_SC_AXIS_START + 7,
        EVT_SC_AXIS_EICAS_LWR_TERR = EVT_SC_AXIS_START + 8,
        //EVT_SC_AXIS_LAST = EVT_SC_AXIS_EICAS_LWR_TERR,  //TODO!!!!
    };
}