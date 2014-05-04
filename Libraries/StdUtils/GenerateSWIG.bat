swig -c++ -csharp StdUtils.i 

del "..\AnimatSimCSharpInterface\*.cs"

xcopy "*.cs" "..\AnimatSimCSharpInterface" /E /Y /R

del *.cs