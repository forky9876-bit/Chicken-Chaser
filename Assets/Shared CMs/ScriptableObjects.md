# Shared Memory / Flyweight pattern and Scriptable objects

Scriptable Objects is an easy way to implement the [flyweight design pattern](https://en.wikipedia.org/wiki/Flyweight_pattern) in Unity. Flyweight is the idea of reusing the same thing many times, instead of making copies.

Consider the following problem: If we have 100 zombies in our game how much RAM (Random Access Memory) does each use? To solve this we need to look at each variable the zombie HAS.

Each variable uses ram, the following table says how much:

| (C#) Type        | (C#) Bytes Used           |
|------------------|---------------------------|
| String           | Number of characters + 1  |
| long             | 8                         |
| double           | 8                         |
| Object Reference | 4                         |
| int              | 4                         |
| float            | 4                         |
| short            | 2                         |
| char             | 1 (depending on encoding) |
| byte             | 1                         |
| bool             | 1                         |

Below is an example class:

```csharp
public class Zombie : MonoBehaviour, IDamagable
{
    [Header("Zombie")]
    [SerializeField] float maxHealth; // 4B
    [SerializeField] MeleeWeapon weapon; // 4B
    
    [Header("Movement")]
    [SerializeField] float speed; // 4B
    [SerializeField] float jumpHeight; // 4B

    float currentHealth; //4B
    bool hasRevived; // 1B
}
```

In total each instance of this object uses 21 Bytes. Let's say our goal is to have 100 running at once,
that's 2100B or >2MB of data. While this doesn't sound like a lot, the CPU is always trying to cache and chunk data  for fast access, and the less work it has to do, the less laggy the game will be.

The idea of Flyweight is to share common data, and use it when needed. It provides the following advantages:

* Possibly reduces memory cost of object (In our case it'd be 21B --> 13B)
* Cheaper Instantiation and Destruction costs (This can be better resolved by [object pooling](https://en.wikipedia.org/wiki/Object_pool_pattern) though)
* Better organization
* Scriptable Objects can be edited while the game is running (and their data will be saved, unlike GameObjects)

You probably SHOULD NOT use scriptable objects if:
* There's only ever one instance of the object
* The object only changes internally (I.E. Not doing upgrades)
* Worried about project size and number of files

# Example

Looking back at the zombie example, consider which variables will be the same in every "Basic" Zombie

```csharp
public class Zombie : MonoBehaviour, IDamagable
{
    [Header("Zombie")]
    [SerializeField] float maxHealth; // SAME
    [SerializeField] MeleeWeapon weapon; 
    
    [Header("Movement")]
    [SerializeField] float speed; // SAME
    [SerializeField] float jumpHeight; // SAME

    float currentHealth; 
    bool hasRevived; 
    
    //For demo purposes
    void Awake()
    {
        currentHealth = maxHealth;
    }
```
Next in Unity, create a new C# script (In unity 6, you can actually choose ScriptableObject)

```csharp
//Define creation in Unity
[CreateAssetMenu(fileName = "Zombie Stats", menuName = "MyScriptables/Zombie", order = 1)]
public class ZombieSO : ScriptableObject // Child of Scriptable Object
{
    [Header("Zombie")]
    [SerializeField] float maxHealth;
    
    [Header("Movement")]
    [SerializeField] float speed; 
    [SerializeField] float jumpHeight; 
    
    //Make Getter Variables
    
    //Getters in C# are best written like this, 
    //they are less instructions on the CPU
    public float MaxHealth => maxHealth; 
    public float JumpHeight => speed; 
    public float JumpHeight => speed; 
```

Then finally, head back over to Zombie and update it to the following:

```csharp
public class Zombie : MonoBehaviour, IDamagable
{
    [Header("Zombie")]
    [SerializeField] ZombieSO stats; // Reference to our SO
    [SerializeField] MeleeWeapon weapon; 
   
    float currentHealth; 
    bool hasRevived; 
    
    //For demo purposes
    void Awake()
    {
        ///Update every old variable with stats. and a capital (maxHealth => stats.MaxHealth)
        currentHealth = stats.MaxHealth;
    }
```