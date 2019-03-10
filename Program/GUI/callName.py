from PyQt5.QtWidgets import QMainWindow, QApplication
from Program.GUI.demoGui import *
import matplotlib.pyplot as plt
import numpy as np
import Program.signal as g


class MyForm(QMainWindow):
    def __init__(self):
        super().__init__()
        self.ui = Ui_MainWindow()
        self.ui.setupUi(self)
        self.ui.ButtonClickMe.clicked.connect(self.dispmessage)
        self.ui.ButtonSinus.clicked.connect(self.displaySinus)
        self.show()

    def dispmessage(self):
        if self.ui.lineEditName.text() == "Kacper":
            self.ui.labelResponse.setText("Elo " + self.ui.lineEditName.text())
        else:
            self.ui.labelResponse.setText("To nie twoje imie!!!")

    def displaySinus(self):
        # Jakis przykladowy
        Fs = 8000
        f = 5
        sample = 8000
        x = np.arange(sample)
        y = np.sin(2 * np.pi * f * x / Fs)
        plt.plot(x, y)
        plt.show()
