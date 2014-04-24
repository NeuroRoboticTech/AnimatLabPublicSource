#ifndef __RB_ERROR_CONSTANTS_H__
#define __RB_ERROR_CONSTANTS_H__

//*** Errors ****

#define Rb_Err_lUnableToConvertToRbSimulator -4000
#define Rb_Err_strUnableToConvertToRbSimulator "Unable to convert Simulator to RbSimulator."

#define Rb_Err_lUnableToConvertToRbRigidBody -4001
#define Rb_Err_strUnableToConvertToRbRigidBody "Unable to convert RigidBody to RbRigidBody."

#define Rb_Err_lUnableToConvertToRbJoint -4002
#define Rb_Err_strUnableToConvertToRbJoint "Unable to convert Joint to RbJoint."

#define Rb_Err_lJointToParentNotHingeType -4003
#define Rb_Err_strJointToParentNotHingeType "The joint to the parent is not a hinge type."

#define Rb_Err_lJointConstraintNotDefined -4004
#define Rb_Err_strJointConstraintNotDefined "The joint constraint is not defined."

#define Rb_Err_lLimitIDNotDefined -4006
#define Rb_Err_strLimitIDNotDefined "The limit ID for the joint is not defined."

#define Rb_Err_lUnableToCreateRecorder -4007
#define Rb_Err_strUnableToCreateRecorder "Unable to create the simulation recorder."

#define Rb_Err_lAlreadyHaveActiveRecorder -4008
#define Rb_Err_strAlreadyHaveActiveRecorder "The system is attempting to set an active recorder while another recorder is already active."

#define Rb_Err_lVideoPlaybackNotSupported -4009
#define Rb_Err_strVideoPlaybackNotSupported "Snapshot keyframes do not support video playback capabilities."

#define Rb_Err_lMoveToKeyFrameNotSupported -4010
#define Rb_Err_strMoveToKeyFrameNotSupported "Video keyframes do not support move to key frame capabilities."

#define Rb_Err_lRecorderNotDefined -4011
#define Rb_Err_strRecorderNotDefined "Video recorder is not defined."

#define Rb_Err_lBodyIDNotDefinded -4012
#define Rb_Err_strBodyIDNotDefinded "BodyID is not defined."

#define Rb_Err_lCreatingGeometry -4013
#define Rb_Err_strCreatingGeometry "An error occurred while creating the collision geometry for the speicifed body."

#define Rb_Err_lOSGDrawableNotDefined -4020
#define Rb_Err_strOSGDrawableNotDefined "The OSG Drawable node was note defined before calling RbRigidBody::CreateParts."

#define Rb_Err_lVxGeometryNotDefined -4021
#define Rb_Err_strVxGeometryNotDefined "The Vortex geometry node was note defined before calling RbRigidBody::CreateParts."

#define Rb_Err_lTextureLoad -4022
#define Rb_Err_strTextureLoad "An error occured while loading the texture image file."

#define Rb_Err_lInvalidHudItemType -4023
#define Rb_Err_strInvalidHudItemType "Inavlid Hud Item Type was specified."

#define Rb_Err_lInvalidMaterialItemType -4024
#define Rb_Err_strInvalidMaterialItemType "Inavlid Material Pair Item Type was specified."

#define Rb_Err_lMaterial_Table_Not_Defined -4025
#define Rb_Err_strMaterial_Table_Not_Defined "Material table was not defined."

#define Rb_Err_lUnableToConvertToVsWinMgr -4026
#define Rb_Err_strUnableToConvertToVsWinMgr "Unable to convert the windows manager."

#define Rb_Err_lInvalidSimWindowType -4027
#define Rb_Err_strInvalidSimWindowType "Inavlid Simulation Window Item Type was specified."

#define Rb_Err_lInvalidGraphicsContext -4028
#define Rb_Err_strInvalidGraphicsContext "Error creating a valid graphics context for simulation window."

#define Rb_Err_lUpdateMatricesDoNotMatch -4029
#define Rb_Err_strUpdateMatricesDoNotMatch "Matrices do not match during call to UpdatePositionAndRotationFromMatrix."

#define Rb_Err_lNodeNotGeode -4030
#define Rb_Err_strNodeNotGeode "The specified node was not a geode type."

