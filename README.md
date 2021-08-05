# AIRocketLanding
Using reinforcement learning to make an agent land a rocket with somewhat realistic physics in Unity

The setup:

At the start of the episode:

-Spawn at realistic altitude and distance from barge with some randomness (about 7km high and 700m from barge)

-Set realistic approach angle with some randomness 

-Set realistic velocity with some randomness (about 420m/s)


Observations:

-Current booster position

-Current barge position

-Current rotation

-Current throttle

-Current X Gimbal

-Current Y Gimbal

-All of above from previous step


Actions:

-Increase or decrease throttle (discrete)

-Change X Gimbal (continuous)

-Change Y Gimbal (continuous)


Rewards (at end of episode):

-Negative velocity (slower is better)

-Negative distance between booster and barge (shorter is better)

-Negative angle from upright position (closer to upright is better)


Some images of the project:

![alt text](https://github.com/Jonathon-A/AIRocketLanding/blob/main/Images/Rocket.png)
![alt text](https://github.com/Jonathon-A/AIRocketLanding/blob/main/Images/RocketTornado.png)
![alt text](https://github.com/Jonathon-A/AIRocketLanding/blob/main/Images/RocketSpawn.png)
![alt text](https://github.com/Jonathon-A/AIRocketLanding/blob/main/Images/RocketDiver2.png)
![alt text](https://github.com/Jonathon-A/AIRocketLanding/blob/main/Images/RocketsDivert.png)

