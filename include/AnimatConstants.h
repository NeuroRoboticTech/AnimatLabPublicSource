#ifndef __ANIMAT_CONSTANTS_H__
#define __ANIMAT_CONSTANTS_H__

#define Al_Err_lInvalidPartType -3000
#define Al_Err_strInvalidPartType "Invalid body part type specified."

#define Al_Err_lInvalidJointType -3001
#define Al_Err_strInvalidJointType "Invalid body joint type specified."

#define Al_Err_lLayoutBlank -3002
#define Al_Err_strLayoutBlank "The body plan file name is blank."

#define Al_Err_lNeuralNetworkBlank -3003
#define Al_Err_strNeuralNetworkBlank "The neural network file name is blank."

#define Al_Err_lOrganismIDBlank -3004
#define Al_Err_strOrganismIDBlank "The organism ID is blank."

#define Al_Err_lProjectPathBlank -3005
#define Al_Err_strProjectPathBlank "The project path is blank."

#define Al_Err_lParentNotDefined -3006
#define Al_Err_strParentNotDefined "The parent part is not defined."

#define Al_Err_lChildNotDefined -3007
#define Al_Err_strChildNotDefined "The child part is not defined."

#define Al_Err_lRenderContextNotDefined -3008
#define Al_Err_strRenderContextNotDefined "The render context is not defined."

#define Al_Err_lSimNotDefined -3008
#define Al_Err_strSimNotDefined "The simulation is not defined."

#define Al_Err_lJointParentMismatch -3009
#define Al_Err_strJointParentMismatch "A parent part is defined for this part, but no joint is defined between them!"

#define Al_Err_lJointIDNotFound -3010
#define Al_Err_strJointIDNotFound "The specified joint was not found."

#define Al_Err_lBodyNotDefined -3011
#define Al_Err_strBodyNotDefined "The rigid body was not defined."

#define Al_Err_lOrganismIDNotFound -3012
#define Al_Err_strOrganismIDNotFound "The specified organism was not found."

#define Al_Err_lLimitNotDefined -3013
#define Al_Err_strLimitNotDefined "The specified constraint limit was not defined."

#define Al_Err_lClassFactoryNotDefined -3014
#define Al_Err_strClassFactoryNotDefined "The class factory was not defined."

#define Al_Err_lInvalidOrganismType -3015
#define Al_Err_strInvalidOrganismType "An invalid organism type was specified."

#define Al_Err_lForceLessThanZero -3016
#define Al_Err_strForceLessThanZero "The specified muscle force was less than zero."

#define Al_Err_lPartNotAttachment -3017
#define Al_Err_strPartNotAttachment "The body part was not a muscle attachment part type."

#define Al_Err_lRigidBodyIDNotFound -3018
#define Al_Err_strRigidBodyIDNotFound "The specified rigid body was not found."

#define Al_Err_lStructureIDBlank -3019
#define Al_Err_strStructureIDBlank "The structure ID is blank."

#define Al_Err_lStructureIDNotFound -3020
#define Al_Err_strStructureIDNotFound "The specified structure was not found."

#define Al_Err_lJointNotDefined -3021
#define Al_Err_strJointNotDefined "The joint was not defined."

#define Al_Err_lStructureNotDefined -3022
#define Al_Err_strStructureNotDefined "Structure not defined."

#define Al_Err_lUnableToCastBodyToDesiredType -3023
#define Al_Err_strUnableToCastBodyToDesiredType "Unable to cast rigid body to desired type."

#define Al_Err_lMuscleNotFound -3024
#define Al_Err_strMuscleNotFound "The specified muscle was not found."

#define Al_Err_lUnableToCastSimToDesiredType -3025
#define Al_Err_strUnableToCastSimToDesiredType "Unable to cast simulator to desired type."

#define Al_Err_lInvalidSurceContactCount -3026
#define Al_Err_strInvalidSurceContactCount "Invalid surface contact count. You are attempting to reset the contact count below zero."

#define Al_Err_lMustBeContactBodyToGetCount -3027
#define Al_Err_strMustBeContactBodyToGetCount "The specified rigid body must be a contact sensor in order to get the surface contact count data."

