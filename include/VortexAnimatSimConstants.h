#ifndef __VS_ERROR_CONSTANTS_H__
#define __VS_ERROR_CONSTANTS_H__

//*** Errors ****

#define Vs_Err_lUnableToConvertToVsSimulator -4000
#define Vs_Err_strUnableToConvertToVsSimulator "Unable to convert Simulator to VsSimulator."

#define Vs_Err_lUnableToConvertToVsRigidBody -4001
#define Vs_Err_strUnableToConvertToVsRigidBody "Unable to convert RigidBody to VsRigidBody."

#define Vs_Err_lUnableToConvertToVsJoint -4002
#define Vs_Err_strUnableToConvertToVsJoint "Unable to convert Joint to VsJoint."

#define Vs_Err_lJointToParentNotHingeType -4003
#define Vs_Err_strJointToParentNotHingeType "The joint to the parent is not a hinge type."

#define Vs_Err_lJointConstraintNotDefined -4004
#define Vs_Err_strJointConstraintNotDefined "The joint constraint is not defined."

#define Vs_Err_lLimitIDNotDefined -4006
#define Vs_Err_strLimitIDNotDefined "The limit ID for the joint is not defined."

#define Vs_Err_lUnableToCreateRecorder -4007
#define Vs_Err_strUnableToCreateRecorder "Unable to create the simulation recorder."

#define Vs_Err_lAlreadyHaveActiveRecorder -4008
#define Vs_Err_strAlreadyHaveActiveRecorder "The system is attempting to set an active recorder while another recorder is already active."

#define Vs_Err_lVideoPlaybackNotSupported -4009
#define Vs_Err_strVideoPlaybackNotSupported "Snapshot keyframes do not support video playback capabilities."

#define Vs_Err_lMoveToKeyFrameNotSupported -4010
#define Vs_Err_strMoveToKeyFrameNotSupported "Video keyframes do not support move to key frame capabilities."

#define Vs_Err_lRecorderNotDefined -4011
#define Vs_Err_strRecorderNotDefined "Video recorder is not defined."

#define Vs_Err_lBodyIDNotDefinded -4012
#define Vs_Err_strBodyIDNotDefinded "BodyID is not defined."

#define Vs_Err_lCreatingGeometry -4013
#define Vs_Err_strCreatingGeometry "An error occurred while creating the collision geometry for the speicifed body."

#define Vs_Err_lOSGDrawableNotDefined -4020
#define Vs_Err_strOSGDrawableNotDefined "The OSG Drawable node was note defined before calling VsRigidBody::CreateParts."

#define Vs_Err_lVxGeometryNotDefined -4021
#define Vs_Err_strVxGeometryNotDefined "The Vortex geometry node was note defined before calling VsRigidBody::CreateParts."

#define Vs_Err_lTextureLoad -4022
#define Vs_Err_strTextureLoad "An error occured while loading the texture image file."

#define Vs_Err_lInvalidHudItemType -4023
#define Vs_Err_strInvalidHudItemType "Inavlid Hud Item Type was specified."

#define Vs_Err_lInvalidMaterialItemType -4024
#define Vs_Err_strInvalidMaterialItemType "Inavlid Material Pair Item Type was specified."

#define Vs_Err_lMaterial_Table_Not_Defined -4025
#define Vs_Err_strMaterial_Table_Not_Defined "Material table was not defined."

#define Vs_Err_lUnableToConvertToVsWinMgr -4026
#define Vs_Err_strUnableToConvertToVsWinMgr "Unable to convert the windows manager."

#define Vs_Err_lInvalidSimWindowType -4027
#define Vs_Err_strInvalidSimWindowType "Inavlid Simulation Window Item Type was specified."

#define Vs_Err_lInvalidGraphicsContext -4028
#define Vs_Err_strInvalidGraphicsContext "Error creating a valid graphics context for simulation window."

#define Vs_Err_lUpdateMatricesDoNotMatch -4029
#define Vs_Err_strUpdateMatricesDoNotMatch "Matrices do not match during call to UpdatePositionAndRotationFromMatrix."

#define Vs_Err_lNodeNotGeode -4030
#define Vs_Err_strNodeNotGeode "The specified node was not a geode type."

#define Vs_Err_lGeometryMismatch -4031
#define Vs_Err_strGeometryMismatch "Mismatch between the geometry type."

#define Vs_Err_lRemovingCollisionGeometry -4032
#define Vs_Err_strRemovingCollisionGeometry "Error removing existing collision geometry."

#define Vs_Err_lThisPointerNotDefined -4033
#define Vs_Err_strThisPointerNotDefined "A This pointer was not defined."

#define Vs_Err_lHeightFieldImageNotDefined -4033
#define Vs_Err_strHeightFieldImageNotDefined "An image that is required to load a height field was not loaded."

#define Vs_Err_lMeshOsgNodeGroupNotDefined -4034
#define Vs_Err_strMeshOsgNodeGroupNotDefined "The osg mesh node group was not defined."

#define Vs_Err_lErrorLoadingMesh -4035
#define Vs_Err_strErrorLoadingMesh "An error occured while loading the mesh node."

#define Vs_Err_lHitArrayMismatch -4036
#define Vs_Err_strHitArrayMismatch "A mismatch was found in the size of the ratio and index list returned for a vertex pick operation."

#define Vs_Err_lHudProjectionNotDefined -4037
#define Vs_Err_strHudProjectionNotDefined "The heads-up display projection variable was not defined."

#define Vs_Err_lInvalidLightType -4038
#define Vs_Err_strInvalidLightType "Invalid light type."

#define Vs_Err_lCollisionGeomNotDefined -4039
#define Vs_Err_strCollisionGeomNotDefined "Collision geometry not defined."

#define Vs_Err_lMeshIDNotFound -4040
#define Vs_Err_strMeshNotFound "Mesh filename was not found in the mesh manager."

#define Vs_Err_lUnableToAddWaypoint -4041
#define Vs_Err_strUnableToAddWaypoint "Inavlid waypoint defined. Time overlaps with another waypoint."

#endif // __VS_ERROR_CONSTANTS_H__

