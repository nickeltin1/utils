# utils
Different universal utility scripts for unity

1. Singletons
UnconsistentSingleton - generic singleton which loads from resource folder if not presented on scene, not consist over scenes
ConsistentSingleton - inherets from UnconsistentSingleton, but consists over scenes

2. Object Pools
PoolBase - abstract class wich containts base pool fields and abstract Get() and AddToPool() methods
Pool - contains regural Pool and generic Pool<T> both of them inherets form PoolBase, regural Pool uses Transform as poolObject, generic Pool<T> uses PoolObject<T>
PoolObject<T> - MonoBehaviour with its Pool<T> reference

3. State Machine