#define Al_Err_lUnableToCastJointToDesiredType -3028
#define Al_Err_strUnableToCastJointToDesiredType "Unable to cast joint to desired type."

#define Al_Err_lBodyOrJointIDNotFound -3028
#define Al_Err_strBodyOrJointIDNotFound "No body or joint was found with the specified ID."

#define Al_Err_lParentHwndNotDefined -3029
#define Al_Err_strParentHwndNotDefined "No Parent HWND was defined."

#define Al_Err_lSimFileBlank -3030
#define Al_Err_strSimFileBlank "The simulation file name is blank."

#define Al_Err_lModuleFactoryNotNeural -3031
#define Al_Err_strModuleFactoryNotNeural "The class factory loaded from the specified module is not a neural class factory type."

#define Al_Err_lModuleFactoryNotAnimat -3032
#define Al_Err_strModuleFactoryNotAnimat "The class factory loaded from the specified module is not an animat class factory type."

#define Al_Err_lNoProjectParamOnCommandLine -3033
#define Al_Err_strNoProjectParamOnCommandLine "There was no project parameter defined on the command line."

#define Al_Err_lInvalidSimulatorType -3034
#define Al_Err_strInvalidSimulatorType "Invalid simulator type."

#define Al_Err_lFullPathNotSpecified -3035
#define Al_Err_strFullPathNotSpecified "A full path to the project simulation file was not given."

#define Err_lSimulationWndNotDefined -3036
#define Err_strSimulationWndNotDefined "A full path to the project simulation file was not given."

#define Al_Err_lItemNotKeyFrameType -3037
#define Al_Err_strItemNotKeyFrameType "The activated item is not a key frame type of object."

#define Al_Err_lInvalidKeyFrameType -3038
#define Al_Err_strInvalidKeyFrameType "Invalid Keyframe type."

#define Al_Err_lNoRecorderDefined -3039
#define Al_Err_strNoRecorderDefined "You can not perform actions on recorder keyframes when no simulation recorder has been defined."

#define Al_Err_lKeyFrameOverlap -3040
#define Al_Err_strKeyFrameOverlap "The keyframe with the following ID overlaps with this keyframe."

#define Al_Err_lOpNotDefinedForAdapter -3041
#define Al_Err_strOpNotDefinedForAdapter "The following opertion is not defined for an adapter object."

#define Al_Err_lModuleNameBlank -3042
#define Al_Err_strModuleNameBlank "The name for the following module is blank."

#define Al_Err_lDataTypeBlank -3043
#define Al_Err_strDataTypeBlank "The data type is blank."

#define Al_Err_lConvertingClassToType -3044
#define Al_Err_strConvertingClassToType "A failure occured while trying to convert a newly created object from the class factory to the type specified."

#define Al_Err_lModuleNameNotFound -3044
#define Al_Err_strModuleNameNotFound "The specified neural module name was not found in the list of modules."

#define Al_Err_lNeuralModuleNotDefined -3045
#define Al_Err_strNeuralModuleNotDefined "The neural module was not defined."

#define Al_Err_lInvalidAdapterType -3046
#define Al_Err_strInvalidAdapterType "Invalid adapter type."

#define Al_Err_lInvalidGainType -3046
#define Al_Err_strInvalidGainType "Invalid gain type."

#define Al_Err_lBodyTypeBlank -3047
#define Al_Err_strBodyTypeBlank "The body type is blank."

#define Al_Err_lBodyIDBlank -3047
#define Al_Err_strBodyIDBlank "The body ID is blank."

#define Al_Err_lJointIDBlank -3048
#define Al_Err_strJointIDBlank "The joint ID is blank."

#define Al_Err_lModuleClassFactoryNotDefined -3048
#define Al_Err_strModuleClassFactoryNotDefined "The class factory for the specified neural module is not defined."

#define Al_Err_lSimulationNotDefined -3024
#define Al_Err_strSimulationNotDefined "Simulation has not been defined."

