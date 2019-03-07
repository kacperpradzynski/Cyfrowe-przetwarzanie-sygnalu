## Co tutaj się dzieje :D
Aktualnie program odpala się skryptem callName.py.<br />
Robiłem na Pythonie 3.6 nie wiem czy na 3.7 będzie działać.<br />
Qt5 to jest interface typu "przeciągnij/upuść" także nie ma problemu.<br />
Qt razem z innymi pluginami musisz sobie pobrać FILE->SETTINGS->Project:...->Project Interpreter.<br />
Najważniejsze PyQt5 PyQt5-sip pyqt5-tools PyQt5Designer no i reszta typu matplotlib ale to juz nie do GUI.<br />
W interpreterze "zrób sobie virtual" folder venv poszukaj pliku designer.exe powinien być w pyqt5_tools.<br />
To jest Qt do robienia GUI.<br />
Plik zapisujesz sobie z rozszerzeniem .ui gdześ w naszym folderze Program.<br />
Potem komendą **pyuic5 {nazwa pliku z roszerzeniem .ui} -o {nazwa pliku z rozszerzeniem .py}** tłumaczy się xml na pythona.<br />
Ten plik to jest GUI napisane w Pythonie. <br />
Kontroler do tego robi się jak jest pokazane w callName.py.<br />
Żeby wykresy rysowały się w okienku a nie w IDE to trzeba w PyCharmie wyłączyć Scientific Mode. <br />
_FILE->SETTINGS->TOOLS->Python Scientific i odznaczyć opcję_.<br />
To tyle :D Jak już Cię to przeraziło to możemy jeszcze zmienić język.<br />
