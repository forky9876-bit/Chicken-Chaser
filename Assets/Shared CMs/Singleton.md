# Singleton Design Pattern

The [singleton design](https://en.wikipedia.org/wiki/Singleton_pattern) pattern is a frequent pattern used in games.
Singletons are used when you want to access variables from a class without needing know a direct instance or reference to an object.
To use a Singleton we must ensure that there can only be one instance of the object.

Singletons are useful because:
* They can make code more readable
* They can support Lazily Loaded code
* They can provide logic structure ensuring a ruleset
* Allows you to escape static space

Be careful using singletons, as they are static objects, it's easy to abuse memory. Thankfully, Unity will take care of any mistakes, but it can still lead to longer loading times.

# Example

Let's say we have a class call Player and the player wants to know if the game is running
Currently, we'd do something like serialize a variable, or accessing it in Awake
```csharp
public class Player : MonoBehaviour
{
    [SerializeField] private GameManager ExampleA;
    
    private GameManager ExampleB;
    void Awake()
    {
        ExampleB = GameObject.Find("GameManager").GetComponenet<GameManager>();
    }
    
    void Update()
    {
        //If ExampleA is running, then cancel (Don't update)
        if(!ExampleA.IsRunning) return;
        //If ExampleB is running, then cancel (Don't update)
        if(!ExampleB.IsRunning) return;
        
        //Update logic goes here...
    }
}
```

Using ExampleA isn't ideal because
* Serialization increases loading time,
* A class variable is constant memory use
* each instance of Player would need their own instance of ExampleA

using ExampleB is even worse because
* Each player has to check EVERY object to find the GameManager O(n)
* And all the issues listed for ExampleA

So to implement a singleton let's do this to the GameManager class

```csharp
//Ensure the script is attached to an object in game
[DefaultExecutionOrder(-1000)] // This will load the script as early as possible
public class GameManager : MonoBehaviour
{
    [SerializeField] private bool isRunning;
    //NOTE: Variable is static meaning there is only one of these in this class, and every GameManager knows about it
    private static GameManger _instance;
    
    //These are "Get Only" functions, but more optimized than writing a full function
    public bool IsRunning => isRunning;
    
    //Because this is a public getter of a static thing, we've essentially created a GLOBAL VARIABLE
    public static GameManager Instance => _instance;
    
    //Use OnEnable or Awake as they run first.
    //Also, Singletons are a great example of why we should restrict Awake to local access, 
    // and use Start instead if you need to read other files
    void OnEnable()
    {
        if(_instance && _instance != this) // If there's an instance, and it's not us
        {
            Destroy(GameObject); //Destroy the object
            return; //Don't do anything else
        }
        _instance = this; //We become the new _instance
        DontDestroyOnLoad(gameObject); //Keep the object alive through scenes
    }
}
```

Finally, the player script will now look like this:
```csharp
public class Player : MonoBehaviour
{
    void Update()
    {
        //If GameManager is running, then cancel (Don't update)
        if(!GameManager.Instance.IsRunning) return;
        
        //Update logic goes here...
    }
}
```