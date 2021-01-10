import matplotlib
import matplotlib.pyplot as plt
import numpy as np

t = np.arange(0.0, 1.0, 0.01)
s = 1 + np.sin(2 * np.pi * t) * 127

fig, ax = plt.subplots()
ax.plot(t, s)

ax.set(xlabel='time (s)', ylabel='midi value',
       title='Zeitverlauf Boid-X Positionen')
ax.grid()

#fig.savefig("test.png")
plt.show()

def ReadUnityData(dataFilePath):
    pass
