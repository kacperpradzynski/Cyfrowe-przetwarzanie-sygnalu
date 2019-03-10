import Program.signal as s
import numpy as np


class Generator:
    def __init__(self, sign, numberOfSamples):
        self.signal = sign
        self.signal = np.arange(numberOfSamples)

    def returnFunction(self):
        pass