#define Al_Err_lInvalidDataType -3022
#define Al_Err_strInvalidDataType "Invalid data type."

#define Al_Err_lIDBlank -3020
#define Al_Err_strIDBlank "ID is blank."

#define Al_Err_lItemNotStimulusType -3028
#define Al_Err_strItemNotStimulusType "Activated item is not an external stimulus type."

#define Al_Err_lItemNotDataChartType -3026
#define Al_Err_strItemNotDataChartType "The activated object is not a data chart type."

#define Al_Err_lExceededMaxBuffer -3025
#define Al_Err_strExceededMaxBuffer "Data Buffer size has been exceeded."

#define Al_Err_lInvalidDataColumnType -3021
#define Al_Err_strInvalidDataColumnType "Invalid data column type."

#define Al_Err_lFilenameBlank -3019
#define Al_Err_strFilenameBlank "Filename is blank."

#define Al_Err_lActivatedItemIDNotFound -3031
#define Al_Err_strActivatedItemIDNotFound "The specified activated item was not found."

#define Al_Err_lInvalidExternalStimulusType -3027
#define Al_Err_strInvalidExternalStimulusType "Invalid external stimulus type."

#define Al_Err_lInvalidDataChartType -3023
#define Al_Err_strInvalidDataChartType "Invalid data chart type."

#define Al_Err_lInvalidNeuralModuleType -3032
#define Al_Err_strInvalidNeuralModuleType "An invalid neural module type was specified."

#define Al_Err_lInvalidMassUnits -3035
#define Al_Err_strInvalidMassUnits "Invalid data column type."

#define Al_Err_lInvalidDistanceUnits -3036
#define Al_Err_strInvalidDistanceUnits "Invalid data column type."

#define Al_Err_lNeuralModuleNameBlank -3037
#define Al_Err_strNeuralModuleNameBlank "The neural module name is blank."

#define Al_Err_lMuscleRestPercExceed100 -3038
#define Al_Err_strMuscleRestPercExceed100 "The total resting length for the elements of the muscle exceed 100%."

#define Al_Err_lMuscleRestPercLessThan100 -3039
#define Al_Err_strMuscleRestPercLessThan100 "The total resting length for the elements of the muscle not be less than 100%."

#define Al_Err_lTLCurveEndPointsNotZero -3040
#define Al_Err_strTLCurveEndPointsNotZero "The y values for the end points of the tension length curve must be 0."

#define Al_Err_lObtainingCriticalSection -3050
#define Al_Err_strObtainingCriticalSection "Unable to obtain a critical section for the memory data chart."

#define Al_Err_lDataColumnIDNotFound -3051
#define Al_Err_strDataColumnIDNotFound "The specified data column was not found."

#define Al_Err_lInvalidPlaybackRate -3052
#define Al_Err_strInvalidPlaybackRate "An invalid playback rate was specified."

#define Al_Err_lActivatedItemNull -3053
#define Al_Err_strActivatedItemNull "The activated item is null."

#define Al_Err_lDataPointNotFound -3054
#define Al_Err_strDataPointNotFound "The data pointer for the following type was not found."

#define Al_Err_lVortexSimulationError -3055
#define Al_Err_strVortexSimulationError "An error occured in the vortex simulator."

#define Al_Err_lInvalidConeRadius -3056
#define Al_Err_strInvalidConeRadius "Both the upper and lower radius of a cone can not be zero."

#define Al_Err_lNeedTwoMuscleAttachments -3057
#define Al_Err_strNeedTwoMuscleAttachments "At least two muscle attachments are required."

#define Al_Err_lNoVerticesDefined -3058
#define Al_Err_strNoVerticesDefined "No vertices have been defined for this body."

#define Al_Err_lGraphicsMeshNotDefined -3059
#define Al_Err_strGraphicsMeshNotDefined "No graphics mesh was defined for this body."

#define Al_Err_lInvalidCollisionMeshType -3060
#define Al_Err_strInvalidCollisionMeshType "An invalid collision mesh type was specified."

#define Al_Err_lReceptiveFieldVertexNotFound -3061
#define Al_Err_strReceptiveFieldVertexNotFound "The specified receptive field index was not found."

