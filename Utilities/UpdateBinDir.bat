@set AnimatLab-bin=..\..\AnimatLabPublicSource\bin
@set OsgBullet=..\..\3rdParty\osgBullet_03_00_00\Build\bin
@set OsgWorks=..\..\3rdParty\osgWorks_03_00_00\Build\bin
@set Osg=..\..\3rdParty\OpenSceneGraph-3.0.1\bin

@mkdir %AnimatLab-bin%\osgPlugins-3.0.1

xcopy "%Osg%\osgPlugins-3.0.1\*.dll" "%AnimatLab-bin%\osgPlugins-3.0.1" /E /Y /R
xcopy "%Osg%\*.dll" "%AnimatLab-bin%" /E /Y /R

xcopy "%OsgWorks%\Debug\*.dll" "%AnimatLab-bin%" /E /Y /R
xcopy "%OsgWorks%\Debug\*.dll" "%AnimatLab-bin%" /E /Y /R
xcopy "%OsgWorks%\Release\*.dll" "%AnimatLab-bin%" /E /Y /R
xcopy "%OsgBullet%\Debug\*.dll" "%AnimatLab-bin%" /E /Y /R
xcopy "%OsgBullet%\Release\*.dll" "%AnimatLab-bin%" /E /Y /R
