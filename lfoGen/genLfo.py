import matplotlib
import matplotlib.pyplot as plt
import numpy as np

# Generiert ein Sinus Array und erzeugt daraus einen Graph
def GenerateSin():
    freq = 1
    amp = 127
    t = np.arange(0.0,10,0.02)
    s = amp/2 + (amp/2 * np.sin(2*np.pi*freq*t))

    fig, ax = plt.subplots()
    ax.plot(t, s)
    ax.set(xlabel='time (s)', ylabel='midi value')
    ax.grid()
    plt.show()

# Erzeugt ein Random Step array moduliert nach der Random Funktion
# des Ableton Live LFO
def GenerateRandom():
    rndArray = []
    freq = 50
    duration = 10
    cnt = 0
    while cnt < duration:
        incnt = 0
        rand = np.random.randint(0,127)
        while incnt < freq:
            rndArray.append(rand)
            incnt += 1
        cnt += 1
    
    rndArray = np.array(rndArray)
    t = np.arange(0.0,duration,1/freq)
    fig, ax = plt.subplots()
    ax.plot(t,rndArray)
    ax.set(xlabel='time (s)', ylabel='midi value')
    ax.grid()
    plt.show()

#GenerateSin()
GenerateRandom()