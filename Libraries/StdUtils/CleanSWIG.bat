cd SWIG
mkdir temp
move GenerateSWIG.bat temp
move StdUtils.i temp
erase *.* /Q
move temp\* .
rmdir temp
cd ..

