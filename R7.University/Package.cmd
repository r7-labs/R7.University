@echo off
setlocal
pushd

set Z7="C:\Program Files\7-Zip\7z.exe"
set PACKAGE=tmp_Package

cd ..

rmdir /Q /S %PACKAGE%
mkdir %PACKAGE%\bin

cd R7.University.Employee
%Z7% a ..\%PACKAGE%\R7.University.Employee.zip *.ascx *.css *.js App_LocalResources\*.resx js 
cd ..
cd R7.University.EmployeeList
%Z7% a ..\%PACKAGE%\R7.University.EmployeeList.zip *.ascx *.css *.js App_LocalResources\*.resx js 
cd ..
cd R7.University.Launchpad
%Z7% a ..\%PACKAGE%\R7.University.Launchpad.zip *.ascx *.css *.js App_LocalResources\*.resx js 
cd ..
cd R7.University

xcopy /Y *.SqlDataProvider ..\%PACKAGE%\
xcopy /Y *.dnn ..\%PACKAGE%\
xcopy /Y *.txt ..\%PACKAGE%\

cd ..
xcopy /Y ..\..\bin\R7.University*.dll %PACKAGE%\bin\

cd %PACKAGE%
%Z7% a ..\R7.University-%1-Install.zip *

cd ..
rmdir /Q /S %PACKAGE%

popd
endlocal