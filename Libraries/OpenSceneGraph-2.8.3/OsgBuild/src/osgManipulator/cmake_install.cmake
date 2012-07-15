# Install script for directory: C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/src/osgManipulator

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
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY OPTIONAL FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/lib/Debug/../osgManipulatord.lib")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Dd][Ee][Bb][Uu][Gg])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY OPTIONAL FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/lib/Release/../osgManipulator.lib")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY OPTIONAL FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/lib/MinSizeRel/../osgManipulator.lib")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY OPTIONAL FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/lib/RelWithDebInfo/../osgManipulator.lib")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
ENDIF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph-dev")

IF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Dd][Ee][Bb][Uu][Gg])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE SHARED_LIBRARY FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/bin/Debug/../../bin/osg65-osgManipulatord.dll")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Dd][Ee][Bb][Uu][Gg])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE SHARED_LIBRARY FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/bin/Release/../../bin/osg65-osgManipulator.dll")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE SHARED_LIBRARY FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/bin/MinSizeRel/../../bin/osg65-osgManipulator.dll")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE SHARED_LIBRARY FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/bin/RelWithDebInfo/../../bin/osg65-osgManipulator.dll")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
ENDIF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph")

IF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph-dev")
  FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/osgManipulator" TYPE FILE FILES
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/AntiSquish"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/Command"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/CommandManager"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/Constraint"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/Dragger"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/Export"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/Projector"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/RotateCylinderDragger"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/RotateSphereDragger"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/Scale1DDragger"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/Scale2DDragger"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/ScaleAxisDragger"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/Selection"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/TabBoxDragger"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/TabPlaneDragger"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/TabPlaneTrackballDragger"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/TrackballDragger"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/Translate1DDragger"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/Translate2DDragger"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/TranslateAxisDragger"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/TranslatePlaneDragger"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgManipulator/Version"
    )
ENDIF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph-dev")

