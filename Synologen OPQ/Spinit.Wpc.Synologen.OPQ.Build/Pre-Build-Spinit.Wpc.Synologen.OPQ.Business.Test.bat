ECHO "User is: " %USERNAME%
xcopy %1..\Spinit.Wpc.Synologen.OPQ.Config\%USERNAME%\%2\*.config %1..\%2\ /R /K /Y /Q /S
xcopy %1..\Spinit.Wpc.Synologen.OPQ.Config\%USERNAME%\%2\Properties\*.cs %1..\%2\Properties\ /R /K /Y /Q /S
xcopy %1..\Spinit.Wpc.Synologen.OPQ.Config\%USERNAME%\%2\Properties\*.settings %1..\%2\Properties\ /R /K /Y /Q /S