#define Al_Err_lOdorNotDefined -3062
#define Al_Err_strOdorNotDefined "The odor was not defined."

#define Al_Err_lOdorIDNotFound -3063
#define Al_Err_strOdorIDNotFound "The specified odor ID was not found."

#define Al_Err_lUnableToConvertToCeSimulator -3064
#define Al_Err_strUnableToConvertToCeSimulator "Unable to convert simulator pointer to CeSimulator type."

#define Al_Err_lKeyFramNotDefinedForCE -3065
#define Al_Err_strKeyFramNotDefinedForCE "Keyframes are not defined for the CeSimulator type."

#define Al_Err_lInvalidMicrocontrollerType -3066
#define Al_Err_strInvalidMicrocontrollerType "An invalid microcontroller type was specified."

#define Al_Err_lOpenCommPort -3067
#define Al_Err_strOpenCommPort "Unable to open the comm port for communications."

#define Al_Err_lGetCommState -3068
#define Al_Err_strGetCommState "Unable to get the comm state of the serial port."

#define Al_Err_lSetCommState -3069
#define Al_Err_strSetCommState "Unable to set the comm state of the serial port."

#define Al_Err_lUnableToCreateProcessThread -3070
#define Al_Err_strUnableToCreateProcessThread "Unable to create the processing IO thread for the microcontroller."

#define Al_Err_lGraphicsMeshNoExtension -3071
#define Al_Err_strGraphicsMeshNoExtension "No extension was defined for the graphics mesh."

#define Al_Err_lInvalidSpringForceType -3072
#define Al_Err_strInvalidSpringForceType "Invalid spring force type."

#define Al_Err_lInvalidItemType -3073
#define Al_Err_strInvalidItemType "Invalid Data Item Type."

#define Al_Err_lInvalidMusc_Vel_Avg -3073
#define Al_Err_strInvalidMusc_Vel_Avg "Invalid average muscle velocity count value."

#define Al_Err_lCollisionMeshNotDefined -3074
#define Al_Err_strCollisionMeshNotDefined "No collision mesh was defined for this body."

#define Al_Err_lInavlidPlaneSize -4023
#define Al_Err_strInavlidPlaneSize "Invalid plane size."

#define Al_Err_lInavlidPlaneGridSize -4024
#define Al_Err_strInavlidPlaneGridSize "Invalid plane grid size."

#define Al_Err_lTerrainFileNotDefined -4025
#define Al_Err_strTerrainFileNotDefined "The terrain file is not defined."

#define Al_Err_lIDNotFound -4026
#define Al_Err_strIDNotFound "The specified ID was not found."

#define Al_Err_lNo_Default_Material -4027
#define Al_Err_strNo_Default_Material "The specified ID was not found."

#define Al_Err_lMaterial_Pair_Not_Defined -4028
#define Al_Err_strMaterial_Pair_Not_Defined "Material pair was not defined."

#define Al_Err_lMaterialPairBlank -4029
#define Al_Err_strMaterialPairBlank "A material pair name was blank."

#define Al_Err_lSimWindowNotFound -4030
#define Al_Err_strSimWindowNotFound "The simulation window was not found."

#define Al_Err_lSimWinPosInvalid -4031
#define Al_Err_strSimWinPosInvalid "The simulation window position was invalid."

#define Al_Err_lSimWinSizeInvalid -4032
#define Al_Err_strSimWinSizeInvalid "The simulation window size was invalid."

#define Al_Err_lNodeNotFound -4033
#define Al_Err_strNodeNotFound "The specified node object was not found."

#define Al_Err_lStructureNotFound -4034
#define Al_Err_strStructureNotFound "Structure not found."

#define Al_Err_lItemTypeInvalid -4035
#define Al_Err_strItemTypeInvalid "The specified item type is invalid."

#define Al_Err_lItemNotFound -4036
#define Al_Err_strItemNotFound "The specified item was not found."

#define Al_Err_lNeuralModuleNotFound -4037
#define Al_Err_strNeuralModuleNotFound "The specified neural module was not found."

