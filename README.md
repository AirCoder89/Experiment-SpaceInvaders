
# Space  Invaders
3D Space invaders game prototype

### Main Goal:
The main goal of this project is to achieve a clone of Space Invaders with **gameplay** one variation and multiple tech goals.

### Variation from the original game:
- Each enemy has a color assigned to it, and that colors will be used when destroying them. The idea is that when you kill an enemy of Color B it will destroy all connected enemies with the same properties.
<img width="612" alt="Screenshot 2021-07-11 at 17 03 40" src="https://user-images.githubusercontent.com/62396712/125202080-f7b1cc00-e269-11eb-948e-28728ffef289.png">

### Leaderboards Strategy:
- The leaderboards returns a sample group from the entire db, we can replicate that right
now returning not more than 10 users. A Top 10.
- Users are sorted by the score.
- The game must save and load leaderboards, we will have a local leaderboard
system stored in disk but we need to leave the system ready to replace the
implementation easily to be able to point to a real server and fetch the results.
- Any action executed must be asynchronous and UI accessing that data must be
responsive to it.
- Before accessing leaderboards the user must get a valid Auth Token from the Auth
service.

### Services Contracts:

v1/auth/register (GET):
- Response (json string):

`{
"idToken" : "string value",
"refreshToken": "string value",
"user" : {
"id" : "userid"
}
}`

v1/leaderboards (GET):
- Query parameters:
- “country”: “string value”
- Headers:
-  Bearer authentication (_idToken_)
- Response (json string):

`{
"group": {
"week": 6,
"start": "2014-02-09T00:00:20Z",
"end": "2060-02-19T00:00:20Z",
"tournamentId": "",
"players": [
{
"uid": "",
"name": "",
"scores": {
"current": 0,
"past": 0
}
}
]
}`

v1/leaderboards/submit (POST)
- Body parameters

`{
"tournamentId": "dk",
"name": "Jesper",
"score": 1000
}`

Headers:
- Bearer authentication (_idToken_)

### Game Flow:
- Player starts in Main Menu
- Allows to play or view the **leaderboards**
- Player plays a session
- After the winning/ condition the game allows the user to submit his score with a
name only if his score is better than any score in the **leaderboard**.
- Game to back to main menu after winning/losing condition.

### Presentation:
_I would like to walk through the source code and explain my perspective about a few Points[] in this project._

### Points[0] = Architecture:

Usually when I have to create a project like space invaders I directly choose MVC Architecture. but I decided to move with something more complicated, may that can attract your attention.
This architecture is based on avoiding using **MonoBehaviour** as we can! The idea is to create all gameplay just by using a single **MonoBehaviour** that acts as an entry point and a code flow distributor.

### Points[1] = **Game states:**

I believe the best way to navigate among game states is by creating a state machine with manual transitions. And the diagram shows the relationship among each part of the state machine in the game.

![1](https://user-images.githubusercontent.com/62396712/125201486-50339a00-e267-11eb-8252-e1bfbde7b782.png)


### Points[2] = **Code flow:**

The entry point (Main) takes charge to tick any object in the project. that will give us more control to decide who will be initialized before and which object has to tick etc..

![2](https://user-images.githubusercontent.com/62396712/125201525-8113cf00-e267-11eb-80d5-83fc264c60ee.png)

### Points[3] = **Game Systems:**

A Game system is who creates and manages entities in the game, and I didn't separate the logic from the entities like ECS. I wrote logic in both of entities (GameView) and systems taking in consideration the first S.O.L.I.D principle (Single Responsibility). which means each part has its own logic.

![3](https://user-images.githubusercontent.com/62396712/125201559-a0126100-e267-11eb-98e4-98eabfa160d8.png)

### Points[4] = **Game Views:**

Entity that represents actors in the Unity space through a GameObject. With association of Unity Components (BoxCollider, RigidBody, MeshRenderer...) And by using classic classes instead of MonoBehaviour we reduce Garbage Collection and Allocations because we disposed of all unnecessary allocations that come from MonoBehaviour/Behaviour/Component/Object.

![4](https://user-images.githubusercontent.com/62396712/125201600-c59f6a80-e267-11eb-83a6-68ad0a802aed.png)

### Points[5] = **Game Settings & Parameters:**

By gathering all settings and system configurations in one place, that will give us full control on each detail in the game without starting digging for a script that has the desired parameter.

All data stored into **scriptableObject** assets. and by doing that we can avoid a lot of dependencies issues, we can adjust parameters in play mode without losing values, create different game modes easily,

A **SystemConfig** is a **scriptableObject** containing a set of data models (struct) to configure each system and feed it with views information.

![5](https://user-images.githubusercontent.com/62396712/125201654-fb445380-e267-11eb-817e-16b045f7c09c.png)

### Points[6] = **Services Contract:**

In order to create a local leaderboard system I have to choose where I can store database json, sqlite.., then I figure out that we can use scriptableObject for testing purposes.I choose to move with RestClient utility instead of using UnityWebRequest or HttpClient, because it's super handy to use and we can use it asynchronously with callbacks.All database requests will be pass through the class DBManager, and we can replace implementation with only two steps :

- Turn off 'isLocal' in Settings/DbConfig.
- In Scripts/Database/UriList.cs and replace 'v1' with the real base.

![6](https://user-images.githubusercontent.com/62396712/125201711-2c248880-e268-11eb-90ab-259d168e8290.png)

### Points[7] = **Game systems briefly:**

**Timing System**
{
*/* * _Helper systems provide an instance of Timer that can be used to invoke a callback after a certain amount of time_. * */*
}

**Level System**
{
*/* * _Define level's edges by using colliders, trigger an event when invaders hit one of the vertical edges_ * */*
}

**Audio System**
{
*/* * _Provide an AudioPool holds AudioSource's instances and allow us to play sound effects with options such as defines the audioClip if he has to play once (always on the same AudioSource) or to play without increasing the previous clip (each AudioClip plays in different AudioSource)._ * */*
}

**Inputs System**
{
*/* * _switch the control between Editor or Touch, by default the system detects the target platform and set the control, If you face any problem with control please turn off the option 'autoDetect' from InputConfig._ * */*
}

**Player System**
{
*/* * _Listen to the Inputs system and he commands the player view to do an action and calculate fire rate time._ * */*
}

**Grid System**
{
*/* * _Take in charge to distribute invader's cells in grid layout, animate and synchronize invaders movements, usually when I deal with a 2d array that represent graphic elements I like to create a class with an indexer (Matrix<T>) to encapsulate behaviors and using it as sub system._ * */*
}

**Shields System**
{
*/* * _Holds and generates shields view, and each shield view contains several shield pieces._ * */*
}

**Invaders System**
{
*/* * _Create and Tick special ship, communicate with the Grid system to move, Get matches, shoot, destroy (not physically), and because we have a predicted amount of invaders I decide to not use a pool, just I change the data and visibility instead spawn and de-spawn the object._ * */*
}

**Animation System**
{
*/* * _Animate invaders by changing the mesh each frame._ * */*
}

**Shooting System**
{
*/* * _Working with an ObjectPool to provide a bullet view for any party who asks for (player, invaders)._ * */*
}
