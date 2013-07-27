#ifndef __OSG_ERROR_CONSTANTS_H__
#define __OSG_ERROR_CONSTANTS_H__

//*** Errors ****

#define Osg_Err_lUnableToConvertToVsSimulator -4000
#define Osg_Err_strUnableToConvertToVsSimulator "Unable to convert Simulator to VsSimulator."

#define Osg_Err_lUnableToConvertToVsRigidBody -4001
#define Osg_Err_strUnableToConvertToVsRigidBody "Unable to convert RigidBody to VsRigidBody."

#define Osg_Err_lUnableToConvertToVsJoint -4002
#define Osg_Err_strUnableToConvertToVsJoint "Unable to convert Joint to VsJoint."

#define Osg_Err_lJointToParentNotHingeType -4003
#define Osg_Err_strJointToParentNotHingeType "The joint to the parent is not a hinge type."

#define Osg_Err_lJointConstraintNotDefined -4004
#define Osg_Err_strJointConstraintNotDefined "The joint constraint is not defined."

#define Osg_Err_lLimitIDNotDefined -4006
#define Osg_Err_strLimitIDNotDefined "The limit ID for the joint is not defined."

#define Osg_Err_lUnableToCreateRecorder -4007
#define Osg_Err_strUnableToCreateRecorder "Unable to create the simulation recorder."

#define Osg_Err_lAlreadyHaveActiveRecorder -4008
#define Osg_Err_strAlreadyHaveActiveRecorder "The system is attempting to set an active recorder while another recorder is already active."

#define Osg_Err_lVideoPlaybackNotSupported -4009
#define Osg_Err_strVideoPlaybackNotSupported "Snapshot keyframes do not support video playback capabilities."

#define Osg_Err_lMoveToKeyFrameNotSupported -4010
#define Osg_Err_strMoveToKeyFrameNotSupported "Video keyframes do not support move to key frame capabilities."

#define Osg_Err_lRecorderNotDefined -4011
#define Osg_Err_strRecorderNotDefined "Video recorder is not defined."

#define Osg_Err_lBodyIDNotDefinded -4012
#define Osg_Err_strBodyIDNotDefinded "BodyID is not defined."

#define Osg_Err_lCreatingGeometry -4013
#define Osg_Err_strCreatingGeometry "An error occurred while creating the collision geometry for the speicifed body."

#define Osg_Err_lOSGDrawableNotDefined -4020
#define Osg_Err_strOSGDrawableNotDefined "The OSG Drawable node was note defined before calling VsRigidBody::CreateParts."

#define Osg_Err_lVxGeometryNotDefined -4021
#define Osg_Err_strVxGeometryNotDefined "The Vortex geometry node was note defined before calling VsRigidBody::CreateParts."

#define Osg_Err_lTextureLoad -4022
#define Osg_Err_strTextureLoad "An error occured while loading the texture image file."

#define Osg_Err_lInvalidHudItemType -4023
#define Osg_Err_strInvalidHudItemType "Inavlid Hud Item Type was specified."

#define Osg_Err_lInvalidMaterialItemType -4024
#define Osg_Err_strInvalidMaterialItemType "Inavlid Material Pair Item Type was specified."

#define Osg_Err_lMaterial_Table_Not_Defined -4025
#define Osg_Err_strMaterial_Table_Not_Defined "Material table was not defined."

#define Osg_Err_lUnableToConvertToVsWinMgr -4026
#define Osg_Err_strUnableToConvertToVsWinMgr "Unable to convert the windows manager."

#define Osg_Err_lInvalidSimWindowType -4027
#define Osg_Err_strInvalidSimWindowType "Inavlid Simulation Window Item Type was specified."

#define Osg_Err_lInvalidGraphicsContext -4028
#define Osg_Err_strInvalidGraphicsContext "Error creating a valid graphics context for simulation window."

#define Osg_Err_lUpdateMatricesDoNotMatch -4029
#define Osg_Err_strUpdateMatricesDoNotMatch "Matrices do not match during call to UpdatePositionAndRotationFromMatrix."

#define Osg_Err_lNodeNotGeode -4030
#define Osg_Err_strNodeNotGeode "The specified node was not a geode type."

#define Osg_Err_lGeometryMismatch -4031
#define Osg_Err_strGeometryMismatch "Mismatch between the geometry type."

#define Osg_Err_lRemovingCollisionGeometry -4032
#define Osg_Err_strRemovingCollisionGeometry "Error removing existing collision geometry."

#define Osg_Err_lThisPointerNotDefined -4033
#define Osg_Err_strThisPointerNotDefined "A This pointer was not defined."

#define Osg_Err_lHeightFieldImageNotDefined -4033
#define Osg_Err_strHeightFieldImageNotDefined "An image that is required to load a height field was not loaded."

#define Osg_Err_lMeshOsgNodeGroupNotDefined -4034
#define Osg_Err_strMeshOsgNodeGroupNotDefined "The osg mesh node group was not defined."

#define Osg_Err_lErrorLoadingMesh -4035
#define Osg_Err_strErrorLoadingMesh "An error occured while loading the mesh node."

#define Osg_Err_lHitArrayMismatch -4036
#define Osg_Err_strHitArrayMismatch "A mismatch was found in the size of the ratio and index list returned for a vertex pick operation."

#define Osg_Err_lHudProjectionNotDefined -4037
#define Osg_Err_strHudProjectionNotDefined "The heads-up display projection variable was not defined."

#define Osg_Err_lInvalidLightType -4038
#define Osg_Err_strInvalidLightType "Invalid light type."

#define Osg_Err_lCollisionGeomNotDefined -4039
#define Osg_Err_strCollisionGeomNotDefined "Collision geometry not defined."

#define Osg_Err_lMeshIDNotFound -4040
#define Osg_Err_strMeshNotFound "Mesh filename was not found in the mesh manager."

#define Osg_Err_lUnableToAddWaypoint -4041
#define Osg_Err_strUnableToAddWaypoint "Inavlid waypoint defined. Time overlaps with another waypoint."

#endif // __OSG_ERROR_CONSTANTS_H__

