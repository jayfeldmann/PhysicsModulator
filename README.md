# PhysicsModulator

The PhysicsModulator is a Software developed in combination with the Bachelorthesis by Jan Lukas Feldmann.
The original thesis title is: "Simulation physikalischer Systeme zur Modulation von Effekt- und Synthesizerparametern in der Audioproduktion"
The english thesis title is: "Simulation of physical System to modulate Effect and Synthesizerparameters for audioproduction"

Written at University of Applied Sciences Darmstadt.

Overview:
The PhysicsModulator simulates (currently just one) physical, dynamicall systems. These enable the users to map specific parameters of those systems to controlvalues from varied mediaproduction
tools via MIDI. The idea is to get natural feeling, randomized values to use in mediaproduction. This software is tested on Windows with audioproduction tools (Nuendo & Live 10).

Usage:
External Midi Device:
To use this software with external MIDI devices connected for example over USB you dont need additional drivers. Just select the corresponding device in the settings menu.

Windows: 
On windows you will need a MIDI-Bus or similar. The one I tested with was loopMidi. Install loopMidi, start it and select as a device in the settings menu. Then select loopMidi as a Midi Input
in your DAW or similar.

Mac:
Mac is similar to Windows, but you dont need additional tools, because Mac OS already comes with a Midi Bus pre-installed. Just activate it in the Mac-Midi settings and use it as a device in both
PhysicsModulator and your DAW or similar.
