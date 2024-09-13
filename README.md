# About
A prototype shooter game. Made with Unity DOTS. Futuregames school assignment, 2024. 

When creating this game I wanted to spawn 1000 enemies, but when instantiating
that many entities I caused nauseating lag spikes. To fix it I created some
sort of pooling-like system.

My pooling system:\
EnemyAuthoring.cs\
EnemyCreationSystem.cs\
SpawnerSystem.cs

EnemyPrefab is baked in 'EnemyAuthoring.cs', it is then instantiated in 'EnemyCreationSystem.cs' until it is finally brought into play by 'SpawnerSystem.cs'

# Controls
WASD - move.\
Space - shoot.
