import Program.GUI.callName as gui
import sys


if __name__ == "__main__":
    app = gui.QApplication(sys.argv)
    w = gui.MyForm()
    w.show()
    sys.exit(app.exec_())
