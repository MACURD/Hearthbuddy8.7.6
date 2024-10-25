CHCP 65001
if not exist Routines (cd ../)
del /f /s /q myLog.txt
del /f /s /q Logs\*.*
del /f /s /q Routines\DefaultRoutine\Silverfish\UltimateLogs\*.*
del /f /s /q Routines\DefaultRoutine\Silverfish\Test\Data\对局记录\*.*
pause