#define Rb_Err_lGeometryMismatch -4031
#define Rb_Err_strGeometryMismatch "Mismatch between the geometry type."

#define Rb_Err_lRemovingCollisionGeometry -4032
#define Rb_Err_strRemovingCollisionGeometry "Error removing existing collision geometry."

#define Rb_Err_lThisPointerNotDefined -4033
#define Rb_Err_strThisPointerNotDefined "A This pointer was not defined."

#define Rb_Err_lHeightFieldImageNotDefined -4033
#define Rb_Err_strHeightFieldImageNotDefined "An image that is required to load a height field was not loaded."

#define Rb_Err_lMeshRbNodeGroupNotDefined -4034
#define Rb_Err_strMeshRbNodeGroupNotDefined "The osg mesh node group was not defined."

#define Rb_Err_lErrorLoadingMesh -4035
#define Rb_Err_strErrorLoadingMesh "An error occured while loading the mesh node."

#define Rb_Err_lHitArrayMismatch -4036
#define Rb_Err_strHitArrayMismatch "A mismatch was found in the size of the ratio and index list returned for a vertex pick operation."

#define Rb_Err_lHudProjectionNotDefined -4037
#define Rb_Err_strHudProjectionNotDefined "The heads-up display projection variable was not defined."

#define Rb_Err_lInvalidLightType -4038
#define Rb_Err_strInvalidLightType "Invalid light type."

#define Rb_Err_lCollisionGeomNotDefined -4039
#define Rb_Err_strCollisionGeomNotDefined "Collision geometry not defined."

#define Rb_Err_lMeshIDNotFound -4040
#define Rb_Err_strMeshNotFound "Mesh filename was not found in the mesh manager."

#define Rb_Err_lUnableToAddWaypoint -4041
#define Rb_Err_strUnableToAddWaypoint "Inavlid waypoint defined. Time overlaps with another waypoint."

#define Rb_Err_lTriangleCollisionMeshNotAllowed -4042
#define Rb_Err_strTriangleCollisionMeshNotAllowed "Dynamic triangle mesh collision objects are not allowed."

#define Rb_Err_lConvertingMeshToConvexHull -4043
#define Rb_Err_strConvertingMeshToConvexHull "Error converting a mesh to a convex hull."

#define Rb_Err_lConvertingMaterialType -4044
#define Rb_Err_strConvertingMaterialType "Error converting a material type to bullet material type."


#define Rb_Err_lUnableToConvertToVsSimulator -4000
#define Rb_Err_strUnableToConvertToVsSimulator "Unable to convert Simulator to VsSimulator."

#define Rb_Err_lUnableToConvertToVsRigidBody -4001
#define Rb_Err_strUnableToConvertToVsRigidBody "Unable to convert RigidBody to VsRigidBody."

#define Rb_Err_lUnableToConvertToVsJoint -4002
#define Rb_Err_strUnableToConvertToVsJoint "Unable to convert Joint to VsJoint."

#define Rb_Err_lJointToParentNotHingeType -4003
#define Rb_Err_strJointToParentNotHingeType "The joint to the parent is not a hinge type."

#define Rb_Err_lJointConstraintNotDefined -4004
#define Rb_Err_strJointConstraintNotDefined "The joint constraint is not defined."

#define Rb_Err_lLimitIDNotDefined -4006
#define Rb_Err_strLimitIDNotDefined "The limit ID for the joint is not defined."

#define Rb_Err_lUnableToCreateRecorder -4007
#define Rb_Err_strUnableToCreateRecorder "Unable to create the simulation recorder."

#define Rb_Err_lAlreadyHaveActiveRecorder -4008
#define Rb_Err_strAlreadyHaveActiveRecorder "The system is attempting to set an active recorder while another recorder is already active."

#define Rb_Err_lVideoPlaybackNotSupported -4009
#define Rb_Err_strVideoPlaybackNotSupported "Snapshot keyframes do not support video playback capabilities."

#define Rb_Err_lMoveToKeyFrameNotSupported -4010
#define Rb_Err_strMoveToKeyFrameNotSupported "Video keyframes do not support move to key frame capabilities."

#define Rb_Err_lRecorderNotDefined -4011
#define Rb_Err_strRecorderNotDefined "Video recorder is not defined."

