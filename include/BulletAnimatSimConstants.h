#ifndef __BL_ERROR_CONSTANTS_H__
#define __BL_ERROR_CONSTANTS_H__

//*** Errors ****

#define Bl_Err_lUnableToConvertToBlSimulator -4000
#define Bl_Err_strUnableToConvertToBlSimulator "Unable to convert Simulator to BlSimulator."

#define Bl_Err_lUnableToConvertToBlRigidBody -4001
#define Bl_Err_strUnableToConvertToBlRigidBody "Unable to convert RigidBody to BlRigidBody."

#define Bl_Err_lUnableToConvertToBlJoint -4002
#define Bl_Err_strUnableToConvertToBlJoint "Unable to convert Joint to BlJoint."

#define Bl_Err_lJointToParentNotHingeType -4003
#define Bl_Err_strJointToParentNotHingeType "The joint to the parent is not a hinge type."

#define Bl_Err_lJointConstraintNotDefined -4004
#define Bl_Err_strJointConstraintNotDefined "The joint constraint is not defined."

#define Bl_Err_lLimitIDNotDefined -4006
#define Bl_Err_strLimitIDNotDefined "The limit ID for the joint is not defined."

#define Bl_Err_lUnableToCreateRecorder -4007
#define Bl_Err_strUnableToCreateRecorder "Unable to create the simulation recorder."

#define Bl_Err_lAlreadyHaveActiveRecorder -4008
#define Bl_Err_strAlreadyHaveActiveRecorder "The system is attempting to set an active recorder while another recorder is already active."

#define Bl_Err_lVideoPlaybackNotSupported -4009
#define Bl_Err_strVideoPlaybackNotSupported "Snapshot keyframes do not support video playback capabilities."

#define Bl_Err_lMoveToKeyFrameNotSupported -4010
#define Bl_Err_strMoveToKeyFrameNotSupported "Video keyframes do not support move to key frame capabilities."

#define Bl_Err_lRecorderNotDefined -4011
#define Bl_Err_strRecorderNotDefined "Video recorder is not defined."

#define Bl_Err_lBodyIDNotDefinded -4012
#define Bl_Err_strBodyIDNotDefinded "BodyID is not defined."

#define Bl_Err_lCreatingGeometry -4013
#define Bl_Err_strCreatingGeometry "An error occurred while creating the collision geometry for the speicifed body."

#define Bl_Err_lOSGDrawableNotDefined -4020
#define Bl_Err_strOSGDrawableNotDefined "The OSG Drawable node was note defined before calling BlRigidBody::CreateParts."

#define Bl_Err_lVxGeometryNotDefined -4021
#define Bl_Err_strVxGeometryNotDefined "The Vortex geometry node was note defined before calling BlRigidBody::CreateParts."

#define Bl_Err_lTextureLoad -4022
#define Bl_Err_strTextureLoad "An error occured while loading the texture image file."

#define Bl_Err_lInvalidHudItemType -4023
#define Bl_Err_strInvalidHudItemType "Inavlid Hud Item Type was specified."

#define Bl_Err_lInvalidMaterialItemType -4024
#define Bl_Err_strInvalidMaterialItemType "Inavlid Material Pair Item Type was specified."

#define Bl_Err_lMaterial_Table_Not_Defined -4025
#define Bl_Err_strMaterial_Table_Not_Defined "Material table was not defined."

#define Bl_Err_lUnableToConvertToVsWinMgr -4026
#define Bl_Err_strUnableToConvertToVsWinMgr "Unable to convert the windows manager."

#define Bl_Err_lInvalidSimWindowType -4027
#define Bl_Err_strInvalidSimWindowType "Inavlid Simulation Window Item Type was specified."

#define Bl_Err_lInvalidGraphicsContext -4028
#define Bl_Err_strInvalidGraphicsContext "Error creating a valid graphics context for simulation window."

#define Bl_Err_lUpdateMatricesDoNotMatch -4029
#define Bl_Err_strUpdateMatricesDoNotMatch "Matrices do not match during call to UpdatePositionAndRotationFromMatrix."

#define Bl_Err_lNodeNotGeode -4030
#define Bl_Err_strNodeNotGeode "The specified node was not a geode type."

#define Bl_Err_lGeometryMismatch -4031
#define Bl_Err_strGeometryMismatch "Mismatch between the geometry type."

#define Bl_Err_lRemovingCollisionGeometry -4032
#define Bl_Err_strRemovingCollisionGeometry "Error removing existing collision geometry."

#define Bl_Err_lThisPointerNotDefined -4033
#define Bl_Err_strThisPointerNotDefined "A This pointer was not defined."

#define Bl_Err_lHeightFieldImageNotDefined -4033
#define Bl_Err_strHeightFieldImageNotDefined "An image that is required to load a height field was not loaded."

#define Bl_Err_lMeshOsgNodeGroupNotDefined -4034
#define Bl_Err_strMeshOsgNodeGroupNotDefined "The osg mesh node group was not defined."

#define Bl_Err_lErrorLoadingMesh -4035
#define Bl_Err_strErrorLoadingMesh "An error occured while loading the mesh node."

#define Bl_Err_lHitArrayMismatch -4036
#define Bl_Err_strHitArrayMismatch "A mismatch was found in the size of the ratio and index list returned for a vertex pick operation."

#define Bl_Err_lHudProjectionNotDefined -4037
#define Bl_Err_strHudProjectionNotDefined "The heads-up display projection variable was not defined."

#define Bl_Err_lInvalidLightType -4038
#define Bl_Err_strInvalidLightType "Invalid light type."

#define Bl_Err_lCollisionGeomNotDefined -4039
#define Bl_Err_strCollisionGeomNotDefined "Collision geometry not defined."

#define Bl_Err_lMeshIDNotFound -4040
#define Bl_Err_strMeshNotFound "Mesh filename was not found in the mesh manager."

#define Bl_Err_lUnableToAddWaypoint -4041
#define Bl_Err_strUnableToAddWaypoint "Inavlid waypoint defined. Time overlaps with another waypoint."

#define Bl_Err_lTriangleCollisionMeshNotAllowed -4042
#define Bl_Err_strTriangleCollisionMeshNotAllowed "Dynamic triangle mesh collision objects are not allowed."

#define Bl_Err_lConvertingMeshToConvexHull -4043
#define Bl_Err_strConvertingMeshToConvexHull "Error converting a mesh to a convex hull."

enum AnimatCollisionTypes {
    NOTHING                 = 0,    // things that don't collide
    RIGID_BODY              = 1<<1, // regular rigid body party
    CONTACT_SENSOR          = 1<<2,  // contact sensor part type
    RECEPTIVE_FIELD_SENSOR  = 1<<3  // receptive field part type 
};

#define ALL_COLLISIONS (RIGID_BODY | CONTACT_SENSOR | RECEPTIVE_FIELD_SENSOR)

#endif // __BL_ERROR_CONSTANTS_H__

