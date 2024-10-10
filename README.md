# DemoVisualizationTMS

Simplified Unity project of the AR-guided TMS (without the TCP connection) to be deployed on Magic Leap for quick deployement and setup in demo context.

The demo is broken down into two stages: registration and TMS targetting.

## Registration
The user should place the markers (spheres) at key fiducials on the subject's head (chose one side of the face): 
- inner ear (pick a side of the head)
- corner of the eye (on the same side of the head previously chosen)
- nostril
- chin

To do so, align the white sphere located in front of the controller to the corresponding fiducials. If not satisfied by the position of a sphere, the sphere can be deleted. 

Commands summary:
- Place sphere: trigger
- Delete sphere: double trigger
- Registration: Home button



## AR-guided TMS
A mesh of the coil is overlayed on top of the controller. The trigger enables to shoot a raycast from the top of the controller (coil centroid position) to the center of the subject's brain. At the 
