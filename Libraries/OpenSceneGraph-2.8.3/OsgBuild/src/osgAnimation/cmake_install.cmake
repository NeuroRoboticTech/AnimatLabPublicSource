# Install script for directory: C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/src/osgAnimation

# Set the install prefix
IF(NOT DEFINED CMAKE_INSTALL_PREFIX)
  SET(CMAKE_INSTALL_PREFIX "C:/Program Files (x86)/OpenSceneGraph")
ENDIF(NOT DEFINED CMAKE_INSTALL_PREFIX)
STRING(REGEX REPLACE "/$" "" CMAKE_INSTALL_PREFIX "${CMAKE_INSTALL_PREFIX}")

# Set the install configuration name.
IF(NOT DEFINED CMAKE_INSTALL_CONFIG_NAME)
  IF(BUILD_TYPE)
    STRING(REGEX REPLACE "^[^A-Za-z0-9_]+" ""
           CMAKE_INSTALL_CONFIG_NAME "${BUILD_TYPE}")
  ELSE(BUILD_TYPE)
    SET(CMAKE_INSTALL_CONFIG_NAME "Release")
  ENDIF(BUILD_TYPE)
  MESSAGE(STATUS "Install configuration: \"${CMAKE_INSTALL_CONFIG_NAME}\"")
ENDIF(NOT DEFINED CMAKE_INSTALL_CONFIG_NAME)

# Set the component getting installed.
IF(NOT CMAKE_INSTALL_COMPONENT)
  IF(COMPONENT)
    MESSAGE(STATUS "Install component: \"${COMPONENT}\"")
    SET(CMAKE_INSTALL_COMPONENT "${COMPONENT}")
  ELSE(COMPONENT)
    SET(CMAKE_INSTALL_COMPONENT)
  ENDIF(COMPONENT)
ENDIF(NOT CMAKE_INSTALL_COMPONENT)

IF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph-dev")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Dd][Ee][Bb][Uu][Gg])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY OPTIONAL FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/lib/Debug/../osgAnimationd.lib")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Dd][Ee][Bb][Uu][Gg])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY OPTIONAL FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/lib/Release/../osgAnimation.lib")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY OPTIONAL FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/lib/MinSizeRel/../osgAnimation.lib")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY OPTIONAL FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/lib/RelWithDebInfo/../osgAnimation.lib")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
ENDIF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph-dev")

IF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Dd][Ee][Bb][Uu][Gg])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE SHARED_LIBRARY FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/bin/Debug/../../bin/osg65-osgAnimationd.dll")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Dd][Ee][Bb][Uu][Gg])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE SHARED_LIBRARY FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/bin/Release/../../bin/osg65-osgAnimation.dll")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE SHARED_LIBRARY FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/bin/MinSizeRel/../../bin/osg65-osgAnimation.dll")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE SHARED_LIBRARY FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/bin/RelWithDebInfo/../../bin/osg65-osgAnimation.dll")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
ENDIF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph")

IF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph-dev")
  FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/osgAnimation" TYPE FILE FILES
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/Action"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/ActionAnimation"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/ActionBlendIn"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/ActionBlendOut"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/ActionCallback"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/ActionStripAnimation"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/ActionVisitor"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/Animation"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/AnimationManagerBase"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/AnimationUpdateCallback"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/BasicAnimationManager"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/Bone"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/BoneMapVisitor"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/Channel"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/CubicBezier"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/EaseMotion"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/Export"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/FrameAction"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/Interpolator"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/Keyframe"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/LinkVisitor"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/MorphGeometry"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/RigGeometry"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/RigTransform"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/RigTransformHardware"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/RigTransformSoftware"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/Sampler"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/Skeleton"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/StackedMatrixElement"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/StackedQuaternionElement"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/StackedRotateAxisElement"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/StackedScaleElement"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/StackedTransformElement"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/StackedTranslateElement"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/StackedTransform"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/StatsVisitor"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/StatsHandler"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/Target"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/Timeline"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/TimelineAnimationManager"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/UpdateBone"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/UpdateMaterial"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/UpdateMatrixTransform"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/Vec3Packed"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgAnimation/VertexInfluence"
    )
ENDIF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph-dev")

