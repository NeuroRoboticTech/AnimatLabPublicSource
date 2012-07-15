# Install script for directory: C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/src/osgFX

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
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY OPTIONAL FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/lib/Debug/../osgFXd.lib")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Dd][Ee][Bb][Uu][Gg])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY OPTIONAL FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/lib/Release/../osgFX.lib")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY OPTIONAL FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/lib/MinSizeRel/../osgFX.lib")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib" TYPE STATIC_LIBRARY OPTIONAL FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/lib/RelWithDebInfo/../osgFX.lib")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
ENDIF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph-dev")

IF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Dd][Ee][Bb][Uu][Gg])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE SHARED_LIBRARY FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/bin/Debug/../../bin/osg65-osgFXd.dll")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Dd][Ee][Bb][Uu][Gg])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE SHARED_LIBRARY FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/bin/Release/../../bin/osg65-osgFX.dll")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE SHARED_LIBRARY FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/bin/MinSizeRel/../../bin/osg65-osgFX.dll")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
  IF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
    FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE SHARED_LIBRARY FILES "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/OsgBuild/bin/RelWithDebInfo/../../bin/osg65-osgFX.dll")
  ENDIF("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
ENDIF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph")

IF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph-dev")
  FILE(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/osgFX" TYPE FILE FILES
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgFX/AnisotropicLighting"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgFX/BumpMapping"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgFX/Cartoon"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgFX/Effect"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgFX/Export"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgFX/MultiTextureControl"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgFX/Registry"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgFX/Scribe"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgFX/SpecularHighlights"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgFX/Technique"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgFX/Validator"
    "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/OpenSceneGraph-2.8.3/include/osgFX/Version"
    )
ENDIF(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "libopenscenegraph-dev")

