import matplotlib, json
import matplotlib.pyplot as plt
import numpy as np

def PlotGraph(duration, step, dataValues):
    t = np.arange(0.0,duration,step)
    
    fig, ax = plt.subplots()
    ax.plot(t,dataValues)

    ax.set(xlabel='time (s)', ylabel='midi value')
    ax.grid()

    plt.show()

def ReadUnityData(dataFilePath):
    jsonData = GetJsonData(dataFilePath)
    duration = float(jsonData["readDuration"])
    frequenzy = int(jsonData["readFrequenzy"])
    timeStep = float(1/frequenzy);
    values = np.array(jsonData["values"])
    PlotGraph(duration,timeStep,values)

def GetJsonData(dataFilePath):
    with open(dataFilePath,'r') as jsonFile:
        jsonData = json.load(jsonFile)
        return jsonData

def main():
    ReadUnityData('20testX.json')

main()