#define Rb_Err_lBodyIDNotDefinded -4012
#define Rb_Err_strBodyIDNotDefinded "BodyID is not defined."

#define Rb_Err_lCreatingGeometry -4013
#define Rb_Err_strCreatingGeometry "An error occurred while creating the collision geometry for the speicifed body."

#define Rb_Err_lTextureLoad -4022
#define Rb_Err_strTextureLoad "An error occured while loading the texture image file."

#define Rb_Err_lInvalidHudItemType -4023
#define Rb_Err_strInvalidHudItemType "Inavlid Hud Item Type was specified."

#define Rb_Err_lInvalidMaterialItemType -4024
#define Rb_Err_strInvalidMaterialItemType "Inavlid Material Pair Item Type was specified."

#define Rb_Err_lMaterial_Table_Not_Defined -4025
#define Rb_Err_strMaterial_Table_Not_Defined "Material table was not defined."

#define Rb_Err_lUnableToConvertToVsWinMgr -4026
#define Rb_Err_strUnableToConvertToVsWinMgr "Unable to convert the windows manager."

#define Rb_Err_lInvalidSimWindowType -4027
#define Rb_Err_strInvalidSimWindowType "Inavlid Simulation Window Item Type was specified."

#define Rb_Err_lInvalidGraphicsContext -4028
#define Rb_Err_strInvalidGraphicsContext "Error creating a valid graphics context for simulation window."

#define Rb_Err_lUpdateMatricesDoNotMatch -4029
#define Rb_Err_strUpdateMatricesDoNotMatch "Matrices do not match during call to UpdatePositionAndRotationFromMatrix."

#define Rb_Err_lNodeNotGeode -4030
#define Rb_Err_strNodeNotGeode "The specified node was not a geode type."

#define Rb_Err_lGeometryMismatch -4031
#define Rb_Err_strGeometryMismatch "Mismatch between the geometry type."

#define Rb_Err_lRemovingCollisionGeometry -4032
#define Rb_Err_strRemovingCollisionGeometry "Error removing existing collision geometry."

#define Rb_Err_lThisPointerNotDefined -4033
#define Rb_Err_strThisPointerNotDefined "A This pointer was not defined."

#define Rb_Err_lHeightFieldImageNotDefined -4033
#define Rb_Err_strHeightFieldImageNotDefined "An image that is required to load a height field was not loaded."

#define Rb_Err_lMeshRbNodeGroupNotDefined -4034
#define Rb_Err_strMeshRbNodeGroupNotDefined "The osg mesh node group was not defined."

#define Rb_Err_lErrorLoadingMesh -4035
#define Rb_Err_strErrorLoadingMesh "An error occured while loading the mesh node."

#define Rb_Err_lHitArrayMismatch -4036
#define Rb_Err_strHitArrayMismatch "A mismatch was found in the size of the ratio and index list returned for a vertex pick operation."

#define Rb_Err_lHudProjectionNotDefined -4037
#define Rb_Err_strHudProjectionNotDefined "The heads-up display projection variable was not defined."

#define Rb_Err_lInvalidLightType -4038
#define Rb_Err_strInvalidLightType "Invalid light type."

#define Rb_Err_lCollisionGeomNotDefined -4039
#define Rb_Err_strCollisionGeomNotDefined "Collision geometry not defined."

#define Rb_Err_lMeshIDNotFound -4040
#define Rb_Err_strMeshNotFound "Mesh filename was not found in the mesh manager."

#define Rb_Err_lUnableToAddWaypoint -4041
#define Rb_Err_strUnableToAddWaypoint "Inavlid waypoint defined. Time overlaps with another waypoint."

#define Rb_Err_lMatrixUtilNotDefined -4042
#define Rb_Err_strMatrixUtilNotDefined "Matrix utility was not defined."



#define Rb_Err_lInvalidBaudRate -4043
#define Rb_Err_strInvalidBaudRate "Invalid baud rate specified."

#define Rb_Err_lFailedDynamixelConnection -4044
#define Rb_Err_strFailedDynamixelConnection "Failed to connect to the Dynamixel."


#define RB_PI 3.14159

#endif // __RB_ERROR_CONSTANTS_H__

