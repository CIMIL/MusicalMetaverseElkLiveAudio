# MusicalMetaverseElkLiveAudio

## Setup and Usage

### Scene
The official scene is the `Waves` Scene, found in the `Assets > Assets > Scenes` directory. Other scenes have been left in as testing scenes, but are not feature-complete and are not to be used.

### Local Server
In order to run the demo with a local server, head over to the [Ubiq Repository](https://github.com/UCL-VR/ubiq) and clone it. Then, inside the `Node` directory, run the `npm start` command (make sure you have Node installed).

### IP addresses
Inside the `Assets > Scriptable Objects` folder, you will find the `Ubiq Localhost` prefab. If you're running the server locally, then leave it on its default value (`localhost`), otherwise change it with the IP of the machine running the server

With regards to OSC connectivity, inside the `Waves` scene, you will find an `OSC Transmitter` object on the root level. Inside, insert the IP address of your Elk device.
  
## Logging:

The demo automatically starts logging various types of events as soon as you start it. These logs are buffered into memory unless writing is explicitly enabled. To start collecting these logs, find the `Ubiq Network Scene > Log Manager` object inside the `Waves` scene and press `Start Collection` on the `Log Collector` component. This will start collecting (i.e. writing) all the logs produced locally and by remote peers. You can find these logs by pressing `Open Folder`on the `Log Collector component`.

### Info logs
Inside `Info_log` files you will find `Main Camera` events and `Looking At` events.

- `Main Camera`: reports Main Camera position and look direction over time. It's columns are:
  - ticks: the timestamp of the event
  - peer: the NetworkSceneId of the Peer that emitted the event
  - event: the name of the event (`Main Camera`)
  - arg1: a JSON object containing the details of the event
    - forward: Camera forward vector, it indicates where the direction the player is facing
    - position: Camera position vector, it indicates the player's camera position inside the playable space

- `Looking At`: reports when one player is looking at another. Its columns are:
  - ticks: the timestamp of the event
  - peer: the NetworkSceneId of the Peer that emitted the event
  - event: the name of the event (`Looking At`)
  - arg1: a JSON object containing the details of the event
    - type: the type of the event, can either be `Started` or `Stopped`
    - target: the id of the peer the player is looking at

### Experiment logs
Inside the `Experiment_log` files you can find interactions with musical instruments, called `Block Interaction` events. Its columns are:

- ticks: the timestamp of the event
- peer: the NetworkSceneId of the Peer that emitted the event
- event: the name of the event (`Block Interaction`)
- arg1: a JSON object containing the details of the event
  - type: either `Entered` or `Exited`
  - note: the MIDI note of the block that has been played
  - preset: the Elk preset currently selected for that instrument