#define Al_Err_lUnableToCastNeuralModuleToDesiredType -4038
#define Al_Err_strUnableToCastNeuralModuleToDesiredType "Unable to cast neural module to desired type."

#define Al_Err_lUnableToCastOrganismToDesiredType -4039
#define Al_Err_strUnableToCastOrganismToDesiredType "Unable to cast organism to desired type."

#define Al_Err_lUnableToCastNodeToDesiredType -4040
#define Al_Err_strUnableToCastNodeToDesiredType "Unable to cast node to desired type."

#define Al_Err_lUnableToCastLinkToDesiredType -4041
#define Al_Err_strUnableToCastLinkToDesiredType "Unable to cast link to desired type."

#define Al_Err_lInvalidSelMode -4042
#define Al_Err_strInvalidSelMode "Invalid selection mode."

#define Al_Err_lChartNotDefined -4043
#define Al_Err_strChartNotDefined "Chart was not defined."

#define Al_Err_lNodeNotDefined -4044
#define Al_Err_strNodeNotDefined "Node was not defined."

#define Al_Err_lInvalidCurrentType -4045
#define Al_Err_strInvalidCurrentType "An invalid current type was specified."

#define Al_Err_lJointNotMotorized -4046
#define Al_Err_strJointNotMotorized "The specified joint was not motorized."

#define Al_Err_lInvalidLinearType -4047
#define Al_Err_strInvalidLinearType "An invlaid linear joint type was specified."

#define Al_Err_lPartTypeNotStomach -4048
#define Al_Err_strPartTypeNotStomach "Specified part was not a stomach type."

#define Al_Err_lPartTypeNotOdorType -4049
#define Al_Err_strPartTypeNotOdorType "Specified part was not an OdorType part."

#define Al_Err_lAttempedToReadAbsPosBeforeDefined -4050
#define Al_Err_strAttempedToReadAbsPosBeforeDefined "An attempt was made to read the absolute position before the simulation object was defined."

#define Al_Err_lCriticalSimError -4051
#define Al_Err_strCriticalSimError "A critical simulation error has occurred. The application is being shut down."

#define Al_Err_lAdapterIDNotFound -4052
#define Al_Err_strAdapterIDNotFound "The specified adapter ID was not found."

#define Al_Err_lContactSensorExists -4053
#define Al_Err_strContactSensorExists "A contact sensor already exists for the specified rigid body. You must remove that sensor before attempting to add a new one."

#define Al_Err_lReceptiveFieldIDNotFound -4054
#define Al_Err_strReceptiveFieldIDNotFound "The specified receptor ID was not found."

#define Al_Err_lDuplicateAddOfObject -4055
#define Al_Err_strDuplicateAddOfObject "Attempted to add an object with the same ID twice to the simulation."

#define Al_Err_lDefaultMaterialNotFound -4056
#define Al_Err_strDefaultMaterialNotFound "The default material type was not found while loading."

#define Al_Err_lMaterialTypeIDNotFound -4057
#define Al_Err_strMaterialTypeIDNotFound "The material type ID was not found."

#define Al_Err_lMaterialPairIDNotFound -4058
#define Al_Err_strMaterialPairIDNotFound "The material pair ID was not found."

#define Al_Err_lMatrixElementCountInvalid -4059
#define Al_Err_strMatrixElementCountInvalid "The element count for the loaded transformation matrix is incorrect."

#define Al_Err_lMatrixElementCountInvalid -4059
#define Al_Err_strMatrixElementCountInvalid "The element count for the loaded transformation matrix is incorrect."

#define Al_Err_lNoModuleParamOnCommandLine -4060
#define Al_Err_strNoModuleParamOnCommandLine "There was no module parameter defined on the command line."

#define Al_Err_lInvalidPlaybackMode -4061
#define Al_Err_strInvalidPlaybackMode "Invalid plaback control mode specified."

#define Al_Err_lInvalidPresetPlaybackTimeStep -4062
#define Al_Err_strInvalidPresetPlaybackTimeStep "Preset playback time step must be greater than or equal to zero."

