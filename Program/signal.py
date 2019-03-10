import numpy as np


class Signal:
    def __init__(self, amplitude, startTime, duration, bPeriod, fFactor):
        self.amplitude = amplitude
        self.startTime = startTime
        self.duration = duration
        self.bPeriod = bPeriod
        self.fFactor = fFactor

    def Sinus(self):
        return self.amplitude * np.sin((2*np.pi)/(self.duration - self.startTime) / self.bPeriod)


