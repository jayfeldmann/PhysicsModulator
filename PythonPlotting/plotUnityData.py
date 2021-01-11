# Dieses script dient als Hilfsscript, um die aus Unity gelesenen MIDI Daten
# zu verwenden um Graphen zu generieren, die als Grafik in der Bachelorarbeit verwendet 
# werden können.

import matplotlib, json
import matplotlib.pyplot as plt
import numpy as np

# Funktion um aus den vorbereiteten Daten einen Graphen zu generieren und anzuzeigen.
def PlotGraph(duration, step, dataValues):
    t = np.arange(0.0,duration,step)
    ax = plt.subplots()
    ax.plot(t,dataValues)
    ax.set(xlabel='time (s)', ylabel='midi value')
    ax.grid()
    plt.show()

# Funktion um JSON Daten auszulesen und für die Generation vorzubereiten.
def ReadUnityData(dataFilePath):
    jsonData = GetJsonData(dataFilePath)
    duration = float(jsonData["readDuration"])
    frequency = int(jsonData["readFrequency"])
    timeStep = float(1/frequency);
    values = np.array(jsonData["values"])
    PlotGraph(duration,timeStep,values)

# Funktion um JSON Datei zu lesen.
def GetJsonData(dataFilePath):
    with open(dataFilePath,'r') as jsonFile:
        jsonData = json.load(jsonFile)
        return jsonData

# Programm Einstiegspunkt
def main():
    jsonPath = input("JSON File Path: ")
    ReadUnityData(jsonPath)

main()