#define Al_Err_lLoadingReleaseLib -4063
#define Al_Err_strLoadingReleaseLib "You are attempting to load release libraries while in debug mode."

#define Al_Err_lLoadingDebugLib -4064
#define Al_Err_strLoadingDebugLib "You are attempting to load debug libraries while in release mode."

#define Al_Err_lOpenFile -4065
#define Al_Err_strOpenFile "Unable to open the specified file."

#define Al_Err_lInvalidMuscleLengthCols -4066
#define Al_Err_strInvalidMuscleLengthCols "There can only be 3 items in a muscle length data file: Time, Length, and Velocity"

#define Al_Err_lMuscleLengthDataEmpty -4067
#define Al_Err_strMuscleLengthDataEmpty "There was no data in the muscle length file?"

#define Al_Err_lMuscleLengthTimeStep -4068
#define Al_Err_strMuscleLengthTimeStep "The time step for the predicted muscle length data does not match the time step of the physics engine."

#define Al_Err_lMuscleLengthStartTime -4069
#define Al_Err_strMuscleLengthStartTime "The start and end times for the stimulus and the data file do not match."

#define Al_Err_lTargetObjectNotDefined -4070
#define Al_Err_strTargetObjectNotDefined "Target object was not defined."

#define Al_Err_lPropertyNameBlank -4071
#define Al_Err_strPropertyNameBlank "Target object property name was blank."

#define Al_Err_lTargetDoesNotHaveProperty -4072
#define Al_Err_strTargetDoesNotHaveProperty "Target object does not have a property with specified name."

#define Al_Err_lTargetInvalidPropertyType -4073
#define Al_Err_strTargetInvalidPropertyType "Target object has an invalid property type. Only Boolean, Integer, and Float are allowed to be used for property control."

#define Al_Err_lInvalidSetThreshold -4074
#define Al_Err_strInvalidSetThreshold "Invalid set threshold. It must be greater than or equal to zero."

#define Al_Err_lInvalidRelaxationType -4075
#define Al_Err_strInvalidRelaxationType "An invalid constraint relaxation type was specified."

#define Al_Err_lInvalidFrictionType -4076
#define Al_Err_strInvalidFrictionType "An invalid constraint friction type was specified."

#define Al_Err_lSimFileNotFound -4077
#define Al_Err_strSimFileNotFound "The specified simulation file was not found."

#define Al_Err_lInvalidRobotInterfaceType -4078
#define Al_Err_strInvalidRobotInterfaceType "An invalid robot interface type was specified."

#define Al_Err_lInvalidRobotPartInterfaceType -4079
#define Al_Err_strInvalidRobotPartInterfaceType "An invalid robot part interface type was specified."

#define Al_Err_lAnimatModuleTagNotFound -4080
#define Al_Err_strAnimatModuleTagNotFound "<AnimatModule> tag was not found within the first 1000 characters of the simulation file."

#define MAX_DATA_CHART_BUFFER 10485760


#define ERROR_THRESHOLD 0.0001

#define AL_INFINITY ((float) 0x7fffffff)

//Spring Force Types
#define AL_SPRING_COMPRESSION_ONLY 0
#define AL_SPRING_EXTENSION_ONLY 1
#define AL_SPRING_BOTH 2

#define AL_TONIC_CURRENT 0
#define AL_REPETITIVE_CURRENT 1
#define AL_BURST_CURRENT 2

#define GRAPHICS_SELECTION_MODE 1
#define COLLISION_SELECTION_MODE 2
#define JOINT_SELECTION_MODE 4
#define RECEPTIVE_FIELD_SELECTION_MODE 8
#define SIMULATION_SELECTION_MODE 18

#define PLAYBACK_MODE_FASTEST_POSSIBLE 0
#define PLAYBACK_MODE_MATCH_PHYSICS_STEP 1
#define PLAYBACK_MODE_PRESET_VALUE 2

#define ANIMAT_X_AXIS 0
#define ANIMAT_Y_AXIS 1
#define ANIMAT_Z_AXIS 2

#endif // __ANIMAT_CONSTANTS_H__
