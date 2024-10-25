if exist Routines (cd ./Routines) else (goto exception1)
if exist .git (echo ok) else (goto init)
git fetch --all  
git reset origin/master --hard
git config --global core.quotepath false
git config --global gui.encoding utf-8
git config --global i18n.commit.encoding utf-8
git config --global i18n.logoutputencoding utf-8
set LESSCHARSET=utf-8
git log --pretty=format:"%%an %%x09 %%ad %%x09 %%s %%x09" -5 --date=format:"%%y-%%m-%%d %%H:%%M:%%S"
echo Done
pause
exit
:init 
echo init.......
git init .
ping 127.0.0.1 -n 3 >nul
if exist .git (echo Git intalled) else (goto exception2)
cd ..
RD /S /Q Routines
git clone https://gitee.com/notnow/hearthstoneRoutine.git ./Routines
if exist Routines (echo finished) else (echo Install Git First!)
pause
exit
:exception1
echo ERROR
pause
exit
:exception2
echo  Install Git First!
pause
exit