# Install script for directory: C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/src/osgSim

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
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY OPTIONAL FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/lib/Debug/../osgSimd.lib")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Dd][Ee][Bb][Uu][Gg])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY OPTIONAL FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/lib/Release/../osgSim.lib")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY OPTIONAL FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/lib/MinSizeRel/../osgSim.lib")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY OPTIONAL FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/lib/RelWithDebInfo/../osgSim.lib")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
ENDIF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph-dev")

IF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Dd][Ee][Bb][Uu][Gg])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE SHARED_LIBRARY FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/bin/Debug/../../bin/osg65-osgSimd.dll")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Dd][Ee][Bb][Uu][Gg])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE SHARED_LIBRARY FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/bin/Release/../../bin/osg65-osgSim.dll")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE SHARED_LIBRARY FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/bin/MinSizeRel/../../bin/osg65-osgSim.dll")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE SHARED_LIBRARY FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/bin/RelWithDebInfo/../../bin/osg65-osgSim.dll")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
ENDIF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph")

IF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph-dev")
  FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/osgSim" TYPE FILE FILES
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/BlinkSequence"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/ColorRange"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/DOFTransform"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/ElevationSlice"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/HeightAboveTerrain"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/Export"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/GeographicLocation"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/Impostor"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/ImpostorSprite"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/InsertImpostorsVisitor"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/LightPoint"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/LightPointNode"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/LightPointSystem"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/LineOfSight"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/MultiSwitch"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/OverlayNode"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/ObjectRecordData"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/ScalarBar"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/ScalarsToColors"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/Sector"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/ShapeAttribute"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/SphereSegment"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/Version"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgSim/VisibilityGroup"
    )
ENDIF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph-dev")

