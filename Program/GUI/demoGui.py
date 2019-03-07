from PyQt5 import QtCore, QtGui, QtWidgets

class Ui_MainWindow(object):
    def setupUi(self, MainWindow):
        MainWindow.setObjectName("MainWindow")
        MainWindow.resize(414, 300)
        self.centralwidget = QtWidgets.QWidget(MainWindow)
        self.centralwidget.setObjectName("centralwidget")
        self.label = QtWidgets.QLabel(self.centralwidget)
        self.label.setGeometry(QtCore.QRect(70, 60, 81, 16))
        self.label.setObjectName("label")
        self.labelResponse = QtWidgets.QLabel(self.centralwidget)
        self.labelResponse.setGeometry(QtCore.QRect(150, 110, 111, 20))
        self.labelResponse.setObjectName("labelResponse")
        self.lineEditName = QtWidgets.QLineEdit(self.centralwidget)
        self.lineEditName.setGeometry(QtCore.QRect(160, 60, 113, 20))
        self.lineEditName.setObjectName("lineEditName")
        self.ButtonClickMe = QtWidgets.QPushButton(self.centralwidget)
        self.ButtonClickMe.setGeometry(QtCore.QRect(170, 140, 75, 23))
        self.ButtonClickMe.setObjectName("ButtonClickMe")
        self.ButtonSinus = QtWidgets.QPushButton(self.centralwidget)
        self.ButtonSinus.setGeometry(QtCore.QRect(170, 210, 75, 23))
        self.ButtonSinus.setObjectName("ButtonSinus")
        MainWindow.setCentralWidget(self.centralwidget)
        self.menubar = QtWidgets.QMenuBar(MainWindow)
        self.menubar.setGeometry(QtCore.QRect(0, 0, 414, 21))
        self.menubar.setObjectName("menubar")
        MainWindow.setMenuBar(self.menubar)
        self.statusbar = QtWidgets.QStatusBar(MainWindow)
        self.statusbar.setObjectName("statusbar")
        MainWindow.setStatusBar(self.statusbar)

        self.retranslateUi(MainWindow)
        QtCore.QMetaObject.connectSlotsByName(MainWindow)

    def retranslateUi(self, MainWindow):
        _translate = QtCore.QCoreApplication.translate
        MainWindow.setWindowTitle(_translate("MainWindow", "MainWindow"))
        self.label.setText(_translate("MainWindow", "Wpisz swoje imię"))
        self.labelResponse.setText(_translate("MainWindow", "Input Text"))
        self.ButtonClickMe.setText(_translate("MainWindow", "OK"))
        self.ButtonSinus.setText(_translate("MainWindow", "Rysuj sinusa"))

