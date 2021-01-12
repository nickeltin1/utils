# utils
Different universal utility scripts for unity

#Mine:

1. Singletons
UnconsistentSingleton - generic singleton which loads from resource folder if not presented on scene, not consist over scenes
ConsistentSingleton - inherets from UnconsistentSingleton, but consists over scenes

2. Object Pools
PoolBase - abstract class wich containts base pool fields and abstract Get() and AddToPool() methods
Pool - contains regural Pool and generic Pool<T> both of them inherets form PoolBase, regural Pool uses Transform as poolObject, generic Pool<T> uses PoolObject<T>
PoolObject<T> - MonoBehaviour with its Pool<T> reference

3. State Machine
StateMachine - regural class, but with StateMachine engine as MonoBehaviour, uses Update, FixedUpdate unity events, different state mschines can be merged to work on one engine, for better performance
There is main state which runs always
State - class containing all actions for state: onUpdate, onFixedUpdate, onStateStart, onStateEnd
Trsnsition between states is specified in state itself, as yourStateMachine.SwitchState(Enum type)
Each stae should be provided with its Enum value for type
States itn state machine added in code, with .AddState(State newState), state can be overrided

4. Save System
Inherit class that need to be saved form Saveable<SaveType>, provide it with ID (save file name) and .Register() it
SaveType - clear class for all data that need to be stored 

5. Extensions
Different extension methods for Vectors, Integer, Floats, Transform, Color... 

#Not mine:

Localization
PathCreator
NaughtyAttributes
LeanTween

